using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Scratch;

namespace CSScratchpad.Script {
    public class RunCmd : Common, IRunnable {
        public void Run() {
            try {
                var cmdApp = new CommandLine("git", String.Empty);
                IList<String> result = cmdApp.Execute(String.Empty, new CommandLineArgument("--version"));
                Console.WriteLine(String.Join(Environment.NewLine, result.Select(line => line.Trim())));

                Console.WriteLine();
                Console.WriteLine("-------------------------------------");
                Console.WriteLine();

                Console.WriteLine(Environment.CurrentDirectory);

                Console.WriteLine();
                Console.WriteLine("-------------------------------------");
                Console.WriteLine();

                String tempGitFolder = @"D:\temp\AspNetMembershipPasswordReset";

                cmdApp = new CommandLine("git", tempGitFolder);
                result = cmdApp.Execute("log", new CommandLineArgument("-n", "5"));
                Console.WriteLine(String.Join(Environment.NewLine, result.Select(line => line.Trim())));

                Console.WriteLine();
                Console.WriteLine("-------------------------------------");
                Console.WriteLine();

                Environment.CurrentDirectory = tempGitFolder;
                cmdApp = new CommandLine("git", String.Empty);
                result = cmdApp.Execute("log", new CommandLineArgument("-n", "5"), new CommandLineArgument("--pretty=oneline"));
                Console.WriteLine(String.Join(Environment.NewLine, result.Select(line => line.Trim())));
            }
            catch (Exception ex) {
                Console.WriteLine(ex.GetExceptionMessage());
            }
        }

        #region :: CommandLine Runner ::

        public class CommandLineArgument : NameValueItem {
            public CommandLineArgument(String name) : this(name, String.Empty) { }

            public CommandLineArgument(String name, String value) : base(name, value) { }

            public override String ToString() {
                String name = Name.Trim();
                String value = Value.Trim();

                if (String.IsNullOrEmpty(value))
                    return name;

                return $"{name} {value}";
            }
        }

        public class CommandLine {
            readonly String cmdApp;
            readonly String workingDir;

            public CommandLine(String cmdApp, String workingDir) {
                this.cmdApp = cmdApp ?? throw new ArgumentNullException(nameof(cmdApp));
                this.workingDir = workingDir ?? throw new ArgumentNullException(nameof(workingDir));
            }

            public IList<String> Execute(String command, params CommandLineArgument[] args) {
                StreamWriter consoleWriter = null;
                StreamReader consoleReader = null;

                try {
                    String processArgs = BuildProcessArgs(command, args);
                    var process = new Process {
                        StartInfo = new ProcessStartInfo(cmdApp) {
                            UseShellExecute = false,
                            ErrorDialog = false,
                            CreateNoWindow = true,
                            RedirectStandardError = true,
                            RedirectStandardInput = true,
                            RedirectStandardOutput = true,
                            StandardOutputEncoding = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.OEMCodePage),
                            StandardErrorEncoding = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.OEMCodePage),

                            WorkingDirectory = workingDir,
                            Arguments = processArgs
                        },
                        EnableRaisingEvents = true
                    };
                    process.Start();

                    consoleWriter = process.StandardInput;
                    consoleReader = process.StandardOutput;
                    String output = consoleReader.ReadToEnd();

                    process.Dispose();

                    return output
                        .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                        .ToList();
                }
                finally {
                    consoleWriter?.Close();
                    consoleReader?.Close();
                }
            }

            String BuildProcessArgs(String command, IEnumerable<CommandLineArgument> args) {
                String cmdAppCommand = command.Trim();
                if (!String.IsNullOrEmpty(cmdAppCommand)) {
                    ActionResponseViewModel response = ValidateCommand(cmdAppCommand);
                    if (response.ResponseType == ActionResponseViewModel.Error)
                        throw new InvalidOperationException(response.Message);

                    return cmdAppCommand + " " + BuildCmdAppCommandArgs(args).Trim();
                }

                return BuildCmdAppCommandArgs(args).Trim();
            }

            String BuildCmdAppCommandArgs(IEnumerable<CommandLineArgument> args) {
                var validated = new Validated<CommandLineArgument> {
                    Messages = args
                        .Select(ValidateCommandArgument)
                        .ToList()
                };

                if (validated.ContainsFail) {
                    var msgBuilder = new StringBuilder()
                        .AppendLine("Please correct the command arguments below:")
                        .AppendLine();

                    foreach (ActionResponseViewModel response in validated.Messages)
                        msgBuilder.AppendLine(response.Message);

                    throw new InvalidOperationException(msgBuilder.ToString());
                }

                return String.Join(" ", args.Select(arg => arg.ToString()));
            }

            readonly Regex CmdAppCommandValidator = new Regex(@"^[a-zA-Z0-9\._\-]+$", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            ActionResponseViewModel ValidateCommand(String command) {
                Boolean valid = CmdAppCommandValidator.IsMatch(command);
                return new ActionResponseViewModel {
                    Message = valid ? String.Empty : $"{command} is not valid command.",
                    ResponseType = valid ? ActionResponseViewModel.Success : ActionResponseViewModel.Error
                };
            }

            /*
            -u
            -U
            --use-database
            --Use-Database
            :usedatabase
            :UseDatabase
            useDatabase:
            :--cancel
            -cancel
            --cancel
            anomalydata
            AnomalyData
            --multi-valued:a,b,c,d
            --single-value:a
            --multi-valued=a,b,c,d
            --single-value=a
            */
            readonly Regex CmdAppCommandArgumentValidator = new Regex(@"^\-{1,2}[a-zA-Z0-9\._\-:=,]+$", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            ActionResponseViewModel ValidateCommandArgument(CommandLineArgument arg) {
                Boolean valid = CmdAppCommandArgumentValidator.IsMatch(arg.Name);
                return new ActionResponseViewModel {
                    Message = valid ? String.Empty : $"{arg.Name} is not valid command argument.",
                    ResponseType = valid ? ActionResponseViewModel.Success : ActionResponseViewModel.Error
                };
            }
        }

        #endregion

        #region :: References ::

        public class Item<N, V>
            where N : class
            where V : class {

            protected N InternalName { get; set; }
            protected V InternalValue { get; set; }
        }

        [Serializable]
        public class NameValueItem : Item<String, String> {
            public const String NameProperty = "Name";
            public const String ValueProperty = "Value";
            public const Char ListDelimiter = '·'; // ALT+250
            public const Char ItemDelimiter = '•'; // ALT+7

            public String Name {
                get {
                    return InternalName;
                }

                private set {
                    InternalName = value;
                }
            }

            public String Value {
                get {
                    return InternalValue;
                }

                private set {
                    InternalValue = value;
                }
            }

            public static NameValueItem Empty => new NameValueItem();

            public NameValueItem(String name, String value) {
                InternalName = name;
                InternalValue = value;
            }

            public NameValueItem() : this(String.Empty, String.Empty) { }
        }

        [Serializable]
        public class ActionResponseViewModel {
            public const String Info = "I";
            public const String Warning = "W";
            public const String Error = "E";
            public const String Success = "S";
            public String ResponseType { get; set; }
            public String Message { get; set; }

            public override String ToString() => ToString(true);

            public String ToString(Boolean alwaysReturn) {
                if (!alwaysReturn && ResponseType == Error)
                    throw new InvalidOperationException(Message);

                return ResponseType + "|" + Message;
            }
        }

        public class Validated<TData> {
            public IList<ActionResponseViewModel> Messages { get; set; } = new List<ActionResponseViewModel>();
            public IList<TData> ValidData { get; set; } = new List<TData>();
            public Boolean ContainsFail => Messages.Any(message => message.ResponseType == ActionResponseViewModel.Error);
            public Boolean AllFails => Messages.All(message => message.ResponseType == ActionResponseViewModel.Error);
        }

        #endregion
    }
}
