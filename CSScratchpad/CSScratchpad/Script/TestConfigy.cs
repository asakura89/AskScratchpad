using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Configy.Containers;
using Configy.Parsing;
using Scratch;

namespace CSScratchpad.Script {
    class TestConfigy : Common, IRunnable {
        public void Run() {
            XmlNode configNode = LoadFromPath(GetDataPath("ProtectedInnerData\\AdminLevel1\\data-3e.xml"));

            //var configNode = 
            var parser = new XmlContainerParser(configNode["configurations"], configNode["compositionRoot"], new XmlInheritanceEngine());
            IList<ContainerDefinition> definitions = parser.GetContainers();
            IContainer[] containers = GetContainers(definitions).ToArray();

            Dbg(
                containers
                    .Select(cont => cont.Name)
            );

            var a = containers.Select(cont => cont.Resolve(typeof(IRunnable))).First();
            Console.Write("");
        }

        #region :: Taken from Unicorn / XmlContainerBuilder ::

        IEnumerable<IContainer> GetContainers(IEnumerable<ContainerDefinition> definitions) {
            foreach (ContainerDefinition definition in definitions) {
                if (definition.Abstract)
                    continue;

                yield return GetContainer(definition);
            }
        }

        readonly IContainerDefinitionVariablesReplacer _variablesReplacer;

        IContainer GetContainer(ContainerDefinition definition) {
            _variablesReplacer?.ReplaceVariables(definition);

            IContainer container = CreateContainer(definition);
            foreach (XmlElement dependency in definition.Definition.ChildNodes.OfType<XmlElement>()) {
                RegisterConfigTypeInterfaces(dependency, container);
            }

            return container;
        }

        IContainer CreateContainer(ContainerDefinition definition) => new MicroContainer(definition.Name, definition.Extends);

        void RegisterConfigTypeInterfaces(XmlElement dependency, IContainer container) {
            TypeRegistration type = GetConfigType(dependency);
            Type[] interfaces = type.Type.GetInterfaces();
            KeyValuePair<String, Object>[] attributes = GetUnmappedAttributes(dependency);

            foreach (Type @interface in interfaces) {
                RegisterConfigTypeInterface(container, @interface, type, attributes, dependency);
            }
        }

        TypeRegistration GetConfigType(XmlElement dependency) {
            String typeString = GetAttributeValue(dependency, "type");

            Boolean isSingleInstance = Boolean.TrueString.Equals(GetAttributeValue(dependency, "singleInstance"), StringComparison.OrdinalIgnoreCase);

            if (String.IsNullOrEmpty(typeString)) {
                throw new InvalidOperationException($"Missing type attribute for dependency '{dependency.Name}'. Specify an assembly-qualified name for your dependency.");
            }

            var type = Type.GetType(typeString, false);

            if (type == null) {
                throw new InvalidOperationException($"Unable to resolve type '{typeString}' on dependency config node '{dependency.Name}'");
            }

            return new TypeRegistration { Type = type, SingleInstance = isSingleInstance };
        }

        protected class TypeRegistration {
            public Type Type { get; set; }
            public Boolean SingleInstance { get; set; }
        }

        KeyValuePair<String, Object>[] GetUnmappedAttributes(XmlElement dependency) {
            // ReSharper disable once PossibleNullReferenceException
            IEnumerable<KeyValuePair<String, Object>> attributes = dependency.Attributes.Cast<XmlAttribute>()
                .Where(attr => attr.Name != "type" && attr.Name != "singleInstance")
                .Select(attr => {
                    // mapping boolean values to bool constructor params
                    Boolean boolean;
                    if (Boolean.TryParse(attr.InnerText, out boolean))
                        return new KeyValuePair<String, Object>(attr.Name, boolean);

                    // mapping integer values to int constructor params
                    Int32 integer;
                    if (Int32.TryParse(attr.InnerText, out integer))
                        return new KeyValuePair<String, Object>(attr.Name, integer);

                    return new KeyValuePair<String, Object>(attr.Name, attr.InnerText);
                });

            // we pass it the XML element as 'configNode'
            attributes = attributes.Concat(new[] { new KeyValuePair<String, Object>("configNode", dependency) });

            return attributes.ToArray();
        }

        void RegisterConfigTypeInterface(IContainer container, Type interfaceType, TypeRegistration implementationRegistration, KeyValuePair<String, Object>[] unmappedAttributes, XmlElement dependency) =>
            container.Register(interfaceType, () => container.Activate(implementationRegistration.Type, unmappedAttributes), implementationRegistration.SingleInstance);

        #endregion

        #region :: Xml helper ::

        XmlNode LoadFromPath(String path) {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);

            String content = File.ReadAllText(path);
            return Load(content);
        }

        XmlNode Load(String xml) {
            if (String.IsNullOrEmpty(xml))
                return null;

            if (String.IsNullOrEmpty(xml.Trim()))
                return null;

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return xmlDoc.FirstChild;
        }

        XmlNode GetAttribute(XmlNode node, String name) {
            if (node != null && node.Attributes != null) {
                XmlAttribute attr = node.Attributes[name];
                if (attr != null)
                    return (XmlNode) attr;
            }

            return null;
        }

        String GetAttributeValue(XmlNode node, String name) {
            XmlNode attr = GetAttribute(node, name);
            if (attr == null)
                return String.Empty;

            return attr.Value;
        }

        #endregion
    }
}
