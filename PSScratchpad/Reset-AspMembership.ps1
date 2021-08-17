param([System.String]$mode, [System.String]$connString, [System.String]$appName, [System.String]$hash, [System.String]$username, [System.String]$password, [System.String]$email, [System.String]$role)

Clear-Host

function Log($message, $starting = $false, $writeToScreen = $true) {
    $scriptfile = (Get-Item $PSCommandPath)
    $logdir = $scriptfile.Directory
    $logname = "$($scriptfile.BaseName)_$([System.DateTime]::Now.ToString("yyyyMMddHHmm")).log"
    $logfile = [System.IO.Path]::Combine($logdir, $logname)

    $logmsg = "[$([System.DateTime]::Now.ToString("yyyy.MM.dd.HH:mm:ss"))] $($message)"
    If ($writeToScreen) {
        Write-Host $logmsg
    }

    If ($starting) {
        $logmsg | Out-File -Encoding "UTF8" -FilePath $logfile
    }
    Else {
        $logmsg | Add-Content -Encoding "UTF8" -Path $logfile
    }
}

function GetExceptionMessage([System.Exception]$ex) {
    $errorList = New-Object System.Text.StringBuilder
    [System.Exception]$current = $ex;
    While ($current -Ne $null) {
        [void]$errorList.AppendLine()
        [void]$errorList.AppendLine("Exception: $($current.GetType().FullName)")
        [void]$errorList.AppendLine("Message: $($current.Message)")
        [void]$errorList.AppendLine("Source: $($current.Source)")
        [void]$errorList.AppendLine($current.StackTrace)
        [void]$errorList.AppendLine()

        $current = $current.InnerException
    }

    Return $errorList.ToString()
}

$source = @"
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace Databossy
{
    public interface IDatabaseFactory
    {
        IDatabase CreateSession();
        IDatabase CreateSession(String connectionStringOrName, Boolean useConnectionString = false, String provider = Database.SqlServerProvider);
    }

    public class DatabaseFactory : IDatabaseFactory
    {
        public IDatabase CreateSession()
        {
            return CreateSession(ConfigurationManager.ConnectionStrings[0].Name);
        }

        public IDatabase CreateSession(String connectionStringOrName, Boolean useConnectionString = false, String provider = Database.SqlServerProvider)
        {
            return new Database(connectionStringOrName, useConnectionString, provider);
        }
    }

    public interface IDatabase : IDisposable
    {
        String ContextName { get; set; }
        String ConnectionString { get; set; }
        String Provider { get; set; }

        DataTable QueryDataTable(String queryString);
        DataTable QueryDataTable(String queryString, params Object[] queryParams);
        DataTable NQueryDataTable(String queryString, Object paramObj);
        DataSet QueryDataSet(String queryString);
        DataSet QueryDataSet(String queryString, params Object[] queryParams);
        DataSet NQueryDataSet(String queryString, Object paramObj);
        IEnumerable<T> Query<T>(String queryString);
        IEnumerable<T> Query<T>(String queryString, params Object[] queryParams);
        IEnumerable<T> NQuery<T>(String queryString, Object paramObj);
        T QuerySingle<T>(String queryString);
        T QuerySingle<T>(String queryString, params Object[] queryParams);
        T NQuerySingle<T>(String queryString, Object paramObj);
        T QueryScalar<T>(String queryString);
        T QueryScalar<T>(String queryString, params Object[] queryParams);
        T NQueryScalar<T, TParam>(String queryString, TParam paramObj);
        Int32 Execute(String queryString);
        Int32 Execute(String queryString, params Object[] queryParams);
        Int32 NExecute(String queryString, Object paramObj);
        Int32 WithTransaction(Func<Database, Int32> doThisWithTrx);
    }

    public class Database : IDatabase
    {
        internal const String SqlServerProvider = "System.Data.SqlClient";
        private DbConnection connection;
        private DbProviderFactory factory;

        public String ContextName { get; set; }
        public String ConnectionString { get; set; }
        public String Provider { get; set; }

        public Database() : this(ConfigurationManager.ConnectionStrings[0].Name) { }

        public Database(String connectionStringOrName, Boolean useConnectionString = false, String provider = SqlServerProvider)
        {
            Provider = provider;
            if (useConnectionString)
                ConnectionString = connectionStringOrName;
            else
            {
                String connString = ConfigurationManager.ConnectionStrings[connectionStringOrName].ConnectionString;
                ConnectionString = connString;
                ContextName = connectionStringOrName;
            }
        }

        private void Open()
        {
            factory = DbProviderFactories.GetFactory(Provider);
            connection = factory.CreateConnection();
            if (connection == null)
                throw new Exception("Connection creation from factory failed.");

            connection.ConnectionString = ConnectionString;
            connection.Open();
        }

        private DbCommand BuildSqlCommand(String queryString)
        {
            DbCommand builtSqlCommand = connection.CreateCommand();
            builtSqlCommand.CommandText = queryString;
            builtSqlCommand.CommandType = CommandType.Text;

            return builtSqlCommand;
        }

        private DbCommand BuildSqlCommand(String queryString, Object[] queryParams)
        {
            DbCommand builtSqlCommand = connection.CreateCommand();
            builtSqlCommand.CommandText = queryString;
            builtSqlCommand.CommandType = CommandType.Text;
            BuildSqlCommandParameter(ref builtSqlCommand, queryParams);

            return builtSqlCommand;
        }

        private DbCommand NBuildSqlCommand<TParam>(String queryString, TParam paramObj)
        {
            DbCommand builtSqlCommand = connection.CreateCommand();
            builtSqlCommand.CommandText = queryString;
            builtSqlCommand.CommandType = CommandType.Text;
            NBuildSqlCommandParameter(ref builtSqlCommand, paramObj);

            return builtSqlCommand;
        }

        private void BuildSqlCommandParameter(ref DbCommand builtSqlCommand, Object[] queryParams)
        {
            builtSqlCommand.Parameters.Clear();
            for (Int32 paramIdx = 0; paramIdx < queryParams.Length; paramIdx++)
            {
                Object currentqueryParams = queryParams[paramIdx] ?? DBNull.Value;
                DbParameter param = builtSqlCommand.CreateParameter();
                param.ParameterName = paramIdx.ToString();
                param.Value = currentqueryParams;
                builtSqlCommand.Parameters.Add(param);
            }
        }

        private void NBuildSqlCommandParameter<TParam>(ref DbCommand builtSqlCommand, TParam paramObj)
        {
            PropertyInfo[] properties = ValidateAndGetNParam(builtSqlCommand.CommandText, paramObj);

            builtSqlCommand.Parameters.Clear();
            foreach (PropertyInfo currentProperty in properties)
            {
                Object currentParam = currentProperty.GetValue(paramObj, null) ?? DBNull.Value;
                DbParameter param = builtSqlCommand.CreateParameter();
                param.ParameterName = currentProperty.Name;
                param.Value = currentParam;
                builtSqlCommand.Parameters.Add(param);
            }
        }

        private PropertyInfo[] ValidateAndGetNParam(String queryString, Object paramObj)
        {
            var queryParamRgx = new Regex("(?<!@)@\\w+", RegexOptions.Compiled);
            MatchCollection definedParams = queryParamRgx.Matches(queryString);
            PropertyInfo[] properties = paramObj.GetType().GetProperties();
            foreach (Match param in definedParams)
            {
                String closureParam = param.Value.Replace("@", "");
                PropertyInfo foundProperty = properties.FirstOrDefault(p => p.Name == closureParam);
                if (foundProperty == null)
                    throw new InvalidOperationException("Sql Param \"" + closureParam + "\" is defined but value isn't supplied.");
            }

            return properties;
        }

        private DbDataAdapter BuildSelectDataAdapter(DbCommand builtSqlCommand)
        {
            DbDataAdapter builtDataAdapter = factory.CreateDataAdapter();
            if (builtDataAdapter == null)
                throw new Exception("Data Adapter creation from factory failed.");

            builtDataAdapter.SelectCommand = builtSqlCommand;
            return builtDataAdapter;
        }

        public DataTable QueryDataTable(String queryString)
        {
            Open();
            var dt = new DataTable();
            using (DbCommand cmd = BuildSqlCommand(queryString))
            {
                using (DbDataAdapter dataAdapter = BuildSelectDataAdapter(cmd))
                    dataAdapter.Fill(dt);
            }

            return dt;
        }

        public DataTable QueryDataTable(String queryString, params Object[] queryParams)
        {
            Open();
            var dt = new DataTable();
            using (DbCommand cmd = BuildSqlCommand(queryString, queryParams))
            {
                using (DbDataAdapter dataAdapter = BuildSelectDataAdapter(cmd))
                    dataAdapter.Fill(dt);
            }

            return dt;
        }

        public DataTable NQueryDataTable(String queryString, Object paramObj)
        {
            Open();
            var dt = new DataTable();
            using (DbCommand cmd = NBuildSqlCommand(queryString, paramObj))
            {
                using (DbDataAdapter dataAdapter = BuildSelectDataAdapter(cmd))
                    dataAdapter.Fill(dt);
            }

            return dt;
        }

        public DataSet QueryDataSet(String queryString)
        {
            Open();
            var ds = new DataSet();
            using (DbCommand cmd = BuildSqlCommand(queryString))
            {
                using (DbDataAdapter dataAdapter = BuildSelectDataAdapter(cmd))
                    dataAdapter.Fill(ds);
            }

            return ds;
        }

        public DataSet QueryDataSet(String queryString, params Object[] queryParams)
        {
            Open();
            var ds = new DataSet();
            using (DbCommand cmd = BuildSqlCommand(queryString, queryParams))
            {
                using (DbDataAdapter dataAdapter = BuildSelectDataAdapter(cmd))
                    dataAdapter.Fill(ds);
            }

            return ds;
        }

        public DataSet NQueryDataSet(String queryString, Object paramObj)
        {
            Open();
            var ds = new DataSet();
            using (DbCommand cmd = NBuildSqlCommand(queryString, paramObj))
            {
                using (DbDataAdapter dataAdapter = BuildSelectDataAdapter(cmd))
                    dataAdapter.Fill(ds);
            }

            return ds;
        }

        // NOTE: must be materialized to List
        // --> https://softwareengineering.stackexchange.com/questions/300242/will-the-database-connection-be-closed-if-we-yield-the-datareader-row-and-not-re
        public IEnumerable<T> Query<T>(String queryString)
        {
            Open();
            var result = new List<T>();
            using (DbCommand cmd = BuildSqlCommand(queryString))
                using (DbDataReader reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(ToTResult<T>(reader));

            return result;
        }

        public IEnumerable<T> Query<T>(String queryString, params Object[] queryParams)
        {
            Open();
            var result = new List<T>();
            using (DbCommand cmd = BuildSqlCommand(queryString, queryParams))
                using (DbDataReader reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(ToTResult<T>(reader));

            return result;
        }

        public IEnumerable<T> NQuery<T>(String queryString, Object paramObj)
        {
            Open();
            var result = new List<T>();
            using (DbCommand cmd = NBuildSqlCommand(queryString, paramObj))
                using (DbDataReader reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(ToTResult<T>(reader));

            return result;
        }

        public T QuerySingle<T>(String queryString)
        {
            T result = Query<T>(queryString).FirstOrDefault();

            return result;
        }

        public T QuerySingle<T>(String queryString, params Object[] queryParams)
        {
            T result = Query<T>(queryString, queryParams).FirstOrDefault();

            return result;
        }

        public T NQuerySingle<T>(String queryString, Object paramObj)
        {
            T result = NQuery<T>(queryString, paramObj).FirstOrDefault();

            return result;
        }

        public T QueryScalar<T>(String queryString)
        {
            Open();
            using (DbCommand cmd = BuildSqlCommand(queryString))
                return (T)cmd.ExecuteScalar();
        }

        public T QueryScalar<T>(String queryString, params Object[] queryParams)
        {
            Open();
            using (DbCommand cmd = BuildSqlCommand(queryString, queryParams))
                return (T)cmd.ExecuteScalar();
        }

        public T NQueryScalar<T, TParam>(String queryString, TParam paramObj)
        {
            Open();
            using (DbCommand cmd = NBuildSqlCommand(queryString, paramObj))
                return (T)cmd.ExecuteScalar();
        }

        public Int32 Execute(String queryString)
        {
            Open();
            Int32 result = -1;
            using (DbCommand cmd = BuildSqlCommand(queryString))
                result = cmd.ExecuteNonQuery();

            return result;
        }

        public Int32 Execute(String queryString, params Object[] queryParams)
        {
            Open();
            Int32 result = -1;
            using (DbCommand cmd = BuildSqlCommand(queryString, queryParams))
                result = cmd.ExecuteNonQuery();

            return result;
        }

        public Int32 NExecute(String queryString, Object paramObj)
        {
            Open();
            Int32 result = -1;
            using (DbCommand cmd = NBuildSqlCommand(queryString, paramObj))
                result = cmd.ExecuteNonQuery();

            return result;
        }

        public Int32 WithTransaction(Func<Database, Int32> doThisWithTrx)
        {
            Open();
            Int32 result = -1;
            using (var scope = CreateTransactionScope())
            {
                result = doThisWithTrx(this);
                scope.Complete();
            }

            return result;
        }

        // NOTE: https://blogs.msdn.microsoft.com/dbrowne/2010/06/03/using-new-transactionscope-considered-harmful/
        private TransactionScope CreateTransactionScope()
        {
            var options = new TransactionOptions();
            options.IsolationLevel = IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.MaximumTimeout;
            return new TransactionScope(TransactionScopeOption.Required, options);
        }

        public void Dispose()
        {
            if (connection != null)
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();

                connection.Dispose();
                connection = null;
            }

            GC.SuppressFinalize(this);
        }

        private TResult ToTResult<TResult>(IDataRecord record)
        {
            var t = Activator.CreateInstance<TResult>();
            Type tType = typeof(TResult);
            PropertyInfo[] tProperties = tType.GetProperties();
            FieldInfo[] tFields = tType.GetFields();

            if (tProperties.Length != 0)
            {
                foreach (PropertyInfo property in tProperties)
                {
                    Object result = record[record.GetOrdinal(property.Name)];
                    if (result != DBNull.Value)
                        property.SetValue(t, Convert.ChangeType(result, property.PropertyType), null);
                }
            }
            else
            {
                foreach (FieldInfo field in tFields)
                {
                    Object result = record[record.GetOrdinal(field.Name)];
                    if (result != DBNull.Value)
                        field.SetValue(t, Convert.ChangeType(result, field.FieldType));
                }
            }

            return t;
        }
    }
}
"@

$refs = (
    "System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
    "System.Configuration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
    "System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
    "System.Xml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
    "System.Transactions"
)

Add-Type -ReferencedAssemblies $refs -TypeDefinition $source -Language CSharp

Add-Type -Assembly System.Web
Add-Type -Assembly System.Configuration

function InitializeAndGetAspMembershipConfig($connString, $appName, $hashAlgo) {
    $configElementColl = [System.Type]([System.Configuration.ConfigurationElementCollection])
    $readOnly = $configElementColl.GetField("bReadOnly", ([System.Reflection.BindingFlags]::Instance -bOr [System.Reflection.BindingFlags]::NonPublic))
    $readOnly.SetValue([System.Configuration.ConfigurationManager]::ConnectionStrings, $false)
    
    [System.Configuration.ConfigurationManager]::ConnectionStrings.Add($(New-Object System.Configuration.ConnectionStringSettings("DefaultConnection", $connString, "System.Data.SqlClient")))

    $providerConfig = New-Object System.Collections.Specialized.NameValueCollection
    $providerConfig.Add("connectionStringName", "DefaultConnection")
    $providerConfig.Add("applicationName", $appName)
    $providerConfig.Add("enablePasswordRetrieval", "false")
    $providerConfig.Add("enablePasswordReset", "true")
    $providerConfig.Add("requiresQuestionAndAnswer", "false")
    $providerConfig.Add("requiresUniqueEmail", "true")
    $providerConfig.Add("minRequiredNonalphanumericCharacters", "0")
    $providerConfig.Add("minRequiredPasswordLength", "1")
    $providerConfig.Add("maxInvalidPasswordAttempts", "10")
    $providerConfig.Add("passwordStrengthRegularExpression", ".+")
    $providerConfig.Add("passwordFormat", "Hashed")

    $provider = New-Object System.Web.Security.SqlMembershipProvider
    $provider.Initialize("AspNetSqlMembershipProvider", $providerConfig)

    Return $provider
}

function ResetAction($connString, $appName, $hash, $username, $pasword) {
    [System.Web.Security.SqlMembershipProvider]$provider = InitializeAndGetAspMembershipConfig
    [MembershipUser]$user = $provider.GetUser($username, $false)
    If ($user -Eq $null) {
        Throw [System.InvalidOperationException] "User not found."
    }

    Log "User '$($username)' found."

    $reset = $provider.ResetPassword($username, $null);
    $provider.ChangePassword($username, $reset, $password);
    
    #UpdateUserLoginProperty(connString, username);
}

function CreateAction($connString, $appName, $hash, $username, $pasword, $email, $role) {
    [System.Web.Security.SqlMembershipProvider]$provider = InitializeAndGetAspMembershipConfig

}

function DeleteAction($connString, $appName, $hash, $username) {
    [System.Web.Security.SqlMembershipProvider]$provider = InitializeAndGetAspMembershipConfig

}

function ViewAction($connString, $appName, $username) {
    $db = New-Object Databossy.Database($connString, $true)
    $dt = $db.QueryDataTable(@"
        SET NOCOUNT ON
        ;
        WITH AspApp AS (
            SELECT
            ApplicationId [Id],
            ApplicationName [Name],
            [Description] [Desc]
            FROM aspnet_Applications
        ),
        AspUser AS (
            SELECT
            ApplicationId AppId,
            UserId [Id],
            UserName Username,
            LastActivityDate LastActivity
            FROM aspnet_Users
        ),
        AspMembership AS (
            SELECT
            ApplicationId AppId,
            UserId,
            Email,
            IsApproved Approved,
            IsLockedOut LockedOut,
            LastLoginDate LastLogin,
            LastPasswordChangedDate LastPwdChanged,
            LastLockoutDate LastLockedOut,
            FailedPasswordAttemptCount FailedLoginCount,
            FailedPasswordAnswerAttemptCount FailedPwdAnswerCount
            FROM aspnet_Membership mbr
        ),
        AspRole AS (
            SELECT
            r.ApplicationId AppId,
            usr.UserId,
            us.Username,
            r.RoleName [Role],
            r.[Description] [Desc]
            FROM aspnet_UsersInRoles usr
            LEFT JOIN AspUser us
            ON usr.UserId = us.[Id]
            LEFT JOIN aspnet_Roles r
            ON usr.RoleId = r.RoleId
            AND r.ApplicationId = us.AppId
        ),
        AspProfile AS (
            SELECT
            us.[Id] UserId,
            prf.PropertyNames,
            prf.PropertyValuesString,
            prf.PropertyValuesBinary
            FROM aspnet_Profile prf
            LEFT JOIN AspUser us ON prf.UserId = us.[Id]
        ),
        AspProfileNV AS (
            SELECT
            UserId,
            ':' + CAST(PropertyNames AS VARCHAR(8000)) Names,
            PropertyValuesString [Values]
            FROM AspProfile
        )
        SELECT
        app.[Name] App,
        app.[Desc] AppDesc,
        us.Username,
        CONVERT(VARCHAR, us.LastActivity, 104) + ' ' + CONVERT(VARCHAR, us.LastActivity, 108) LastActivity,
        mbr.Email,
        CASE mbr.Approved
            WHEN 0 THEN 'False'
            WHEN 1 THEN 'True'
        END Approved,
        CASE mbr.LockedOut
            WHEN 0 THEN 'False'
            WHEN 1 THEN 'True'
        END LockedOut,
        CONVERT(VARCHAR, mbr.LastLogin, 104) + ' ' + CONVERT(VARCHAR, mbr.LastLogin, 108) LastLogin,
        CONVERT(VARCHAR, mbr.LastPwdChanged, 104) + ' ' + CONVERT(VARCHAR, mbr.LastPwdChanged, 108) LastPwdChanged,
        CONVERT(VARCHAR, mbr.LastLockedOut, 104) + ' ' + CONVERT(VARCHAR, mbr.LastLockedOut, 108) LastLockedOut,
        mbr.FailedLoginCount,
        mbr.FailedPwdAnswerCount,
        r.[Role],
        r.[Desc] RoleDesc,
        prf.Names ProfileNames,
        prf.[Values] ProfileValues
        FROM AspApp app
        LEFT JOIN AspUser us
        ON app.[Id] = us.AppId
        LEFT JOIN AspMembership mbr
        ON app.[Id] = mbr.AppId
        AND us.[Id] = mbr.UserId
        LEFT JOIN AspRole r
        ON us.AppId = r.AppId
        AND us.[Id] = r.UserId
        LEFT JOIN AspProfileNV prf
        ON prf.UserId = us.[Id]
        WHERE app.[Name] = @0
        AND us.Username = @1

        SET NOCOUNT OFF
"@, [System.String]$appName, [System.String]$username)

    $db.Dispose()

    ForEach ($dr In $dt.Rows) {
        Log "  > App: $($dr.App)"
        Log "  > AppDesc: $($dr.AppDesc)"
        Log "  > Username: $($dr.Username)"
        Log "  > LastActivity: $($dr.LastActivity)"
        Log "  > Email: $($dr.Email)"
        Log "  > Approved: $($dr.Approved)"
        Log "  > LockedOut: $($dr.LockedOut)"
        Log "  > LastLogin: $($dr.LastLogin)"
        Log "  > LastPwdChanged: $($dr.LastPwdChanged)"
        Log "  > LastLockedOut: $($dr.LastLockedOut)"
        Log "  > FailedLoginCount: $($dr.FailedLoginCount)"
        Log "  > FailedPwdAnswerCount: $($dr.FailedPwdAnswerCount)"
        Log "  > Role: $($dr.Role)"
        Log "  > RoleDesc: $($dr.RoleDesc)"
        Log "  > ProfileNames: $($dr.ProfileNames)"
        Log "  > ProfileValues: $($dr.ProfileValues)"
    }
}

function ListAction($connString) {
    $db = New-Object Databossy.Database($connString, $true)
    $dt = $db.QueryDataTable(@"
        SET NOCOUNT ON
        ;
        WITH AspApp AS (
            SELECT
            ApplicationId [Id],
            ApplicationName [Name],
            [Description] [Desc]
            FROM aspnet_Applications
        ),
        AspUser AS (
            SELECT
            ApplicationId AppId,
            UserId [Id],
            UserName Username,
            LastActivityDate LastActivity
            FROM aspnet_Users
        )
        SELECT
        app.[Name] App,
        us.Username
        FROM AspApp app
        LEFT JOIN AspUser us
        ON app.[Id] = us.AppId

        SET NOCOUNT OFF
"@)
    $db.Dispose()

    ForEach ($dr In $dt.Rows) {
        Log "  > App: $($dr.App), Username: $($dr.Username)"
    }
}

#function UpdateUserLoginProperty

Try {
    Log "Start." $true
    [System.String[]]$modeActions = @("r", "c", "d", "v", "l")

    If ([System.Linq.Enumerable]::Contains([System.String[]]$modeActions, $mode.ToLowerInvariant())) {
        Switch ($mode) {
            "r" {
                #ResetAction 
                Break
            }

            "c" {
                #CreateAction 
                Break
            }

            "d" {
                #DeleteAction 
                Break
            }

            "v" {
                ViewAction $connString $appName $username
                Break
            }

            "l" {
                ListAction $connString
                Break
            }
            
            Default {
                Log "Choose correct mode."
                Break
            }
        }
    }
    Log "Done."
}
Catch {
    Log "$(GetExceptionMessage $_.Exception)"
}







