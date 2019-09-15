using System.Configuration;
using System.Web.Configuration;

namespace CSScratchpad.Script {
    class ReadAppConfigSection : Common, IRunnable {
        public void Run() =>
            Dbg(
                new {
                    SystemWebAuthentication = ConfigurationManager.GetSection("system.web/authentication") as AuthenticationSection,
                    SystemWebAuthorization = ConfigurationManager.GetSection("system.web/authorization") as AuthorizationSection,
                    SystemWebMembership = ConfigurationManager.GetSection("system.web/membership") as MembershipSection
                }
            );
    }
}
