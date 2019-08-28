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
                new FormatJson(),
                new FormatJson2(),
                new CompareSingletonAndTransient(),
                new CountDecimalPlaces(),
                new CombineFiles(),
                new DisectCakeBuild()
            }
            .Last()
            .Run();

            Console.ReadLine();
        }
    }
}
