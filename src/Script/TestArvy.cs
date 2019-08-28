using System;
using System.Collections.Generic;
using System.Linq;

namespace CSScratchpad.Script {
    class TestArvy : Common, IRunnable {
        public void Run() => Dbg(
            GetLogs()
                .Select(str => str.AsActionResponseViewModel(true))
                .Select(arvy => $"ResponseType: {arvy.ResponseType}, Message: {arvy.Message}")
        );

        public IEnumerable<String> GetLogs() {
            yield return "I|Start";
            yield return "I|Get ProcessId ::begin::";
            yield return "I|Get ProcessId ::end::";
            yield return "I|Update unfinish process Log status ::begin::";
            yield return "I|Update unfinish process Log status ::end::";
            yield return "I|Delete data from temp_Item where CreatedBy: user_1 ::begin::";
            yield return "I|Delete data from temp_Item where CreatedBy: user_1 ::end::";
            yield return "I|Delete data from temp_SubItem where CreatedBy: user_1 ::begin::";
            yield return "I|Delete data from temp_SubItem where CreatedBy: user_1 ::end::";
            yield return "I|Delete data from temp_Condition where CreatedBy: user_1 ::begin::";
            yield return "I|Delete data from temp_Condition where CreatedBy: user_1 ::end::";
            yield return "S|Finish: It's an Add process";
            yield return "I|Start";
            yield return "I|Get currency rate ::begin::";
            yield return "I|Get currency rate ::end::";
            yield return "I|Select from physic_Item then Insert to temp_Item where HeaderNo: 5000000107 and ItemNo: 1 ::begin::";
            yield return "I|Select from physic_Item then Insert to temp_Item where HeaderNo: 5000000107 and ItemNo: 1 ::end::";
            yield return "I|Select from physic_SubItem then Insert to temp_SubItem where HeaderNo: , SequenceNo: 1, HeaderNo: 5000000107 and ItemNo: 1 ::begin::";
            yield return "I|Select from physic_SubItem then Insert to temp_SubItem where HeaderNo: , SequenceNo: 1, HeaderNo: 5000000107 and ItemNo: 1 ::end::";
            yield return "I|Calculating temp_Condition price amount where ActionId: 201601140003 and HeaderNo:  and SequenceNo: 1 ::begin::";
            yield return "I|Clean calculation temp data ::begin::";
            yield return "I|Clean calculation temp data ::end::";
            yield return "I|Insert data to temp_Condition where ActionId: 201601140003 and HeaderNo:  and SequenceNo: 1 and PriceCode: TAX_1 ::begin::";
            yield return "I|Insert data to temp_Condition where ActionId: 201601140003 and HeaderNo:  and SequenceNo: 1 and PriceCode: TAX_1 ::end::";
            yield return "I|Insert data to temp_Condition where ActionId: 201601140003 and HeaderNo:  and SequenceNo: 1 and PriceCode: TAX_25 ::begin::";
            yield return "I|Insert data to temp_Condition where ActionId: 201601140003 and HeaderNo:  and SequenceNo: 1 and PriceCode: TAX_25 ::end::";
            yield return "I|Insert data to temp_Condition where ActionId: 201601140003 and HeaderNo:  and SequenceNo: 1 and PriceCode: TAX_2 ::begin::";
            yield return "I|Insert data to temp_Condition where ActionId: 201601140003 and HeaderNo:  and SequenceNo: 1 and PriceCode: TAX_2 ::end::";
            yield return "I|Insert data to temp_Condition where ActionId: 201601140003 and HeaderNo:  and SequenceNo: 1 and PriceCode: TAX_3 ::begin::";
            yield return "I|Insert data to temp_Condition where ActionId: 201601140003 and HeaderNo:  and SequenceNo: 1 and PriceCode: TAX_3 ::end::";
            yield return "I|Calculating temp_Condition price amount where ActionId: 201601140003 and HeaderNo:  and SequenceNo: 1 ::end::";
            yield return "I|Get currency rate ::begin::";
            yield return "I|Get currency rate ::end::";
            yield return "I|Select from physic_Item then Insert to temp_Item where HeaderNo: 5000000142 and ItemNo: 1 ::begin::";
            yield return "I|Select from physic_Item then Insert to temp_Item where HeaderNo: 5000000142 and ItemNo: 1 ::end::";
            yield return "I|Select from physic_SubItem then Insert to temp_SubItem where HeaderNo: , SequenceNo: 2, HeaderNo: 5000000142 and ItemNo: 1 ::begin::";
            yield return "I|Select from physic_SubItem then Insert to temp_SubItem where HeaderNo: , SequenceNo: 2, HeaderNo: 5000000142 and ItemNo: 1 ::end::";
            yield return "I|Calculating temp_Condition price amount where ActionId: 201601140003 and HeaderNo:  and SequenceNo: 2 ::begin::";
            yield return "I|Clean calculation temp data ::begin::";
            yield return "I|Clean calculation temp data ::end::";
            yield return "I|Insert data to temp_Condition where ActionId: 201601140003 and HeaderNo:  and SequenceNo: 2 and PriceCode: TAX_1 ::begin::";
            yield return "I|Insert data to temp_Condition where ActionId: 201601140003 and HeaderNo:  and SequenceNo: 2 and PriceCode: TAX_1 ::end::";
            yield return "I|Insert data to temp_Condition where ActionId: 201601140003 and HeaderNo:  and SequenceNo: 2 and PriceCode: TAX_25 ::begin::";
            yield return "I|Insert data to temp_Condition where ActionId: 201601140003 and HeaderNo:  and SequenceNo: 2 and PriceCode: TAX_25 ::end::";
            yield return "I|Insert data to temp_Condition where ActionId: 201601140003 and HeaderNo:  and SequenceNo: 2 and PriceCode: TAX_2 ::begin::";
            yield return "I|Insert data to temp_Condition where ActionId: 201601140003 and HeaderNo:  and SequenceNo: 2 and PriceCode: TAX_2 ::end::";
            yield return "I|Insert data to temp_Condition where ActionId: 201601140003 and HeaderNo:  and SequenceNo: 2 and PriceCode: TAX_3 ::begin::";
            yield return "I|Insert data to temp_Condition where ActionId: 201601140003 and HeaderNo:  and SequenceNo: 2 and PriceCode: TAX_3 ::end::";
            yield return "I|Calculating temp_Condition price amount where ActionId: 201601140003 and HeaderNo:  and SequenceNo: 2 ::end::";
            yield return "S|Finish";
            yield return "I|Start";
            yield return "I|Get currency rate ::begin::";
            yield return "I|Get currency rate ::end::";
            yield return "E|37 - Conversion failed when converting the varchar value 'I|Update data in temp_Item where HeaderNo: NULL and ItemNo: ' to data type int.";
        }
    }

    #region : Arvy :

    [Serializable]
    public class ActionResponseViewModel {
        public const String Info = "I";
        public const String Warning = "W";
        public const String Error = "E";
        public const String Success = "S";
        public const String Tab = "{tab}";
        public const String NewLine = "{newline}";
        public String ResponseType { get; set; }
        public String Message { get; set; }

        public override String ToString() => ToString(true);

        public String ToString(Boolean alwaysReturn) {
            if (!alwaysReturn && ResponseType == Error)
                throw new InvalidOperationException(Message);

            return ResponseType + "|" + Message;
        }
    }

    #endregion

    #region : ActionResponseExt :

    public static class ActionResponseExt {
        public static ActionResponseViewModel AsActionResponseViewModel(this String resultString, Boolean alwaysReturn = false) {
            String[] splittedResult = new[] { resultString.Substring(0, 1), resultString.Substring(2, resultString.Length - 2) };
            String[] responseTypeList = new[] { ActionResponseViewModel.Info, ActionResponseViewModel.Warning, ActionResponseViewModel.Error, ActionResponseViewModel.Success };
            if (!responseTypeList.Contains(splittedResult[0]))
                throw new ArgumentException("resultString is bad formatted.");

            var viewModel = new ActionResponseViewModel {
                ResponseType = splittedResult[0],
                Message = splittedResult[1]
                    .Replace(ActionResponseViewModel.Tab, "\t")
                    .Replace(ActionResponseViewModel.NewLine, Environment.NewLine)
            };

            if (!alwaysReturn && viewModel.ResponseType == ActionResponseViewModel.Error)
                throw new InvalidOperationException(viewModel.Message);

            return viewModel;
        }

        public static ActionResponseViewModel AsActionResponseViewModel(this Exception ex) {
            var viewModel = new ActionResponseViewModel {
                ResponseType = ActionResponseViewModel.Error,
                Message = ex.Message
            };

            return viewModel;
        }
    }

    #endregion

}
