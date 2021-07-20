using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using Scratch;

namespace CSScratchpad.Script {
    public class EventHandling : Common, IRunnable {
        public void Run() {
            var classA = new ClassA();
            classA.TriggerStart();
            classA.TriggerFinish();

            classA.TriggerStart();
            classA.TriggerFinish();
        }

        public class ClassA {
            event EventHandler<WithContextEventArgs> ClassAStart;
            event EventHandler<WithContextEventArgs> ClassAFinish;

            public ClassA() {
                var registrant = new XmlConfigEventRegistrant($"{AppDomain.CurrentDomain.BaseDirectory}\\Data\\App_Config\\application-event.xml");
                registrant.Register(this);
            }

            public void TriggerStart() =>
                ClassAStart?.Invoke(this, new WithContextEventArgs(nameof(ClassA.ClassAStart)));

            public void TriggerFinish() =>
                ClassAFinish?.Invoke(this, new WithContextEventArgs(nameof(ClassA.ClassAFinish)));
        }

        public class EventList {
            public void OnClassAStarted(Object source, WithContextEventArgs args) =>
                Console.WriteLine(source.GetType().Name + "+" + nameof(OnClassAStarted));

            public void OnClassAFinished(Object source, WithContextEventArgs args) =>
                Console.WriteLine(source.GetType().Name + "+" + nameof(OnClassAFinished));

            public void OnGlobalStarted(Object source, WithContextEventArgs args) =>
                Console.WriteLine(source.GetType().Name + "+" + nameof(OnGlobalStarted));

            public void OnGlobalFinished(Object source, WithContextEventArgs args) =>
                Console.WriteLine(source.GetType().Name + "+" + nameof(OnGlobalFinished));
        }

        #region : XmlExt :

        public static class XmlExt {
            public static XmlDocument LoadFromPath(String path) {
                if (!File.Exists(path))
                    throw new FileNotFoundException(path);

                String content = File.ReadAllText(path);
                return Load(content);
            }

            public static XmlDocument Load(String xml) {
                if (String.IsNullOrEmpty(xml) || String.IsNullOrEmpty(xml.Trim()))
                    return null;

                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                return xmlDoc;
            }

            public static XmlNode GetAttribute(XmlNode node, String name) {
                if (node != null && node.Attributes != null) {
                    XmlAttribute attr = node.Attributes[name];
                    if (attr != null)
                        return (XmlNode) attr;
                }

                return null;
            }

            public static String GetAttributeValue(XmlNode node, String name) {
                XmlNode attr = GetAttribute(node, name);
                if (attr != null)
                    return attr.Value;

                return String.Empty;
            }
        }

        #endregion

        #region : XmlLoader :

        public interface IEventRegistrant {
            void Register(IEnumerable<EventHandler> handlers);
        }

        public class XmlEventDefinition {
            public String Name { get; set; }
            public Boolean OnlyOnce { get; set; }
            public String Type { get; set; }
            public String Method { get; set; }
        }

        public class WithContextEventArgs : EventArgs {
            public Object Context { get; }

            public WithContextEventArgs(Object context) {
                Context = context;
            }
        }

        public class XmlConfigEventRegistrant {//: IEventRegistrant {
            readonly String configPath;
            readonly IDictionary<String, EventInfo> handlers;

            public XmlConfigEventRegistrant() : this($"{AppDomain.CurrentDomain.BaseDirectory}\\emitter.config.xml") { }

            public XmlConfigEventRegistrant(String configPath) {
                this.configPath = configPath ?? throw new ArgumentNullException(nameof(configPath));
                handlers = new Dictionary<String, EventInfo>();
            }

            public void Register(Object classWithHandlers) {
                Type classWithHandlersType = classWithHandlers.GetType();
                EventInfo[] eventHandlers = classWithHandlersType.GetEvents(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (EventInfo handler in eventHandlers) {
                    String handlerName = classWithHandlersType.Name + ":" + handler.Name;
                    if (handlers.ContainsKey(handlerName))
                        handlers[handlerName] = handler;
                    else
                        handlers.Add(handlerName, handler);
                }

                XmlDocument config = XmlExt.LoadFromPath(configPath);
                String eventsSelector = $"configuration/events";
                XmlNode eventsConfig = config.SelectSingleNode(eventsSelector);
                if (eventsConfig != null) {
                    IEnumerable<XmlEventDefinition> events = eventsConfig
                        .SelectNodes("event")
                        .Cast<XmlNode>()
                        .Select(eventNode => new XmlEventDefinition {
                            Name = XmlExt.GetAttributeValue(eventNode, "name"),
                            OnlyOnce = new Func<String, Boolean>(attrValue => {
                                if (String.IsNullOrEmpty(attrValue))
                                    return false;

                                return Convert.ToBoolean(attrValue);
                            })(XmlExt.GetAttributeValue(eventNode, "onlyOnce")),
                            Type = XmlExt.GetAttributeValue(eventNode, "type"),
                            Method = XmlExt.GetAttributeValue(eventNode, "method")
                        });

                    var typeNameRgx = new Regex("^(?<TypeN>.+)(?:,\\s{1,}?)(?<AsmN>.+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    foreach (XmlEventDefinition definition in events) {
                        String typeName = typeNameRgx.Match(definition.Type).Groups["TypeN"].Value;
                        String asmName = typeNameRgx.Match(definition.Type).Groups["AsmN"].Value;
                        if (String.IsNullOrEmpty(typeName) || String.IsNullOrEmpty(asmName))
                            throw new InvalidOperationException($"{eventsSelector} wrong configuration. {definition.Type}");

                        Assembly asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(appDAsm => appDAsm.GetName().Name == asmName);
                        if (asm == null)
                            throw new InvalidOperationException($"{eventsSelector} wrong configuration. {definition.Type}");

                        Type type = asm.GetTypes().FirstOrDefault(asmType => asmType.FullName.Replace("+", ".") == typeName);
                        if (type == null)
                            throw new InvalidOperationException($"{eventsSelector} wrong configuration. {definition.Type}");

                        Object instance = Activator.CreateInstance(type);
                        MethodInfo methodInfo = instance.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault(method => method.Name == definition.Method);
                        if (methodInfo == null)
                            throw new InvalidOperationException($"{eventsSelector} wrong configuration. {definition.Method}");

                        /*
                        //var eventDelegate = (Action<Object, WithContextEventArgs>) Delegate.CreateDelegate(typeof(Action<Object, WithContextEventArgs>), instance, methodInfo);
                        EventInfo handler = handlers[definition.Name];
                        MethodInfo addHandler = handler.AddMethod;
                        var eventDelegate = Delegate.CreateDelegate(handler.EventHandlerType, instance, methodInfo);
                        if (definition.OnlyOnce) {
                            Action<Object, WithContextEventArgs> wrapper = null;
                            wrapper = (source, args) => {
                                Object localEventSource = classWithHandlers;
                                //Delegate localDelegate = eventDelegate;
                                EventInfo localHandler = handler;
                                var localDelegate = Delegate.CreateDelegate(localHandler.EventHandlerType, wrapper.Target, wrapper.Method);
                                localHandler.RemoveMethod.Invoke(localEventSource, new[] { localDelegate});
                                localDelegate.DynamicInvoke(source, args);
                            };
                            var wrapperDelegate = Delegate.CreateDelegate(handler.EventHandlerType, classWithHandlers, wrapper.Method);
                            addHandler.Invoke(classWithHandlers, new[] { wrapperDelegate });
                            // handler.AddEventHandler(classWithHandlers, wrapper); // this needs public Add Event methiod
                        }
                        else
                            addHandler.Invoke(classWithHandlers, new[] { eventDelegate });
                        */

                        EventInfo handler = handlers[definition.Name];
                        var eventDelegate = Delegate.CreateDelegate(handler.EventHandlerType, instance, methodInfo);
                        //var eventDelegateForHandler = Delegate.CreateDelegate(handler.EventHandlerType, this, eventDelegate.Method);

                        MethodInfo addHandler = handler.AddMethod;
                        if (definition.OnlyOnce) {
                            EventHandler<WithContextEventArgs> wrapper = null;
                            Delegate wrapperDelegate = null;
                            wrapper = (source, args) => {
                                Delegate localWrapperDelegate = wrapperDelegate;
                                Object localEventSource = classWithHandlers;
                                EventInfo localHandler = handler;
                                localHandler.RemoveMethod.Invoke(localEventSource, new[] { localWrapperDelegate });
                                Delegate localDelegate = eventDelegate;
                                //handler.RaiseMethod.Invoke(localEventSource, new[] { source, args });
                                localDelegate.DynamicInvoke(source, args);
                            };
                            wrapperDelegate = Delegate.CreateDelegate(handler.EventHandlerType, wrapper.Target, wrapper.Method);
                            addHandler.Invoke(classWithHandlers, new[] { wrapperDelegate });
                        }
                        else
                            addHandler.Invoke(classWithHandlers, new[] { eventDelegate });
                    }
                }
            }
        }

        #endregion
    }
}