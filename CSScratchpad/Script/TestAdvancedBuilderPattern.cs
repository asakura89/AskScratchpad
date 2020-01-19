using System;
using System.Collections.Generic;
using System.Reflection;
using Scratch;

namespace CSScratchpad.Script {
    class TestAdvancedBuilderPattern : Common, IRunnable {
        public void Run() {
            Console.WriteLine("Start.");
            Dbg(
                Rule.Create()
                    .DispatchWith(new Disp())
                    .ThenWith(new NullDisp())
                    .ExpressWith(new Expr())
                    .OrWith(new Expr())
                    .OrWith(new Expr())
                    .AndWith(new Expr())
                    .Evaluate(new List<BaseType>())
            );
            Console.WriteLine("Done.");

            Console.WriteLine("Start.");

            #region : Current State :

            // Correct use
            IWfEngine wfe = new WfEngine();
            ActionResponseViewModel result1 = wfe
                .WithAuditTrailUsername("Username")
                .WithApp("App")
                .WithDocNo("DocNo")
                .WithExecutor(new ApprovalUser())
                .UseSuperiorsConfigurator((wfm, pic) => new List<ApprovalUser>())
                .WithSegmentPersonInCharges(new List<ApprovalUser>())
                .WithSegmentActuals(new List<SegmentConditionActuals>())
                .WithApprovalLevelActuals(new List<ApprovalLevelConditionActuals>())
                .Start();

            ActionResponseViewModel result2 = wfe
                .WithAuditTrailUsername("Username")
                .WithApp("App")
                .WithDocNo("DocNo")
                .WithExecutor(new ApprovalUser())
                .WithPowerOfAttorneys(new List<PowerOfAttorney>())
                .Reject();

            // Abused
            ActionResponseViewModel result3 = wfe
                .WithAuditTrailUsername("Username")
                .WithApp("App")
                .WithDocNo("DocNo")
                .WithExecutor(new ApprovalUser())
                .UseSuperiorsConfigurator((wfman, pic) => new List<ApprovalUser>())
                .WithSegmentPersonInCharges(new List<ApprovalUser>())
                .WithSegmentActuals(new List<SegmentConditionActuals>())
                .WithApprovalLevelActuals(new List<ApprovalLevelConditionActuals>())
                .UsePowerOfAttorneyConfigurator(wfm => new List<PowerOfAttorney>())
                .Start();

            ActionResponseViewModel result4 = wfe
                .WithAuditTrailUsername("Username")
                .WithApp("App")
                .WithDocNo("DocNo")
                .WithExecutor(new ApprovalUser())
                .UseSuperiorsConfigurator((wfman, pic) => new List<ApprovalUser>())
                .WithSegmentPersonInCharges(new List<ApprovalUser>())
                .WithSegmentActuals(new List<SegmentConditionActuals>())
                .WithApprovalLevelActuals(new List<ApprovalLevelConditionActuals>())
                .UsePowerOfAttorneyConfigurator(wfm => new List<PowerOfAttorney>())
                .WithAdditionalApprovalLevelConditions(new List<WfApprovalLevelConditionViewModel>())
                .WithAdditionalSegmentConditions(new List<WfSegmentConditionViewModel>())
                .Approve();

            #endregion

            #region : New API :

            // Can't be abused
            IApprovalDocument ad = new ApprovalDocument();
            ActionResponseViewModel result21 = ad
                .WithAuditTrailUsername("Username")
                .WithApp("App")
                .WithDocNo("DocNo")
                .WithExecutor(new ApprovalUser())
                .UseSuperiorsConfigurator((wfm, pic) => new List<ApprovalUser>())
                .WithSegmentPersonInCharges(new List<ApprovalUser>())
                .WithSegmentActuals(new List<SegmentConditionActuals>())
                .WithApprovalLevelActuals(new List<ApprovalLevelConditionActuals>())
                .Create();

            IApprovalEngine er = new ApprovalEngine();
            ActionResponseViewModel result22 = er
                .Document()
                .WithAuditTrailUsername("Username")
                .WithApp("App")
                .WithDocNo("DocNo")
                .WithExecutor(new ApprovalUser())
                .UseSuperiorsConfigurator((wfm, pic) => new List<ApprovalUser>())
                .WithSegmentPersonInCharges(new List<ApprovalUser>())
                .WithSegmentActuals(new List<SegmentConditionActuals>())
                .WithApprovalLevelActuals(new List<ApprovalLevelConditionActuals>())
                .Create();

            ActionResponseViewModel result23 = er
                .Approval()
                .WithAuditTrailUsername("Username")
                .WithApp("App")
                .WithDocNo("DocNo")
                .WithExecutor(new ApprovalUser())
                .UseSuperiorsConfigurator((wfman, pic) => new List<ApprovalUser>())
                .WithSegmentPersonInCharges(new List<ApprovalUser>())
                .WithSegmentActuals(new List<SegmentConditionActuals>())
                .WithApprovalLevelActuals(new List<ApprovalLevelConditionActuals>())
                .UsePowerOfAttorneyConfigurator(wfm => new List<PowerOfAttorney>())
                .Approve();

            #endregion

            Console.WriteLine("Done.");

            Console.WriteLine(String.Empty);
            Dbg("Base Type",
                new {
                    BaseType1 = BaseTypeImpl.Get("hello"),
                    BaseType2 = BaseTypeImpl.Get("87246877"),
                    BaseType3 = BaseTypeImpl.Get("hello9834789"),
                    BaseType4 = BaseTypeImpl.Get("98ir739"),
                    BaseType5 = BaseTypeImpl.Get("97983347.9479"),
                    BaseType6 = BaseTypeImpl.Get("hello9873498.0934987"),

                    AppvRule1 = new ApproverRule { Id = "15", RegistrationNo = "08362549" }.AsRuleData(),
                    AppvRule2 = new ApproverRule { Id = "16", RegistrationNo = "07463528" }.AsRuleData(),
                    AppvRule3 = new ApproverRule { Id = "25", RegistrationNo = "06492649" }.AsRuleData()
                }
            );
        }
    }

    #region :: Advanced Type 2 ::

    public interface IApprovalEngine {
        IApprovalDocument Document();
        IApproval Approval();
    }

    public class ApprovalEngine : IApprovalEngine {
        public IApprovalDocument Document() => new ApprovalDocument();

        public IApproval Approval() => new Approval();
    }

    public interface IApprovalDocumentMandatory {
        IApprovalDocument WithAuditTrailUsername(String username);
        IApprovalDocument WithApp(String app);
        IApprovalDocument WithDocNo(String docNo);
        IApprovalDocument WithExecutor(ApprovalUser executor);
        IApprovalDocument UseExecutorConfigurator(Func<IWfManagement, ApprovalUser> configurator);
        IApprovalDocument WithSegmentPersonInCharges(IList<ApprovalUser> pics);
        IApprovalDocument UseSegmentPersonInChargesConfigurator(Func<IWfManagement, IList<ApprovalUser>> configurator);
        IApprovalDocument UseSuperiorsConfigurator(Func<IWfManagement, ApprovalUser, IList<ApprovalUser>> configurator);
        IApprovalDocument WithSegmentActuals(IList<SegmentConditionActuals> actuals);
        IApprovalDocument WithApprovalLevelActuals(IList<ApprovalLevelConditionActuals> actuals);
        IApprovalDocument UseSegmentConditionActualsConfigurator(Func<IWfManagement, ApprovalUser, IList<SegmentConditionActuals>> configurator);
        IApprovalDocument UseApprovalLevelConditionActualsConfigurator(Func<IWfManagement, IList<ApprovalUser>, IList<ApprovalLevelConditionActuals>> configurator);
    }

    public interface IApprovalDocumentOptional {
        IApprovalDocument WithAdditionalSegmentConditions(IList<WfSegmentConditionViewModel> rules);
        IApprovalDocument WithAdditionalApprovalLevelConditions(IList<WfApprovalLevelConditionViewModel> rules);
        IApprovalDocument UseAdditionalSegmentConditionConfigurator(Func<IList<WfSegmentConditionViewModel>> configurator);
        IApprovalDocument UseAdditionalApprovalLevelConditionConfigurator(Func<IList<WfApprovalLevelConditionViewModel>> configurator);
    }

    public interface IApprovalDocument : IApprovalDocumentMandatory, IApprovalDocumentOptional {
        ActionResponseViewModel Create();
    }

    public class ApprovalDocument : IApprovalDocument {
        public IApprovalDocument WithAuditTrailUsername(string username) => this;

        public IApprovalDocument WithApp(string app) => this;

        public IApprovalDocument WithDocNo(string docNo) => this;

        public IApprovalDocument WithExecutor(ApprovalUser executor) => this;

        public IApprovalDocument UseExecutorConfigurator(Func<IWfManagement, ApprovalUser> configurator) => this;

        public IApprovalDocument WithSegmentPersonInCharges(IList<ApprovalUser> pics) => this;

        public IApprovalDocument UseSegmentPersonInChargesConfigurator(Func<IWfManagement, IList<ApprovalUser>> configurator) => this;

        public IApprovalDocument UseSuperiorsConfigurator(Func<IWfManagement, ApprovalUser, IList<ApprovalUser>> configurator) => this;

        public IApprovalDocument WithSegmentActuals(IList<SegmentConditionActuals> actuals) => this;

        public IApprovalDocument WithApprovalLevelActuals(IList<ApprovalLevelConditionActuals> actuals) => this;

        public IApprovalDocument UseSegmentConditionActualsConfigurator(Func<IWfManagement, ApprovalUser, IList<SegmentConditionActuals>> configurator) => this;

        public IApprovalDocument UseApprovalLevelConditionActualsConfigurator(Func<IWfManagement, IList<ApprovalUser>, IList<ApprovalLevelConditionActuals>> configurator) => this;

        public IApprovalDocument WithAdditionalSegmentConditions(IList<WfSegmentConditionViewModel> rules) => this;

        public IApprovalDocument WithAdditionalApprovalLevelConditions(IList<WfApprovalLevelConditionViewModel> rules) => this;

        public IApprovalDocument UseAdditionalSegmentConditionConfigurator(Func<IList<WfSegmentConditionViewModel>> configurator) => this;

        public IApprovalDocument UseAdditionalApprovalLevelConditionConfigurator(Func<IList<WfApprovalLevelConditionViewModel>> configurator) => this;

        public ActionResponseViewModel Create() => new ActionResponseViewModel();
    }

    public interface IApprovalMandatory {
        IApproval WithAuditTrailUsername(String username);
        IApproval WithApp(String app);
        IApproval WithDocNo(String docNo);
        IApproval WithExecutor(ApprovalUser executor);
        IApproval UseExecutorConfigurator(Func<IWfManagement, ApprovalUser> configurator);
        IApproval WithSegmentPersonInCharges(IList<ApprovalUser> pics);
        IApproval UseSegmentPersonInChargesConfigurator(Func<IWfManagement, IList<ApprovalUser>> configurator);
        IApproval UseSuperiorsConfigurator(Func<IWfManagement, ApprovalUser, IList<ApprovalUser>> configurator);
        IApproval WithSegmentActuals(IList<SegmentConditionActuals> actuals);
        IApproval WithApprovalLevelActuals(IList<ApprovalLevelConditionActuals> actuals);
        IApproval UseSegmentConditionActualsConfigurator(Func<IWfManagement, ApprovalUser, IList<SegmentConditionActuals>> configurator);
        IApproval UseApprovalLevelConditionActualsConfigurator(Func<IWfManagement, IList<ApprovalUser>, IList<ApprovalLevelConditionActuals>> configurator);
    }

    public interface IApprovalOptional {
        IApproval WithPowerOfAttorneys(IList<PowerOfAttorney> poas);
        IApproval UsePowerOfAttorneyConfigurator(Func<IWfManagement, IList<PowerOfAttorney>> configurator);
    }

    public interface IApproval : IApprovalMandatory, IApprovalOptional {
        ActionResponseViewModel Approve();
        ActionResponseViewModel Reject();
    }

    public class Approval : IApproval {
        public IApproval WithAuditTrailUsername(string username) => this;

        public IApproval WithApp(string app) => this;

        public IApproval WithDocNo(string docNo) => this;

        public IApproval WithExecutor(ApprovalUser executor) => this;

        public IApproval UseExecutorConfigurator(Func<IWfManagement, ApprovalUser> configurator) => this;

        public IApproval WithSegmentPersonInCharges(IList<ApprovalUser> pics) => this;

        public IApproval UseSegmentPersonInChargesConfigurator(Func<IWfManagement, IList<ApprovalUser>> configurator) => this;

        public IApproval UseSuperiorsConfigurator(Func<IWfManagement, ApprovalUser, IList<ApprovalUser>> configurator) => this;

        public IApproval WithSegmentActuals(IList<SegmentConditionActuals> actuals) => this;

        public IApproval WithApprovalLevelActuals(IList<ApprovalLevelConditionActuals> actuals) => this;

        public IApproval UseSegmentConditionActualsConfigurator(Func<IWfManagement, ApprovalUser, IList<SegmentConditionActuals>> configurator) => this;

        public IApproval UseApprovalLevelConditionActualsConfigurator(Func<IWfManagement, IList<ApprovalUser>, IList<ApprovalLevelConditionActuals>> configurator) => this;

        public IApproval WithPowerOfAttorneys(IList<PowerOfAttorney> poas) => this;

        public IApproval UsePowerOfAttorneyConfigurator(Func<IWfManagement, IList<PowerOfAttorney>> configurator) => this;

        public ActionResponseViewModel Approve() => new ActionResponseViewModel();

        public ActionResponseViewModel Reject() => new ActionResponseViewModel();
    }

    public interface IWfEngine {
        IWfEngine WithAuditTrailUsername(String username);
        IWfEngine WithApp(String app);
        IWfEngine WithDocNo(String docNo);
        IWfEngine WithExecutor(ApprovalUser executor);
        IWfEngine UseExecutorConfigurator(Func<IWfManagement, ApprovalUser> configurator);
        IWfEngine WithSegmentPersonInCharges(IList<ApprovalUser> pics);
        IWfEngine UseSegmentPersonInChargesConfigurator(Func<IWfManagement, IList<ApprovalUser>> configurator);
        IWfEngine UseSuperiorsConfigurator(Func<IWfManagement, ApprovalUser, IList<ApprovalUser>> configurator);
        IWfEngine WithSegmentActuals(IList<SegmentConditionActuals> actuals);
        IWfEngine WithApprovalLevelActuals(IList<ApprovalLevelConditionActuals> actuals);
        IWfEngine UseSegmentConditionActualsConfigurator(Func<IWfManagement, ApprovalUser, IList<SegmentConditionActuals>> configurator);
        IWfEngine UseApprovalLevelConditionActualsConfigurator(Func<IWfManagement, IList<ApprovalUser>, IList<ApprovalLevelConditionActuals>> configurator);

        IWfEngine WithAdditionalSegmentConditions(IList<WfSegmentConditionViewModel> rules);
        IWfEngine WithAdditionalApprovalLevelConditions(IList<WfApprovalLevelConditionViewModel> rules);
        IWfEngine UseAdditionalSegmentConditionConfigurator(Func<IList<WfSegmentConditionViewModel>> configurator);
        IWfEngine UseAdditionalApprovalLevelConditionConfigurator(Func<IList<WfApprovalLevelConditionViewModel>> configurator);

        IWfEngine WithPowerOfAttorneys(IList<PowerOfAttorney> poas);
        IWfEngine UsePowerOfAttorneyConfigurator(Func<IWfManagement, IList<PowerOfAttorney>> configurator);
        ActionResponseViewModel Start();
        ActionResponseViewModel Approve();
        ActionResponseViewModel Reject();
    }

    public class WfEngine : IWfEngine {
        public IWfEngine WithAuditTrailUsername(string username) => this;

        public IWfEngine WithApp(string app) => this;

        public IWfEngine WithDocNo(string docNo) => this;

        public IWfEngine WithExecutor(ApprovalUser executor) => this;

        public IWfEngine UseExecutorConfigurator(Func<IWfManagement, ApprovalUser> configurator) => this;

        public IWfEngine WithSegmentPersonInCharges(IList<ApprovalUser> pics) => this;

        public IWfEngine UseSegmentPersonInChargesConfigurator(Func<IWfManagement, IList<ApprovalUser>> configurator) => this;

        public IWfEngine UseSuperiorsConfigurator(Func<IWfManagement, ApprovalUser, IList<ApprovalUser>> configurator) => this;

        public IWfEngine WithSegmentActuals(IList<SegmentConditionActuals> actuals) => this;

        public IWfEngine WithApprovalLevelActuals(IList<ApprovalLevelConditionActuals> actuals) => this;

        public IWfEngine UseSegmentConditionActualsConfigurator(Func<IWfManagement, ApprovalUser, IList<SegmentConditionActuals>> configurator) => this;

        public IWfEngine UseApprovalLevelConditionActualsConfigurator(Func<IWfManagement, IList<ApprovalUser>, IList<ApprovalLevelConditionActuals>> configurator) => this;

        public IWfEngine WithAdditionalSegmentConditions(IList<WfSegmentConditionViewModel> rules) => this;

        public IWfEngine WithAdditionalApprovalLevelConditions(IList<WfApprovalLevelConditionViewModel> rules) => this;

        public IWfEngine UseAdditionalSegmentConditionConfigurator(Func<IList<WfSegmentConditionViewModel>> configurator) => this;

        public IWfEngine UseAdditionalApprovalLevelConditionConfigurator(Func<IList<WfApprovalLevelConditionViewModel>> configurator) => this;

        public IWfEngine WithPowerOfAttorneys(IList<PowerOfAttorney> poas) => this;

        public IWfEngine UsePowerOfAttorneyConfigurator(Func<IWfManagement, IList<PowerOfAttorney>> configurator) => this;

        public ActionResponseViewModel Start() => new ActionResponseViewModel();

        public ActionResponseViewModel Approve() => new ActionResponseViewModel();

        public ActionResponseViewModel Reject() => new ActionResponseViewModel();
    }

    public interface IWfManagement { }

    public class PowerOfAttorney { }

    public class WfApprovalLevelConditionViewModel { }

    public class WfSegmentConditionViewModel { }

    public class ApprovalLevelConditionActuals { }

    public class SegmentConditionActuals { }

    public class ApprovalUser { }

    // public class ActionResponseViewModel { }

    #endregion

    #region :: Advanced Type 1 ::

    public class Rule : IDispatchedRule {
        private Rule() { }

        public static IDispatchedRule Create() => new Rule();

        public IExpressedRule ExpressWith(IExpression expr) => this;

        public IExpressedRule OrWith(IExpression expr) => this;

        public IExpressedRule AndWith(IExpression expr) => this;

        public IDispatchedRule DispatchWith(IActionDispatcher action) => this;

        public IDispatchedRule ThenWith(IActionDispatcher action) => this;

        public Boolean Evaluate(IList<BaseType> actuals) => true;
    }

    public interface IRule {
        Boolean Evaluate(IList<BaseType> actuals);
    }

    public interface IExpressedRule : IRule {
        IExpressedRule ExpressWith(IExpression expr);
        IExpressedRule OrWith(IExpression expr);
        IExpressedRule AndWith(IExpression expr);
    }

    public interface IDispatchedRule : IExpressedRule {
        IDispatchedRule DispatchWith(IActionDispatcher action);
        IDispatchedRule ThenWith(IActionDispatcher action);
    }

    public interface IExpression { }
    public interface IActionDispatcher { }

    public class Expr : IExpression { }
    public class Disp : IActionDispatcher { }
    public class NullDisp : IActionDispatcher { }

    public class BaseType { }

    #endregion

    #region :: BaseType ::

    public class ApproverRule {
        public String Id { get; set; }
        public String RegistrationNo { get; set; }
    }

    public class BaseTypeImpl {
        public Object Value { get; private set; }
        public Type Type { get; private set; }

        public BaseTypeImpl(Object value, Type type) {
            Value = value;
            Type = type;
        }

        public static BaseTypeImpl Get(String stringVar) {
            if (String.IsNullOrEmpty(stringVar))
                throw new InvalidOperationException("The provided string must not be null");

            String processed = stringVar.ToLowerInvariant();
            if (processed == "true" || processed == "false")
                return new BaseTypeImpl(Convert.ToBoolean(processed), typeof(Boolean));
            if (Decimal.TryParse(processed, out Decimal parsedD))
                return new BaseTypeImpl(parsedD, typeof(Decimal));

            return new BaseTypeImpl(stringVar, typeof(String));
        }
    }

    public static class Ext {
        public static Dictionary<String, BaseTypeImpl> AsRuleData<T>(this T tObj) where T : class {
            var result = new Dictionary<String, BaseTypeImpl>();
            Type tType = typeof(T);
            PropertyInfo[] tProperties = tType.GetProperties();
            if (tProperties.Length != 0) {
                foreach (PropertyInfo property in tProperties) {
                    Type propType = property.PropertyType;
                    Object propOriValue = property.GetValue(tObj, null);
                    Object propValue = propType.FullName == "System.String" ? "'" + Convert.ChangeType(propOriValue, typeof(String)) + "'" : propOriValue;
                    var ruleValue = new BaseTypeImpl(propValue, propType);
                    result.Add(property.Name, ruleValue);
                }
            }

            return result;
        }
    }

    #endregion
}
