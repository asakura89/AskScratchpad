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
            lock (logTextWriterLock) {
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
// ***** Done, took 00:00:04 (4.86 sec)   *****
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
// Execute: D:\Microsoft\Workspace\asakura89-project\release\AskScratchpad\CSScratchpad\CSScratchpad\bin\Release\883981ac-c734-42b2-8df1-c05314150d15.exe --benchmarkName "CSScratchpad.Script.WriteText.RunTextWriter" --job ".NET Framework 4.6.1" --benchmarkId 0 in 
// BeforeAnythingElse

// Benchmark Process Environment Information:
// Runtime=.NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
// GC=Concurrent Workstation
// Job: .NET Framework 4.6.1

OverheadJitting  1: 1 op, 217300.00 ns, 217.3000 us/op
WorkloadJitting  1: 1 op, 8862300.00 ns, 8.8623 ms/op

OverheadJitting  2: 16 op, 144300.00 ns, 9.0188 us/op
WorkloadJitting  2: 16 op, 70372400.00 ns, 4.3983 ms/op

WorkloadPilot    1: 16 op, 65799000.00 ns, 4.1124 ms/op
WorkloadPilot    2: 32 op, 140287600.00 ns, 4.3840 ms/op
WorkloadPilot    3: 64 op, 260308400.00 ns, 4.0673 ms/op
WorkloadPilot    4: 128 op, 578521900.00 ns, 4.5197 ms/op

OverheadWarmup   1: 128 op, 4000.00 ns, 31.2500 ns/op
OverheadWarmup   2: 128 op, 1300.00 ns, 10.1563 ns/op
OverheadWarmup   3: 128 op, 2000.00 ns, 15.6250 ns/op
OverheadWarmup   4: 128 op, 800.00 ns, 6.2500 ns/op
OverheadWarmup   5: 128 op, 900.00 ns, 7.0313 ns/op
OverheadWarmup   6: 128 op, 1500.00 ns, 11.7188 ns/op
OverheadWarmup   7: 128 op, 800.00 ns, 6.2500 ns/op

OverheadActual   1: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadActual   2: 128 op, 1200.00 ns, 9.3750 ns/op
OverheadActual   3: 128 op, 1000.00 ns, 7.8125 ns/op
OverheadActual   4: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadActual   5: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual   6: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual   7: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual   8: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual   9: 128 op, 900.00 ns, 7.0313 ns/op
OverheadActual  10: 128 op, 1300.00 ns, 10.1563 ns/op
OverheadActual  11: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual  12: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual  13: 128 op, 900.00 ns, 7.0313 ns/op
OverheadActual  14: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual  15: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual  16: 128 op, 1000.00 ns, 7.8125 ns/op
OverheadActual  17: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual  18: 128 op, 900.00 ns, 7.0313 ns/op
OverheadActual  19: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual  20: 128 op, 700.00 ns, 5.4688 ns/op

WorkloadWarmup   1: 128 op, 561747600.00 ns, 4.3887 ms/op
WorkloadWarmup   2: 128 op, 574170200.00 ns, 4.4857 ms/op
WorkloadWarmup   3: 128 op, 601206000.00 ns, 4.6969 ms/op
WorkloadWarmup   4: 128 op, 654951900.00 ns, 5.1168 ms/op
WorkloadWarmup   5: 128 op, 648427400.00 ns, 5.0658 ms/op
WorkloadWarmup   6: 128 op, 749422200.00 ns, 5.8549 ms/op
WorkloadWarmup   7: 128 op, 901885800.00 ns, 7.0460 ms/op
WorkloadWarmup   8: 128 op, 1390462300.00 ns, 10.8630 ms/op
WorkloadWarmup   9: 128 op, 1572159400.00 ns, 12.2825 ms/op
WorkloadWarmup  10: 128 op, 723008600.00 ns, 5.6485 ms/op

// BeforeActualRun
WorkloadActual   1: 128 op, 597786000.00 ns, 4.6702 ms/op
WorkloadActual   2: 128 op, 592116200.00 ns, 4.6259 ms/op
WorkloadActual   3: 128 op, 607918400.00 ns, 4.7494 ms/op
WorkloadActual   4: 128 op, 669583100.00 ns, 5.2311 ms/op
WorkloadActual   5: 128 op, 597931400.00 ns, 4.6713 ms/op
WorkloadActual   6: 128 op, 593000300.00 ns, 4.6328 ms/op
WorkloadActual   7: 128 op, 894852900.00 ns, 6.9910 ms/op
WorkloadActual   8: 128 op, 1721956900.00 ns, 13.4528 ms/op
WorkloadActual   9: 128 op, 1464880200.00 ns, 11.4444 ms/op
WorkloadActual  10: 128 op, 666653600.00 ns, 5.2082 ms/op
WorkloadActual  11: 128 op, 758293400.00 ns, 5.9242 ms/op
WorkloadActual  12: 128 op, 671546600.00 ns, 5.2465 ms/op
WorkloadActual  13: 128 op, 672750800.00 ns, 5.2559 ms/op
WorkloadActual  14: 128 op, 616452600.00 ns, 4.8160 ms/op
WorkloadActual  15: 128 op, 1293570200.00 ns, 10.1060 ms/op
WorkloadActual  16: 128 op, 1370331100.00 ns, 10.7057 ms/op
WorkloadActual  17: 128 op, 1132249900.00 ns, 8.8457 ms/op
WorkloadActual  18: 128 op, 600653500.00 ns, 4.6926 ms/op
WorkloadActual  19: 128 op, 679326100.00 ns, 5.3072 ms/op
WorkloadActual  20: 128 op, 580989700.00 ns, 4.5390 ms/op
WorkloadActual  21: 128 op, 645842700.00 ns, 5.0456 ms/op
WorkloadActual  22: 128 op, 598448300.00 ns, 4.6754 ms/op
WorkloadActual  23: 128 op, 1124903900.00 ns, 8.7883 ms/op
WorkloadActual  24: 128 op, 1422604400.00 ns, 11.1141 ms/op
WorkloadActual  25: 128 op, 1320423700.00 ns, 10.3158 ms/op
WorkloadActual  26: 128 op, 609846500.00 ns, 4.7644 ms/op
WorkloadActual  27: 128 op, 613503000.00 ns, 4.7930 ms/op
WorkloadActual  28: 128 op, 636789400.00 ns, 4.9749 ms/op
WorkloadActual  29: 128 op, 620748700.00 ns, 4.8496 ms/op
WorkloadActual  30: 128 op, 656845700.00 ns, 5.1316 ms/op
WorkloadActual  31: 128 op, 1366299500.00 ns, 10.6742 ms/op
WorkloadActual  32: 128 op, 1532707600.00 ns, 11.9743 ms/op
WorkloadActual  33: 128 op, 1190862600.00 ns, 9.3036 ms/op
WorkloadActual  34: 128 op, 661736200.00 ns, 5.1698 ms/op
WorkloadActual  35: 128 op, 588044200.00 ns, 4.5941 ms/op
WorkloadActual  36: 128 op, 633094700.00 ns, 4.9461 ms/op
WorkloadActual  37: 128 op, 602032300.00 ns, 4.7034 ms/op
WorkloadActual  38: 128 op, 596239800.00 ns, 4.6581 ms/op
WorkloadActual  39: 128 op, 1201292100.00 ns, 9.3851 ms/op
WorkloadActual  40: 128 op, 1473626500.00 ns, 11.5127 ms/op
WorkloadActual  41: 128 op, 1231229400.00 ns, 9.6190 ms/op
WorkloadActual  42: 128 op, 593690400.00 ns, 4.6382 ms/op
WorkloadActual  43: 128 op, 603964000.00 ns, 4.7185 ms/op
WorkloadActual  44: 128 op, 627883400.00 ns, 4.9053 ms/op
WorkloadActual  45: 128 op, 724601100.00 ns, 5.6609 ms/op
WorkloadActual  46: 128 op, 620784100.00 ns, 4.8499 ms/op
WorkloadActual  47: 128 op, 1237311400.00 ns, 9.6665 ms/op
WorkloadActual  48: 128 op, 1416953800.00 ns, 11.0700 ms/op
WorkloadActual  49: 128 op, 1246381100.00 ns, 9.7374 ms/op
WorkloadActual  50: 128 op, 602767800.00 ns, 4.7091 ms/op
WorkloadActual  51: 128 op, 627024500.00 ns, 4.8986 ms/op
WorkloadActual  52: 128 op, 581812600.00 ns, 4.5454 ms/op
WorkloadActual  53: 128 op, 608140300.00 ns, 4.7511 ms/op
WorkloadActual  54: 128 op, 605587500.00 ns, 4.7312 ms/op
WorkloadActual  55: 128 op, 905667700.00 ns, 7.0755 ms/op
WorkloadActual  56: 128 op, 1511436000.00 ns, 11.8081 ms/op
WorkloadActual  57: 128 op, 2240367100.00 ns, 17.5029 ms/op
WorkloadActual  58: 128 op, 711971900.00 ns, 5.5623 ms/op
WorkloadActual  59: 128 op, 648052000.00 ns, 5.0629 ms/op
WorkloadActual  60: 128 op, 593750200.00 ns, 4.6387 ms/op
WorkloadActual  61: 128 op, 587344400.00 ns, 4.5886 ms/op
WorkloadActual  62: 128 op, 1115381700.00 ns, 8.7139 ms/op
WorkloadActual  63: 128 op, 1385232000.00 ns, 10.8221 ms/op
WorkloadActual  64: 128 op, 1335017700.00 ns, 10.4298 ms/op
WorkloadActual  65: 128 op, 615608100.00 ns, 4.8094 ms/op
WorkloadActual  66: 128 op, 616021900.00 ns, 4.8127 ms/op
WorkloadActual  67: 128 op, 633945000.00 ns, 4.9527 ms/op
WorkloadActual  68: 128 op, 620909800.00 ns, 4.8509 ms/op
WorkloadActual  69: 128 op, 613822400.00 ns, 4.7955 ms/op
WorkloadActual  70: 128 op, 1068966800.00 ns, 8.3513 ms/op
WorkloadActual  71: 128 op, 1337932900.00 ns, 10.4526 ms/op
WorkloadActual  72: 128 op, 1347453800.00 ns, 10.5270 ms/op
WorkloadActual  73: 128 op, 730537100.00 ns, 5.7073 ms/op
WorkloadActual  74: 128 op, 584406400.00 ns, 4.5657 ms/op
WorkloadActual  75: 128 op, 620827000.00 ns, 4.8502 ms/op
WorkloadActual  76: 128 op, 617886400.00 ns, 4.8272 ms/op
WorkloadActual  77: 128 op, 604025100.00 ns, 4.7189 ms/op
WorkloadActual  78: 128 op, 725672500.00 ns, 5.6693 ms/op
WorkloadActual  79: 128 op, 1515595900.00 ns, 11.8406 ms/op
WorkloadActual  80: 128 op, 1384084100.00 ns, 10.8132 ms/op
WorkloadActual  81: 128 op, 954837500.00 ns, 7.4597 ms/op
WorkloadActual  82: 128 op, 672479000.00 ns, 5.2537 ms/op
WorkloadActual  83: 128 op, 742321200.00 ns, 5.7994 ms/op
WorkloadActual  84: 128 op, 721958100.00 ns, 5.6403 ms/op
WorkloadActual  85: 128 op, 621409900.00 ns, 4.8548 ms/op
WorkloadActual  86: 128 op, 1129199800.00 ns, 8.8219 ms/op
WorkloadActual  87: 128 op, 1446299800.00 ns, 11.2992 ms/op
WorkloadActual  88: 128 op, 1324496500.00 ns, 10.3476 ms/op
WorkloadActual  89: 128 op, 601696800.00 ns, 4.7008 ms/op
WorkloadActual  90: 128 op, 625913700.00 ns, 4.8900 ms/op
WorkloadActual  91: 128 op, 609989500.00 ns, 4.7655 ms/op
WorkloadActual  92: 128 op, 609159600.00 ns, 4.7591 ms/op
WorkloadActual  93: 128 op, 595441300.00 ns, 4.6519 ms/op
WorkloadActual  94: 128 op, 1027885900.00 ns, 8.0304 ms/op
WorkloadActual  95: 128 op, 1348692600.00 ns, 10.5367 ms/op
WorkloadActual  96: 128 op, 1551558500.00 ns, 12.1216 ms/op
WorkloadActual  97: 128 op, 749699900.00 ns, 5.8570 ms/op
WorkloadActual  98: 128 op, 628553900.00 ns, 4.9106 ms/op
WorkloadActual  99: 128 op, 655247300.00 ns, 5.1191 ms/op
WorkloadActual  100: 128 op, 582902100.00 ns, 4.5539 ms/op

// AfterActualRun
WorkloadResult   1: 128 op, 597785200.00 ns, 4.6702 ms/op
WorkloadResult   2: 128 op, 592115400.00 ns, 4.6259 ms/op
WorkloadResult   3: 128 op, 607917600.00 ns, 4.7494 ms/op
WorkloadResult   4: 128 op, 669582300.00 ns, 5.2311 ms/op
WorkloadResult   5: 128 op, 597930600.00 ns, 4.6713 ms/op
WorkloadResult   6: 128 op, 592999500.00 ns, 4.6328 ms/op
WorkloadResult   7: 128 op, 894852100.00 ns, 6.9910 ms/op
WorkloadResult   8: 128 op, 1721956100.00 ns, 13.4528 ms/op
WorkloadResult   9: 128 op, 1464879400.00 ns, 11.4444 ms/op
WorkloadResult  10: 128 op, 666652800.00 ns, 5.2082 ms/op
WorkloadResult  11: 128 op, 758292600.00 ns, 5.9242 ms/op
WorkloadResult  12: 128 op, 671545800.00 ns, 5.2465 ms/op
WorkloadResult  13: 128 op, 672750000.00 ns, 5.2559 ms/op
WorkloadResult  14: 128 op, 616451800.00 ns, 4.8160 ms/op
WorkloadResult  15: 128 op, 1293569400.00 ns, 10.1060 ms/op
WorkloadResult  16: 128 op, 1370330300.00 ns, 10.7057 ms/op
WorkloadResult  17: 128 op, 1132249100.00 ns, 8.8457 ms/op
WorkloadResult  18: 128 op, 600652700.00 ns, 4.6926 ms/op
WorkloadResult  19: 128 op, 679325300.00 ns, 5.3072 ms/op
WorkloadResult  20: 128 op, 580988900.00 ns, 4.5390 ms/op
WorkloadResult  21: 128 op, 645841900.00 ns, 5.0456 ms/op
WorkloadResult  22: 128 op, 598447500.00 ns, 4.6754 ms/op
WorkloadResult  23: 128 op, 1124903100.00 ns, 8.7883 ms/op
WorkloadResult  24: 128 op, 1422603600.00 ns, 11.1141 ms/op
WorkloadResult  25: 128 op, 1320422900.00 ns, 10.3158 ms/op
WorkloadResult  26: 128 op, 609845700.00 ns, 4.7644 ms/op
WorkloadResult  27: 128 op, 613502200.00 ns, 4.7930 ms/op
WorkloadResult  28: 128 op, 636788600.00 ns, 4.9749 ms/op
WorkloadResult  29: 128 op, 620747900.00 ns, 4.8496 ms/op
WorkloadResult  30: 128 op, 656844900.00 ns, 5.1316 ms/op
WorkloadResult  31: 128 op, 1366298700.00 ns, 10.6742 ms/op
WorkloadResult  32: 128 op, 1532706800.00 ns, 11.9743 ms/op
WorkloadResult  33: 128 op, 1190861800.00 ns, 9.3036 ms/op
WorkloadResult  34: 128 op, 661735400.00 ns, 5.1698 ms/op
WorkloadResult  35: 128 op, 588043400.00 ns, 4.5941 ms/op
WorkloadResult  36: 128 op, 633093900.00 ns, 4.9460 ms/op
WorkloadResult  37: 128 op, 602031500.00 ns, 4.7034 ms/op
WorkloadResult  38: 128 op, 596239000.00 ns, 4.6581 ms/op
WorkloadResult  39: 128 op, 1201291300.00 ns, 9.3851 ms/op
WorkloadResult  40: 128 op, 1473625700.00 ns, 11.5127 ms/op
WorkloadResult  41: 128 op, 1231228600.00 ns, 9.6190 ms/op
WorkloadResult  42: 128 op, 593689600.00 ns, 4.6382 ms/op
WorkloadResult  43: 128 op, 603963200.00 ns, 4.7185 ms/op
WorkloadResult  44: 128 op, 627882600.00 ns, 4.9053 ms/op
WorkloadResult  45: 128 op, 724600300.00 ns, 5.6609 ms/op
WorkloadResult  46: 128 op, 620783300.00 ns, 4.8499 ms/op
WorkloadResult  47: 128 op, 1237310600.00 ns, 9.6665 ms/op
WorkloadResult  48: 128 op, 1416953000.00 ns, 11.0699 ms/op
WorkloadResult  49: 128 op, 1246380300.00 ns, 9.7373 ms/op
WorkloadResult  50: 128 op, 602767000.00 ns, 4.7091 ms/op
WorkloadResult  51: 128 op, 627023700.00 ns, 4.8986 ms/op
WorkloadResult  52: 128 op, 581811800.00 ns, 4.5454 ms/op
WorkloadResult  53: 128 op, 608139500.00 ns, 4.7511 ms/op
WorkloadResult  54: 128 op, 605586700.00 ns, 4.7311 ms/op
WorkloadResult  55: 128 op, 905666900.00 ns, 7.0755 ms/op
WorkloadResult  56: 128 op, 1511435200.00 ns, 11.8081 ms/op
WorkloadResult  57: 128 op, 711971100.00 ns, 5.5623 ms/op
WorkloadResult  58: 128 op, 648051200.00 ns, 5.0629 ms/op
WorkloadResult  59: 128 op, 593749400.00 ns, 4.6387 ms/op
WorkloadResult  60: 128 op, 587343600.00 ns, 4.5886 ms/op
WorkloadResult  61: 128 op, 1115380900.00 ns, 8.7139 ms/op
WorkloadResult  62: 128 op, 1385231200.00 ns, 10.8221 ms/op
WorkloadResult  63: 128 op, 1335016900.00 ns, 10.4298 ms/op
WorkloadResult  64: 128 op, 615607300.00 ns, 4.8094 ms/op
WorkloadResult  65: 128 op, 616021100.00 ns, 4.8127 ms/op
WorkloadResult  66: 128 op, 633944200.00 ns, 4.9527 ms/op
WorkloadResult  67: 128 op, 620909000.00 ns, 4.8509 ms/op
WorkloadResult  68: 128 op, 613821600.00 ns, 4.7955 ms/op
WorkloadResult  69: 128 op, 1068966000.00 ns, 8.3513 ms/op
WorkloadResult  70: 128 op, 1337932100.00 ns, 10.4526 ms/op
WorkloadResult  71: 128 op, 1347453000.00 ns, 10.5270 ms/op
WorkloadResult  72: 128 op, 730536300.00 ns, 5.7073 ms/op
WorkloadResult  73: 128 op, 584405600.00 ns, 4.5657 ms/op
WorkloadResult  74: 128 op, 620826200.00 ns, 4.8502 ms/op
WorkloadResult  75: 128 op, 617885600.00 ns, 4.8272 ms/op
WorkloadResult  76: 128 op, 604024300.00 ns, 4.7189 ms/op
WorkloadResult  77: 128 op, 725671700.00 ns, 5.6693 ms/op
WorkloadResult  78: 128 op, 1515595100.00 ns, 11.8406 ms/op
WorkloadResult  79: 128 op, 1384083300.00 ns, 10.8132 ms/op
WorkloadResult  80: 128 op, 954836700.00 ns, 7.4597 ms/op
WorkloadResult  81: 128 op, 672478200.00 ns, 5.2537 ms/op
WorkloadResult  82: 128 op, 742320400.00 ns, 5.7994 ms/op
WorkloadResult  83: 128 op, 721957300.00 ns, 5.6403 ms/op
WorkloadResult  84: 128 op, 621409100.00 ns, 4.8548 ms/op
WorkloadResult  85: 128 op, 1129199000.00 ns, 8.8219 ms/op
WorkloadResult  86: 128 op, 1446299000.00 ns, 11.2992 ms/op
WorkloadResult  87: 128 op, 1324495700.00 ns, 10.3476 ms/op
WorkloadResult  88: 128 op, 601696000.00 ns, 4.7008 ms/op
WorkloadResult  89: 128 op, 625912900.00 ns, 4.8899 ms/op
WorkloadResult  90: 128 op, 609988700.00 ns, 4.7655 ms/op
WorkloadResult  91: 128 op, 609158800.00 ns, 4.7591 ms/op
WorkloadResult  92: 128 op, 595440500.00 ns, 4.6519 ms/op
WorkloadResult  93: 128 op, 1027885100.00 ns, 8.0304 ms/op
WorkloadResult  94: 128 op, 1348691800.00 ns, 10.5367 ms/op
WorkloadResult  95: 128 op, 1551557700.00 ns, 12.1215 ms/op
WorkloadResult  96: 128 op, 749699100.00 ns, 5.8570 ms/op
WorkloadResult  97: 128 op, 628553100.00 ns, 4.9106 ms/op
WorkloadResult  98: 128 op, 655246500.00 ns, 5.1191 ms/op
WorkloadResult  99: 128 op, 582901300.00 ns, 4.5539 ms/op

// AfterAll
// Benchmark Process 12348 has exited with code 0.

Mean = 6.774 ms, StdErr = 0.267 ms (3.94%), N = 99, StdDev = 2.653 ms
Min = 4.539 ms, Q1 = 4.762 ms, Median = 5.170 ms, Q3 = 9.344 ms, Max = 13.453 ms
IQR = 4.583 ms, LowerFence = -2.112 ms, UpperFence = 16.218 ms
ConfidenceInterval = [5.869 ms; 7.679 ms] (CI 99.9%), Margin = 0.905 ms (13.36% of Mean)
Skewness = 0.87, Kurtosis = 2.11, MValue = 2.69

// **************************
// Benchmark: WriteText.RunFileStream: .NET Framework 4.6.1(Runtime=.NET Framework 4.6.1)
// *** Execute ***
// Launch: 1 / 1
// Execute: D:\Microsoft\Workspace\asakura89-project\release\AskScratchpad\CSScratchpad\CSScratchpad\bin\Release\883981ac-c734-42b2-8df1-c05314150d15.exe --benchmarkName "CSScratchpad.Script.WriteText.RunFileStream" --job ".NET Framework 4.6.1" --benchmarkId 1 in 
// BeforeAnythingElse

// Benchmark Process Environment Information:
// Runtime=.NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
// GC=Concurrent Workstation
// Job: .NET Framework 4.6.1

OverheadJitting  1: 1 op, 202800.00 ns, 202.8000 us/op
WorkloadJitting  1: 1 op, 11569400.00 ns, 11.5694 ms/op

OverheadJitting  2: 16 op, 141300.00 ns, 8.8313 us/op
WorkloadJitting  2: 16 op, 85831000.00 ns, 5.3644 ms/op

WorkloadPilot    1: 16 op, 76196300.00 ns, 4.7623 ms/op
WorkloadPilot    2: 32 op, 160909200.00 ns, 5.0284 ms/op
WorkloadPilot    3: 64 op, 582375700.00 ns, 9.0996 ms/op

OverheadWarmup   1: 64 op, 8200.00 ns, 128.1250 ns/op
OverheadWarmup   2: 64 op, 1300.00 ns, 20.3125 ns/op
OverheadWarmup   3: 64 op, 1600.00 ns, 25.0000 ns/op
OverheadWarmup   4: 64 op, 1600.00 ns, 25.0000 ns/op
OverheadWarmup   5: 64 op, 1800.00 ns, 28.1250 ns/op
OverheadWarmup   6: 64 op, 1600.00 ns, 25.0000 ns/op

OverheadActual   1: 64 op, 1500.00 ns, 23.4375 ns/op
OverheadActual   2: 64 op, 1800.00 ns, 28.1250 ns/op
OverheadActual   3: 64 op, 1900.00 ns, 29.6875 ns/op
OverheadActual   4: 64 op, 2700.00 ns, 42.1875 ns/op
OverheadActual   5: 64 op, 1700.00 ns, 26.5625 ns/op
OverheadActual   6: 64 op, 1800.00 ns, 28.1250 ns/op
OverheadActual   7: 64 op, 1900.00 ns, 29.6875 ns/op
OverheadActual   8: 64 op, 3000.00 ns, 46.8750 ns/op
OverheadActual   9: 64 op, 1700.00 ns, 26.5625 ns/op
OverheadActual  10: 64 op, 1700.00 ns, 26.5625 ns/op
OverheadActual  11: 64 op, 1800.00 ns, 28.1250 ns/op
OverheadActual  12: 64 op, 1500.00 ns, 23.4375 ns/op
OverheadActual  13: 64 op, 2000.00 ns, 31.2500 ns/op
OverheadActual  14: 64 op, 2300.00 ns, 35.9375 ns/op
OverheadActual  15: 64 op, 2300.00 ns, 35.9375 ns/op
OverheadActual  16: 64 op, 1800.00 ns, 28.1250 ns/op
OverheadActual  17: 64 op, 1800.00 ns, 28.1250 ns/op
OverheadActual  18: 64 op, 2100.00 ns, 32.8125 ns/op
OverheadActual  19: 64 op, 1600.00 ns, 25.0000 ns/op
OverheadActual  20: 64 op, 1900.00 ns, 29.6875 ns/op

WorkloadWarmup   1: 64 op, 648896900.00 ns, 10.1390 ms/op
WorkloadWarmup   2: 64 op, 633903000.00 ns, 9.9047 ms/op
WorkloadWarmup   3: 64 op, 618740500.00 ns, 9.6678 ms/op
WorkloadWarmup   4: 64 op, 666560200.00 ns, 10.4150 ms/op
WorkloadWarmup   5: 64 op, 530494800.00 ns, 8.2890 ms/op
WorkloadWarmup   6: 64 op, 286989900.00 ns, 4.4842 ms/op
WorkloadWarmup   7: 64 op, 326985100.00 ns, 5.1091 ms/op
WorkloadWarmup   8: 64 op, 292359900.00 ns, 4.5681 ms/op

// BeforeActualRun
WorkloadActual   1: 64 op, 313064800.00 ns, 4.8916 ms/op
WorkloadActual   2: 64 op, 289241000.00 ns, 4.5194 ms/op
WorkloadActual   3: 64 op, 297381500.00 ns, 4.6466 ms/op
WorkloadActual   4: 64 op, 321076600.00 ns, 5.0168 ms/op
WorkloadActual   5: 64 op, 391943200.00 ns, 6.1241 ms/op
WorkloadActual   6: 64 op, 364392100.00 ns, 5.6936 ms/op
WorkloadActual   7: 64 op, 356911100.00 ns, 5.5767 ms/op
WorkloadActual   8: 64 op, 655307000.00 ns, 10.2392 ms/op
WorkloadActual   9: 64 op, 791771800.00 ns, 12.3714 ms/op
WorkloadActual  10: 64 op, 774549000.00 ns, 12.1023 ms/op
WorkloadActual  11: 64 op, 768997900.00 ns, 12.0156 ms/op
WorkloadActual  12: 64 op, 772215700.00 ns, 12.0659 ms/op
WorkloadActual  13: 64 op, 360043600.00 ns, 5.6257 ms/op
WorkloadActual  14: 64 op, 341314600.00 ns, 5.3330 ms/op
WorkloadActual  15: 64 op, 385513100.00 ns, 6.0236 ms/op
WorkloadActual  16: 64 op, 329719300.00 ns, 5.1519 ms/op
WorkloadActual  17: 64 op, 315484700.00 ns, 4.9294 ms/op
WorkloadActual  18: 64 op, 305108000.00 ns, 4.7673 ms/op
WorkloadActual  19: 64 op, 328909700.00 ns, 5.1392 ms/op
WorkloadActual  20: 64 op, 344348100.00 ns, 5.3804 ms/op
WorkloadActual  21: 64 op, 345751700.00 ns, 5.4024 ms/op
WorkloadActual  22: 64 op, 390684900.00 ns, 6.1045 ms/op
WorkloadActual  23: 64 op, 788307300.00 ns, 12.3173 ms/op
WorkloadActual  24: 64 op, 751387300.00 ns, 11.7404 ms/op
WorkloadActual  25: 64 op, 799953000.00 ns, 12.4993 ms/op
WorkloadActual  26: 64 op, 745166300.00 ns, 11.6432 ms/op
WorkloadActual  27: 64 op, 604025600.00 ns, 9.4379 ms/op
WorkloadActual  28: 64 op, 338633900.00 ns, 5.2912 ms/op
WorkloadActual  29: 64 op, 359125200.00 ns, 5.6113 ms/op
WorkloadActual  30: 64 op, 380424800.00 ns, 5.9441 ms/op
WorkloadActual  31: 64 op, 324199800.00 ns, 5.0656 ms/op
WorkloadActual  32: 64 op, 363117000.00 ns, 5.6737 ms/op
WorkloadActual  33: 64 op, 361139000.00 ns, 5.6428 ms/op
WorkloadActual  34: 64 op, 346766400.00 ns, 5.4182 ms/op
WorkloadActual  35: 64 op, 327814600.00 ns, 5.1221 ms/op
WorkloadActual  36: 64 op, 381979000.00 ns, 5.9684 ms/op
WorkloadActual  37: 64 op, 483167200.00 ns, 7.5495 ms/op
WorkloadActual  38: 64 op, 774352300.00 ns, 12.0993 ms/op
WorkloadActual  39: 64 op, 719624000.00 ns, 11.2441 ms/op
WorkloadActual  40: 64 op, 761475500.00 ns, 11.8981 ms/op
WorkloadActual  41: 64 op, 754459900.00 ns, 11.7884 ms/op
WorkloadActual  42: 64 op, 496673100.00 ns, 7.7605 ms/op
WorkloadActual  43: 64 op, 363185800.00 ns, 5.6748 ms/op
WorkloadActual  44: 64 op, 387953200.00 ns, 6.0618 ms/op
WorkloadActual  45: 64 op, 414127900.00 ns, 6.4707 ms/op
WorkloadActual  46: 64 op, 349287500.00 ns, 5.4576 ms/op
WorkloadActual  47: 64 op, 368484800.00 ns, 5.7576 ms/op
WorkloadActual  48: 64 op, 429247900.00 ns, 6.7070 ms/op
WorkloadActual  49: 64 op, 373003400.00 ns, 5.8282 ms/op
WorkloadActual  50: 64 op, 397437200.00 ns, 6.2100 ms/op
WorkloadActual  51: 64 op, 709495900.00 ns, 11.0859 ms/op
WorkloadActual  52: 64 op, 880235700.00 ns, 13.7537 ms/op
WorkloadActual  53: 64 op, 941251600.00 ns, 14.7071 ms/op
WorkloadActual  54: 64 op, 872504200.00 ns, 13.6329 ms/op
WorkloadActual  55: 64 op, 953583100.00 ns, 14.8997 ms/op
WorkloadActual  56: 64 op, 756610600.00 ns, 11.8220 ms/op
WorkloadActual  57: 64 op, 389927200.00 ns, 6.0926 ms/op
WorkloadActual  58: 64 op, 343992000.00 ns, 5.3749 ms/op
WorkloadActual  59: 64 op, 384325600.00 ns, 6.0051 ms/op
WorkloadActual  60: 64 op, 332563300.00 ns, 5.1963 ms/op
WorkloadActual  61: 64 op, 407840300.00 ns, 6.3725 ms/op
WorkloadActual  62: 64 op, 355414700.00 ns, 5.5534 ms/op
WorkloadActual  63: 64 op, 318878000.00 ns, 4.9825 ms/op
WorkloadActual  64: 64 op, 331046300.00 ns, 5.1726 ms/op
WorkloadActual  65: 64 op, 372037100.00 ns, 5.8131 ms/op
WorkloadActual  66: 64 op, 683139800.00 ns, 10.6741 ms/op
WorkloadActual  67: 64 op, 757856800.00 ns, 11.8415 ms/op
WorkloadActual  68: 64 op, 739154800.00 ns, 11.5493 ms/op
WorkloadActual  69: 64 op, 769006500.00 ns, 12.0157 ms/op
WorkloadActual  70: 64 op, 749492500.00 ns, 11.7108 ms/op
WorkloadActual  71: 64 op, 369106300.00 ns, 5.7673 ms/op
WorkloadActual  72: 64 op, 334224200.00 ns, 5.2223 ms/op
WorkloadActual  73: 64 op, 349308800.00 ns, 5.4580 ms/op
WorkloadActual  74: 64 op, 335154500.00 ns, 5.2368 ms/op
WorkloadActual  75: 64 op, 300109200.00 ns, 4.6892 ms/op
WorkloadActual  76: 64 op, 305575700.00 ns, 4.7746 ms/op
WorkloadActual  77: 64 op, 325064400.00 ns, 5.0791 ms/op
WorkloadActual  78: 64 op, 295407900.00 ns, 4.6157 ms/op
WorkloadActual  79: 64 op, 306134000.00 ns, 4.7833 ms/op
WorkloadActual  80: 64 op, 295713100.00 ns, 4.6205 ms/op
WorkloadActual  81: 64 op, 414472600.00 ns, 6.4761 ms/op
WorkloadActual  82: 64 op, 726013100.00 ns, 11.3440 ms/op
WorkloadActual  83: 64 op, 698632400.00 ns, 10.9161 ms/op
WorkloadActual  84: 64 op, 673344800.00 ns, 10.5210 ms/op
WorkloadActual  85: 64 op, 861697600.00 ns, 13.4640 ms/op
WorkloadActual  86: 64 op, 543841600.00 ns, 8.4975 ms/op
WorkloadActual  87: 64 op, 286782100.00 ns, 4.4810 ms/op
WorkloadActual  88: 64 op, 292150400.00 ns, 4.5649 ms/op
WorkloadActual  89: 64 op, 324077200.00 ns, 5.0637 ms/op
WorkloadActual  90: 64 op, 320647400.00 ns, 5.0101 ms/op
WorkloadActual  91: 64 op, 308586400.00 ns, 4.8217 ms/op
WorkloadActual  92: 64 op, 307505800.00 ns, 4.8048 ms/op
WorkloadActual  93: 64 op, 297089200.00 ns, 4.6420 ms/op
WorkloadActual  94: 64 op, 316154100.00 ns, 4.9399 ms/op
WorkloadActual  95: 64 op, 301757100.00 ns, 4.7150 ms/op
WorkloadActual  96: 64 op, 325865000.00 ns, 5.0916 ms/op
WorkloadActual  97: 64 op, 364541100.00 ns, 5.6960 ms/op
WorkloadActual  98: 64 op, 683666300.00 ns, 10.6823 ms/op
WorkloadActual  99: 64 op, 659892900.00 ns, 10.3108 ms/op
WorkloadActual  100: 64 op, 669242600.00 ns, 10.4569 ms/op

// AfterActualRun
WorkloadResult   1: 64 op, 313063000.00 ns, 4.8916 ms/op
WorkloadResult   2: 64 op, 289239200.00 ns, 4.5194 ms/op
WorkloadResult   3: 64 op, 297379700.00 ns, 4.6466 ms/op
WorkloadResult   4: 64 op, 321074800.00 ns, 5.0168 ms/op
WorkloadResult   5: 64 op, 391941400.00 ns, 6.1241 ms/op
WorkloadResult   6: 64 op, 364390300.00 ns, 5.6936 ms/op
WorkloadResult   7: 64 op, 356909300.00 ns, 5.5767 ms/op
WorkloadResult   8: 64 op, 655305200.00 ns, 10.2391 ms/op
WorkloadResult   9: 64 op, 791770000.00 ns, 12.3714 ms/op
WorkloadResult  10: 64 op, 774547200.00 ns, 12.1023 ms/op
WorkloadResult  11: 64 op, 768996100.00 ns, 12.0156 ms/op
WorkloadResult  12: 64 op, 772213900.00 ns, 12.0658 ms/op
WorkloadResult  13: 64 op, 360041800.00 ns, 5.6257 ms/op
WorkloadResult  14: 64 op, 341312800.00 ns, 5.3330 ms/op
WorkloadResult  15: 64 op, 385511300.00 ns, 6.0236 ms/op
WorkloadResult  16: 64 op, 329717500.00 ns, 5.1518 ms/op
WorkloadResult  17: 64 op, 315482900.00 ns, 4.9294 ms/op
WorkloadResult  18: 64 op, 305106200.00 ns, 4.7673 ms/op
WorkloadResult  19: 64 op, 328907900.00 ns, 5.1392 ms/op
WorkloadResult  20: 64 op, 344346300.00 ns, 5.3804 ms/op
WorkloadResult  21: 64 op, 345749900.00 ns, 5.4023 ms/op
WorkloadResult  22: 64 op, 390683100.00 ns, 6.1044 ms/op
WorkloadResult  23: 64 op, 788305500.00 ns, 12.3173 ms/op
WorkloadResult  24: 64 op, 751385500.00 ns, 11.7404 ms/op
WorkloadResult  25: 64 op, 799951200.00 ns, 12.4992 ms/op
WorkloadResult  26: 64 op, 745164500.00 ns, 11.6432 ms/op
WorkloadResult  27: 64 op, 604023800.00 ns, 9.4379 ms/op
WorkloadResult  28: 64 op, 338632100.00 ns, 5.2911 ms/op
WorkloadResult  29: 64 op, 359123400.00 ns, 5.6113 ms/op
WorkloadResult  30: 64 op, 380423000.00 ns, 5.9441 ms/op
WorkloadResult  31: 64 op, 324198000.00 ns, 5.0656 ms/op
WorkloadResult  32: 64 op, 363115200.00 ns, 5.6737 ms/op
WorkloadResult  33: 64 op, 361137200.00 ns, 5.6428 ms/op
WorkloadResult  34: 64 op, 346764600.00 ns, 5.4182 ms/op
WorkloadResult  35: 64 op, 327812800.00 ns, 5.1221 ms/op
WorkloadResult  36: 64 op, 381977200.00 ns, 5.9684 ms/op
WorkloadResult  37: 64 op, 483165400.00 ns, 7.5495 ms/op
WorkloadResult  38: 64 op, 774350500.00 ns, 12.0992 ms/op
WorkloadResult  39: 64 op, 719622200.00 ns, 11.2441 ms/op
WorkloadResult  40: 64 op, 761473700.00 ns, 11.8980 ms/op
WorkloadResult  41: 64 op, 754458100.00 ns, 11.7884 ms/op
WorkloadResult  42: 64 op, 496671300.00 ns, 7.7605 ms/op
WorkloadResult  43: 64 op, 363184000.00 ns, 5.6748 ms/op
WorkloadResult  44: 64 op, 387951400.00 ns, 6.0617 ms/op
WorkloadResult  45: 64 op, 414126100.00 ns, 6.4707 ms/op
WorkloadResult  46: 64 op, 349285700.00 ns, 5.4576 ms/op
WorkloadResult  47: 64 op, 368483000.00 ns, 5.7575 ms/op
WorkloadResult  48: 64 op, 429246100.00 ns, 6.7070 ms/op
WorkloadResult  49: 64 op, 373001600.00 ns, 5.8282 ms/op
WorkloadResult  50: 64 op, 397435400.00 ns, 6.2099 ms/op
WorkloadResult  51: 64 op, 709494100.00 ns, 11.0858 ms/op
WorkloadResult  52: 64 op, 880233900.00 ns, 13.7537 ms/op
WorkloadResult  53: 64 op, 941249800.00 ns, 14.7070 ms/op
WorkloadResult  54: 64 op, 872502400.00 ns, 13.6329 ms/op
WorkloadResult  55: 64 op, 953581300.00 ns, 14.8997 ms/op
WorkloadResult  56: 64 op, 756608800.00 ns, 11.8220 ms/op
WorkloadResult  57: 64 op, 389925400.00 ns, 6.0926 ms/op
WorkloadResult  58: 64 op, 343990200.00 ns, 5.3748 ms/op
WorkloadResult  59: 64 op, 384323800.00 ns, 6.0051 ms/op
WorkloadResult  60: 64 op, 332561500.00 ns, 5.1963 ms/op
WorkloadResult  61: 64 op, 407838500.00 ns, 6.3725 ms/op
WorkloadResult  62: 64 op, 355412900.00 ns, 5.5533 ms/op
WorkloadResult  63: 64 op, 318876200.00 ns, 4.9824 ms/op
WorkloadResult  64: 64 op, 331044500.00 ns, 5.1726 ms/op
WorkloadResult  65: 64 op, 372035300.00 ns, 5.8131 ms/op
WorkloadResult  66: 64 op, 683138000.00 ns, 10.6740 ms/op
WorkloadResult  67: 64 op, 757855000.00 ns, 11.8415 ms/op
WorkloadResult  68: 64 op, 739153000.00 ns, 11.5493 ms/op
WorkloadResult  69: 64 op, 769004700.00 ns, 12.0157 ms/op
WorkloadResult  70: 64 op, 749490700.00 ns, 11.7108 ms/op
WorkloadResult  71: 64 op, 369104500.00 ns, 5.7673 ms/op
WorkloadResult  72: 64 op, 334222400.00 ns, 5.2222 ms/op
WorkloadResult  73: 64 op, 349307000.00 ns, 5.4579 ms/op
WorkloadResult  74: 64 op, 335152700.00 ns, 5.2368 ms/op
WorkloadResult  75: 64 op, 300107400.00 ns, 4.6892 ms/op
WorkloadResult  76: 64 op, 305573900.00 ns, 4.7746 ms/op
WorkloadResult  77: 64 op, 325062600.00 ns, 5.0791 ms/op
WorkloadResult  78: 64 op, 295406100.00 ns, 4.6157 ms/op
WorkloadResult  79: 64 op, 306132200.00 ns, 4.7833 ms/op
WorkloadResult  80: 64 op, 295711300.00 ns, 4.6205 ms/op
WorkloadResult  81: 64 op, 414470800.00 ns, 6.4761 ms/op
WorkloadResult  82: 64 op, 726011300.00 ns, 11.3439 ms/op
WorkloadResult  83: 64 op, 698630600.00 ns, 10.9161 ms/op
WorkloadResult  84: 64 op, 673343000.00 ns, 10.5210 ms/op
WorkloadResult  85: 64 op, 861695800.00 ns, 13.4640 ms/op
WorkloadResult  86: 64 op, 543839800.00 ns, 8.4975 ms/op
WorkloadResult  87: 64 op, 286780300.00 ns, 4.4809 ms/op
WorkloadResult  88: 64 op, 292148600.00 ns, 4.5648 ms/op
WorkloadResult  89: 64 op, 324075400.00 ns, 5.0637 ms/op
WorkloadResult  90: 64 op, 320645600.00 ns, 5.0101 ms/op
WorkloadResult  91: 64 op, 308584600.00 ns, 4.8216 ms/op
WorkloadResult  92: 64 op, 307504000.00 ns, 4.8048 ms/op
WorkloadResult  93: 64 op, 297087400.00 ns, 4.6420 ms/op
WorkloadResult  94: 64 op, 316152300.00 ns, 4.9399 ms/op
WorkloadResult  95: 64 op, 301755300.00 ns, 4.7149 ms/op
WorkloadResult  96: 64 op, 325863200.00 ns, 5.0916 ms/op
WorkloadResult  97: 64 op, 364539300.00 ns, 5.6959 ms/op
WorkloadResult  98: 64 op, 683664500.00 ns, 10.6823 ms/op
WorkloadResult  99: 64 op, 659891100.00 ns, 10.3108 ms/op
WorkloadResult  100: 64 op, 669240800.00 ns, 10.4569 ms/op

// AfterAll
// Benchmark Process 13272 has exited with code 0.

Mean = 7.520 ms, StdErr = 0.312 ms (4.15%), N = 100, StdDev = 3.121 ms
Min = 4.481 ms, Q1 = 5.135 ms, Median = 5.790 ms, Q3 = 10.741 ms, Max = 14.900 ms
IQR = 5.606 ms, LowerFence = -3.274 ms, UpperFence = 19.149 ms
ConfidenceInterval = [6.462 ms; 8.578 ms] (CI 99.9%), Margin = 1.058 ms (14.07% of Mean)
Skewness = 0.81, Kurtosis = 2.03, MValue = 2.81

// **************************
// Benchmark: WriteText.RunFile: .NET Framework 4.6.1(Runtime=.NET Framework 4.6.1)
// *** Execute ***
// Launch: 1 / 1
// Execute: D:\Microsoft\Workspace\asakura89-project\release\AskScratchpad\CSScratchpad\CSScratchpad\bin\Release\883981ac-c734-42b2-8df1-c05314150d15.exe --benchmarkName "CSScratchpad.Script.WriteText.RunFile" --job ".NET Framework 4.6.1" --benchmarkId 2 in 
// BeforeAnythingElse

// Benchmark Process Environment Information:
// Runtime=.NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
// GC=Concurrent Workstation
// Job: .NET Framework 4.6.1

OverheadJitting  1: 1 op, 435400.00 ns, 435.4000 us/op
WorkloadJitting  1: 1 op, 36740000.00 ns, 36.7400 ms/op

WorkloadPilot    1: 2 op, 29028200.00 ns, 14.5141 ms/op
WorkloadPilot    2: 3 op, 33690600.00 ns, 11.2302 ms/op
WorkloadPilot    3: 4 op, 42339900.00 ns, 10.5850 ms/op
WorkloadPilot    4: 5 op, 61118700.00 ns, 12.2237 ms/op
WorkloadPilot    5: 6 op, 99460800.00 ns, 16.5768 ms/op
WorkloadPilot    6: 7 op, 83638100.00 ns, 11.9483 ms/op
WorkloadPilot    7: 8 op, 97106300.00 ns, 12.1383 ms/op
WorkloadPilot    8: 9 op, 42404700.00 ns, 4.7116 ms/op
WorkloadPilot    9: 10 op, 51713600.00 ns, 5.1714 ms/op
WorkloadPilot   10: 11 op, 60561100.00 ns, 5.5056 ms/op
WorkloadPilot   11: 12 op, 66788800.00 ns, 5.5657 ms/op
WorkloadPilot   12: 13 op, 60465600.00 ns, 4.6512 ms/op
WorkloadPilot   13: 14 op, 65049700.00 ns, 4.6464 ms/op
WorkloadPilot   14: 15 op, 77130200.00 ns, 5.1420 ms/op
WorkloadPilot   15: 16 op, 84693300.00 ns, 5.2933 ms/op
WorkloadPilot   16: 32 op, 150604600.00 ns, 4.7064 ms/op
WorkloadPilot   17: 64 op, 297550000.00 ns, 4.6492 ms/op
WorkloadPilot   18: 128 op, 602094400.00 ns, 4.7039 ms/op

WorkloadWarmup   1: 128 op, 710280000.00 ns, 5.5491 ms/op
WorkloadWarmup   2: 128 op, 749793500.00 ns, 5.8578 ms/op
WorkloadWarmup   3: 128 op, 904279100.00 ns, 7.0647 ms/op
WorkloadWarmup   4: 128 op, 1428065300.00 ns, 11.1568 ms/op
WorkloadWarmup   5: 128 op, 1540869700.00 ns, 12.0380 ms/op
WorkloadWarmup   6: 128 op, 1332890800.00 ns, 10.4132 ms/op
WorkloadWarmup   7: 128 op, 605685900.00 ns, 4.7319 ms/op
WorkloadWarmup   8: 128 op, 607105800.00 ns, 4.7430 ms/op
WorkloadWarmup   9: 128 op, 598822800.00 ns, 4.6783 ms/op

// BeforeActualRun
WorkloadActual   1: 128 op, 649758100.00 ns, 5.0762 ms/op
WorkloadActual   2: 128 op, 668958500.00 ns, 5.2262 ms/op
WorkloadActual   3: 128 op, 990987300.00 ns, 7.7421 ms/op
WorkloadActual   4: 128 op, 1399889100.00 ns, 10.9366 ms/op
WorkloadActual   5: 128 op, 1451493500.00 ns, 11.3398 ms/op
WorkloadActual   6: 128 op, 671116000.00 ns, 5.2431 ms/op
WorkloadActual   7: 128 op, 685383200.00 ns, 5.3546 ms/op
WorkloadActual   8: 128 op, 600318400.00 ns, 4.6900 ms/op
WorkloadActual   9: 128 op, 614696400.00 ns, 4.8023 ms/op
WorkloadActual  10: 128 op, 664755700.00 ns, 5.1934 ms/op
WorkloadActual  11: 128 op, 1114996200.00 ns, 8.7109 ms/op
WorkloadActual  12: 128 op, 1400292800.00 ns, 10.9398 ms/op
WorkloadActual  13: 128 op, 1486061800.00 ns, 11.6099 ms/op
WorkloadActual  14: 128 op, 1235150600.00 ns, 9.6496 ms/op
WorkloadActual  15: 128 op, 602922300.00 ns, 4.7103 ms/op
WorkloadActual  16: 128 op, 648895300.00 ns, 5.0695 ms/op
WorkloadActual  17: 128 op, 594388000.00 ns, 4.6437 ms/op
WorkloadActual  18: 128 op, 673319600.00 ns, 5.2603 ms/op
WorkloadActual  19: 128 op, 679620200.00 ns, 5.3095 ms/op
WorkloadActual  20: 128 op, 1421007500.00 ns, 11.1016 ms/op
WorkloadActual  21: 128 op, 1549791800.00 ns, 12.1077 ms/op
WorkloadActual  22: 128 op, 1475698300.00 ns, 11.5289 ms/op
WorkloadActual  23: 128 op, 864671900.00 ns, 6.7552 ms/op
WorkloadActual  24: 128 op, 683379400.00 ns, 5.3389 ms/op
WorkloadActual  25: 128 op, 649375100.00 ns, 5.0732 ms/op
WorkloadActual  26: 128 op, 612984900.00 ns, 4.7889 ms/op
WorkloadActual  27: 128 op, 609685100.00 ns, 4.7632 ms/op
WorkloadActual  28: 128 op, 805037200.00 ns, 6.2894 ms/op
WorkloadActual  29: 128 op, 1486921300.00 ns, 11.6166 ms/op
WorkloadActual  30: 128 op, 1394922500.00 ns, 10.8978 ms/op
WorkloadActual  31: 128 op, 917061400.00 ns, 7.1645 ms/op
WorkloadActual  32: 128 op, 699353300.00 ns, 5.4637 ms/op
WorkloadActual  33: 128 op, 700652800.00 ns, 5.4739 ms/op
WorkloadActual  34: 128 op, 667569600.00 ns, 5.2154 ms/op
WorkloadActual  35: 128 op, 652968100.00 ns, 5.1013 ms/op
WorkloadActual  36: 128 op, 1050587800.00 ns, 8.2077 ms/op
WorkloadActual  37: 128 op, 1407130700.00 ns, 10.9932 ms/op
WorkloadActual  38: 128 op, 1492313000.00 ns, 11.6587 ms/op
WorkloadActual  39: 128 op, 1211315600.00 ns, 9.4634 ms/op
WorkloadActual  40: 128 op, 636819900.00 ns, 4.9752 ms/op
WorkloadActual  41: 128 op, 652768200.00 ns, 5.0998 ms/op
WorkloadActual  42: 128 op, 652638700.00 ns, 5.0987 ms/op
WorkloadActual  43: 128 op, 609005100.00 ns, 4.7579 ms/op
WorkloadActual  44: 128 op, 703889500.00 ns, 5.4991 ms/op
WorkloadActual  45: 128 op, 1566408500.00 ns, 12.2376 ms/op
WorkloadActual  46: 128 op, 1454504200.00 ns, 11.3633 ms/op
WorkloadActual  47: 128 op, 1444291100.00 ns, 11.2835 ms/op
WorkloadActual  48: 128 op, 832420500.00 ns, 6.5033 ms/op
WorkloadActual  49: 128 op, 612165500.00 ns, 4.7825 ms/op
WorkloadActual  50: 128 op, 615180600.00 ns, 4.8061 ms/op
WorkloadActual  51: 128 op, 679656000.00 ns, 5.3098 ms/op
WorkloadActual  52: 128 op, 620268000.00 ns, 4.8458 ms/op
WorkloadActual  53: 128 op, 780904200.00 ns, 6.1008 ms/op
WorkloadActual  54: 128 op, 1572912400.00 ns, 12.2884 ms/op
WorkloadActual  55: 128 op, 1468689000.00 ns, 11.4741 ms/op
WorkloadActual  56: 128 op, 1482727800.00 ns, 11.5838 ms/op
WorkloadActual  57: 128 op, 672168300.00 ns, 5.2513 ms/op
WorkloadActual  58: 128 op, 638059600.00 ns, 4.9848 ms/op
WorkloadActual  59: 128 op, 626890800.00 ns, 4.8976 ms/op
WorkloadActual  60: 128 op, 600309100.00 ns, 4.6899 ms/op
WorkloadActual  61: 128 op, 650308900.00 ns, 5.0805 ms/op
WorkloadActual  62: 128 op, 1106680200.00 ns, 8.6459 ms/op
WorkloadActual  63: 128 op, 1344504900.00 ns, 10.5039 ms/op
WorkloadActual  64: 128 op, 1430550800.00 ns, 11.1762 ms/op
WorkloadActual  65: 128 op, 1234096700.00 ns, 9.6414 ms/op
WorkloadActual  66: 128 op, 628628200.00 ns, 4.9112 ms/op
WorkloadActual  67: 128 op, 658103900.00 ns, 5.1414 ms/op
WorkloadActual  68: 128 op, 686860100.00 ns, 5.3661 ms/op
WorkloadActual  69: 128 op, 605295700.00 ns, 4.7289 ms/op
WorkloadActual  70: 128 op, 727451700.00 ns, 5.6832 ms/op
WorkloadActual  71: 128 op, 1609098200.00 ns, 12.5711 ms/op
WorkloadActual  72: 128 op, 1559360400.00 ns, 12.1825 ms/op
WorkloadActual  73: 128 op, 1458407600.00 ns, 11.3938 ms/op
WorkloadActual  74: 128 op, 762420600.00 ns, 5.9564 ms/op
WorkloadActual  75: 128 op, 594504700.00 ns, 4.6446 ms/op
WorkloadActual  76: 128 op, 604741800.00 ns, 4.7245 ms/op
WorkloadActual  77: 128 op, 600937500.00 ns, 4.6948 ms/op
WorkloadActual  78: 128 op, 626950500.00 ns, 4.8981 ms/op
WorkloadActual  79: 128 op, 709759900.00 ns, 5.5450 ms/op
WorkloadActual  80: 128 op, 1339162300.00 ns, 10.4622 ms/op
WorkloadActual  81: 128 op, 1408606900.00 ns, 11.0047 ms/op
WorkloadActual  82: 128 op, 1377287100.00 ns, 10.7601 ms/op
WorkloadActual  83: 128 op, 840227800.00 ns, 6.5643 ms/op
WorkloadActual  84: 128 op, 625892800.00 ns, 4.8898 ms/op
WorkloadActual  85: 128 op, 653767900.00 ns, 5.1076 ms/op
WorkloadActual  86: 128 op, 640871300.00 ns, 5.0068 ms/op
WorkloadActual  87: 128 op, 666941900.00 ns, 5.2105 ms/op
WorkloadActual  88: 128 op, 746038500.00 ns, 5.8284 ms/op
WorkloadActual  89: 128 op, 1419089100.00 ns, 11.0866 ms/op
WorkloadActual  90: 128 op, 1410803500.00 ns, 11.0219 ms/op
WorkloadActual  91: 128 op, 1347510400.00 ns, 10.5274 ms/op
WorkloadActual  92: 128 op, 870148800.00 ns, 6.7980 ms/op
WorkloadActual  93: 128 op, 603514300.00 ns, 4.7150 ms/op
WorkloadActual  94: 128 op, 597491700.00 ns, 4.6679 ms/op
WorkloadActual  95: 128 op, 610461500.00 ns, 4.7692 ms/op
WorkloadActual  96: 128 op, 635384500.00 ns, 4.9639 ms/op
WorkloadActual  97: 128 op, 820944100.00 ns, 6.4136 ms/op
WorkloadActual  98: 128 op, 1452491700.00 ns, 11.3476 ms/op
WorkloadActual  99: 128 op, 1595326700.00 ns, 12.4635 ms/op
WorkloadActual  100: 128 op, 1421640900.00 ns, 11.1066 ms/op

// AfterActualRun
WorkloadResult   1: 128 op, 649758100.00 ns, 5.0762 ms/op
WorkloadResult   2: 128 op, 668958500.00 ns, 5.2262 ms/op
WorkloadResult   3: 128 op, 990987300.00 ns, 7.7421 ms/op
WorkloadResult   4: 128 op, 1399889100.00 ns, 10.9366 ms/op
WorkloadResult   5: 128 op, 1451493500.00 ns, 11.3398 ms/op
WorkloadResult   6: 128 op, 671116000.00 ns, 5.2431 ms/op
WorkloadResult   7: 128 op, 685383200.00 ns, 5.3546 ms/op
WorkloadResult   8: 128 op, 600318400.00 ns, 4.6900 ms/op
WorkloadResult   9: 128 op, 614696400.00 ns, 4.8023 ms/op
WorkloadResult  10: 128 op, 664755700.00 ns, 5.1934 ms/op
WorkloadResult  11: 128 op, 1114996200.00 ns, 8.7109 ms/op
WorkloadResult  12: 128 op, 1400292800.00 ns, 10.9398 ms/op
WorkloadResult  13: 128 op, 1486061800.00 ns, 11.6099 ms/op
WorkloadResult  14: 128 op, 1235150600.00 ns, 9.6496 ms/op
WorkloadResult  15: 128 op, 602922300.00 ns, 4.7103 ms/op
WorkloadResult  16: 128 op, 648895300.00 ns, 5.0695 ms/op
WorkloadResult  17: 128 op, 594388000.00 ns, 4.6437 ms/op
WorkloadResult  18: 128 op, 673319600.00 ns, 5.2603 ms/op
WorkloadResult  19: 128 op, 679620200.00 ns, 5.3095 ms/op
WorkloadResult  20: 128 op, 1421007500.00 ns, 11.1016 ms/op
WorkloadResult  21: 128 op, 1549791800.00 ns, 12.1077 ms/op
WorkloadResult  22: 128 op, 1475698300.00 ns, 11.5289 ms/op
WorkloadResult  23: 128 op, 864671900.00 ns, 6.7552 ms/op
WorkloadResult  24: 128 op, 683379400.00 ns, 5.3389 ms/op
WorkloadResult  25: 128 op, 649375100.00 ns, 5.0732 ms/op
WorkloadResult  26: 128 op, 612984900.00 ns, 4.7889 ms/op
WorkloadResult  27: 128 op, 609685100.00 ns, 4.7632 ms/op
WorkloadResult  28: 128 op, 805037200.00 ns, 6.2894 ms/op
WorkloadResult  29: 128 op, 1486921300.00 ns, 11.6166 ms/op
WorkloadResult  30: 128 op, 1394922500.00 ns, 10.8978 ms/op
WorkloadResult  31: 128 op, 917061400.00 ns, 7.1645 ms/op
WorkloadResult  32: 128 op, 699353300.00 ns, 5.4637 ms/op
WorkloadResult  33: 128 op, 700652800.00 ns, 5.4739 ms/op
WorkloadResult  34: 128 op, 667569600.00 ns, 5.2154 ms/op
WorkloadResult  35: 128 op, 652968100.00 ns, 5.1013 ms/op
WorkloadResult  36: 128 op, 1050587800.00 ns, 8.2077 ms/op
WorkloadResult  37: 128 op, 1407130700.00 ns, 10.9932 ms/op
WorkloadResult  38: 128 op, 1492313000.00 ns, 11.6587 ms/op
WorkloadResult  39: 128 op, 1211315600.00 ns, 9.4634 ms/op
WorkloadResult  40: 128 op, 636819900.00 ns, 4.9752 ms/op
WorkloadResult  41: 128 op, 652768200.00 ns, 5.0998 ms/op
WorkloadResult  42: 128 op, 652638700.00 ns, 5.0987 ms/op
WorkloadResult  43: 128 op, 609005100.00 ns, 4.7579 ms/op
WorkloadResult  44: 128 op, 703889500.00 ns, 5.4991 ms/op
WorkloadResult  45: 128 op, 1566408500.00 ns, 12.2376 ms/op
WorkloadResult  46: 128 op, 1454504200.00 ns, 11.3633 ms/op
WorkloadResult  47: 128 op, 1444291100.00 ns, 11.2835 ms/op
WorkloadResult  48: 128 op, 832420500.00 ns, 6.5033 ms/op
WorkloadResult  49: 128 op, 612165500.00 ns, 4.7825 ms/op
WorkloadResult  50: 128 op, 615180600.00 ns, 4.8061 ms/op
WorkloadResult  51: 128 op, 679656000.00 ns, 5.3098 ms/op
WorkloadResult  52: 128 op, 620268000.00 ns, 4.8458 ms/op
WorkloadResult  53: 128 op, 780904200.00 ns, 6.1008 ms/op
WorkloadResult  54: 128 op, 1572912400.00 ns, 12.2884 ms/op
WorkloadResult  55: 128 op, 1468689000.00 ns, 11.4741 ms/op
WorkloadResult  56: 128 op, 1482727800.00 ns, 11.5838 ms/op
WorkloadResult  57: 128 op, 672168300.00 ns, 5.2513 ms/op
WorkloadResult  58: 128 op, 638059600.00 ns, 4.9848 ms/op
WorkloadResult  59: 128 op, 626890800.00 ns, 4.8976 ms/op
WorkloadResult  60: 128 op, 600309100.00 ns, 4.6899 ms/op
WorkloadResult  61: 128 op, 650308900.00 ns, 5.0805 ms/op
WorkloadResult  62: 128 op, 1106680200.00 ns, 8.6459 ms/op
WorkloadResult  63: 128 op, 1344504900.00 ns, 10.5039 ms/op
WorkloadResult  64: 128 op, 1430550800.00 ns, 11.1762 ms/op
WorkloadResult  65: 128 op, 1234096700.00 ns, 9.6414 ms/op
WorkloadResult  66: 128 op, 628628200.00 ns, 4.9112 ms/op
WorkloadResult  67: 128 op, 658103900.00 ns, 5.1414 ms/op
WorkloadResult  68: 128 op, 686860100.00 ns, 5.3661 ms/op
WorkloadResult  69: 128 op, 605295700.00 ns, 4.7289 ms/op
WorkloadResult  70: 128 op, 727451700.00 ns, 5.6832 ms/op
WorkloadResult  71: 128 op, 1609098200.00 ns, 12.5711 ms/op
WorkloadResult  72: 128 op, 1559360400.00 ns, 12.1825 ms/op
WorkloadResult  73: 128 op, 1458407600.00 ns, 11.3938 ms/op
WorkloadResult  74: 128 op, 762420600.00 ns, 5.9564 ms/op
WorkloadResult  75: 128 op, 594504700.00 ns, 4.6446 ms/op
WorkloadResult  76: 128 op, 604741800.00 ns, 4.7245 ms/op
WorkloadResult  77: 128 op, 600937500.00 ns, 4.6948 ms/op
WorkloadResult  78: 128 op, 626950500.00 ns, 4.8981 ms/op
WorkloadResult  79: 128 op, 709759900.00 ns, 5.5450 ms/op
WorkloadResult  80: 128 op, 1339162300.00 ns, 10.4622 ms/op
WorkloadResult  81: 128 op, 1408606900.00 ns, 11.0047 ms/op
WorkloadResult  82: 128 op, 1377287100.00 ns, 10.7601 ms/op
WorkloadResult  83: 128 op, 840227800.00 ns, 6.5643 ms/op
WorkloadResult  84: 128 op, 625892800.00 ns, 4.8898 ms/op
WorkloadResult  85: 128 op, 653767900.00 ns, 5.1076 ms/op
WorkloadResult  86: 128 op, 640871300.00 ns, 5.0068 ms/op
WorkloadResult  87: 128 op, 666941900.00 ns, 5.2105 ms/op
WorkloadResult  88: 128 op, 746038500.00 ns, 5.8284 ms/op
WorkloadResult  89: 128 op, 1419089100.00 ns, 11.0866 ms/op
WorkloadResult  90: 128 op, 1410803500.00 ns, 11.0219 ms/op
WorkloadResult  91: 128 op, 1347510400.00 ns, 10.5274 ms/op
WorkloadResult  92: 128 op, 870148800.00 ns, 6.7980 ms/op
WorkloadResult  93: 128 op, 603514300.00 ns, 4.7150 ms/op
WorkloadResult  94: 128 op, 597491700.00 ns, 4.6679 ms/op
WorkloadResult  95: 128 op, 610461500.00 ns, 4.7692 ms/op
WorkloadResult  96: 128 op, 635384500.00 ns, 4.9639 ms/op
WorkloadResult  97: 128 op, 820944100.00 ns, 6.4136 ms/op
WorkloadResult  98: 128 op, 1452491700.00 ns, 11.3476 ms/op
WorkloadResult  99: 128 op, 1595326700.00 ns, 12.4635 ms/op
WorkloadResult  100: 128 op, 1421640900.00 ns, 11.1066 ms/op

// AfterAll
// Benchmark Process 19652 has exited with code 0.

Mean = 7.405 ms, StdErr = 0.289 ms (3.90%), N = 100, StdDev = 2.885 ms
Min = 4.644 ms, Q1 = 5.001 ms, Median = 5.522 ms, Q3 = 10.937 ms, Max = 12.571 ms
IQR = 5.936 ms, LowerFence = -3.903 ms, UpperFence = 19.842 ms
ConfidenceInterval = [6.427 ms; 8.384 ms] (CI 99.9%), Margin = 0.979 ms (13.21% of Mean)
Skewness = 0.56, Kurtosis = 1.52, MValue = 2.98

// **************************
// Benchmark: WriteText.RunTextWriter: .NET Framework 4.7.2(Runtime=.NET Framework 4.7.2)
// *** Execute ***
// Launch: 1 / 1
// Execute: D:\Microsoft\Workspace\asakura89-project\release\AskScratchpad\CSScratchpad\CSScratchpad\bin\Release\337d5ef9-6727-4016-a9ba-d698494aa95b.exe --benchmarkName "CSScratchpad.Script.WriteText.RunTextWriter" --job ".NET Framework 4.7.2" --benchmarkId 0 in 
// BeforeAnythingElse

// Benchmark Process Environment Information:
// Runtime=.NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
// GC=Concurrent Workstation
// Job: .NET Framework 4.7.2

OverheadJitting  1: 1 op, 202200.00 ns, 202.2000 us/op
WorkloadJitting  1: 1 op, 13205400.00 ns, 13.2054 ms/op

OverheadJitting  2: 16 op, 142600.00 ns, 8.9125 us/op
WorkloadJitting  2: 16 op, 79955000.00 ns, 4.9972 ms/op

WorkloadPilot    1: 16 op, 93823500.00 ns, 5.8640 ms/op
WorkloadPilot    2: 32 op, 176935600.00 ns, 5.5292 ms/op
WorkloadPilot    3: 64 op, 366434700.00 ns, 5.7255 ms/op
WorkloadPilot    4: 128 op, 593799800.00 ns, 4.6391 ms/op

OverheadWarmup   1: 128 op, 6400.00 ns, 50.0000 ns/op
OverheadWarmup   2: 128 op, 1000.00 ns, 7.8125 ns/op
OverheadWarmup   3: 128 op, 1900.00 ns, 14.8438 ns/op
OverheadWarmup   4: 128 op, 1000.00 ns, 7.8125 ns/op
OverheadWarmup   5: 128 op, 900.00 ns, 7.0313 ns/op
OverheadWarmup   6: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadWarmup   7: 128 op, 1000.00 ns, 7.8125 ns/op

OverheadActual   1: 128 op, 1300.00 ns, 10.1563 ns/op
OverheadActual   2: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadActual   3: 128 op, 2000.00 ns, 15.6250 ns/op
OverheadActual   4: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadActual   5: 128 op, 2200.00 ns, 17.1875 ns/op
OverheadActual   6: 128 op, 3000.00 ns, 23.4375 ns/op
OverheadActual   7: 128 op, 900.00 ns, 7.0313 ns/op
OverheadActual   8: 128 op, 1900.00 ns, 14.8438 ns/op
OverheadActual   9: 128 op, 2800.00 ns, 21.8750 ns/op
OverheadActual  10: 128 op, 1200.00 ns, 9.3750 ns/op
OverheadActual  11: 128 op, 2100.00 ns, 16.4063 ns/op
OverheadActual  12: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadActual  13: 128 op, 2400.00 ns, 18.7500 ns/op
OverheadActual  14: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual  15: 128 op, 1300.00 ns, 10.1563 ns/op
OverheadActual  16: 128 op, 1500.00 ns, 11.7188 ns/op
OverheadActual  17: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual  18: 128 op, 700.00 ns, 5.4688 ns/op
OverheadActual  19: 128 op, 2000.00 ns, 15.6250 ns/op
OverheadActual  20: 128 op, 800.00 ns, 6.2500 ns/op

WorkloadWarmup   1: 128 op, 675626700.00 ns, 5.2783 ms/op
WorkloadWarmup   2: 128 op, 610587500.00 ns, 4.7702 ms/op
WorkloadWarmup   3: 128 op, 1099758900.00 ns, 8.5919 ms/op
WorkloadWarmup   4: 128 op, 1412725000.00 ns, 11.0369 ms/op
WorkloadWarmup   5: 128 op, 1583966000.00 ns, 12.3747 ms/op
WorkloadWarmup   6: 128 op, 1092644400.00 ns, 8.5363 ms/op
WorkloadWarmup   7: 128 op, 696118100.00 ns, 5.4384 ms/op
WorkloadWarmup   8: 128 op, 694327500.00 ns, 5.4244 ms/op
WorkloadWarmup   9: 128 op, 677417000.00 ns, 5.2923 ms/op
WorkloadWarmup  10: 128 op, 621191800.00 ns, 4.8531 ms/op
WorkloadWarmup  11: 128 op, 646040800.00 ns, 5.0472 ms/op
WorkloadWarmup  12: 128 op, 1401792500.00 ns, 10.9515 ms/op
WorkloadWarmup  13: 128 op, 1392315900.00 ns, 10.8775 ms/op

// BeforeActualRun
WorkloadActual   1: 128 op, 1402607500.00 ns, 10.9579 ms/op
WorkloadActual   2: 128 op, 893125300.00 ns, 6.9775 ms/op
WorkloadActual   3: 128 op, 699638500.00 ns, 5.4659 ms/op
WorkloadActual   4: 128 op, 655945600.00 ns, 5.1246 ms/op
WorkloadActual   5: 128 op, 617240300.00 ns, 4.8222 ms/op
WorkloadActual   6: 128 op, 634641100.00 ns, 4.9581 ms/op
WorkloadActual   7: 128 op, 976023100.00 ns, 7.6252 ms/op
WorkloadActual   8: 128 op, 1432503700.00 ns, 11.1914 ms/op
WorkloadActual   9: 128 op, 1481788000.00 ns, 11.5765 ms/op
WorkloadActual  10: 128 op, 1388950100.00 ns, 10.8512 ms/op
WorkloadActual  11: 128 op, 599507700.00 ns, 4.6837 ms/op
WorkloadActual  12: 128 op, 650773600.00 ns, 5.0842 ms/op
WorkloadActual  13: 128 op, 609094700.00 ns, 4.7586 ms/op
WorkloadActual  14: 128 op, 617385500.00 ns, 4.8233 ms/op
WorkloadActual  15: 128 op, 635186500.00 ns, 4.9624 ms/op
WorkloadActual  16: 128 op, 1000778300.00 ns, 7.8186 ms/op
WorkloadActual  17: 128 op, 1467310700.00 ns, 11.4634 ms/op
WorkloadActual  18: 128 op, 1649393500.00 ns, 12.8859 ms/op
WorkloadActual  19: 128 op, 1196680700.00 ns, 9.3491 ms/op
WorkloadActual  20: 128 op, 634190900.00 ns, 4.9546 ms/op
WorkloadActual  21: 128 op, 641518700.00 ns, 5.0119 ms/op
WorkloadActual  22: 128 op, 631140900.00 ns, 4.9308 ms/op
WorkloadActual  23: 128 op, 636023900.00 ns, 4.9689 ms/op
WorkloadActual  24: 128 op, 669320500.00 ns, 5.2291 ms/op
WorkloadActual  25: 128 op, 1358166500.00 ns, 10.6107 ms/op
WorkloadActual  26: 128 op, 1386645200.00 ns, 10.8332 ms/op
WorkloadActual  27: 128 op, 1418461500.00 ns, 11.0817 ms/op
WorkloadActual  28: 128 op, 959754100.00 ns, 7.4981 ms/op
WorkloadActual  29: 128 op, 647830100.00 ns, 5.0612 ms/op
WorkloadActual  30: 128 op, 705406800.00 ns, 5.5110 ms/op
WorkloadActual  31: 128 op, 616651100.00 ns, 4.8176 ms/op
WorkloadActual  32: 128 op, 638705100.00 ns, 4.9899 ms/op
WorkloadActual  33: 128 op, 714382600.00 ns, 5.5811 ms/op
WorkloadActual  34: 128 op, 1372722800.00 ns, 10.7244 ms/op
WorkloadActual  35: 128 op, 1495330800.00 ns, 11.6823 ms/op
WorkloadActual  36: 128 op, 1441768400.00 ns, 11.2638 ms/op
WorkloadActual  37: 128 op, 762071900.00 ns, 5.9537 ms/op
WorkloadActual  38: 128 op, 614635000.00 ns, 4.8018 ms/op
WorkloadActual  39: 128 op, 608627100.00 ns, 4.7549 ms/op
WorkloadActual  40: 128 op, 654275200.00 ns, 5.1115 ms/op
WorkloadActual  41: 128 op, 768743000.00 ns, 6.0058 ms/op
WorkloadActual  42: 128 op, 954772200.00 ns, 7.4592 ms/op
WorkloadActual  43: 128 op, 1478126600.00 ns, 11.5479 ms/op
WorkloadActual  44: 128 op, 1670630100.00 ns, 13.0518 ms/op
WorkloadActual  45: 128 op, 1212024100.00 ns, 9.4689 ms/op
WorkloadActual  46: 128 op, 838731800.00 ns, 6.5526 ms/op
WorkloadActual  47: 128 op, 1007560600.00 ns, 7.8716 ms/op
WorkloadActual  48: 128 op, 690641800.00 ns, 5.3956 ms/op
WorkloadActual  49: 128 op, 705255700.00 ns, 5.5098 ms/op
WorkloadActual  50: 128 op, 1719969700.00 ns, 13.4373 ms/op
WorkloadActual  51: 128 op, 1851917000.00 ns, 14.4681 ms/op
WorkloadActual  52: 128 op, 1644039500.00 ns, 12.8441 ms/op
WorkloadActual  53: 128 op, 1175313000.00 ns, 9.1821 ms/op
WorkloadActual  54: 128 op, 703865000.00 ns, 5.4989 ms/op
WorkloadActual  55: 128 op, 680467500.00 ns, 5.3162 ms/op
WorkloadActual  56: 128 op, 773819300.00 ns, 6.0455 ms/op
WorkloadActual  57: 128 op, 868085900.00 ns, 6.7819 ms/op
WorkloadActual  58: 128 op, 1627431200.00 ns, 12.7143 ms/op
WorkloadActual  59: 128 op, 1506430100.00 ns, 11.7690 ms/op
WorkloadActual  60: 128 op, 1559017400.00 ns, 12.1798 ms/op
WorkloadActual  61: 128 op, 1495278300.00 ns, 11.6819 ms/op
WorkloadActual  62: 128 op, 714113500.00 ns, 5.5790 ms/op
WorkloadActual  63: 128 op, 766563700.00 ns, 5.9888 ms/op
WorkloadActual  64: 128 op, 689817800.00 ns, 5.3892 ms/op
WorkloadActual  65: 128 op, 676626000.00 ns, 5.2861 ms/op
WorkloadActual  66: 128 op, 971855500.00 ns, 7.5926 ms/op
WorkloadActual  67: 128 op, 1450936500.00 ns, 11.3354 ms/op
WorkloadActual  68: 128 op, 1817006300.00 ns, 14.1954 ms/op
WorkloadActual  69: 128 op, 1423770200.00 ns, 11.1232 ms/op
WorkloadActual  70: 128 op, 791901600.00 ns, 6.1867 ms/op
WorkloadActual  71: 128 op, 711600600.00 ns, 5.5594 ms/op
WorkloadActual  72: 128 op, 671844900.00 ns, 5.2488 ms/op
WorkloadActual  73: 128 op, 622864000.00 ns, 4.8661 ms/op
WorkloadActual  74: 128 op, 1326427100.00 ns, 10.3627 ms/op
WorkloadActual  75: 128 op, 1532561100.00 ns, 11.9731 ms/op
WorkloadActual  76: 128 op, 1603528000.00 ns, 12.5276 ms/op
WorkloadActual  77: 128 op, 1523053200.00 ns, 11.8989 ms/op
WorkloadActual  78: 128 op, 921804100.00 ns, 7.2016 ms/op
WorkloadActual  79: 128 op, 728773700.00 ns, 5.6935 ms/op
WorkloadActual  80: 128 op, 687281700.00 ns, 5.3694 ms/op
WorkloadActual  81: 128 op, 621763600.00 ns, 4.8575 ms/op
WorkloadActual  82: 128 op, 651745500.00 ns, 5.0918 ms/op
WorkloadActual  83: 128 op, 1383733700.00 ns, 10.8104 ms/op
WorkloadActual  84: 128 op, 1417897500.00 ns, 11.0773 ms/op
WorkloadActual  85: 128 op, 1638489200.00 ns, 12.8007 ms/op
WorkloadActual  86: 128 op, 858274700.00 ns, 6.7053 ms/op
WorkloadActual  87: 128 op, 847581500.00 ns, 6.6217 ms/op
WorkloadActual  88: 128 op, 1003350200.00 ns, 7.8387 ms/op
WorkloadActual  89: 128 op, 663361900.00 ns, 5.1825 ms/op
WorkloadActual  90: 128 op, 923071300.00 ns, 7.2115 ms/op
WorkloadActual  91: 128 op, 1473970700.00 ns, 11.5154 ms/op
WorkloadActual  92: 128 op, 1492174800.00 ns, 11.6576 ms/op
WorkloadActual  93: 128 op, 1444731000.00 ns, 11.2870 ms/op
WorkloadActual  94: 128 op, 1228059200.00 ns, 9.5942 ms/op
WorkloadActual  95: 128 op, 663408700.00 ns, 5.1829 ms/op
WorkloadActual  96: 128 op, 658772700.00 ns, 5.1467 ms/op
WorkloadActual  97: 128 op, 711568700.00 ns, 5.5591 ms/op
WorkloadActual  98: 128 op, 633141900.00 ns, 4.9464 ms/op
WorkloadActual  99: 128 op, 651668200.00 ns, 5.0912 ms/op
WorkloadActual  100: 128 op, 1437822000.00 ns, 11.2330 ms/op

// AfterActualRun
WorkloadResult   1: 128 op, 1402606200.00 ns, 10.9579 ms/op
WorkloadResult   2: 128 op, 893124000.00 ns, 6.9775 ms/op
WorkloadResult   3: 128 op, 699637200.00 ns, 5.4659 ms/op
WorkloadResult   4: 128 op, 655944300.00 ns, 5.1246 ms/op
WorkloadResult   5: 128 op, 617239000.00 ns, 4.8222 ms/op
WorkloadResult   6: 128 op, 634639800.00 ns, 4.9581 ms/op
WorkloadResult   7: 128 op, 976021800.00 ns, 7.6252 ms/op
WorkloadResult   8: 128 op, 1432502400.00 ns, 11.1914 ms/op
WorkloadResult   9: 128 op, 1481786700.00 ns, 11.5765 ms/op
WorkloadResult  10: 128 op, 1388948800.00 ns, 10.8512 ms/op
WorkloadResult  11: 128 op, 599506400.00 ns, 4.6836 ms/op
WorkloadResult  12: 128 op, 650772300.00 ns, 5.0842 ms/op
WorkloadResult  13: 128 op, 609093400.00 ns, 4.7585 ms/op
WorkloadResult  14: 128 op, 617384200.00 ns, 4.8233 ms/op
WorkloadResult  15: 128 op, 635185200.00 ns, 4.9624 ms/op
WorkloadResult  16: 128 op, 1000777000.00 ns, 7.8186 ms/op
WorkloadResult  17: 128 op, 1467309400.00 ns, 11.4634 ms/op
WorkloadResult  18: 128 op, 1649392200.00 ns, 12.8859 ms/op
WorkloadResult  19: 128 op, 1196679400.00 ns, 9.3491 ms/op
WorkloadResult  20: 128 op, 634189600.00 ns, 4.9546 ms/op
WorkloadResult  21: 128 op, 641517400.00 ns, 5.0119 ms/op
WorkloadResult  22: 128 op, 631139600.00 ns, 4.9308 ms/op
WorkloadResult  23: 128 op, 636022600.00 ns, 4.9689 ms/op
WorkloadResult  24: 128 op, 669319200.00 ns, 5.2291 ms/op
WorkloadResult  25: 128 op, 1358165200.00 ns, 10.6107 ms/op
WorkloadResult  26: 128 op, 1386643900.00 ns, 10.8332 ms/op
WorkloadResult  27: 128 op, 1418460200.00 ns, 11.0817 ms/op
WorkloadResult  28: 128 op, 959752800.00 ns, 7.4981 ms/op
WorkloadResult  29: 128 op, 647828800.00 ns, 5.0612 ms/op
WorkloadResult  30: 128 op, 705405500.00 ns, 5.5110 ms/op
WorkloadResult  31: 128 op, 616649800.00 ns, 4.8176 ms/op
WorkloadResult  32: 128 op, 638703800.00 ns, 4.9899 ms/op
WorkloadResult  33: 128 op, 714381300.00 ns, 5.5811 ms/op
WorkloadResult  34: 128 op, 1372721500.00 ns, 10.7244 ms/op
WorkloadResult  35: 128 op, 1495329500.00 ns, 11.6823 ms/op
WorkloadResult  36: 128 op, 1441767100.00 ns, 11.2638 ms/op
WorkloadResult  37: 128 op, 762070600.00 ns, 5.9537 ms/op
WorkloadResult  38: 128 op, 614633700.00 ns, 4.8018 ms/op
WorkloadResult  39: 128 op, 608625800.00 ns, 4.7549 ms/op
WorkloadResult  40: 128 op, 654273900.00 ns, 5.1115 ms/op
WorkloadResult  41: 128 op, 768741700.00 ns, 6.0058 ms/op
WorkloadResult  42: 128 op, 954770900.00 ns, 7.4591 ms/op
WorkloadResult  43: 128 op, 1478125300.00 ns, 11.5479 ms/op
WorkloadResult  44: 128 op, 1670628800.00 ns, 13.0518 ms/op
WorkloadResult  45: 128 op, 1212022800.00 ns, 9.4689 ms/op
WorkloadResult  46: 128 op, 838730500.00 ns, 6.5526 ms/op
WorkloadResult  47: 128 op, 1007559300.00 ns, 7.8716 ms/op
WorkloadResult  48: 128 op, 690640500.00 ns, 5.3956 ms/op
WorkloadResult  49: 128 op, 705254400.00 ns, 5.5098 ms/op
WorkloadResult  50: 128 op, 1719968400.00 ns, 13.4373 ms/op
WorkloadResult  51: 128 op, 1851915700.00 ns, 14.4681 ms/op
WorkloadResult  52: 128 op, 1644038200.00 ns, 12.8440 ms/op
WorkloadResult  53: 128 op, 1175311700.00 ns, 9.1821 ms/op
WorkloadResult  54: 128 op, 703863700.00 ns, 5.4989 ms/op
WorkloadResult  55: 128 op, 680466200.00 ns, 5.3161 ms/op
WorkloadResult  56: 128 op, 773818000.00 ns, 6.0455 ms/op
WorkloadResult  57: 128 op, 868084600.00 ns, 6.7819 ms/op
WorkloadResult  58: 128 op, 1627429900.00 ns, 12.7143 ms/op
WorkloadResult  59: 128 op, 1506428800.00 ns, 11.7690 ms/op
WorkloadResult  60: 128 op, 1559016100.00 ns, 12.1798 ms/op
WorkloadResult  61: 128 op, 1495277000.00 ns, 11.6819 ms/op
WorkloadResult  62: 128 op, 714112200.00 ns, 5.5790 ms/op
WorkloadResult  63: 128 op, 766562400.00 ns, 5.9888 ms/op
WorkloadResult  64: 128 op, 689816500.00 ns, 5.3892 ms/op
WorkloadResult  65: 128 op, 676624700.00 ns, 5.2861 ms/op
WorkloadResult  66: 128 op, 971854200.00 ns, 7.5926 ms/op
WorkloadResult  67: 128 op, 1450935200.00 ns, 11.3354 ms/op
WorkloadResult  68: 128 op, 1817005000.00 ns, 14.1954 ms/op
WorkloadResult  69: 128 op, 1423768900.00 ns, 11.1232 ms/op
WorkloadResult  70: 128 op, 791900300.00 ns, 6.1867 ms/op
WorkloadResult  71: 128 op, 711599300.00 ns, 5.5594 ms/op
WorkloadResult  72: 128 op, 671843600.00 ns, 5.2488 ms/op
WorkloadResult  73: 128 op, 622862700.00 ns, 4.8661 ms/op
WorkloadResult  74: 128 op, 1326425800.00 ns, 10.3627 ms/op
WorkloadResult  75: 128 op, 1532559800.00 ns, 11.9731 ms/op
WorkloadResult  76: 128 op, 1603526700.00 ns, 12.5276 ms/op
WorkloadResult  77: 128 op, 1523051900.00 ns, 11.8988 ms/op
WorkloadResult  78: 128 op, 921802800.00 ns, 7.2016 ms/op
WorkloadResult  79: 128 op, 728772400.00 ns, 5.6935 ms/op
WorkloadResult  80: 128 op, 687280400.00 ns, 5.3694 ms/op
WorkloadResult  81: 128 op, 621762300.00 ns, 4.8575 ms/op
WorkloadResult  82: 128 op, 651744200.00 ns, 5.0918 ms/op
WorkloadResult  83: 128 op, 1383732400.00 ns, 10.8104 ms/op
WorkloadResult  84: 128 op, 1417896200.00 ns, 11.0773 ms/op
WorkloadResult  85: 128 op, 1638487900.00 ns, 12.8007 ms/op
WorkloadResult  86: 128 op, 858273400.00 ns, 6.7053 ms/op
WorkloadResult  87: 128 op, 847580200.00 ns, 6.6217 ms/op
WorkloadResult  88: 128 op, 1003348900.00 ns, 7.8387 ms/op
WorkloadResult  89: 128 op, 663360600.00 ns, 5.1825 ms/op
WorkloadResult  90: 128 op, 923070000.00 ns, 7.2115 ms/op
WorkloadResult  91: 128 op, 1473969400.00 ns, 11.5154 ms/op
WorkloadResult  92: 128 op, 1492173500.00 ns, 11.6576 ms/op
WorkloadResult  93: 128 op, 1444729700.00 ns, 11.2870 ms/op
WorkloadResult  94: 128 op, 1228057900.00 ns, 9.5942 ms/op
WorkloadResult  95: 128 op, 663407400.00 ns, 5.1829 ms/op
WorkloadResult  96: 128 op, 658771400.00 ns, 5.1467 ms/op
WorkloadResult  97: 128 op, 711567400.00 ns, 5.5591 ms/op
WorkloadResult  98: 128 op, 633140600.00 ns, 4.9464 ms/op
WorkloadResult  99: 128 op, 651666900.00 ns, 5.0911 ms/op
WorkloadResult  100: 128 op, 1437820700.00 ns, 11.2330 ms/op

// AfterAll
// Benchmark Process 14604 has exited with code 0.

Mean = 7.983 ms, StdErr = 0.305 ms (3.82%), N = 100, StdDev = 3.046 ms
Min = 4.684 ms, Q1 = 5.183 ms, Median = 6.744 ms, Q3 = 11.140 ms, Max = 14.468 ms
IQR = 5.957 ms, LowerFence = -3.753 ms, UpperFence = 20.076 ms
ConfidenceInterval = [6.950 ms; 9.016 ms] (CI 99.9%), Margin = 1.033 ms (12.94% of Mean)
Skewness = 0.47, Kurtosis = 1.61, MValue = 3.06

// **************************
// Benchmark: WriteText.RunFileStream: .NET Framework 4.7.2(Runtime=.NET Framework 4.7.2)
// *** Execute ***
// Launch: 1 / 1
// Execute: D:\Microsoft\Workspace\asakura89-project\release\AskScratchpad\CSScratchpad\CSScratchpad\bin\Release\337d5ef9-6727-4016-a9ba-d698494aa95b.exe --benchmarkName "CSScratchpad.Script.WriteText.RunFileStream" --job ".NET Framework 4.7.2" --benchmarkId 1 in 
// BeforeAnythingElse

// Benchmark Process Environment Information:
// Runtime=.NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
// GC=Concurrent Workstation
// Job: .NET Framework 4.7.2

OverheadJitting  1: 1 op, 474800.00 ns, 474.8000 us/op
WorkloadJitting  1: 1 op, 31598800.00 ns, 31.5988 ms/op

OverheadJitting  2: 16 op, 640300.00 ns, 40.0188 us/op
WorkloadJitting  2: 16 op, 174016600.00 ns, 10.8760 ms/op

WorkloadPilot    1: 16 op, 180344400.00 ns, 11.2715 ms/op
WorkloadPilot    2: 32 op, 330662700.00 ns, 10.3332 ms/op
WorkloadPilot    3: 64 op, 761528700.00 ns, 11.8989 ms/op

OverheadWarmup   1: 64 op, 6600.00 ns, 103.1250 ns/op
OverheadWarmup   2: 64 op, 1600.00 ns, 25.0000 ns/op
OverheadWarmup   3: 64 op, 1700.00 ns, 26.5625 ns/op
OverheadWarmup   4: 64 op, 2600.00 ns, 40.6250 ns/op
OverheadWarmup   5: 64 op, 2900.00 ns, 45.3125 ns/op
OverheadWarmup   6: 64 op, 1300.00 ns, 20.3125 ns/op
OverheadWarmup   7: 64 op, 1100.00 ns, 17.1875 ns/op
OverheadWarmup   8: 64 op, 2600.00 ns, 40.6250 ns/op
OverheadWarmup   9: 64 op, 2000.00 ns, 31.2500 ns/op

OverheadActual   1: 64 op, 1300.00 ns, 20.3125 ns/op
OverheadActual   2: 64 op, 1400.00 ns, 21.8750 ns/op
OverheadActual   3: 64 op, 1300.00 ns, 20.3125 ns/op
OverheadActual   4: 64 op, 1700.00 ns, 26.5625 ns/op
OverheadActual   5: 64 op, 1500.00 ns, 23.4375 ns/op
OverheadActual   6: 64 op, 3300.00 ns, 51.5625 ns/op
OverheadActual   7: 64 op, 2000.00 ns, 31.2500 ns/op
OverheadActual   8: 64 op, 1400.00 ns, 21.8750 ns/op
OverheadActual   9: 64 op, 1300.00 ns, 20.3125 ns/op
OverheadActual  10: 64 op, 2400.00 ns, 37.5000 ns/op
OverheadActual  11: 64 op, 1600.00 ns, 25.0000 ns/op
OverheadActual  12: 64 op, 1600.00 ns, 25.0000 ns/op
OverheadActual  13: 64 op, 1700.00 ns, 26.5625 ns/op
OverheadActual  14: 64 op, 1300.00 ns, 20.3125 ns/op
OverheadActual  15: 64 op, 1200.00 ns, 18.7500 ns/op
OverheadActual  16: 64 op, 2000.00 ns, 31.2500 ns/op
OverheadActual  17: 64 op, 2500.00 ns, 39.0625 ns/op
OverheadActual  18: 64 op, 2700.00 ns, 42.1875 ns/op
OverheadActual  19: 64 op, 1600.00 ns, 25.0000 ns/op
OverheadActual  20: 64 op, 1600.00 ns, 25.0000 ns/op

WorkloadWarmup   1: 64 op, 713450500.00 ns, 11.1477 ms/op
WorkloadWarmup   2: 64 op, 298831000.00 ns, 4.6692 ms/op
WorkloadWarmup   3: 64 op, 313125600.00 ns, 4.8926 ms/op
WorkloadWarmup   4: 64 op, 337016100.00 ns, 5.2659 ms/op
WorkloadWarmup   5: 64 op, 332144000.00 ns, 5.1898 ms/op
WorkloadWarmup   6: 64 op, 376480800.00 ns, 5.8825 ms/op
WorkloadWarmup   7: 64 op, 347137800.00 ns, 5.4240 ms/op

// BeforeActualRun
WorkloadActual   1: 64 op, 316292600.00 ns, 4.9421 ms/op
WorkloadActual   2: 64 op, 318675200.00 ns, 4.9793 ms/op
WorkloadActual   3: 64 op, 306257900.00 ns, 4.7853 ms/op
WorkloadActual   4: 64 op, 308370800.00 ns, 4.8183 ms/op
WorkloadActual   5: 64 op, 424946200.00 ns, 6.6398 ms/op
WorkloadActual   6: 64 op, 811819400.00 ns, 12.6847 ms/op
WorkloadActual   7: 64 op, 756531100.00 ns, 11.8208 ms/op
WorkloadActual   8: 64 op, 740855100.00 ns, 11.5759 ms/op
WorkloadActual   9: 64 op, 764717700.00 ns, 11.9487 ms/op
WorkloadActual  10: 64 op, 734341300.00 ns, 11.4741 ms/op
WorkloadActual  11: 64 op, 748305700.00 ns, 11.6923 ms/op
WorkloadActual  12: 64 op, 678171400.00 ns, 10.5964 ms/op
WorkloadActual  13: 64 op, 594218700.00 ns, 9.2847 ms/op
WorkloadActual  14: 64 op, 307291300.00 ns, 4.8014 ms/op
WorkloadActual  15: 64 op, 341384900.00 ns, 5.3341 ms/op
WorkloadActual  16: 64 op, 306384100.00 ns, 4.7873 ms/op
WorkloadActual  17: 64 op, 301021100.00 ns, 4.7035 ms/op
WorkloadActual  18: 64 op, 295942500.00 ns, 4.6241 ms/op
WorkloadActual  19: 64 op, 313052600.00 ns, 4.8914 ms/op
WorkloadActual  20: 64 op, 311863700.00 ns, 4.8729 ms/op
WorkloadActual  21: 64 op, 297526600.00 ns, 4.6489 ms/op
WorkloadActual  22: 64 op, 307066100.00 ns, 4.7979 ms/op
WorkloadActual  23: 64 op, 323618700.00 ns, 5.0565 ms/op
WorkloadActual  24: 64 op, 398762700.00 ns, 6.2307 ms/op
WorkloadActual  25: 64 op, 797965100.00 ns, 12.4682 ms/op
WorkloadActual  26: 64 op, 781076000.00 ns, 12.2043 ms/op
WorkloadActual  27: 64 op, 698779200.00 ns, 10.9184 ms/op
WorkloadActual  28: 64 op, 667281800.00 ns, 10.4263 ms/op
WorkloadActual  29: 64 op, 775953900.00 ns, 12.1243 ms/op
WorkloadActual  30: 64 op, 908149300.00 ns, 14.1898 ms/op
WorkloadActual  31: 64 op, 325742000.00 ns, 5.0897 ms/op
WorkloadActual  32: 64 op, 305055800.00 ns, 4.7665 ms/op
WorkloadActual  33: 64 op, 328067600.00 ns, 5.1261 ms/op
WorkloadActual  34: 64 op, 308298600.00 ns, 4.8172 ms/op
WorkloadActual  35: 64 op, 348925400.00 ns, 5.4520 ms/op
WorkloadActual  36: 64 op, 319905700.00 ns, 4.9985 ms/op
WorkloadActual  37: 64 op, 353524000.00 ns, 5.5238 ms/op
WorkloadActual  38: 64 op, 389209700.00 ns, 6.0814 ms/op
WorkloadActual  39: 64 op, 334141300.00 ns, 5.2210 ms/op
WorkloadActual  40: 64 op, 339475100.00 ns, 5.3043 ms/op
WorkloadActual  41: 64 op, 635103400.00 ns, 9.9235 ms/op
WorkloadActual  42: 64 op, 740923600.00 ns, 11.5769 ms/op
WorkloadActual  43: 64 op, 769479200.00 ns, 12.0231 ms/op
WorkloadActual  44: 64 op, 743205600.00 ns, 11.6126 ms/op
WorkloadActual  45: 64 op, 747275600.00 ns, 11.6762 ms/op
WorkloadActual  46: 64 op, 749284200.00 ns, 11.7076 ms/op
WorkloadActual  47: 64 op, 757698100.00 ns, 11.8390 ms/op
WorkloadActual  48: 64 op, 716247100.00 ns, 11.1914 ms/op
WorkloadActual  49: 64 op, 408776100.00 ns, 6.3871 ms/op
WorkloadActual  50: 64 op, 343492000.00 ns, 5.3671 ms/op
WorkloadActual  51: 64 op, 358467900.00 ns, 5.6011 ms/op
WorkloadActual  52: 64 op, 352833800.00 ns, 5.5130 ms/op
WorkloadActual  53: 64 op, 468147600.00 ns, 7.3148 ms/op
WorkloadActual  54: 64 op, 341359500.00 ns, 5.3337 ms/op
WorkloadActual  55: 64 op, 357851800.00 ns, 5.5914 ms/op
WorkloadActual  56: 64 op, 338966200.00 ns, 5.2963 ms/op
WorkloadActual  57: 64 op, 336656200.00 ns, 5.2603 ms/op
WorkloadActual  58: 64 op, 341387700.00 ns, 5.3342 ms/op
WorkloadActual  59: 64 op, 746836600.00 ns, 11.6693 ms/op
WorkloadActual  60: 64 op, 1279410600.00 ns, 19.9908 ms/op
WorkloadActual  61: 64 op, 31921646300.00 ns, 498.7757 ms/op
WorkloadActual  62: 64 op, 318883600.00 ns, 4.9826 ms/op
WorkloadActual  63: 64 op, 342808400.00 ns, 5.3564 ms/op
WorkloadActual  64: 64 op, 312945600.00 ns, 4.8898 ms/op
WorkloadActual  65: 64 op, 313416700.00 ns, 4.8971 ms/op
WorkloadActual  66: 64 op, 312884600.00 ns, 4.8888 ms/op
WorkloadActual  67: 64 op, 365878800.00 ns, 5.7169 ms/op
WorkloadActual  68: 64 op, 343530800.00 ns, 5.3677 ms/op
WorkloadActual  69: 64 op, 498191500.00 ns, 7.7842 ms/op
WorkloadActual  70: 64 op, 801883500.00 ns, 12.5294 ms/op
WorkloadActual  71: 64 op, 731502700.00 ns, 11.4297 ms/op
WorkloadActual  72: 64 op, 845682800.00 ns, 13.2138 ms/op
WorkloadActual  73: 64 op, 777955500.00 ns, 12.1556 ms/op
WorkloadActual  74: 64 op, 360439200.00 ns, 5.6319 ms/op
WorkloadActual  75: 64 op, 319929800.00 ns, 4.9989 ms/op
WorkloadActual  76: 64 op, 333505800.00 ns, 5.2110 ms/op
WorkloadActual  77: 64 op, 320568100.00 ns, 5.0089 ms/op
WorkloadActual  78: 64 op, 301928400.00 ns, 4.7176 ms/op
WorkloadActual  79: 64 op, 390813000.00 ns, 6.1065 ms/op
WorkloadActual  80: 64 op, 296921500.00 ns, 4.6394 ms/op
WorkloadActual  81: 64 op, 318509400.00 ns, 4.9767 ms/op
WorkloadActual  82: 64 op, 301202400.00 ns, 4.7063 ms/op
WorkloadActual  83: 64 op, 322199800.00 ns, 5.0344 ms/op
WorkloadActual  84: 64 op, 308261700.00 ns, 4.8166 ms/op
WorkloadActual  85: 64 op, 755147800.00 ns, 11.7992 ms/op
WorkloadActual  86: 64 op, 777355400.00 ns, 12.1462 ms/op
WorkloadActual  87: 64 op, 672073000.00 ns, 10.5011 ms/op
WorkloadActual  88: 64 op, 695677800.00 ns, 10.8700 ms/op
WorkloadActual  89: 64 op, 754382200.00 ns, 11.7872 ms/op
WorkloadActual  90: 64 op, 335042200.00 ns, 5.2350 ms/op
WorkloadActual  91: 64 op, 329573000.00 ns, 5.1496 ms/op
WorkloadActual  92: 64 op, 351647600.00 ns, 5.4945 ms/op
WorkloadActual  93: 64 op, 304600300.00 ns, 4.7594 ms/op
WorkloadActual  94: 64 op, 313933200.00 ns, 4.9052 ms/op
WorkloadActual  95: 64 op, 354272000.00 ns, 5.5355 ms/op
WorkloadActual  96: 64 op, 449458300.00 ns, 7.0228 ms/op
WorkloadActual  97: 64 op, 327002900.00 ns, 5.1094 ms/op
WorkloadActual  98: 64 op, 304834400.00 ns, 4.7630 ms/op
WorkloadActual  99: 64 op, 320282600.00 ns, 5.0044 ms/op
WorkloadActual  100: 64 op, 630436800.00 ns, 9.8506 ms/op

// AfterActualRun
WorkloadResult   1: 64 op, 316291000.00 ns, 4.9420 ms/op
WorkloadResult   2: 64 op, 318673600.00 ns, 4.9793 ms/op
WorkloadResult   3: 64 op, 306256300.00 ns, 4.7853 ms/op
WorkloadResult   4: 64 op, 308369200.00 ns, 4.8183 ms/op
WorkloadResult   5: 64 op, 424944600.00 ns, 6.6398 ms/op
WorkloadResult   6: 64 op, 811817800.00 ns, 12.6847 ms/op
WorkloadResult   7: 64 op, 756529500.00 ns, 11.8208 ms/op
WorkloadResult   8: 64 op, 740853500.00 ns, 11.5758 ms/op
WorkloadResult   9: 64 op, 764716100.00 ns, 11.9487 ms/op
WorkloadResult  10: 64 op, 734339700.00 ns, 11.4741 ms/op
WorkloadResult  11: 64 op, 748304100.00 ns, 11.6923 ms/op
WorkloadResult  12: 64 op, 678169800.00 ns, 10.5964 ms/op
WorkloadResult  13: 64 op, 594217100.00 ns, 9.2846 ms/op
WorkloadResult  14: 64 op, 307289700.00 ns, 4.8014 ms/op
WorkloadResult  15: 64 op, 341383300.00 ns, 5.3341 ms/op
WorkloadResult  16: 64 op, 306382500.00 ns, 4.7872 ms/op
WorkloadResult  17: 64 op, 301019500.00 ns, 4.7034 ms/op
WorkloadResult  18: 64 op, 295940900.00 ns, 4.6241 ms/op
WorkloadResult  19: 64 op, 313051000.00 ns, 4.8914 ms/op
WorkloadResult  20: 64 op, 311862100.00 ns, 4.8728 ms/op
WorkloadResult  21: 64 op, 297525000.00 ns, 4.6488 ms/op
WorkloadResult  22: 64 op, 307064500.00 ns, 4.7979 ms/op
WorkloadResult  23: 64 op, 323617100.00 ns, 5.0565 ms/op
WorkloadResult  24: 64 op, 398761100.00 ns, 6.2306 ms/op
WorkloadResult  25: 64 op, 797963500.00 ns, 12.4682 ms/op
WorkloadResult  26: 64 op, 781074400.00 ns, 12.2043 ms/op
WorkloadResult  27: 64 op, 698777600.00 ns, 10.9184 ms/op
WorkloadResult  28: 64 op, 667280200.00 ns, 10.4263 ms/op
WorkloadResult  29: 64 op, 775952300.00 ns, 12.1243 ms/op
WorkloadResult  30: 64 op, 908147700.00 ns, 14.1898 ms/op
WorkloadResult  31: 64 op, 325740400.00 ns, 5.0897 ms/op
WorkloadResult  32: 64 op, 305054200.00 ns, 4.7665 ms/op
WorkloadResult  33: 64 op, 328066000.00 ns, 5.1260 ms/op
WorkloadResult  34: 64 op, 308297000.00 ns, 4.8171 ms/op
WorkloadResult  35: 64 op, 348923800.00 ns, 5.4519 ms/op
WorkloadResult  36: 64 op, 319904100.00 ns, 4.9985 ms/op
WorkloadResult  37: 64 op, 353522400.00 ns, 5.5238 ms/op
WorkloadResult  38: 64 op, 389208100.00 ns, 6.0814 ms/op
WorkloadResult  39: 64 op, 334139700.00 ns, 5.2209 ms/op
WorkloadResult  40: 64 op, 339473500.00 ns, 5.3043 ms/op
WorkloadResult  41: 64 op, 635101800.00 ns, 9.9235 ms/op
WorkloadResult  42: 64 op, 740922000.00 ns, 11.5769 ms/op
WorkloadResult  43: 64 op, 769477600.00 ns, 12.0231 ms/op
WorkloadResult  44: 64 op, 743204000.00 ns, 11.6126 ms/op
WorkloadResult  45: 64 op, 747274000.00 ns, 11.6762 ms/op
WorkloadResult  46: 64 op, 749282600.00 ns, 11.7075 ms/op
WorkloadResult  47: 64 op, 757696500.00 ns, 11.8390 ms/op
WorkloadResult  48: 64 op, 716245500.00 ns, 11.1913 ms/op
WorkloadResult  49: 64 op, 408774500.00 ns, 6.3871 ms/op
WorkloadResult  50: 64 op, 343490400.00 ns, 5.3670 ms/op
WorkloadResult  51: 64 op, 358466300.00 ns, 5.6010 ms/op
WorkloadResult  52: 64 op, 352832200.00 ns, 5.5130 ms/op
WorkloadResult  53: 64 op, 468146000.00 ns, 7.3148 ms/op
WorkloadResult  54: 64 op, 341357900.00 ns, 5.3337 ms/op
WorkloadResult  55: 64 op, 357850200.00 ns, 5.5914 ms/op
WorkloadResult  56: 64 op, 338964600.00 ns, 5.2963 ms/op
WorkloadResult  57: 64 op, 336654600.00 ns, 5.2602 ms/op
WorkloadResult  58: 64 op, 341386100.00 ns, 5.3342 ms/op
WorkloadResult  59: 64 op, 746835000.00 ns, 11.6693 ms/op
WorkloadResult  60: 64 op, 1279409000.00 ns, 19.9908 ms/op
WorkloadResult  61: 64 op, 318882000.00 ns, 4.9825 ms/op
WorkloadResult  62: 64 op, 342806800.00 ns, 5.3564 ms/op
WorkloadResult  63: 64 op, 312944000.00 ns, 4.8898 ms/op
WorkloadResult  64: 64 op, 313415100.00 ns, 4.8971 ms/op
WorkloadResult  65: 64 op, 312883000.00 ns, 4.8888 ms/op
WorkloadResult  66: 64 op, 365877200.00 ns, 5.7168 ms/op
WorkloadResult  67: 64 op, 343529200.00 ns, 5.3676 ms/op
WorkloadResult  68: 64 op, 498189900.00 ns, 7.7842 ms/op
WorkloadResult  69: 64 op, 801881900.00 ns, 12.5294 ms/op
WorkloadResult  70: 64 op, 731501100.00 ns, 11.4297 ms/op
WorkloadResult  71: 64 op, 845681200.00 ns, 13.2138 ms/op
WorkloadResult  72: 64 op, 777953900.00 ns, 12.1555 ms/op
WorkloadResult  73: 64 op, 360437600.00 ns, 5.6318 ms/op
WorkloadResult  74: 64 op, 319928200.00 ns, 4.9989 ms/op
WorkloadResult  75: 64 op, 333504200.00 ns, 5.2110 ms/op
WorkloadResult  76: 64 op, 320566500.00 ns, 5.0089 ms/op
WorkloadResult  77: 64 op, 301926800.00 ns, 4.7176 ms/op
WorkloadResult  78: 64 op, 390811400.00 ns, 6.1064 ms/op
WorkloadResult  79: 64 op, 296919900.00 ns, 4.6394 ms/op
WorkloadResult  80: 64 op, 318507800.00 ns, 4.9767 ms/op
WorkloadResult  81: 64 op, 301200800.00 ns, 4.7063 ms/op
WorkloadResult  82: 64 op, 322198200.00 ns, 5.0343 ms/op
WorkloadResult  83: 64 op, 308260100.00 ns, 4.8166 ms/op
WorkloadResult  84: 64 op, 755146200.00 ns, 11.7992 ms/op
WorkloadResult  85: 64 op, 777353800.00 ns, 12.1462 ms/op
WorkloadResult  86: 64 op, 672071400.00 ns, 10.5011 ms/op
WorkloadResult  87: 64 op, 695676200.00 ns, 10.8699 ms/op
WorkloadResult  88: 64 op, 754380600.00 ns, 11.7872 ms/op
WorkloadResult  89: 64 op, 335040600.00 ns, 5.2350 ms/op
WorkloadResult  90: 64 op, 329571400.00 ns, 5.1496 ms/op
WorkloadResult  91: 64 op, 351646000.00 ns, 5.4945 ms/op
WorkloadResult  92: 64 op, 304598700.00 ns, 4.7594 ms/op
WorkloadResult  93: 64 op, 313931600.00 ns, 4.9052 ms/op
WorkloadResult  94: 64 op, 354270400.00 ns, 5.5355 ms/op
WorkloadResult  95: 64 op, 449456700.00 ns, 7.0228 ms/op
WorkloadResult  96: 64 op, 327001300.00 ns, 5.1094 ms/op
WorkloadResult  97: 64 op, 304832800.00 ns, 4.7630 ms/op
WorkloadResult  98: 64 op, 320281000.00 ns, 5.0044 ms/op
WorkloadResult  99: 64 op, 630435200.00 ns, 9.8506 ms/op

// AfterAll
// Benchmark Process 26380 has exited with code 0.

Mean = 7.534 ms, StdErr = 0.335 ms (4.45%), N = 99, StdDev = 3.333 ms
Min = 4.624 ms, Q1 = 4.981 ms, Median = 5.494 ms, Q3 = 11.311 ms, Max = 19.991 ms
IQR = 6.330 ms, LowerFence = -4.514 ms, UpperFence = 20.805 ms
ConfidenceInterval = [6.398 ms; 8.671 ms] (CI 99.9%), Margin = 1.136 ms (15.08% of Mean)
Skewness = 0.99, Kurtosis = 3.14, MValue = 2.95

// **************************
// Benchmark: WriteText.RunFile: .NET Framework 4.7.2(Runtime=.NET Framework 4.7.2)
// *** Execute ***
// Launch: 1 / 1
// Execute: D:\Microsoft\Workspace\asakura89-project\release\AskScratchpad\CSScratchpad\CSScratchpad\bin\Release\337d5ef9-6727-4016-a9ba-d698494aa95b.exe --benchmarkName "CSScratchpad.Script.WriteText.RunFile" --job ".NET Framework 4.7.2" --benchmarkId 2 in 
// BeforeAnythingElse

// Benchmark Process Environment Information:
// Runtime=.NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
// GC=Concurrent Workstation
// Job: .NET Framework 4.7.2

OverheadJitting  1: 1 op, 431100.00 ns, 431.1000 us/op
WorkloadJitting  1: 1 op, 36159100.00 ns, 36.1591 ms/op

WorkloadPilot    1: 2 op, 29921500.00 ns, 14.9608 ms/op
WorkloadPilot    2: 3 op, 32646900.00 ns, 10.8823 ms/op
WorkloadPilot    3: 4 op, 46107000.00 ns, 11.5268 ms/op
WorkloadPilot    4: 5 op, 74239000.00 ns, 14.8478 ms/op
WorkloadPilot    5: 6 op, 75049600.00 ns, 12.5083 ms/op
WorkloadPilot    6: 7 op, 90900100.00 ns, 12.9857 ms/op
WorkloadPilot    7: 8 op, 95384600.00 ns, 11.9231 ms/op
WorkloadPilot    8: 9 op, 102370300.00 ns, 11.3745 ms/op
WorkloadPilot    9: 10 op, 109675800.00 ns, 10.9676 ms/op
WorkloadPilot   10: 11 op, 135838800.00 ns, 12.3490 ms/op
WorkloadPilot   11: 12 op, 127775600.00 ns, 10.6480 ms/op
WorkloadPilot   12: 13 op, 156477200.00 ns, 12.0367 ms/op
WorkloadPilot   13: 14 op, 157996800.00 ns, 11.2855 ms/op
WorkloadPilot   14: 15 op, 152670300.00 ns, 10.1780 ms/op
WorkloadPilot   15: 16 op, 154373700.00 ns, 9.6484 ms/op
WorkloadPilot   16: 32 op, 318117700.00 ns, 9.9412 ms/op
WorkloadPilot   17: 64 op, 321439300.00 ns, 5.0225 ms/op
WorkloadPilot   18: 128 op, 686654300.00 ns, 5.3645 ms/op

WorkloadWarmup   1: 128 op, 603544700.00 ns, 4.7152 ms/op
WorkloadWarmup   2: 128 op, 624327200.00 ns, 4.8776 ms/op
WorkloadWarmup   3: 128 op, 619876900.00 ns, 4.8428 ms/op
WorkloadWarmup   4: 128 op, 698986500.00 ns, 5.4608 ms/op
WorkloadWarmup   5: 128 op, 1436233500.00 ns, 11.2206 ms/op
WorkloadWarmup   6: 128 op, 1480403000.00 ns, 11.5656 ms/op
WorkloadWarmup   7: 128 op, 1001163800.00 ns, 7.8216 ms/op

// BeforeActualRun
WorkloadActual   1: 128 op, 615933700.00 ns, 4.8120 ms/op
WorkloadActual   2: 128 op, 610114500.00 ns, 4.7665 ms/op
WorkloadActual   3: 128 op, 614583300.00 ns, 4.8014 ms/op
WorkloadActual   4: 128 op, 619282700.00 ns, 4.8381 ms/op
WorkloadActual   5: 128 op, 718835200.00 ns, 5.6159 ms/op
WorkloadActual   6: 128 op, 1467048300.00 ns, 11.4613 ms/op
WorkloadActual   7: 128 op, 1377888300.00 ns, 10.7648 ms/op
WorkloadActual   8: 128 op, 1425247900.00 ns, 11.1347 ms/op
WorkloadActual   9: 128 op, 835072000.00 ns, 6.5240 ms/op
WorkloadActual  10: 128 op, 607595900.00 ns, 4.7468 ms/op
WorkloadActual  11: 128 op, 745506700.00 ns, 5.8243 ms/op
WorkloadActual  12: 128 op, 611676400.00 ns, 4.7787 ms/op
WorkloadActual  13: 128 op, 619779300.00 ns, 4.8420 ms/op
WorkloadActual  14: 128 op, 841635200.00 ns, 6.5753 ms/op
WorkloadActual  15: 128 op, 1411770900.00 ns, 11.0295 ms/op
WorkloadActual  16: 128 op, 1442272100.00 ns, 11.2678 ms/op
WorkloadActual  17: 128 op, 1487435500.00 ns, 11.6206 ms/op
WorkloadActual  18: 128 op, 646229800.00 ns, 5.0487 ms/op
WorkloadActual  19: 128 op, 628029300.00 ns, 4.9065 ms/op
WorkloadActual  20: 128 op, 628146100.00 ns, 4.9074 ms/op
WorkloadActual  21: 128 op, 622510500.00 ns, 4.8634 ms/op
WorkloadActual  22: 128 op, 655367000.00 ns, 5.1201 ms/op
WorkloadActual  23: 128 op, 1169431700.00 ns, 9.1362 ms/op
WorkloadActual  24: 128 op, 1493481200.00 ns, 11.6678 ms/op
WorkloadActual  25: 128 op, 1514443700.00 ns, 11.8316 ms/op
WorkloadActual  26: 128 op, 1111658900.00 ns, 8.6848 ms/op
WorkloadActual  27: 128 op, 640583100.00 ns, 5.0046 ms/op
WorkloadActual  28: 128 op, 592768500.00 ns, 4.6310 ms/op
WorkloadActual  29: 128 op, 653400300.00 ns, 5.1047 ms/op
WorkloadActual  30: 128 op, 606006000.00 ns, 4.7344 ms/op
WorkloadActual  31: 128 op, 620954500.00 ns, 4.8512 ms/op
WorkloadActual  32: 128 op, 1183434000.00 ns, 9.2456 ms/op
WorkloadActual  33: 128 op, 1493570000.00 ns, 11.6685 ms/op
WorkloadActual  34: 128 op, 1556378900.00 ns, 12.1592 ms/op
WorkloadActual  35: 128 op, 1116483400.00 ns, 8.7225 ms/op
WorkloadActual  36: 128 op, 595176300.00 ns, 4.6498 ms/op
WorkloadActual  37: 128 op, 602527100.00 ns, 4.7072 ms/op
WorkloadActual  38: 128 op, 616444600.00 ns, 4.8160 ms/op
WorkloadActual  39: 128 op, 615417300.00 ns, 4.8079 ms/op
WorkloadActual  40: 128 op, 699618000.00 ns, 5.4658 ms/op
WorkloadActual  41: 128 op, 1877236000.00 ns, 14.6659 ms/op
WorkloadActual  42: 128 op, 2114205900.00 ns, 16.5172 ms/op
WorkloadActual  43: 128 op, 1574762400.00 ns, 12.3028 ms/op
WorkloadActual  44: 128 op, 798129500.00 ns, 6.2354 ms/op
WorkloadActual  45: 128 op, 623163400.00 ns, 4.8685 ms/op
WorkloadActual  46: 128 op, 706850400.00 ns, 5.5223 ms/op
WorkloadActual  47: 128 op, 651622900.00 ns, 5.0908 ms/op
WorkloadActual  48: 128 op, 724582100.00 ns, 5.6608 ms/op
WorkloadActual  49: 128 op, 1076629200.00 ns, 8.4112 ms/op
WorkloadActual  50: 128 op, 2561659100.00 ns, 20.0130 ms/op
WorkloadActual  51: 128 op, 2321639000.00 ns, 18.1378 ms/op
WorkloadActual  52: 128 op, 812089900.00 ns, 6.3445 ms/op
WorkloadActual  53: 128 op, 598749800.00 ns, 4.6777 ms/op
WorkloadActual  54: 128 op, 626697200.00 ns, 4.8961 ms/op
WorkloadActual  55: 128 op, 621156600.00 ns, 4.8528 ms/op
WorkloadActual  56: 128 op, 609623400.00 ns, 4.7627 ms/op
WorkloadActual  57: 128 op, 889879800.00 ns, 6.9522 ms/op
WorkloadActual  58: 128 op, 1479477400.00 ns, 11.5584 ms/op
WorkloadActual  59: 128 op, 1556120800.00 ns, 12.1572 ms/op
WorkloadActual  60: 128 op, 1333202100.00 ns, 10.4156 ms/op
WorkloadActual  61: 128 op, 696457600.00 ns, 5.4411 ms/op
WorkloadActual  62: 128 op, 635124100.00 ns, 4.9619 ms/op
WorkloadActual  63: 128 op, 674115600.00 ns, 5.2665 ms/op
WorkloadActual  64: 128 op, 598409400.00 ns, 4.6751 ms/op
WorkloadActual  65: 128 op, 621793200.00 ns, 4.8578 ms/op
WorkloadActual  66: 128 op, 1099908100.00 ns, 8.5930 ms/op
WorkloadActual  67: 128 op, 1410293400.00 ns, 11.0179 ms/op
WorkloadActual  68: 128 op, 1418495600.00 ns, 11.0820 ms/op
WorkloadActual  69: 128 op, 1298273800.00 ns, 10.1428 ms/op
WorkloadActual  70: 128 op, 604318800.00 ns, 4.7212 ms/op
WorkloadActual  71: 128 op, 599015400.00 ns, 4.6798 ms/op
WorkloadActual  72: 128 op, 660091400.00 ns, 5.1570 ms/op
WorkloadActual  73: 128 op, 687923300.00 ns, 5.3744 ms/op
WorkloadActual  74: 128 op, 672219500.00 ns, 5.2517 ms/op
WorkloadActual  75: 128 op, 1445689900.00 ns, 11.2945 ms/op
WorkloadActual  76: 128 op, 1396315700.00 ns, 10.9087 ms/op
WorkloadActual  77: 128 op, 1498607900.00 ns, 11.7079 ms/op
WorkloadActual  78: 128 op, 869648100.00 ns, 6.7941 ms/op
WorkloadActual  79: 128 op, 617436200.00 ns, 4.8237 ms/op
WorkloadActual  80: 128 op, 711995200.00 ns, 5.5625 ms/op
WorkloadActual  81: 128 op, 703730100.00 ns, 5.4979 ms/op
WorkloadActual  82: 128 op, 692917300.00 ns, 5.4134 ms/op
WorkloadActual  83: 128 op, 892170100.00 ns, 6.9701 ms/op
WorkloadActual  84: 128 op, 1384420200.00 ns, 10.8158 ms/op
WorkloadActual  85: 128 op, 1427019600.00 ns, 11.1486 ms/op
WorkloadActual  86: 128 op, 1399788300.00 ns, 10.9358 ms/op
WorkloadActual  87: 128 op, 602936800.00 ns, 4.7104 ms/op
WorkloadActual  88: 128 op, 601737400.00 ns, 4.7011 ms/op
WorkloadActual  89: 128 op, 625699900.00 ns, 4.8883 ms/op
WorkloadActual  90: 128 op, 614771000.00 ns, 4.8029 ms/op
WorkloadActual  91: 128 op, 606046800.00 ns, 4.7347 ms/op
WorkloadActual  92: 128 op, 944395500.00 ns, 7.3781 ms/op
WorkloadActual  93: 128 op, 1385780300.00 ns, 10.8264 ms/op
WorkloadActual  94: 128 op, 1645779300.00 ns, 12.8577 ms/op
WorkloadActual  95: 128 op, 1366149200.00 ns, 10.6730 ms/op
WorkloadActual  96: 128 op, 1165947600.00 ns, 9.1090 ms/op
WorkloadActual  97: 128 op, 687572700.00 ns, 5.3717 ms/op
WorkloadActual  98: 128 op, 633579600.00 ns, 4.9498 ms/op
WorkloadActual  99: 128 op, 592197600.00 ns, 4.6265 ms/op
WorkloadActual  100: 128 op, 646319300.00 ns, 5.0494 ms/op

// AfterActualRun
WorkloadResult   1: 128 op, 615933700.00 ns, 4.8120 ms/op
WorkloadResult   2: 128 op, 610114500.00 ns, 4.7665 ms/op
WorkloadResult   3: 128 op, 614583300.00 ns, 4.8014 ms/op
WorkloadResult   4: 128 op, 619282700.00 ns, 4.8381 ms/op
WorkloadResult   5: 128 op, 718835200.00 ns, 5.6159 ms/op
WorkloadResult   6: 128 op, 1467048300.00 ns, 11.4613 ms/op
WorkloadResult   7: 128 op, 1377888300.00 ns, 10.7648 ms/op
WorkloadResult   8: 128 op, 1425247900.00 ns, 11.1347 ms/op
WorkloadResult   9: 128 op, 835072000.00 ns, 6.5240 ms/op
WorkloadResult  10: 128 op, 607595900.00 ns, 4.7468 ms/op
WorkloadResult  11: 128 op, 745506700.00 ns, 5.8243 ms/op
WorkloadResult  12: 128 op, 611676400.00 ns, 4.7787 ms/op
WorkloadResult  13: 128 op, 619779300.00 ns, 4.8420 ms/op
WorkloadResult  14: 128 op, 841635200.00 ns, 6.5753 ms/op
WorkloadResult  15: 128 op, 1411770900.00 ns, 11.0295 ms/op
WorkloadResult  16: 128 op, 1442272100.00 ns, 11.2678 ms/op
WorkloadResult  17: 128 op, 1487435500.00 ns, 11.6206 ms/op
WorkloadResult  18: 128 op, 646229800.00 ns, 5.0487 ms/op
WorkloadResult  19: 128 op, 628029300.00 ns, 4.9065 ms/op
WorkloadResult  20: 128 op, 628146100.00 ns, 4.9074 ms/op
WorkloadResult  21: 128 op, 622510500.00 ns, 4.8634 ms/op
WorkloadResult  22: 128 op, 655367000.00 ns, 5.1201 ms/op
WorkloadResult  23: 128 op, 1169431700.00 ns, 9.1362 ms/op
WorkloadResult  24: 128 op, 1493481200.00 ns, 11.6678 ms/op
WorkloadResult  25: 128 op, 1514443700.00 ns, 11.8316 ms/op
WorkloadResult  26: 128 op, 1111658900.00 ns, 8.6848 ms/op
WorkloadResult  27: 128 op, 640583100.00 ns, 5.0046 ms/op
WorkloadResult  28: 128 op, 592768500.00 ns, 4.6310 ms/op
WorkloadResult  29: 128 op, 653400300.00 ns, 5.1047 ms/op
WorkloadResult  30: 128 op, 606006000.00 ns, 4.7344 ms/op
WorkloadResult  31: 128 op, 620954500.00 ns, 4.8512 ms/op
WorkloadResult  32: 128 op, 1183434000.00 ns, 9.2456 ms/op
WorkloadResult  33: 128 op, 1493570000.00 ns, 11.6685 ms/op
WorkloadResult  34: 128 op, 1556378900.00 ns, 12.1592 ms/op
WorkloadResult  35: 128 op, 1116483400.00 ns, 8.7225 ms/op
WorkloadResult  36: 128 op, 595176300.00 ns, 4.6498 ms/op
WorkloadResult  37: 128 op, 602527100.00 ns, 4.7072 ms/op
WorkloadResult  38: 128 op, 616444600.00 ns, 4.8160 ms/op
WorkloadResult  39: 128 op, 615417300.00 ns, 4.8079 ms/op
WorkloadResult  40: 128 op, 699618000.00 ns, 5.4658 ms/op
WorkloadResult  41: 128 op, 1877236000.00 ns, 14.6659 ms/op
WorkloadResult  42: 128 op, 2114205900.00 ns, 16.5172 ms/op
WorkloadResult  43: 128 op, 1574762400.00 ns, 12.3028 ms/op
WorkloadResult  44: 128 op, 798129500.00 ns, 6.2354 ms/op
WorkloadResult  45: 128 op, 623163400.00 ns, 4.8685 ms/op
WorkloadResult  46: 128 op, 706850400.00 ns, 5.5223 ms/op
WorkloadResult  47: 128 op, 651622900.00 ns, 5.0908 ms/op
WorkloadResult  48: 128 op, 724582100.00 ns, 5.6608 ms/op
WorkloadResult  49: 128 op, 1076629200.00 ns, 8.4112 ms/op
WorkloadResult  50: 128 op, 2321639000.00 ns, 18.1378 ms/op
WorkloadResult  51: 128 op, 812089900.00 ns, 6.3445 ms/op
WorkloadResult  52: 128 op, 598749800.00 ns, 4.6777 ms/op
WorkloadResult  53: 128 op, 626697200.00 ns, 4.8961 ms/op
WorkloadResult  54: 128 op, 621156600.00 ns, 4.8528 ms/op
WorkloadResult  55: 128 op, 609623400.00 ns, 4.7627 ms/op
WorkloadResult  56: 128 op, 889879800.00 ns, 6.9522 ms/op
WorkloadResult  57: 128 op, 1479477400.00 ns, 11.5584 ms/op
WorkloadResult  58: 128 op, 1556120800.00 ns, 12.1572 ms/op
WorkloadResult  59: 128 op, 1333202100.00 ns, 10.4156 ms/op
WorkloadResult  60: 128 op, 696457600.00 ns, 5.4411 ms/op
WorkloadResult  61: 128 op, 635124100.00 ns, 4.9619 ms/op
WorkloadResult  62: 128 op, 674115600.00 ns, 5.2665 ms/op
WorkloadResult  63: 128 op, 598409400.00 ns, 4.6751 ms/op
WorkloadResult  64: 128 op, 621793200.00 ns, 4.8578 ms/op
WorkloadResult  65: 128 op, 1099908100.00 ns, 8.5930 ms/op
WorkloadResult  66: 128 op, 1410293400.00 ns, 11.0179 ms/op
WorkloadResult  67: 128 op, 1418495600.00 ns, 11.0820 ms/op
WorkloadResult  68: 128 op, 1298273800.00 ns, 10.1428 ms/op
WorkloadResult  69: 128 op, 604318800.00 ns, 4.7212 ms/op
WorkloadResult  70: 128 op, 599015400.00 ns, 4.6798 ms/op
WorkloadResult  71: 128 op, 660091400.00 ns, 5.1570 ms/op
WorkloadResult  72: 128 op, 687923300.00 ns, 5.3744 ms/op
WorkloadResult  73: 128 op, 672219500.00 ns, 5.2517 ms/op
WorkloadResult  74: 128 op, 1445689900.00 ns, 11.2945 ms/op
WorkloadResult  75: 128 op, 1396315700.00 ns, 10.9087 ms/op
WorkloadResult  76: 128 op, 1498607900.00 ns, 11.7079 ms/op
WorkloadResult  77: 128 op, 869648100.00 ns, 6.7941 ms/op
WorkloadResult  78: 128 op, 617436200.00 ns, 4.8237 ms/op
WorkloadResult  79: 128 op, 711995200.00 ns, 5.5625 ms/op
WorkloadResult  80: 128 op, 703730100.00 ns, 5.4979 ms/op
WorkloadResult  81: 128 op, 692917300.00 ns, 5.4134 ms/op
WorkloadResult  82: 128 op, 892170100.00 ns, 6.9701 ms/op
WorkloadResult  83: 128 op, 1384420200.00 ns, 10.8158 ms/op
WorkloadResult  84: 128 op, 1427019600.00 ns, 11.1486 ms/op
WorkloadResult  85: 128 op, 1399788300.00 ns, 10.9358 ms/op
WorkloadResult  86: 128 op, 602936800.00 ns, 4.7104 ms/op
WorkloadResult  87: 128 op, 601737400.00 ns, 4.7011 ms/op
WorkloadResult  88: 128 op, 625699900.00 ns, 4.8883 ms/op
WorkloadResult  89: 128 op, 614771000.00 ns, 4.8029 ms/op
WorkloadResult  90: 128 op, 606046800.00 ns, 4.7347 ms/op
WorkloadResult  91: 128 op, 944395500.00 ns, 7.3781 ms/op
WorkloadResult  92: 128 op, 1385780300.00 ns, 10.8264 ms/op
WorkloadResult  93: 128 op, 1645779300.00 ns, 12.8577 ms/op
WorkloadResult  94: 128 op, 1366149200.00 ns, 10.6730 ms/op
WorkloadResult  95: 128 op, 1165947600.00 ns, 9.1090 ms/op
WorkloadResult  96: 128 op, 687572700.00 ns, 5.3717 ms/op
WorkloadResult  97: 128 op, 633579600.00 ns, 4.9498 ms/op
WorkloadResult  98: 128 op, 592197600.00 ns, 4.6265 ms/op
WorkloadResult  99: 128 op, 646319300.00 ns, 5.0494 ms/op

// AfterAll
// Benchmark Process 15668 has exited with code 0.

Mean = 7.414 ms, StdErr = 0.322 ms (4.34%), N = 99, StdDev = 3.203 ms
Min = 4.627 ms, Q1 = 4.852 ms, Median = 5.498 ms, Q3 = 10.790 ms, Max = 18.138 ms
IQR = 5.938 ms, LowerFence = -4.055 ms, UpperFence = 19.698 ms
ConfidenceInterval = [6.321 ms; 8.506 ms] (CI 99.9%), Margin = 1.092 ms (14.73% of Mean)
Skewness = 1, Kurtosis = 3.07, MValue = 2.7

// ***** BenchmarkRunner: Finish  *****

// * Export *
  BenchmarkDotNet.Artifacts\results\CSScratchpad.Script.WriteText-report.csv
  BenchmarkDotNet.Artifacts\results\CSScratchpad.Script.WriteText-report-github.md
  BenchmarkDotNet.Artifacts\results\CSScratchpad.Script.WriteText-report.html

// * Detailed results *
WriteText.RunTextWriter: .NET Framework 4.6.1(Runtime=.NET Framework 4.6.1)
Runtime = .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT; GC = Concurrent Workstation
Mean = 6.774 ms, StdErr = 0.267 ms (3.94%), N = 99, StdDev = 2.653 ms
Min = 4.539 ms, Q1 = 4.762 ms, Median = 5.170 ms, Q3 = 9.344 ms, Max = 13.453 ms
IQR = 4.583 ms, LowerFence = -2.112 ms, UpperFence = 16.218 ms
ConfidenceInterval = [5.869 ms; 7.679 ms] (CI 99.9%), Margin = 0.905 ms (13.36% of Mean)
Skewness = 0.87, Kurtosis = 2.11, MValue = 2.69
-------------------- Histogram --------------------
[ 4.479 ms ;  5.984 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[ 5.984 ms ;  6.758 ms) | 
[ 6.758 ms ;  8.292 ms) | @@@@
[ 8.292 ms ; 10.057 ms) | @@@@@@@@@@
[10.057 ms ; 11.562 ms) | @@@@@@@@@@@@@@@@
[11.562 ms ; 12.718 ms) | @@@@
[12.718 ms ; 14.206 ms) | @
---------------------------------------------------

WriteText.RunTextWriter: .NET Framework 4.7.2(Runtime=.NET Framework 4.7.2)
Runtime = .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT; GC = Concurrent Workstation
Mean = 7.983 ms, StdErr = 0.305 ms (3.82%), N = 100, StdDev = 3.046 ms
Min = 4.684 ms, Q1 = 5.183 ms, Median = 6.744 ms, Q3 = 11.140 ms, Max = 14.468 ms
IQR = 5.957 ms, LowerFence = -3.753 ms, UpperFence = 20.076 ms
ConfidenceInterval = [6.950 ms; 9.016 ms] (CI 99.9%), Margin = 1.033 ms (12.94% of Mean)
Skewness = 0.47, Kurtosis = 1.61, MValue = 3.06
-------------------- Histogram --------------------
[ 4.574 ms ;  6.296 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[ 6.296 ms ;  8.073 ms) | @@@@@@@@@@@@@@
[ 8.073 ms ;  8.911 ms) | 
[ 8.911 ms ; 10.534 ms) | @@@@@
[10.534 ms ; 12.257 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@
[12.257 ms ; 14.223 ms) | @@@@@@@@
[14.223 ms ; 15.329 ms) | @
---------------------------------------------------

WriteText.RunFileStream: .NET Framework 4.6.1(Runtime=.NET Framework 4.6.1)
Runtime = .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT; GC = Concurrent Workstation
Mean = 7.520 ms, StdErr = 0.312 ms (4.15%), N = 100, StdDev = 3.121 ms
Min = 4.481 ms, Q1 = 5.135 ms, Median = 5.790 ms, Q3 = 10.741 ms, Max = 14.900 ms
IQR = 5.606 ms, LowerFence = -3.274 ms, UpperFence = 19.149 ms
ConfidenceInterval = [6.462 ms; 8.578 ms] (CI 99.9%), Margin = 1.058 ms (14.07% of Mean)
Skewness = 0.81, Kurtosis = 2.03, MValue = 2.81
-------------------- Histogram --------------------
[ 4.463 ms ;  6.228 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[ 6.228 ms ;  7.949 ms) | @@@@@@
[ 7.949 ms ;  8.992 ms) | @
[ 8.992 ms ; 10.397 ms) | @@@
[10.397 ms ; 12.162 ms) | @@@@@@@@@@@@@@@@@@@@@
[12.162 ms ; 13.918 ms) | @@@@@@
[13.918 ms ; 15.782 ms) | @@
---------------------------------------------------

WriteText.RunFileStream: .NET Framework 4.7.2(Runtime=.NET Framework 4.7.2)
Runtime = .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT; GC = Concurrent Workstation
Mean = 7.534 ms, StdErr = 0.335 ms (4.45%), N = 99, StdDev = 3.333 ms
Min = 4.624 ms, Q1 = 4.981 ms, Median = 5.494 ms, Q3 = 11.311 ms, Max = 19.991 ms
IQR = 6.330 ms, LowerFence = -4.514 ms, UpperFence = 20.805 ms
ConfidenceInterval = [6.398 ms; 8.671 ms] (CI 99.9%), Margin = 1.136 ms (15.08% of Mean)
Skewness = 0.99, Kurtosis = 3.14, MValue = 2.95
-------------------- Histogram --------------------
[ 4.560 ms ;  6.451 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[ 6.451 ms ;  8.158 ms) | @@@@
[ 8.158 ms ; 10.370 ms) | @@@
[10.370 ms ; 12.261 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@
[12.261 ms ; 14.275 ms) | @@@@@
[14.275 ms ; 16.166 ms) | 
[16.166 ms ; 18.057 ms) | 
[18.057 ms ; 19.045 ms) | 
[19.045 ms ; 20.936 ms) | @
---------------------------------------------------

WriteText.RunFile: .NET Framework 4.6.1(Runtime=.NET Framework 4.6.1)
Runtime = .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT; GC = Concurrent Workstation
Mean = 7.405 ms, StdErr = 0.289 ms (3.90%), N = 100, StdDev = 2.885 ms
Min = 4.644 ms, Q1 = 5.001 ms, Median = 5.522 ms, Q3 = 10.937 ms, Max = 12.571 ms
IQR = 5.936 ms, LowerFence = -3.903 ms, UpperFence = 19.842 ms
ConfidenceInterval = [6.427 ms; 8.384 ms] (CI 99.9%), Margin = 0.979 ms (13.21% of Mean)
Skewness = 0.56, Kurtosis = 1.52, MValue = 2.98
-------------------- Histogram --------------------
[ 4.556 ms ;  6.188 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[ 6.188 ms ;  7.832 ms) | @@@@@@@@
[ 7.832 ms ;  9.180 ms) | @@@
[ 9.180 ms ; 10.708 ms) | @@@@@@
[10.708 ms ; 12.340 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@
[12.340 ms ; 13.387 ms) | @@
---------------------------------------------------

WriteText.RunFile: .NET Framework 4.7.2(Runtime=.NET Framework 4.7.2)
Runtime = .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT; GC = Concurrent Workstation
Mean = 7.414 ms, StdErr = 0.322 ms (4.34%), N = 99, StdDev = 3.203 ms
Min = 4.627 ms, Q1 = 4.852 ms, Median = 5.498 ms, Q3 = 10.790 ms, Max = 18.138 ms
IQR = 5.938 ms, LowerFence = -4.055 ms, UpperFence = 19.698 ms
ConfidenceInterval = [6.321 ms; 8.506 ms] (CI 99.9%), Margin = 1.092 ms (14.73% of Mean)
Skewness = 1, Kurtosis = 3.07, MValue = 2.7
-------------------- Histogram --------------------
[ 4.577 ms ;  6.394 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[ 6.394 ms ;  8.368 ms) | @@@@@@
[ 8.368 ms ; 10.186 ms) | @@@@@@@@
[10.186 ms ; 12.397 ms) | @@@@@@@@@@@@@@@@@@@@@@@@
[12.397 ms ; 14.671 ms) | @@
[14.671 ms ; 16.419 ms) | 
[16.419 ms ; 18.236 ms) | @@
---------------------------------------------------

// * Summary *

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1165 (21H1/May2021Update)
Intel Core i7-7500U CPU 2.70GHz (Kaby Lake), 1 CPU, 4 logical and 2 physical cores
  [Host]               : .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT  [AttachedDebugger]
  .NET Framework 4.6.1 : .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
  .NET Framework 4.7.2 : .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT


|        Method |                  Job |              Runtime |     Mean |     Error |   StdDev |   Median | Ratio | RatioSD |
|-------------- |--------------------- |--------------------- |---------:|----------:|---------:|---------:|------:|--------:|
| RunTextWriter | .NET Framework 4.6.1 | .NET Framework 4.6.1 | 6.774 ms | 0.9047 ms | 2.653 ms | 5.170 ms |  1.00 |    0.00 |
| RunTextWriter | .NET Framework 4.7.2 | .NET Framework 4.7.2 | 7.983 ms | 1.0330 ms | 3.046 ms | 6.744 ms |  1.36 |    0.74 |
|               |                      |                      |          |           |          |          |       |         |
| RunFileStream | .NET Framework 4.6.1 | .NET Framework 4.6.1 | 7.520 ms | 1.0584 ms | 3.121 ms | 5.790 ms |  1.00 |    0.00 |
| RunFileStream | .NET Framework 4.7.2 | .NET Framework 4.7.2 | 7.534 ms | 1.1364 ms | 3.333 ms | 5.494 ms |  1.12 |    0.59 |
|               |                      |                      |          |           |          |          |       |         |
|       RunFile | .NET Framework 4.6.1 | .NET Framework 4.6.1 | 7.405 ms | 0.9785 ms | 2.885 ms | 5.522 ms |  1.00 |    0.00 |
|       RunFile | .NET Framework 4.7.2 | .NET Framework 4.7.2 | 7.414 ms | 1.0923 ms | 3.203 ms | 5.498 ms |  1.24 |    0.81 |

// * Warnings *
Environment
  Summary -> Benchmark was executed with attached debugger
MultimodalDistribution
  WriteText.RunTextWriter: .NET Framework 4.7.2 -> It seems that the distribution can have several modes (mValue = 3.06)
  WriteText.RunFileStream: .NET Framework 4.6.1 -> It seems that the distribution can have several modes (mValue = 2.81)
  WriteText.RunFileStream: .NET Framework 4.7.2 -> It seems that the distribution can have several modes (mValue = 2.95)
  WriteText.RunFile: .NET Framework 4.6.1       -> It seems that the distribution can have several modes (mValue = 2.98)

// * Hints *
Outliers
  WriteText.RunTextWriter: .NET Framework 4.6.1 -> 1 outlier  was  removed (17.50 ms)
  WriteText.RunFileStream: .NET Framework 4.7.2 -> 1 outlier  was  removed (498.78 ms)
  WriteText.RunFile: .NET Framework 4.7.2       -> 1 outlier  was  removed (20.01 ms)

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
Run time: 00:09:30 (570.64 sec), executed benchmarks: 6

Global total time: 00:09:35 (575.52 sec), executed benchmarks: 6
// * Artifacts cleanup *

*/
#endregion
