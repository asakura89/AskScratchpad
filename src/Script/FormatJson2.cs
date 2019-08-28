using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSScratchpad.Script {
    class FormatJson2 : Common, IRunnable {
        public void Run() => Console.WriteLine(
            JsonConvert.SerializeObject(new DdfConfig {
                MustIncludes = new[] {
                    new MustCheckStructure {
                        Desc = "Config files",
                        Pattern = ".+\\.config"
                    },
                    new MustCheckStructure {
                        Desc = "Dll files",
                        Pattern = ".+\\.dll"
                    }
                },
                MustExcludes = new[] {
                    new MustCheckStructure {
                        Desc = "Unused config files",
                        Pattern = ".+\\.config\\..+"
                    },
                    new MustCheckStructure {
                        Desc = "Source files",
                        Pattern = ".+\\.aspx\\.cs"
                    },
                    new MustCheckStructure {
                        Desc = "Source files 2",
                        Pattern = ".+\\.cs"
                    }
                },
                MustContain = new[] {
                    new MustCheckFile {
                        Desc = "Staging config files",
                        FilenamePattern = ".+\\.config",
                        ContentPattern = "([Dd]ata [Ss]ource\\s?=\\s?172)"
                    }
                },
                MustNotContain = new[] {
                    new MustCheckFile {
                        Desc = "Debug enabled in staging",
                        FilenamePattern = ".+\\.config",
                        ContentPattern = "([Dd]ebug\\s?=\\s?\"true\")"
                    }
                }
            }, Formatting.Indented));

    }

    public sealed class DdfConfig {
        public IList<MustCheckStructure> MustIncludes { get; set; }
        public IList<MustCheckStructure> MustExcludes { get; set; }
        public IList<MustCheckFile> MustContain { get; set; }
        public IList<MustCheckFile> MustNotContain { get; set; }
    }

    public sealed class MustCheckStructure {
        public String Desc { get; set; }
        public String Pattern { get; set; }
    }

    public sealed class MustCheckFile {
        public String Desc { get; set; }
        public String FilenamePattern { get; set; }
        public String ContentPattern { get; set; }
    }
}
