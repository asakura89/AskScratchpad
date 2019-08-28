using System;

namespace CSScratchpad.Script {
    internal class CompareAsAndIs : Common, IRunnable {
        public void Run() {
            A a = new A();
            B b = new B();
            C c = new C();

            Object objA = new A();
            Object objB = new B();
            Object nullObj = null;

            var objA2 = new Object();
            objA2 = new A();

            Dbg("a is IAuditTrail", (a is IAuditTrail));
            Dbg("b is IAuditTrail", (b is IAuditTrail));
            Dbg("c is IAuditTrail", (c is IAuditTrail));

            Dbg("a as IAuditTrail == null", (a as IAuditTrail == null));
            Dbg("b as IAuditTrail == null", (b as IAuditTrail == null));
            Dbg("c as IAuditTrail == null", (c as IAuditTrail == null));

            Dbg("objA is A", (objA is A));
            Dbg("objB is B", (objB is B));
            Dbg("objA2 is A", (objA2 is A));
            Dbg("nullObj as A", (nullObj as A));
        }
    }

    public class C : A { }

    public class B { }

    public class A : IAuditTrail {
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public String CreatedBy { get; set; }
        public String UpdatedBy { get; set; }
        public String LastModule { get; set; }
        public String LastOperation { get; set; }
        public Boolean IsDeleted { get; set; }
        public Boolean IsActive { get; set; }
    }

    public interface IAuditTrail {
        DateTime CreatedOn { get; set; }
        DateTime UpdatedOn { get; set; }
        String CreatedBy { get; set; }
        String UpdatedBy { get; set; }
        String LastModule { get; set; }
        String LastOperation { get; set; }
        Boolean IsDeleted { get; set; }
        Boolean IsActive { get; set; }
    }
}
