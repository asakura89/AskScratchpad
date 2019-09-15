using System;
using System.Collections.Generic;
using System.Reflection;

namespace CSScratchpad.Script {
    class GetProperties : Common, IRunnable {
        public void Run() {
            Dbg("Vendor property types", GetAllProperties<VendorData>());
            Dbg("Client property types", GetAllProperties<ClientData>());
        }

        IEnumerable<String> GetAllProperties<T>() {
            T t = Activator.CreateInstance<T>();
            Type tType = t.GetType();
            PropertyInfo[] propInfos = tType.GetProperties();

            foreach (PropertyInfo propInfo in propInfos)
                yield return propInfo.PropertyType.Name;
        }
    }

    public class VendorData {
        public String VendorId { get; set; }
        public String VendorName { get; set; }
        public String VendorAddress { get; set; }
        public String VendorPhone { get; set; }
        public String VendorFax { get; set; }
        public String VendorTaxNumber { get; set; }
        public Boolean RecActive { get; set; }
        public String RecCreatedBy { get; set; }
        public DateTime RecCreated { get; set; }
        public String RecEditedBy { get; set; }
        public DateTime RecEdited { get; set; }

        public VendorData() {
            VendorId = "";
            VendorName = "";
            VendorAddress = "";
            VendorPhone = "";
            VendorFax = "";
            VendorTaxNumber = "";
            RecActive = false;
            RecCreatedBy = "";
            RecCreated = DateTime.Now;
            RecEditedBy = "";
            RecEdited = DateTime.Now;
        }
    }

    public class ClientData {
        public String Name { get; set; } = String.Empty;
        public String PIC { get; set; } = String.Empty;
        public DateTime ContractSigned { get; set; } = DateTime.MinValue;
    }
}
