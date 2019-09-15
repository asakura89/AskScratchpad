using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Web.Security;

namespace CSScratchpad.Script {
    class ChangeAspMembershipPassword : Common, IRunnable {
        public void Run() {
            String connectionString = "Data Source=.;Initial Catalog=App_Dev;User ID=sa;Password=AppDevDefault";
            String appName = "AppDev";
            SqlMembershipProvider provider = InitializeAndGetAspMembershipConfig(connectionString, appName);

            String username = "admin";
            MembershipUser user = provider.GetUser(username, false);

            String changedPwd = "#g%D!HLFKs9m";
            // String reset = user.ResetPassword(); // => error
            // Boolean changed = user.ChangePassword(reset, changedPwd); // => error

            String reset = provider.ResetPassword(username, null);
            Boolean changed = provider.ChangePassword(username, reset, changedPwd);

            Dbg(
                new {
                    User = user,
                    ResetPwd = reset,
                    ChangedPwd = changedPwd,
                    Changed = changed
                }
            );
        }

        SqlMembershipProvider InitializeAndGetAspMembershipConfig(String connectionString, String appName) {
            // https://stackoverflow.com/questions/3021877/how-to-read-system-web-section-from-web-config
            // https://stackoverflow.com/questions/1026079/how-can-i-configure-asp-net-membership-providers-through-code
            // https://stackoverflow.com/questions/357465/can-i-add-connectionstrings-to-the-connectionstringcollection-at-runtime

            typeof(ConfigurationElementCollection)
                .GetField("bReadOnly", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(ConfigurationManager.ConnectionStrings, false);

            ConfigurationManager.ConnectionStrings.Add(new ConnectionStringSettings("DefaultConnection", connectionString, "System.Data.SqlClient"));

            var membershipProv = new SqlMembershipProvider();
            membershipProv.Initialize("AspNetSqlMembershipProvider", new NameValueCollection {
                ["connectionStringName"] = "DefaultConnection",
                ["applicationName"] = appName,
                ["enablePasswordRetrieval"] = "false",
                ["enablePasswordReset"] = "true",
                ["requiresQuestionAndAnswer"] = "false",
                ["requiresUniqueEmail"] = "true",
                ["minRequiredNonalphanumericCharacters"] = "0",
                ["minRequiredPasswordLength"] = "12",
                ["maxInvalidPasswordAttempts"] = "10",
                ["passwordStrengthRegularExpression"] = "(?:[A-Z][a-z0-9\\W_])|(?:[a-z][A-Z0-9\\W_])|(?:[0-9][A-Za-z\\W_])|(?:[\\W_][A-Za-z0-9])",
                ["passwordFormat"] = "Hashed"
            });

            return membershipProv;
        }
    }
}
