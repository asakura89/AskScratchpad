
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
                new FindDuplicates(),
                new FindFiles(),
                new FormatJson(),
                new FormatJson2(),
                new FormatJson3(),
                new GenerateId(),
                new GenerateMD5Base64(),
                new GenerateRandomizedList(),
                new GenerateTenGuid(),
                new GenerateToken(),
                new GetProperties(),
                new HostAConfiguredWebApi(),
                new HostAWebApi(),
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
                new PrintUri(),
                new ReadAppConfigSection(),
                // new RunCmd(), // [NEED TO BE FIXED]
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
                new TestSecurity(),
                new TestShiro(),
                new TestVarya(),
                new TestXml(),
                new ZipStructureViewer()
            }
            .Last()
            .Run();

            Console.ReadLine();
        }
    }
}
