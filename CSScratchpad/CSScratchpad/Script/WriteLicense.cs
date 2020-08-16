using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using Dfy;
using Newtonsoft.Json;
using Scratch;
using SecurityExt = CSScratchpad.Script.TestSecurity.SecurityExt;

namespace CSScratchpad.Script {
    public class WriteLicense : Common, IRunnable {
        public void Run() {
            CreateRequestTicket();
            CreateLicFile();
            //ValidateLicFile();
            //ValidateLicFile2();
        }

        // NOTE: client and server should have different salt and key
        // maybe can be refcatored to use public key instead
        readonly static String EnxKey = SecurityExt.EncodeBase64Url("PnBQQS54JFEoVlQwTGctOnVVaEE9RS1ZWEtMJTFKUTQ3JStLT2ZENDhtQE5UdGFlbn1sRnx8JTg6S0lGW2F0IQ");
        readonly static String EnxSalt = SecurityExt.EncodeBase64Url("LVZWcWYmcCh5bl03XCZuPnBYSH06RCZaVU52PzZFUzV5XFgtPnshNHM3VTZoKVRVTGAyUmN6UChEdUdmNX1hKjhieDs9XDRLRFxtWmMhI35VeCs4VjtwfkFlZjJMJEhWKWArXStiYFttLmh4KU1QSiFjMzdKVCRzQ1YwI3FzMDs");

        void CreateRequestTicket() {
            String tenantName = "Celestial Being GN-00";
            String tenantIdentifier = String.Join(".", tenantName, GetComputerName());

            // Example 1
            DownloadFileInfo requestTicket = License.GetRequestTicket(tenantIdentifier);
            File.WriteAllText(GetOutputPath(requestTicket.Filename), SecurityExt.EncodeBase64UrlFromBytes(requestTicket.FileByteArray));

            // Example 2
            License.SaveRequestTicket(OutputDirPath, tenantIdentifier);
        }

        void CreateLicFile() {
            String tenantName = "Celestial Being GN-00";
            String tenantIdentifier = String.Join(".", tenantName, GetComputerName());

            DownloadFileInfo rqsFileInfo = License.GetRequestTicket(tenantIdentifier);

            var licInfo = new LicenseInfo {
                RequestTicket = SecurityExt.EncodeBase64UrlFromBytes(rqsFileInfo.FileByteArray),
                ClientName = tenantName,
                Type = LicenseType.Trial,
                EffectiveDate = DateTime.Now.AddDays(-3),
                ExpiredDate = DateTime.Now.AddDays(10),
                UsersCount = 50,
                Buffer = 10,
                Modules = new List<String> { "Report", "Analytics", "Contacts" }
            };

            DownloadFileInfo licFile = License.GetLicense(tenantName, licInfo);

            using (var writer = new BinaryWriter(File.Open(GetOutputPath(licFile.Filename), FileMode.Create)))
                foreach (Byte b in licFile.FileByteArray)
                    writer.Write(b);
        }

        /*
        void ValidateLicFile() {
            String identifier = GetComputerName();
            Boolean isValid = License.IsValid(identifier, @"E:\Dita\", () => new List<String> { "Celestial Being GN-00" }, () => 55, () => new List<String> { "Report", "Analytics", "Contacts" });

            Console.Write(isValid);
        }

        void ValidateLicFile2() {
            String identifier = GetComputerName();
            Boolean isValid = License.IsValid(identifier, @"E:\Dita\", () => new List<String> { "PT BIT" }, () => 57, () => new List<String> { "Report", "Analytics", "Contacts" });

            Console.WriteLine(isValid);
        }
        */

        String GetComputerName() {
            var netwQuery = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = true");
            IEnumerable queryCollection = netwQuery.Get();
            String compName = queryCollection
                .OfType<ManagementObject>()
                .SelectMany(obj => obj
                    .Properties.OfType<PropertyData>()
                    .Where(pd => pd.Name == "DNSHostName")
                    .Select(pd => pd.Value.ToString().Trim())
                )
                .FirstOrDefault();

            return compName;
        }

        #region :: License ::

        public enum LicenseType : Byte {
            Trial = 0,
            Full = 1
        }

        public sealed class LicenseInfo {
            public String RequestTicket { get; set; }
            public DateTime EffectiveDate { get; set; }
            public String ClientName { get; set; }
            public LicenseType Type { get; set; }
            public DateTime ExpiredDate { get; set; }
            public Int32 UsersCount { get; set; }
            public Double Buffer { get; set; }
            public IList<String> Modules { get; set; }
        }

        public static class License {
            const String BinaryFileMime = "application/octet-stream";

            public static void SaveLicenseFile(String outputDir, String identifier, LicenseInfo license) {
                DownloadFileInfo lic = GetLicense(identifier, license);
                using (var writer = new BinaryWriter(File.Open(Path.Combine(outputDir, lic.Filename), FileMode.Create)))
                    foreach (Byte b in lic.FileByteArray)
                        writer.Write(b);
            }

            public static DownloadFileInfo GetLicense(String identifier, LicenseInfo license) {
                String jsonLic = JsonConvert.SerializeObject(license);
                String licHash = SecurityExt.Encrypt(jsonLic, EnxKey, EnxSalt); // NOTE: should be get from config
                String filename = Path.Combine($"{identifier}.{Guid.NewGuid().ToString("N")}.lcs");

                Byte[] fileBytes;
                using (var stream = new MemoryStream()) {
                    using (var writer = new BinaryWriter(stream)) {
                        writer.Write("LCS");
                        writer.Write(0x76); // L
                        writer.Write(0x67); // C
                        writer.Write(0x83); // S
                        writer.Write(licHash);
                        writer.Write(0x76); // L
                        writer.Write(0x67); // C
                        writer.Write(0x83); // S
                    }

                    fileBytes = stream.ToArray();
                }

                return new DownloadFileInfo {
                    Filename = filename,
                    FileByteArray = fileBytes,
                    MimeType = BinaryFileMime
                };
            }

            public static void SaveRequestTicket(String outputDir, String identifier) {
                DownloadFileInfo requestTicket = GetRequestTicket(identifier);
                using (var writer = new BinaryWriter(File.Open(Path.Combine(outputDir, requestTicket.Filename), FileMode.Create)))
                    foreach (Byte b in requestTicket.FileByteArray)
                        writer.Write(b);
            }

            // NOTE: call this on clientmachine
            public static DownloadFileInfo GetRequestTicket(String identifier) {
                String delimited = String.Join(TestNvy.NameValueItem.ItemDelimiter.ToString(), identifier, GetDelimitedHardwareIds());
                String hwHash = SecurityExt.Encrypt(delimited, EnxKey, EnxSalt); // NOTE: should be get from config
                String filename = Path.Combine($"{identifier}.{Guid.NewGuid().ToString("N")}.rqs");

                Byte[] fileBytes;
                using (var stream = new MemoryStream()) {
                    using (var writer = new BinaryWriter(stream)) {
                        writer.Write("RQS");
                        writer.Write(0x82); // R
                        writer.Write(0x81); // Q
                        writer.Write(0x83); // S
                        writer.Write(hwHash);
                        writer.Write(0x82); // R
                        writer.Write(0x81); // Q
                        writer.Write(0x83); // S
                    }

                    fileBytes = stream.ToArray();
                }

                return new DownloadFileInfo {
                    Filename = filename,
                    FileByteArray = fileBytes,
                    MimeType = BinaryFileMime
                };
            }

            static String GetDelimitedHardwareIds() {
                IList<String> hwList = GetHardwareIds();
                String delimited = String.Join(TestNvy.NameValueItem.ItemDelimiter.ToString(), hwList).PadLeft(1000, TestNvy.NameValueItem.ItemDelimiter);

                return delimited;
            }

            static IList<String> GetHardwareIds() {
                IList<String> hwList = new List<String>();

                // https://stackoverflow.com/a/3580907 --> OfType
                var procQuery = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                IEnumerable queryCollection = procQuery.Get();
                hwList = hwList.Concat(
                    queryCollection
                        .OfType<ManagementObject>()
                        .SelectMany(obj => obj
                            .Properties.OfType<PropertyData>()
                            .Where(pd => new[] { "DeviceID", "ProcessorId" }.Contains(pd.Name))
                            .Select(pd => pd.Value.ToString().Trim())
                        )
                ).ToList();

                var hddQuery = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                queryCollection = hddQuery.Get();
                hwList = hwList.Concat(
                    queryCollection
                        .OfType<ManagementObject>()
                        .SelectMany(obj => obj
                            .Properties.OfType<PropertyData>()
                            .Where(pd => new[] { "Model", "SerialNumber" }.Contains(pd.Name))
                            .Select(pd => pd.Value.ToString().Trim())
                        )
                ).ToList();

                var netwQuery = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = true");
                queryCollection = netwQuery.Get();
                hwList = hwList.Concat(
                    queryCollection
                        .OfType<ManagementObject>()
                        .SelectMany(obj => obj
                            .Properties.OfType<PropertyData>()
                            .Where(pd => new[] { "DNSHostName", "MACAddress" }.Contains(pd.Name))
                            .Select(pd => pd.Value.ToString().Trim())
                        )
                ).ToList();

                hwList = hwList.Concat(
                    queryCollection
                        .OfType<ManagementObject>()
                        .SelectMany(obj => obj
                            .Properties.OfType<PropertyData>()
                            .Where(pd => pd.Name == "IPAddress")
                            .SelectMany(pd => pd.Value as String[])
                        )
                ).ToList();

                return hwList;
            }

            /*
            public static Boolean IsValid(String identifier, String licDirectoryPath, Func<IList<String>> getClientIdAction, Func<Int32> getUsersCountAction, Func<IList<String>> getModulesAction) {
                IList<String> licPaths = Directory.EnumerateFiles(licDirectoryPath, "*.lcs").ToList();
                if (!licPaths.Any())
                    return false;

                return Validate(licPaths.FirstOrDefault(), getClientIdAction, getUsersCountAction, getModulesAction);
            }
            */

            /*private static Boolean Validate(String identifier, String licDirectoryPath, Func<IList<String>> getClientIdAction, Func<Int32> getEmployeeCountAction, Func<IList<String>> getModulesAction)
            {
                Int32 actualRqsLength;
                Boolean isRequestTicketValid = ValidateRequestTicket(identifier, licDirectoryPath, out actualRqsLength);
                Boolean isLicenseValid = ValidateLicense(identifier, licDirectoryPath, actualRqsLength, getClientIdAction, getEmployeeCountAction, getModulesAction);

                return isRequestTicketValid && isLicenseValid;
            }*/

            /*private static Boolean ValidateRequestTicket(String identifier, String licDirectoryPath, out Int32 actualRqsLength)
            {
                DownloadFileInfo rqsFile = GetRequestTicket(identifier);
                Byte[] actualRqs = rqsFile.FileByteArray;
                actualRqsLength = actualRqs.Length;
                //String actualRqsString = Encoding.UTF8.GetString(actualRqs);

                /*Byte[] wholeLic = File.ReadAllBytes(licDirectoryPath);
                var rqs = wholeLic.Take(actualRqsLength).ToArray();
                String rqsString = Encoding.UTF8.GetString(rqs);#1#


                Boolean isValid = PBKDF2SHA1.ValidatePassword(GetDelimitedHardwareIds(), rqsString);

                /*Int32 result = String.CompareOrdinal(actualRqsString, rqsString);

                return result == 0;#1#

                return isValid;
            }*/

                /*
            static Boolean Validate(String licDirectoryPath, Func<IList<String>> getClientNamesAction, Func<Int32> getUsersCountAction, Func<IList<String>> getModulesAction) {
                LicenseInfo licInfo = GetLicenseInfo(licDirectoryPath);

                String hwIds = GetDelimitedHardwareIds();
                Boolean isValid = PBKDF2SHA1.ValidatePassword(hwIds, licInfo.RequestTicket);
                if (!isValid)
                    return false;

                DateTime today = DateTime.Now.Date;
                DateTime effectiveDate = licInfo.EffectiveDate.Date;
                if (today.Subtract(effectiveDate).Days < 0)
                    return false;

                if (licInfo.Type == LicenseType.Trial)
                {
                    DateTime expiredDate = licInfo.ExpiredDate.Date;
                    if (today.Subtract(expiredDate).Days > 0)
                        return false;
                }

                IList<String> actualClientNames = getClientNamesAction();
                if (!actualClientNames.Contains(licInfo.ClientName))
                    return false;

                Int32 allowedEmployeeCount = Convert.ToInt32(licInfo.UsersCount + (licInfo.UsersCount * (licInfo.Buffer / 100)));
                Int32 actualEmployeeCount = getUsersCountAction();
                if (allowedEmployeeCount < actualEmployeeCount)
                    return false;

                return true;
            }

            static LicenseInfo GetLicenseInfo(String licDirectoryPath) {
                Byte[] lic = File.ReadAllBytes(licDirectoryPath);
                String licString = Encoding.UTF8.GetString(lic);
                String licJson = Cryptor.Decrypt(licString);
                LicenseInfo licInfo = JsonConvert.DeserializeObject<LicenseInfo>(licJson);

                return licInfo;
            }

            public static IList<String> GetAllowedModules(String licDirectoryPath) {
                LicenseInfo licInfo = GetLicenseInfo(licDirectoryPath);
                return licInfo.Modules;
            }
            */
        }

        String GetMacAddress() {
            const Int32 MinMacAddrLength = 12;
            String macAddress = String.Empty;
            Int64 maxSpeed = -1;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces()) {
                Console.WriteLine(
                    "Found MAC Address: " + nic.GetPhysicalAddress() +
                    " Type: " + nic.NetworkInterfaceType);

                String tempMac = nic.GetPhysicalAddress().ToString();
                if (nic.Speed > maxSpeed &&
                    !String.IsNullOrEmpty(tempMac) &&
                    tempMac.Length >= MinMacAddrLength) {
                    Console.WriteLine("New Max Speed = " + nic.Speed + ", MAC: " + tempMac);
                    maxSpeed = nic.Speed;
                    macAddress = tempMac;
                }
            }

            return macAddress;
        }

        String GetNetworkHw() =>
            (from nic in NetworkInterface.GetAllNetworkInterfaces()
                where nic.OperationalStatus == OperationalStatus.Up
                      && nic.NetworkInterfaceType.ToString().ToLowerInvariant() != "loopback"
                select nic.GetPhysicalAddress().ToString()
            ).Aggregate("", (p, n) => (p != String.Empty ? p + "|" : p).Trim() + n.Trim());

        #endregion
    }
}
