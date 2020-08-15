using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using Dfy;
using Newtonsoft.Json;
using Scratch;
using SpreadsheetLight;
using JsonFormatting = Newtonsoft.Json.Formatting;

namespace CSScratchpad.Script {
    public class ReplicateAspMvcDownload : Common, IRunnable {
        public void Run() {
            var file = new ReportingController().Download("PAY_SLIP") as FileContentResult;
            Boolean result = ByteArrayToFile(file.FileDownloadName, file.FileContents);

            Dbg("ByteArrayToFile result", result);


            Byte[] modified = UpdateCreatedDateAndUpdatedDateOfExcel((Stream) File.Open(file.FileDownloadName, FileMode.OpenOrCreate, FileAccess.ReadWrite), DateTime.Now);
            ByteArrayToFile(file.FileDownloadName, modified);

            //NOTE: Must have excel installed
            //new Process { StartInfo = new ProcessStartInfo("excel", file.FileDownloadName) }.Start();

            Dbg("AppDomain.CurrentDomain.BaseDirectory", AppDomain.CurrentDomain.BaseDirectory);
            Dbg("Get all current domain's assemblies", AppDomain.CurrentDomain.GetAssemblies().Select(assm => assm.GetName().Name));
        }

        #region :: Controller ::

        public class ReportingController : Controller {
            [HttpPost]
            public FileResult Download(String reportCode) {
                DownloadFileInfo file = GetReportFileInfo(reportCode);
                return File(file.FileByteArray, file.MimeType, file.DocumentFullPath);
            }

            DownloadFileInfo GetReportFileInfo(String reportCode) {
                IExcelReport report = ExcelReportAssembly.LoadReport(reportCode);
                report.Render();

                return report.GetDownloadFileInfo();
            }
        }

        #endregion

        #region :: Document ::

        public interface IExcelReport {
            String DocumentName { get; set; }
            String DocumentNo { get; set; }
            DateTime DocumentDate { get; set; }
            String Filename { get; set; }
            DataSet DataSource { get; set; }
            String Creator { get; set; }
            String Status { get; set; } // NOTE: could be Draft || Final

            void Render();
            DownloadFileInfo GetDownloadFileInfo();
        }

        public sealed class ExcelReportDataSource {
            public DataSet TitleDataSource { get; set; }
            public DataSet HeaderDataSource { get; set; }
            public DataSet BodyDataSource { get; set; }
            public DataSet FooterDataSource { get; set; }
        }

        public abstract class ExcelReport : IExcelReport {
            Byte[] documentBytes;
            protected SLDocument document;
            protected SLWorksheetStatistics documentStat;
            protected Int32 rowIndex = 1;
            protected Int32 columnIndex = 1;
            protected IDictionary<String, String> columnMapping;

            public String DocumentName { get; set; }
            public String DocumentNo { get; set; }
            public DateTime DocumentDate { get; set; }
            public String Filename { get; set; }
            public DataSet DataSource { get; set; }
            public String Creator { get; set; }
            public String Status { get; set; }

            protected virtual void SetupExcelDocument() {
                document.DocumentProperties.Creator = "Asp Mvc Download Replicator";
                document.DocumentProperties.LastModifiedBy = "Asp Mvc Download Replicator";
                document.DocumentProperties.ContentStatus = Status;
                document.DocumentProperties.Title = DocumentName;
                document.DocumentProperties.Description = String.IsNullOrEmpty(DocumentNo) ? DocumentName : String.Format("{0}-{1}", DocumentName, DocumentNo);

                documentStat = document.GetWorksheetStatistics();

                AdditionalSetupExcelDocument();
            }

            protected abstract void AdditionalSetupExcelDocument();

            protected abstract void RenderTitle();
            protected abstract void RenderHeader();
            protected abstract void RenderBody();
            protected abstract void RenderFooter();

            public void Render() {
                if (String.IsNullOrEmpty(DocumentName))
                    throw new ArgumentNullException("DocumentName");
                if (String.IsNullOrEmpty(Filename))
                    throw new ArgumentNullException("Filename");
                if (DataSource == null)
                    throw new ArgumentNullException("DataSource");
                if (DataSource.Tables.Count < 1)
                    throw new ArgumentOutOfRangeException("DataSource.Tables");

                using (var stream = new MemoryStream()) {
                    using (document = new SLDocument()) {
                        SetupExcelDocument();
                        RenderTitle();
                        RenderHeader();
                        RenderBody();
                        RenderFooter();

                        document.SaveAs(stream);
                        stream.Position = 0;
                        documentBytes = stream.GetBuffer().Clone() as Byte[];
                    }
                }
            }

            public DownloadFileInfo GetDownloadFileInfo() =>
                new DownloadFileInfo {
                    DocumentName = DocumentName,
                    DocumentNo = DocumentNo,
                    DocumentFullPath = Filename,
                    Filename = Path.GetFileName(Filename),
                    MimeType = MimeTypes.DetermineByExtension("xlsx"),
                    FileByteArray = documentBytes
                };

            protected String MapColumnIntoUpperHeader(String columnName) => MapColumnIntoHeader(columnName).ToUpperInvariant();

            protected String MapColumnIntoHeader(String columnName) {
                if (!columnMapping.Keys.Contains(columnName))
                    return columnName;

                return columnMapping[columnName];
            }
        }

        #endregion

        #region :: Document Helper ::

        public Boolean ByteArrayToFile(String filename, Byte[] byteArray) {
            try {
                using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
                    fs.Write(byteArray, 0, byteArray.Length);

                return true;
            }
            catch (Exception ex) {
                Console.WriteLine("Exception caught in process: {0}", ex.ToString());
            }

            return false;
        }

        public static class MimeTypes {
            static readonly IDictionary<String, String> registeredMimeTypes = new Dictionary<String, String> {
                [""] = "application/txt",
                ["txt"] = "application/txt",
                ["csv"] = "application/csv",
                ["xlsx"] = "application/vnd.ms-excel",
                ["xlsm"] = "application/xslx",
                ["xltx"] = "application/xslx",
                ["xltm"] = "application/xslx",
                ["pdf"] = "application/pdf",
                ["ods"] = "application/ods"
            };

            public static String DetermineByExtension(String ext) =>
                !registeredMimeTypes.ContainsKey(ext) ?
                    registeredMimeTypes[String.Empty] :
                    registeredMimeTypes[ext];
        }

        public static class ExcelReportAssembly {
            public static IExcelReport LoadReport(String reportCode) {
                // NOTE: read app.config

                String assemblyName = "this";
                Assembly asm = assemblyName.ToLowerInvariant() == "this" ? Assembly.GetExecutingAssembly() : Assembly.LoadFile(assemblyName);
                
                Type report = asm.GetTypes()
                    .Where(type => type.GetCustomAttributes<ExcelReportAttribute>(false).Any())
                    .SingleOrDefault(type => type
                        .GetCustomAttribute<ExcelReportAttribute>(false)
                        .ReportCode.ToLowerInvariant() == reportCode.ToLowerInvariant());

                return (IExcelReport) Activator.CreateInstance(report);
            }
        }

        [AttributeUsage(AttributeTargets.Class)]
        public class ExcelReportAttribute : Attribute {
            public String ReportCode { get; private set; }
            public ExcelReportAttribute(String reportCode) {
                ReportCode = reportCode;
            }
        }

        #endregion

        #region :: Document Implementation ::

        static String Int32ToString(Int32 value) => value == 0 ? "0,00" : value.ToString("#,###.00");

        static DateTime DateTimeToUtc(DateTime src) => src.AddHours(DateTime.UtcNow.Subtract(DateTime.Now).Hours);

        static Byte[] UpdateCreatedDateAndUpdatedDateOfExcel(Stream documentStream, DateTime documentDate) {
            using (var pkg = Package.Open(documentStream, FileMode.OpenOrCreate, FileAccess.ReadWrite)) {
                PackagePart docPropPart = pkg.GetParts()
                    .FirstOrDefault(part =>
                        part.Uri.OriginalString.IndexOf("docProps") != -1 &&
                        part.Uri.OriginalString.IndexOf("core.xml") != -1);

                XDocument docPropXml = XDocument.Load(XmlReader.Create(docPropPart.GetStream()));
                IList<XElement> propEls = docPropXml
                    .Elements()
                    .First(el => el.Name.LocalName == "coreProperties")
                    .Elements()
                    .ToList();

                XElement
                    createdEl = propEls.First(el => el.Name.LocalName == "created"),
                    modifiedEl = propEls.First(el => el.Name.LocalName == "modified");

                createdEl.Value = modifiedEl.Value = documentDate
                    .AddHours(DateTime.UtcNow.Subtract(DateTime.Now).Hours)
                    .ToString("yyyy-MM-ddTHH:mm:ssZ");

                using (var stream = new MemoryStream()) {
                    using (var streamWriter = new StreamWriter((Stream) stream)) {
                        docPropXml.Save((TextWriter) streamWriter);
                        pkg.GetPart(docPropPart.Uri)
                            .GetStream(FileMode.OpenOrCreate, FileAccess.ReadWrite)
                            .Write(stream.ToArray(), 0, (Int32) stream.Length);
                    }
                }

                pkg.Close();
            }

            var ms = new MemoryStream();
            documentStream.Position = 0;
            documentStream.CopyTo(ms);

            return ms.ToArray();
        }

        static DataTable GetData() {
            var dt = new DataTable();
            dt.Columns.AddRange(new[] {
                new DataColumn { ColumnName = "Period", DataType = typeof(String) },
                new DataColumn { ColumnName = "EmpCode", DataType = typeof(String) },
                new DataColumn { ColumnName = "JobPos", DataType = typeof(String) },
                new DataColumn { ColumnName = "Dept", DataType = typeof(String) },

                new DataColumn { ColumnName = "Currency", DataType = typeof(String) },
                new DataColumn { ColumnName = "BasicSalary", DataType = typeof(String) },
                new DataColumn { ColumnName = "TaxAllow", DataType = typeof(String) },
                new DataColumn { ColumnName = "Tax", DataType = typeof(String) },

                new DataColumn { ColumnName = "Allow", DataType = typeof(String) },
                new DataColumn { ColumnName = "Deduction", DataType = typeof(String) },

                new DataColumn { ColumnName = "HpAllow", DataType = typeof(String) },
                new DataColumn { ColumnName = "GrossAllow", DataType = typeof(String) },

                new DataColumn { ColumnName = "HIns", DataType = typeof(String) },
                new DataColumn { ColumnName = "RIns", DataType = typeof(String) },
                new DataColumn { ColumnName = "TotalDeduct", DataType = typeof(String) },
                new DataColumn { ColumnName = "THP", DataType = typeof(String) },

                new DataColumn { ColumnName = "InWords", DataType = typeof(String) },
            });

            Int32
                sal = 1200,
                taxAllow = 0,
                hpAllow = 100,
                tax = -382,
                healthins = 80,
                retireins = 24;

            Int32
                grossAllow = sal + taxAllow + hpAllow,
                totalDeduct = (tax < 0 ? 0 : -tax) - healthins - retireins;

            Int32 thp = (tax < 0 ? Math.Abs(tax) : 0) + grossAllow + totalDeduct;

            DataRow value = dt.NewRow();
            value[0] = "August 2020";
            value[1] = "8734648926 - Random Employee";
            value[2] = "Developer";
            value[3] = "Asp Mvc Download Replicator";
            value[4] = "USD";
            value[5] = Int32ToString(sal);
            value[6] = Int32ToString(taxAllow);
            value[7] = Int32ToString(tax);
            value[8] = String.Empty;
            value[9] = String.Empty;
            value[10] = Int32ToString(hpAllow);
            value[11] = Int32ToString(grossAllow);
            value[12] = Int32ToString(healthins);
            value[13] = Int32ToString(retireins);
            value[14] = Int32ToString(totalDeduct);
            value[15] = Int32ToString(thp);
            value[16] = "In words";

            dt.Rows.Add(value);

            return dt;
        }

        static DataTable GetTitle() {
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn { ColumnName = "Title", DataType = typeof(String) });

            DataRow value = dt.NewRow();
            value[0] = "PAYSLIP";
            dt.Rows.Add(value);

            return dt;
        }

        [ExcelReport("PAY_SLIP")]
        public class PaySlipReport : ExcelReport {
            String OutputDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp");

            String GetOutputPath(String filename) {
                if (!Directory.Exists(OutputDirPath))
                    Directory.CreateDirectory(OutputDirPath);

                return Path.Combine(OutputDirPath, filename);
            }

            void Dbg(String title, Object obj) {
                if (!(obj is String && String.IsNullOrEmpty(obj.ToString()))) {
                    Console.WriteLine(JsonConvert.SerializeObject(obj, JsonFormatting.Indented));
                    Console.WriteLine();
                }
            }

            public PaySlipReport() {
                DocumentName = "Pay Slip - Random Employee";
                DocumentDate = DateTime.Now;
                Filename = GetOutputPath($"{DocumentName} - {DocumentDate:ddMMyyyyhhmmss}.xlsx");

                DataTable title = GetTitle();
                title.TableName = "TitleDataTable";

                DataTable data = GetData();
                data.TableName = "DataTable";

                columnMapping = new Dictionary<String, String> {
                    { "Period", "Period" },
                    { "EmpCode", "Employee Code - Name" },
                    { "JobPos", "Job Position" },
                    { "Dept", "Department" },

                    { "Currency", "Payment Currency" },
                    { "BasicSalary", "Basic Salary" },
                    { "TaxAllow", "Tax Allowance" },
                    { "Tax", "Tax" },

                    { "Allow", "Allowance" },
                    { "Deduction", "Deduction" },

                    { "HpAllow", "Handphone Allowance (Tax)" },
                    { "GrossAllow", "Gross Allowance" },

                    { "HIns", "Health Insurance" },
                    { "RIns", "Retirement Insurance" },
                    { "TotalDeduct", "Total Deduction" },
                    { "THP", "Take Home Pay" },

                    { "InWords", "In Words" }
                };

                DataSource = new DataSet { Tables = { title, data } };

                Dbg("DataSource", DataSource);
            }

            protected override void AdditionalSetupExcelDocument() {
                var pageSetting = new SLPageSettings { ShowGridLines = false, ZoomScale = 65 };

                String sheet1Name = DocumentName;
                document.RenameWorksheet(SLDocument.DefaultFirstSheetName, sheet1Name);
                document.SetPageSettings(pageSetting, sheet1Name);
                document.SelectWorksheet(sheet1Name);
            }

            protected override void RenderTitle() {
                DataTable dataSource = DataSource.Tables["TitleDataTable"];

                if (dataSource != null) {
                    /* NOTE: do coding here. */

                    rowIndex += 2;
                    columnIndex = documentStat.StartColumnIndex + 2;

                    document.SetRowHeight(rowIndex, 120);
                    document.SetCellValue(rowIndex, columnIndex, dataSource.Rows[0][0].ToString());

                    Int32 endColumnIndex = columnIndex + dataSource.Columns.Count;
                    SLStyle style = document.CreateStyle();
                    style.Font = new SLFont {
                        FontName = "Arial",
                        Bold = true,
                        FontSize = 20,
                        FontColor = System.Drawing.Color.Black
                    };
                    style.SetWrapText(false);
                    document.AutoFitRow(rowIndex);

                    style.RemoveFill();
                    style.RemoveBorder();
                    document.SetCellStyle(rowIndex, columnIndex, style);
                    document.MergeWorksheetCells(rowIndex, columnIndex, rowIndex, endColumnIndex);

                    rowIndex++;
                }
            }

            protected override void RenderHeader() {
                DataTable dataSource = DataSource.Tables["HeaderDataTable"];

                if (dataSource != null) { /* NOTE: do coding here. */ }
            }

            protected override void RenderBody() {
                DataTable dataSource = DataSource.Tables["DataTable"];
                DataTable dataGroupSource = DataSource.Tables["GroupDataTable"];

                if (dataSource != null) {
                    if (dataGroupSource != null) { /* NOTE: do coding here. */ }

                    /* NOTE: do coding here. */

                    rowIndex += 2;
                    columnIndex = documentStat.StartColumnIndex + 2;

                    Int32
                        leftValueColIndex = columnIndex + 4,
                        rightColIndex = leftValueColIndex + 4,
                        rightValueColIndex = rightColIndex + 4;

                    SLStyle defaultStyle = document.CreateStyle();
                    defaultStyle.Font = new SLFont { FontName = "Arial" };
                    defaultStyle.RemoveFill();
                    defaultStyle.RemoveBorder();

                    document.SetCellStyle(rowIndex, columnIndex, defaultStyle);
                    document.SetCellValue(rowIndex, columnIndex, MapColumnIntoHeader("Period"));
                    document.SetCellValue(rowIndex, leftValueColIndex, dataSource.Rows[0]["Period"].ToString());
                    rowIndex++;

                    document.SetCellStyle(rowIndex, columnIndex, defaultStyle);
                    document.SetCellValue(rowIndex, columnIndex, MapColumnIntoHeader("EmpCode"));
                    document.SetCellValue(rowIndex, leftValueColIndex, dataSource.Rows[0]["EmpCode"].ToString());
                    rowIndex++;

                    document.SetCellStyle(rowIndex, columnIndex, defaultStyle);
                    document.SetCellValue(rowIndex, columnIndex, MapColumnIntoHeader("JobPos"));
                    document.SetCellValue(rowIndex, leftValueColIndex, dataSource.Rows[0]["JobPos"].ToString());
                    rowIndex++;

                    document.SetCellStyle(rowIndex, columnIndex, defaultStyle);
                    document.SetCellValue(rowIndex, columnIndex, MapColumnIntoHeader("Dept"));
                    document.SetCellValue(rowIndex, leftValueColIndex, dataSource.Rows[0]["Dept"].ToString());
                    rowIndex++;

                    document.SetCellStyle(rowIndex, columnIndex, defaultStyle);
                    document.SetCellValue(rowIndex, columnIndex, MapColumnIntoHeader("Currency"));
                    document.SetCellValue(rowIndex, leftValueColIndex, dataSource.Rows[0]["Currency"].ToString());
                    rowIndex++;

                    document.SetCellStyle(rowIndex, columnIndex, defaultStyle);
                    document.SetCellValue(rowIndex, columnIndex, MapColumnIntoHeader("BasicSalary"));
                    document.SetCellValue(rowIndex, leftValueColIndex, dataSource.Rows[0]["BasicSalary"].ToString());
                    rowIndex++;

                    document.SetCellStyle(rowIndex, columnIndex, defaultStyle);
                    document.SetCellValue(rowIndex, columnIndex, MapColumnIntoHeader("TaxAllow"));
                    document.SetCellValue(rowIndex, leftValueColIndex, dataSource.Rows[0]["TaxAllow"].ToString());
                    rowIndex++;

                    document.SetCellStyle(rowIndex, columnIndex, defaultStyle);
                    document.SetCellValue(rowIndex, columnIndex, MapColumnIntoHeader("Tax"));
                    document.SetCellValue(rowIndex, leftValueColIndex, dataSource.Rows[0]["Tax"].ToString());
                    rowIndex++;

                    document.SetCellStyle(rowIndex, columnIndex, defaultStyle);
                    document.SetCellValue(rowIndex, columnIndex, MapColumnIntoHeader("Allow"));
                    document.SetCellValue(rowIndex, leftValueColIndex, dataSource.Rows[0]["Allow"].ToString());
                    rowIndex++;

                    document.SetCellStyle(rowIndex, columnIndex, defaultStyle);
                    document.SetCellValue(rowIndex, columnIndex, MapColumnIntoHeader("Deduction"));
                    document.SetCellValue(rowIndex, leftValueColIndex, dataSource.Rows[0]["Deduction"].ToString());
                    rowIndex++;

                    document.SetCellStyle(rowIndex, columnIndex, defaultStyle);
                    document.SetCellValue(rowIndex, columnIndex, MapColumnIntoHeader("HpAllow"));
                    document.SetCellValue(rowIndex, leftValueColIndex, dataSource.Rows[0]["HpAllow"].ToString());
                    rowIndex++;

                    document.SetCellStyle(rowIndex, columnIndex, defaultStyle);
                    document.SetCellValue(rowIndex, columnIndex, MapColumnIntoHeader("GrossAllow"));
                    document.SetCellValue(rowIndex, leftValueColIndex, dataSource.Rows[0]["GrossAllow"].ToString());
                    rowIndex++;

                    document.SetCellStyle(rowIndex, columnIndex, defaultStyle);
                    document.SetCellValue(rowIndex, columnIndex, MapColumnIntoHeader("HIns"));
                    document.SetCellValue(rowIndex, leftValueColIndex, dataSource.Rows[0]["HIns"].ToString());
                    rowIndex++;

                    document.SetCellStyle(rowIndex, columnIndex, defaultStyle);
                    document.SetCellValue(rowIndex, columnIndex, MapColumnIntoHeader("RIns"));
                    document.SetCellValue(rowIndex, leftValueColIndex, dataSource.Rows[0]["RIns"].ToString());
                    rowIndex++;

                    document.SetCellStyle(rowIndex, columnIndex, defaultStyle);
                    document.SetCellValue(rowIndex, columnIndex, MapColumnIntoHeader("TotalDeduct"));
                    document.SetCellValue(rowIndex, leftValueColIndex, dataSource.Rows[0]["TotalDeduct"].ToString());
                    rowIndex++;

                    document.SetCellStyle(rowIndex, columnIndex, defaultStyle);
                    document.SetCellValue(rowIndex, columnIndex, MapColumnIntoHeader("THP"));
                    document.SetCellValue(rowIndex, leftValueColIndex, dataSource.Rows[0]["THP"].ToString());
                    rowIndex++;

                    document.SetCellStyle(rowIndex, columnIndex, defaultStyle);
                    document.SetCellValue(rowIndex, columnIndex, MapColumnIntoHeader("InWords"));
                    document.SetCellValue(rowIndex, leftValueColIndex, dataSource.Rows[0]["InWords"].ToString());
                    rowIndex++;
                }
            }

            protected override void RenderFooter() {
                DataTable dataSource = DataSource.Tables["FooterDataTable"];

                if (dataSource != null) { /* NOTE: do coding here. */ }
            }
        }

        #endregion
    }
}
