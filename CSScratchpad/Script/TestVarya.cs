using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Scratch;

namespace CSScratchpad.Script {
    class TestVarya : Common, IRunnable {
        public void Run() {
            String template = @"
                <div class='weather-widget'>
                    <div class='weather-widget__content'>
                        <span class='weather-widget__day'>${Day}</span>
                        <span class='weather-widget__date'>${Date}</span>
                    </div>
                    <div class='weather-widget__content'>
                        <div class='weather-widget__temp'>
                            <i class='icon-weather-sun-cloud'></i>
                            <span class='weather-widget__temp__deg'>
                                <a href='${ForecastUrl}' target='_blank'>
                                    <span class='weather-widget__temp__deg__max'>${LowForcastedTemp}</span><span> - ${HighForecastedTemp} &deg;C</span>
                                </a>
                            </span>
                        </div>
                        <div class='weather-widget__psi'>
                            <span class='weather-widget__psi__info'>24Hr PSI</span>
                            <span class='weather-widget__psi__reading'>
                                <a href='${HazeUrl}' target='_blank'>
                                    ${LowForecastedPollutantIndex}-${HighForecastedPollutantIndex}PSI
                                </a>
                            </span>
                        </div>
                    </div>
                </div>";

            IDictionary<String, String> replacements = new Dictionary<String, String> {
                ["Day"] = new Func<String>(() => DateTime.Now.DayOfWeek.ToString())(),
                ["Date"] = new Func<String>(() => DateTime.Now.ToString("dd MMMM yyyy"))(),
                ["ForecastUrl"] = "https://weather-info.fake-api.io",
                ["LowForcastedTemp"] = "27",
                ["HighForecastedTemp"] = "36",
                ["HazeUrl"] = "https://haze-info.fake-api.io",
                ["LowForecastedPollutantIndex"] = "11.7",
                ["HighForecastedPollutantIndex"] = "13.0"
            };

            Console.WriteLine(template.ReplaceWith(replacements));
        }
    }

    #region : VaryaExt :

    public static class VaryaExt {
        public static String ReplaceWith(this String string2Replace, IDictionary<String, String> replacements) {
            if (replacements != null && replacements.Any()) {
                return Regex.Replace(string2Replace, "\\$\\{(?<key>\\w*)\\}", match => {
                    if (match.Success) {
                        String key = match.Groups["key"].Value;
                        if (replacements.ContainsKey(key))
                            return replacements[key];

                        return key;
                    }

                    return String.Empty;
                });
            }

            return string2Replace;
        }
    }

    #endregion
}
