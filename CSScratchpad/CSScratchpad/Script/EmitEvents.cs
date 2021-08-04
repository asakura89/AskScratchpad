using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security.AntiXss;
using Newtonsoft.Json.Linq;
using Scratch;
using Console = System.Console;

namespace CSScratchpad.Script {
    class EmitEvents : Common, IRunnable {
        public void Run() {
            PatchableConfig.Load();
            //JObject events = PatchableConfig.All["events"].AsJEnumerable().ToList();
            IList<PatchableEvent> events = PatchableConfig.Events;
            
            var emitter = new EventEmitter(new PatchableConfigEventStorage());
        }
            
        #region :: VaryaExt ::

        public static class StringExt {
            //const String MainRegex = @"\$\{(?<key>[a-zA-Z0-9\\.\\-\\_]*)(:(?<value>[a-zA-Z0-9\\.\\-\\_\s]*))?\}";
            const String MainRegex = @"\$\{(?<key>[a-zA-Z0-9\.\-\\_]*)(:(?<value>[^\{\}]+))?\}";

            public static String Resolve(String string2Resolve) => ReplaceWithDictionary(string2Resolve, new Dictionary<String, String>());

            public static String ReplaceWith<TReplace>(String string2Replace, TReplace replacements) where TReplace : class, new() =>
                ReplaceWithDictionary(
                    string2Replace,
                    replacements
                        .GetType()
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .ToDictionary(prop => prop.Name, prop => Convert.ToString(prop.GetValue(replacements, null)))
                );

            public static String ReplaceWithDictionary(String string2Replace, IDictionary<String, String> replacements) {
                String result = Regex.Replace(
                    string2Replace,
                    MainRegex,
                    match => HandleRegex(match, replacements),
                    RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);

                Match containsExpressionMatch = Regex.Match(result, MainRegex, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);
                if (containsExpressionMatch.Success)
                    return ReplaceWithDictionary(result, replacements);

                return result;
            }

            static String HandleRegex(Match match, IDictionary<String, String> replacements) {
                String key = match.Groups["key"].Value;
                String value = match.Groups["value"].Value;
                if (String.IsNullOrEmpty(key))
                    return match.Value;

                IEnumerable<String> operations = new[] { "config", "econfig", "var", "urle", "urld", "htmle", "htmld" };
                if (!operations.Contains(key)) {
                    if (replacements == null)
                        return match.Value;

                    if (!replacements.Any())
                        return match.Value;

                    return replacements.ContainsKey(key) ? replacements[key] : String.Empty;
                }

                if (value == null)
                    return String.Empty;

                if (value.All(Char.IsWhiteSpace))
                    return String.Empty;

                switch (key) {
                    case "config":
                        return HandleConfig(value, replacements);
                    case "econfig":
                        return HandleEncryptedConfig(value, replacements);
                    case "var":
                        return HandleVar(value);
                    case "urle":
                        return HandleUrlEncode(value);
                    case "urld":
                        return HandleUrlDecode(value);
                    case "htmle":
                        return HandleHtmlEncode(value);
                    case "htmld":
                        return HandleHtmlDecode(value);
                }

                return String.Empty;
            }

            static String HandleConfig(String configKey, IDictionary<String, String> replacements) => ReplaceWithDictionary(ConfigurationManager.AppSettings.Get(configKey) ?? String.Empty, replacements);

            static String HandleEncryptedConfig(String configKey, IDictionary<String, String> replacements) {
                String config = HandleConfig(configKey, replacements);
                return SecurityExt.Decrypt(config);
            }

            static String HandleVar(String var) {
                var variableRegexes = new Dictionary<String, String> {
                    ["Timespan"] = @"(?<digit>\d{1,})(?<type>[Dd]|[Hh]|[Mm]|[Ss])",
                    ["Datetime"] = @"[Dd]ate[Tt]ime\((?<dtformat>\w*)\)",
                    ["BaseDir"] = "BaseDir"
                };

                var variableHandlers = new Dictionary<String, Func<Match, String>> {
                    ["Timespan"] = HandleTimespan,
                    ["Datetime"] = HandleDatetime,
                    ["BaseDir"] = HandleBaseDir
                };

                foreach (String key in variableRegexes.Keys) {
                    Match varMatch = Regex.Match(var, variableRegexes[key], RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);
                    if (varMatch.Success)
                        return variableHandlers[key].Invoke(varMatch);
                }

                return String.Empty;
            }

            static String HandleTimespan(Match match) {
                String digit = match.Groups["digit"].Value;
                String type = match.Groups["type"].Value;
                if (String.IsNullOrEmpty(digit))
                    return String.Empty;

                if (String.IsNullOrEmpty(type))
                    return String.Empty;

                String duration = digit.PadLeft(2, '0');
                switch (type) {
                    case "d":
                        return $"{duration}:00:00:00";
                    case "h":
                        return $"00:{duration}:00:00";
                    case "m":
                        return $"00:00:{duration}:00";
                    case "s":
                        return $"00:00:00:{duration}";
                    default:
                        return "00:00:00:00";
                }
            }

            static String HandleDatetime(Match match) {
                String format = match.Groups["dtformat"].Value;
                if (String.IsNullOrEmpty(format))
                    return String.Empty;

                return DateTime.Now.ToString(format);
            }

            static String HandleBaseDir(Match match) => AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');

            static String HandleUrlEncode(String value) => AntiXssEncoder.UrlEncode(value).Replace("+", "%20");

            static String HandleUrlDecode(String value) => HttpUtility.UrlDecode(value);

            static String HandleHtmlEncode(String value) => AntiXssEncoder.HtmlEncode(value, true);

            static String HandleHtmlDecode(String value) => HttpUtility.HtmlDecode(value);
        }

        #endregion

        #region :: Security Ext ::

        public static class SecurityExt {
            static RijndaelManaged CreateRijndaelAlgorithm(String securityKey, String securitySalt) {
                Byte[] saltBytes = Encoding.UTF8.GetBytes(securityKey + securitySalt);
                var randByte = new Rfc2898DeriveBytes(securityKey, saltBytes, 12000);

                const Int32 MaxOutSize = 256;
                const Int32 MaxOutSizeInBytes = MaxOutSize / 8;
                return new RijndaelManaged {
                    BlockSize = MaxOutSize,
                    Key = randByte.GetBytes(MaxOutSizeInBytes),
                    IV = randByte.GetBytes(MaxOutSizeInBytes),
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7
                };
            }

            public static String Encrypt(String plainText) => Encrypt(plainText, GetKey(), GetSalt());

            public static String Encrypt(String plainText, String securityKey, String securitySalt) {
                using (RijndaelManaged algorithm = CreateRijndaelAlgorithm(securityKey, securitySalt)) {
                    Byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    Byte[] cipherBytes = null;
                    using (var stream = new MemoryStream()) {
                        using (var cryptoStream = new CryptoStream(stream, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
                            cryptoStream.Write(plainBytes, 0, plainBytes.Length);

                        cipherBytes = stream.ToArray();
                    }

                    return EncodeBase64UrlFromBytes(cipherBytes);
                }
            }

            public static String Decrypt(String chiperText) => Decrypt(chiperText, GetKey(), GetSalt());

            public static String Decrypt(String chiperText, String securityKey, String securitySalt) {
                using (RijndaelManaged algorithm = CreateRijndaelAlgorithm(securityKey, securitySalt)) {
                    Byte[] cipherBytes = DecodeBase64UrlToBytes(chiperText);
                    Byte[] plainBytes = null;
                    using (var encstream = new MemoryStream(cipherBytes)) {
                        using (var decstream = new MemoryStream()) {
                            using (var cryptoStream = new CryptoStream(encstream, algorithm.CreateDecryptor(), CryptoStreamMode.Read)) {
                                Int32 data;
                                while ((data = cryptoStream.ReadByte()) != -1)
                                    decstream.WriteByte((Byte) data);

                                decstream.Position = 0;
                                plainBytes = decstream.ToArray();
                            }
                        }
                    }

                    return Encoding.UTF8.GetString(plainBytes);
                }
            }

            static String GetKey() {
                String key = ConfigurationManager.AppSettings.Get("Security.Key");
                if (String.IsNullOrEmpty(key))
                    return String.Empty;

                return EncodeBase64Url(key);
            }

            static String GetSalt() {
                String salt = ConfigurationManager.AppSettings.Get("Security.Salt");
                if (String.IsNullOrEmpty(salt))
                    return String.Empty;

                return EncodeBase64Url(salt);
            }

            const String Base64Plus = "+";
            const String Base64Slash = "/";
            const String Base64Underscore = "_";
            const String Base64Minus = "-";
            const String Base64Equal = "=";
            const String Base64DoubleEqual = "==";
            const Char Base64EqualChar = '=';

            public static String EncodeBase64Url(String plain) =>
                EncodeBase64UrlFromBytes(
                    Encoding.UTF8.GetBytes(plain)
                );

            static String EncodeBase64UrlFromBytes(Byte[] bytes) =>
                Convert
                    .ToBase64String(bytes)
                    .TrimEnd(Base64EqualChar)
                    .Replace(Base64Plus, Base64Minus)
                    .Replace(Base64Slash, Base64Underscore);

            public static String DecodeBase64Url(String base64Url) =>
                Encoding.UTF8.GetString(
                    DecodeBase64UrlToBytes(base64Url)
                );

            static Byte[] DecodeBase64UrlToBytes(String base64Url) {
                String halfProcessed = base64Url
                    .Replace(Base64Minus, Base64Plus)
                    .Replace(Base64Underscore, Base64Slash);

                String base64 = halfProcessed;
                if (halfProcessed.Length % 4 == 2)
                    base64 = halfProcessed + Base64DoubleEqual;
                else if (halfProcessed.Length % 4 == 3)
                    base64 = halfProcessed + Base64Equal;

                return Convert.FromBase64String(base64);
            }

            public static SecureString AsSecureString(String plain) {
                if (String.IsNullOrEmpty(plain) || String.IsNullOrWhiteSpace(plain))
                    throw new ArgumentNullException(nameof(plain));

                return new NetworkCredential(String.Empty, plain).SecurePassword;
            }

            public static String AsPlainString(SecureString secure) {
                if (secure == null)
                    throw new ArgumentNullException(nameof(secure));

                return new NetworkCredential(String.Empty, secure).Password;
            }

            const String UppercaseAlphabet = "A B C D E F G H I J K L M N O P Q R S T U V W X Y Z";
            const String LowercaseAlphabet = "a b c d e f g h i j k l m n o p q r s t u v w x y z";
            const String Numeric = "1 2 3 4 5 6 7 8 9 0";
            const String Symbol = "~ ! @ # $ % ^ & * _ - + = ` | \\ ( ) { } [ ] : ; < > . ? /";

            static Int32 GenerateRandomNo(Int32 upperBound) {
                Int32 seed = Guid.NewGuid().GetHashCode() % 50001;
                var rnd = new Random(seed);
                return rnd.Next(0, upperBound);
            }

            static String GenerateRandomAlphanumeric(Int32 length) {
                String[] charCombination = (UppercaseAlphabet + " " + LowercaseAlphabet + " " + Numeric + " " + Symbol).Split(' ');
                var output = new StringBuilder();
                for (Int32 ctr = 0; ctr < length; ctr++) {
                    Int32 randomIdx = GenerateRandomNo(charCombination.Length - 1);
                    output.Append(charCombination[randomIdx]);
                }

                return output.ToString();
            }

            public static String GenerateKey() => EncodeBase64Url(GenerateRandomAlphanumeric(64));

            public static String GenerateSalt() => EncodeBase64Url(GenerateRandomAlphanumeric(128));
        }

        #endregion

        #region :: PatchableConfig ::

        public interface IPatchable {
            String Name { get; }
            String Put { get; }
        }

        public sealed class PatchableEvent : IPatchable {
            public PatchableEvent(String name, String put, String type, String @event) {
                Name = name ?? throw new ArgumentNullException(nameof(name));
                Put = put ?? throw new ArgumentNullException(nameof(put));
                Type = type ?? throw new ArgumentNullException(nameof(type));
                Event = @event ?? throw new ArgumentNullException(nameof(@event));
            }

            public String Name { get; }
            public String Put { get; }
            public String Type { get; }
            public String Event { get; }
        }

        public static class PatchableConfig {
            public static JObject All { get; } = JObject.Parse("{}");

            public static IList<PatchableEvent> Events { get; } = new List<PatchableEvent>();

            public static void Load() {
                String configSource = ConfigurationManager.AppSettings.Get("PatchableConfigSource") ?? String.Empty;
                String configSourcePath = StringExt.Resolve(configSource);
                Load(configSourcePath);
            }

            public static void Load(String configSource) {
                if (String.IsNullOrEmpty(configSource))
                    throw new ArgumentNullException(nameof(configSource));

                if (!Directory.Exists(configSource))
                    throw new DirectoryNotFoundException(configSource);

                IList<String> configPaths = GetRecurseConfigPaths(configSource);
                MergeAllConfigs(configPaths);
                //CleanupConfigs();
                PlotConfigToEachObject();
                SortByPut();
            }

            static void SortByPut() {
                SortEvents();
                // ... other Sort ...
                Console.WriteLine("-");
            }

            static void SortEvents() {
                IList<PatchableEvent> events = new List<PatchableEvent>(Events);
            } 

            static void PlotConfigToEachObject() {
                PlotEvents();
                // ... other Plot ...
                Console.WriteLine("-");
            }

            static void PlotEvents() =>
                (All["events"] as JArray)
                .Select(arrItem => new PatchableEvent (
                    (String) arrItem.SelectToken("name"),
                    (String) arrItem.SelectToken("put"),
                    (String) arrItem.SelectToken("type"),
                    (String) arrItem.SelectToken("event")))
                .ToList()
                .ForEach(Events.Add);

            //static readonly IList<String> knownConfigSections = new List<String> { "config.var", "events" };

            //static void CleanupConfigs() {
            //    IList<String> unkownSections = All
            //        .Properties()
            //        .Select(token => token.Name)
            //        .Where(section => !knownConfigSections.Contains(section))
            //        .ToList();

            //    if (unkownSections.Any())
            //        foreach (String unkownSection in unkownSections)
            //            All.Remove(unkownSection);
            //}

            static void MergeAllConfigs(IList<String> configPaths) {
                foreach (String path in configPaths) {
                    var config = JObject.Parse(File.ReadAllText(path));
                    All.Merge(config, new JsonMergeSettings {
                        PropertyNameComparison = StringComparison.OrdinalIgnoreCase,
                        MergeNullValueHandling = MergeNullValueHandling.Merge,
                        MergeArrayHandling = MergeArrayHandling.Union
                    });
                }
            }

            static List<String> GetRecurseConfigPaths(String configSource) =>
                FlattenItemRecursively(GetFileSystemEntries(configSource), 0)
                    .Select(level => level.Entry.Path)
                    .Where(path => Path.GetExtension(path).Equals(".json", StringComparison.OrdinalIgnoreCase))
                    .OrderBy(Path.GetFileNameWithoutExtension)
                    .ToList();

            public class Level {
                public Int32 LevelCount;
                public FileSystemEntry Entry;
            }

            public class FileSystemEntry {
                public String Path;
                public Boolean IsDirectory;
            }

            static String GetDirectory(String filepath) => IsDirectory(filepath) ? filepath : Path.GetDirectoryName(filepath);

            static IEnumerable<FileSystemEntry> GetFileSystemEntries(String path) => Directory
                .EnumerateFileSystemEntries(path)
                .Select(entry => new FileSystemEntry { Path = entry, IsDirectory = IsDirectory(entry) });

            static Boolean IsDirectory(String path) => File
                .GetAttributes(path)
                .HasFlag(FileAttributes.Directory);

            static IList<Level> FlattenItemRecursively(IEnumerable<FileSystemEntry> entries, Int32 currentLevel) =>
                entries
                    .Select(entry => new Level { LevelCount = currentLevel, Entry = entry })
                    .Concat(entries
                        .SelectMany(entry => FlattenItemRecursively(entry.IsDirectory ? GetFileSystemEntries(entry.Path) : new List<FileSystemEntry>(), currentLevel +1)))
                    .ToList();
        }

        #endregion

        #region :: EventEmitter ::

        public interface IEvent {
            void OnRaised(Object sender, EventArgs args);
        }

        public sealed class DynamicEventArgs : EventArgs {
            public DynamicEventArgs(dynamic value) {
                Value = value ?? throw new ArgumentNullException(nameof(value));
            }

            public dynamic Value { get; }
        }

        public sealed class DynamicEvent : IEvent {
            readonly Action<Object, DynamicEventArgs> callback;

            public DynamicEvent(Action<Object, DynamicEventArgs> callback) {
                this.callback = callback ?? throw new ArgumentNullException(nameof(callback));
            }

            public void OnRaised(Object sender, EventArgs args) => callback(sender, args as DynamicEventArgs);
        }

        public sealed class SimpleEventArgs : EventArgs {
            public SimpleEventArgs(Object value) {
                Value = value ?? throw new ArgumentNullException(nameof(value));
            }

            public Object Value { get; }
        }

        public sealed class SimpleEvent : IEvent {
            readonly Action<Object, SimpleEventArgs> callback;

            public SimpleEvent(Action<Object, SimpleEventArgs> callback) {
                this.callback = callback ?? throw new ArgumentNullException(nameof(callback));
            }

            public void OnRaised(Object sender, EventArgs args) => callback(sender, args as SimpleEventArgs);
        }

        public interface IEventStorage {
            IList<IEvent> Get(String name);

            void Add(String name, params IEvent[] events);

            void Remove(String name, params IEvent[] events);

            void RemoveAll(String name);

            void Clear();
        }

        public sealed class MemoryEventStorage : IEventStorage {
            readonly IDictionary<String, IList<IEvent>> storage = new Dictionary<String, IList<IEvent>>();

            public IList<IEvent> Get(String name) {
                if (String.IsNullOrEmpty(name))
                    throw new ArgumentNullException(nameof(name));

                if (!storage.ContainsKey(name))
                    return new List<IEvent>();

                return storage[name];
            }

            public void Add(String name, params IEvent[] events) {
                if (String.IsNullOrEmpty(name))
                    throw new ArgumentNullException(nameof(name));

                if (events == null)
                    throw new ArgumentNullException(nameof(events));

                if (events.Any(@event => @event == null))
                    throw new ArgumentOutOfRangeException(nameof(events));

                foreach (IEvent @event in events)
                    Add(name, @event);
            }

            void Add(String name, IEvent @event) {
                IList<IEvent> stored = !storage.ContainsKey(name) ?
                    new List<IEvent>() :
                    storage[name];

                if (stored.Contains(@event))
                    return;

                stored.Add(@event);
                storage[name] = stored;
            }

            public void Remove(String name, params IEvent[] events) {
                if (String.IsNullOrEmpty(name))
                    throw new ArgumentNullException(nameof(name));

                if (events == null)
                    throw new ArgumentNullException(nameof(events));

                if (events.Any(@event => @event == null))
                    throw new ArgumentOutOfRangeException(nameof(events));

                foreach (IEvent @event in events)
                    Remove(name, @event);
            }

            void Remove(String name, IEvent @event) {
                IList<IEvent> stored = !storage.ContainsKey(name) ?
                    new List<IEvent>() :
                    storage[name];

                if (!stored.Contains(@event))
                    return;

                stored.Remove(@event);
                storage[name] = stored;
            }

            public void RemoveAll(String name) {
                if (String.IsNullOrEmpty(name))
                    throw new ArgumentNullException(nameof(name));

                if (storage.ContainsKey(name))
                    storage.Remove(name);
            }

            public void Clear() => storage.Clear();
        }

        public sealed class PatchableConfigEventStorage : IEventStorage {
            readonly IDictionary<String, IList<IEvent>> storage = new Dictionary<String, IList<IEvent>>();

            public PatchableConfigEventStorage() {
                IList<PatchableEvent> events = PatchableConfig.Events;
                foreach (PatchableEvent @event in events) {
                    //MethodInfo eventInfo = 
                    //Add(@event.Event, new SimpleEvent());
                }
            }

            public IList<IEvent> Get(String name) {
                if (String.IsNullOrEmpty(name))
                    throw new ArgumentNullException(nameof(name));

                if (!storage.ContainsKey(name))
                    return new List<IEvent>();

                return storage[name];
            }

            public void Add(String name, params IEvent[] events) {
                if (String.IsNullOrEmpty(name))
                    throw new ArgumentNullException(nameof(name));

                if (events == null)
                    throw new ArgumentNullException(nameof(events));

                if (events.Any(@event => @event == null))
                    throw new ArgumentOutOfRangeException(nameof(events));

                foreach (IEvent @event in events)
                    Add(name, @event);
            }

            void Add(String name, IEvent @event) {
                IList<IEvent> stored = !storage.ContainsKey(name) ?
                    new List<IEvent>() :
                    storage[name];

                if (stored.Contains(@event))
                    return;

                stored.Add(@event);
                storage[name] = stored;
            }

            public void Remove(String name, params IEvent[] events) {
                if (String.IsNullOrEmpty(name))
                    throw new ArgumentNullException(nameof(name));

                if (events == null)
                    throw new ArgumentNullException(nameof(events));

                if (events.Any(@event => @event == null))
                    throw new ArgumentOutOfRangeException(nameof(events));

                foreach (IEvent @event in events)
                    Remove(name, @event);
            }

            void Remove(String name, IEvent @event) {
                IList<IEvent> stored = !storage.ContainsKey(name) ?
                    new List<IEvent>() :
                    storage[name];

                if (!stored.Contains(@event))
                    return;

                stored.Remove(@event);
                storage[name] = stored;
            }

            public void RemoveAll(String name) {
                if (String.IsNullOrEmpty(name))
                    throw new ArgumentNullException(nameof(name));

                if (storage.ContainsKey(name))
                    storage.Remove(name);
            }

            public void Clear() => storage.Clear();
        }

        public class EventEmitter {
            readonly IEventStorage eventStorage;

            public EventEmitter(IEventStorage eventStorage) {
                this.eventStorage = eventStorage ?? throw new ArgumentNullException(nameof(eventStorage));
            }

            public void Emit(String name) {
                
            }

            public void On(String name, Action<Object, EventArgs> callback) {
                
            }

            public void Off() {
                
            }

            public void Once() {
                
            }
        }

        #endregion
    }

    

    

    

    

    class LoginProcessEventArgs : EventArgs {
        public String Username { get; }
        public String UserId { get; }
    }

    //class LoginProcessEvent : IEvent<LoginProcessEventArgs> {
    //    public void OnRaised(Object sender, LoginProcessEventArgs args) {
    //        Console.Write($"{this.GetType().FullName} called.");
    //        Console.Write($"{this.GetType().FullName} processing.");
    //    }
    //}

    //interface IEvent {
    //    void OnRaised(Object sender, String jsonArgs);
    //}

    //class LoginProcessEventCustom : IEvent {
    //    public void OnRaised(Object sender, String jsonArgs) {
    //        Console.Write($"{this.GetType().FullName} called.");
    //        Console.Write($"{this.GetType().FullName} processing.");
    //    }
    //}
}
