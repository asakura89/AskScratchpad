using System;
using System.Collections.Generic;
using System.Linq;
using CSScratchpad.Script;

namespace CSScratchpad {
    public class Program {
        public static void Main(String[] args) {
            new List<IRunnable> {
                new AnalyzeHex(),
                new BartSimpson(),
                new Base64UrlAndJwtBase64(),
                new ChangeAspMembershipPassword(),
                new CloneList(),
                new CombineFiles(),
                new CompareAsAndIs(),
                new CompareSingletonAndTransient(),
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
                new GenerateTenGuid(),
                new GenerateToken(),
                new GetProperties(),
                new PrintAssemblyVersion(),
                new PrintSimpleHashPassword(),
                new PrintTodoList(),
                new PrintUri(),
                new ReadAppConfigSection(),
                new ShowCacheControl(),
                new SimpleStickyNotesDatetimeFromFloat(),
                new TestArvy(),
                new TestDatetimeUtil(),
                new TestExy(),
                new TestVarya()
            }
            .Last()
            .Run();

            Console.ReadLine();
        }
    }
}
