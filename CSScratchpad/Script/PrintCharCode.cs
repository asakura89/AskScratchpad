using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scratch;

namespace CSScratchpad.Script {
    public class PrintCharCode : Common, IRunnable {
        public void Run()
        {
            //Console.OutputEncoding = System.Text.Encoding.Unicode; // None encoding works

            //const String String1 = "⠋ ⠙ ⠹ ⠸ ⠼ ⠴ ⠦ ⠧ ⠇ ⠏ 建国先贤纪念园 卫生科学局 防止网络假信息和网络操纵办事处";
            //const String String2 = "⣾ ⣽ ⣻ ⢿ ⡿ ⣟ ⣯ ⣷";

            //Dbg("String1", String1);
            //Dbg("String1.Analyzed",
            //    String1
            //        .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
            //        .Select(ch => new Dictionary<String, String> {
            //            //NumbericValue = Char.GetNumericValue(Convert.ToChar(ch)),
            //            //UnicodeCategory = Char.GetUnicodeCategory(Convert.ToChar(ch))
            //            ["UTF-8"] = String.Join(" ", Encoding.UTF8.GetBytes(str).Select(byt => byt.ToString("x2"))),
            //            ["UTF-16"] = String.Join(" ", Encoding.BigEndianUnicode.GetBytes(str).Select(byt => byt.ToString("x2")))
            //        })
            //);

            //Dbg("String2", String2);
            //Dbg("String2.Analyzed",
            //    String2
            //        .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
            //        .Select(ch => new Dictionary<String, String>() {
            //            //NumbericValue = Char.GetNumericValue(Convert.ToChar(ch)),
            //            //UnicodeCategory = Char.GetUnicodeCategory(Convert.ToChar(ch))
            //            ["UTF-8"] = String.Join(" ", Encoding.UTF8.GetBytes(str).Select(byt => byt.ToString("x2"))),
            //            ["UTF-16"] = String.Join(" ", Encoding.BigEndianUnicode.GetBytes(str).Select(byt => byt.ToString("x2")))
            //        })
            //);

            String chinese = "卫生科学局";
            var dt = new DataTable();
            dt.Columns.AddRange(new[] {
                new DataColumn("Char", typeof(String)),
                new DataColumn("UTF-8", typeof(String)),
                new DataColumn("UTF-16", typeof(String))
            });

            for (Int32 idx = 0; idx < chinese.Length; idx++) {
                DataRow row = dt.NewRow();
                row["Char"] = chinese[idx].ToString();
                row["UTF-8"] = String.Join(" ", Encoding.UTF8.GetBytes(new[] {chinese[idx]}).Select(byt => byt.ToString("x2")));
                row["UTF-16"] = String.Join(" ", Encoding.BigEndianUnicode.GetBytes(new[] {chinese[idx]}).Select(byt => byt.ToString("x2")));

                dt.Rows.Add(row);
            }

            IDictionary<Int32, Int32> columnLengths = GetColumnLengths(dt);
            Console.WriteLine(new StringBuilder()
                .AppendLine(CreateHeader(dt, columnLengths, tableStyle))
                .AppendLine(CreateBody(dt, columnLengths, tableStyle))
                .ToString());

            var ascii = ".?q`w~e!r@t#y$ui^op*a(s)d_f+ gh{jk-l]z:x[c}vbnmQWER|T0Y1U2I3O4P5A6S7D8F\\9GHJ\"KL/ZX=C<V'B%>N&M;,";

            dt = new DataTable();
            dt.Columns.AddRange(new[] {
                new DataColumn("Char", typeof(String)),
                new DataColumn("ASCII", typeof(String))
            });

            for (Int32 idx = 0; idx < ascii.Length; idx++) {
                DataRow row = dt.NewRow();
                row["Char"] = ascii[idx].ToString();
                row["ASCII"] = String.Join(" ", Encoding.UTF8.GetBytes(new[] {ascii[idx]}).Select(byt => byt.ToString("x2")));

                dt.Rows.Add(row);
            }

            columnLengths = GetColumnLengths(dt);
            Console.WriteLine(new StringBuilder()
                .AppendLine(CreateHeader(dt, columnLengths, tableStyle))
                .AppendLine(CreateBody(dt, columnLengths, tableStyle))
                .ToString());

            var orderedAscii = ascii
                .ToCharArray()
                .OrderBy(ch => ch)
                .ToArray();

            dt.Rows.Clear();

            for (Int32 idx = 0; idx < orderedAscii.Length; idx++) {
                DataRow row = dt.NewRow();
                row["Char"] = orderedAscii[idx].ToString();
                row["ASCII"] = String.Join(" ", Encoding.UTF8.GetBytes(new[] {orderedAscii[idx]}).Select(byt => byt.ToString("x2")));

                dt.Rows.Add(row);
            }

            columnLengths = GetColumnLengths(dt);
            Console.WriteLine(new StringBuilder()
                .AppendLine(CreateHeader(dt, columnLengths, tableStyle))
                .AppendLine(CreateBody(dt, columnLengths, tableStyle))
                .ToString());
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
