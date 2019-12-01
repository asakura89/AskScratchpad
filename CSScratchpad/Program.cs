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
                new PrintAssemblyVersion(),
                new PrintCharCode(),
                new PrintFromFile(),
                new PrintLookup(),
                new PrintPatchedJson(),
                new PrintRandomizedList(),
                new PrintSimpleHashPassword(),
                new PrintTodoList(),
                new PrintUri(),
                new ReadAppConfigSection(),
                new ShowCacheControl(),
                new SimpleStickyNotesDatetimeFromFloat(),
                new TestAllAboutArray(),
                new TestArvy(),
                new TestDatetimeUtil(),
                new TestExy(),
                new TestMaaya(),
                new TestNvy(),
                new TestPOCOBuilder(),
                new TestSecurity(),
                new TestVarya(),
                new ZipStructureViewer()
            }
            .Last()
            .Run();

            Console.ReadLine();
        }
    }
}
