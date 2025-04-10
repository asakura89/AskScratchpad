
using System;
using System.Collections.Generic;
using System.Linq;
using CSScratchpad.Script;
using Scratch;

namespace CSScratchpad {
    public class Program {
        public static void Main(String[] args) {
            new List<IRunnable> {
                new AnalyzeHex(),
                new ApplicationPipeline(),
                new ArrayMethods(),
                new BartSimpson(),
                new Base64UrlAndJwtBase64(),
                new ChangeAspMembershipPassword(),
                new CheckPath(),
                new CleanUrl(),
                new CloneList(),
                new CombineFiles(),
                new CombineUrl(),
                // new CombineXml(), // [NEED TO BE FIXED]
                new CompareAsAndIs(),
                new CompareSingletonAndTransient(),
                new CompareString(),
                new ConvertToPascalCase(),
                new CountCharacter(),
                new CountDecimalPlaces(),
                new DisectCakeBuild(),

                new DisplayingList(),
                new EmitEvents(),
                new FindDuplicates(),
                new FindFiles(),
                new FormatJson(),
                new FormatJson2(),
                new FormatJson3(),
                new FormatJson4(),
                new GenerateId(),
                new GenerateMD5Base64(),
                new GenerateRandomizedList(),
                new GenerateTenGuid(),
                new GenerateToken(),
                new GetProperties(),
                new HostAConfiguredWebApi(),
                new HostAWcf(),
                new HostAWebApi(),
                new ManipulateString(),
                new Motoko(),
                new PrintAssemblyVersion(),
                new PrintCharCode(),
                new PrintDir(),
                new PrintDynamic(),
                new PrintExpressionBody(),
                new PrintFromFile(),
                new PrintHashCode(),
                new PrintLoadedAssemblies(),
                new PrintLookup(),
                new PrintPatchedJson(),
                new PrintQueryBuilder(),
                new PrintRandomizedList(),
                new PrintSimpleHashPassword(),
                new PrintTable(),
                new PrintTodoList(),
                new PrintTree(),
                new PrintTypeFullName(),
                new PrintUri(),
                new ReadAppConfigSection(),
                new ReplicateAspMvcDownload(),
                // new RunCmd(), // [NEED TO BE FIXED]
                new SelfHostAspWebApi(),
                new SelfHostTheWcf(),
                new ShowCacheControl(),
                new SimpleStickyNotesDatetimeFromFloat(),
                new TestAdvancedBuilderPattern(),
                new TestAllAboutArray(),
                new TestAnonymous(),
                new TestArvy(),
                new TestBitwise(),
                new TestConfigy(),
                new TestDatetimeUtil(),
                new TestDecoratorPattern(),
                new TestExy(),
                new TestMaaya(),
                new TestNvy(),
                new TestPOCOBuilder(),
                new TestQueryCrafter(),
                new TestSecurity(),
                new TestShiro(),
                new TestTryParse(),
                new TestVarya(),
                new TestXml(),
                new WriteBinary(),
                new WriteLicense(),
                new ZipStructureViewer()
            }
            .Last()
            .Run();

            Console.ReadLine();
        }
    }
}
