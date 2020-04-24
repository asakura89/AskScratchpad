using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Scratch;

[assembly: AssemblyTitle("CSScratchpad")]
[assembly: AssemblyDescription("CSharp Scratchpad")]
[assembly: AssemblyCompany("asakura89 own's company")]
[assembly: AssemblyProduct("AssemblyVersion_CSScratchpad")]
[assembly: AssemblyVersion("1.0.2017.0111")]
[assembly: AssemblyFileVersion("1.0.2017.0111")]
[assembly: AssemblyInformationalVersion("1.0.2017.0111")]
[assembly: AssemblyCopyright("Copyright © asakura89 own's company 2017")]
[assembly: AssemblyTrademark("No TM yet")]
[assembly: AssemblyConfiguration("Release")]
//[assembly: Guid("a9340416-c17b-4bc1-a623-2ad601db2050")]
//[assembly: ComVisible(false)]
[assembly: CLSCompliant(false)]

namespace CSScratchpad.Script {
    class PrintAssemblyVersion : Common, IRunnable {
        public void Run() {
            DateTime date = DateTime.Now;
            DateTime buildYearMo = new DateTime(date.Year, date.Month, 1);
            DateTime projectYear = new DateTime(2017, 7, 1);

            Dbg("Actual Build Date (fist day of the month)", buildYearMo);
            Dbg("Project Started Date (fist day of the month)", projectYear);

            Int32 major = 5;
            Int32 minor = 0;
            Int32 build = buildYearMo.Year;
            String revisionFirstCombination = (Convert.ToInt32(buildYearMo.Subtract(projectYear).TotalDays / 30) + 1).ToString().PadLeft(2, '0');
            String revisionSecondCombination = date.Day.ToString().PadLeft(2, '0');

            Dbg("Generated Version",
                new StringBuilder()
                    .Append(major).Append(".")
                    .Append(minor).Append(".")
                    .Append(build).Append(".")
                    .Append(revisionFirstCombination).Append(revisionSecondCombination)
                    .ToString()
            );

            Dbg("Build Date constructed from version number", projectYear.AddMonths(Convert.ToInt32(revisionFirstCombination)));
            Dbg("Current App Version", AppVersion);
        }

        static AssemblyVersionAttribute AsmVersion => Assembly
            .GetExecutingAssembly()
            .GetCustomAttributes(typeof(AssemblyVersionAttribute), false)
            .Cast<AssemblyVersionAttribute>()
            .SingleOrDefault();

        static AssemblyFileVersionAttribute AsmFileVersion => Assembly
            .GetExecutingAssembly()
            .GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)
            .Cast<AssemblyFileVersionAttribute>()
            .SingleOrDefault();


        public static String AppVersion =>
            AsmVersion != null ?
                AsmVersion.Version : AsmFileVersion != null ?
                    AsmFileVersion.Version : "1.0.0.0";
    }
}
