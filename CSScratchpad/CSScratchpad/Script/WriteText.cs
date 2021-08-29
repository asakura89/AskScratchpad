using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Scratch;

namespace CSScratchpad.Script {
    [SimpleJob(RuntimeMoniker.Net461, baseline: true)]
    [SimpleJob(RuntimeMoniker.Net472)]
    public class WriteText : Common, IRunnable {
        public void Run() => BenchmarkRunner.Run<WriteText>();

        [GlobalSetup]
        public void Setup() {
            if (!Directory.Exists(OutputDirPath)) {
                Directory.CreateDirectory(OutputDirPath);
            }

            String path = Path.Combine(OutputDirPath, "logtextwriter.log");
            if (!File.Exists(path)) {
                File.Create(path);
            }

            path = Path.Combine(OutputDirPath, "logfilestream.log");
            if (!File.Exists(path)) {
                File.Create(path);
            }

            path = Path.Combine(OutputDirPath, "logfile.log");
            if (!File.Exists(path)) {
                File.Create(path);
            }
        }

        [Benchmark]
        public void RunTextWriter() {
            LogTextWriter("``:: Start Script ::'");
            LogTextWriter("Doing something ...");
            LogTextWriter("Doing another thing ...");
            LogTextWriter(".:: Finish Script ::.");
        }

        Object logTextWriterLock = new Object();
        void LogTextWriter(String message) {
            String path = Path.Combine(OutputDirPath, "logtextwriter.log");
            lock (logTextWriterLock) {
                using (TextWriter tw = File.AppendText(path)) {
                    tw.WriteLine(message);
                }
            }
        }

        [Benchmark]
        public void RunFileStream() {
            LogFileStream("``:: Start Script ::'");
            LogFileStream("Doing something ...");
            LogFileStream("Doing another thing ...");
            LogFileStream(".:: Finish Script ::.");
        }

        Object logFileStreamLock = new Object();
        void LogFileStream(String message) {
            String path = Path.Combine(OutputDirPath, "logfilestream.log");
            lock (logFileStreamLock) {
                using (Stream fStr = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read)) {
                    using (TextWriter strW = new StreamWriter(fStr)) {
                        strW.WriteLine(message);
                    }
                }
            }
        }

        [Benchmark]
        public void RunFile() {
            LogFile("``:: Start Script ::'");
            LogFile("Doing something ...");
            LogFile("Doing another thing ...");
            LogFile(".:: Finish Script ::.");
        }

        Object logFileLock = new Object();
        void LogFile(String message) {
            String path = Path.Combine(OutputDirPath, "logfile.log");
            lock (logFileLock) {
                File.AppendAllText(path, message);
            }
        }
    }
}

#region : Benchmark Results :
/*

// Validating benchmarks:
// ***** BenchmarkRunner: Start   *****
// ***** Found 6 benchmark(s) in total *****
// ***** Building 2 exe(s) in Parallel: Start   *****
// ***** Done, took 00:00:03 (3.59 sec)   *****
// Found 6 benchmarks:
//   WriteText.RunTextWriter: .NET Framework 4.6.1(Runtime=.NET Framework 4.6.1)
//   WriteText.RunFileStream: .NET Framework 4.6.1(Runtime=.NET Framework 4.6.1)
//   WriteText.RunFile: .NET Framework 4.6.1(Runtime=.NET Framework 4.6.1)
//   WriteText.RunTextWriter: .NET Framework 4.7.2(Runtime=.NET Framework 4.7.2)
//   WriteText.RunFileStream: .NET Framework 4.7.2(Runtime=.NET Framework 4.7.2)
//   WriteText.RunFile: .NET Framework 4.7.2(Runtime=.NET Framework 4.7.2)

// **************************
// Benchmark: WriteText.RunTextWriter: .NET Framework 4.6.1(Runtime=.NET Framework 4.6.1)
// *** Execute ***
// Launch: 1 / 1
// Execute: D:\Microsoft\Workspace\asakura89-project\release\AskScratchpad\CSScratchpad\CSScratchpad\bin\Release\c9bdf9ef-9f94-4234-a269-3f1a8720d89a.exe --benchmarkName "CSScratchpad.Script.WriteText.RunTextWriter" --job ".NET Framework 4.6.1" --benchmarkId 0 in 
// BeforeAnythingElse

// Benchmark Process Environment Information:
// Runtime=.NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
// GC=Concurrent Workstation
// Job: .NET Framework 4.6.1

OverheadJitting  1: 1 op, 218600.00 ns, 218.6000 us/op
WorkloadJitting  1: 1 op, 12667500.00 ns, 12.6675 ms/op

OverheadJitting  2: 16 op, 144700.00 ns, 9.0438 us/op
WorkloadJitting  2: 16 op, 75779200.00 ns, 4.7362 ms/op

WorkloadPilot    1: 16 op, 86685500.00 ns, 5.4178 ms/op
WorkloadPilot    2: 32 op, 149176300.00 ns, 4.6618 ms/op
WorkloadPilot    3: 64 op, 319517800.00 ns, 4.9925 ms/op
WorkloadPilot    4: 128 op, 637787800.00 ns, 4.9827 ms/op

OverheadWarmup   1: 128 op, 9400.00 ns, 73.4375 ns/op
OverheadWarmup   2: 128 op, 1200.00 ns, 9.3750 ns/op
OverheadWarmup   3: 128 op, 700.00 ns, 5.4688 ns/op
OverheadWarmup   4: 128 op, 800.00 ns, 6.2500 ns/op
OverheadWarmup   5: 128 op, 800.00 ns, 6.2500 ns/op
OverheadWarmup   6: 128 op, 700.00 ns, 5.4688 ns/op
OverheadWarmup   7: 128 op, 1100.00 ns, 8.5938 ns/op

OverheadActual   1: 128 op, 1300.00 ns, 10.1563 ns/op
OverheadActual   2: 128 op, 1000.00 ns, 7.8125 ns/op
OverheadActual   3: 128 op, 1200.00 ns, 9.3750 ns/op
OverheadActual   4: 128 op, 1400.00 ns, 10.9375 ns/op
OverheadActual   5: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadActual   6: 128 op, 2200.00 ns, 17.1875 ns/op
OverheadActual   7: 128 op, 2700.00 ns, 21.0938 ns/op
OverheadActual   8: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadActual   9: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual  10: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadActual  11: 128 op, 900.00 ns, 7.0313 ns/op
OverheadActual  12: 128 op, 900.00 ns, 7.0313 ns/op
OverheadActual  13: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual  14: 128 op, 1700.00 ns, 13.2813 ns/op
OverheadActual  15: 128 op, 4100.00 ns, 32.0313 ns/op
OverheadActual  16: 128 op, 900.00 ns, 7.0313 ns/op
OverheadActual  17: 128 op, 900.00 ns, 7.0313 ns/op
OverheadActual  18: 128 op, 2100.00 ns, 16.4063 ns/op
OverheadActual  19: 128 op, 1300.00 ns, 10.1563 ns/op
OverheadActual  20: 128 op, 800.00 ns, 6.2500 ns/op

WorkloadWarmup   1: 128 op, 654282000.00 ns, 5.1116 ms/op
WorkloadWarmup   2: 128 op, 647182600.00 ns, 5.0561 ms/op
WorkloadWarmup   3: 128 op, 669632900.00 ns, 5.2315 ms/op
WorkloadWarmup   4: 128 op, 674596300.00 ns, 5.2703 ms/op
WorkloadWarmup   5: 128 op, 629414900.00 ns, 4.9173 ms/op
WorkloadWarmup   6: 128 op, 675666300.00 ns, 5.2786 ms/op
WorkloadWarmup   7: 128 op, 679985200.00 ns, 5.3124 ms/op
WorkloadWarmup   8: 128 op, 649381200.00 ns, 5.0733 ms/op

// BeforeActualRun
WorkloadActual   1: 128 op, 631649400.00 ns, 4.9348 ms/op
WorkloadActual   2: 128 op, 674810300.00 ns, 5.2720 ms/op
WorkloadActual   3: 128 op, 636282100.00 ns, 4.9710 ms/op
WorkloadActual   4: 128 op, 614674700.00 ns, 4.8021 ms/op
WorkloadActual   5: 128 op, 629694900.00 ns, 4.9195 ms/op
WorkloadActual   6: 128 op, 665032600.00 ns, 5.1956 ms/op
WorkloadActual   7: 128 op, 708342200.00 ns, 5.5339 ms/op
WorkloadActual   8: 128 op, 923892400.00 ns, 7.2179 ms/op
WorkloadActual   9: 128 op, 1736759600.00 ns, 13.5684 ms/op
WorkloadActual  10: 128 op, 1572733500.00 ns, 12.2870 ms/op
WorkloadActual  11: 128 op, 624947400.00 ns, 4.8824 ms/op
WorkloadActual  12: 128 op, 627250900.00 ns, 4.9004 ms/op
WorkloadActual  13: 128 op, 619371300.00 ns, 4.8388 ms/op
WorkloadActual  14: 128 op, 632384100.00 ns, 4.9405 ms/op
WorkloadActual  15: 128 op, 686448000.00 ns, 5.3629 ms/op
WorkloadActual  16: 128 op, 632225900.00 ns, 4.9393 ms/op
WorkloadActual  17: 128 op, 643110100.00 ns, 5.0243 ms/op
WorkloadActual  18: 128 op, 1688226500.00 ns, 13.1893 ms/op
WorkloadActual  19: 128 op, 1568271000.00 ns, 12.2521 ms/op
WorkloadActual  20: 128 op, 907894200.00 ns, 7.0929 ms/op
WorkloadActual  21: 128 op, 654174700.00 ns, 5.1107 ms/op
WorkloadActual  22: 128 op, 604550900.00 ns, 4.7231 ms/op
WorkloadActual  23: 128 op, 618219800.00 ns, 4.8298 ms/op
WorkloadActual  24: 128 op, 617042000.00 ns, 4.8206 ms/op
WorkloadActual  25: 128 op, 612783700.00 ns, 4.7874 ms/op
WorkloadActual  26: 128 op, 623150000.00 ns, 4.8684 ms/op
WorkloadActual  27: 128 op, 628210600.00 ns, 4.9079 ms/op
WorkloadActual  28: 128 op, 640416500.00 ns, 5.0033 ms/op
WorkloadActual  29: 128 op, 976201400.00 ns, 7.6266 ms/op
WorkloadActual  30: 128 op, 1739424100.00 ns, 13.5893 ms/op
WorkloadActual  31: 128 op, 1414229100.00 ns, 11.0487 ms/op
WorkloadActual  32: 128 op, 633021300.00 ns, 4.9455 ms/op
WorkloadActual  33: 128 op, 638607500.00 ns, 4.9891 ms/op
WorkloadActual  34: 128 op, 620329600.00 ns, 4.8463 ms/op
WorkloadActual  35: 128 op, 632042700.00 ns, 4.9378 ms/op
WorkloadActual  36: 128 op, 651356200.00 ns, 5.0887 ms/op
WorkloadActual  37: 128 op, 1313954700.00 ns, 10.2653 ms/op
WorkloadActual  38: 128 op, 1434670100.00 ns, 11.2084 ms/op
WorkloadActual  39: 128 op, 1145369400.00 ns, 8.9482 ms/op
WorkloadActual  40: 128 op, 606126000.00 ns, 4.7354 ms/op
WorkloadActual  41: 128 op, 614360400.00 ns, 4.7997 ms/op
WorkloadActual  42: 128 op, 600305200.00 ns, 4.6899 ms/op
WorkloadActual  43: 128 op, 629397400.00 ns, 4.9172 ms/op
WorkloadActual  44: 128 op, 672041900.00 ns, 5.2503 ms/op
WorkloadActual  45: 128 op, 1194832700.00 ns, 9.3346 ms/op
WorkloadActual  46: 128 op, 1382332000.00 ns, 10.7995 ms/op
WorkloadActual  47: 128 op, 1304203400.00 ns, 10.1891 ms/op
WorkloadActual  48: 128 op, 609463300.00 ns, 4.7614 ms/op
WorkloadActual  49: 128 op, 609951400.00 ns, 4.7652 ms/op
WorkloadActual  50: 128 op, 611783200.00 ns, 4.7796 ms/op
WorkloadActual  51: 128 op, 632737300.00 ns, 4.9433 ms/op
WorkloadActual  52: 128 op, 601224900.00 ns, 4.6971 ms/op
WorkloadActual  53: 128 op, 1094390900.00 ns, 8.5499 ms/op
WorkloadActual  54: 128 op, 1429707100.00 ns, 11.1696 ms/op
WorkloadActual  55: 128 op, 1439626700.00 ns, 11.2471 ms/op
WorkloadActual  56: 128 op, 745403500.00 ns, 5.8235 ms/op
WorkloadActual  57: 128 op, 679228700.00 ns, 5.3065 ms/op
WorkloadActual  58: 128 op, 691275500.00 ns, 5.4006 ms/op
WorkloadActual  59: 128 op, 622362800.00 ns, 4.8622 ms/op
WorkloadActual  60: 128 op, 593432300.00 ns, 4.6362 ms/op
WorkloadActual  61: 128 op, 1407155900.00 ns, 10.9934 ms/op
WorkloadActual  62: 128 op, 1569855100.00 ns, 12.2645 ms/op
WorkloadActual  63: 128 op, 998367100.00 ns, 7.7997 ms/op
WorkloadActual  64: 128 op, 641601800.00 ns, 5.0125 ms/op
WorkloadActual  65: 128 op, 629800200.00 ns, 4.9203 ms/op
WorkloadActual  66: 128 op, 620485600.00 ns, 4.8475 ms/op
WorkloadActual  67: 128 op, 680719900.00 ns, 5.3181 ms/op
WorkloadActual  68: 128 op, 605427700.00 ns, 4.7299 ms/op
WorkloadActual  69: 128 op, 1416745500.00 ns, 11.0683 ms/op
WorkloadActual  70: 128 op, 1435969700.00 ns, 11.2185 ms/op
WorkloadActual  71: 128 op, 1065031300.00 ns, 8.3206 ms/op
WorkloadActual  72: 128 op, 617992300.00 ns, 4.8281 ms/op
WorkloadActual  73: 128 op, 694115700.00 ns, 5.4228 ms/op
WorkloadActual  74: 128 op, 649196500.00 ns, 5.0718 ms/op
WorkloadActual  75: 128 op, 701725200.00 ns, 5.4822 ms/op
WorkloadActual  76: 128 op, 781411200.00 ns, 6.1048 ms/op
WorkloadActual  77: 128 op, 1502370200.00 ns, 11.7373 ms/op
WorkloadActual  78: 128 op, 1399827600.00 ns, 10.9362 ms/op
WorkloadActual  79: 128 op, 906508900.00 ns, 7.0821 ms/op
WorkloadActual  80: 128 op, 695865200.00 ns, 5.4364 ms/op
WorkloadActual  81: 128 op, 627234300.00 ns, 4.9003 ms/op
WorkloadActual  82: 128 op, 600636700.00 ns, 4.6925 ms/op
WorkloadActual  83: 128 op, 591487100.00 ns, 4.6210 ms/op
WorkloadActual  84: 128 op, 904817300.00 ns, 7.0689 ms/op
WorkloadActual  85: 128 op, 1530698600.00 ns, 11.9586 ms/op
WorkloadActual  86: 128 op, 1490950800.00 ns, 11.6481 ms/op
WorkloadActual  87: 128 op, 747301600.00 ns, 5.8383 ms/op
WorkloadActual  88: 128 op, 602229700.00 ns, 4.7049 ms/op
WorkloadActual  89: 128 op, 621315800.00 ns, 4.8540 ms/op
WorkloadActual  90: 128 op, 677058300.00 ns, 5.2895 ms/op
WorkloadActual  91: 128 op, 603653900.00 ns, 4.7160 ms/op
WorkloadActual  92: 128 op, 982659500.00 ns, 7.6770 ms/op
WorkloadActual  93: 128 op, 1621225500.00 ns, 12.6658 ms/op
WorkloadActual  94: 128 op, 1455940200.00 ns, 11.3745 ms/op
WorkloadActual  95: 128 op, 659706900.00 ns, 5.1540 ms/op
WorkloadActual  96: 128 op, 639691400.00 ns, 4.9976 ms/op
WorkloadActual  97: 128 op, 626661900.00 ns, 4.8958 ms/op
WorkloadActual  98: 128 op, 639313100.00 ns, 4.9946 ms/op
WorkloadActual  99: 128 op, 617305100.00 ns, 4.8227 ms/op
WorkloadActual  100: 128 op, 1209673200.00 ns, 9.4506 ms/op

// AfterActualRun
WorkloadResult   1: 128 op, 631648300.00 ns, 4.9348 ms/op
WorkloadResult   2: 128 op, 674809200.00 ns, 5.2719 ms/op
WorkloadResult   3: 128 op, 636281000.00 ns, 4.9709 ms/op
WorkloadResult   4: 128 op, 614673600.00 ns, 4.8021 ms/op
WorkloadResult   5: 128 op, 629693800.00 ns, 4.9195 ms/op
WorkloadResult   6: 128 op, 665031500.00 ns, 5.1956 ms/op
WorkloadResult   7: 128 op, 708341100.00 ns, 5.5339 ms/op
WorkloadResult   8: 128 op, 923891300.00 ns, 7.2179 ms/op
WorkloadResult   9: 128 op, 1736758500.00 ns, 13.5684 ms/op
WorkloadResult  10: 128 op, 1572732400.00 ns, 12.2870 ms/op
WorkloadResult  11: 128 op, 624946300.00 ns, 4.8824 ms/op
WorkloadResult  12: 128 op, 627249800.00 ns, 4.9004 ms/op
WorkloadResult  13: 128 op, 619370200.00 ns, 4.8388 ms/op
WorkloadResult  14: 128 op, 632383000.00 ns, 4.9405 ms/op
WorkloadResult  15: 128 op, 686446900.00 ns, 5.3629 ms/op
WorkloadResult  16: 128 op, 632224800.00 ns, 4.9393 ms/op
WorkloadResult  17: 128 op, 643109000.00 ns, 5.0243 ms/op
WorkloadResult  18: 128 op, 1688225400.00 ns, 13.1893 ms/op
WorkloadResult  19: 128 op, 1568269900.00 ns, 12.2521 ms/op
WorkloadResult  20: 128 op, 907893100.00 ns, 7.0929 ms/op
WorkloadResult  21: 128 op, 654173600.00 ns, 5.1107 ms/op
WorkloadResult  22: 128 op, 604549800.00 ns, 4.7230 ms/op
WorkloadResult  23: 128 op, 618218700.00 ns, 4.8298 ms/op
WorkloadResult  24: 128 op, 617040900.00 ns, 4.8206 ms/op
WorkloadResult  25: 128 op, 612782600.00 ns, 4.7874 ms/op
WorkloadResult  26: 128 op, 623148900.00 ns, 4.8684 ms/op
WorkloadResult  27: 128 op, 628209500.00 ns, 4.9079 ms/op
WorkloadResult  28: 128 op, 640415400.00 ns, 5.0032 ms/op
WorkloadResult  29: 128 op, 976200300.00 ns, 7.6266 ms/op
WorkloadResult  30: 128 op, 1739423000.00 ns, 13.5892 ms/op
WorkloadResult  31: 128 op, 1414228000.00 ns, 11.0487 ms/op
WorkloadResult  32: 128 op, 633020200.00 ns, 4.9455 ms/op
WorkloadResult  33: 128 op, 638606400.00 ns, 4.9891 ms/op
WorkloadResult  34: 128 op, 620328500.00 ns, 4.8463 ms/op
WorkloadResult  35: 128 op, 632041600.00 ns, 4.9378 ms/op
WorkloadResult  36: 128 op, 651355100.00 ns, 5.0887 ms/op
WorkloadResult  37: 128 op, 1313953600.00 ns, 10.2653 ms/op
WorkloadResult  38: 128 op, 1434669000.00 ns, 11.2084 ms/op
WorkloadResult  39: 128 op, 1145368300.00 ns, 8.9482 ms/op
WorkloadResult  40: 128 op, 606124900.00 ns, 4.7354 ms/op
WorkloadResult  41: 128 op, 614359300.00 ns, 4.7997 ms/op
WorkloadResult  42: 128 op, 600304100.00 ns, 4.6899 ms/op
WorkloadResult  43: 128 op, 629396300.00 ns, 4.9172 ms/op
WorkloadResult  44: 128 op, 672040800.00 ns, 5.2503 ms/op
WorkloadResult  45: 128 op, 1194831600.00 ns, 9.3346 ms/op
WorkloadResult  46: 128 op, 1382330900.00 ns, 10.7995 ms/op
WorkloadResult  47: 128 op, 1304202300.00 ns, 10.1891 ms/op
WorkloadResult  48: 128 op, 609462200.00 ns, 4.7614 ms/op
WorkloadResult  49: 128 op, 609950300.00 ns, 4.7652 ms/op
WorkloadResult  50: 128 op, 611782100.00 ns, 4.7795 ms/op
WorkloadResult  51: 128 op, 632736200.00 ns, 4.9433 ms/op
WorkloadResult  52: 128 op, 601223800.00 ns, 4.6971 ms/op
WorkloadResult  53: 128 op, 1094389800.00 ns, 8.5499 ms/op
WorkloadResult  54: 128 op, 1429706000.00 ns, 11.1696 ms/op
WorkloadResult  55: 128 op, 1439625600.00 ns, 11.2471 ms/op
WorkloadResult  56: 128 op, 745402400.00 ns, 5.8235 ms/op
WorkloadResult  57: 128 op, 679227600.00 ns, 5.3065 ms/op
WorkloadResult  58: 128 op, 691274400.00 ns, 5.4006 ms/op
WorkloadResult  59: 128 op, 622361700.00 ns, 4.8622 ms/op
WorkloadResult  60: 128 op, 593431200.00 ns, 4.6362 ms/op
WorkloadResult  61: 128 op, 1407154800.00 ns, 10.9934 ms/op
WorkloadResult  62: 128 op, 1569854000.00 ns, 12.2645 ms/op
WorkloadResult  63: 128 op, 998366000.00 ns, 7.7997 ms/op
WorkloadResult  64: 128 op, 641600700.00 ns, 5.0125 ms/op
WorkloadResult  65: 128 op, 629799100.00 ns, 4.9203 ms/op
WorkloadResult  66: 128 op, 620484500.00 ns, 4.8475 ms/op
WorkloadResult  67: 128 op, 680718800.00 ns, 5.3181 ms/op
WorkloadResult  68: 128 op, 605426600.00 ns, 4.7299 ms/op
WorkloadResult  69: 128 op, 1416744400.00 ns, 11.0683 ms/op
WorkloadResult  70: 128 op, 1435968600.00 ns, 11.2185 ms/op
WorkloadResult  71: 128 op, 1065030200.00 ns, 8.3205 ms/op
WorkloadResult  72: 128 op, 617991200.00 ns, 4.8281 ms/op
WorkloadResult  73: 128 op, 694114600.00 ns, 5.4228 ms/op
WorkloadResult  74: 128 op, 649195400.00 ns, 5.0718 ms/op
WorkloadResult  75: 128 op, 701724100.00 ns, 5.4822 ms/op
WorkloadResult  76: 128 op, 781410100.00 ns, 6.1048 ms/op
WorkloadResult  77: 128 op, 1502369100.00 ns, 11.7373 ms/op
WorkloadResult  78: 128 op, 1399826500.00 ns, 10.9361 ms/op
WorkloadResult  79: 128 op, 906507800.00 ns, 7.0821 ms/op
WorkloadResult  80: 128 op, 695864100.00 ns, 5.4364 ms/op
WorkloadResult  81: 128 op, 627233200.00 ns, 4.9003 ms/op
WorkloadResult  82: 128 op, 600635600.00 ns, 4.6925 ms/op
WorkloadResult  83: 128 op, 591486000.00 ns, 4.6210 ms/op
WorkloadResult  84: 128 op, 904816200.00 ns, 7.0689 ms/op
WorkloadResult  85: 128 op, 1530697500.00 ns, 11.9586 ms/op
WorkloadResult  86: 128 op, 1490949700.00 ns, 11.6480 ms/op
WorkloadResult  87: 128 op, 747300500.00 ns, 5.8383 ms/op
WorkloadResult  88: 128 op, 602228600.00 ns, 4.7049 ms/op
WorkloadResult  89: 128 op, 621314700.00 ns, 4.8540 ms/op
WorkloadResult  90: 128 op, 677057200.00 ns, 5.2895 ms/op
WorkloadResult  91: 128 op, 603652800.00 ns, 4.7160 ms/op
WorkloadResult  92: 128 op, 982658400.00 ns, 7.6770 ms/op
WorkloadResult  93: 128 op, 1621224400.00 ns, 12.6658 ms/op
WorkloadResult  94: 128 op, 1455939100.00 ns, 11.3745 ms/op
WorkloadResult  95: 128 op, 659705800.00 ns, 5.1540 ms/op
WorkloadResult  96: 128 op, 639690300.00 ns, 4.9976 ms/op
WorkloadResult  97: 128 op, 626660800.00 ns, 4.8958 ms/op
WorkloadResult  98: 128 op, 639312000.00 ns, 4.9946 ms/op
WorkloadResult  99: 128 op, 617304000.00 ns, 4.8227 ms/op
WorkloadResult  100: 128 op, 1209672100.00 ns, 9.4506 ms/op

// AfterAll
// Benchmark Process 26500 has exited with code 0.

Mean = 6.832 ms, StdErr = 0.282 ms (4.12%), N = 100, StdDev = 2.816 ms
Min = 4.621 ms, Q1 = 4.867 ms, Median = 5.132 ms, Q3 = 8.649 ms, Max = 13.589 ms
IQR = 3.783 ms, LowerFence = -0.807 ms, UpperFence = 14.323 ms
ConfidenceInterval = [5.877 ms; 7.787 ms] (CI 99.9%), Margin = 0.955 ms (13.98% of Mean)
Skewness = 1.07, Kurtosis = 2.54, MValue = 2.61

// **************************
// Benchmark: WriteText.RunFileStream: .NET Framework 4.6.1(Runtime=.NET Framework 4.6.1)
// *** Execute ***
// Launch: 1 / 1
// Execute: D:\Microsoft\Workspace\asakura89-project\release\AskScratchpad\CSScratchpad\CSScratchpad\bin\Release\c9bdf9ef-9f94-4234-a269-3f1a8720d89a.exe --benchmarkName "CSScratchpad.Script.WriteText.RunFileStream" --job ".NET Framework 4.6.1" --benchmarkId 1 in 
// BeforeAnythingElse

// Benchmark Process Environment Information:
// Runtime=.NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
// GC=Concurrent Workstation
// Job: .NET Framework 4.6.1

OverheadJitting  1: 1 op, 497300.00 ns, 497.3000 us/op
WorkloadJitting  1: 1 op, 32762000.00 ns, 32.7620 ms/op

OverheadJitting  2: 16 op, 439300.00 ns, 27.4563 us/op
WorkloadJitting  2: 16 op, 217065600.00 ns, 13.5666 ms/op

WorkloadPilot    1: 16 op, 184724600.00 ns, 11.5453 ms/op
WorkloadPilot    2: 32 op, 370554400.00 ns, 11.5798 ms/op
WorkloadPilot    3: 64 op, 631956100.00 ns, 9.8743 ms/op

OverheadWarmup   1: 64 op, 10500.00 ns, 164.0625 ns/op
OverheadWarmup   2: 64 op, 600.00 ns, 9.3750 ns/op
OverheadWarmup   3: 64 op, 700.00 ns, 10.9375 ns/op
OverheadWarmup   4: 64 op, 700.00 ns, 10.9375 ns/op
OverheadWarmup   5: 64 op, 700.00 ns, 10.9375 ns/op
OverheadWarmup   6: 64 op, 1100.00 ns, 17.1875 ns/op

OverheadActual   1: 64 op, 700.00 ns, 10.9375 ns/op
OverheadActual   2: 64 op, 900.00 ns, 14.0625 ns/op
OverheadActual   3: 64 op, 1100.00 ns, 17.1875 ns/op
OverheadActual   4: 64 op, 700.00 ns, 10.9375 ns/op
OverheadActual   5: 64 op, 1300.00 ns, 20.3125 ns/op
OverheadActual   6: 64 op, 800.00 ns, 12.5000 ns/op
OverheadActual   7: 64 op, 700.00 ns, 10.9375 ns/op
OverheadActual   8: 64 op, 300.00 ns, 4.6875 ns/op
OverheadActual   9: 64 op, 400.00 ns, 6.2500 ns/op
OverheadActual  10: 64 op, 500.00 ns, 7.8125 ns/op
OverheadActual  11: 64 op, 400.00 ns, 6.2500 ns/op
OverheadActual  12: 64 op, 700.00 ns, 10.9375 ns/op
OverheadActual  13: 64 op, 1100.00 ns, 17.1875 ns/op
OverheadActual  14: 64 op, 1100.00 ns, 17.1875 ns/op
OverheadActual  15: 64 op, 600.00 ns, 9.3750 ns/op
OverheadActual  16: 64 op, 500.00 ns, 7.8125 ns/op
OverheadActual  17: 64 op, 700.00 ns, 10.9375 ns/op
OverheadActual  18: 64 op, 600.00 ns, 9.3750 ns/op
OverheadActual  19: 64 op, 600.00 ns, 9.3750 ns/op
OverheadActual  20: 64 op, 400.00 ns, 6.2500 ns/op

WorkloadWarmup   1: 64 op, 307072700.00 ns, 4.7980 ms/op
WorkloadWarmup   2: 64 op, 341801700.00 ns, 5.3407 ms/op
WorkloadWarmup   3: 64 op, 312774400.00 ns, 4.8871 ms/op
WorkloadWarmup   4: 64 op, 398271500.00 ns, 6.2230 ms/op
WorkloadWarmup   5: 64 op, 484327800.00 ns, 7.5676 ms/op
WorkloadWarmup   6: 64 op, 414532900.00 ns, 6.4771 ms/op

// BeforeActualRun
WorkloadActual   1: 64 op, 438607400.00 ns, 6.8532 ms/op
WorkloadActual   2: 64 op, 425416000.00 ns, 6.6471 ms/op
WorkloadActual   3: 64 op, 612443700.00 ns, 9.5694 ms/op
WorkloadActual   4: 64 op, 861882500.00 ns, 13.4669 ms/op
WorkloadActual   5: 64 op, 905640400.00 ns, 14.1506 ms/op
WorkloadActual   6: 64 op, 712944500.00 ns, 11.1398 ms/op
WorkloadActual   7: 64 op, 690849400.00 ns, 10.7945 ms/op
WorkloadActual   8: 64 op, 312236000.00 ns, 4.8787 ms/op
WorkloadActual   9: 64 op, 307429700.00 ns, 4.8036 ms/op
WorkloadActual  10: 64 op, 362286400.00 ns, 5.6607 ms/op
WorkloadActual  11: 64 op, 296917600.00 ns, 4.6393 ms/op
WorkloadActual  12: 64 op, 319399700.00 ns, 4.9906 ms/op
WorkloadActual  13: 64 op, 313608100.00 ns, 4.9001 ms/op
WorkloadActual  14: 64 op, 301644800.00 ns, 4.7132 ms/op
WorkloadActual  15: 64 op, 303219600.00 ns, 4.7378 ms/op
WorkloadActual  16: 64 op, 319171200.00 ns, 4.9871 ms/op
WorkloadActual  17: 64 op, 322285000.00 ns, 5.0357 ms/op
WorkloadActual  18: 64 op, 312579400.00 ns, 4.8841 ms/op
WorkloadActual  19: 64 op, 746600300.00 ns, 11.6656 ms/op
WorkloadActual  20: 64 op, 740552300.00 ns, 11.5711 ms/op
WorkloadActual  21: 64 op, 752444300.00 ns, 11.7569 ms/op
WorkloadActual  22: 64 op, 735872100.00 ns, 11.4980 ms/op
WorkloadActual  23: 64 op, 685617500.00 ns, 10.7128 ms/op
WorkloadActual  24: 64 op, 332328300.00 ns, 5.1926 ms/op
WorkloadActual  25: 64 op, 315996600.00 ns, 4.9374 ms/op
WorkloadActual  26: 64 op, 306151200.00 ns, 4.7836 ms/op
WorkloadActual  27: 64 op, 323439000.00 ns, 5.0537 ms/op
WorkloadActual  28: 64 op, 409517700.00 ns, 6.3987 ms/op
WorkloadActual  29: 64 op, 327808000.00 ns, 5.1220 ms/op
WorkloadActual  30: 64 op, 306197200.00 ns, 4.7843 ms/op
WorkloadActual  31: 64 op, 305829200.00 ns, 4.7786 ms/op
WorkloadActual  32: 64 op, 306533500.00 ns, 4.7896 ms/op
WorkloadActual  33: 64 op, 332551200.00 ns, 5.1961 ms/op
WorkloadActual  34: 64 op, 530718200.00 ns, 8.2925 ms/op
WorkloadActual  35: 64 op, 667153100.00 ns, 10.4243 ms/op
WorkloadActual  36: 64 op, 725788500.00 ns, 11.3404 ms/op
WorkloadActual  37: 64 op, 695275300.00 ns, 10.8637 ms/op
WorkloadActual  38: 64 op, 756113900.00 ns, 11.8143 ms/op
WorkloadActual  39: 64 op, 511246000.00 ns, 7.9882 ms/op
WorkloadActual  40: 64 op, 293669700.00 ns, 4.5886 ms/op
WorkloadActual  41: 64 op, 320789900.00 ns, 5.0123 ms/op
WorkloadActual  42: 64 op, 306508600.00 ns, 4.7892 ms/op
WorkloadActual  43: 64 op, 324230300.00 ns, 5.0661 ms/op
WorkloadActual  44: 64 op, 319412100.00 ns, 4.9908 ms/op
WorkloadActual  45: 64 op, 313320800.00 ns, 4.8956 ms/op
WorkloadActual  46: 64 op, 309553100.00 ns, 4.8368 ms/op
WorkloadActual  47: 64 op, 350787600.00 ns, 5.4811 ms/op
WorkloadActual  48: 64 op, 299308300.00 ns, 4.6767 ms/op
WorkloadActual  49: 64 op, 301347300.00 ns, 4.7086 ms/op
WorkloadActual  50: 64 op, 481013900.00 ns, 7.5158 ms/op
WorkloadActual  51: 64 op, 791580500.00 ns, 12.3684 ms/op
WorkloadActual  52: 64 op, 826623100.00 ns, 12.9160 ms/op
WorkloadActual  53: 64 op, 810568900.00 ns, 12.6651 ms/op
WorkloadActual  54: 64 op, 888182500.00 ns, 13.8779 ms/op
WorkloadActual  55: 64 op, 345523300.00 ns, 5.3988 ms/op
WorkloadActual  56: 64 op, 365835600.00 ns, 5.7162 ms/op
WorkloadActual  57: 64 op, 299704700.00 ns, 4.6829 ms/op
WorkloadActual  58: 64 op, 313069100.00 ns, 4.8917 ms/op
WorkloadActual  59: 64 op, 325932800.00 ns, 5.0927 ms/op
WorkloadActual  60: 64 op, 311340600.00 ns, 4.8647 ms/op
WorkloadActual  61: 64 op, 303664600.00 ns, 4.7448 ms/op
WorkloadActual  62: 64 op, 341139000.00 ns, 5.3303 ms/op
WorkloadActual  63: 64 op, 306973800.00 ns, 4.7965 ms/op
WorkloadActual  64: 64 op, 325529100.00 ns, 5.0864 ms/op
WorkloadActual  65: 64 op, 390389800.00 ns, 6.0998 ms/op
WorkloadActual  66: 64 op, 740333000.00 ns, 11.5677 ms/op
WorkloadActual  67: 64 op, 811502000.00 ns, 12.6797 ms/op
WorkloadActual  68: 64 op, 757979500.00 ns, 11.8434 ms/op
WorkloadActual  69: 64 op, 784788600.00 ns, 12.2623 ms/op
WorkloadActual  70: 64 op, 483772600.00 ns, 7.5589 ms/op
WorkloadActual  71: 64 op, 331042300.00 ns, 5.1725 ms/op
WorkloadActual  72: 64 op, 293367400.00 ns, 4.5839 ms/op
WorkloadActual  73: 64 op, 319333500.00 ns, 4.9896 ms/op
WorkloadActual  74: 64 op, 325473900.00 ns, 5.0855 ms/op
WorkloadActual  75: 64 op, 312449500.00 ns, 4.8820 ms/op
WorkloadActual  76: 64 op, 306320500.00 ns, 4.7863 ms/op
WorkloadActual  77: 64 op, 314914000.00 ns, 4.9205 ms/op
WorkloadActual  78: 64 op, 334296200.00 ns, 5.2234 ms/op
WorkloadActual  79: 64 op, 312658900.00 ns, 4.8853 ms/op
WorkloadActual  80: 64 op, 385340400.00 ns, 6.0209 ms/op
WorkloadActual  81: 64 op, 634190500.00 ns, 9.9092 ms/op
WorkloadActual  82: 64 op, 699899700.00 ns, 10.9359 ms/op
WorkloadActual  83: 64 op, 693341500.00 ns, 10.8335 ms/op
WorkloadActual  84: 64 op, 698710000.00 ns, 10.9173 ms/op
WorkloadActual  85: 64 op, 748292300.00 ns, 11.6921 ms/op
WorkloadActual  86: 64 op, 425892000.00 ns, 6.6546 ms/op
WorkloadActual  87: 64 op, 310182500.00 ns, 4.8466 ms/op
WorkloadActual  88: 64 op, 301710100.00 ns, 4.7142 ms/op
WorkloadActual  89: 64 op, 312358300.00 ns, 4.8806 ms/op
WorkloadActual  90: 64 op, 316558900.00 ns, 4.9462 ms/op
WorkloadActual  91: 64 op, 347888500.00 ns, 5.4358 ms/op
WorkloadActual  92: 64 op, 309857300.00 ns, 4.8415 ms/op
WorkloadActual  93: 64 op, 323348300.00 ns, 5.0523 ms/op
WorkloadActual  94: 64 op, 306833100.00 ns, 4.7943 ms/op
WorkloadActual  95: 64 op, 328055100.00 ns, 5.1259 ms/op
WorkloadActual  96: 64 op, 303929400.00 ns, 4.7489 ms/op
WorkloadActual  97: 64 op, 565585200.00 ns, 8.8373 ms/op
WorkloadActual  98: 64 op, 703100900.00 ns, 10.9860 ms/op
WorkloadActual  99: 64 op, 727508900.00 ns, 11.3673 ms/op
WorkloadActual  100: 64 op, 727224400.00 ns, 11.3629 ms/op

// AfterActualRun
WorkloadResult   1: 64 op, 438606700.00 ns, 6.8532 ms/op
WorkloadResult   2: 64 op, 425415300.00 ns, 6.6471 ms/op
WorkloadResult   3: 64 op, 612443000.00 ns, 9.5694 ms/op
WorkloadResult   4: 64 op, 861881800.00 ns, 13.4669 ms/op
WorkloadResult   5: 64 op, 905639700.00 ns, 14.1506 ms/op
WorkloadResult   6: 64 op, 712943800.00 ns, 11.1397 ms/op
WorkloadResult   7: 64 op, 690848700.00 ns, 10.7945 ms/op
WorkloadResult   8: 64 op, 312235300.00 ns, 4.8787 ms/op
WorkloadResult   9: 64 op, 307429000.00 ns, 4.8036 ms/op
WorkloadResult  10: 64 op, 362285700.00 ns, 5.6607 ms/op
WorkloadResult  11: 64 op, 296916900.00 ns, 4.6393 ms/op
WorkloadResult  12: 64 op, 319399000.00 ns, 4.9906 ms/op
WorkloadResult  13: 64 op, 313607400.00 ns, 4.9001 ms/op
WorkloadResult  14: 64 op, 301644100.00 ns, 4.7132 ms/op
WorkloadResult  15: 64 op, 303218900.00 ns, 4.7378 ms/op
WorkloadResult  16: 64 op, 319170500.00 ns, 4.9870 ms/op
WorkloadResult  17: 64 op, 322284300.00 ns, 5.0357 ms/op
WorkloadResult  18: 64 op, 312578700.00 ns, 4.8840 ms/op
WorkloadResult  19: 64 op, 746599600.00 ns, 11.6656 ms/op
WorkloadResult  20: 64 op, 740551600.00 ns, 11.5711 ms/op
WorkloadResult  21: 64 op, 752443600.00 ns, 11.7569 ms/op
WorkloadResult  22: 64 op, 735871400.00 ns, 11.4980 ms/op
WorkloadResult  23: 64 op, 685616800.00 ns, 10.7128 ms/op
WorkloadResult  24: 64 op, 332327600.00 ns, 5.1926 ms/op
WorkloadResult  25: 64 op, 315995900.00 ns, 4.9374 ms/op
WorkloadResult  26: 64 op, 306150500.00 ns, 4.7836 ms/op
WorkloadResult  27: 64 op, 323438300.00 ns, 5.0537 ms/op
WorkloadResult  28: 64 op, 409517000.00 ns, 6.3987 ms/op
WorkloadResult  29: 64 op, 327807300.00 ns, 5.1220 ms/op
WorkloadResult  30: 64 op, 306196500.00 ns, 4.7843 ms/op
WorkloadResult  31: 64 op, 305828500.00 ns, 4.7786 ms/op
WorkloadResult  32: 64 op, 306532800.00 ns, 4.7896 ms/op
WorkloadResult  33: 64 op, 332550500.00 ns, 5.1961 ms/op
WorkloadResult  34: 64 op, 530717500.00 ns, 8.2925 ms/op
WorkloadResult  35: 64 op, 667152400.00 ns, 10.4243 ms/op
WorkloadResult  36: 64 op, 725787800.00 ns, 11.3404 ms/op
WorkloadResult  37: 64 op, 695274600.00 ns, 10.8637 ms/op
WorkloadResult  38: 64 op, 756113200.00 ns, 11.8143 ms/op
WorkloadResult  39: 64 op, 511245300.00 ns, 7.9882 ms/op
WorkloadResult  40: 64 op, 293669000.00 ns, 4.5886 ms/op
WorkloadResult  41: 64 op, 320789200.00 ns, 5.0123 ms/op
WorkloadResult  42: 64 op, 306507900.00 ns, 4.7892 ms/op
WorkloadResult  43: 64 op, 324229600.00 ns, 5.0661 ms/op
WorkloadResult  44: 64 op, 319411400.00 ns, 4.9908 ms/op
WorkloadResult  45: 64 op, 313320100.00 ns, 4.8956 ms/op
WorkloadResult  46: 64 op, 309552400.00 ns, 4.8368 ms/op
WorkloadResult  47: 64 op, 350786900.00 ns, 5.4810 ms/op
WorkloadResult  48: 64 op, 299307600.00 ns, 4.6767 ms/op
WorkloadResult  49: 64 op, 301346600.00 ns, 4.7085 ms/op
WorkloadResult  50: 64 op, 481013200.00 ns, 7.5158 ms/op
WorkloadResult  51: 64 op, 791579800.00 ns, 12.3684 ms/op
WorkloadResult  52: 64 op, 826622400.00 ns, 12.9160 ms/op
WorkloadResult  53: 64 op, 810568200.00 ns, 12.6651 ms/op
WorkloadResult  54: 64 op, 888181800.00 ns, 13.8778 ms/op
WorkloadResult  55: 64 op, 345522600.00 ns, 5.3988 ms/op
WorkloadResult  56: 64 op, 365834900.00 ns, 5.7162 ms/op
WorkloadResult  57: 64 op, 299704000.00 ns, 4.6829 ms/op
WorkloadResult  58: 64 op, 313068400.00 ns, 4.8917 ms/op
WorkloadResult  59: 64 op, 325932100.00 ns, 5.0927 ms/op
WorkloadResult  60: 64 op, 311339900.00 ns, 4.8647 ms/op
WorkloadResult  61: 64 op, 303663900.00 ns, 4.7447 ms/op
WorkloadResult  62: 64 op, 341138300.00 ns, 5.3303 ms/op
WorkloadResult  63: 64 op, 306973100.00 ns, 4.7965 ms/op
WorkloadResult  64: 64 op, 325528400.00 ns, 5.0864 ms/op
WorkloadResult  65: 64 op, 390389100.00 ns, 6.0998 ms/op
WorkloadResult  66: 64 op, 740332300.00 ns, 11.5677 ms/op
WorkloadResult  67: 64 op, 811501300.00 ns, 12.6797 ms/op
WorkloadResult  68: 64 op, 757978800.00 ns, 11.8434 ms/op
WorkloadResult  69: 64 op, 784787900.00 ns, 12.2623 ms/op
WorkloadResult  70: 64 op, 483771900.00 ns, 7.5589 ms/op
WorkloadResult  71: 64 op, 331041600.00 ns, 5.1725 ms/op
WorkloadResult  72: 64 op, 293366700.00 ns, 4.5839 ms/op
WorkloadResult  73: 64 op, 319332800.00 ns, 4.9896 ms/op
WorkloadResult  74: 64 op, 325473200.00 ns, 5.0855 ms/op
WorkloadResult  75: 64 op, 312448800.00 ns, 4.8820 ms/op
WorkloadResult  76: 64 op, 306319800.00 ns, 4.7862 ms/op
WorkloadResult  77: 64 op, 314913300.00 ns, 4.9205 ms/op
WorkloadResult  78: 64 op, 334295500.00 ns, 5.2234 ms/op
WorkloadResult  79: 64 op, 312658200.00 ns, 4.8853 ms/op
WorkloadResult  80: 64 op, 385339700.00 ns, 6.0209 ms/op
WorkloadResult  81: 64 op, 634189800.00 ns, 9.9092 ms/op
WorkloadResult  82: 64 op, 699899000.00 ns, 10.9359 ms/op
WorkloadResult  83: 64 op, 693340800.00 ns, 10.8335 ms/op
WorkloadResult  84: 64 op, 698709300.00 ns, 10.9173 ms/op
WorkloadResult  85: 64 op, 748291600.00 ns, 11.6921 ms/op
WorkloadResult  86: 64 op, 425891300.00 ns, 6.6546 ms/op
WorkloadResult  87: 64 op, 310181800.00 ns, 4.8466 ms/op
WorkloadResult  88: 64 op, 301709400.00 ns, 4.7142 ms/op
WorkloadResult  89: 64 op, 312357600.00 ns, 4.8806 ms/op
WorkloadResult  90: 64 op, 316558200.00 ns, 4.9462 ms/op
WorkloadResult  91: 64 op, 347887800.00 ns, 5.4357 ms/op
WorkloadResult  92: 64 op, 309856600.00 ns, 4.8415 ms/op
WorkloadResult  93: 64 op, 323347600.00 ns, 5.0523 ms/op
WorkloadResult  94: 64 op, 306832400.00 ns, 4.7943 ms/op
WorkloadResult  95: 64 op, 328054400.00 ns, 5.1259 ms/op
WorkloadResult  96: 64 op, 303928700.00 ns, 4.7489 ms/op
WorkloadResult  97: 64 op, 565584500.00 ns, 8.8373 ms/op
WorkloadResult  98: 64 op, 703100200.00 ns, 10.9859 ms/op
WorkloadResult  99: 64 op, 727508200.00 ns, 11.3673 ms/op
WorkloadResult  100: 64 op, 727223700.00 ns, 11.3629 ms/op

// AfterAll
// Benchmark Process 13300 has exited with code 0.

Mean = 7.203 ms, StdErr = 0.307 ms (4.26%), N = 100, StdDev = 3.067 ms
Min = 4.584 ms, Q1 = 4.880 ms, Median = 5.183 ms, Q3 = 10.804 ms, Max = 14.151 ms
IQR = 5.924 ms, LowerFence = -4.006 ms, UpperFence = 19.690 ms
ConfidenceInterval = [6.162 ms; 8.243 ms] (CI 99.9%), Margin = 1.040 ms (14.44% of Mean)
Skewness = 0.82, Kurtosis = 1.98, MValue = 2.74

// **************************
// Benchmark: WriteText.RunFile: .NET Framework 4.6.1(Runtime=.NET Framework 4.6.1)
// *** Execute ***
// Launch: 1 / 1
// Execute: D:\Microsoft\Workspace\asakura89-project\release\AskScratchpad\CSScratchpad\CSScratchpad\bin\Release\c9bdf9ef-9f94-4234-a269-3f1a8720d89a.exe --benchmarkName "CSScratchpad.Script.WriteText.RunFile" --job ".NET Framework 4.6.1" --benchmarkId 2 in 
// BeforeAnythingElse

// Benchmark Process Environment Information:
// Runtime=.NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
// GC=Concurrent Workstation
// Job: .NET Framework 4.6.1

OverheadJitting  1: 1 op, 208100.00 ns, 208.1000 us/op
WorkloadJitting  1: 1 op, 12543000.00 ns, 12.5430 ms/op

OverheadJitting  2: 16 op, 206500.00 ns, 12.9063 us/op
WorkloadJitting  2: 16 op, 81192000.00 ns, 5.0745 ms/op

WorkloadPilot    1: 16 op, 80027400.00 ns, 5.0017 ms/op
WorkloadPilot    2: 32 op, 186302900.00 ns, 5.8220 ms/op
WorkloadPilot    3: 64 op, 307517300.00 ns, 4.8050 ms/op
WorkloadPilot    4: 128 op, 710096200.00 ns, 5.5476 ms/op

OverheadWarmup   1: 128 op, 5400.00 ns, 42.1875 ns/op
OverheadWarmup   2: 128 op, 900.00 ns, 7.0313 ns/op
OverheadWarmup   3: 128 op, 900.00 ns, 7.0313 ns/op
OverheadWarmup   4: 128 op, 1300.00 ns, 10.1563 ns/op
OverheadWarmup   5: 128 op, 1200.00 ns, 9.3750 ns/op
OverheadWarmup   6: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadWarmup   7: 128 op, 1700.00 ns, 13.2813 ns/op

OverheadActual   1: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadActual   2: 128 op, 2000.00 ns, 15.6250 ns/op
OverheadActual   3: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadActual   4: 128 op, 1800.00 ns, 14.0625 ns/op
OverheadActual   5: 128 op, 1000.00 ns, 7.8125 ns/op
OverheadActual   6: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadActual   7: 128 op, 1200.00 ns, 9.3750 ns/op
OverheadActual   8: 128 op, 900.00 ns, 7.0313 ns/op
OverheadActual   9: 128 op, 1300.00 ns, 10.1563 ns/op
OverheadActual  10: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadActual  11: 128 op, 2400.00 ns, 18.7500 ns/op
OverheadActual  12: 128 op, 2100.00 ns, 16.4063 ns/op
OverheadActual  13: 128 op, 1000.00 ns, 7.8125 ns/op
OverheadActual  14: 128 op, 900.00 ns, 7.0313 ns/op
OverheadActual  15: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadActual  16: 128 op, 1000.00 ns, 7.8125 ns/op
OverheadActual  17: 128 op, 900.00 ns, 7.0313 ns/op
OverheadActual  18: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadActual  19: 128 op, 1000.00 ns, 7.8125 ns/op
OverheadActual  20: 128 op, 900.00 ns, 7.0313 ns/op

WorkloadWarmup   1: 128 op, 688392300.00 ns, 5.3781 ms/op
WorkloadWarmup   2: 128 op, 623372700.00 ns, 4.8701 ms/op
WorkloadWarmup   3: 128 op, 650252800.00 ns, 5.0801 ms/op
WorkloadWarmup   4: 128 op, 1316926900.00 ns, 10.2885 ms/op
WorkloadWarmup   5: 128 op, 1429504600.00 ns, 11.1680 ms/op
WorkloadWarmup   6: 128 op, 1184004900.00 ns, 9.2500 ms/op
WorkloadWarmup   7: 128 op, 612617100.00 ns, 4.7861 ms/op
WorkloadWarmup   8: 128 op, 615378600.00 ns, 4.8076 ms/op
WorkloadWarmup   9: 128 op, 636762000.00 ns, 4.9747 ms/op
WorkloadWarmup  10: 128 op, 660533300.00 ns, 5.1604 ms/op
WorkloadWarmup  11: 128 op, 679060300.00 ns, 5.3052 ms/op
WorkloadWarmup  12: 128 op, 1307165200.00 ns, 10.2122 ms/op
WorkloadWarmup  13: 128 op, 1492627300.00 ns, 11.6612 ms/op
WorkloadWarmup  14: 128 op, 1239415900.00 ns, 9.6829 ms/op

// BeforeActualRun
WorkloadActual   1: 128 op, 596879600.00 ns, 4.6631 ms/op
WorkloadActual   2: 128 op, 600712200.00 ns, 4.6931 ms/op
WorkloadActual   3: 128 op, 643308900.00 ns, 5.0259 ms/op
WorkloadActual   4: 128 op, 597458000.00 ns, 4.6676 ms/op
WorkloadActual   5: 128 op, 628030700.00 ns, 4.9065 ms/op
WorkloadActual   6: 128 op, 1415130200.00 ns, 11.0557 ms/op
WorkloadActual   7: 128 op, 1547308100.00 ns, 12.0883 ms/op
WorkloadActual   8: 128 op, 1588679500.00 ns, 12.4116 ms/op
WorkloadActual   9: 128 op, 747169900.00 ns, 5.8373 ms/op
WorkloadActual  10: 128 op, 616535400.00 ns, 4.8167 ms/op
WorkloadActual  11: 128 op, 642986200.00 ns, 5.0233 ms/op
WorkloadActual  12: 128 op, 774617100.00 ns, 6.0517 ms/op
WorkloadActual  13: 128 op, 690228800.00 ns, 5.3924 ms/op
WorkloadActual  14: 128 op, 1159601300.00 ns, 9.0594 ms/op
WorkloadActual  15: 128 op, 1453552300.00 ns, 11.3559 ms/op
WorkloadActual  16: 128 op, 1353378200.00 ns, 10.5733 ms/op
WorkloadActual  17: 128 op, 709437300.00 ns, 5.5425 ms/op
WorkloadActual  18: 128 op, 612152300.00 ns, 4.7824 ms/op
WorkloadActual  19: 128 op, 595621500.00 ns, 4.6533 ms/op
WorkloadActual  20: 128 op, 615037500.00 ns, 4.8050 ms/op
WorkloadActual  21: 128 op, 628201600.00 ns, 4.9078 ms/op
WorkloadActual  22: 128 op, 1297456500.00 ns, 10.1364 ms/op
WorkloadActual  23: 128 op, 1405928900.00 ns, 10.9838 ms/op
WorkloadActual  24: 128 op, 1239256500.00 ns, 9.6817 ms/op
WorkloadActual  25: 128 op, 602802300.00 ns, 4.7094 ms/op
WorkloadActual  26: 128 op, 647640800.00 ns, 5.0597 ms/op
WorkloadActual  27: 128 op, 624983100.00 ns, 4.8827 ms/op
WorkloadActual  28: 128 op, 622107200.00 ns, 4.8602 ms/op
WorkloadActual  29: 128 op, 632977500.00 ns, 4.9451 ms/op
WorkloadActual  30: 128 op, 1185359000.00 ns, 9.2606 ms/op
WorkloadActual  31: 128 op, 1453833700.00 ns, 11.3581 ms/op
WorkloadActual  32: 128 op, 1477637000.00 ns, 11.5440 ms/op
WorkloadActual  33: 128 op, 1136314200.00 ns, 8.8775 ms/op
WorkloadActual  34: 128 op, 614215900.00 ns, 4.7986 ms/op
WorkloadActual  35: 128 op, 614270900.00 ns, 4.7990 ms/op
WorkloadActual  36: 128 op, 600831400.00 ns, 4.6940 ms/op
WorkloadActual  37: 128 op, 688493400.00 ns, 5.3789 ms/op
WorkloadActual  38: 128 op, 755463500.00 ns, 5.9021 ms/op
WorkloadActual  39: 128 op, 1588491200.00 ns, 12.4101 ms/op
WorkloadActual  40: 128 op, 1407943200.00 ns, 10.9996 ms/op
WorkloadActual  41: 128 op, 1386836500.00 ns, 10.8347 ms/op
WorkloadActual  42: 128 op, 759270100.00 ns, 5.9318 ms/op
WorkloadActual  43: 128 op, 656160400.00 ns, 5.1263 ms/op
WorkloadActual  44: 128 op, 694399600.00 ns, 5.4250 ms/op
WorkloadActual  45: 128 op, 660740200.00 ns, 5.1620 ms/op
WorkloadActual  46: 128 op, 659854600.00 ns, 5.1551 ms/op
WorkloadActual  47: 128 op, 947149900.00 ns, 7.3996 ms/op
WorkloadActual  48: 128 op, 1436797600.00 ns, 11.2250 ms/op
WorkloadActual  49: 128 op, 1492847600.00 ns, 11.6629 ms/op
WorkloadActual  50: 128 op, 1320991900.00 ns, 10.3202 ms/op
WorkloadActual  51: 128 op, 636348800.00 ns, 4.9715 ms/op
WorkloadActual  52: 128 op, 694997500.00 ns, 5.4297 ms/op
WorkloadActual  53: 128 op, 604060800.00 ns, 4.7192 ms/op
WorkloadActual  54: 128 op, 641945800.00 ns, 5.0152 ms/op
WorkloadActual  55: 128 op, 622111400.00 ns, 4.8602 ms/op
WorkloadActual  56: 128 op, 1198023200.00 ns, 9.3596 ms/op
WorkloadActual  57: 128 op, 1378805200.00 ns, 10.7719 ms/op
WorkloadActual  58: 128 op, 1354149100.00 ns, 10.5793 ms/op
WorkloadActual  59: 128 op, 602689200.00 ns, 4.7085 ms/op
WorkloadActual  60: 128 op, 624253000.00 ns, 4.8770 ms/op
WorkloadActual  61: 128 op, 612233400.00 ns, 4.7831 ms/op
WorkloadActual  62: 128 op, 1143380600.00 ns, 8.9327 ms/op
WorkloadActual  63: 128 op, 1193021800.00 ns, 9.3205 ms/op
WorkloadActual  64: 128 op, 1653901800.00 ns, 12.9211 ms/op
WorkloadActual  65: 128 op, 1622889800.00 ns, 12.6788 ms/op
WorkloadActual  66: 128 op, 1013973100.00 ns, 7.9217 ms/op
WorkloadActual  67: 128 op, 597085700.00 ns, 4.6647 ms/op
WorkloadActual  68: 128 op, 597508500.00 ns, 4.6680 ms/op
WorkloadActual  69: 128 op, 646168500.00 ns, 5.0482 ms/op
WorkloadActual  70: 128 op, 633595700.00 ns, 4.9500 ms/op
WorkloadActual  71: 128 op, 610501200.00 ns, 4.7695 ms/op
WorkloadActual  72: 128 op, 1335866100.00 ns, 10.4365 ms/op
WorkloadActual  73: 128 op, 1378184000.00 ns, 10.7671 ms/op
WorkloadActual  74: 128 op, 1415748500.00 ns, 11.0605 ms/op
WorkloadActual  75: 128 op, 1073480600.00 ns, 8.3866 ms/op
WorkloadActual  76: 128 op, 601528300.00 ns, 4.6994 ms/op
WorkloadActual  77: 128 op, 654329000.00 ns, 5.1119 ms/op
WorkloadActual  78: 128 op, 819633600.00 ns, 6.4034 ms/op
WorkloadActual  79: 128 op, 800873300.00 ns, 6.2568 ms/op
WorkloadActual  80: 128 op, 1543302800.00 ns, 12.0571 ms/op
WorkloadActual  81: 128 op, 1524123000.00 ns, 11.9072 ms/op
WorkloadActual  82: 128 op, 1483487300.00 ns, 11.5897 ms/op
WorkloadActual  83: 128 op, 953842400.00 ns, 7.4519 ms/op
WorkloadActual  84: 128 op, 695914700.00 ns, 5.4368 ms/op
WorkloadActual  85: 128 op, 687287400.00 ns, 5.3694 ms/op
WorkloadActual  86: 128 op, 651146700.00 ns, 5.0871 ms/op
WorkloadActual  87: 128 op, 628134000.00 ns, 4.9073 ms/op
WorkloadActual  88: 128 op, 1026897100.00 ns, 8.0226 ms/op
WorkloadActual  89: 128 op, 1483391100.00 ns, 11.5890 ms/op
WorkloadActual  90: 128 op, 1603617400.00 ns, 12.5283 ms/op
WorkloadActual  91: 128 op, 1195337800.00 ns, 9.3386 ms/op
WorkloadActual  92: 128 op, 620009600.00 ns, 4.8438 ms/op
WorkloadActual  93: 128 op, 607033400.00 ns, 4.7424 ms/op
WorkloadActual  94: 128 op, 606848700.00 ns, 4.7410 ms/op
WorkloadActual  95: 128 op, 642681900.00 ns, 5.0210 ms/op
WorkloadActual  96: 128 op, 701278500.00 ns, 5.4787 ms/op
WorkloadActual  97: 128 op, 1435257800.00 ns, 11.2130 ms/op
WorkloadActual  98: 128 op, 1477876700.00 ns, 11.5459 ms/op
WorkloadActual  99: 128 op, 1480393700.00 ns, 11.5656 ms/op
WorkloadActual  100: 128 op, 943037100.00 ns, 7.3675 ms/op

// AfterActualRun
WorkloadResult   1: 128 op, 596878500.00 ns, 4.6631 ms/op
WorkloadResult   2: 128 op, 600711100.00 ns, 4.6931 ms/op
WorkloadResult   3: 128 op, 643307800.00 ns, 5.0258 ms/op
WorkloadResult   4: 128 op, 597456900.00 ns, 4.6676 ms/op
WorkloadResult   5: 128 op, 628029600.00 ns, 4.9065 ms/op
WorkloadResult   6: 128 op, 1415129100.00 ns, 11.0557 ms/op
WorkloadResult   7: 128 op, 1547307000.00 ns, 12.0883 ms/op
WorkloadResult   8: 128 op, 1588678400.00 ns, 12.4116 ms/op
WorkloadResult   9: 128 op, 747168800.00 ns, 5.8373 ms/op
WorkloadResult  10: 128 op, 616534300.00 ns, 4.8167 ms/op
WorkloadResult  11: 128 op, 642985100.00 ns, 5.0233 ms/op
WorkloadResult  12: 128 op, 774616000.00 ns, 6.0517 ms/op
WorkloadResult  13: 128 op, 690227700.00 ns, 5.3924 ms/op
WorkloadResult  14: 128 op, 1159600200.00 ns, 9.0594 ms/op
WorkloadResult  15: 128 op, 1453551200.00 ns, 11.3559 ms/op
WorkloadResult  16: 128 op, 1353377100.00 ns, 10.5733 ms/op
WorkloadResult  17: 128 op, 709436200.00 ns, 5.5425 ms/op
WorkloadResult  18: 128 op, 612151200.00 ns, 4.7824 ms/op
WorkloadResult  19: 128 op, 595620400.00 ns, 4.6533 ms/op
WorkloadResult  20: 128 op, 615036400.00 ns, 4.8050 ms/op
WorkloadResult  21: 128 op, 628200500.00 ns, 4.9078 ms/op
WorkloadResult  22: 128 op, 1297455400.00 ns, 10.1364 ms/op
WorkloadResult  23: 128 op, 1405927800.00 ns, 10.9838 ms/op
WorkloadResult  24: 128 op, 1239255400.00 ns, 9.6817 ms/op
WorkloadResult  25: 128 op, 602801200.00 ns, 4.7094 ms/op
WorkloadResult  26: 128 op, 647639700.00 ns, 5.0597 ms/op
WorkloadResult  27: 128 op, 624982000.00 ns, 4.8827 ms/op
WorkloadResult  28: 128 op, 622106100.00 ns, 4.8602 ms/op
WorkloadResult  29: 128 op, 632976400.00 ns, 4.9451 ms/op
WorkloadResult  30: 128 op, 1185357900.00 ns, 9.2606 ms/op
WorkloadResult  31: 128 op, 1453832600.00 ns, 11.3581 ms/op
WorkloadResult  32: 128 op, 1477635900.00 ns, 11.5440 ms/op
WorkloadResult  33: 128 op, 1136313100.00 ns, 8.8774 ms/op
WorkloadResult  34: 128 op, 614214800.00 ns, 4.7986 ms/op
WorkloadResult  35: 128 op, 614269800.00 ns, 4.7990 ms/op
WorkloadResult  36: 128 op, 600830300.00 ns, 4.6940 ms/op
WorkloadResult  37: 128 op, 688492300.00 ns, 5.3788 ms/op
WorkloadResult  38: 128 op, 755462400.00 ns, 5.9021 ms/op
WorkloadResult  39: 128 op, 1588490100.00 ns, 12.4101 ms/op
WorkloadResult  40: 128 op, 1407942100.00 ns, 10.9995 ms/op
WorkloadResult  41: 128 op, 1386835400.00 ns, 10.8347 ms/op
WorkloadResult  42: 128 op, 759269000.00 ns, 5.9318 ms/op
WorkloadResult  43: 128 op, 656159300.00 ns, 5.1262 ms/op
WorkloadResult  44: 128 op, 694398500.00 ns, 5.4250 ms/op
WorkloadResult  45: 128 op, 660739100.00 ns, 5.1620 ms/op
WorkloadResult  46: 128 op, 659853500.00 ns, 5.1551 ms/op
WorkloadResult  47: 128 op, 947148800.00 ns, 7.3996 ms/op
WorkloadResult  48: 128 op, 1436796500.00 ns, 11.2250 ms/op
WorkloadResult  49: 128 op, 1492846500.00 ns, 11.6629 ms/op
WorkloadResult  50: 128 op, 1320990800.00 ns, 10.3202 ms/op
WorkloadResult  51: 128 op, 636347700.00 ns, 4.9715 ms/op
WorkloadResult  52: 128 op, 694996400.00 ns, 5.4297 ms/op
WorkloadResult  53: 128 op, 604059700.00 ns, 4.7192 ms/op
WorkloadResult  54: 128 op, 641944700.00 ns, 5.0152 ms/op
WorkloadResult  55: 128 op, 622110300.00 ns, 4.8602 ms/op
WorkloadResult  56: 128 op, 1198022100.00 ns, 9.3595 ms/op
WorkloadResult  57: 128 op, 1378804100.00 ns, 10.7719 ms/op
WorkloadResult  58: 128 op, 1354148000.00 ns, 10.5793 ms/op
WorkloadResult  59: 128 op, 602688100.00 ns, 4.7085 ms/op
WorkloadResult  60: 128 op, 624251900.00 ns, 4.8770 ms/op
WorkloadResult  61: 128 op, 612232300.00 ns, 4.7831 ms/op
WorkloadResult  62: 128 op, 1143379500.00 ns, 8.9327 ms/op
WorkloadResult  63: 128 op, 1193020700.00 ns, 9.3205 ms/op
WorkloadResult  64: 128 op, 1653900700.00 ns, 12.9211 ms/op
WorkloadResult  65: 128 op, 1622888700.00 ns, 12.6788 ms/op
WorkloadResult  66: 128 op, 1013972000.00 ns, 7.9217 ms/op
WorkloadResult  67: 128 op, 597084600.00 ns, 4.6647 ms/op
WorkloadResult  68: 128 op, 597507400.00 ns, 4.6680 ms/op
WorkloadResult  69: 128 op, 646167400.00 ns, 5.0482 ms/op
WorkloadResult  70: 128 op, 633594600.00 ns, 4.9500 ms/op
WorkloadResult  71: 128 op, 610500100.00 ns, 4.7695 ms/op
WorkloadResult  72: 128 op, 1335865000.00 ns, 10.4364 ms/op
WorkloadResult  73: 128 op, 1378182900.00 ns, 10.7671 ms/op
WorkloadResult  74: 128 op, 1415747400.00 ns, 11.0605 ms/op
WorkloadResult  75: 128 op, 1073479500.00 ns, 8.3866 ms/op
WorkloadResult  76: 128 op, 601527200.00 ns, 4.6994 ms/op
WorkloadResult  77: 128 op, 654327900.00 ns, 5.1119 ms/op
WorkloadResult  78: 128 op, 819632500.00 ns, 6.4034 ms/op
WorkloadResult  79: 128 op, 800872200.00 ns, 6.2568 ms/op
WorkloadResult  80: 128 op, 1543301700.00 ns, 12.0570 ms/op
WorkloadResult  81: 128 op, 1524121900.00 ns, 11.9072 ms/op
WorkloadResult  82: 128 op, 1483486200.00 ns, 11.5897 ms/op
WorkloadResult  83: 128 op, 953841300.00 ns, 7.4519 ms/op
WorkloadResult  84: 128 op, 695913600.00 ns, 5.4368 ms/op
WorkloadResult  85: 128 op, 687286300.00 ns, 5.3694 ms/op
WorkloadResult  86: 128 op, 651145600.00 ns, 5.0871 ms/op
WorkloadResult  87: 128 op, 628132900.00 ns, 4.9073 ms/op
WorkloadResult  88: 128 op, 1026896000.00 ns, 8.0226 ms/op
WorkloadResult  89: 128 op, 1483390000.00 ns, 11.5890 ms/op
WorkloadResult  90: 128 op, 1603616300.00 ns, 12.5283 ms/op
WorkloadResult  91: 128 op, 1195336700.00 ns, 9.3386 ms/op
WorkloadResult  92: 128 op, 620008500.00 ns, 4.8438 ms/op
WorkloadResult  93: 128 op, 607032300.00 ns, 4.7424 ms/op
WorkloadResult  94: 128 op, 606847600.00 ns, 4.7410 ms/op
WorkloadResult  95: 128 op, 642680800.00 ns, 5.0209 ms/op
WorkloadResult  96: 128 op, 701277400.00 ns, 5.4787 ms/op
WorkloadResult  97: 128 op, 1435256700.00 ns, 11.2129 ms/op
WorkloadResult  98: 128 op, 1477875600.00 ns, 11.5459 ms/op
WorkloadResult  99: 128 op, 1480392600.00 ns, 11.5656 ms/op
WorkloadResult  100: 128 op, 943036000.00 ns, 7.3675 ms/op

// AfterAll
// Benchmark Process 21468 has exited with code 0.

Mean = 7.467 ms, StdErr = 0.291 ms (3.90%), N = 100, StdDev = 2.910 ms
Min = 4.653 ms, Q1 = 4.901 ms, Median = 5.690 ms, Q3 = 10.626 ms, Max = 12.921 ms
IQR = 5.726 ms, LowerFence = -3.688 ms, UpperFence = 19.215 ms
ConfidenceInterval = [6.480 ms; 8.454 ms] (CI 99.9%), Margin = 0.987 ms (13.22% of Mean)
Skewness = 0.5, Kurtosis = 1.53, MValue = 2.76

// **************************
// Benchmark: WriteText.RunTextWriter: .NET Framework 4.7.2(Runtime=.NET Framework 4.7.2)
// *** Execute ***
// Launch: 1 / 1
// Execute: D:\Microsoft\Workspace\asakura89-project\release\AskScratchpad\CSScratchpad\CSScratchpad\bin\Release\333bbec9-29d9-40ad-b68d-152696ea02b4.exe --benchmarkName "CSScratchpad.Script.WriteText.RunTextWriter" --job ".NET Framework 4.7.2" --benchmarkId 0 in 
// BeforeAnythingElse

// Benchmark Process Environment Information:
// Runtime=.NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
// GC=Concurrent Workstation
// Job: .NET Framework 4.7.2

OverheadJitting  1: 1 op, 310000.00 ns, 310.0000 us/op
WorkloadJitting  1: 1 op, 15615800.00 ns, 15.6158 ms/op

OverheadJitting  2: 16 op, 216300.00 ns, 13.5188 us/op
WorkloadJitting  2: 16 op, 91122900.00 ns, 5.6952 ms/op

WorkloadPilot    1: 16 op, 102479000.00 ns, 6.4049 ms/op
WorkloadPilot    2: 32 op, 219061000.00 ns, 6.8457 ms/op
WorkloadPilot    3: 64 op, 338374900.00 ns, 5.2871 ms/op
WorkloadPilot    4: 128 op, 664453700.00 ns, 5.1910 ms/op

OverheadWarmup   1: 128 op, 3700.00 ns, 28.9063 ns/op
OverheadWarmup   2: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadWarmup   3: 128 op, 1000.00 ns, 7.8125 ns/op
OverheadWarmup   4: 128 op, 600.00 ns, 4.6875 ns/op
OverheadWarmup   5: 128 op, 900.00 ns, 7.0313 ns/op
OverheadWarmup   6: 128 op, 900.00 ns, 7.0313 ns/op
OverheadWarmup   7: 128 op, 800.00 ns, 6.2500 ns/op
OverheadWarmup   8: 128 op, 900.00 ns, 7.0313 ns/op

OverheadActual   1: 128 op, 1200.00 ns, 9.3750 ns/op
OverheadActual   2: 128 op, 1200.00 ns, 9.3750 ns/op
OverheadActual   3: 128 op, 1000.00 ns, 7.8125 ns/op
OverheadActual   4: 128 op, 1500.00 ns, 11.7188 ns/op
OverheadActual   5: 128 op, 900.00 ns, 7.0313 ns/op
OverheadActual   6: 128 op, 1000.00 ns, 7.8125 ns/op
OverheadActual   7: 128 op, 600.00 ns, 4.6875 ns/op
OverheadActual   8: 128 op, 1000.00 ns, 7.8125 ns/op
OverheadActual   9: 128 op, 1000.00 ns, 7.8125 ns/op
OverheadActual  10: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual  11: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadActual  12: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadActual  13: 128 op, 900.00 ns, 7.0313 ns/op
OverheadActual  14: 128 op, 700.00 ns, 5.4688 ns/op
OverheadActual  15: 128 op, 1000.00 ns, 7.8125 ns/op
OverheadActual  16: 128 op, 2000.00 ns, 15.6250 ns/op
OverheadActual  17: 128 op, 2100.00 ns, 16.4063 ns/op
OverheadActual  18: 128 op, 900.00 ns, 7.0313 ns/op
OverheadActual  19: 128 op, 1500.00 ns, 11.7188 ns/op
OverheadActual  20: 128 op, 1300.00 ns, 10.1563 ns/op

WorkloadWarmup   1: 128 op, 666910900.00 ns, 5.2102 ms/op
WorkloadWarmup   2: 128 op, 1417006100.00 ns, 11.0704 ms/op
WorkloadWarmup   3: 128 op, 1542344100.00 ns, 12.0496 ms/op
WorkloadWarmup   4: 128 op, 1471534200.00 ns, 11.4964 ms/op
WorkloadWarmup   5: 128 op, 960087700.00 ns, 7.5007 ms/op
WorkloadWarmup   6: 128 op, 623848100.00 ns, 4.8738 ms/op
WorkloadWarmup   7: 128 op, 619062200.00 ns, 4.8364 ms/op
WorkloadWarmup   8: 128 op, 650997100.00 ns, 5.0859 ms/op
WorkloadWarmup   9: 128 op, 638223100.00 ns, 4.9861 ms/op

// BeforeActualRun
WorkloadActual   1: 128 op, 805672900.00 ns, 6.2943 ms/op
WorkloadActual   2: 128 op, 1471423400.00 ns, 11.4955 ms/op
WorkloadActual   3: 128 op, 1500762100.00 ns, 11.7247 ms/op
WorkloadActual   4: 128 op, 1526018500.00 ns, 11.9220 ms/op
WorkloadActual   5: 128 op, 677884000.00 ns, 5.2960 ms/op
WorkloadActual   6: 128 op, 703554800.00 ns, 5.4965 ms/op
WorkloadActual   7: 128 op, 649827100.00 ns, 5.0768 ms/op
WorkloadActual   8: 128 op, 604101200.00 ns, 4.7195 ms/op
WorkloadActual   9: 128 op, 597279700.00 ns, 4.6662 ms/op
WorkloadActual  10: 128 op, 1155225500.00 ns, 9.0252 ms/op
WorkloadActual  11: 128 op, 1401945600.00 ns, 10.9527 ms/op
WorkloadActual  12: 128 op, 1494869600.00 ns, 11.6787 ms/op
WorkloadActual  13: 128 op, 1158014100.00 ns, 9.0470 ms/op
WorkloadActual  14: 128 op, 611746000.00 ns, 4.7793 ms/op
WorkloadActual  15: 128 op, 621484300.00 ns, 4.8553 ms/op
WorkloadActual  16: 128 op, 637137800.00 ns, 4.9776 ms/op
WorkloadActual  17: 128 op, 612139800.00 ns, 4.7823 ms/op
WorkloadActual  18: 128 op, 638932800.00 ns, 4.9917 ms/op
WorkloadActual  19: 128 op, 1332740900.00 ns, 10.4120 ms/op
WorkloadActual  20: 128 op, 1444141900.00 ns, 11.2824 ms/op
WorkloadActual  21: 128 op, 1498181800.00 ns, 11.7045 ms/op
WorkloadActual  22: 128 op, 1161230200.00 ns, 9.0721 ms/op
WorkloadActual  23: 128 op, 815890400.00 ns, 6.3741 ms/op
WorkloadActual  24: 128 op, 777290200.00 ns, 6.0726 ms/op
WorkloadActual  25: 128 op, 824288700.00 ns, 6.4398 ms/op
WorkloadActual  26: 128 op, 968589600.00 ns, 7.5671 ms/op
WorkloadActual  27: 128 op, 1559837700.00 ns, 12.1862 ms/op
WorkloadActual  28: 128 op, 1893968100.00 ns, 14.7966 ms/op
WorkloadActual  29: 128 op, 1766684600.00 ns, 13.8022 ms/op
WorkloadActual  30: 128 op, 1044522100.00 ns, 8.1603 ms/op
WorkloadActual  31: 128 op, 907751200.00 ns, 7.0918 ms/op
WorkloadActual  32: 128 op, 768558600.00 ns, 6.0044 ms/op
WorkloadActual  33: 128 op, 927561800.00 ns, 7.2466 ms/op
WorkloadActual  34: 128 op, 2192716600.00 ns, 17.1306 ms/op
WorkloadActual  35: 128 op, 2115275100.00 ns, 16.5256 ms/op
WorkloadActual  36: 128 op, 1920292100.00 ns, 15.0023 ms/op
WorkloadActual  37: 128 op, 891531900.00 ns, 6.9651 ms/op
WorkloadActual  38: 128 op, 923059100.00 ns, 7.2114 ms/op
WorkloadActual  39: 128 op, 748753900.00 ns, 5.8496 ms/op
WorkloadActual  40: 128 op, 921903200.00 ns, 7.2024 ms/op
WorkloadActual  41: 128 op, 1711038900.00 ns, 13.3675 ms/op
WorkloadActual  42: 128 op, 1944177700.00 ns, 15.1889 ms/op
WorkloadActual  43: 128 op, 1881475800.00 ns, 14.6990 ms/op
WorkloadActual  44: 128 op, 1018490000.00 ns, 7.9570 ms/op
WorkloadActual  45: 128 op, 821693000.00 ns, 6.4195 ms/op
WorkloadActual  46: 128 op, 693485400.00 ns, 5.4179 ms/op
WorkloadActual  47: 128 op, 885221500.00 ns, 6.9158 ms/op
WorkloadActual  48: 128 op, 1558475400.00 ns, 12.1756 ms/op
WorkloadActual  49: 128 op, 1740639200.00 ns, 13.5987 ms/op
WorkloadActual  50: 128 op, 1472401000.00 ns, 11.5031 ms/op
WorkloadActual  51: 128 op, 1476602700.00 ns, 11.5360 ms/op
WorkloadActual  52: 128 op, 623297700.00 ns, 4.8695 ms/op
WorkloadActual  53: 128 op, 625835900.00 ns, 4.8893 ms/op
WorkloadActual  54: 128 op, 606800800.00 ns, 4.7406 ms/op
WorkloadActual  55: 128 op, 665099900.00 ns, 5.1961 ms/op
WorkloadActual  56: 128 op, 672199200.00 ns, 5.2516 ms/op
WorkloadActual  57: 128 op, 1161672500.00 ns, 9.0756 ms/op
WorkloadActual  58: 128 op, 1609198700.00 ns, 12.5719 ms/op
WorkloadActual  59: 128 op, 1437733800.00 ns, 11.2323 ms/op
WorkloadActual  60: 128 op, 1086361700.00 ns, 8.4872 ms/op
WorkloadActual  61: 128 op, 601014700.00 ns, 4.6954 ms/op
WorkloadActual  62: 128 op, 623034000.00 ns, 4.8675 ms/op
WorkloadActual  63: 128 op, 653721500.00 ns, 5.1072 ms/op
WorkloadActual  64: 128 op, 642023400.00 ns, 5.0158 ms/op
WorkloadActual  65: 128 op, 663655600.00 ns, 5.1848 ms/op
WorkloadActual  66: 128 op, 1519876600.00 ns, 11.8740 ms/op
WorkloadActual  67: 128 op, 1641186700.00 ns, 12.8218 ms/op
WorkloadActual  68: 128 op, 1665729200.00 ns, 13.0135 ms/op
WorkloadActual  69: 128 op, 1337653300.00 ns, 10.4504 ms/op
WorkloadActual  70: 128 op, 663030700.00 ns, 5.1799 ms/op
WorkloadActual  71: 128 op, 622477400.00 ns, 4.8631 ms/op
WorkloadActual  72: 128 op, 609148900.00 ns, 4.7590 ms/op
WorkloadActual  73: 128 op, 624362100.00 ns, 4.8778 ms/op
WorkloadActual  74: 128 op, 636464200.00 ns, 4.9724 ms/op
WorkloadActual  75: 128 op, 1291027900.00 ns, 10.0862 ms/op
WorkloadActual  76: 128 op, 1396135100.00 ns, 10.9073 ms/op
WorkloadActual  77: 128 op, 1592122300.00 ns, 12.4385 ms/op
WorkloadActual  78: 128 op, 1312980600.00 ns, 10.2577 ms/op
WorkloadActual  79: 128 op, 852233700.00 ns, 6.6581 ms/op
WorkloadActual  80: 128 op, 921059400.00 ns, 7.1958 ms/op
WorkloadActual  81: 128 op, 717335800.00 ns, 5.6042 ms/op
WorkloadActual  82: 128 op, 1167568900.00 ns, 9.1216 ms/op
WorkloadActual  83: 128 op, 1503457800.00 ns, 11.7458 ms/op
WorkloadActual  84: 128 op, 1440613200.00 ns, 11.2548 ms/op
WorkloadActual  85: 128 op, 1261226600.00 ns, 9.8533 ms/op
WorkloadActual  86: 128 op, 609990600.00 ns, 4.7656 ms/op
WorkloadActual  87: 128 op, 627808600.00 ns, 4.9048 ms/op
WorkloadActual  88: 128 op, 618111300.00 ns, 4.8290 ms/op
WorkloadActual  89: 128 op, 690976000.00 ns, 5.3983 ms/op
WorkloadActual  90: 128 op, 668121900.00 ns, 5.2197 ms/op
WorkloadActual  91: 128 op, 1411560300.00 ns, 11.0278 ms/op
WorkloadActual  92: 128 op, 1454599800.00 ns, 11.3641 ms/op
WorkloadActual  93: 128 op, 1546891400.00 ns, 12.0851 ms/op
WorkloadActual  94: 128 op, 806739200.00 ns, 6.3027 ms/op
WorkloadActual  95: 128 op, 644589500.00 ns, 5.0359 ms/op
WorkloadActual  96: 128 op, 626958000.00 ns, 4.8981 ms/op
WorkloadActual  97: 128 op, 658387800.00 ns, 5.1437 ms/op
WorkloadActual  98: 128 op, 638809700.00 ns, 4.9907 ms/op
WorkloadActual  99: 128 op, 935121900.00 ns, 7.3056 ms/op
WorkloadActual  100: 128 op, 1588924000.00 ns, 12.4135 ms/op

// AfterActualRun
WorkloadResult   1: 128 op, 805671900.00 ns, 6.2943 ms/op
WorkloadResult   2: 128 op, 1471422400.00 ns, 11.4955 ms/op
WorkloadResult   3: 128 op, 1500761100.00 ns, 11.7247 ms/op
WorkloadResult   4: 128 op, 1526017500.00 ns, 11.9220 ms/op
WorkloadResult   5: 128 op, 677883000.00 ns, 5.2960 ms/op
WorkloadResult   6: 128 op, 703553800.00 ns, 5.4965 ms/op
WorkloadResult   7: 128 op, 649826100.00 ns, 5.0768 ms/op
WorkloadResult   8: 128 op, 604100200.00 ns, 4.7195 ms/op
WorkloadResult   9: 128 op, 597278700.00 ns, 4.6662 ms/op
WorkloadResult  10: 128 op, 1155224500.00 ns, 9.0252 ms/op
WorkloadResult  11: 128 op, 1401944600.00 ns, 10.9527 ms/op
WorkloadResult  12: 128 op, 1494868600.00 ns, 11.6787 ms/op
WorkloadResult  13: 128 op, 1158013100.00 ns, 9.0470 ms/op
WorkloadResult  14: 128 op, 611745000.00 ns, 4.7793 ms/op
WorkloadResult  15: 128 op, 621483300.00 ns, 4.8553 ms/op
WorkloadResult  16: 128 op, 637136800.00 ns, 4.9776 ms/op
WorkloadResult  17: 128 op, 612138800.00 ns, 4.7823 ms/op
WorkloadResult  18: 128 op, 638931800.00 ns, 4.9917 ms/op
WorkloadResult  19: 128 op, 1332739900.00 ns, 10.4120 ms/op
WorkloadResult  20: 128 op, 1444140900.00 ns, 11.2824 ms/op
WorkloadResult  21: 128 op, 1498180800.00 ns, 11.7045 ms/op
WorkloadResult  22: 128 op, 1161229200.00 ns, 9.0721 ms/op
WorkloadResult  23: 128 op, 815889400.00 ns, 6.3741 ms/op
WorkloadResult  24: 128 op, 777289200.00 ns, 6.0726 ms/op
WorkloadResult  25: 128 op, 824287700.00 ns, 6.4397 ms/op
WorkloadResult  26: 128 op, 968588600.00 ns, 7.5671 ms/op
WorkloadResult  27: 128 op, 1559836700.00 ns, 12.1862 ms/op
WorkloadResult  28: 128 op, 1893967100.00 ns, 14.7966 ms/op
WorkloadResult  29: 128 op, 1766683600.00 ns, 13.8022 ms/op
WorkloadResult  30: 128 op, 1044521100.00 ns, 8.1603 ms/op
WorkloadResult  31: 128 op, 907750200.00 ns, 7.0918 ms/op
WorkloadResult  32: 128 op, 768557600.00 ns, 6.0044 ms/op
WorkloadResult  33: 128 op, 927560800.00 ns, 7.2466 ms/op
WorkloadResult  34: 128 op, 2192715600.00 ns, 17.1306 ms/op
WorkloadResult  35: 128 op, 2115274100.00 ns, 16.5256 ms/op
WorkloadResult  36: 128 op, 1920291100.00 ns, 15.0023 ms/op
WorkloadResult  37: 128 op, 891530900.00 ns, 6.9651 ms/op
WorkloadResult  38: 128 op, 923058100.00 ns, 7.2114 ms/op
WorkloadResult  39: 128 op, 748752900.00 ns, 5.8496 ms/op
WorkloadResult  40: 128 op, 921902200.00 ns, 7.2024 ms/op
WorkloadResult  41: 128 op, 1711037900.00 ns, 13.3675 ms/op
WorkloadResult  42: 128 op, 1944176700.00 ns, 15.1889 ms/op
WorkloadResult  43: 128 op, 1881474800.00 ns, 14.6990 ms/op
WorkloadResult  44: 128 op, 1018489000.00 ns, 7.9569 ms/op
WorkloadResult  45: 128 op, 821692000.00 ns, 6.4195 ms/op
WorkloadResult  46: 128 op, 693484400.00 ns, 5.4178 ms/op
WorkloadResult  47: 128 op, 885220500.00 ns, 6.9158 ms/op
WorkloadResult  48: 128 op, 1558474400.00 ns, 12.1756 ms/op
WorkloadResult  49: 128 op, 1740638200.00 ns, 13.5987 ms/op
WorkloadResult  50: 128 op, 1472400000.00 ns, 11.5031 ms/op
WorkloadResult  51: 128 op, 1476601700.00 ns, 11.5360 ms/op
WorkloadResult  52: 128 op, 623296700.00 ns, 4.8695 ms/op
WorkloadResult  53: 128 op, 625834900.00 ns, 4.8893 ms/op
WorkloadResult  54: 128 op, 606799800.00 ns, 4.7406 ms/op
WorkloadResult  55: 128 op, 665098900.00 ns, 5.1961 ms/op
WorkloadResult  56: 128 op, 672198200.00 ns, 5.2515 ms/op
WorkloadResult  57: 128 op, 1161671500.00 ns, 9.0756 ms/op
WorkloadResult  58: 128 op, 1609197700.00 ns, 12.5719 ms/op
WorkloadResult  59: 128 op, 1437732800.00 ns, 11.2323 ms/op
WorkloadResult  60: 128 op, 1086360700.00 ns, 8.4872 ms/op
WorkloadResult  61: 128 op, 601013700.00 ns, 4.6954 ms/op
WorkloadResult  62: 128 op, 623033000.00 ns, 4.8674 ms/op
WorkloadResult  63: 128 op, 653720500.00 ns, 5.1072 ms/op
WorkloadResult  64: 128 op, 642022400.00 ns, 5.0158 ms/op
WorkloadResult  65: 128 op, 663654600.00 ns, 5.1848 ms/op
WorkloadResult  66: 128 op, 1519875600.00 ns, 11.8740 ms/op
WorkloadResult  67: 128 op, 1641185700.00 ns, 12.8218 ms/op
WorkloadResult  68: 128 op, 1665728200.00 ns, 13.0135 ms/op
WorkloadResult  69: 128 op, 1337652300.00 ns, 10.4504 ms/op
WorkloadResult  70: 128 op, 663029700.00 ns, 5.1799 ms/op
WorkloadResult  71: 128 op, 622476400.00 ns, 4.8631 ms/op
WorkloadResult  72: 128 op, 609147900.00 ns, 4.7590 ms/op
WorkloadResult  73: 128 op, 624361100.00 ns, 4.8778 ms/op
WorkloadResult  74: 128 op, 636463200.00 ns, 4.9724 ms/op
WorkloadResult  75: 128 op, 1291026900.00 ns, 10.0861 ms/op
WorkloadResult  76: 128 op, 1396134100.00 ns, 10.9073 ms/op
WorkloadResult  77: 128 op, 1592121300.00 ns, 12.4384 ms/op
WorkloadResult  78: 128 op, 1312979600.00 ns, 10.2577 ms/op
WorkloadResult  79: 128 op, 852232700.00 ns, 6.6581 ms/op
WorkloadResult  80: 128 op, 921058400.00 ns, 7.1958 ms/op
WorkloadResult  81: 128 op, 717334800.00 ns, 5.6042 ms/op
WorkloadResult  82: 128 op, 1167567900.00 ns, 9.1216 ms/op
WorkloadResult  83: 128 op, 1503456800.00 ns, 11.7458 ms/op
WorkloadResult  84: 128 op, 1440612200.00 ns, 11.2548 ms/op
WorkloadResult  85: 128 op, 1261225600.00 ns, 9.8533 ms/op
WorkloadResult  86: 128 op, 609989600.00 ns, 4.7655 ms/op
WorkloadResult  87: 128 op, 627807600.00 ns, 4.9047 ms/op
WorkloadResult  88: 128 op, 618110300.00 ns, 4.8290 ms/op
WorkloadResult  89: 128 op, 690975000.00 ns, 5.3982 ms/op
WorkloadResult  90: 128 op, 668120900.00 ns, 5.2197 ms/op
WorkloadResult  91: 128 op, 1411559300.00 ns, 11.0278 ms/op
WorkloadResult  92: 128 op, 1454598800.00 ns, 11.3641 ms/op
WorkloadResult  93: 128 op, 1546890400.00 ns, 12.0851 ms/op
WorkloadResult  94: 128 op, 806738200.00 ns, 6.3026 ms/op
WorkloadResult  95: 128 op, 644588500.00 ns, 5.0358 ms/op
WorkloadResult  96: 128 op, 626957000.00 ns, 4.8981 ms/op
WorkloadResult  97: 128 op, 658386800.00 ns, 5.1436 ms/op
WorkloadResult  98: 128 op, 638808700.00 ns, 4.9907 ms/op
WorkloadResult  99: 128 op, 935120900.00 ns, 7.3056 ms/op
WorkloadResult  100: 128 op, 1588923000.00 ns, 12.4135 ms/op

// AfterAll
// Benchmark Process 25188 has exited with code 0.

Mean = 8.395 ms, StdErr = 0.343 ms (4.09%), N = 100, StdDev = 3.433 ms
Min = 4.666 ms, Q1 = 5.135 ms, Median = 7.207 ms, Q3 = 11.497 ms, Max = 17.131 ms
IQR = 6.363 ms, LowerFence = -4.410 ms, UpperFence = 21.042 ms
ConfidenceInterval = [7.230 ms; 9.559 ms] (CI 99.9%), Margin = 1.164 ms (13.87% of Mean)
Skewness = 0.56, Kurtosis = 2.06, MValue = 3.27

// **************************
// Benchmark: WriteText.RunFileStream: .NET Framework 4.7.2(Runtime=.NET Framework 4.7.2)
// *** Execute ***
// Launch: 1 / 1
// Execute: D:\Microsoft\Workspace\asakura89-project\release\AskScratchpad\CSScratchpad\CSScratchpad\bin\Release\333bbec9-29d9-40ad-b68d-152696ea02b4.exe --benchmarkName "CSScratchpad.Script.WriteText.RunFileStream" --job ".NET Framework 4.7.2" --benchmarkId 1 in 
// BeforeAnythingElse

// Benchmark Process Environment Information:
// Runtime=.NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
// GC=Concurrent Workstation
// Job: .NET Framework 4.7.2

OverheadJitting  1: 1 op, 623200.00 ns, 623.2000 us/op
WorkloadJitting  1: 1 op, 32567500.00 ns, 32.5675 ms/op

OverheadJitting  2: 16 op, 347700.00 ns, 21.7313 us/op
WorkloadJitting  2: 16 op, 179142200.00 ns, 11.1964 ms/op

WorkloadPilot    1: 16 op, 172576600.00 ns, 10.7860 ms/op
WorkloadPilot    2: 32 op, 345175600.00 ns, 10.7867 ms/op
WorkloadPilot    3: 64 op, 715738100.00 ns, 11.1834 ms/op

OverheadWarmup   1: 64 op, 3200.00 ns, 50.0000 ns/op
OverheadWarmup   2: 64 op, 900.00 ns, 14.0625 ns/op
OverheadWarmup   3: 64 op, 1100.00 ns, 17.1875 ns/op
OverheadWarmup   4: 64 op, 800.00 ns, 12.5000 ns/op
OverheadWarmup   5: 64 op, 1100.00 ns, 17.1875 ns/op
OverheadWarmup   6: 64 op, 800.00 ns, 12.5000 ns/op

OverheadActual   1: 64 op, 1000.00 ns, 15.6250 ns/op
OverheadActual   2: 64 op, 800.00 ns, 12.5000 ns/op
OverheadActual   3: 64 op, 800.00 ns, 12.5000 ns/op
OverheadActual   4: 64 op, 1400.00 ns, 21.8750 ns/op
OverheadActual   5: 64 op, 900.00 ns, 14.0625 ns/op
OverheadActual   6: 64 op, 2700.00 ns, 42.1875 ns/op
OverheadActual   7: 64 op, 800.00 ns, 12.5000 ns/op
OverheadActual   8: 64 op, 1300.00 ns, 20.3125 ns/op
OverheadActual   9: 64 op, 1500.00 ns, 23.4375 ns/op
OverheadActual  10: 64 op, 4200.00 ns, 65.6250 ns/op
OverheadActual  11: 64 op, 1300.00 ns, 20.3125 ns/op
OverheadActual  12: 64 op, 900.00 ns, 14.0625 ns/op
OverheadActual  13: 64 op, 1300.00 ns, 20.3125 ns/op
OverheadActual  14: 64 op, 800.00 ns, 12.5000 ns/op
OverheadActual  15: 64 op, 800.00 ns, 12.5000 ns/op
OverheadActual  16: 64 op, 1300.00 ns, 20.3125 ns/op
OverheadActual  17: 64 op, 800.00 ns, 12.5000 ns/op
OverheadActual  18: 64 op, 500.00 ns, 7.8125 ns/op
OverheadActual  19: 64 op, 1100.00 ns, 17.1875 ns/op
OverheadActual  20: 64 op, 700.00 ns, 10.9375 ns/op

WorkloadWarmup   1: 64 op, 317122100.00 ns, 4.9550 ms/op
WorkloadWarmup   2: 64 op, 313090500.00 ns, 4.8920 ms/op
WorkloadWarmup   3: 64 op, 315157500.00 ns, 4.9243 ms/op
WorkloadWarmup   4: 64 op, 314561600.00 ns, 4.9150 ms/op
WorkloadWarmup   5: 64 op, 307810900.00 ns, 4.8095 ms/op
WorkloadWarmup   6: 64 op, 296852100.00 ns, 4.6383 ms/op
WorkloadWarmup   7: 64 op, 340063400.00 ns, 5.3135 ms/op
WorkloadWarmup   8: 64 op, 318143500.00 ns, 4.9710 ms/op

// BeforeActualRun
WorkloadActual   1: 64 op, 298584300.00 ns, 4.6654 ms/op
WorkloadActual   2: 64 op, 328509700.00 ns, 5.1330 ms/op
WorkloadActual   3: 64 op, 397340700.00 ns, 6.2084 ms/op
WorkloadActual   4: 64 op, 789249000.00 ns, 12.3320 ms/op
WorkloadActual   5: 64 op, 729182600.00 ns, 11.3935 ms/op
WorkloadActual   6: 64 op, 691084800.00 ns, 10.7982 ms/op
WorkloadActual   7: 64 op, 737750600.00 ns, 11.5274 ms/op
WorkloadActual   8: 64 op, 722909300.00 ns, 11.2955 ms/op
WorkloadActual   9: 64 op, 711471000.00 ns, 11.1167 ms/op
WorkloadActual  10: 64 op, 416428400.00 ns, 6.5067 ms/op
WorkloadActual  11: 64 op, 308697800.00 ns, 4.8234 ms/op
WorkloadActual  12: 64 op, 312232800.00 ns, 4.8786 ms/op
WorkloadActual  13: 64 op, 304510200.00 ns, 4.7580 ms/op
WorkloadActual  14: 64 op, 333197100.00 ns, 5.2062 ms/op
WorkloadActual  15: 64 op, 353557300.00 ns, 5.5243 ms/op
WorkloadActual  16: 64 op, 347698100.00 ns, 5.4328 ms/op
WorkloadActual  17: 64 op, 356013800.00 ns, 5.5627 ms/op
WorkloadActual  18: 64 op, 306769500.00 ns, 4.7933 ms/op
WorkloadActual  19: 64 op, 308918600.00 ns, 4.8269 ms/op
WorkloadActual  20: 64 op, 329484100.00 ns, 5.1482 ms/op
WorkloadActual  21: 64 op, 610512600.00 ns, 9.5393 ms/op
WorkloadActual  22: 64 op, 749363700.00 ns, 11.7088 ms/op
WorkloadActual  23: 64 op, 814184600.00 ns, 12.7216 ms/op
WorkloadActual  24: 64 op, 746292000.00 ns, 11.6608 ms/op
WorkloadActual  25: 64 op, 715055300.00 ns, 11.1727 ms/op
WorkloadActual  26: 64 op, 718004500.00 ns, 11.2188 ms/op
WorkloadActual  27: 64 op, 683362700.00 ns, 10.6775 ms/op
WorkloadActual  28: 64 op, 750708500.00 ns, 11.7298 ms/op
WorkloadActual  29: 64 op, 403530900.00 ns, 6.3052 ms/op
WorkloadActual  30: 64 op, 304212000.00 ns, 4.7533 ms/op
WorkloadActual  31: 64 op, 325919300.00 ns, 5.0925 ms/op
WorkloadActual  32: 64 op, 334088900.00 ns, 5.2201 ms/op
WorkloadActual  33: 64 op, 312849100.00 ns, 4.8883 ms/op
WorkloadActual  34: 64 op, 306457600.00 ns, 4.7884 ms/op
WorkloadActual  35: 64 op, 314527700.00 ns, 4.9145 ms/op
WorkloadActual  36: 64 op, 320048500.00 ns, 5.0008 ms/op
WorkloadActual  37: 64 op, 352924300.00 ns, 5.5144 ms/op
WorkloadActual  38: 64 op, 336407700.00 ns, 5.2564 ms/op
WorkloadActual  39: 64 op, 310075700.00 ns, 4.8449 ms/op
WorkloadActual  40: 64 op, 714523000.00 ns, 11.1644 ms/op
WorkloadActual  41: 64 op, 713747100.00 ns, 11.1523 ms/op
WorkloadActual  42: 64 op, 721674300.00 ns, 11.2762 ms/op
WorkloadActual  43: 64 op, 697699400.00 ns, 10.9016 ms/op
WorkloadActual  44: 64 op, 872180400.00 ns, 13.6278 ms/op
WorkloadActual  45: 64 op, 707581500.00 ns, 11.0560 ms/op
WorkloadActual  46: 64 op, 513676300.00 ns, 8.0262 ms/op
WorkloadActual  47: 64 op, 306624200.00 ns, 4.7910 ms/op
WorkloadActual  48: 64 op, 335328000.00 ns, 5.2395 ms/op
WorkloadActual  49: 64 op, 336765200.00 ns, 5.2620 ms/op
WorkloadActual  50: 64 op, 295111300.00 ns, 4.6111 ms/op
WorkloadActual  51: 64 op, 314697300.00 ns, 4.9171 ms/op
WorkloadActual  52: 64 op, 338467000.00 ns, 5.2885 ms/op
WorkloadActual  53: 64 op, 333833500.00 ns, 5.2161 ms/op
WorkloadActual  54: 64 op, 347932200.00 ns, 5.4364 ms/op
WorkloadActual  55: 64 op, 319952200.00 ns, 4.9993 ms/op
WorkloadActual  56: 64 op, 304239100.00 ns, 4.7537 ms/op
WorkloadActual  57: 64 op, 649578900.00 ns, 10.1497 ms/op
WorkloadActual  58: 64 op, 743766700.00 ns, 11.6214 ms/op
WorkloadActual  59: 64 op, 698853700.00 ns, 10.9196 ms/op
WorkloadActual  60: 64 op, 818644800.00 ns, 12.7913 ms/op
WorkloadActual  61: 64 op, 688080800.00 ns, 10.7513 ms/op
WorkloadActual  62: 64 op, 713864900.00 ns, 11.1541 ms/op
WorkloadActual  63: 64 op, 698999200.00 ns, 10.9219 ms/op
WorkloadActual  64: 64 op, 335338100.00 ns, 5.2397 ms/op
WorkloadActual  65: 64 op, 347002100.00 ns, 5.4219 ms/op
WorkloadActual  66: 64 op, 297380600.00 ns, 4.6466 ms/op
WorkloadActual  67: 64 op, 322050800.00 ns, 5.0320 ms/op
WorkloadActual  68: 64 op, 301879800.00 ns, 4.7169 ms/op
WorkloadActual  69: 64 op, 317085000.00 ns, 4.9545 ms/op
WorkloadActual  70: 64 op, 314068500.00 ns, 4.9073 ms/op
WorkloadActual  71: 64 op, 326903600.00 ns, 5.1079 ms/op
WorkloadActual  72: 64 op, 346341400.00 ns, 5.4116 ms/op
WorkloadActual  73: 64 op, 305931100.00 ns, 4.7802 ms/op
WorkloadActual  74: 64 op, 554999700.00 ns, 8.6719 ms/op
WorkloadActual  75: 64 op, 690479000.00 ns, 10.7887 ms/op
WorkloadActual  76: 64 op, 769626100.00 ns, 12.0254 ms/op
WorkloadActual  77: 64 op, 732071800.00 ns, 11.4386 ms/op
WorkloadActual  78: 64 op, 785552700.00 ns, 12.2743 ms/op
WorkloadActual  79: 64 op, 648820700.00 ns, 10.1378 ms/op
WorkloadActual  80: 64 op, 689323900.00 ns, 10.7707 ms/op
WorkloadActual  81: 64 op, 304720400.00 ns, 4.7613 ms/op
WorkloadActual  82: 64 op, 323518300.00 ns, 5.0550 ms/op
WorkloadActual  83: 64 op, 327018500.00 ns, 5.1097 ms/op
WorkloadActual  84: 64 op, 358065900.00 ns, 5.5948 ms/op
WorkloadActual  85: 64 op, 399404100.00 ns, 6.2407 ms/op
WorkloadActual  86: 64 op, 297264200.00 ns, 4.6448 ms/op
WorkloadActual  87: 64 op, 306523700.00 ns, 4.7894 ms/op
WorkloadActual  88: 64 op, 326501800.00 ns, 5.1016 ms/op
WorkloadActual  89: 64 op, 313609200.00 ns, 4.9001 ms/op
WorkloadActual  90: 64 op, 302523600.00 ns, 4.7269 ms/op
WorkloadActual  91: 64 op, 426596800.00 ns, 6.6656 ms/op
WorkloadActual  92: 64 op, 681143300.00 ns, 10.6429 ms/op
WorkloadActual  93: 64 op, 794159900.00 ns, 12.4087 ms/op
WorkloadActual  94: 64 op, 777333700.00 ns, 12.1458 ms/op
WorkloadActual  95: 64 op, 732307500.00 ns, 11.4423 ms/op
WorkloadActual  96: 64 op, 678978600.00 ns, 10.6090 ms/op
WorkloadActual  97: 64 op, 705995200.00 ns, 11.0312 ms/op
WorkloadActual  98: 64 op, 418055700.00 ns, 6.5321 ms/op
WorkloadActual  99: 64 op, 311196900.00 ns, 4.8625 ms/op
WorkloadActual  100: 64 op, 303399800.00 ns, 4.7406 ms/op

// AfterActualRun
WorkloadResult   1: 64 op, 298583350.00 ns, 4.6654 ms/op
WorkloadResult   2: 64 op, 328508750.00 ns, 5.1329 ms/op
WorkloadResult   3: 64 op, 397339750.00 ns, 6.2084 ms/op
WorkloadResult   4: 64 op, 789248050.00 ns, 12.3320 ms/op
WorkloadResult   5: 64 op, 729181650.00 ns, 11.3935 ms/op
WorkloadResult   6: 64 op, 691083850.00 ns, 10.7982 ms/op
WorkloadResult   7: 64 op, 737749650.00 ns, 11.5273 ms/op
WorkloadResult   8: 64 op, 722908350.00 ns, 11.2954 ms/op
WorkloadResult   9: 64 op, 711470050.00 ns, 11.1167 ms/op
WorkloadResult  10: 64 op, 416427450.00 ns, 6.5067 ms/op
WorkloadResult  11: 64 op, 308696850.00 ns, 4.8234 ms/op
WorkloadResult  12: 64 op, 312231850.00 ns, 4.8786 ms/op
WorkloadResult  13: 64 op, 304509250.00 ns, 4.7580 ms/op
WorkloadResult  14: 64 op, 333196150.00 ns, 5.2062 ms/op
WorkloadResult  15: 64 op, 353556350.00 ns, 5.5243 ms/op
WorkloadResult  16: 64 op, 347697150.00 ns, 5.4328 ms/op
WorkloadResult  17: 64 op, 356012850.00 ns, 5.5627 ms/op
WorkloadResult  18: 64 op, 306768550.00 ns, 4.7933 ms/op
WorkloadResult  19: 64 op, 308917650.00 ns, 4.8268 ms/op
WorkloadResult  20: 64 op, 329483150.00 ns, 5.1482 ms/op
WorkloadResult  21: 64 op, 610511650.00 ns, 9.5392 ms/op
WorkloadResult  22: 64 op, 749362750.00 ns, 11.7088 ms/op
WorkloadResult  23: 64 op, 814183650.00 ns, 12.7216 ms/op
WorkloadResult  24: 64 op, 746291050.00 ns, 11.6608 ms/op
WorkloadResult  25: 64 op, 715054350.00 ns, 11.1727 ms/op
WorkloadResult  26: 64 op, 718003550.00 ns, 11.2188 ms/op
WorkloadResult  27: 64 op, 683361750.00 ns, 10.6775 ms/op
WorkloadResult  28: 64 op, 750707550.00 ns, 11.7298 ms/op
WorkloadResult  29: 64 op, 403529950.00 ns, 6.3052 ms/op
WorkloadResult  30: 64 op, 304211050.00 ns, 4.7533 ms/op
WorkloadResult  31: 64 op, 325918350.00 ns, 5.0925 ms/op
WorkloadResult  32: 64 op, 334087950.00 ns, 5.2201 ms/op
WorkloadResult  33: 64 op, 312848150.00 ns, 4.8883 ms/op
WorkloadResult  34: 64 op, 306456650.00 ns, 4.7884 ms/op
WorkloadResult  35: 64 op, 314526750.00 ns, 4.9145 ms/op
WorkloadResult  36: 64 op, 320047550.00 ns, 5.0007 ms/op
WorkloadResult  37: 64 op, 352923350.00 ns, 5.5144 ms/op
WorkloadResult  38: 64 op, 336406750.00 ns, 5.2564 ms/op
WorkloadResult  39: 64 op, 310074750.00 ns, 4.8449 ms/op
WorkloadResult  40: 64 op, 714522050.00 ns, 11.1644 ms/op
WorkloadResult  41: 64 op, 713746150.00 ns, 11.1523 ms/op
WorkloadResult  42: 64 op, 721673350.00 ns, 11.2761 ms/op
WorkloadResult  43: 64 op, 697698450.00 ns, 10.9015 ms/op
WorkloadResult  44: 64 op, 872179450.00 ns, 13.6278 ms/op
WorkloadResult  45: 64 op, 707580550.00 ns, 11.0559 ms/op
WorkloadResult  46: 64 op, 513675350.00 ns, 8.0262 ms/op
WorkloadResult  47: 64 op, 306623250.00 ns, 4.7910 ms/op
WorkloadResult  48: 64 op, 335327050.00 ns, 5.2395 ms/op
WorkloadResult  49: 64 op, 336764250.00 ns, 5.2619 ms/op
WorkloadResult  50: 64 op, 295110350.00 ns, 4.6111 ms/op
WorkloadResult  51: 64 op, 314696350.00 ns, 4.9171 ms/op
WorkloadResult  52: 64 op, 338466050.00 ns, 5.2885 ms/op
WorkloadResult  53: 64 op, 333832550.00 ns, 5.2161 ms/op
WorkloadResult  54: 64 op, 347931250.00 ns, 5.4364 ms/op
WorkloadResult  55: 64 op, 319951250.00 ns, 4.9992 ms/op
WorkloadResult  56: 64 op, 304238150.00 ns, 4.7537 ms/op
WorkloadResult  57: 64 op, 649577950.00 ns, 10.1497 ms/op
WorkloadResult  58: 64 op, 743765750.00 ns, 11.6213 ms/op
WorkloadResult  59: 64 op, 698852750.00 ns, 10.9196 ms/op
WorkloadResult  60: 64 op, 818643850.00 ns, 12.7913 ms/op
WorkloadResult  61: 64 op, 688079850.00 ns, 10.7512 ms/op
WorkloadResult  62: 64 op, 713863950.00 ns, 11.1541 ms/op
WorkloadResult  63: 64 op, 698998250.00 ns, 10.9218 ms/op
WorkloadResult  64: 64 op, 335337150.00 ns, 5.2396 ms/op
WorkloadResult  65: 64 op, 347001150.00 ns, 5.4219 ms/op
WorkloadResult  66: 64 op, 297379650.00 ns, 4.6466 ms/op
WorkloadResult  67: 64 op, 322049850.00 ns, 5.0320 ms/op
WorkloadResult  68: 64 op, 301878850.00 ns, 4.7169 ms/op
WorkloadResult  69: 64 op, 317084050.00 ns, 4.9544 ms/op
WorkloadResult  70: 64 op, 314067550.00 ns, 4.9073 ms/op
WorkloadResult  71: 64 op, 326902650.00 ns, 5.1079 ms/op
WorkloadResult  72: 64 op, 346340450.00 ns, 5.4116 ms/op
WorkloadResult  73: 64 op, 305930150.00 ns, 4.7802 ms/op
WorkloadResult  74: 64 op, 554998750.00 ns, 8.6719 ms/op
WorkloadResult  75: 64 op, 690478050.00 ns, 10.7887 ms/op
WorkloadResult  76: 64 op, 769625150.00 ns, 12.0254 ms/op
WorkloadResult  77: 64 op, 732070850.00 ns, 11.4386 ms/op
WorkloadResult  78: 64 op, 785551750.00 ns, 12.2742 ms/op
WorkloadResult  79: 64 op, 648819750.00 ns, 10.1378 ms/op
WorkloadResult  80: 64 op, 689322950.00 ns, 10.7707 ms/op
WorkloadResult  81: 64 op, 304719450.00 ns, 4.7612 ms/op
WorkloadResult  82: 64 op, 323517350.00 ns, 5.0550 ms/op
WorkloadResult  83: 64 op, 327017550.00 ns, 5.1096 ms/op
WorkloadResult  84: 64 op, 358064950.00 ns, 5.5948 ms/op
WorkloadResult  85: 64 op, 399403150.00 ns, 6.2407 ms/op
WorkloadResult  86: 64 op, 297263250.00 ns, 4.6447 ms/op
WorkloadResult  87: 64 op, 306522750.00 ns, 4.7894 ms/op
WorkloadResult  88: 64 op, 326500850.00 ns, 5.1016 ms/op
WorkloadResult  89: 64 op, 313608250.00 ns, 4.9001 ms/op
WorkloadResult  90: 64 op, 302522650.00 ns, 4.7269 ms/op
WorkloadResult  91: 64 op, 426595850.00 ns, 6.6656 ms/op
WorkloadResult  92: 64 op, 681142350.00 ns, 10.6428 ms/op
WorkloadResult  93: 64 op, 794158950.00 ns, 12.4087 ms/op
WorkloadResult  94: 64 op, 777332750.00 ns, 12.1458 ms/op
WorkloadResult  95: 64 op, 732306550.00 ns, 11.4423 ms/op
WorkloadResult  96: 64 op, 678977650.00 ns, 10.6090 ms/op
WorkloadResult  97: 64 op, 705994250.00 ns, 11.0312 ms/op
WorkloadResult  98: 64 op, 418054750.00 ns, 6.5321 ms/op
WorkloadResult  99: 64 op, 311195950.00 ns, 4.8624 ms/op
WorkloadResult  100: 64 op, 303398850.00 ns, 4.7406 ms/op

// AfterAll
// Benchmark Process 3484 has exited with code 0.

Mean = 7.633 ms, StdErr = 0.307 ms (4.03%), N = 100, StdDev = 3.072 ms
Min = 4.611 ms, Q1 = 4.916 ms, Median = 5.519 ms, Q3 = 11.037 ms, Max = 13.628 ms
IQR = 6.121 ms, LowerFence = -4.265 ms, UpperFence = 20.219 ms
ConfidenceInterval = [6.591 ms; 8.675 ms] (CI 99.9%), Margin = 1.042 ms (13.65% of Mean)
Skewness = 0.44, Kurtosis = 1.36, MValue = 3.23

// **************************
// Benchmark: WriteText.RunFile: .NET Framework 4.7.2(Runtime=.NET Framework 4.7.2)
// *** Execute ***
// Launch: 1 / 1
// Execute: D:\Microsoft\Workspace\asakura89-project\release\AskScratchpad\CSScratchpad\CSScratchpad\bin\Release\333bbec9-29d9-40ad-b68d-152696ea02b4.exe --benchmarkName "CSScratchpad.Script.WriteText.RunFile" --job ".NET Framework 4.7.2" --benchmarkId 2 in 
// BeforeAnythingElse

// Benchmark Process Environment Information:
// Runtime=.NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
// GC=Concurrent Workstation
// Job: .NET Framework 4.7.2

OverheadJitting  1: 1 op, 306200.00 ns, 306.2000 us/op
WorkloadJitting  1: 1 op, 14350900.00 ns, 14.3509 ms/op

OverheadJitting  2: 16 op, 149900.00 ns, 9.3688 us/op
WorkloadJitting  2: 16 op, 89153400.00 ns, 5.5721 ms/op

WorkloadPilot    1: 16 op, 80371700.00 ns, 5.0232 ms/op
WorkloadPilot    2: 32 op, 159613900.00 ns, 4.9879 ms/op
WorkloadPilot    3: 64 op, 318429500.00 ns, 4.9755 ms/op
WorkloadPilot    4: 128 op, 599640900.00 ns, 4.6847 ms/op

OverheadWarmup   1: 128 op, 4900.00 ns, 38.2813 ns/op
OverheadWarmup   2: 128 op, 1000.00 ns, 7.8125 ns/op
OverheadWarmup   3: 128 op, 1000.00 ns, 7.8125 ns/op
OverheadWarmup   4: 128 op, 900.00 ns, 7.0313 ns/op
OverheadWarmup   5: 128 op, 900.00 ns, 7.0313 ns/op
OverheadWarmup   6: 128 op, 1000.00 ns, 7.8125 ns/op

OverheadActual   1: 128 op, 900.00 ns, 7.0313 ns/op
OverheadActual   2: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual   3: 128 op, 700.00 ns, 5.4688 ns/op
OverheadActual   4: 128 op, 900.00 ns, 7.0313 ns/op
OverheadActual   5: 128 op, 1900.00 ns, 14.8438 ns/op
OverheadActual   6: 128 op, 1000.00 ns, 7.8125 ns/op
OverheadActual   7: 128 op, 900.00 ns, 7.0313 ns/op
OverheadActual   8: 128 op, 500.00 ns, 3.9063 ns/op
OverheadActual   9: 128 op, 500.00 ns, 3.9063 ns/op
OverheadActual  10: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual  11: 128 op, 700.00 ns, 5.4688 ns/op
OverheadActual  12: 128 op, 1000.00 ns, 7.8125 ns/op
OverheadActual  13: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual  14: 128 op, 2000.00 ns, 15.6250 ns/op
OverheadActual  15: 128 op, 600.00 ns, 4.6875 ns/op
OverheadActual  16: 128 op, 600.00 ns, 4.6875 ns/op
OverheadActual  17: 128 op, 600.00 ns, 4.6875 ns/op
OverheadActual  18: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadActual  19: 128 op, 600.00 ns, 4.6875 ns/op
OverheadActual  20: 128 op, 800.00 ns, 6.2500 ns/op

WorkloadWarmup   1: 128 op, 771329800.00 ns, 6.0260 ms/op
WorkloadWarmup   2: 128 op, 1249010600.00 ns, 9.7579 ms/op
WorkloadWarmup   3: 128 op, 1475755100.00 ns, 11.5293 ms/op
WorkloadWarmup   4: 128 op, 1474883600.00 ns, 11.5225 ms/op
WorkloadWarmup   5: 128 op, 1050359600.00 ns, 8.2059 ms/op
WorkloadWarmup   6: 128 op, 612969100.00 ns, 4.7888 ms/op
WorkloadWarmup   7: 128 op, 652110500.00 ns, 5.0946 ms/op
WorkloadWarmup   8: 128 op, 634728100.00 ns, 4.9588 ms/op

// BeforeActualRun
WorkloadActual   1: 128 op, 598072800.00 ns, 4.6724 ms/op
WorkloadActual   2: 128 op, 751343100.00 ns, 5.8699 ms/op
WorkloadActual   3: 128 op, 1400959400.00 ns, 10.9450 ms/op
WorkloadActual   4: 128 op, 1479154400.00 ns, 11.5559 ms/op
WorkloadActual   5: 128 op, 1613371700.00 ns, 12.6045 ms/op
WorkloadActual   6: 128 op, 745453800.00 ns, 5.8239 ms/op
WorkloadActual   7: 128 op, 596248200.00 ns, 4.6582 ms/op
WorkloadActual   8: 128 op, 656307900.00 ns, 5.1274 ms/op
WorkloadActual   9: 128 op, 612934300.00 ns, 4.7885 ms/op
WorkloadActual  10: 128 op, 620039600.00 ns, 4.8441 ms/op
WorkloadActual  11: 128 op, 972117400.00 ns, 7.5947 ms/op
WorkloadActual  12: 128 op, 1329678400.00 ns, 10.3881 ms/op
WorkloadActual  13: 128 op, 1435617500.00 ns, 11.2158 ms/op
WorkloadActual  14: 128 op, 1463933300.00 ns, 11.4370 ms/op
WorkloadActual  15: 128 op, 619375300.00 ns, 4.8389 ms/op
WorkloadActual  16: 128 op, 743103800.00 ns, 5.8055 ms/op
WorkloadActual  17: 128 op, 641258400.00 ns, 5.0098 ms/op
WorkloadActual  18: 128 op, 631013100.00 ns, 4.9298 ms/op
WorkloadActual  19: 128 op, 613669000.00 ns, 4.7943 ms/op
WorkloadActual  20: 128 op, 1128494600.00 ns, 8.8164 ms/op
WorkloadActual  21: 128 op, 1421446900.00 ns, 11.1051 ms/op
WorkloadActual  22: 128 op, 1445929700.00 ns, 11.2963 ms/op
WorkloadActual  23: 128 op, 1171043000.00 ns, 9.1488 ms/op
WorkloadActual  24: 128 op, 607174700.00 ns, 4.7436 ms/op
WorkloadActual  25: 128 op, 614573000.00 ns, 4.8014 ms/op
WorkloadActual  26: 128 op, 611345000.00 ns, 4.7761 ms/op
WorkloadActual  27: 128 op, 640140500.00 ns, 5.0011 ms/op
WorkloadActual  28: 128 op, 685746300.00 ns, 5.3574 ms/op
WorkloadActual  29: 128 op, 1270852100.00 ns, 9.9285 ms/op
WorkloadActual  30: 128 op, 1456742500.00 ns, 11.3808 ms/op
WorkloadActual  31: 128 op, 1615414200.00 ns, 12.6204 ms/op
WorkloadActual  32: 128 op, 1481952900.00 ns, 11.5778 ms/op
WorkloadActual  33: 128 op, 750333600.00 ns, 5.8620 ms/op
WorkloadActual  34: 128 op, 627407400.00 ns, 4.9016 ms/op
WorkloadActual  35: 128 op, 672378800.00 ns, 5.2530 ms/op
WorkloadActual  36: 128 op, 651863100.00 ns, 5.0927 ms/op
WorkloadActual  37: 128 op, 615686400.00 ns, 4.8101 ms/op
WorkloadActual  38: 128 op, 1055161200.00 ns, 8.2434 ms/op
WorkloadActual  39: 128 op, 1673940400.00 ns, 13.0777 ms/op
WorkloadActual  40: 128 op, 1372227200.00 ns, 10.7205 ms/op
WorkloadActual  41: 128 op, 1371981400.00 ns, 10.7186 ms/op
WorkloadActual  42: 128 op, 1112154900.00 ns, 8.6887 ms/op
WorkloadActual  43: 128 op, 610274800.00 ns, 4.7678 ms/op
WorkloadActual  44: 128 op, 619304700.00 ns, 4.8383 ms/op
WorkloadActual  45: 128 op, 658177600.00 ns, 5.1420 ms/op
WorkloadActual  46: 128 op, 597600800.00 ns, 4.6688 ms/op
WorkloadActual  47: 128 op, 632951000.00 ns, 4.9449 ms/op
WorkloadActual  48: 128 op, 1244745100.00 ns, 9.7246 ms/op
WorkloadActual  49: 128 op, 1480827800.00 ns, 11.5690 ms/op
WorkloadActual  50: 128 op, 1519475100.00 ns, 11.8709 ms/op
WorkloadActual  51: 128 op, 986769700.00 ns, 7.7091 ms/op
WorkloadActual  52: 128 op, 605574400.00 ns, 4.7311 ms/op
WorkloadActual  53: 128 op, 621515100.00 ns, 4.8556 ms/op
WorkloadActual  54: 128 op, 679212400.00 ns, 5.3063 ms/op
WorkloadActual  55: 128 op, 603873900.00 ns, 4.7178 ms/op
WorkloadActual  56: 128 op, 641268900.00 ns, 5.0099 ms/op
WorkloadActual  57: 128 op, 1378401300.00 ns, 10.7688 ms/op
WorkloadActual  58: 128 op, 1373408000.00 ns, 10.7298 ms/op
WorkloadActual  59: 128 op, 1414514500.00 ns, 11.0509 ms/op
WorkloadActual  60: 128 op, 981900000.00 ns, 7.6711 ms/op
WorkloadActual  61: 128 op, 587689200.00 ns, 4.5913 ms/op
WorkloadActual  62: 128 op, 625359300.00 ns, 4.8856 ms/op
WorkloadActual  63: 128 op, 643448600.00 ns, 5.0269 ms/op
WorkloadActual  64: 128 op, 612722700.00 ns, 4.7869 ms/op
WorkloadActual  65: 128 op, 806349000.00 ns, 6.2996 ms/op
WorkloadActual  66: 128 op, 1378452900.00 ns, 10.7692 ms/op
WorkloadActual  67: 128 op, 1424908000.00 ns, 11.1321 ms/op
WorkloadActual  68: 128 op, 1506712000.00 ns, 11.7712 ms/op
WorkloadActual  69: 128 op, 1447476600.00 ns, 11.3084 ms/op
WorkloadActual  70: 128 op, 658946300.00 ns, 5.1480 ms/op
WorkloadActual  71: 128 op, 635281300.00 ns, 4.9631 ms/op
WorkloadActual  72: 128 op, 593764100.00 ns, 4.6388 ms/op
WorkloadActual  73: 128 op, 608610500.00 ns, 4.7548 ms/op
WorkloadActual  74: 128 op, 660519500.00 ns, 5.1603 ms/op
WorkloadActual  75: 128 op, 1093481900.00 ns, 8.5428 ms/op
WorkloadActual  76: 128 op, 1404272600.00 ns, 10.9709 ms/op
WorkloadActual  77: 128 op, 1640320000.00 ns, 12.8150 ms/op
WorkloadActual  78: 128 op, 1112694600.00 ns, 8.6929 ms/op
WorkloadActual  79: 128 op, 614709000.00 ns, 4.8024 ms/op
WorkloadActual  80: 128 op, 600582700.00 ns, 4.6921 ms/op
WorkloadActual  81: 128 op, 610417000.00 ns, 4.7689 ms/op
WorkloadActual  82: 128 op, 643438200.00 ns, 5.0269 ms/op
WorkloadActual  83: 128 op, 619443500.00 ns, 4.8394 ms/op
WorkloadActual  84: 128 op, 1074900900.00 ns, 8.3977 ms/op
WorkloadActual  85: 128 op, 1485639500.00 ns, 11.6066 ms/op
WorkloadActual  86: 128 op, 1530407000.00 ns, 11.9563 ms/op
WorkloadActual  87: 128 op, 1388776400.00 ns, 10.8498 ms/op
WorkloadActual  88: 128 op, 1218038100.00 ns, 9.5159 ms/op
WorkloadActual  89: 128 op, 958577400.00 ns, 7.4889 ms/op
WorkloadActual  90: 128 op, 651317700.00 ns, 5.0884 ms/op
WorkloadActual  91: 128 op, 645587900.00 ns, 5.0437 ms/op
WorkloadActual  92: 128 op, 591261400.00 ns, 4.6192 ms/op
WorkloadActual  93: 128 op, 1318270600.00 ns, 10.2990 ms/op
WorkloadActual  94: 128 op, 1420659800.00 ns, 11.0989 ms/op
WorkloadActual  95: 128 op, 1372479300.00 ns, 10.7225 ms/op
WorkloadActual  96: 128 op, 1092609800.00 ns, 8.5360 ms/op
WorkloadActual  97: 128 op, 715857500.00 ns, 5.5926 ms/op
WorkloadActual  98: 128 op, 616622700.00 ns, 4.8174 ms/op
WorkloadActual  99: 128 op, 598339000.00 ns, 4.6745 ms/op
WorkloadActual  100: 128 op, 618608600.00 ns, 4.8329 ms/op

// AfterActualRun
WorkloadResult   1: 128 op, 598072000.00 ns, 4.6724 ms/op
WorkloadResult   2: 128 op, 751342300.00 ns, 5.8699 ms/op
WorkloadResult   3: 128 op, 1400958600.00 ns, 10.9450 ms/op
WorkloadResult   4: 128 op, 1479153600.00 ns, 11.5559 ms/op
WorkloadResult   5: 128 op, 1613370900.00 ns, 12.6045 ms/op
WorkloadResult   6: 128 op, 745453000.00 ns, 5.8239 ms/op
WorkloadResult   7: 128 op, 596247400.00 ns, 4.6582 ms/op
WorkloadResult   8: 128 op, 656307100.00 ns, 5.1274 ms/op
WorkloadResult   9: 128 op, 612933500.00 ns, 4.7885 ms/op
WorkloadResult  10: 128 op, 620038800.00 ns, 4.8441 ms/op
WorkloadResult  11: 128 op, 972116600.00 ns, 7.5947 ms/op
WorkloadResult  12: 128 op, 1329677600.00 ns, 10.3881 ms/op
WorkloadResult  13: 128 op, 1435616700.00 ns, 11.2158 ms/op
WorkloadResult  14: 128 op, 1463932500.00 ns, 11.4370 ms/op
WorkloadResult  15: 128 op, 619374500.00 ns, 4.8389 ms/op
WorkloadResult  16: 128 op, 743103000.00 ns, 5.8055 ms/op
WorkloadResult  17: 128 op, 641257600.00 ns, 5.0098 ms/op
WorkloadResult  18: 128 op, 631012300.00 ns, 4.9298 ms/op
WorkloadResult  19: 128 op, 613668200.00 ns, 4.7943 ms/op
WorkloadResult  20: 128 op, 1128493800.00 ns, 8.8164 ms/op
WorkloadResult  21: 128 op, 1421446100.00 ns, 11.1050 ms/op
WorkloadResult  22: 128 op, 1445928900.00 ns, 11.2963 ms/op
WorkloadResult  23: 128 op, 1171042200.00 ns, 9.1488 ms/op
WorkloadResult  24: 128 op, 607173900.00 ns, 4.7435 ms/op
WorkloadResult  25: 128 op, 614572200.00 ns, 4.8013 ms/op
WorkloadResult  26: 128 op, 611344200.00 ns, 4.7761 ms/op
WorkloadResult  27: 128 op, 640139700.00 ns, 5.0011 ms/op
WorkloadResult  28: 128 op, 685745500.00 ns, 5.3574 ms/op
WorkloadResult  29: 128 op, 1270851300.00 ns, 9.9285 ms/op
WorkloadResult  30: 128 op, 1456741700.00 ns, 11.3808 ms/op
WorkloadResult  31: 128 op, 1615413400.00 ns, 12.6204 ms/op
WorkloadResult  32: 128 op, 1481952100.00 ns, 11.5778 ms/op
WorkloadResult  33: 128 op, 750332800.00 ns, 5.8620 ms/op
WorkloadResult  34: 128 op, 627406600.00 ns, 4.9016 ms/op
WorkloadResult  35: 128 op, 672378000.00 ns, 5.2530 ms/op
WorkloadResult  36: 128 op, 651862300.00 ns, 5.0927 ms/op
WorkloadResult  37: 128 op, 615685600.00 ns, 4.8100 ms/op
WorkloadResult  38: 128 op, 1055160400.00 ns, 8.2434 ms/op
WorkloadResult  39: 128 op, 1673939600.00 ns, 13.0777 ms/op
WorkloadResult  40: 128 op, 1372226400.00 ns, 10.7205 ms/op
WorkloadResult  41: 128 op, 1371980600.00 ns, 10.7186 ms/op
WorkloadResult  42: 128 op, 1112154100.00 ns, 8.6887 ms/op
WorkloadResult  43: 128 op, 610274000.00 ns, 4.7678 ms/op
WorkloadResult  44: 128 op, 619303900.00 ns, 4.8383 ms/op
WorkloadResult  45: 128 op, 658176800.00 ns, 5.1420 ms/op
WorkloadResult  46: 128 op, 597600000.00 ns, 4.6688 ms/op
WorkloadResult  47: 128 op, 632950200.00 ns, 4.9449 ms/op
WorkloadResult  48: 128 op, 1244744300.00 ns, 9.7246 ms/op
WorkloadResult  49: 128 op, 1480827000.00 ns, 11.5690 ms/op
WorkloadResult  50: 128 op, 1519474300.00 ns, 11.8709 ms/op
WorkloadResult  51: 128 op, 986768900.00 ns, 7.7091 ms/op
WorkloadResult  52: 128 op, 605573600.00 ns, 4.7310 ms/op
WorkloadResult  53: 128 op, 621514300.00 ns, 4.8556 ms/op
WorkloadResult  54: 128 op, 679211600.00 ns, 5.3063 ms/op
WorkloadResult  55: 128 op, 603873100.00 ns, 4.7178 ms/op
WorkloadResult  56: 128 op, 641268100.00 ns, 5.0099 ms/op
WorkloadResult  57: 128 op, 1378400500.00 ns, 10.7688 ms/op
WorkloadResult  58: 128 op, 1373407200.00 ns, 10.7297 ms/op
WorkloadResult  59: 128 op, 1414513700.00 ns, 11.0509 ms/op
WorkloadResult  60: 128 op, 981899200.00 ns, 7.6711 ms/op
WorkloadResult  61: 128 op, 587688400.00 ns, 4.5913 ms/op
WorkloadResult  62: 128 op, 625358500.00 ns, 4.8856 ms/op
WorkloadResult  63: 128 op, 643447800.00 ns, 5.0269 ms/op
WorkloadResult  64: 128 op, 612721900.00 ns, 4.7869 ms/op
WorkloadResult  65: 128 op, 806348200.00 ns, 6.2996 ms/op
WorkloadResult  66: 128 op, 1378452100.00 ns, 10.7692 ms/op
WorkloadResult  67: 128 op, 1424907200.00 ns, 11.1321 ms/op
WorkloadResult  68: 128 op, 1506711200.00 ns, 11.7712 ms/op
WorkloadResult  69: 128 op, 1447475800.00 ns, 11.3084 ms/op
WorkloadResult  70: 128 op, 658945500.00 ns, 5.1480 ms/op
WorkloadResult  71: 128 op, 635280500.00 ns, 4.9631 ms/op
WorkloadResult  72: 128 op, 593763300.00 ns, 4.6388 ms/op
WorkloadResult  73: 128 op, 608609700.00 ns, 4.7548 ms/op
WorkloadResult  74: 128 op, 660518700.00 ns, 5.1603 ms/op
WorkloadResult  75: 128 op, 1093481100.00 ns, 8.5428 ms/op
WorkloadResult  76: 128 op, 1404271800.00 ns, 10.9709 ms/op
WorkloadResult  77: 128 op, 1640319200.00 ns, 12.8150 ms/op
WorkloadResult  78: 128 op, 1112693800.00 ns, 8.6929 ms/op
WorkloadResult  79: 128 op, 614708200.00 ns, 4.8024 ms/op
WorkloadResult  80: 128 op, 600581900.00 ns, 4.6920 ms/op
WorkloadResult  81: 128 op, 610416200.00 ns, 4.7689 ms/op
WorkloadResult  82: 128 op, 643437400.00 ns, 5.0269 ms/op
WorkloadResult  83: 128 op, 619442700.00 ns, 4.8394 ms/op
WorkloadResult  84: 128 op, 1074900100.00 ns, 8.3977 ms/op
WorkloadResult  85: 128 op, 1485638700.00 ns, 11.6066 ms/op
WorkloadResult  86: 128 op, 1530406200.00 ns, 11.9563 ms/op
WorkloadResult  87: 128 op, 1388775600.00 ns, 10.8498 ms/op
WorkloadResult  88: 128 op, 1218037300.00 ns, 9.5159 ms/op
WorkloadResult  89: 128 op, 958576600.00 ns, 7.4889 ms/op
WorkloadResult  90: 128 op, 651316900.00 ns, 5.0884 ms/op
WorkloadResult  91: 128 op, 645587100.00 ns, 5.0436 ms/op
WorkloadResult  92: 128 op, 591260600.00 ns, 4.6192 ms/op
WorkloadResult  93: 128 op, 1318269800.00 ns, 10.2990 ms/op
WorkloadResult  94: 128 op, 1420659000.00 ns, 11.0989 ms/op
WorkloadResult  95: 128 op, 1372478500.00 ns, 10.7225 ms/op
WorkloadResult  96: 128 op, 1092609000.00 ns, 8.5360 ms/op
WorkloadResult  97: 128 op, 715856700.00 ns, 5.5926 ms/op
WorkloadResult  98: 128 op, 616621900.00 ns, 4.8174 ms/op
WorkloadResult  99: 128 op, 598338200.00 ns, 4.6745 ms/op
WorkloadResult  100: 128 op, 618607800.00 ns, 4.8329 ms/op

// AfterAll
// Benchmark Process 22492 has exited with code 0.

Mean = 7.504 ms, StdErr = 0.292 ms (3.90%), N = 100, StdDev = 2.923 ms
Min = 4.591 ms, Q1 = 4.839 ms, Median = 5.815 ms, Q3 = 10.739 ms, Max = 13.078 ms
IQR = 5.900 ms, LowerFence = -4.011 ms, UpperFence = 19.590 ms
ConfidenceInterval = [6.513 ms; 8.496 ms] (CI 99.9%), Margin = 0.991 ms (13.21% of Mean)
Skewness = 0.44, Kurtosis = 1.47, MValue = 3.17

// ***** BenchmarkRunner: Finish  *****

// * Export *
  BenchmarkDotNet.Artifacts\results\CSScratchpad.Script.WriteText-report.csv
  BenchmarkDotNet.Artifacts\results\CSScratchpad.Script.WriteText-report-github.md
  BenchmarkDotNet.Artifacts\results\CSScratchpad.Script.WriteText-report.html

// * Detailed results *
WriteText.RunTextWriter: .NET Framework 4.6.1(Runtime=.NET Framework 4.6.1)
Runtime = .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT; GC = Concurrent Workstation
Mean = 6.832 ms, StdErr = 0.282 ms (4.12%), N = 100, StdDev = 2.816 ms
Min = 4.621 ms, Q1 = 4.867 ms, Median = 5.132 ms, Q3 = 8.649 ms, Max = 13.589 ms
IQR = 3.783 ms, LowerFence = -0.807 ms, UpperFence = 14.323 ms
ConfidenceInterval = [5.877 ms; 7.787 ms] (CI 99.9%), Margin = 0.955 ms (13.98% of Mean)
Skewness = 1.07, Kurtosis = 2.54, MValue = 2.61
-------------------- Histogram --------------------
[ 4.567 ms ;  6.159 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[ 6.159 ms ;  7.013 ms) | 
[ 7.013 ms ;  8.606 ms) | @@@@@@@@@
[ 8.606 ms ; 10.403 ms) | @@@@@
[10.403 ms ; 12.339 ms) | @@@@@@@@@@@@@@@@
[12.339 ms ; 13.924 ms) | @@@@
---------------------------------------------------

WriteText.RunTextWriter: .NET Framework 4.7.2(Runtime=.NET Framework 4.7.2)
Runtime = .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT; GC = Concurrent Workstation
Mean = 8.395 ms, StdErr = 0.343 ms (4.09%), N = 100, StdDev = 3.433 ms
Min = 4.666 ms, Q1 = 5.135 ms, Median = 7.207 ms, Q3 = 11.497 ms, Max = 17.131 ms
IQR = 6.363 ms, LowerFence = -4.410 ms, UpperFence = 21.042 ms
ConfidenceInterval = [7.230 ms; 9.559 ms] (CI 99.9%), Margin = 1.164 ms (13.87% of Mean)
Skewness = 0.56, Kurtosis = 2.06, MValue = 3.27
-------------------- Histogram --------------------
[ 4.582 ms ;  6.524 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[ 6.524 ms ;  7.188 ms) | @@@@
[ 7.188 ms ;  9.129 ms) | @@@@@@@@@@@@@@
[ 9.129 ms ; 10.894 ms) | @@@@@
[10.894 ms ; 12.835 ms) | @@@@@@@@@@@@@@@@@@@@@@@
[12.835 ms ; 15.249 ms) | @@@@@@@@
[15.249 ms ; 18.101 ms) | @@
---------------------------------------------------

WriteText.RunFileStream: .NET Framework 4.6.1(Runtime=.NET Framework 4.6.1)
Runtime = .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT; GC = Concurrent Workstation
Mean = 7.203 ms, StdErr = 0.307 ms (4.26%), N = 100, StdDev = 3.067 ms
Min = 4.584 ms, Q1 = 4.880 ms, Median = 5.183 ms, Q3 = 10.804 ms, Max = 14.151 ms
IQR = 5.924 ms, LowerFence = -4.006 ms, UpperFence = 19.690 ms
ConfidenceInterval = [6.162 ms; 8.243 ms] (CI 99.9%), Margin = 1.040 ms (14.44% of Mean)
Skewness = 0.82, Kurtosis = 1.98, MValue = 2.74
-------------------- Histogram --------------------
[ 4.475 ms ;  6.209 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[ 6.209 ms ;  8.061 ms) | @@@@@@@
[ 8.061 ms ;  8.763 ms) | @
[ 8.763 ms ; 10.673 ms) | @@@@
[10.673 ms ; 12.408 ms) | @@@@@@@@@@@@@@@@@@@@@
[12.408 ms ; 14.275 ms) | @@@@@@
---------------------------------------------------

WriteText.RunFileStream: .NET Framework 4.7.2(Runtime=.NET Framework 4.7.2)
Runtime = .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT; GC = Concurrent Workstation
Mean = 7.633 ms, StdErr = 0.307 ms (4.03%), N = 100, StdDev = 3.072 ms
Min = 4.611 ms, Q1 = 4.916 ms, Median = 5.519 ms, Q3 = 11.037 ms, Max = 13.628 ms
IQR = 6.121 ms, LowerFence = -4.265 ms, UpperFence = 20.219 ms
ConfidenceInterval = [6.591 ms; 8.675 ms] (CI 99.9%), Margin = 1.042 ms (13.65% of Mean)
Skewness = 0.44, Kurtosis = 1.36, MValue = 3.23
-------------------- Histogram --------------------
[ 4.589 ms ;  6.327 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[ 6.327 ms ;  8.542 ms) | @@@@
[ 8.542 ms ; 10.602 ms) | @@@@
[10.602 ms ; 12.339 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[12.339 ms ; 13.887 ms) | @@@@
---------------------------------------------------

WriteText.RunFile: .NET Framework 4.6.1(Runtime=.NET Framework 4.6.1)
Runtime = .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT; GC = Concurrent Workstation
Mean = 7.467 ms, StdErr = 0.291 ms (3.90%), N = 100, StdDev = 2.910 ms
Min = 4.653 ms, Q1 = 4.901 ms, Median = 5.690 ms, Q3 = 10.626 ms, Max = 12.921 ms
IQR = 5.726 ms, LowerFence = -3.688 ms, UpperFence = 19.215 ms
ConfidenceInterval = [6.480 ms; 8.454 ms] (CI 99.9%), Margin = 0.987 ms (13.22% of Mean)
Skewness = 0.5, Kurtosis = 1.53, MValue = 2.76
-------------------- Histogram --------------------
[ 4.632 ms ;  6.278 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[ 6.278 ms ;  7.054 ms) | @
[ 7.054 ms ;  8.834 ms) | @@@@@@
[ 8.834 ms ; 10.508 ms) | @@@@@@@@@@@
[10.508 ms ; 12.154 ms) | @@@@@@@@@@@@@@@@@@@@@@
[12.154 ms ; 13.744 ms) | @@@@@
---------------------------------------------------

WriteText.RunFile: .NET Framework 4.7.2(Runtime=.NET Framework 4.7.2)
Runtime = .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT; GC = Concurrent Workstation
Mean = 7.504 ms, StdErr = 0.292 ms (3.90%), N = 100, StdDev = 2.923 ms
Min = 4.591 ms, Q1 = 4.839 ms, Median = 5.815 ms, Q3 = 10.739 ms, Max = 13.078 ms
IQR = 5.900 ms, LowerFence = -4.011 ms, UpperFence = 19.590 ms
ConfidenceInterval = [6.513 ms; 8.496 ms] (CI 99.9%), Margin = 0.991 ms (13.21% of Mean)
Skewness = 0.44, Kurtosis = 1.47, MValue = 3.17
-------------------- Histogram --------------------
[ 4.404 ms ;  6.057 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[ 6.057 ms ;  7.326 ms) | @
[ 7.326 ms ;  8.979 ms) | @@@@@@@@@@@
[ 8.979 ms ; 10.346 ms) | @@@@@
[10.346 ms ; 11.999 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@
[11.999 ms ; 13.904 ms) | @@@@
---------------------------------------------------

// * Summary *

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1165 (21H1/May2021Update)
Intel Core i7-7500U CPU 2.70GHz (Kaby Lake), 1 CPU, 4 logical and 2 physical cores
  [Host]               : .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT  [AttachedDebugger]
  .NET Framework 4.6.1 : .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
  .NET Framework 4.7.2 : .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT


|        Method |                  Job |              Runtime |     Mean |     Error |   StdDev |   Median | Ratio | RatioSD |
|-------------- |--------------------- |--------------------- |---------:|----------:|---------:|---------:|------:|--------:|
| RunTextWriter | .NET Framework 4.6.1 | .NET Framework 4.6.1 | 6.832 ms | 0.9550 ms | 2.816 ms | 5.132 ms |  1.00 |    0.00 |
| RunTextWriter | .NET Framework 4.7.2 | .NET Framework 4.7.2 | 8.395 ms | 1.1643 ms | 3.433 ms | 7.207 ms |  1.43 |    0.80 |
|               |                      |                      |          |           |          |          |       |         |
| RunFileStream | .NET Framework 4.6.1 | .NET Framework 4.6.1 | 7.203 ms | 1.0402 ms | 3.067 ms | 5.183 ms |  1.00 |    0.00 |
| RunFileStream | .NET Framework 4.7.2 | .NET Framework 4.7.2 | 7.633 ms | 1.0420 ms | 3.072 ms | 5.519 ms |  1.27 |    0.73 |
|               |                      |                      |          |           |          |          |       |         |
|       RunFile | .NET Framework 4.6.1 | .NET Framework 4.6.1 | 7.467 ms | 0.9868 ms | 2.910 ms | 5.690 ms |  1.00 |    0.00 |
|       RunFile | .NET Framework 4.7.2 | .NET Framework 4.7.2 | 7.504 ms | 0.9915 ms | 2.923 ms | 5.815 ms |  1.16 |    0.63 |

// * Warnings *
Environment
  Summary -> Benchmark was executed with attached debugger
MultimodalDistribution
  WriteText.RunTextWriter: .NET Framework 4.7.2 -> It seems that the distribution is bimodal (mValue = 3.27)
  WriteText.RunFileStream: .NET Framework 4.7.2 -> It seems that the distribution is bimodal (mValue = 3.23)
  WriteText.RunFile: .NET Framework 4.7.2       -> It seems that the distribution can have several modes (mValue = 3.17)

// * Legends *
  Mean    : Arithmetic mean of all measurements
  Error   : Half of 99.9% confidence interval
  StdDev  : Standard deviation of all measurements
  Median  : Value separating the higher half of all measurements (50th percentile)
  Ratio   : Mean of the ratio distribution ([Current]/[Baseline])
  RatioSD : Standard deviation of the ratio distribution ([Current]/[Baseline])
  1 ms    : 1 Millisecond (0.001 sec)

// ***** BenchmarkRunner: End *****
// ** Remained 0 benchmark(s) to run **
Run time: 00:08:58 (538.45 sec), executed benchmarks: 6

Global total time: 00:09:02 (542.04 sec), executed benchmarks: 6
// * Artifacts cleanup *

*/
#endregion
