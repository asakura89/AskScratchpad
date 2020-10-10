using System;
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
        public void Run() {
            String configPath = GetDataPath("App_Config\\application-pipeline.xml");
            XmlDocument config = LoadFromPath(configPath);
            String pipelineSelector = $"configuration/applicationPipeline[@name='pipe:ApplicationStart']";
            XmlNode pipelineConfig = config.SelectSingleNode(pipelineSelector);
            if (pipelineConfig != null) {
                IEnumerable<(String TypeName, String MethodName)> pipes = pipelineConfig
                    .SelectNodes("pipe")
                    .Cast<XmlNode>()
                    .Select(pipeNode => (TypeName: GetAttributeValue(pipeNode, "type"), MethodName: GetAttributeValue(pipeNode, "method")));

                var context = new ApplicationStartPipelineContext();
                var typeNameRgx = new Regex("^(?<TypeN>.+)(?:,\\s{1,}?)(?<AsmN>.+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach ((String TypeName, String MethodName) pipe in pipes) {
                    String typeName = typeNameRgx.Match(pipe.TypeName).Groups["TypeN"].Value;
                    String asmName = typeNameRgx.Match(pipe.TypeName).Groups["AsmN"].Value;
                    if (String.IsNullOrEmpty(typeName) || String.IsNullOrEmpty(asmName))
                        throw new InvalidOperationException($"{pipelineSelector} wrong configuration. {pipe.TypeName}");

                    Assembly asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(appDAsm => appDAsm.GetName().Name == asmName);
                    if (asm == null)
                        throw new InvalidOperationException($"{pipelineSelector} wrong configuration. {pipe.TypeName}");

                    Type type = asm.GetTypes().FirstOrDefault(asmType => asmType.FullName.Replace("+", ".") == typeName);
                    if (type == null)
                        throw new InvalidOperationException($"{pipelineSelector} wrong configuration. {pipe.TypeName}");

                    Object instance = Activator.CreateInstance(type);
                    MethodInfo methodInfo = instance.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault(method => method.Name == pipe.MethodName);
                    if (methodInfo == null)
                        throw new InvalidOperationException($"{pipelineSelector} wrong configuration. {pipe.MethodName}");

                    methodInfo.Invoke(instance, new[] { context });
                }
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