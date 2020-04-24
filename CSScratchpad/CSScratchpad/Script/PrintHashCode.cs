using System;
using System.Collections.Generic;
using System.Linq;
using Scratch;

namespace CSScratchpad.Script {
    internal class PrintHashCode : Common, IRunnable {
        public void Run() {
            Dbg("Start", "-");
            IList<(String Name, Int32 HashCode)> hashCodes = new[] {
                (Name: nameof(MallardDuck), HashCode: new MallardDuck().GetHashCode()),
                (Name: nameof(TaskModel), HashCode: new TaskModel().GetHashCode()),
                (Name: nameof(WildTurkey), HashCode: new WildTurkey().GetHashCode()),
                (Name: nameof(A), HashCode: new A().GetHashCode()),
                (Name: nameof(B), HashCode: new B().GetHashCode()),
                (Name: nameof(C), HashCode: new C().GetHashCode()),
                (Name: nameof(DdfConfig), HashCode: new DdfConfig().GetHashCode()),
                (Name: nameof(MustCheckStructure), HashCode: new MustCheckStructure().GetHashCode()),
                (Name: nameof(MustCheckFile), HashCode: new MustCheckFile().GetHashCode()),
                (Name: nameof(Student), HashCode: new Student().GetHashCode()),
                (Name: nameof(User), HashCode: new User().GetHashCode()),
                (Name: nameof(TableStyle), HashCode: new TableStyle().GetHashCode())
            }
            .ToList();

            Dbg("List", hashCodes);
            Dbg("Sorted", hashCodes
                .Select(item => new { item.HashCode, item.HashCode.ToString().Length })
                .OrderBy(item => item.HashCode)
                .ToList());
            Dbg("Done", "-");
        }

        #region : class :

        class MallardDuck {
            public void Fly() => Console.WriteLine($"{nameof(MallardDuck)} Flying.");

            public void Quack() => Console.WriteLine($"{nameof(MallardDuck)} Quacking.");
        }

        class TaskModel {
            public String Id;
            public String Title;
            public Boolean Done;
        }

        class WildTurkey {
            public void Fly() => Console.WriteLine($"{nameof(WildTurkey)} Flying.");

            public void Gobble() => Console.WriteLine($"{nameof(WildTurkey)} Gobble Gobble.");
        }

        class C : A { }

        class B { }

        class A {
            public DateTime CreatedOn { get; set; }
            public DateTime UpdatedOn { get; set; }
            public String CreatedBy { get; set; }
            public String UpdatedBy { get; set; }
            public String LastModule { get; set; }
            public String LastOperation { get; set; }
            public Boolean IsDeleted { get; set; }
            public Boolean IsActive { get; set; }
        }

        sealed class DdfConfig {
            public IList<MustCheckStructure> MustIncludes { get; set; }
            public IList<MustCheckStructure> MustExcludes { get; set; }
            public IList<MustCheckFile> MustContain { get; set; }
            public IList<MustCheckFile> MustNotContain { get; set; }
        }

        sealed class MustCheckStructure {
            public String Desc { get; set; }
            public String Pattern { get; set; }
        }

        sealed class MustCheckFile {
            public String Desc { get; set; }
            public String FilenamePattern { get; set; }
            public String ContentPattern { get; set; }
        }

        class Student {
            public String Id { get; set; }
            public String Name { get; set; }
        }

        class User {
            public String Id { get; set; }
            public String Username { get; set; }
        }

        class TableStyle {
            public String TopLeftCorner { get; set; }
            public String TopMiddleCorner { get; set; }
            public String TopRightCorner { get; set; }
            public String MiddleLeftCorner { get; set; }
            public String CenterCorner { get; set; }
            public String MiddleRightCorner { get; set; }
            public String BottomLeftCorner { get; set; }
            public String BottomMiddleCorner { get; set; }
            public String BottomRightCorner { get; set; }
            public String HeaderLeftVertical { get; set; }
            public String HeaderMiddleVertical { get; set; }
            public String HeaderRightVertical { get; set; }
            public String HeaderTopHorizontal { get; set; }
            public String HeaderBottomHorizontal { get; set; }
            public String BodyLeftVertical { get; set; }
            public String BodyMiddleVertical { get; set; }
            public String BodyRightVertical { get; set; }
            public String BodyMiddleHorizontal { get; set; }
            public String BodyBottomHorizontal { get; set; }
        }

        #endregion
    }
}
