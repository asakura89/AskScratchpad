using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using Newtonsoft.Json;
using Scratch;
using Formatting = Newtonsoft.Json.Formatting;

namespace CSScratchpad.Script {
    public class ApplicationPipeline : Common, IRunnable {
        public class XmlPipelinesDefinition {
            public String Name { get; }
            public IList<XmlPipelineActionDefinition> Actions { get; }

            public XmlPipelinesDefinition(String name, IList<XmlPipelineActionDefinition> actions) {
                if (String.IsNullOrEmpty(name))
                    throw new ArgumentNullException(nameof(name));

                if (actions == null || !actions.Any())
                    throw new ArgumentNullException(nameof(actions));

                Name = name;
                Actions = actions;
            }
        }

        public class XmlPipelineActionDefinition {
            public String Type { get; }
            public String Assembly { get; }
            public String Method { get; }

            public XmlPipelineActionDefinition(String type, String assembly, String method) {
                if (String.IsNullOrEmpty(type))
                    throw new ArgumentNullException(nameof(type));

                if (String.IsNullOrEmpty(assembly))
                    throw new ArgumentNullException(nameof(assembly));

                if (String.IsNullOrEmpty(method))
                    throw new ArgumentNullException(nameof(method));

                Type = type;
                Assembly = assembly;
                Method = method;
            }
        }

        XmlPipelineActionDefinition MapConfigToActionDefinition(XmlNode actionConfig) {
            String typeValue = GetAttributeValue(actionConfig, "type");
            String methodValue = GetAttributeValue(actionConfig, "method");
            var typeNameRgx = new Regex("^(?<TypeN>.+)(?:,\\s{1,}?)(?<AsmN>.+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
            String typeName = typeNameRgx.Match(typeValue).Groups["TypeN"].Value;
            String asmName = typeNameRgx.Match(typeValue).Groups["AsmN"].Value;
            if (String.IsNullOrEmpty(typeName) || String.IsNullOrEmpty(asmName))
                throw new InvalidOperationException($"Wrong Type or Assembly configuration. {typeValue}.");

            if (String.IsNullOrEmpty(methodValue))
                throw new InvalidOperationException($"Wrong Method configuration. {methodValue}.");

            return new XmlPipelineActionDefinition(typeName, asmName, methodValue);
        }

        XmlPipelinesDefinition MapConfigToPipelineDefinition(XmlNode pipelineConfig) {
            IList<XmlPipelineActionDefinition> actions =
                pipelineConfig
                    .SelectNodes("pipe")
                    .Cast<XmlNode>()
                    .Select(MapConfigToActionDefinition)
                    .ToList();
            
            return new XmlPipelinesDefinition(
                GetAttributeValue(pipelineConfig, "name"),
                actions);
        }

        public void Run() {
            String configPath = GetDataPath("App_Config\\application-pipeline.xml");
            XmlDocument config = LoadFromPath(configPath);
            String pipelinesSelector = $"configuration/applicationPipeline";
            var pipelinesConfig = config.SelectNodes(pipelinesSelector).Cast<XmlNode>();
            if (pipelinesConfig == null || !pipelinesConfig.Any())
                throw new InvalidOperationException($"{pipelinesSelector} wrong configuration.");

            if (pipelinesConfig != null) {
                IList<XmlPipelinesDefinition> pipelines = pipelinesConfig
                    .Select(MapConfigToPipelineDefinition)
                    .ToList();

                Dbg(pipelines);
                //IEnumerable<(String TypeName, String MethodName)> pipes = pipelineConfig
                //    .SelectNodes("pipe")
                //    .Cast<XmlNode>()
                //    .Select(pipeNode => (TypeName: GetAttributeValue(pipeNode, "type"), MethodName: GetAttributeValue(pipeNode, "method")));

                //var context = new ApplicationStartPipelineContext();
                
                //foreach ((String TypeName, String MethodName) pipe in pipes) {
                    

                //    Assembly asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(appDAsm => appDAsm.GetName().Name == asmName);
                //    if (asm == null)
                //        throw new InvalidOperationException($"{pipelineSelector} wrong configuration. {pipe.TypeName}");

                //    Type type = asm.GetTypes().FirstOrDefault(asmType => asmType.FullName.Replace("+", ".") == typeName);
                //    if (type == null)
                //        throw new InvalidOperationException($"{pipelineSelector} wrong configuration. {pipe.TypeName}");

                //    Object instance = Activator.CreateInstance(type);
                //    MethodInfo methodInfo = instance.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault(method => method.Name == pipe.MethodName);
                //    if (methodInfo == null)
                //        throw new InvalidOperationException($"{pipelineSelector} wrong configuration. {pipe.MethodName}");

                //    methodInfo.Invoke(instance, new[] { context });
                //}
            }
        }

        #region XML Helper

        static XmlDocument LoadFromPath(String path) {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);

            String content = File.ReadAllText(path);
            return Load(content);
        }

        static XmlDocument Load(String xml) {
            if (String.IsNullOrEmpty(xml) || String.IsNullOrEmpty(xml.Trim()))
                return null;

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return xmlDoc;
        }

        static XmlNode GetAttribute(XmlNode node, String name) {
            if (node != null && node.Attributes != null) {
                XmlAttribute attr = node.Attributes[name];
                if (attr != null)
                    return (XmlNode) attr;
            }

            return null;
        }

        static String GetAttributeValue(XmlNode node, String name) {
            XmlNode attr = GetAttribute(node, name);
            if (attr == null)
                return String.Empty;

            return attr.Value;
        }

        #endregion

        #region Custom Code

        public class PipelineContext {
            public IList<ActionResponseViewModel> ActionMessages { get; set; } = new List<ActionResponseViewModel>();
            public dynamic ContextBag { get; set; }
        }

        public class ApplicationStartPipelineContext : PipelineContext {
            public String Username { get; set; }
            public Guid UserId { get; set; }
        }

        public class ApplicationStartPipeline {
            public void LoadConfiguration(ApplicationStartPipelineContext ctx) {
                Console.WriteLine(nameof(LoadConfiguration));
                ctx.ActionMessages.Add(new ActionResponseViewModel {
                    Message = $"{nameof(LoadConfiguration)}: Starting",
                    ResponseType = ActionResponseViewModel.Info
                });
                Console.WriteLine(JsonConvert.SerializeObject(ctx, Formatting.Indented));
                ctx.ActionMessages.Add(new ActionResponseViewModel {
                    Message = $"{nameof(LoadConfiguration)}: Done",
                    ResponseType = ActionResponseViewModel.Success
                });
                Console.WriteLine("Done.");
            }

            public void LoadDatabase(ApplicationStartPipelineContext ctx) {
                Console.WriteLine(nameof(LoadDatabase));
                ctx.ActionMessages.Add(new ActionResponseViewModel {
                    Message = $"{nameof(LoadDatabase)}: Starting",
                    ResponseType = ActionResponseViewModel.Info
                });
                Console.WriteLine(JsonConvert.SerializeObject(ctx, Formatting.Indented));
                ctx.UserId = Guid.NewGuid();
                ctx.Username = "PipelineUser";
                ctx.ActionMessages.Add(new ActionResponseViewModel {
                    Message = $"{nameof(LoadConfiguration)}: Done",
                    ResponseType = ActionResponseViewModel.Success
                });
                Console.WriteLine("Done.");
            }

            public void LoadMaterial(ApplicationStartPipelineContext ctx) {
                Console.WriteLine(nameof(LoadMaterial));
                ctx.ActionMessages.Add(new ActionResponseViewModel {
                    Message = $"{nameof(LoadMaterial)}: Starting",
                    ResponseType = ActionResponseViewModel.Info
                });
                Console.WriteLine(JsonConvert.SerializeObject(ctx, Formatting.Indented));
                ctx.ActionMessages.Add(new ActionResponseViewModel {
                    Message = $"{nameof(LoadMaterial)}: Done",
                    ResponseType = ActionResponseViewModel.Info
                });
                Console.WriteLine("Done.");
            }
        }

        public class MyPipeline {
            public void LogTheApplicationStartPipelineContext(ApplicationStartPipelineContext ctx) {
                Console.WriteLine(nameof(LogTheApplicationStartPipelineContext));
                ctx.ActionMessages.Add(new ActionResponseViewModel {
                    Message = $"{nameof(LogTheApplicationStartPipelineContext)}: Starting",
                    ResponseType = ActionResponseViewModel.Info
                });
                Console.WriteLine("Pipeline Start.");
                Console.WriteLine("Logging Debug.");
                Console.WriteLine(JsonConvert.SerializeObject(ctx, Formatting.Indented));
                ctx.Username = "Custom Pipeline User";
                Console.WriteLine("Doing something.");
                ctx.ActionMessages.Add(new ActionResponseViewModel {
                    Message = $"{nameof(LogTheApplicationStartPipelineContext)}: Error",
                    ResponseType = ActionResponseViewModel.Error
                });
                Console.WriteLine("Pipeline Finish.");
            }
        }

        #endregion
    }
}