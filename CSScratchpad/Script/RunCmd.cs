using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Scratch;

namespace CSScratchpad.Script {
    public class RunCmd : Common, IRunnable {
        public void Run() {
            //IList<Func<String, String>> cleaners = new List<Func<String, String>> {
            //    CleanUpOutput1,
            //    CleanUpOutput2
            //};
            //String output = RunCommandLineExe("", cleaners);
            //Dbg("Output", output);
        }

        //public String CleanUpOutput2(String originalOutput) {
        //    return originalOutput;
        //}

        //public String CleanUpOutput1(String originalOutput) {
        //    return originalOutput;
        //}

        //String RunCommandLineExe(String path, IList<Func<String, String>> outputCleaners) {
        //    //ProcessStartInfo processStartInfo = null;
        //    Process process = null;
        //    StreamWriter consoleWriter = null;
        //    StreamReader consoleReader = null;
        //    String output = String.Empty;

        //    try {
        //        process = new Process {
        //            StartInfo = new ProcessStartInfo("cmd") {
        //                UseShellExecute = false,
        //                ErrorDialog = false,
        //                CreateNoWindow = true,
        //                RedirectStandardError = true,
        //                RedirectStandardInput = true,
        //                RedirectStandardOutput = true
        //            }
        //        };
        //        process.Start();

        //        consoleWriter = process.StandardInput;
        //        consoleReader = process.StandardOutput;

        //        consoleWriter.WriteLine("@echo on");
        //        consoleWriter.WriteLine(MARS_CMD);
        //        consoleWriter.WriteLine("exit");

        //        String MARSOutput = consoleReader.ReadToEnd();
        //        Int32 startIdx = MARSOutput.LastIndexOf("[");
        //        Int32 endIdx = MARSOutput.LastIndexOf("]");
        //        if (MARSOutput.ToLower().Contains("error") || MARSOutput.ToLower().Contains("exception"))
        //            throw new Exception(MARSOutput);
        //        else {
        //            output = MARSOutput.Substring(startIdx, endIdx - startIdx + 1);
        //            var outp = output
        //                .Replace("[", "")
        //                .Replace("]", "")
        //                .Replace("'", "")
        //                .Split(',')
        //                .Select(o => o.Trim());

        //            Console.WriteLine(outp);
        //        }
        //    }
        //    catch (Exception ex) {
        //        Console.WriteLine(ex.Message);
        //    }
        //    finally {
        //        if (consoleWriter != null)
        //            consoleWriter.Close();
        //        if (consoleReader != null)
        //            consoleReader.Close();
        //    }

        //    return output;
        //}
    }
}
