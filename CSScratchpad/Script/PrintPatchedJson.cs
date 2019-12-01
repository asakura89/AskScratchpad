using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Scratch;

namespace CSScratchpad.Script {
    public class PrintPatchedJson : Common, IRunnable {
        public void Run() {
            var sampleData = JToken.Parse(File.ReadAllText(GetDataPath("sample-data.json")));
            Dbg("Root children count", sampleData.Children().Count());
            Dbg("Root child with key: config.var", sampleData["config.var"]);
            Dbg("Root child with key: myvar", sampleData["myvar"]);

            IDictionary<String, String> vars = sampleData["config.var"]
                .ToDictionary(
                    token => token["name"].Value<String>(),
                    token => token["value"].Value<String>());

            Dbg("Dictionary", vars);

            String patchedJson = StringExt.ReplaceWith(File.ReadAllText(GetDataPath("sample-config.json")), vars);
            var sampleConfig = JToken.Parse(patchedJson);
            Dbg(sampleConfig["WebApp"]);
        }

        #region :: VaryaExt ::

        public static class StringExt {
            public static String ReplaceWith(String string2Replace, IDictionary<String, String> replacements) =>
            Regex.Replace(
                string2Replace,
                @"\$\{(?<key>[a-zA-Z0-9\\.\\-\\_]*)(:(?<value>[a-zA-Z0-9\\.\\-\\_]*))?\}",
                match => HandleRegex(match, replacements),
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);

            static String HandleRegex(Match match, IDictionary<String, String> replacements) {
                String key = match.Groups["key"].Value;
                String value = match.Groups["value"].Value;
                if (!String.IsNullOrEmpty(key)) {
                    if (!(value == null || value.All(Char.IsWhiteSpace))) {
                        switch (key) {
                            case "config":
                                return HandleConfig(value, replacements);
                            case "econfig":
                                return HandleEncryptedConfig(value, replacements);
                            case "var":
                                return HandleVar(value);
                            default:
                                return String.Empty;
                        }
                    }

                    if (replacements == null)
                        return match.Value;

                    if (!replacements.Any())
                        return match.Value;

                    return replacements.ContainsKey(key) ? replacements[key] : String.Empty;
                }

                return match.Value;
            }

            static String HandleConfig(String configKey, IDictionary<String, String> replacements) => StringExt.ReplaceWith(ConfigurationManager.AppSettings.Get(configKey) ?? String.Empty, replacements);

            static String HandleEncryptedConfig(String configKey, IDictionary<String, String> replacements) {
                String config = HandleConfig(configKey, replacements);
                return TestVarya.SecurityExt.Decrypt(config);
            }

            static String HandleVar(String var) {
                var variableRegexes = new Dictionary<String, String> {
                    ["Timespan"] = @"(?<digit>\d{1,})(?<type>[Dd]|[Hh]|[Mm]|[Ss])",
                    ["Datetime"] = @"[Dd]ate[Tt]ime\((?<dtformat>\w*)\)"
                };

                var variableHandlers = new Dictionary<String, Func<Match, String>> {
                    ["Timespan"] = HandleTimespan,
                    ["Datetime"] = HandleDatetime
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
        }

        #endregion
    }
}
