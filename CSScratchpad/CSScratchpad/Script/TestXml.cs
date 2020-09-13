using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Scratch;

namespace CSScratchpad.Script {
    class TestXml : Common, IRunnable {
        public void Run() {
            XmlDocument xmlDocFromPath = LoadFromPath(GetDataPath("ProtectedInnerData\\AdminLevel1\\data-31.xml"));

            String content = File.ReadAllText(GetDataPath("ProtectedInnerData\\AdminLevel1\\data-32.xml"));
            XmlDocument xmlDoc = Load(content);

            XmlNode root = xmlDoc.DocumentElement;

            XmlDocument anotherXmlDocFromPath = LoadFromPath(GetDataPath("ProtectedInnerData\\AdminLevel1\\data-33.xml"));

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
                GetAttributeValue = GetAttributeValue(root.SelectSingleNode("compositionRoot/singleton"), "type"),
                AnotherXmlDocFromPath = anotherXmlDocFromPath
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

            RunDbgXml();
        }

        // Still cn't serialize
        void RunDbgXml() {
            DbgXml("Start", "-");
            //IList<dynamic> hashCodes = new[] {
            //        (dynamic) new { Name = nameof(MallardDuck), HashCode = new MallardDuck().GetHashCode() },
            //        new { Name = nameof(TaskModel), HashCode = new TaskModel().GetHashCode() },
            //        new { Name = nameof(WildTurkey), HashCode = new WildTurkey().GetHashCode() },
            //        new { Name = nameof(A), HashCode = new A().GetHashCode() },
            //        new { Name = nameof(B), HashCode = new B().GetHashCode() },
            //        new { Name = nameof(C), HashCode = new C().GetHashCode() },
            //        new { Name = nameof(DdfConfig), HashCode = new DdfConfig().GetHashCode() },
            //        new { Name = nameof(MustCheckStructure), HashCode = new MustCheckStructure().GetHashCode() },
            //        new { Name = nameof(MustCheckFile), HashCode = new MustCheckFile().GetHashCode() },
            //        new { Name = nameof(Student), HashCode = new Student().GetHashCode() },
            //        new { Name = nameof(User), HashCode = new User().GetHashCode() },
            //        new { Name = nameof(TableStyle), HashCode = new TableStyle().GetHashCode() }
            //    }
            //    .ToList();

            //DbgXml("List", hashCodes);
            //DbgXml("Sorted", hashCodes
            //    .Select(item => new { item.HashCode, item.HashCode.ToString().Length })
            //    .OrderBy(item => item.HashCode)
            //    .ToList());
            DbgXml("Done", "-");

            //DbgXml((dynamic) new {
            //    MallardDuck = new MallardDuck(),
            //    TaskModel = new TaskModel(),
            //    WildTurkey = new WildTurkey(),
            //    A = new A(),
            //    B = new B(),
            //    C = new C(),
            //    DdfConfig = new DdfConfig(),
            //    MustCheckStructure = new MustCheckStructure(),
            //    MustCheckFile = new MustCheckFile(),
            //    Student = new Student(),
            //    User = new User(),
            //    TableStyle = new TableStyle()
            //});

            DbgXml(new MallardDuck());
            DbgXml(new TaskModel());
            DbgXml(new WildTurkey());
            DbgXml(new A());
            DbgXml(new B());
            DbgXml(new C());
            DbgXml(new DdfConfig());
            DbgXml(new MustCheckStructure());
            DbgXml(new MustCheckFile());
            DbgXml(new Student());
            DbgXml(new User());
            DbgXml(new TableStyle());
        }

        void DbgXml(Object obj) => DbgXml(String.Empty, obj);

        void DbgXml(String title, Object obj) {
            DisplayTitle(title);

            if (!(obj is String && String.IsNullOrEmpty(obj.ToString()))) {
                Console.WriteLine(Serialize(obj));
                Console.WriteLine();
            }
        }

        String Serialize<T>(T value) {
            if (value == null)
                return String.Empty;

            Type typeofT = typeof(T);
            if (value.GetType().IsGenericType)
                typeofT = value.GetType().GetGenericTypeDefinition();

            var serializer = new XmlSerializer(typeofT);
            var writer = new StringWriter();
            using (var xmlw = XmlWriter.Create(writer)) {
                serializer.Serialize(xmlw, value);
                return writer.ToString();
            }
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

        #region : class :

        class MallardDuck {
            public void Fly() => Console.WriteLine($"{nameof(MallardDuck)} Flying.");

            public void Quack() => Console.WriteLine($"{nameof(MallardDuck)} Quacking.");
        }

        class TaskModel {
            public String Id;
            public String Title;
            public Boolean Done;
        }

        class WildTurkey {
            public void Fly() => Console.WriteLine($"{nameof(WildTurkey)} Flying.");

            public void Gobble() => Console.WriteLine($"{nameof(WildTurkey)} Gobble Gobble.");
        }

        class C : A { }

        class B { }

        class A {
            public DateTime CreatedOn { get; set; }
            public DateTime UpdatedOn { get; set; }
            public String CreatedBy { get; set; }
            public String UpdatedBy { get; set; }
            public String LastModule { get; set; }
            public String LastOperation { get; set; }
            public Boolean IsDeleted { get; set; }
            public Boolean IsActive { get; set; }
        }

        sealed class DdfConfig {
            public IList<MustCheckStructure> MustIncludes { get; set; }
            public IList<MustCheckStructure> MustExcludes { get; set; }
            public IList<MustCheckFile> MustContain { get; set; }
            public IList<MustCheckFile> MustNotContain { get; set; }
        }

        sealed class MustCheckStructure {
            public String Desc { get; set; }
            public String Pattern { get; set; }
        }

        sealed class MustCheckFile {
            public String Desc { get; set; }
            public String FilenamePattern { get; set; }
            public String ContentPattern { get; set; }
        }

        class Student {
            public String Id { get; set; }
            public String Name { get; set; }
        }

        class User {
            public String Id { get; set; }
            public String Username { get; set; }
        }

        class TableStyle {
            public String TopLeftCorner { get; set; }
            public String TopMiddleCorner { get; set; }
            public String TopRightCorner { get; set; }
            public String MiddleLeftCorner { get; set; }
            public String CenterCorner { get; set; }
            public String MiddleRightCorner { get; set; }
            public String BottomLeftCorner { get; set; }
            public String BottomMiddleCorner { get; set; }
            public String BottomRightCorner { get; set; }
            public String HeaderLeftVertical { get; set; }
            public String HeaderMiddleVertical { get; set; }
            public String HeaderRightVertical { get; set; }
            public String HeaderTopHorizontal { get; set; }
            public String HeaderBottomHorizontal { get; set; }
            public String BodyLeftVertical { get; set; }
            public String BodyMiddleVertical { get; set; }
            public String BodyRightVertical { get; set; }
            public String BodyMiddleHorizontal { get; set; }
            public String BodyBottomHorizontal { get; set; }
        }

        #endregion
    }
}
