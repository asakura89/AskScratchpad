using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Scratch;

namespace CSScratchpad.Script {
    class PrintTable : Common, IRunnable {
        public void Run() {
            #region :: Data ::

            var tasks = new DataTable();
            tasks.Columns.AddRange(new[] {
                new DataColumn("Task Id", typeof(String)),
                new DataColumn("Title", typeof(String)),
                new DataColumn("Note", typeof(String)),
                new DataColumn("Recurrence", typeof(RecurrenceType)),
                new DataColumn("Recurrence Count", typeof(Int32)),
                new DataColumn("Done", typeof(Boolean))
            });

            DataRow row = tasks.NewRow();
            row["Task Id"] = "0020";
            row["Title"] = "Beli ikan";
            row["Note"] = String.Empty;
            row["Recurrence"] = RecurrenceType.None;
            row["Recurrence Count"] = 0;
            row["Done"] = false;
            tasks.Rows.Add(row);

            row = tasks.NewRow();
            row["Task Id"] = "0420";
            row["Title"] = "Beli kentang";
            row["Note"] = String.Empty;
            row["Recurrence"] = RecurrenceType.None;
            row["Recurrence Count"] = 0;
            row["Done"] = false;
            tasks.Rows.Add(row);

            row = tasks.NewRow();
            row["Task Id"] = "0320";
            row["Title"] = "Beli cabe";
            row["Note"] = String.Empty;
            row["Recurrence"] = RecurrenceType.None;
            row["Recurrence Count"] = 0;
            row["Done"] = false;
            tasks.Rows.Add(row);

            row = tasks.NewRow();
            row["Task Id"] = "0342";
            row["Title"] = "Beli kecap";
            row["Note"] = String.Empty;
            row["Recurrence"] = RecurrenceType.None;
            row["Recurrence Count"] = 0;
            row["Done"] = false;
            tasks.Rows.Add(row);

            row = tasks.NewRow();
            row["Task Id"] = "09872";
            row["Title"] = "fixing bug website js";
            row["Note"] = String.Empty;
            row["Recurrence"] = RecurrenceType.None;
            row["Recurrence Count"] = 0;
            row["Done"] = false;
            tasks.Rows.Add(row);

            row = tasks.NewRow();
            row["Task Id"] = "09873";
            row["Title"] = "integrasi modul bar";
            row["Note"] = String.Empty;
            row["Recurrence"] = RecurrenceType.None;
            row["Recurrence Count"] = 0;
            row["Done"] = false;
            tasks.Rows.Add(row);

            row = tasks.NewRow();
            row["Task Id"] = "97986";
            row["Title"] = "ngepel rumah";
            row["Note"] = String.Empty;
            row["Recurrence"] = RecurrenceType.None;
            row["Recurrence Count"] = 0;
            row["Done"] = false;
            tasks.Rows.Add(row);

            row = tasks.NewRow();
            row["Task Id"] = "97768";
            row["Title"] = "potong rumput";
            row["Note"] = String.Empty;
            row["Recurrence"] = RecurrenceType.None;
            row["Recurrence Count"] = 0;
            row["Done"] = false;
            tasks.Rows.Add(row);

            row = tasks.NewRow();
            row["Task Id"] = "97868";
            row["Title"] = "siram halaman";
            row["Note"] = String.Empty;
            row["Recurrence"] = RecurrenceType.None;
            row["Recurrence Count"] = 0;
            row["Done"] = false;
            tasks.Rows.Add(row);

            #endregion

            IDictionary<Int32, Int32> columnLengths = GetColumnLengths(tasks);
            Dbg(columnLengths);

            Console.WriteLine(
                new StringBuilder()
                    .AppendLine(CreateLineRow(columnLengths, tableStyle.TopLeftCorner, tableStyle.TopMiddleCorner, tableStyle.TopRightCorner, tableStyle.HeaderTopHorizontal))
                    .AppendLine(CreateDataRow(columnLengths, tableStyle.HeaderLeftVertical, tableStyle.HeaderMiddleVertical, tableStyle.HeaderRightVertical, tasks.Columns.Cast<DataColumn>().Select(dc => dc.ColumnName).ToArray()))
                    .AppendLine(CreateLineRow(columnLengths, tableStyle.MiddleLeftCorner, tableStyle.CenterCorner, tableStyle.MiddleRightCorner, tableStyle.HeaderBottomHorizontal))
                    .ToString()
            );

            Dbg(CreateHeader(tasks, columnLengths, tableStyle));
            Console.WriteLine(new StringBuilder()
                .AppendLine(CreateHeader(tasks, columnLengths, tableStyle))
                .AppendLine(CreateBody(tasks, columnLengths, tableStyle))
                .ToString());
        }

        public enum RecurrenceType {
            None,
            Days,
            Weeks,
            Months,
            Years
        }

        public class TableStyle {
            public String TopLeftCorner { get; set; }
            public String TopMiddleCorner { get; set; }
            public String TopRightCorner { get; set; }
            public String MiddleLeftCorner { get; set; }
            public String CenterCorner { get; set; }
            public String MiddleRightCorner { get; set; }
            public String BottomLeftCorner { get; set; }
            public String BottomMiddleCorner { get; set; }
            public String BottomRightCorner { get; set; }
            public String HeaderLeftVertical { get; set; }
            public String HeaderMiddleVertical { get; set; }
            public String HeaderRightVertical { get; set; }
            public String HeaderTopHorizontal { get; set; }
            public String HeaderBottomHorizontal { get; set; }
            public String BodyLeftVertical { get; set; }
            public String BodyMiddleVertical { get; set; }
            public String BodyRightVertical { get; set; }
            public String BodyMiddleHorizontal { get; set; }
            public String BodyBottomHorizontal { get; set; }
        }

        static readonly TableStyle tableStyle = new TableStyle {
            TopLeftCorner = "+",
            TopMiddleCorner = "+",
            TopRightCorner = "+",
            MiddleLeftCorner = "+",
            CenterCorner = "+",
            MiddleRightCorner = "+",
            BottomLeftCorner = "+",
            BottomMiddleCorner = "+",
            BottomRightCorner = "+",
            HeaderLeftVertical = "|",
            HeaderMiddleVertical = "|",
            HeaderRightVertical = "|",
            HeaderTopHorizontal = "-",
            HeaderBottomHorizontal = "-",
            BodyLeftVertical = "|",
            BodyMiddleVertical = "|",
            BodyRightVertical = "|",
            BodyMiddleHorizontal = "-",
            BodyBottomHorizontal = "-"
        };

        IDictionary<Int32, Int32> GetColumnLengths(DataTable dt) {
            var columnLength = new Dictionary<Int32, Int32>();
            for (Int32 cidx = 0; cidx < dt.Columns.Count; cidx++) {
                var rowLengths = new List<Int32>();
                rowLengths.Add(dt.Columns[cidx].ColumnName.Trim().Length);
                for (Int32 ridx = 0; ridx < dt.Rows.Count; ridx++)
                    rowLengths.Add(dt.Rows[ridx][cidx].ToString().Trim().Length);
                columnLength[cidx] = rowLengths.Max();
            }

            return columnLength;
        }

        String CreateLineRow(IDictionary<Int32, Int32> columnLengths, String left, String middle, String right, String horizontal) {
            var rowBuilder = new StringBuilder();
            for (Int32 idx = 0; idx < columnLengths.Count; idx++) {
                Int32 length = columnLengths[idx] +2; // left and right space
                if (idx == 0) {
                    rowBuilder
                        .Append(left)
                        .Append("".PadLeft(length, horizontal[0]));
                }
                else if (idx < columnLengths.Count -1) {
                    rowBuilder
                        .Append(middle)
                        .Append("".PadLeft(length, horizontal[0]));
                }
                else {
                    rowBuilder
                        .Append(middle)
                        .Append("".PadLeft(length, horizontal[0]))
                        .Append(right);
                }
            }

            return rowBuilder.ToString();
        }

        String CreateDataRow(IDictionary<Int32, Int32> columnLengths, String left, String middle, String right, String[] data) {
            var rowBuilder = new StringBuilder();
            for (Int32 idx = 0; idx < columnLengths.Count; idx++) {
                Int32 length = columnLengths[idx] +1; // right space
                if (idx == 0) {
                    rowBuilder
                        .Append(left)
                        .Append(" ") // left space added manually
                        .Append(data[idx].PadRight(length, ' '));
                }
                else if (idx < columnLengths.Count -1) {
                    rowBuilder
                        .Append(middle)
                        .Append(" ")
                        .Append(data[idx].PadRight(length, ' '));
                }
                else {
                    rowBuilder
                        .Append(middle)
                        .Append(" ")
                        .Append(data[idx].PadRight(length, ' '))
                        .Append(right);
                }
            }

            return rowBuilder.ToString();
        }

        String CreateHeader(DataTable dt, IDictionary<Int32, Int32> columnLengths, TableStyle style) =>
            new StringBuilder()
                .AppendLine(CreateLineRow(columnLengths, style.TopLeftCorner, style.TopMiddleCorner, style.TopRightCorner, style.HeaderTopHorizontal))
                .AppendLine(CreateDataRow(columnLengths, style.HeaderLeftVertical, style.HeaderMiddleVertical, style.HeaderRightVertical, dt.Columns.Cast<DataColumn>().Select(dc => dc.ColumnName).ToArray()))
                .Append(CreateLineRow(columnLengths, style.MiddleLeftCorner, style.CenterCorner, style.MiddleRightCorner, style.HeaderBottomHorizontal))
                .ToString();

        String CreateBody(DataTable dt, IDictionary<Int32, Int32> columnLengths, TableStyle style) {
            var bodyBuilder = new StringBuilder();
            for (Int32 ridx = 0; ridx < dt.Rows.Count; ridx++)
                bodyBuilder.AppendLine(CreateDataRow(columnLengths, style.BodyLeftVertical, style.BodyMiddleVertical, style.BodyRightVertical, dt.Rows[ridx].ItemArray.Select(item => item as String ?? Convert.ToString(item)).ToArray()));

            bodyBuilder.Append(CreateLineRow(columnLengths, style.BottomLeftCorner, style.BottomMiddleCorner, style.BottomRightCorner, style.BodyBottomHorizontal));
            return bodyBuilder.ToString();
        }
    }
}