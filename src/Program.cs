using System;
using System.Collections.Generic;
using System.Linq;
using CSScratchpad.Script;

namespace CSScratchpad {
    public class Program {
        public static void Main(String[] args) {

            new List<IRunnable> {
                new GenerateTenGuid(),
                new PrintTodoList(),
                new ConvertToPascalCase(),
                new PrintUri(),
                new TestArvy(),
                new ShowCacheControl(),
                new TestVarya(),
                new TestExy(),
                new GenerateId(),
                new GenerateToken(),
                new FindFiles(),
                new FindDuplicates(),
                new CloneList(),
                new CountCharacter(),
                new TestDatetimeUtil(),
                new SimpleStickyNotesDatetimeFromFloat(),
                new GetProperties(),
                new FormatJson(),
                new FormatJson2(),
                new CompareSingletonAndTransient(),
                new CountDecimalPlaces(),
                new CombineFiles(),
                new ReadAppConfigSection(),
                new ChangeAspMembershipPassword(),
                new DisectCakeBuild(),
                new CompareAsAndIs(),
                new PrintAssemblyVersion(),
                new Base64UrlAndJwtBase64(),
                new BartSimpson(),
                new PrintSimpleHashPassword(),
                new AnalyzeHex()
            }
            .Last()
            .Run();

            Console.ReadLine();
        }
    }
}
