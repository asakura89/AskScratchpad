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
// ***** Found 4 benchmark(s) in total *****
// ***** Building 2 exe(s) in Parallel: Start   *****
// ***** Done, took 00:00:03 (3.59 sec)   *****
// Found 4 benchmarks:
//   WriteText.RunTextWriter: .NET Framework 4.6.1(Runtime=.NET Framework 4.6.1)
//   WriteText.RunFile: .NET Framework 4.6.1(Runtime=.NET Framework 4.6.1)
//   WriteText.RunTextWriter: .NET Framework 4.7.2(Runtime=.NET Framework 4.7.2)
//   WriteText.RunFile: .NET Framework 4.7.2(Runtime=.NET Framework 4.7.2)

// **************************
// Benchmark: WriteText.RunTextWriter: .NET Framework 4.6.1(Runtime=.NET Framework 4.6.1)
// *** Execute ***
// Launch: 1 / 1
// Execute: D:\Microsoft\Workspace\asakura89-project\release\AskScratchpad\CSScratchpad\CSScratchpad\bin\Release\1f7e4b00-9986-4126-998c-f5c92e73d41f.exe --benchmarkName "CSScratchpad.Script.WriteText.RunTextWriter" --job ".NET Framework 4.6.1" --benchmarkId 0 in 
// BeforeAnythingElse

// Benchmark Process Environment Information:
// Runtime=.NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
// GC=Concurrent Workstation
// Job: .NET Framework 4.6.1

OverheadJitting  1: 1 op, 321200.00 ns, 321.2000 us/op
WorkloadJitting  1: 1 op, 4706800.00 ns, 4.7068 ms/op

OverheadJitting  2: 16 op, 228500.00 ns, 14.2813 us/op
WorkloadJitting  2: 16 op, 58478500.00 ns, 3.6549 ms/op

WorkloadPilot    1: 16 op, 66984800.00 ns, 4.1866 ms/op
WorkloadPilot    2: 32 op, 122498800.00 ns, 3.8281 ms/op
WorkloadPilot    3: 64 op, 222077500.00 ns, 3.4700 ms/op
WorkloadPilot    4: 128 op, 413654900.00 ns, 3.2317 ms/op
WorkloadPilot    5: 256 op, 822456100.00 ns, 3.2127 ms/op

OverheadWarmup   1: 256 op, 7000.00 ns, 27.3438 ns/op
OverheadWarmup   2: 256 op, 1300.00 ns, 5.0781 ns/op
OverheadWarmup   3: 256 op, 3800.00 ns, 14.8438 ns/op
OverheadWarmup   4: 256 op, 1200.00 ns, 4.6875 ns/op
OverheadWarmup   5: 256 op, 1300.00 ns, 5.0781 ns/op
OverheadWarmup   6: 256 op, 3200.00 ns, 12.5000 ns/op
OverheadWarmup   7: 256 op, 3600.00 ns, 14.0625 ns/op
OverheadWarmup   8: 256 op, 3600.00 ns, 14.0625 ns/op

OverheadActual   1: 256 op, 2700.00 ns, 10.5469 ns/op
OverheadActual   2: 256 op, 1600.00 ns, 6.2500 ns/op
OverheadActual   3: 256 op, 1500.00 ns, 5.8594 ns/op
OverheadActual   4: 256 op, 1200.00 ns, 4.6875 ns/op
OverheadActual   5: 256 op, 1200.00 ns, 4.6875 ns/op
OverheadActual   6: 256 op, 1200.00 ns, 4.6875 ns/op
OverheadActual   7: 256 op, 1100.00 ns, 4.2969 ns/op
OverheadActual   8: 256 op, 1100.00 ns, 4.2969 ns/op
OverheadActual   9: 256 op, 1100.00 ns, 4.2969 ns/op
OverheadActual  10: 256 op, 1100.00 ns, 4.2969 ns/op
OverheadActual  11: 256 op, 1300.00 ns, 5.0781 ns/op
OverheadActual  12: 256 op, 1400.00 ns, 5.4688 ns/op
OverheadActual  13: 256 op, 1300.00 ns, 5.0781 ns/op
OverheadActual  14: 256 op, 1400.00 ns, 5.4688 ns/op
OverheadActual  15: 256 op, 1500.00 ns, 5.8594 ns/op
OverheadActual  16: 256 op, 1300.00 ns, 5.0781 ns/op
OverheadActual  17: 256 op, 2400.00 ns, 9.3750 ns/op
OverheadActual  18: 256 op, 1500.00 ns, 5.8594 ns/op
OverheadActual  19: 256 op, 1800.00 ns, 7.0313 ns/op
OverheadActual  20: 256 op, 1300.00 ns, 5.0781 ns/op

WorkloadWarmup   1: 256 op, 836441500.00 ns, 3.2673 ms/op
WorkloadWarmup   2: 256 op, 863217700.00 ns, 3.3719 ms/op
WorkloadWarmup   3: 256 op, 790876300.00 ns, 3.0894 ms/op
WorkloadWarmup   4: 256 op, 799528500.00 ns, 3.1232 ms/op
WorkloadWarmup   5: 256 op, 804349400.00 ns, 3.1420 ms/op
WorkloadWarmup   6: 256 op, 805399100.00 ns, 3.1461 ms/op
WorkloadWarmup   7: 256 op, 1128129200.00 ns, 4.4068 ms/op
WorkloadWarmup   8: 256 op, 1797998200.00 ns, 7.0234 ms/op
WorkloadWarmup   9: 256 op, 1422071100.00 ns, 5.5550 ms/op

// BeforeActualRun
WorkloadActual   1: 256 op, 814609700.00 ns, 3.1821 ms/op
WorkloadActual   2: 256 op, 849891300.00 ns, 3.3199 ms/op
WorkloadActual   3: 256 op, 838564600.00 ns, 3.2756 ms/op
WorkloadActual   4: 256 op, 996771000.00 ns, 3.8936 ms/op
WorkloadActual   5: 256 op, 1735481900.00 ns, 6.7792 ms/op
WorkloadActual   6: 256 op, 1661407800.00 ns, 6.4899 ms/op
WorkloadActual   7: 256 op, 828347800.00 ns, 3.2357 ms/op
WorkloadActual   8: 256 op, 808584800.00 ns, 3.1585 ms/op
WorkloadActual   9: 256 op, 846695400.00 ns, 3.3074 ms/op
WorkloadActual  10: 256 op, 807041000.00 ns, 3.1525 ms/op
WorkloadActual  11: 256 op, 787464900.00 ns, 3.0760 ms/op
WorkloadActual  12: 256 op, 1201519300.00 ns, 4.6934 ms/op
WorkloadActual  13: 256 op, 1716168500.00 ns, 6.7038 ms/op
WorkloadActual  14: 256 op, 1433669200.00 ns, 5.6003 ms/op
WorkloadActual  15: 256 op, 834384500.00 ns, 3.2593 ms/op
WorkloadActual  16: 256 op, 779775600.00 ns, 3.0460 ms/op
WorkloadActual  17: 256 op, 829932700.00 ns, 3.2419 ms/op
WorkloadActual  18: 256 op, 927608600.00 ns, 3.6235 ms/op
WorkloadActual  19: 256 op, 1677868700.00 ns, 6.5542 ms/op
WorkloadActual  20: 256 op, 1696514200.00 ns, 6.6270 ms/op
WorkloadActual  21: 256 op, 920741800.00 ns, 3.5966 ms/op
WorkloadActual  22: 256 op, 845440100.00 ns, 3.3025 ms/op
WorkloadActual  23: 256 op, 857580900.00 ns, 3.3499 ms/op
WorkloadActual  24: 256 op, 806504100.00 ns, 3.1504 ms/op
WorkloadActual  25: 256 op, 1604452000.00 ns, 6.2674 ms/op
WorkloadActual  26: 256 op, 1689395800.00 ns, 6.5992 ms/op
WorkloadActual  27: 256 op, 1050068200.00 ns, 4.1018 ms/op
WorkloadActual  28: 256 op, 849775400.00 ns, 3.3194 ms/op
WorkloadActual  29: 256 op, 820375100.00 ns, 3.2046 ms/op
WorkloadActual  30: 256 op, 801511100.00 ns, 3.1309 ms/op
WorkloadActual  31: 256 op, 1265762200.00 ns, 4.9444 ms/op
WorkloadActual  32: 256 op, 1683085300.00 ns, 6.5746 ms/op
WorkloadActual  33: 256 op, 1451457700.00 ns, 5.6698 ms/op
WorkloadActual  34: 256 op, 812588800.00 ns, 3.1742 ms/op
WorkloadActual  35: 256 op, 838700100.00 ns, 3.2762 ms/op
WorkloadActual  36: 256 op, 825086000.00 ns, 3.2230 ms/op
WorkloadActual  37: 256 op, 1072351800.00 ns, 4.1889 ms/op
WorkloadActual  38: 256 op, 1701253600.00 ns, 6.6455 ms/op
WorkloadActual  39: 256 op, 1577053600.00 ns, 6.1604 ms/op
WorkloadActual  40: 256 op, 804161800.00 ns, 3.1413 ms/op
WorkloadActual  41: 256 op, 806590400.00 ns, 3.1507 ms/op
WorkloadActual  42: 256 op, 800199300.00 ns, 3.1258 ms/op
WorkloadActual  43: 256 op, 927683300.00 ns, 3.6238 ms/op
WorkloadActual  44: 256 op, 1707917000.00 ns, 6.6716 ms/op
WorkloadActual  45: 256 op, 1726008000.00 ns, 6.7422 ms/op
WorkloadActual  46: 256 op, 902592000.00 ns, 3.5258 ms/op
WorkloadActual  47: 256 op, 805431900.00 ns, 3.1462 ms/op
WorkloadActual  48: 256 op, 823751000.00 ns, 3.2178 ms/op
WorkloadActual  49: 256 op, 801247800.00 ns, 3.1299 ms/op
WorkloadActual  50: 256 op, 1387592200.00 ns, 5.4203 ms/op
WorkloadActual  51: 256 op, 1683136700.00 ns, 6.5748 ms/op
WorkloadActual  52: 256 op, 1417888300.00 ns, 5.5386 ms/op
WorkloadActual  53: 256 op, 785183500.00 ns, 3.0671 ms/op
WorkloadActual  54: 256 op, 783012000.00 ns, 3.0586 ms/op
WorkloadActual  55: 256 op, 848311900.00 ns, 3.3137 ms/op
WorkloadActual  56: 256 op, 1196539000.00 ns, 4.6740 ms/op
WorkloadActual  57: 256 op, 1734876600.00 ns, 6.7769 ms/op
WorkloadActual  58: 256 op, 1397628500.00 ns, 5.4595 ms/op
WorkloadActual  59: 256 op, 846991100.00 ns, 3.3086 ms/op
WorkloadActual  60: 256 op, 866269900.00 ns, 3.3839 ms/op
WorkloadActual  61: 256 op, 814351600.00 ns, 3.1811 ms/op
WorkloadActual  62: 256 op, 902036600.00 ns, 3.5236 ms/op
WorkloadActual  63: 256 op, 1722850400.00 ns, 6.7299 ms/op
WorkloadActual  64: 256 op, 1705477300.00 ns, 6.6620 ms/op
WorkloadActual  65: 256 op, 823294000.00 ns, 3.2160 ms/op
WorkloadActual  66: 256 op, 810018400.00 ns, 3.1641 ms/op
WorkloadActual  67: 256 op, 805078500.00 ns, 3.1448 ms/op
WorkloadActual  68: 256 op, 814896900.00 ns, 3.1832 ms/op
WorkloadActual  69: 256 op, 1444686400.00 ns, 5.6433 ms/op
WorkloadActual  70: 256 op, 1677550100.00 ns, 6.5529 ms/op
WorkloadActual  71: 256 op, 1160912600.00 ns, 4.5348 ms/op
WorkloadActual  72: 256 op, 851878500.00 ns, 3.3277 ms/op
WorkloadActual  73: 256 op, 803891800.00 ns, 3.1402 ms/op
WorkloadActual  74: 256 op, 790183600.00 ns, 3.0867 ms/op
WorkloadActual  75: 256 op, 1206197000.00 ns, 4.7117 ms/op
WorkloadActual  76: 256 op, 1672403000.00 ns, 6.5328 ms/op
WorkloadActual  77: 256 op, 1584316700.00 ns, 6.1887 ms/op
WorkloadActual  78: 256 op, 809092800.00 ns, 3.1605 ms/op
WorkloadActual  79: 256 op, 810046300.00 ns, 3.1642 ms/op
WorkloadActual  80: 256 op, 899536600.00 ns, 3.5138 ms/op
WorkloadActual  81: 256 op, 1035040100.00 ns, 4.0431 ms/op
WorkloadActual  82: 256 op, 1699847000.00 ns, 6.6400 ms/op
WorkloadActual  83: 256 op, 1630183300.00 ns, 6.3679 ms/op
WorkloadActual  84: 256 op, 809681100.00 ns, 3.1628 ms/op
WorkloadActual  85: 256 op, 823495000.00 ns, 3.2168 ms/op
WorkloadActual  86: 256 op, 828597400.00 ns, 3.2367 ms/op
WorkloadActual  87: 256 op, 795209300.00 ns, 3.1063 ms/op
WorkloadActual  88: 256 op, 1631944400.00 ns, 6.3748 ms/op
WorkloadActual  89: 256 op, 1691864000.00 ns, 6.6088 ms/op
WorkloadActual  90: 256 op, 1071803300.00 ns, 4.1867 ms/op
WorkloadActual  91: 256 op, 811615500.00 ns, 3.1704 ms/op
WorkloadActual  92: 256 op, 817270600.00 ns, 3.1925 ms/op
WorkloadActual  93: 256 op, 903919800.00 ns, 3.5309 ms/op
WorkloadActual  94: 256 op, 1483687800.00 ns, 5.7957 ms/op
WorkloadActual  95: 256 op, 1711501100.00 ns, 6.6856 ms/op
WorkloadActual  96: 256 op, 1155949300.00 ns, 4.5154 ms/op
WorkloadActual  97: 256 op, 825014100.00 ns, 3.2227 ms/op
WorkloadActual  98: 256 op, 788491400.00 ns, 3.0800 ms/op
WorkloadActual  99: 256 op, 800049000.00 ns, 3.1252 ms/op
WorkloadActual  100: 256 op, 1094091000.00 ns, 4.2738 ms/op

// AfterActualRun
WorkloadResult   1: 256 op, 814608400.00 ns, 3.1821 ms/op
WorkloadResult   2: 256 op, 849890000.00 ns, 3.3199 ms/op
WorkloadResult   3: 256 op, 838563300.00 ns, 3.2756 ms/op
WorkloadResult   4: 256 op, 996769700.00 ns, 3.8936 ms/op
WorkloadResult   5: 256 op, 1735480600.00 ns, 6.7792 ms/op
WorkloadResult   6: 256 op, 1661406500.00 ns, 6.4899 ms/op
WorkloadResult   7: 256 op, 828346500.00 ns, 3.2357 ms/op
WorkloadResult   8: 256 op, 808583500.00 ns, 3.1585 ms/op
WorkloadResult   9: 256 op, 846694100.00 ns, 3.3074 ms/op
WorkloadResult  10: 256 op, 807039700.00 ns, 3.1525 ms/op
WorkloadResult  11: 256 op, 787463600.00 ns, 3.0760 ms/op
WorkloadResult  12: 256 op, 1201518000.00 ns, 4.6934 ms/op
WorkloadResult  13: 256 op, 1716167200.00 ns, 6.7038 ms/op
WorkloadResult  14: 256 op, 1433667900.00 ns, 5.6003 ms/op
WorkloadResult  15: 256 op, 834383200.00 ns, 3.2593 ms/op
WorkloadResult  16: 256 op, 779774300.00 ns, 3.0460 ms/op
WorkloadResult  17: 256 op, 829931400.00 ns, 3.2419 ms/op
WorkloadResult  18: 256 op, 927607300.00 ns, 3.6235 ms/op
WorkloadResult  19: 256 op, 1677867400.00 ns, 6.5542 ms/op
WorkloadResult  20: 256 op, 1696512900.00 ns, 6.6270 ms/op
WorkloadResult  21: 256 op, 920740500.00 ns, 3.5966 ms/op
WorkloadResult  22: 256 op, 845438800.00 ns, 3.3025 ms/op
WorkloadResult  23: 256 op, 857579600.00 ns, 3.3499 ms/op
WorkloadResult  24: 256 op, 806502800.00 ns, 3.1504 ms/op
WorkloadResult  25: 256 op, 1604450700.00 ns, 6.2674 ms/op
WorkloadResult  26: 256 op, 1689394500.00 ns, 6.5992 ms/op
WorkloadResult  27: 256 op, 1050066900.00 ns, 4.1018 ms/op
WorkloadResult  28: 256 op, 849774100.00 ns, 3.3194 ms/op
WorkloadResult  29: 256 op, 820373800.00 ns, 3.2046 ms/op
WorkloadResult  30: 256 op, 801509800.00 ns, 3.1309 ms/op
WorkloadResult  31: 256 op, 1265760900.00 ns, 4.9444 ms/op
WorkloadResult  32: 256 op, 1683084000.00 ns, 6.5745 ms/op
WorkloadResult  33: 256 op, 1451456400.00 ns, 5.6698 ms/op
WorkloadResult  34: 256 op, 812587500.00 ns, 3.1742 ms/op
WorkloadResult  35: 256 op, 838698800.00 ns, 3.2762 ms/op
WorkloadResult  36: 256 op, 825084700.00 ns, 3.2230 ms/op
WorkloadResult  37: 256 op, 1072350500.00 ns, 4.1889 ms/op
WorkloadResult  38: 256 op, 1701252300.00 ns, 6.6455 ms/op
WorkloadResult  39: 256 op, 1577052300.00 ns, 6.1604 ms/op
WorkloadResult  40: 256 op, 804160500.00 ns, 3.1413 ms/op
WorkloadResult  41: 256 op, 806589100.00 ns, 3.1507 ms/op
WorkloadResult  42: 256 op, 800198000.00 ns, 3.1258 ms/op
WorkloadResult  43: 256 op, 927682000.00 ns, 3.6238 ms/op
WorkloadResult  44: 256 op, 1707915700.00 ns, 6.6715 ms/op
WorkloadResult  45: 256 op, 1726006700.00 ns, 6.7422 ms/op
WorkloadResult  46: 256 op, 902590700.00 ns, 3.5257 ms/op
WorkloadResult  47: 256 op, 805430600.00 ns, 3.1462 ms/op
WorkloadResult  48: 256 op, 823749700.00 ns, 3.2178 ms/op
WorkloadResult  49: 256 op, 801246500.00 ns, 3.1299 ms/op
WorkloadResult  50: 256 op, 1387590900.00 ns, 5.4203 ms/op
WorkloadResult  51: 256 op, 1683135400.00 ns, 6.5747 ms/op
WorkloadResult  52: 256 op, 1417887000.00 ns, 5.5386 ms/op
WorkloadResult  53: 256 op, 785182200.00 ns, 3.0671 ms/op
WorkloadResult  54: 256 op, 783010700.00 ns, 3.0586 ms/op
WorkloadResult  55: 256 op, 848310600.00 ns, 3.3137 ms/op
WorkloadResult  56: 256 op, 1196537700.00 ns, 4.6740 ms/op
WorkloadResult  57: 256 op, 1734875300.00 ns, 6.7769 ms/op
WorkloadResult  58: 256 op, 1397627200.00 ns, 5.4595 ms/op
WorkloadResult  59: 256 op, 846989800.00 ns, 3.3086 ms/op
WorkloadResult  60: 256 op, 866268600.00 ns, 3.3839 ms/op
WorkloadResult  61: 256 op, 814350300.00 ns, 3.1811 ms/op
WorkloadResult  62: 256 op, 902035300.00 ns, 3.5236 ms/op
WorkloadResult  63: 256 op, 1722849100.00 ns, 6.7299 ms/op
WorkloadResult  64: 256 op, 1705476000.00 ns, 6.6620 ms/op
WorkloadResult  65: 256 op, 823292700.00 ns, 3.2160 ms/op
WorkloadResult  66: 256 op, 810017100.00 ns, 3.1641 ms/op
WorkloadResult  67: 256 op, 805077200.00 ns, 3.1448 ms/op
WorkloadResult  68: 256 op, 814895600.00 ns, 3.1832 ms/op
WorkloadResult  69: 256 op, 1444685100.00 ns, 5.6433 ms/op
WorkloadResult  70: 256 op, 1677548800.00 ns, 6.5529 ms/op
WorkloadResult  71: 256 op, 1160911300.00 ns, 4.5348 ms/op
WorkloadResult  72: 256 op, 851877200.00 ns, 3.3276 ms/op
WorkloadResult  73: 256 op, 803890500.00 ns, 3.1402 ms/op
WorkloadResult  74: 256 op, 790182300.00 ns, 3.0866 ms/op
WorkloadResult  75: 256 op, 1206195700.00 ns, 4.7117 ms/op
WorkloadResult  76: 256 op, 1672401700.00 ns, 6.5328 ms/op
WorkloadResult  77: 256 op, 1584315400.00 ns, 6.1887 ms/op
WorkloadResult  78: 256 op, 809091500.00 ns, 3.1605 ms/op
WorkloadResult  79: 256 op, 810045000.00 ns, 3.1642 ms/op
WorkloadResult  80: 256 op, 899535300.00 ns, 3.5138 ms/op
WorkloadResult  81: 256 op, 1035038800.00 ns, 4.0431 ms/op
WorkloadResult  82: 256 op, 1699845700.00 ns, 6.6400 ms/op
WorkloadResult  83: 256 op, 1630182000.00 ns, 6.3679 ms/op
WorkloadResult  84: 256 op, 809679800.00 ns, 3.1628 ms/op
WorkloadResult  85: 256 op, 823493700.00 ns, 3.2168 ms/op
WorkloadResult  86: 256 op, 828596100.00 ns, 3.2367 ms/op
WorkloadResult  87: 256 op, 795208000.00 ns, 3.1063 ms/op
WorkloadResult  88: 256 op, 1631943100.00 ns, 6.3748 ms/op
WorkloadResult  89: 256 op, 1691862700.00 ns, 6.6088 ms/op
WorkloadResult  90: 256 op, 1071802000.00 ns, 4.1867 ms/op
WorkloadResult  91: 256 op, 811614200.00 ns, 3.1704 ms/op
WorkloadResult  92: 256 op, 817269300.00 ns, 3.1925 ms/op
WorkloadResult  93: 256 op, 903918500.00 ns, 3.5309 ms/op
WorkloadResult  94: 256 op, 1483686500.00 ns, 5.7957 ms/op
WorkloadResult  95: 256 op, 1711499800.00 ns, 6.6855 ms/op
WorkloadResult  96: 256 op, 1155948000.00 ns, 4.5154 ms/op
WorkloadResult  97: 256 op, 825012800.00 ns, 3.2227 ms/op
WorkloadResult  98: 256 op, 788490100.00 ns, 3.0800 ms/op
WorkloadResult  99: 256 op, 800047700.00 ns, 3.1252 ms/op
WorkloadResult  100: 256 op, 1094089700.00 ns, 4.2738 ms/op

// AfterAll
// Benchmark Process 18208 has exited with code 0.

Mean = 4.340 ms, StdErr = 0.143 ms (3.29%), N = 100, StdDev = 1.428 ms
Min = 3.046 ms, Q1 = 3.179 ms, Median = 3.449 ms, Q3 = 5.701 ms, Max = 6.779 ms
IQR = 2.522 ms, LowerFence = -0.604 ms, UpperFence = 9.484 ms
ConfidenceInterval = [3.856 ms; 4.825 ms] (CI 99.9%), Margin = 0.484 ms (11.15% of Mean)
Skewness = 0.7, Kurtosis = 1.72, MValue = 2.83

// **************************
// Benchmark: WriteText.RunFile: .NET Framework 4.6.1(Runtime=.NET Framework 4.6.1)
// *** Execute ***
// Launch: 1 / 1
// Execute: D:\Microsoft\Workspace\asakura89-project\release\AskScratchpad\CSScratchpad\CSScratchpad\bin\Release\1f7e4b00-9986-4126-998c-f5c92e73d41f.exe --benchmarkName "CSScratchpad.Script.WriteText.RunFile" --job ".NET Framework 4.6.1" --benchmarkId 1 in 
// BeforeAnythingElse

// Benchmark Process Environment Information:
// Runtime=.NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
// GC=Concurrent Workstation
// Job: .NET Framework 4.6.1

OverheadJitting  1: 1 op, 685100.00 ns, 685.1000 us/op
WorkloadJitting  1: 1 op, 29927500.00 ns, 29.9275 ms/op

OverheadJitting  2: 16 op, 516200.00 ns, 32.2625 us/op
WorkloadJitting  2: 16 op, 172797700.00 ns, 10.7999 ms/op

WorkloadPilot    1: 16 op, 177714200.00 ns, 11.1071 ms/op
WorkloadPilot    2: 32 op, 377108200.00 ns, 11.7846 ms/op
WorkloadPilot    3: 64 op, 707049200.00 ns, 11.0476 ms/op

OverheadWarmup   1: 64 op, 4000.00 ns, 62.5000 ns/op
OverheadWarmup   2: 64 op, 900.00 ns, 14.0625 ns/op
OverheadWarmup   3: 64 op, 900.00 ns, 14.0625 ns/op
OverheadWarmup   4: 64 op, 800.00 ns, 12.5000 ns/op
OverheadWarmup   5: 64 op, 800.00 ns, 12.5000 ns/op
OverheadWarmup   6: 64 op, 700.00 ns, 10.9375 ns/op

OverheadActual   1: 64 op, 900.00 ns, 14.0625 ns/op
OverheadActual   2: 64 op, 1100.00 ns, 17.1875 ns/op
OverheadActual   3: 64 op, 1000.00 ns, 15.6250 ns/op
OverheadActual   4: 64 op, 1300.00 ns, 20.3125 ns/op
OverheadActual   5: 64 op, 1200.00 ns, 18.7500 ns/op
OverheadActual   6: 64 op, 1000.00 ns, 15.6250 ns/op
OverheadActual   7: 64 op, 1000.00 ns, 15.6250 ns/op
OverheadActual   8: 64 op, 800.00 ns, 12.5000 ns/op
OverheadActual   9: 64 op, 600.00 ns, 9.3750 ns/op
OverheadActual  10: 64 op, 900.00 ns, 14.0625 ns/op
OverheadActual  11: 64 op, 700.00 ns, 10.9375 ns/op
OverheadActual  12: 64 op, 800.00 ns, 12.5000 ns/op
OverheadActual  13: 64 op, 600.00 ns, 9.3750 ns/op
OverheadActual  14: 64 op, 600.00 ns, 9.3750 ns/op
OverheadActual  15: 64 op, 700.00 ns, 10.9375 ns/op
OverheadActual  16: 64 op, 700.00 ns, 10.9375 ns/op
OverheadActual  17: 64 op, 500.00 ns, 7.8125 ns/op
OverheadActual  18: 64 op, 600.00 ns, 9.3750 ns/op
OverheadActual  19: 64 op, 600.00 ns, 9.3750 ns/op
OverheadActual  20: 64 op, 1600.00 ns, 25.0000 ns/op

WorkloadWarmup   1: 64 op, 303704700.00 ns, 4.7454 ms/op
WorkloadWarmup   2: 64 op, 287652700.00 ns, 4.4946 ms/op
WorkloadWarmup   3: 64 op, 323334300.00 ns, 5.0521 ms/op
WorkloadWarmup   4: 64 op, 275053200.00 ns, 4.2977 ms/op
WorkloadWarmup   5: 64 op, 317723900.00 ns, 4.9644 ms/op
WorkloadWarmup   6: 64 op, 338805500.00 ns, 5.2938 ms/op
WorkloadWarmup   7: 64 op, 351381700.00 ns, 5.4903 ms/op
WorkloadWarmup   8: 64 op, 354664100.00 ns, 5.5416 ms/op
WorkloadWarmup   9: 64 op, 285310200.00 ns, 4.4580 ms/op

// BeforeActualRun
WorkloadActual   1: 64 op, 297736300.00 ns, 4.6521 ms/op
WorkloadActual   2: 64 op, 356368500.00 ns, 5.5683 ms/op
WorkloadActual   3: 64 op, 707576800.00 ns, 11.0559 ms/op
WorkloadActual   4: 64 op, 702651300.00 ns, 10.9789 ms/op
WorkloadActual   5: 64 op, 722004300.00 ns, 11.2813 ms/op
WorkloadActual   6: 64 op, 653961800.00 ns, 10.2182 ms/op
WorkloadActual   7: 64 op, 723898600.00 ns, 11.3109 ms/op
WorkloadActual   8: 64 op, 383008900.00 ns, 5.9845 ms/op
WorkloadActual   9: 64 op, 338560700.00 ns, 5.2900 ms/op
WorkloadActual  10: 64 op, 341941600.00 ns, 5.3428 ms/op
WorkloadActual  11: 64 op, 310236500.00 ns, 4.8474 ms/op
WorkloadActual  12: 64 op, 307489800.00 ns, 4.8045 ms/op
WorkloadActual  13: 64 op, 307026300.00 ns, 4.7973 ms/op
WorkloadActual  14: 64 op, 308548200.00 ns, 4.8211 ms/op
WorkloadActual  15: 64 op, 317663100.00 ns, 4.9635 ms/op
WorkloadActual  16: 64 op, 288241100.00 ns, 4.5038 ms/op
WorkloadActual  17: 64 op, 297252100.00 ns, 4.6446 ms/op
WorkloadActual  18: 64 op, 518662300.00 ns, 8.1041 ms/op
WorkloadActual  19: 64 op, 787335600.00 ns, 12.3021 ms/op
WorkloadActual  20: 64 op, 665035600.00 ns, 10.3912 ms/op
WorkloadActual  21: 64 op, 732445300.00 ns, 11.4445 ms/op
WorkloadActual  22: 64 op, 684908900.00 ns, 10.7017 ms/op
WorkloadActual  23: 64 op, 593894200.00 ns, 9.2796 ms/op
WorkloadActual  24: 64 op, 298990400.00 ns, 4.6717 ms/op
WorkloadActual  25: 64 op, 335051200.00 ns, 5.2352 ms/op
WorkloadActual  26: 64 op, 290398200.00 ns, 4.5375 ms/op
WorkloadActual  27: 64 op, 327091200.00 ns, 5.1108 ms/op
WorkloadActual  28: 64 op, 308729200.00 ns, 4.8239 ms/op
WorkloadActual  29: 64 op, 291553300.00 ns, 4.5555 ms/op
WorkloadActual  30: 64 op, 306213900.00 ns, 4.7846 ms/op
WorkloadActual  31: 64 op, 330322100.00 ns, 5.1613 ms/op
WorkloadActual  32: 64 op, 319184400.00 ns, 4.9873 ms/op
WorkloadActual  33: 64 op, 338181200.00 ns, 5.2841 ms/op
WorkloadActual  34: 64 op, 483491300.00 ns, 7.5546 ms/op
WorkloadActual  35: 64 op, 806111200.00 ns, 12.5955 ms/op
WorkloadActual  36: 64 op, 781620600.00 ns, 12.2128 ms/op
WorkloadActual  37: 64 op, 826566600.00 ns, 12.9151 ms/op
WorkloadActual  38: 64 op, 894324300.00 ns, 13.9738 ms/op
WorkloadActual  39: 64 op, 360719000.00 ns, 5.6362 ms/op
WorkloadActual  40: 64 op, 311913700.00 ns, 4.8737 ms/op
WorkloadActual  41: 64 op, 300223200.00 ns, 4.6910 ms/op
WorkloadActual  42: 64 op, 312718000.00 ns, 4.8862 ms/op
WorkloadActual  43: 64 op, 298745100.00 ns, 4.6679 ms/op
WorkloadActual  44: 64 op, 310418500.00 ns, 4.8503 ms/op
WorkloadActual  45: 64 op, 351461400.00 ns, 5.4916 ms/op
WorkloadActual  46: 64 op, 303916000.00 ns, 4.7487 ms/op
WorkloadActual  47: 64 op, 298175900.00 ns, 4.6590 ms/op
WorkloadActual  48: 64 op, 358393500.00 ns, 5.5999 ms/op
WorkloadActual  49: 64 op, 344029100.00 ns, 5.3755 ms/op
WorkloadActual  50: 64 op, 701069800.00 ns, 10.9542 ms/op
WorkloadActual  51: 64 op, 679057800.00 ns, 10.6103 ms/op
WorkloadActual  52: 64 op, 705492900.00 ns, 11.0233 ms/op
WorkloadActual  53: 64 op, 871828500.00 ns, 13.6223 ms/op
WorkloadActual  54: 64 op, 693611500.00 ns, 10.8377 ms/op
WorkloadActual  55: 64 op, 322013400.00 ns, 5.0315 ms/op
WorkloadActual  56: 64 op, 330341000.00 ns, 5.1616 ms/op
WorkloadActual  57: 64 op, 314467100.00 ns, 4.9135 ms/op
WorkloadActual  58: 64 op, 307389700.00 ns, 4.8030 ms/op
WorkloadActual  59: 64 op, 304184900.00 ns, 4.7529 ms/op
WorkloadActual  60: 64 op, 338479300.00 ns, 5.2887 ms/op
WorkloadActual  61: 64 op, 325550700.00 ns, 5.0867 ms/op
WorkloadActual  62: 64 op, 301965600.00 ns, 4.7182 ms/op
WorkloadActual  63: 64 op, 309369900.00 ns, 4.8339 ms/op
WorkloadActual  64: 64 op, 331490700.00 ns, 5.1795 ms/op
WorkloadActual  65: 64 op, 476845800.00 ns, 7.4507 ms/op
WorkloadActual  66: 64 op, 745546100.00 ns, 11.6492 ms/op
WorkloadActual  67: 64 op, 2419333600.00 ns, 37.8021 ms/op
WorkloadActual  68: 64 op, 460612700.00 ns, 7.1971 ms/op
WorkloadActual  69: 64 op, 374288200.00 ns, 5.8483 ms/op
WorkloadActual  70: 64 op, 305812500.00 ns, 4.7783 ms/op
WorkloadActual  71: 64 op, 312834900.00 ns, 4.8880 ms/op
WorkloadActual  72: 64 op, 329559200.00 ns, 5.1494 ms/op
WorkloadActual  73: 64 op, 312502000.00 ns, 4.8828 ms/op
WorkloadActual  74: 64 op, 306640300.00 ns, 4.7913 ms/op
WorkloadActual  75: 64 op, 302715400.00 ns, 4.7299 ms/op
WorkloadActual  76: 64 op, 304933900.00 ns, 4.7646 ms/op
WorkloadActual  77: 64 op, 299873200.00 ns, 4.6855 ms/op
WorkloadActual  78: 64 op, 296331500.00 ns, 4.6302 ms/op
WorkloadActual  79: 64 op, 751057300.00 ns, 11.7353 ms/op
WorkloadActual  80: 64 op, 647798600.00 ns, 10.1219 ms/op
WorkloadActual  81: 64 op, 848952700.00 ns, 13.2649 ms/op
WorkloadActual  82: 64 op, 826453800.00 ns, 12.9133 ms/op
WorkloadActual  83: 64 op, 672411700.00 ns, 10.5064 ms/op
WorkloadActual  84: 64 op, 313893600.00 ns, 4.9046 ms/op
WorkloadActual  85: 64 op, 303524400.00 ns, 4.7426 ms/op
WorkloadActual  86: 64 op, 311095900.00 ns, 4.8609 ms/op
WorkloadActual  87: 64 op, 310269200.00 ns, 4.8480 ms/op
WorkloadActual  88: 64 op, 316773000.00 ns, 4.9496 ms/op
WorkloadActual  89: 64 op, 348624300.00 ns, 5.4473 ms/op
WorkloadActual  90: 64 op, 324962400.00 ns, 5.0775 ms/op
WorkloadActual  91: 64 op, 302370400.00 ns, 4.7245 ms/op
WorkloadActual  92: 64 op, 319414400.00 ns, 4.9909 ms/op
WorkloadActual  93: 64 op, 366767600.00 ns, 5.7307 ms/op
WorkloadActual  94: 64 op, 795328200.00 ns, 12.4270 ms/op
WorkloadActual  95: 64 op, 728896200.00 ns, 11.3890 ms/op
WorkloadActual  96: 64 op, 723304400.00 ns, 11.3016 ms/op
WorkloadActual  97: 64 op, 706355100.00 ns, 11.0368 ms/op
WorkloadActual  98: 64 op, 786192100.00 ns, 12.2843 ms/op
WorkloadActual  99: 64 op, 299131100.00 ns, 4.6739 ms/op
WorkloadActual  100: 64 op, 316840300.00 ns, 4.9506 ms/op

// AfterActualRun
WorkloadResult   1: 64 op, 297735500.00 ns, 4.6521 ms/op
WorkloadResult   2: 64 op, 356367700.00 ns, 5.5682 ms/op
WorkloadResult   3: 64 op, 707576000.00 ns, 11.0559 ms/op
WorkloadResult   4: 64 op, 702650500.00 ns, 10.9789 ms/op
WorkloadResult   5: 64 op, 722003500.00 ns, 11.2813 ms/op
WorkloadResult   6: 64 op, 653961000.00 ns, 10.2181 ms/op
WorkloadResult   7: 64 op, 723897800.00 ns, 11.3109 ms/op
WorkloadResult   8: 64 op, 383008100.00 ns, 5.9845 ms/op
WorkloadResult   9: 64 op, 338559900.00 ns, 5.2900 ms/op
WorkloadResult  10: 64 op, 341940800.00 ns, 5.3428 ms/op
WorkloadResult  11: 64 op, 310235700.00 ns, 4.8474 ms/op
WorkloadResult  12: 64 op, 307489000.00 ns, 4.8045 ms/op
WorkloadResult  13: 64 op, 307025500.00 ns, 4.7973 ms/op
WorkloadResult  14: 64 op, 308547400.00 ns, 4.8211 ms/op
WorkloadResult  15: 64 op, 317662300.00 ns, 4.9635 ms/op
WorkloadResult  16: 64 op, 288240300.00 ns, 4.5038 ms/op
WorkloadResult  17: 64 op, 297251300.00 ns, 4.6446 ms/op
WorkloadResult  18: 64 op, 518661500.00 ns, 8.1041 ms/op
WorkloadResult  19: 64 op, 787334800.00 ns, 12.3021 ms/op
WorkloadResult  20: 64 op, 665034800.00 ns, 10.3912 ms/op
WorkloadResult  21: 64 op, 732444500.00 ns, 11.4444 ms/op
WorkloadResult  22: 64 op, 684908100.00 ns, 10.7017 ms/op
WorkloadResult  23: 64 op, 593893400.00 ns, 9.2796 ms/op
WorkloadResult  24: 64 op, 298989600.00 ns, 4.6717 ms/op
WorkloadResult  25: 64 op, 335050400.00 ns, 5.2352 ms/op
WorkloadResult  26: 64 op, 290397400.00 ns, 4.5375 ms/op
WorkloadResult  27: 64 op, 327090400.00 ns, 5.1108 ms/op
WorkloadResult  28: 64 op, 308728400.00 ns, 4.8239 ms/op
WorkloadResult  29: 64 op, 291552500.00 ns, 4.5555 ms/op
WorkloadResult  30: 64 op, 306213100.00 ns, 4.7846 ms/op
WorkloadResult  31: 64 op, 330321300.00 ns, 5.1613 ms/op
WorkloadResult  32: 64 op, 319183600.00 ns, 4.9872 ms/op
WorkloadResult  33: 64 op, 338180400.00 ns, 5.2841 ms/op
WorkloadResult  34: 64 op, 483490500.00 ns, 7.5545 ms/op
WorkloadResult  35: 64 op, 806110400.00 ns, 12.5955 ms/op
WorkloadResult  36: 64 op, 781619800.00 ns, 12.2128 ms/op
WorkloadResult  37: 64 op, 826565800.00 ns, 12.9151 ms/op
WorkloadResult  38: 64 op, 894323500.00 ns, 13.9738 ms/op
WorkloadResult  39: 64 op, 360718200.00 ns, 5.6362 ms/op
WorkloadResult  40: 64 op, 311912900.00 ns, 4.8736 ms/op
WorkloadResult  41: 64 op, 300222400.00 ns, 4.6910 ms/op
WorkloadResult  42: 64 op, 312717200.00 ns, 4.8862 ms/op
WorkloadResult  43: 64 op, 298744300.00 ns, 4.6679 ms/op
WorkloadResult  44: 64 op, 310417700.00 ns, 4.8503 ms/op
WorkloadResult  45: 64 op, 351460600.00 ns, 5.4916 ms/op
WorkloadResult  46: 64 op, 303915200.00 ns, 4.7487 ms/op
WorkloadResult  47: 64 op, 298175100.00 ns, 4.6590 ms/op
WorkloadResult  48: 64 op, 358392700.00 ns, 5.5999 ms/op
WorkloadResult  49: 64 op, 344028300.00 ns, 5.3754 ms/op
WorkloadResult  50: 64 op, 701069000.00 ns, 10.9542 ms/op
WorkloadResult  51: 64 op, 679057000.00 ns, 10.6103 ms/op
WorkloadResult  52: 64 op, 705492100.00 ns, 11.0233 ms/op
WorkloadResult  53: 64 op, 871827700.00 ns, 13.6223 ms/op
WorkloadResult  54: 64 op, 693610700.00 ns, 10.8377 ms/op
WorkloadResult  55: 64 op, 322012600.00 ns, 5.0314 ms/op
WorkloadResult  56: 64 op, 330340200.00 ns, 5.1616 ms/op
WorkloadResult  57: 64 op, 314466300.00 ns, 4.9135 ms/op
WorkloadResult  58: 64 op, 307388900.00 ns, 4.8030 ms/op
WorkloadResult  59: 64 op, 304184100.00 ns, 4.7529 ms/op
WorkloadResult  60: 64 op, 338478500.00 ns, 5.2887 ms/op
WorkloadResult  61: 64 op, 325549900.00 ns, 5.0867 ms/op
WorkloadResult  62: 64 op, 301964800.00 ns, 4.7182 ms/op
WorkloadResult  63: 64 op, 309369100.00 ns, 4.8339 ms/op
WorkloadResult  64: 64 op, 331489900.00 ns, 5.1795 ms/op
WorkloadResult  65: 64 op, 476845000.00 ns, 7.4507 ms/op
WorkloadResult  66: 64 op, 745545300.00 ns, 11.6491 ms/op
WorkloadResult  67: 64 op, 460611900.00 ns, 7.1971 ms/op
WorkloadResult  68: 64 op, 374287400.00 ns, 5.8482 ms/op
WorkloadResult  69: 64 op, 305811700.00 ns, 4.7783 ms/op
WorkloadResult  70: 64 op, 312834100.00 ns, 4.8880 ms/op
WorkloadResult  71: 64 op, 329558400.00 ns, 5.1494 ms/op
WorkloadResult  72: 64 op, 312501200.00 ns, 4.8828 ms/op
WorkloadResult  73: 64 op, 306639500.00 ns, 4.7912 ms/op
WorkloadResult  74: 64 op, 302714600.00 ns, 4.7299 ms/op
WorkloadResult  75: 64 op, 304933100.00 ns, 4.7646 ms/op
WorkloadResult  76: 64 op, 299872400.00 ns, 4.6855 ms/op
WorkloadResult  77: 64 op, 296330700.00 ns, 4.6302 ms/op
WorkloadResult  78: 64 op, 751056500.00 ns, 11.7353 ms/op
WorkloadResult  79: 64 op, 647797800.00 ns, 10.1218 ms/op
WorkloadResult  80: 64 op, 848951900.00 ns, 13.2649 ms/op
WorkloadResult  81: 64 op, 826453000.00 ns, 12.9133 ms/op
WorkloadResult  82: 64 op, 672410900.00 ns, 10.5064 ms/op
WorkloadResult  83: 64 op, 313892800.00 ns, 4.9046 ms/op
WorkloadResult  84: 64 op, 303523600.00 ns, 4.7426 ms/op
WorkloadResult  85: 64 op, 311095100.00 ns, 4.8609 ms/op
WorkloadResult  86: 64 op, 310268400.00 ns, 4.8479 ms/op
WorkloadResult  87: 64 op, 316772200.00 ns, 4.9496 ms/op
WorkloadResult  88: 64 op, 348623500.00 ns, 5.4472 ms/op
WorkloadResult  89: 64 op, 324961600.00 ns, 5.0775 ms/op
WorkloadResult  90: 64 op, 302369600.00 ns, 4.7245 ms/op
WorkloadResult  91: 64 op, 319413600.00 ns, 4.9908 ms/op
WorkloadResult  92: 64 op, 366766800.00 ns, 5.7307 ms/op
WorkloadResult  93: 64 op, 795327400.00 ns, 12.4270 ms/op
WorkloadResult  94: 64 op, 728895400.00 ns, 11.3890 ms/op
WorkloadResult  95: 64 op, 723303600.00 ns, 11.3016 ms/op
WorkloadResult  96: 64 op, 706354300.00 ns, 11.0368 ms/op
WorkloadResult  97: 64 op, 786191300.00 ns, 12.2842 ms/op
WorkloadResult  98: 64 op, 299130300.00 ns, 4.6739 ms/op
WorkloadResult  99: 64 op, 316839500.00 ns, 4.9506 ms/op

// AfterAll
// Benchmark Process 18668 has exited with code 0.

Mean = 7.074 ms, StdErr = 0.309 ms (4.37%), N = 99, StdDev = 3.078 ms
Min = 4.504 ms, Q1 = 4.813 ms, Median = 5.162 ms, Q3 = 10.558 ms, Max = 13.974 ms
IQR = 5.746 ms, LowerFence = -3.806 ms, UpperFence = 19.177 ms
ConfidenceInterval = [6.024 ms; 8.124 ms] (CI 99.9%), Margin = 1.050 ms (14.84% of Mean)
Skewness = 0.88, Kurtosis = 2.03, MValue = 2.74

// **************************
// Benchmark: WriteText.RunTextWriter: .NET Framework 4.7.2(Runtime=.NET Framework 4.7.2)
// *** Execute ***
// Launch: 1 / 1
// Execute: D:\Microsoft\Workspace\asakura89-project\release\AskScratchpad\CSScratchpad\CSScratchpad\bin\Release\cd639051-2e07-41f2-ba8b-0beb0feeb07d.exe --benchmarkName "CSScratchpad.Script.WriteText.RunTextWriter" --job ".NET Framework 4.7.2" --benchmarkId 0 in 
// BeforeAnythingElse

// Benchmark Process Environment Information:
// Runtime=.NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
// GC=Concurrent Workstation
// Job: .NET Framework 4.7.2

OverheadJitting  1: 1 op, 285500.00 ns, 285.5000 us/op
WorkloadJitting  1: 1 op, 7767900.00 ns, 7.7679 ms/op

OverheadJitting  2: 16 op, 215300.00 ns, 13.4563 us/op
WorkloadJitting  2: 16 op, 57566100.00 ns, 3.5979 ms/op

WorkloadPilot    1: 16 op, 57842000.00 ns, 3.6151 ms/op
WorkloadPilot    2: 32 op, 104423300.00 ns, 3.2632 ms/op
WorkloadPilot    3: 64 op, 204689600.00 ns, 3.1983 ms/op
WorkloadPilot    4: 128 op, 412791300.00 ns, 3.2249 ms/op
WorkloadPilot    5: 256 op, 831181200.00 ns, 3.2468 ms/op

OverheadWarmup   1: 256 op, 5200.00 ns, 20.3125 ns/op
OverheadWarmup   2: 256 op, 1500.00 ns, 5.8594 ns/op
OverheadWarmup   3: 256 op, 1200.00 ns, 4.6875 ns/op
OverheadWarmup   4: 256 op, 1400.00 ns, 5.4688 ns/op
OverheadWarmup   5: 256 op, 1200.00 ns, 4.6875 ns/op
OverheadWarmup   6: 256 op, 1100.00 ns, 4.2969 ns/op
OverheadWarmup   7: 256 op, 1200.00 ns, 4.6875 ns/op
OverheadWarmup   8: 256 op, 1400.00 ns, 5.4688 ns/op
OverheadWarmup   9: 256 op, 1200.00 ns, 4.6875 ns/op

OverheadActual   1: 256 op, 4400.00 ns, 17.1875 ns/op
OverheadActual   2: 256 op, 1500.00 ns, 5.8594 ns/op
OverheadActual   3: 256 op, 1500.00 ns, 5.8594 ns/op
OverheadActual   4: 256 op, 1200.00 ns, 4.6875 ns/op
OverheadActual   5: 256 op, 1200.00 ns, 4.6875 ns/op
OverheadActual   6: 256 op, 1500.00 ns, 5.8594 ns/op
OverheadActual   7: 256 op, 1300.00 ns, 5.0781 ns/op
OverheadActual   8: 256 op, 1200.00 ns, 4.6875 ns/op
OverheadActual   9: 256 op, 5300.00 ns, 20.7031 ns/op
OverheadActual  10: 256 op, 1300.00 ns, 5.0781 ns/op
OverheadActual  11: 256 op, 1400.00 ns, 5.4688 ns/op
OverheadActual  12: 256 op, 1100.00 ns, 4.2969 ns/op
OverheadActual  13: 256 op, 1400.00 ns, 5.4688 ns/op
OverheadActual  14: 256 op, 1200.00 ns, 4.6875 ns/op
OverheadActual  15: 256 op, 1200.00 ns, 4.6875 ns/op
OverheadActual  16: 256 op, 1700.00 ns, 6.6406 ns/op
OverheadActual  17: 256 op, 3700.00 ns, 14.4531 ns/op
OverheadActual  18: 256 op, 1300.00 ns, 5.0781 ns/op
OverheadActual  19: 256 op, 1700.00 ns, 6.6406 ns/op
OverheadActual  20: 256 op, 1400.00 ns, 5.4688 ns/op

WorkloadWarmup   1: 256 op, 1314708000.00 ns, 5.1356 ms/op
WorkloadWarmup   2: 256 op, 1754113800.00 ns, 6.8520 ms/op
WorkloadWarmup   3: 256 op, 1411563600.00 ns, 5.5139 ms/op
WorkloadWarmup   4: 256 op, 835109600.00 ns, 3.2621 ms/op
WorkloadWarmup   5: 256 op, 825028600.00 ns, 3.2228 ms/op
WorkloadWarmup   6: 256 op, 820432400.00 ns, 3.2048 ms/op
WorkloadWarmup   7: 256 op, 1058335700.00 ns, 4.1341 ms/op
WorkloadWarmup   8: 256 op, 1732343600.00 ns, 6.7670 ms/op
WorkloadWarmup   9: 256 op, 1747247700.00 ns, 6.8252 ms/op
WorkloadWarmup  10: 256 op, 1280934600.00 ns, 5.0037 ms/op

// BeforeActualRun
WorkloadActual   1: 256 op, 845658200.00 ns, 3.3034 ms/op
WorkloadActual   2: 256 op, 812748900.00 ns, 3.1748 ms/op
WorkloadActual   3: 256 op, 834295500.00 ns, 3.2590 ms/op
WorkloadActual   4: 256 op, 832329100.00 ns, 3.2513 ms/op
WorkloadActual   5: 256 op, 1457736900.00 ns, 5.6943 ms/op
WorkloadActual   6: 256 op, 1693368600.00 ns, 6.6147 ms/op
WorkloadActual   7: 256 op, 1202312500.00 ns, 4.6965 ms/op
WorkloadActual   8: 256 op, 875699400.00 ns, 3.4207 ms/op
WorkloadActual   9: 256 op, 808074800.00 ns, 3.1565 ms/op
WorkloadActual  10: 256 op, 822454900.00 ns, 3.2127 ms/op
WorkloadActual  11: 256 op, 1308667600.00 ns, 5.1120 ms/op
WorkloadActual  12: 256 op, 1721785400.00 ns, 6.7257 ms/op
WorkloadActual  13: 256 op, 1344006000.00 ns, 5.2500 ms/op
WorkloadActual  14: 256 op, 816487000.00 ns, 3.1894 ms/op
WorkloadActual  15: 256 op, 846415100.00 ns, 3.3063 ms/op
WorkloadActual  16: 256 op, 807618700.00 ns, 3.1548 ms/op
WorkloadActual  17: 256 op, 1003198500.00 ns, 3.9187 ms/op
WorkloadActual  18: 256 op, 1691848900.00 ns, 6.6088 ms/op
WorkloadActual  19: 256 op, 1683470900.00 ns, 6.5761 ms/op
WorkloadActual  20: 256 op, 1229331500.00 ns, 4.8021 ms/op
WorkloadActual  21: 256 op, 810256700.00 ns, 3.1651 ms/op
WorkloadActual  22: 256 op, 840872200.00 ns, 3.2847 ms/op
WorkloadActual  23: 256 op, 1219462300.00 ns, 4.7635 ms/op
WorkloadActual  24: 256 op, 1722529400.00 ns, 6.7286 ms/op
WorkloadActual  25: 256 op, 1585271600.00 ns, 6.1925 ms/op
WorkloadActual  26: 256 op, 803385000.00 ns, 3.1382 ms/op
WorkloadActual  27: 256 op, 894038600.00 ns, 3.4923 ms/op
WorkloadActual  28: 256 op, 808081800.00 ns, 3.1566 ms/op
WorkloadActual  29: 256 op, 1156681700.00 ns, 4.5183 ms/op
WorkloadActual  30: 256 op, 1711427300.00 ns, 6.6853 ms/op
WorkloadActual  31: 256 op, 1648882700.00 ns, 6.4409 ms/op
WorkloadActual  32: 256 op, 878566000.00 ns, 3.4319 ms/op
WorkloadActual  33: 256 op, 932360700.00 ns, 3.6420 ms/op
WorkloadActual  34: 256 op, 842630900.00 ns, 3.2915 ms/op
WorkloadActual  35: 256 op, 1495664600.00 ns, 5.8424 ms/op
WorkloadActual  36: 256 op, 1868136600.00 ns, 7.2974 ms/op
WorkloadActual  37: 256 op, 1537108300.00 ns, 6.0043 ms/op
WorkloadActual  38: 256 op, 884093000.00 ns, 3.4535 ms/op
WorkloadActual  39: 256 op, 800343500.00 ns, 3.1263 ms/op
WorkloadActual  40: 256 op, 830969300.00 ns, 3.2460 ms/op
WorkloadActual  41: 256 op, 1600312500.00 ns, 6.2512 ms/op
WorkloadActual  42: 256 op, 1686939600.00 ns, 6.5896 ms/op
WorkloadActual  43: 256 op, 1061392300.00 ns, 4.1461 ms/op
WorkloadActual  44: 256 op, 800795700.00 ns, 3.1281 ms/op
WorkloadActual  45: 256 op, 841165200.00 ns, 3.2858 ms/op
WorkloadActual  46: 256 op, 845532400.00 ns, 3.3029 ms/op
WorkloadActual  47: 256 op, 1281810200.00 ns, 5.0071 ms/op
WorkloadActual  48: 256 op, 1688337500.00 ns, 6.5951 ms/op
WorkloadActual  49: 256 op, 1684992200.00 ns, 6.5820 ms/op
WorkloadActual  50: 256 op, 1184548800.00 ns, 4.6271 ms/op
WorkloadActual  51: 256 op, 807690700.00 ns, 3.1550 ms/op
WorkloadActual  52: 256 op, 788803400.00 ns, 3.0813 ms/op
WorkloadActual  53: 256 op, 791679300.00 ns, 3.0925 ms/op
WorkloadActual  54: 256 op, 1399388800.00 ns, 5.4664 ms/op
WorkloadActual  55: 256 op, 1684041700.00 ns, 6.5783 ms/op
WorkloadActual  56: 256 op, 1684859600.00 ns, 6.5815 ms/op
WorkloadActual  57: 256 op, 1032251100.00 ns, 4.0322 ms/op
WorkloadActual  58: 256 op, 789047200.00 ns, 3.0822 ms/op
WorkloadActual  59: 256 op, 795656700.00 ns, 3.1080 ms/op
WorkloadActual  60: 256 op, 830841300.00 ns, 3.2455 ms/op
WorkloadActual  61: 256 op, 1247075400.00 ns, 4.8714 ms/op
WorkloadActual  62: 256 op, 1697418900.00 ns, 6.6305 ms/op
WorkloadActual  63: 256 op, 1690864500.00 ns, 6.6049 ms/op
WorkloadActual  64: 256 op, 1082881700.00 ns, 4.2300 ms/op
WorkloadActual  65: 256 op, 804892500.00 ns, 3.1441 ms/op
WorkloadActual  66: 256 op, 809475600.00 ns, 3.1620 ms/op
WorkloadActual  67: 256 op, 832243100.00 ns, 3.2509 ms/op
WorkloadActual  68: 256 op, 1177865600.00 ns, 4.6010 ms/op
WorkloadActual  69: 256 op, 1699522300.00 ns, 6.6388 ms/op
WorkloadActual  70: 256 op, 1691173500.00 ns, 6.6061 ms/op
WorkloadActual  71: 256 op, 1295007600.00 ns, 5.0586 ms/op
WorkloadActual  72: 256 op, 802106200.00 ns, 3.1332 ms/op
WorkloadActual  73: 256 op, 794572800.00 ns, 3.1038 ms/op
WorkloadActual  74: 256 op, 807390200.00 ns, 3.1539 ms/op
WorkloadActual  75: 256 op, 1202301900.00 ns, 4.6965 ms/op
WorkloadActual  76: 256 op, 1680748200.00 ns, 6.5654 ms/op
WorkloadActual  77: 256 op, 1688095000.00 ns, 6.5941 ms/op
WorkloadActual  78: 256 op, 1124816100.00 ns, 4.3938 ms/op
WorkloadActual  79: 256 op, 815035000.00 ns, 3.1837 ms/op
WorkloadActual  80: 256 op, 827247700.00 ns, 3.2314 ms/op
WorkloadActual  81: 256 op, 862657500.00 ns, 3.3698 ms/op
WorkloadActual  82: 256 op, 1069166700.00 ns, 4.1764 ms/op
WorkloadActual  83: 256 op, 1673658700.00 ns, 6.5377 ms/op
WorkloadActual  84: 256 op, 1747266900.00 ns, 6.8253 ms/op
WorkloadActual  85: 256 op, 1369592000.00 ns, 5.3500 ms/op
WorkloadActual  86: 256 op, 837591600.00 ns, 3.2718 ms/op
WorkloadActual  87: 256 op, 807868700.00 ns, 3.1557 ms/op
WorkloadActual  88: 256 op, 831821100.00 ns, 3.2493 ms/op
WorkloadActual  89: 256 op, 1215009000.00 ns, 4.7461 ms/op
WorkloadActual  90: 256 op, 1695842300.00 ns, 6.6244 ms/op
WorkloadActual  91: 256 op, 1735158800.00 ns, 6.7780 ms/op
WorkloadActual  92: 256 op, 1141009100.00 ns, 4.4571 ms/op
WorkloadActual  93: 256 op, 857284500.00 ns, 3.3488 ms/op
WorkloadActual  94: 256 op, 804776700.00 ns, 3.1437 ms/op
WorkloadActual  95: 256 op, 841950000.00 ns, 3.2889 ms/op
WorkloadActual  96: 256 op, 1321351000.00 ns, 5.1615 ms/op
WorkloadActual  97: 256 op, 1710017100.00 ns, 6.6798 ms/op
WorkloadActual  98: 256 op, 1719871900.00 ns, 6.7182 ms/op
WorkloadActual  99: 256 op, 1032809000.00 ns, 4.0344 ms/op
WorkloadActual  100: 256 op, 809275300.00 ns, 3.1612 ms/op

// AfterActualRun
WorkloadResult   1: 256 op, 845656800.00 ns, 3.3033 ms/op
WorkloadResult   2: 256 op, 812747500.00 ns, 3.1748 ms/op
WorkloadResult   3: 256 op, 834294100.00 ns, 3.2590 ms/op
WorkloadResult   4: 256 op, 832327700.00 ns, 3.2513 ms/op
WorkloadResult   5: 256 op, 1457735500.00 ns, 5.6943 ms/op
WorkloadResult   6: 256 op, 1693367200.00 ns, 6.6147 ms/op
WorkloadResult   7: 256 op, 1202311100.00 ns, 4.6965 ms/op
WorkloadResult   8: 256 op, 875698000.00 ns, 3.4207 ms/op
WorkloadResult   9: 256 op, 808073400.00 ns, 3.1565 ms/op
WorkloadResult  10: 256 op, 822453500.00 ns, 3.2127 ms/op
WorkloadResult  11: 256 op, 1308666200.00 ns, 5.1120 ms/op
WorkloadResult  12: 256 op, 1721784000.00 ns, 6.7257 ms/op
WorkloadResult  13: 256 op, 1344004600.00 ns, 5.2500 ms/op
WorkloadResult  14: 256 op, 816485600.00 ns, 3.1894 ms/op
WorkloadResult  15: 256 op, 846413700.00 ns, 3.3063 ms/op
WorkloadResult  16: 256 op, 807617300.00 ns, 3.1548 ms/op
WorkloadResult  17: 256 op, 1003197100.00 ns, 3.9187 ms/op
WorkloadResult  18: 256 op, 1691847500.00 ns, 6.6088 ms/op
WorkloadResult  19: 256 op, 1683469500.00 ns, 6.5761 ms/op
WorkloadResult  20: 256 op, 1229330100.00 ns, 4.8021 ms/op
WorkloadResult  21: 256 op, 810255300.00 ns, 3.1651 ms/op
WorkloadResult  22: 256 op, 840870800.00 ns, 3.2847 ms/op
WorkloadResult  23: 256 op, 1219460900.00 ns, 4.7635 ms/op
WorkloadResult  24: 256 op, 1722528000.00 ns, 6.7286 ms/op
WorkloadResult  25: 256 op, 1585270200.00 ns, 6.1925 ms/op
WorkloadResult  26: 256 op, 803383600.00 ns, 3.1382 ms/op
WorkloadResult  27: 256 op, 894037200.00 ns, 3.4923 ms/op
WorkloadResult  28: 256 op, 808080400.00 ns, 3.1566 ms/op
WorkloadResult  29: 256 op, 1156680300.00 ns, 4.5183 ms/op
WorkloadResult  30: 256 op, 1711425900.00 ns, 6.6853 ms/op
WorkloadResult  31: 256 op, 1648881300.00 ns, 6.4409 ms/op
WorkloadResult  32: 256 op, 878564600.00 ns, 3.4319 ms/op
WorkloadResult  33: 256 op, 932359300.00 ns, 3.6420 ms/op
WorkloadResult  34: 256 op, 842629500.00 ns, 3.2915 ms/op
WorkloadResult  35: 256 op, 1495663200.00 ns, 5.8424 ms/op
WorkloadResult  36: 256 op, 1868135200.00 ns, 7.2974 ms/op
WorkloadResult  37: 256 op, 1537106900.00 ns, 6.0043 ms/op
WorkloadResult  38: 256 op, 884091600.00 ns, 3.4535 ms/op
WorkloadResult  39: 256 op, 800342100.00 ns, 3.1263 ms/op
WorkloadResult  40: 256 op, 830967900.00 ns, 3.2460 ms/op
WorkloadResult  41: 256 op, 1600311100.00 ns, 6.2512 ms/op
WorkloadResult  42: 256 op, 1686938200.00 ns, 6.5896 ms/op
WorkloadResult  43: 256 op, 1061390900.00 ns, 4.1461 ms/op
WorkloadResult  44: 256 op, 800794300.00 ns, 3.1281 ms/op
WorkloadResult  45: 256 op, 841163800.00 ns, 3.2858 ms/op
WorkloadResult  46: 256 op, 845531000.00 ns, 3.3029 ms/op
WorkloadResult  47: 256 op, 1281808800.00 ns, 5.0071 ms/op
WorkloadResult  48: 256 op, 1688336100.00 ns, 6.5951 ms/op
WorkloadResult  49: 256 op, 1684990800.00 ns, 6.5820 ms/op
WorkloadResult  50: 256 op, 1184547400.00 ns, 4.6271 ms/op
WorkloadResult  51: 256 op, 807689300.00 ns, 3.1550 ms/op
WorkloadResult  52: 256 op, 788802000.00 ns, 3.0813 ms/op
WorkloadResult  53: 256 op, 791677900.00 ns, 3.0925 ms/op
WorkloadResult  54: 256 op, 1399387400.00 ns, 5.4664 ms/op
WorkloadResult  55: 256 op, 1684040300.00 ns, 6.5783 ms/op
WorkloadResult  56: 256 op, 1684858200.00 ns, 6.5815 ms/op
WorkloadResult  57: 256 op, 1032249700.00 ns, 4.0322 ms/op
WorkloadResult  58: 256 op, 789045800.00 ns, 3.0822 ms/op
WorkloadResult  59: 256 op, 795655300.00 ns, 3.1080 ms/op
WorkloadResult  60: 256 op, 830839900.00 ns, 3.2455 ms/op
WorkloadResult  61: 256 op, 1247074000.00 ns, 4.8714 ms/op
WorkloadResult  62: 256 op, 1697417500.00 ns, 6.6305 ms/op
WorkloadResult  63: 256 op, 1690863100.00 ns, 6.6049 ms/op
WorkloadResult  64: 256 op, 1082880300.00 ns, 4.2300 ms/op
WorkloadResult  65: 256 op, 804891100.00 ns, 3.1441 ms/op
WorkloadResult  66: 256 op, 809474200.00 ns, 3.1620 ms/op
WorkloadResult  67: 256 op, 832241700.00 ns, 3.2509 ms/op
WorkloadResult  68: 256 op, 1177864200.00 ns, 4.6010 ms/op
WorkloadResult  69: 256 op, 1699520900.00 ns, 6.6388 ms/op
WorkloadResult  70: 256 op, 1691172100.00 ns, 6.6061 ms/op
WorkloadResult  71: 256 op, 1295006200.00 ns, 5.0586 ms/op
WorkloadResult  72: 256 op, 802104800.00 ns, 3.1332 ms/op
WorkloadResult  73: 256 op, 794571400.00 ns, 3.1038 ms/op
WorkloadResult  74: 256 op, 807388800.00 ns, 3.1539 ms/op
WorkloadResult  75: 256 op, 1202300500.00 ns, 4.6965 ms/op
WorkloadResult  76: 256 op, 1680746800.00 ns, 6.5654 ms/op
WorkloadResult  77: 256 op, 1688093600.00 ns, 6.5941 ms/op
WorkloadResult  78: 256 op, 1124814700.00 ns, 4.3938 ms/op
WorkloadResult  79: 256 op, 815033600.00 ns, 3.1837 ms/op
WorkloadResult  80: 256 op, 827246300.00 ns, 3.2314 ms/op
WorkloadResult  81: 256 op, 862656100.00 ns, 3.3698 ms/op
WorkloadResult  82: 256 op, 1069165300.00 ns, 4.1764 ms/op
WorkloadResult  83: 256 op, 1673657300.00 ns, 6.5377 ms/op
WorkloadResult  84: 256 op, 1747265500.00 ns, 6.8253 ms/op
WorkloadResult  85: 256 op, 1369590600.00 ns, 5.3500 ms/op
WorkloadResult  86: 256 op, 837590200.00 ns, 3.2718 ms/op
WorkloadResult  87: 256 op, 807867300.00 ns, 3.1557 ms/op
WorkloadResult  88: 256 op, 831819700.00 ns, 3.2493 ms/op
WorkloadResult  89: 256 op, 1215007600.00 ns, 4.7461 ms/op
WorkloadResult  90: 256 op, 1695840900.00 ns, 6.6244 ms/op
WorkloadResult  91: 256 op, 1735157400.00 ns, 6.7780 ms/op
WorkloadResult  92: 256 op, 1141007700.00 ns, 4.4571 ms/op
WorkloadResult  93: 256 op, 857283100.00 ns, 3.3488 ms/op
WorkloadResult  94: 256 op, 804775300.00 ns, 3.1437 ms/op
WorkloadResult  95: 256 op, 841948600.00 ns, 3.2889 ms/op
WorkloadResult  96: 256 op, 1321349600.00 ns, 5.1615 ms/op
WorkloadResult  97: 256 op, 1710015700.00 ns, 6.6797 ms/op
WorkloadResult  98: 256 op, 1719870500.00 ns, 6.7182 ms/op
WorkloadResult  99: 256 op, 1032807600.00 ns, 4.0344 ms/op
WorkloadResult  100: 256 op, 809273900.00 ns, 3.1612 ms/op

// AfterAll
// Benchmark Process 22632 has exited with code 0.

Mean = 4.572 ms, StdErr = 0.145 ms (3.17%), N = 100, StdDev = 1.447 ms
Min = 3.081 ms, Q1 = 3.242 ms, Median = 4.161 ms, Q3 = 6.299 ms, Max = 7.297 ms
IQR = 3.057 ms, LowerFence = -1.343 ms, UpperFence = 10.884 ms
ConfidenceInterval = [4.081 ms; 5.063 ms] (CI 99.9%), Margin = 0.491 ms (10.74% of Mean)
Skewness = 0.44, Kurtosis = 1.52, MValue = 3.26

// **************************
// Benchmark: WriteText.RunFile: .NET Framework 4.7.2(Runtime=.NET Framework 4.7.2)
// *** Execute ***
// Launch: 1 / 1
// Execute: D:\Microsoft\Workspace\asakura89-project\release\AskScratchpad\CSScratchpad\CSScratchpad\bin\Release\cd639051-2e07-41f2-ba8b-0beb0feeb07d.exe --benchmarkName "CSScratchpad.Script.WriteText.RunFile" --job ".NET Framework 4.7.2" --benchmarkId 1 in 
// BeforeAnythingElse

// Benchmark Process Environment Information:
// Runtime=.NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
// GC=Concurrent Workstation
// Job: .NET Framework 4.7.2

OverheadJitting  1: 1 op, 236100.00 ns, 236.1000 us/op
WorkloadJitting  1: 1 op, 22844800.00 ns, 22.8448 ms/op

OverheadJitting  2: 16 op, 217800.00 ns, 13.6125 us/op
WorkloadJitting  2: 16 op, 87891700.00 ns, 5.4932 ms/op

WorkloadPilot    1: 16 op, 82132500.00 ns, 5.1333 ms/op
WorkloadPilot    2: 32 op, 171591100.00 ns, 5.3622 ms/op
WorkloadPilot    3: 64 op, 322090500.00 ns, 5.0327 ms/op
WorkloadPilot    4: 128 op, 660006100.00 ns, 5.1563 ms/op

OverheadWarmup   1: 128 op, 10400.00 ns, 81.2500 ns/op
OverheadWarmup   2: 128 op, 1000.00 ns, 7.8125 ns/op
OverheadWarmup   3: 128 op, 800.00 ns, 6.2500 ns/op
OverheadWarmup   4: 128 op, 900.00 ns, 7.0313 ns/op
OverheadWarmup   5: 128 op, 800.00 ns, 6.2500 ns/op
OverheadWarmup   6: 128 op, 800.00 ns, 6.2500 ns/op
OverheadWarmup   7: 128 op, 700.00 ns, 5.4688 ns/op

OverheadActual   1: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadActual   2: 128 op, 900.00 ns, 7.0313 ns/op
OverheadActual   3: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadActual   4: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadActual   5: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual   6: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual   7: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual   8: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual   9: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual  10: 128 op, 1100.00 ns, 8.5938 ns/op
OverheadActual  11: 128 op, 1000.00 ns, 7.8125 ns/op
OverheadActual  12: 128 op, 700.00 ns, 5.4688 ns/op
OverheadActual  13: 128 op, 700.00 ns, 5.4688 ns/op
OverheadActual  14: 128 op, 600.00 ns, 4.6875 ns/op
OverheadActual  15: 128 op, 500.00 ns, 3.9063 ns/op
OverheadActual  16: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual  17: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual  18: 128 op, 900.00 ns, 7.0313 ns/op
OverheadActual  19: 128 op, 800.00 ns, 6.2500 ns/op
OverheadActual  20: 128 op, 900.00 ns, 7.0313 ns/op

WorkloadWarmup   1: 128 op, 1565320900.00 ns, 12.2291 ms/op
WorkloadWarmup   2: 128 op, 2088497300.00 ns, 16.3164 ms/op
WorkloadWarmup   3: 128 op, 1336280200.00 ns, 10.4397 ms/op
WorkloadWarmup   4: 128 op, 612840800.00 ns, 4.7878 ms/op
WorkloadWarmup   5: 128 op, 601168500.00 ns, 4.6966 ms/op
WorkloadWarmup   6: 128 op, 675354600.00 ns, 5.2762 ms/op
WorkloadWarmup   7: 128 op, 810251100.00 ns, 6.3301 ms/op
WorkloadWarmup   8: 128 op, 1244049400.00 ns, 9.7191 ms/op
WorkloadWarmup   9: 128 op, 1926658500.00 ns, 15.0520 ms/op
WorkloadWarmup  10: 128 op, 1938327400.00 ns, 15.1432 ms/op
WorkloadWarmup  11: 128 op, 1459448000.00 ns, 11.4019 ms/op

// BeforeActualRun
WorkloadActual   1: 128 op, 672167100.00 ns, 5.2513 ms/op
WorkloadActual   2: 128 op, 625676000.00 ns, 4.8881 ms/op
WorkloadActual   3: 128 op, 653823300.00 ns, 5.1080 ms/op
WorkloadActual   4: 128 op, 632378100.00 ns, 4.9405 ms/op
WorkloadActual   5: 128 op, 629189500.00 ns, 4.9155 ms/op
WorkloadActual   6: 128 op, 1169670000.00 ns, 9.1380 ms/op
WorkloadActual   7: 128 op, 1526759800.00 ns, 11.9278 ms/op
WorkloadActual   8: 128 op, 1612344600.00 ns, 12.5964 ms/op
WorkloadActual   9: 128 op, 1052235200.00 ns, 8.2206 ms/op
WorkloadActual  10: 128 op, 658135800.00 ns, 5.1417 ms/op
WorkloadActual  11: 128 op, 626376000.00 ns, 4.8936 ms/op
WorkloadActual  12: 128 op, 643499100.00 ns, 5.0273 ms/op
WorkloadActual  13: 128 op, 628208700.00 ns, 4.9079 ms/op
WorkloadActual  14: 128 op, 977245800.00 ns, 7.6347 ms/op
WorkloadActual  15: 128 op, 1516883900.00 ns, 11.8507 ms/op
WorkloadActual  16: 128 op, 1408404000.00 ns, 11.0032 ms/op
WorkloadActual  17: 128 op, 1389015100.00 ns, 10.8517 ms/op
WorkloadActual  18: 128 op, 616956200.00 ns, 4.8200 ms/op
WorkloadActual  19: 128 op, 685887600.00 ns, 5.3585 ms/op
WorkloadActual  20: 128 op, 729377200.00 ns, 5.6983 ms/op
WorkloadActual  21: 128 op, 628913200.00 ns, 4.9134 ms/op
WorkloadActual  22: 128 op, 610246200.00 ns, 4.7675 ms/op
WorkloadActual  23: 128 op, 1211901000.00 ns, 9.4680 ms/op
WorkloadActual  24: 128 op, 1404923200.00 ns, 10.9760 ms/op
WorkloadActual  25: 128 op, 1403692800.00 ns, 10.9664 ms/op
WorkloadActual  26: 128 op, 1194930600.00 ns, 9.3354 ms/op
WorkloadActual  27: 128 op, 620755600.00 ns, 4.8497 ms/op
WorkloadActual  28: 128 op, 619220300.00 ns, 4.8377 ms/op
WorkloadActual  29: 128 op, 617023100.00 ns, 4.8205 ms/op
WorkloadActual  30: 128 op, 680873700.00 ns, 5.3193 ms/op
WorkloadActual  31: 128 op, 930358800.00 ns, 7.2684 ms/op
WorkloadActual  32: 128 op, 1462626600.00 ns, 11.4268 ms/op
WorkloadActual  33: 128 op, 1438439400.00 ns, 11.2378 ms/op
WorkloadActual  34: 128 op, 1427899700.00 ns, 11.1555 ms/op
WorkloadActual  35: 128 op, 716509700.00 ns, 5.5977 ms/op
WorkloadActual  36: 128 op, 613686100.00 ns, 4.7944 ms/op
WorkloadActual  37: 128 op, 609439200.00 ns, 4.7612 ms/op
WorkloadActual  38: 128 op, 687526400.00 ns, 5.3713 ms/op
WorkloadActual  39: 128 op, 617440500.00 ns, 4.8238 ms/op
WorkloadActual  40: 128 op, 907466000.00 ns, 7.0896 ms/op
WorkloadActual  41: 128 op, 1653144000.00 ns, 12.9152 ms/op
WorkloadActual  42: 128 op, 1522201200.00 ns, 11.8922 ms/op
WorkloadActual  43: 128 op, 1395881300.00 ns, 10.9053 ms/op
WorkloadActual  44: 128 op, 774634600.00 ns, 6.0518 ms/op
WorkloadActual  45: 128 op, 841846000.00 ns, 6.5769 ms/op
WorkloadActual  46: 128 op, 819037200.00 ns, 6.3987 ms/op
WorkloadActual  47: 128 op, 928668900.00 ns, 7.2552 ms/op
WorkloadActual  48: 128 op, 1723450400.00 ns, 13.4645 ms/op
WorkloadActual  49: 128 op, 1597766400.00 ns, 12.4826 ms/op
WorkloadActual  50: 128 op, 1391224700.00 ns, 10.8689 ms/op
WorkloadActual  51: 128 op, 621234300.00 ns, 4.8534 ms/op
WorkloadActual  52: 128 op, 602902800.00 ns, 4.7102 ms/op
WorkloadActual  53: 128 op, 650140600.00 ns, 5.0792 ms/op
WorkloadActual  54: 128 op, 700381300.00 ns, 5.4717 ms/op
WorkloadActual  55: 128 op, 750018500.00 ns, 5.8595 ms/op
WorkloadActual  56: 128 op, 1499448000.00 ns, 11.7144 ms/op
WorkloadActual  57: 128 op, 1575837500.00 ns, 12.3112 ms/op
WorkloadActual  58: 128 op, 1526935400.00 ns, 11.9292 ms/op
WorkloadActual  59: 128 op, 734416300.00 ns, 5.7376 ms/op
WorkloadActual  60: 128 op, 609303500.00 ns, 4.7602 ms/op
WorkloadActual  61: 128 op, 611129400.00 ns, 4.7744 ms/op
WorkloadActual  62: 128 op, 620813400.00 ns, 4.8501 ms/op
WorkloadActual  63: 128 op, 612658000.00 ns, 4.7864 ms/op
WorkloadActual  64: 128 op, 822521200.00 ns, 6.4259 ms/op
WorkloadActual  65: 128 op, 1746764200.00 ns, 13.6466 ms/op
WorkloadActual  66: 128 op, 1445359100.00 ns, 11.2919 ms/op
WorkloadActual  67: 128 op, 1351183500.00 ns, 10.5561 ms/op
WorkloadActual  68: 128 op, 614268900.00 ns, 4.7990 ms/op
WorkloadActual  69: 128 op, 640715900.00 ns, 5.0056 ms/op
WorkloadActual  70: 128 op, 636410800.00 ns, 4.9720 ms/op
WorkloadActual  71: 128 op, 654661600.00 ns, 5.1145 ms/op
WorkloadActual  72: 128 op, 636146500.00 ns, 4.9699 ms/op
WorkloadActual  73: 128 op, 1250643800.00 ns, 9.7707 ms/op
WorkloadActual  74: 128 op, 1506285000.00 ns, 11.7679 ms/op
WorkloadActual  75: 128 op, 1529930100.00 ns, 11.9526 ms/op
WorkloadActual  76: 128 op, 1065854900.00 ns, 8.3270 ms/op
WorkloadActual  77: 128 op, 699345500.00 ns, 5.4636 ms/op
WorkloadActual  78: 128 op, 616404800.00 ns, 4.8157 ms/op
WorkloadActual  79: 128 op, 652727200.00 ns, 5.0994 ms/op
WorkloadActual  80: 128 op, 621626400.00 ns, 4.8565 ms/op
WorkloadActual  81: 128 op, 804077800.00 ns, 6.2819 ms/op
WorkloadActual  82: 128 op, 3220101100.00 ns, 25.1570 ms/op
WorkloadActual  83: 128 op, 1611803200.00 ns, 12.5922 ms/op
WorkloadActual  84: 128 op, 1122126900.00 ns, 8.7666 ms/op
WorkloadActual  85: 128 op, 660330600.00 ns, 5.1588 ms/op
WorkloadActual  86: 128 op, 634268900.00 ns, 4.9552 ms/op
WorkloadActual  87: 128 op, 679831200.00 ns, 5.3112 ms/op
WorkloadActual  88: 128 op, 678032800.00 ns, 5.2971 ms/op
WorkloadActual  89: 128 op, 897123800.00 ns, 7.0088 ms/op
WorkloadActual  90: 128 op, 1506382000.00 ns, 11.7686 ms/op
WorkloadActual  91: 128 op, 1411171500.00 ns, 11.0248 ms/op
WorkloadActual  92: 128 op, 1459227500.00 ns, 11.4002 ms/op
WorkloadActual  93: 128 op, 1306356700.00 ns, 10.2059 ms/op
WorkloadActual  94: 128 op, 642324700.00 ns, 5.0182 ms/op
WorkloadActual  95: 128 op, 659470600.00 ns, 5.1521 ms/op
WorkloadActual  96: 128 op, 616157200.00 ns, 4.8137 ms/op
WorkloadActual  97: 128 op, 618217600.00 ns, 4.8298 ms/op
WorkloadActual  98: 128 op, 631382600.00 ns, 4.9327 ms/op
WorkloadActual  99: 128 op, 1465011100.00 ns, 11.4454 ms/op
WorkloadActual  100: 128 op, 1632681300.00 ns, 12.7553 ms/op

// AfterActualRun
WorkloadResult   1: 128 op, 672166300.00 ns, 5.2513 ms/op
WorkloadResult   2: 128 op, 625675200.00 ns, 4.8881 ms/op
WorkloadResult   3: 128 op, 653822500.00 ns, 5.1080 ms/op
WorkloadResult   4: 128 op, 632377300.00 ns, 4.9404 ms/op
WorkloadResult   5: 128 op, 629188700.00 ns, 4.9155 ms/op
WorkloadResult   6: 128 op, 1169669200.00 ns, 9.1380 ms/op
WorkloadResult   7: 128 op, 1526759000.00 ns, 11.9278 ms/op
WorkloadResult   8: 128 op, 1612343800.00 ns, 12.5964 ms/op
WorkloadResult   9: 128 op, 1052234400.00 ns, 8.2206 ms/op
WorkloadResult  10: 128 op, 658135000.00 ns, 5.1417 ms/op
WorkloadResult  11: 128 op, 626375200.00 ns, 4.8936 ms/op
WorkloadResult  12: 128 op, 643498300.00 ns, 5.0273 ms/op
WorkloadResult  13: 128 op, 628207900.00 ns, 4.9079 ms/op
WorkloadResult  14: 128 op, 977245000.00 ns, 7.6347 ms/op
WorkloadResult  15: 128 op, 1516883100.00 ns, 11.8506 ms/op
WorkloadResult  16: 128 op, 1408403200.00 ns, 11.0032 ms/op
WorkloadResult  17: 128 op, 1389014300.00 ns, 10.8517 ms/op
WorkloadResult  18: 128 op, 616955400.00 ns, 4.8200 ms/op
WorkloadResult  19: 128 op, 685886800.00 ns, 5.3585 ms/op
WorkloadResult  20: 128 op, 729376400.00 ns, 5.6983 ms/op
WorkloadResult  21: 128 op, 628912400.00 ns, 4.9134 ms/op
WorkloadResult  22: 128 op, 610245400.00 ns, 4.7675 ms/op
WorkloadResult  23: 128 op, 1211900200.00 ns, 9.4680 ms/op
WorkloadResult  24: 128 op, 1404922400.00 ns, 10.9760 ms/op
WorkloadResult  25: 128 op, 1403692000.00 ns, 10.9663 ms/op
WorkloadResult  26: 128 op, 1194929800.00 ns, 9.3354 ms/op
WorkloadResult  27: 128 op, 620754800.00 ns, 4.8496 ms/op
WorkloadResult  28: 128 op, 619219500.00 ns, 4.8377 ms/op
WorkloadResult  29: 128 op, 617022300.00 ns, 4.8205 ms/op
WorkloadResult  30: 128 op, 680872900.00 ns, 5.3193 ms/op
WorkloadResult  31: 128 op, 930358000.00 ns, 7.2684 ms/op
WorkloadResult  32: 128 op, 1462625800.00 ns, 11.4268 ms/op
WorkloadResult  33: 128 op, 1438438600.00 ns, 11.2378 ms/op
WorkloadResult  34: 128 op, 1427898900.00 ns, 11.1555 ms/op
WorkloadResult  35: 128 op, 716508900.00 ns, 5.5977 ms/op
WorkloadResult  36: 128 op, 613685300.00 ns, 4.7944 ms/op
WorkloadResult  37: 128 op, 609438400.00 ns, 4.7612 ms/op
WorkloadResult  38: 128 op, 687525600.00 ns, 5.3713 ms/op
WorkloadResult  39: 128 op, 617439700.00 ns, 4.8237 ms/op
WorkloadResult  40: 128 op, 907465200.00 ns, 7.0896 ms/op
WorkloadResult  41: 128 op, 1653143200.00 ns, 12.9152 ms/op
WorkloadResult  42: 128 op, 1522200400.00 ns, 11.8922 ms/op
WorkloadResult  43: 128 op, 1395880500.00 ns, 10.9053 ms/op
WorkloadResult  44: 128 op, 774633800.00 ns, 6.0518 ms/op
WorkloadResult  45: 128 op, 841845200.00 ns, 6.5769 ms/op
WorkloadResult  46: 128 op, 819036400.00 ns, 6.3987 ms/op
WorkloadResult  47: 128 op, 928668100.00 ns, 7.2552 ms/op
WorkloadResult  48: 128 op, 1723449600.00 ns, 13.4645 ms/op
WorkloadResult  49: 128 op, 1597765600.00 ns, 12.4825 ms/op
WorkloadResult  50: 128 op, 1391223900.00 ns, 10.8689 ms/op
WorkloadResult  51: 128 op, 621233500.00 ns, 4.8534 ms/op
WorkloadResult  52: 128 op, 602902000.00 ns, 4.7102 ms/op
WorkloadResult  53: 128 op, 650139800.00 ns, 5.0792 ms/op
WorkloadResult  54: 128 op, 700380500.00 ns, 5.4717 ms/op
WorkloadResult  55: 128 op, 750017700.00 ns, 5.8595 ms/op
WorkloadResult  56: 128 op, 1499447200.00 ns, 11.7144 ms/op
WorkloadResult  57: 128 op, 1575836700.00 ns, 12.3112 ms/op
WorkloadResult  58: 128 op, 1526934600.00 ns, 11.9292 ms/op
WorkloadResult  59: 128 op, 734415500.00 ns, 5.7376 ms/op
WorkloadResult  60: 128 op, 609302700.00 ns, 4.7602 ms/op
WorkloadResult  61: 128 op, 611128600.00 ns, 4.7744 ms/op
WorkloadResult  62: 128 op, 620812600.00 ns, 4.8501 ms/op
WorkloadResult  63: 128 op, 612657200.00 ns, 4.7864 ms/op
WorkloadResult  64: 128 op, 822520400.00 ns, 6.4259 ms/op
WorkloadResult  65: 128 op, 1746763400.00 ns, 13.6466 ms/op
WorkloadResult  66: 128 op, 1445358300.00 ns, 11.2919 ms/op
WorkloadResult  67: 128 op, 1351182700.00 ns, 10.5561 ms/op
WorkloadResult  68: 128 op, 614268100.00 ns, 4.7990 ms/op
WorkloadResult  69: 128 op, 640715100.00 ns, 5.0056 ms/op
WorkloadResult  70: 128 op, 636410000.00 ns, 4.9720 ms/op
WorkloadResult  71: 128 op, 654660800.00 ns, 5.1145 ms/op
WorkloadResult  72: 128 op, 636145700.00 ns, 4.9699 ms/op
WorkloadResult  73: 128 op, 1250643000.00 ns, 9.7706 ms/op
WorkloadResult  74: 128 op, 1506284200.00 ns, 11.7678 ms/op
WorkloadResult  75: 128 op, 1529929300.00 ns, 11.9526 ms/op
WorkloadResult  76: 128 op, 1065854100.00 ns, 8.3270 ms/op
WorkloadResult  77: 128 op, 699344700.00 ns, 5.4636 ms/op
WorkloadResult  78: 128 op, 616404000.00 ns, 4.8157 ms/op
WorkloadResult  79: 128 op, 652726400.00 ns, 5.0994 ms/op
WorkloadResult  80: 128 op, 621625600.00 ns, 4.8565 ms/op
WorkloadResult  81: 128 op, 804077000.00 ns, 6.2819 ms/op
WorkloadResult  82: 128 op, 1611802400.00 ns, 12.5922 ms/op
WorkloadResult  83: 128 op, 1122126100.00 ns, 8.7666 ms/op
WorkloadResult  84: 128 op, 660329800.00 ns, 5.1588 ms/op
WorkloadResult  85: 128 op, 634268100.00 ns, 4.9552 ms/op
WorkloadResult  86: 128 op, 679830400.00 ns, 5.3112 ms/op
WorkloadResult  87: 128 op, 678032000.00 ns, 5.2971 ms/op
WorkloadResult  88: 128 op, 897123000.00 ns, 7.0088 ms/op
WorkloadResult  89: 128 op, 1506381200.00 ns, 11.7686 ms/op
WorkloadResult  90: 128 op, 1411170700.00 ns, 11.0248 ms/op
WorkloadResult  91: 128 op, 1459226700.00 ns, 11.4002 ms/op
WorkloadResult  92: 128 op, 1306355900.00 ns, 10.2059 ms/op
WorkloadResult  93: 128 op, 642323900.00 ns, 5.0182 ms/op
WorkloadResult  94: 128 op, 659469800.00 ns, 5.1521 ms/op
WorkloadResult  95: 128 op, 616156400.00 ns, 4.8137 ms/op
WorkloadResult  96: 128 op, 618216800.00 ns, 4.8298 ms/op
WorkloadResult  97: 128 op, 631381800.00 ns, 4.9327 ms/op
WorkloadResult  98: 128 op, 1465010300.00 ns, 11.4454 ms/op
WorkloadResult  99: 128 op, 1632680500.00 ns, 12.7553 ms/op

// AfterAll
// Benchmark Process 12660 has exited with code 0.

Mean = 7.587 ms, StdErr = 0.305 ms (4.03%), N = 99, StdDev = 3.039 ms
Min = 4.710 ms, Q1 = 4.937 ms, Median = 5.738 ms, Q3 = 10.971 ms, Max = 13.647 ms
IQR = 6.035 ms, LowerFence = -4.115 ms, UpperFence = 20.023 ms
ConfidenceInterval = [6.551 ms; 8.624 ms] (CI 99.9%), Margin = 1.036 ms (13.66% of Mean)
Skewness = 0.57, Kurtosis = 1.61, MValue = 2.89

// ***** BenchmarkRunner: Finish  *****

// * Export *
  BenchmarkDotNet.Artifacts\results\CSScratchpad.Script.WriteText-report.csv
  BenchmarkDotNet.Artifacts\results\CSScratchpad.Script.WriteText-report-github.md
  BenchmarkDotNet.Artifacts\results\CSScratchpad.Script.WriteText-report.html

// * Detailed results *
WriteText.RunTextWriter: .NET Framework 4.6.1(Runtime=.NET Framework 4.6.1)
Runtime = .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT; GC = Concurrent Workstation
Mean = 4.340 ms, StdErr = 0.143 ms (3.29%), N = 100, StdDev = 1.428 ms
Min = 3.046 ms, Q1 = 3.179 ms, Median = 3.449 ms, Q3 = 5.701 ms, Max = 6.779 ms
IQR = 2.522 ms, LowerFence = -0.604 ms, UpperFence = 9.484 ms
ConfidenceInterval = [3.856 ms; 4.825 ms] (CI 99.9%), Margin = 0.484 ms (11.15% of Mean)
Skewness = 0.7, Kurtosis = 1.72, MValue = 2.83
-------------------- Histogram --------------------
[2.931 ms ; 3.974 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[3.974 ms ; 4.781 ms) | @@@@@@@@@@
[4.781 ms ; 5.204 ms) | @
[5.204 ms ; 6.066 ms) | @@@@@@@
[6.066 ms ; 6.873 ms) | @@@@@@@@@@@@@@@@@@@@@@@@
---------------------------------------------------

WriteText.RunTextWriter: .NET Framework 4.7.2(Runtime=.NET Framework 4.7.2)
Runtime = .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT; GC = Concurrent Workstation
Mean = 4.572 ms, StdErr = 0.145 ms (3.17%), N = 100, StdDev = 1.447 ms
Min = 3.081 ms, Q1 = 3.242 ms, Median = 4.161 ms, Q3 = 6.299 ms, Max = 7.297 ms
IQR = 3.057 ms, LowerFence = -1.343 ms, UpperFence = 10.884 ms
ConfidenceInterval = [4.081 ms; 5.063 ms] (CI 99.9%), Margin = 0.491 ms (10.74% of Mean)
Skewness = 0.44, Kurtosis = 1.52, MValue = 3.26
-------------------- Histogram --------------------
[2.952 ms ; 3.771 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[3.771 ms ; 4.368 ms) | @@@@@@
[4.368 ms ; 5.187 ms) | @@@@@@@@@@@@@@@
[5.187 ms ; 6.100 ms) | @@@@@@
[6.100 ms ; 6.918 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@
[6.918 ms ; 7.707 ms) | @
---------------------------------------------------

WriteText.RunFile: .NET Framework 4.6.1(Runtime=.NET Framework 4.6.1)
Runtime = .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT; GC = Concurrent Workstation
Mean = 7.074 ms, StdErr = 0.309 ms (4.37%), N = 99, StdDev = 3.078 ms
Min = 4.504 ms, Q1 = 4.813 ms, Median = 5.162 ms, Q3 = 10.558 ms, Max = 13.974 ms
IQR = 5.746 ms, LowerFence = -3.806 ms, UpperFence = 19.177 ms
ConfidenceInterval = [6.024 ms; 8.124 ms] (CI 99.9%), Margin = 1.050 ms (14.84% of Mean)
Skewness = 0.88, Kurtosis = 2.03, MValue = 2.74
-------------------- Histogram --------------------
[ 4.371 ms ;  6.118 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[ 6.118 ms ;  6.777 ms) | 
[ 6.777 ms ;  8.524 ms) | @@@@
[ 8.524 ms ; 10.055 ms) | @
[10.055 ms ; 12.044 ms) | @@@@@@@@@@@@@@@@@@@
[12.044 ms ; 13.791 ms) | @@@@@@@@@
[13.791 ms ; 14.847 ms) | @
---------------------------------------------------

WriteText.RunFile: .NET Framework 4.7.2(Runtime=.NET Framework 4.7.2)
Runtime = .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT; GC = Concurrent Workstation
Mean = 7.587 ms, StdErr = 0.305 ms (4.03%), N = 99, StdDev = 3.039 ms
Min = 4.710 ms, Q1 = 4.937 ms, Median = 5.738 ms, Q3 = 10.971 ms, Max = 13.647 ms
IQR = 6.035 ms, LowerFence = -4.115 ms, UpperFence = 20.023 ms
ConfidenceInterval = [6.551 ms; 8.624 ms] (CI 99.9%), Margin = 1.036 ms (13.66% of Mean)
Skewness = 0.57, Kurtosis = 1.61, MValue = 2.89
-------------------- Histogram --------------------
[ 4.706 ms ;  6.430 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[ 6.430 ms ;  8.530 ms) | @@@@@@@@
[ 8.530 ms ; 10.805 ms) | @@@@@@@
[10.805 ms ; 12.529 ms) | @@@@@@@@@@@@@@@@@@@@@@@
[12.529 ms ; 14.509 ms) | @@@@@@
---------------------------------------------------

// * Summary *

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1165 (21H1/May2021Update)
Intel Core i7-7500U CPU 2.70GHz (Kaby Lake), 1 CPU, 4 logical and 2 physical cores
  [Host]               : .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT  [AttachedDebugger]
  .NET Framework 4.6.1 : .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
  .NET Framework 4.7.2 : .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT


|        Method |                  Job |              Runtime |     Mean |     Error |   StdDev |   Median | Ratio | RatioSD |
|-------------- |--------------------- |--------------------- |---------:|----------:|---------:|---------:|------:|--------:|
| RunTextWriter | .NET Framework 4.6.1 | .NET Framework 4.6.1 | 4.340 ms | 0.4841 ms | 1.428 ms | 3.449 ms |  1.00 |    0.00 |
| RunTextWriter | .NET Framework 4.7.2 | .NET Framework 4.7.2 | 4.572 ms | 0.4909 ms | 1.447 ms | 4.161 ms |  1.15 |    0.48 |
|               |                      |                      |          |           |          |          |       |         |
|       RunFile | .NET Framework 4.6.1 | .NET Framework 4.6.1 | 7.074 ms | 1.0497 ms | 3.078 ms | 5.162 ms |  1.00 |    0.00 |
|       RunFile | .NET Framework 4.7.2 | .NET Framework 4.7.2 | 7.587 ms | 1.0363 ms | 3.039 ms | 5.738 ms |  1.29 |    0.72 |

// * Warnings *
Environment
  Summary -> Benchmark was executed with attached debugger
MultimodalDistribution
  WriteText.RunTextWriter: .NET Framework 4.6.1 -> It seems that the distribution can have several modes (mValue = 2.83)
  WriteText.RunTextWriter: .NET Framework 4.7.2 -> It seems that the distribution is bimodal (mValue = 3.26)
  WriteText.RunFile: .NET Framework 4.7.2       -> It seems that the distribution can have several modes (mValue = 2.89)

// * Hints *
Outliers
  WriteText.RunFile: .NET Framework 4.6.1 -> 1 outlier  was  removed (37.80 ms)
  WriteText.RunFile: .NET Framework 4.7.2 -> 1 outlier  was  removed (25.16 ms)

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
Run time: 00:08:06 (486.6 sec), executed benchmarks: 4

Global total time: 00:08:10 (490.19 sec), executed benchmarks: 4
// * Artifacts cleanup *

*/
#endregion
