using System;
using System.IO;
using System.Linq;
using System.Text;

//using et = xml.etree.ElementTree;
//using glob = glob.glob;

namespace CSScratchpad.Script {
    public class CombineXml : Common, IRunnable {
        public void Run() {
            
        }
    }

    /*
    public static class create_compendiums {

        public static object COMPENDIUM = "Compendiums/{category} Compendium.xml";

        // Combiner for xml files with multiple way to perform the combining
        public class XMLCombiner
            : object {

            public XMLCombiner(object filenames) {
                Debug.Assert(filenames.Count > 0);
                Debug.Assert("No filenames!");
                this.files = (from f in filenames
                              select this.informed_parse(f)).ToList();
                this.roots = (from f in this.files
                              select f.getroot()).ToList();
            }

            public virtual object informed_parse(object filename) {
                try {
                    return et.parse(filename);
                }
                catch {
                    Console.WriteLine(filename);
                    throw;
                }
            }

            // Combine the xml files and sort the items alphabetically
            // 
            //         Items with the same name are removed.
            // 
            //         :param output: filepath in with the result will be stored.
            // 
            //         
            public virtual object combine_pruned(object output) {
                var items = new List<object>();
                foreach (var r in this.roots) {
                    foreach (var element in r) {
                        var name = element.findtext("name");
                        items.append(Tuple.Create(name, element));
                    }
                }
                items.sort();
                // Include only of of each element with same name
                var elements = (from _tup_1 in items.Select((_p_1, _p_2) => Tuple.Create(_p_2, _p_1)).Chop((i, item) => Tuple.Create(i, item))
                                let i = _tup_1.Item1
                                let item = _tup_1.Item2
                                where !i || item[0] != items[i - 1][0]
                                select item[-1]).ToList();
                Console.WriteLine(String.Format("Removed %d duplicate(s)", items.Count - elements.Count));
                this.roots[0][":"] = elements;
                return this.files[0].write(output, encoding: "UTF-8");
            }

            // Combine the xml files by concating the items
            // 
            //         :param output: filepath in with the result will be stored.
            // 
            //         
            public virtual object combine_concatenate(object output) {
                foreach (var r in this.roots[1]) {
                    this.roots[0].extend(r.getchildren());
                }
                return this.files[0].write(output, encoding: "UTF-8");
            }
        }

        // Create the category compendiums
        // 
        //     :return: list of output paths.
        // 
        //     
        public static object create_category_compendiums() {
            var categories = new List<object> {
            "Items",
            "Character",
            "Spells",
            "Bestiary"
        };
            var output_paths = new List<object>();
            foreach (var category in categories) {
                var filenames = glob(String.Format("%s/*.xml", category));
                var output_path = COMPENDIUM.format(category: category);
                output_paths.append(output_path);
                XMLCombiner(filenames).combine_pruned(output_path);
            }
            return output_paths;
        }

        // Create the category compendiums and combine them into full compendium
        public static object create_full_compendium() {
            var category_paths = create_category_compendiums();
            var full_path = COMPENDIUM.format(category: "Full");
            XMLCombiner(category_paths).combine_concatenate(full_path);
        }

        static create_compendiums() {
            create_full_compendium();
        }
    }
    */
}
