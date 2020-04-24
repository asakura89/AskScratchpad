using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Scratch;

namespace CSScratchpad.Script {
    public class PrintQueryBuilder : Common, IRunnable {
        public void Run() {
            var qb = new QueryBuilder()
                .Select<User>()
                .Build();
            Dbg(qb);

            qb = new QueryBuilder()
                .Select<User>(us => us.Name, us => us.Email)
                .Build();
            Dbg(qb);
        }

        [Table("tb_User")]
        class User {
            [Column("col_User_Id", PrimaryKey = true)]
            public String Id { get; set; }

            [Column("col_Name")]
            public String Name { get; set; }

            [Column("col_Email")]
            public String Email { get; set; }
        }

        class QueryBuilder {
            StringBuilder inner = new StringBuilder();
            const String SelectTemplate = "SELECT {0} FROM {1}";

            public QueryBuilder Select<TData>() where TData : class {
                String table = GetTableName(typeof(TData));
                inner.AppendFormat(SelectTemplate, "*", table);
                return this;
            }

            public QueryBuilder Select<TData>(params Expression<Func<TData, Object>>[] expressions) where TData : class {
                String table = GetTableName(typeof(TData));
                String columns = GetColumnsName(expressions);
                inner.AppendFormat(SelectTemplate, columns, table);
                return this;
            }

            String GetTableName(Type dataType) {
                TableAttribute info = dataType
                    .GetDecorators<TableAttribute>()
                    .Single();

                return info.Name;
            }

            String GetColumnsName<TData>(IEnumerable<Expression<Func<TData, Object>>> exprs) where TData : class {
                if (exprs == null || !exprs.Any())
                    return String.Empty;

                IList<String> columnNames = new List<String>();
                IList<ColumnInfo> columns = typeof(TData).GetColumnInfos();
                Func<ColumnInfo, MemberExpression, Boolean> nameComparer = (col, expr) => col.Property.Name == expr.Member.Name;
                foreach (Expression<Func<TData, Object>> expr in exprs) {
                    if (expr.Body is MemberExpression) {
                        var innerExpr = expr.Body as MemberExpression;
                        if (columns.Any(col => nameComparer(col, innerExpr))) {
                            columnNames.Add(columns.Single(col => nameComparer(col, innerExpr)).Column.Name);
                        }
                    }
                }

                return String.Join(", ", columnNames);
            }

            public String Build() => inner.ToString();
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    class TableAttribute : Attribute {
        public String Name { get; set; }
        public String DisplayName { get; set; }

        public TableAttribute(String name, String displayName) {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            DisplayName = displayName;
        }

        public TableAttribute(String name) : this(name, String.Empty) { }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    class ColumnAttribute : Attribute {
        public String Name { get; set; }
        public String DisplayName { get; set; }
        public Boolean PrimaryKey { get; set; }
        public Int32 Length { get; set; }
        public Int32 Index { get; set; } = -1;
        public Boolean OutputParameter { get; set; }

        public ColumnAttribute(String name, String displayName) {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            DisplayName = displayName;
        }

        public ColumnAttribute(String name) : this(name, String.Empty) { }
    }

    class ObjectMember {
        public String Name { get; }
        public Type Type { get; }

        public ObjectMember(String name, Type type) {
            Name = name;
            Type = type;
        }
    }

    [Serializable]
    class DataType {
        public String Name { get; }
        public Object Value { get; }
        public Type Type { get; }

        public DataType(String name, Object value, Type type) {
            Name = name;
            Value = value;
            Type = type;
        }
    }

    sealed class ColumnInfo {
        public ColumnAttribute Column { get; set; }
        public ObjectMember Property { get; set; }
    }

    static class QueryBExt {
        public static IList<TAttribute> GetDecorators<TAttribute>(this Type objType) =>
            objType
                .GetCustomAttributes(typeof(TAttribute), false)
                .Cast<TAttribute>()
                .ToList();

        public static IList<TAttribute> GetDecorators<TAttribute, T>(this T obj) where T : class =>
            obj
                .GetType()
                .GetDecorators<TAttribute>();

        public static IList<TAttribute> GetMemberDecorators<TAttribute>(this Type objType) =>
            objType
                .GetProperties()
                .SelectMany(prop => prop
                    .GetCustomAttributes(typeof(TAttribute), false))
                .Cast<TAttribute>()
                .ToList();

        public static IList<TAttribute> GetMemberDecorators<TAttribute, T>(this T obj) where T : class =>
            obj
                .GetType()
                .GetMemberDecorators<TAttribute>();

        public static IList<ColumnInfo> GetColumnInfos<T>(this T data) where T : class => data
            .GetType()
            .GetColumnInfos();

        public static IList<ColumnInfo> GetColumnInfos(this Type dataType) => dataType
            .GetProperties()
            .Select(prop => new ColumnInfo {
                Column = prop.GetCustomAttributes(typeof(ColumnAttribute), false).Single() as ColumnAttribute,
                Property = new ObjectMember(prop.Name, prop.PropertyType)
            })
            .ToList();
    }
}
