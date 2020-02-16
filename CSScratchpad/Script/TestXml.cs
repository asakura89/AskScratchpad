using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Scratch;

namespace CSScratchpad.Script {
    class TestXml : Common, IRunnable {
        public void Run() {
            XmlDocument xmlDocFromPath = LoadFromPath(GetDataPath("ProtectedInnerData\\AdminLevel1\\data-31.xml"));

            String content = File.ReadAllText(GetDataPath("ProtectedInnerData\\AdminLevel1\\data-32.xml"));
            XmlDocument xmlDoc = Load(content);

            XmlNode root = xmlDoc.DocumentElement;

            Dbg("Xml", new {
                LoadFromPath = xmlDocFromPath,
                Load = xmlDoc,
                Root = root,
                RootName = root.Name,
                SelectSingleNode = root.SelectSingleNode("compositionRoot/singleton"),
                SelectTheOnlyNode = root.SelectSingleNode("compositionRoot/singleton[1]"),
                SelectNodes = root.SelectNodes("compositionRoot"),
                SelectNonExistent = root.SelectSingleNode("compositionRoot/singlet"),
                Select2ndNode = root.SelectSingleNode("compositionRoot/transient[2]"),
                SelectByAttribute = root.SelectSingleNode("compositionRoot/transient[@type='CSScratchpad.Others.Task.ITaskService, CSScratchpad']"),
                GetAttribute = GetAttribute(root.SelectSingleNode("compositionRoot/singleton"), "type"),
                GetAttributeValue = GetAttributeValue(root.SelectSingleNode("compositionRoot/singleton"), "type")
            });

            XmlNodeList nodes = root.SelectNodes("compositionRoot/*");
            var typeNResolveList = new List<Object>();
            foreach (Object node in nodes) {
                var xmlN = node as XmlNode;
                String type = GetAttributeValue(xmlN, "type");
                String resolve = GetAttributeValue(xmlN, "resolve");

                typeNResolveList.Add(new {
                    Type = Type.GetType(type),
                    Resolve = Type.GetType(resolve)
                });
            }

            Dbg("TypeInfo", typeNResolveList);
        }

        XmlDocument LoadFromPath(String path) {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);

            String content = File.ReadAllText(path);
            return Load(content);
        }

        XmlDocument Load(String xml) {
            if (String.IsNullOrEmpty(xml))
                return null;

            if (String.IsNullOrEmpty(xml.Trim()))
                return null;

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return xmlDoc;
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
    }
}
