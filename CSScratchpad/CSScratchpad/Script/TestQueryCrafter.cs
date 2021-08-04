using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scratch;

namespace CSScratchpad.Script {
    public static class QueryCrafterStringExt {
        public static String TrimStart(this String target, String trimString) {
            String result = target;
            while (result.StartsWith(trimString))
                result = result.Substring(trimString.Length);

            return result;
        }

        public static String TrimEnd(this String target, String trimString) {
            String result = target;
            while (result.EndsWith(trimString))
                result = result.Substring(0, result.Length - trimString.Length);

            return result;
        }
    }

    public static class IQueryComponentExt {
        public static String Craft(this IQueryComponent component) {
            var sql = new StringBuilder()
                .AppendLine("SELECT");

            if (!component.Columns.Any())
                throw new InvalidOperationException($"Query can't be crafted. Cause: '{nameof(component.Columns)}'.");

            String columns = String.Join($",{Environment.NewLine}", component.Columns);
            sql.AppendLine(columns);

            if (component.Tables.Any()) {
                String tables = String.Join(Environment.NewLine, component.Tables.Select(table => $"FROM {table}"));
                sql.AppendLine(tables);
            }

            if (component.Predicates.Any()) {
                String wheres = String.Join(Environment.NewLine, component.Predicates).TrimStart("AND ").TrimStart("OR ");
                sql.AppendLine($"WHERE {wheres}");
            }

            return sql.ToString();
        }
    }

    public interface IQueryComponent {
        IList<String> Columns { get; }
        IList<String> Tables { get; }
        IList<String> Predicates { get; }
        IList<String> GroupBys { get; }
        IList<String> Orders { get; }
    }

    public interface ICraftable {
        String Craft();
    }

    public interface IQueryStatementCreator : IQueryComponent {
        IColumnSelector Select(params String[] columns);
        IColumnSelector SelectAll();
        IInsert Insert();
        IInsert InsertInto();
        IUpdate Update();
        IDelete Delete();
    }

    public interface IDelete {
    }

    public interface IUpdate {
    }

    public interface IInsert {
    }

    public interface IColumnSelector : ICraftable {
        ITableSelector From(params String[] tables);
    }

    public class ColumnSelector : IColumnSelector {
        readonly IQueryComponent component;

        public ColumnSelector(String[] columns, IQueryComponent component) {
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));
            if (!columns.Any())
                throw new ArgumentOutOfRangeException(nameof(columns));
            if (columns.Any(column => column.Trim() == String.Empty))
                throw new InvalidOperationException("Invalid columns.");

            this.component = component??throw new ArgumentNullException(nameof(component));
            foreach (String column in columns)
                component.Columns.Add(column);
        }

        public ITableSelector From(params String[] tables) => new TableSelector(tables, component);

        public String Craft() => component.Craft();
    }

    public interface ITableSelector : ICraftable {
        IPredicate Where(String predicate);
    }

    public class TableSelector : ITableSelector {
        readonly IQueryComponent component;

        public TableSelector(String[] tables, IQueryComponent component) {
            if (tables == null)
                throw new ArgumentNullException(nameof(tables));
            if (!tables.Any())
                throw new ArgumentOutOfRangeException(nameof(tables));
            if (tables.Any(table => table.Trim() == String.Empty))
                throw new InvalidOperationException("Invalid tables.");

            this.component = component??throw new ArgumentNullException(nameof(component));
            foreach (String table in tables)
                component.Tables.Add(table);
        }

        public IPredicate Where(String predicate) => new Predicate(predicate, component);

        public String Craft() => component.Craft();
    }

    public interface IPredicate : ICraftable {
        IPredicate AndWhere(String predicate);
        IPredicate OrWhere(String predicate);
    }

    public class Predicate : IPredicate {
        readonly IQueryComponent component;

        public Predicate(String predicate, IQueryComponent component) {
            if (String.IsNullOrEmpty(predicate))
                throw new ArgumentNullException(nameof(predicate));

            this.component = component??throw new ArgumentNullException(nameof(component));
            component.Predicates.Add(predicate);
        }

        public IPredicate AndWhere(String predicate) {
            component.Predicates.Add($"AND {predicate}");

            return this;
        }

        public IPredicate OrWhere(String predicate) {
            component.Predicates.Add($"OR {predicate}");

            return this;
        }

        public String Craft() => component.Craft();
    }

    public class QueryStatementCreator : IQueryStatementCreator {
        public IList<String> Columns { get; } = new List<String>();

        public IList<String> Tables { get; } = new List<String>();

        public IList<String> Predicates { get; } = new List<String>();

        public IList<String> GroupBys { get; } = new List<String>();

        public IList<String> Orders { get; } = new List<String>();

        public IColumnSelector Select(params String[] columns) => new ColumnSelector(columns, this);

        public IColumnSelector SelectAll() => new ColumnSelector(new[] { "*" }, this);

        public IInsert Insert() => throw new NotImplementedException();

        public IInsert InsertInto() => throw new NotImplementedException();

        public IUpdate Update() => throw new NotImplementedException();

        public IDelete Delete() => throw new NotImplementedException();
    }

    public class TestQueryCrafter : Common, IRunnable {
        public void Run() {
            DisplayTitle("Select");
            Console.WriteLine(
                new QueryStatementCreator()
                    .Select("CompanyName", "ContactName")
                    .Craft());

            DisplayTitle("From");
            Console.WriteLine(
                new QueryStatementCreator()
                    .Select("CompanyName", "ContactName")
                    .From("Customers")
                    .Craft());

            DisplayTitle("Where");
            Console.WriteLine(
                new QueryStatementCreator()
                    .Select("CompanyName", "ContactName")
                    .From("Customers")
                    .Where("CompanyName LIKE '%@Condition1%'")
                    .AndWhere("ContactTitle = @Condition2")
                    .Craft());
        }
    }
}
