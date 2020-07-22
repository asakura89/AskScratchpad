
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Scratch;

namespace CSScratchpad.Script {
    public class PrintTypeFullName : Common, IRunnable {
        public void Run() {
            Console.Title = "Type dari String";

            dynamic obj1 = new ExpandoObject();
            String[] obj2 = new[] { "", String.Empty };
            var obj3 = new ExpandoObject();
            IDictionary<String, Object> obj4 = new Dictionary<String, Object>();
            IDictionary<string, int> obj5 = new Dictionary<string, int>();

            Console.WriteLine(".:: Type Fullname ::.");
            Console.WriteLine("---------------------");

            new[] {
                    obj1.GetType().FullName,
                    obj2.GetType().FullName,
                    obj3.GetType().FullName,
                    obj4.GetType().FullName,
                    obj5.GetType().FullName
                }
                .Select((item, idx) => $"{idx +1}. {item}")
                .ToList()
                .ForEach(Console.WriteLine);


            Console.WriteLine();
            Console.WriteLine(".:: Type dari string Fullname ::.");
            Console.WriteLine("---------------------------------");

            new[] {
                    obj1.GetType().FullName,
                    obj2.GetType().FullName,
                    obj3.GetType().FullName,
                    obj4.GetType().FullName,
                    obj5.GetType().FullName
                }
                .Select(name => Type.GetType(name))
                .Select(type => type == null ? null : (type.IsArray ? Array.CreateInstance(Type.GetType(type.FullName.Replace("[]", String.Empty)), 10) : Activator.CreateInstance(type)))
                .Select(obj => obj == null ? null : obj.GetType().FullName)
                .Select((item, idx) => $"{idx +1}. {(item == null ? "null" : item)}")
                .ToList()
                .ForEach(Console.WriteLine);

            Console.WriteLine();
            Console.WriteLine(".:: Type dari string ExpandoObject ::.");
            Console.WriteLine("--------------------------------------");

            Console.WriteLine(Type.GetType("System.Dynamic.ExpandoObject, System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089").FullName);

            Console.ReadLine();
        }
    }
}
