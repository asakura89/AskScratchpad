using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scratch;

namespace CSScratchpad.Script {
    public class PrintLookup : Common, IRunnable {
        public void Run() {
            Console.OutputEncoding = Encoding.UTF8;

            var products = new List<Product> {
                new Product {Code = "SPM1", Description = "Spam"},
                new Product {Code = "SPM2", Description = "Mechanically Separated Chicken"},
                new Product {Code = "LME1", Description = "Bologna"},
                new Product {Code = "SUP1", Description = "Tomato"},
                new Product {Code = "SUP2", Description = "Chicken Noodle"}
            };

            var lookup = (Lookup<String, String>) products
                .ToLookup(c => c.Code.Substring(0, 3),
                    c => $"{c.Code} {c.Description}");

            Console.WriteLine("── Getting all Lookup keys ──");
            lookup
                .Select(grp => grp.Key)
                .ToList()
                .ForEach(key => Console.WriteLine($"• {key}"));

            Console.WriteLine();

            Console.WriteLine("── Getting all Lookup keys ──");
            lookup
                .Select((grp, idx) => new {Value = grp.Key, Idx = idx})
                .ToList()
                .ForEach(key => Console.WriteLine($"   {(lookup.Count -1 == key.Idx ? "└" : "├")}── {key.Value}"));

            Console.WriteLine();

            Console.WriteLine("── Getting all Lookup keys and values ──");
            foreach ((Int32 Idx, IGrouping<String, String> Value) lookupGroup in lookup.Select((grp, idx) => (Idx: idx, Value: grp) )) {
                Console.WriteLine($"• {lookupGroup.Value.Key}");
                foreach ((Int32 Idx, String Value) lookupItem in lookupGroup.Value.Select((val, idx) => (Idx: idx, Value: val)))
                    Console.WriteLine($"  {(lookupGroup.Value.Count() -1 == lookupItem.Idx ? "└" : "├")}── {lookupItem.Value}");

                Console.WriteLine();
            }

            Console.WriteLine("── Get all items from 'SPM' lookup ──");
            lookup["SPM"]
                .ToList()
                .ForEach(str => Console.WriteLine($"• {str}"));
        }
    }

    public class Product {
        public String Code { get; set; }
        public String Description { get; set; }
    }
}
