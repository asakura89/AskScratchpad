using System;
using System.Linq.Expressions;
using Scratch;

namespace CSScratchpad.Script {
    public class PrintExpressionBody : Common, IRunnable {
        readonly String[] exprTypes = new[] {
            "0: Add", "1: AddChecked", "2: And", "3: AndAlso", "4: ArrayLength", "5: ArrayIndex", "6: Call", "7: Coalesce", "8: Conditional", "9: Constant",
            "10: Convert", "11: ConvertChecked", "12: Divide", "13: Equal", "14: ExclusiveOr", "15: GreaterThan", "16: GreaterThanOrEqual", "17: Invoke", "18: Lambda", "19: LeftShift",
            "20: LessThan", "21: LessThanOrEqual", "22: ListInit", "23: MemberAccess", "24: MemberInit", "25: Modulo", "26: Multiply", "27: MultiplyChecked", "28: Negate", "29: UnaryPlus",
            "30: NegateChecked", "31: New", "32: NewArrayInit", "33: NewArrayBounds", "34: Not", "35: NotEqual", "36: Or", "37: OrElse", "38: Parameter", "39: Power",
            "40: Quote", "41: RightShift", "42: Subtract", "43: SubtractChecked", "44: TypeAs", "45: TypeIs", "46: Assign", "57: Block", "48: DebugInfo", "49: Decrement",
            "50: Dynamic", "51: Default", "52: Extension", "53: Goto", "54: Increment", "55: Index", "56: Label", "57: RuntimeVariables", "58: Loop", "59: Switch",
            "60: Throw", "61: Try", "62: Unbox", "63: AddAssign", "64: AndAssign", "65: DivideAssign", "66: ExclusiveOrAssign", "67: LeftShiftAssign", "68: ModuloAssign", "69: MultiplyAssign",
            "70: OrAssign", "71: PowerAssign", "72: RightShiftAssign", "73: SubtractAssign", "74: AddAssignChecked", "75: MultiplyAssignChecked", "76: SubtractAssignChecked", "77: PreIncrementAssign", "78: PreDecrementAssign", "79: PostIncrementAssign",
            "80: PostDecrementAssign", "81: TypeEqual", "82: OnesComplement", "83: IsTrue", "84: IsFalse"
        };

        public void Run() {
            Dbg("ExprBody");
            AnalyzeExprBody<User>(us => us.Name, us => us.Email);
            AnalyzeExprBody<User>(us => us.Name != String.Empty, us => String.IsNullOrEmpty(us.Email));
            AnalyzeExprBody<User>(us => Process(us), us => Process2(us));
            AnalyzeExprBody<User>(us => Process(us), us => Process3(us));
        }

        Object Process(User us) {
            us.Name = "A";
            return us;
        }

        Boolean Process2(User us) => String.IsNullOrEmpty(us.Name);

        Boolean Process3(User us) => !String.IsNullOrEmpty(us.Name);

        void AnalyzeExprBody<TData>(params Expression<Func<TData, Object>>[] exprs) where TData : class {
            foreach (Expression<Func<TData, Object>> expr in exprs)
                Dbg(exprTypes[(UInt32) expr.Body.NodeType]);
        }

        class User {
            public String Id { get; set; }
            public String Name { get; set; }
            public String Email { get; set; }
        }
    }
}
