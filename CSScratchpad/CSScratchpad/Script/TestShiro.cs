using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using Scratch;

namespace CSScratchpad.Script {
    class TestShiro : Common, IRunnable {
        public void Run() {
            String log4netConfigString = @"
            <log4net>
              <root>
                <level value=""DEBUG"" />
                <appender-ref ref=""ColoredConsoleAppender"" />
              </root>
              <appender name=""ColoredConsoleAppender"" type=""log4net.Appender.ColoredConsoleAppender"">
                <mapping>
                  <level value=""INFO"" />
                  <forecolor value=""White"" />
                </mapping>
                <mapping>
                  <level value=""WARN"" />
                  <forecolor value=""Yellow"" />
                </mapping>
                <mapping>
                  <level value=""ERROR"" />
                  <forecolor value=""Red"" />
                </mapping>
                <mapping>
                  <level value=""FATAL"" />
                  <forecolor value=""White"" />
                  <backColor value=""Red, HighIntensity"" />
                </mapping>
                <mapping>
                  <level value=""DEBUG"" />
                  <forecolor value=""Green"" />
                </mapping>
                <layout type=""log4net.Layout.PatternLayout"">
                  <conversionPattern value=""%-4t %d{ABSOLUTE} %-5p [%c] %m%n"" />
                </layout>
              </appender>
            </log4net>
            ";

            using (var stream = new MemoryStream()) {
                using (var writer = new StreamWriter(stream)) {
                    writer.Write(log4netConfigString);
                    writer.Flush();

                    stream.Position = 0;
                    XmlConfigurator.Configure(stream);
                }
            }

            String caller = this.GetFormattedCallerInfoString();
            LogExt.Info(caller, "Doing log.");
            LogExt.Debug(caller, "Log4Net config:");
            LogExt.Debug(caller, log4netConfigString);
            try {
                LogExt.Info(caller, "Read the network.");
                NetworkReader();
                LogExt.Info(caller, "Read the network done.");
            }
            catch (Exception ex) {
                LogExt.Error(caller, ex);
            }

            try {
                LogExt.Info(caller, "Process the data.");
                FakeProcessingMethod();
                LogExt.Info(caller, "Process the data done.");
            }
            catch (Exception ex) {
                LogExt.Error(caller, ex);
            }

            LogExt.Info(caller, "Doing log done.");
        }

        void NetworkReader() => throw new IOException("Can't read from network.");

        String FakeDataGetter() {
            NetworkReader();
            return "Data from Network.";
        }

        void FakeProcessingMethod() {
            try {
                String caller = this.GetFormattedCallerInfoString();
                LogExt.Info(caller, "Starting Processing.");

                LogExt.Debug(caller, "Calling Network.");
                String data = FakeDataGetter();
                LogExt.Debug(caller, data);


                LogExt.Info(caller, "Finishing Processing.");
            }
            catch (Exception ex) {
                throw new InvalidOperationException("Failed get data because of I/O problem.", ex);
            }
        }
    }

    #region :: Shiro ::

    sealed class LogCallerInfo {
        public String CallerType { get; set; }
        public String CallerMember { get; set; }
    }

    static class LogExt {
        const String EmptyMessageReplacer = "-";
        static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void UseExternalLogger(ILog externalLogger) => logger = externalLogger;

        public static void Debug<T>(String caller, T obj) => Debug(caller, JsonConvert.SerializeObject(obj, Formatting.Indented));

        public static void Debug(String caller, String message = EmptyMessageReplacer) => logger.Debug($"[{caller}] {message}");

        public static void Info<T>(String caller, T obj) => Info(caller, JsonConvert.SerializeObject(obj, Formatting.Indented));

        public static void Info(String caller, String message = EmptyMessageReplacer) => logger.Info($"[{caller}] {message}");

        public static void Warn<T>(String caller, T obj) => Warn(caller, JsonConvert.SerializeObject(obj, Formatting.Indented));

        public static void Warn(String caller, String message = EmptyMessageReplacer) => logger.Warn($"[{caller}] {message}");

        public static void Error<T>(String caller, T obj) => Error(caller, JsonConvert.SerializeObject(obj, Formatting.Indented));

        public static void Error(String caller, Exception ex) => Error(caller, ex.GetExceptionMessage());

        public static void Error(String caller, String message = EmptyMessageReplacer) => logger.Error($"[{caller}] {message}");

        // NOTE: One is not encouraged to fill `memberName` parameter as it'll be injected by the compiler service
        public static LogCallerInfo GetCallerInfo<T>(this T caller, [System.Runtime.CompilerServices.CallerMemberName] String memberName = "") =>
            new LogCallerInfo {
                CallerType = caller.GetType().FullName,
                CallerMember = memberName
            };

        public static String GetFormattedCallerInfoString<T>(this T caller, [System.Runtime.CompilerServices.CallerMemberName] String memberName = "") =>
            $"{caller.GetType().FullName}.{memberName}";
    }

    #endregion
}
