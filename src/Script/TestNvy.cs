using System;
using System.Collections.Generic;
using System.Linq;
using static CSScratchpad.Script.TestNvy;

namespace CSScratchpad.Script {
    class TestNvy : Common, IRunnable {
        public void Run() {
            Dbg("Menu : LinkItem<String, String>", GetMenus());

            IEnumerable<NotificationUser> users = GetUsers();
            String delimitedString = NameValueItemExtensions.AsDelimitedString(users, u => u.Name, u => u.Email);

            Dbg("Delimited String", delimitedString);
            Dbg("NameValueList", NameValueItemExtensions.AsNameValueList(delimitedString));

            IList<String> emails = users
                .Where(u => !String.IsNullOrEmpty(u.Email))
                .Select(u => u.Email.ToLowerInvariant())
                .GroupBy(e => e).Select(g => g.First())
                .ToList();

            Dbg("ExDelimitedString", NameValueItemExtensions.AsExDelimitedString(emails, ";"));
            Dbg("ExDelimitedString", NameValueItemExtensions.AsExDelimitedString(emails, ";", s => s.ToString()));
            Dbg("ExDelimitedString", NameValueItemExtensions.AsExDelimitedString(emails, ";", s => NameValueItemExtensions.AsExDelimitedString(s, ",", ss => ss.ToString())));

            Dbg("ExDelimitAggregateedString", emails.Aggregate(String.Empty, (p, nxt) => p + (p == String.Empty ? String.Empty : ";") + nxt));

            Dbg("ExDelimitedString", NameValueItemExtensions.AsExDelimitedString(users, ";", ",", u => u.Name, u => u.Email));
            Dbg("ExDelimitedString", NameValueItemExtensions.AsExDelimitedString(users, ";", ",", u => u.Name, u => u.Email, u => u.Name, u => u.Name));
        }

        #region : Data :

        IEnumerable<Menu> GetMenus() {
            yield return new Menu() {
                Name = "Pizza",
                Link = "https://www.pizzahut.co.id/index.php?lang=IN&mib=menus.detail&id=2009100215260801"
            };

            yield return new Menu() {
                Name = "Pasta",
                Link = "https://www.pizzahut.co.id/index.php?lang=IN&mib=menus.detail&id=2009100215272402"
            };

            yield return new Menu() {
                Name = "Nasi",
                Link = "https://www.pizzahut.co.id/index.php?lang=IN&mib=menus.detail&id=2009100215282203"
            };

            yield return new Menu() {
                Name = "Minuman",
                Link = "https://www.pizzahut.co.id/index.php?lang=IN&mib=menus.detail&id=2009100215284004"
            };

            yield return new Menu() {
                Name = "Hidangan Sampingan",
                Link = "https://www.pizzahut.co.id/index.php?lang=IN&mib=menus.detail&id=2009100215285805"
            };

            yield return new Menu() {
                Name = "Hidangan Penutup",
                Link = "https://www.pizzahut.co.id/index.php?lang=IN&mib=menus.detail&id=2009100215291706"
            };
        }

        IEnumerable<NotificationUser> GetUsers() {
            yield return new NotificationUser();
        }

        #endregion

        public class Menu : ListItem<String, String> {
            public new String Name {
                get {
                    return base.Name;
                }

                set {
                    base.Name = value;
                }
            }

            public String Link {
                get {
                    return Value;
                }

                set {
                    Value = value;
                }
            }
        }

        public class NotificationUser {
            public String Name { get; set; }
            public String Email { get; set; }
        }

        #region : Nvy :

        [Serializable]
        public abstract class ListItem<N, V> {
            protected N Name { get; set; }
            protected V Value { get; set; }
        }

        [Serializable]
        public sealed class NameValueItem : ListItem<String, String> {
            public const String NameProperty = "Name";
            public const String ValueProperty = "Value";
            public const Char ListDelimiter = '·';
            public const Char ItemDelimiter = '•';

            public new String Name {
                get {
                    return base.Name;
                }

                private set {
                    base.Name = value;
                }
            }

            public new String Value {
                get {
                    return base.Value;
                }

                private set {
                    base.Value = value;
                }
            }

            public static NameValueItem Empty => new NameValueItem(String.Empty, String.Empty);

            public static NameValueItem None => new NameValueItem("None", String.Empty);

            public NameValueItem(String name, String value) {
                Name = name;
                Value = value;
            }

            public NameValueItem() : this(String.Empty, String.Empty) { }
        }

        public static class NameValueItemExtensions {
            public static IEnumerable<NameValueItem> AsNameValueList(String delimitedString) {
                String[] splittedList = delimitedString.Split(NameValueItem.ListDelimiter);
                return splittedList
                    .Select(item => item.Split(NameValueItem.ItemDelimiter))
                    .Select(splittedItem => new NameValueItem(splittedItem[0], splittedItem[1]));
            }

            public static IEnumerable<NameValueItem> AsNameValueList<T>(IEnumerable<T> dataList, Func<T, String> nameSelector, Func<T, String> valueSelector) where T : class {
                return dataList.Select(data => new NameValueItem(nameSelector(data), valueSelector(data)));
            }

            public static String AsDelimitedString(IEnumerable<NameValueItem> nameValueList) {
                String[] delimitedStringList = nameValueList.Select(item => item.Name + NameValueItem.ItemDelimiter + item.Value).ToArray();
                String delimitedString = String.Join(NameValueItem.ListDelimiter.ToString(), delimitedStringList);

                return delimitedString;
            }

            public static String AsDelimitedString<T>(IEnumerable<T> dataList, Func<T, String> nameSelector, Func<T, String> valueSelector) where T : class {
                return NameValueItemExtensions.AsDelimitedString(NameValueItemExtensions.AsNameValueList(dataList, nameSelector, valueSelector));
            }

            public static String AsExDelimitedString<T>(T t, String itemDelimiter, params Func<T, String>[] tSelector) where T : class {
                return String.Join(itemDelimiter, tSelector.Select(selector => selector(t)));
            }

            public static String AsExDelimitedString<T>(IEnumerable<T> tList, String itemDelimiter, String listDelimiter, params Func<T, String>[] tSelector) where T : class {
                IEnumerable<String> delimitedStringList = tList.Select(t => NameValueItemExtensions.AsExDelimitedString(t, itemDelimiter, tSelector));
                String delimitedString = String.Join(listDelimiter, delimitedStringList);

                return delimitedString;
            }
        }

        #endregion
    }
}
