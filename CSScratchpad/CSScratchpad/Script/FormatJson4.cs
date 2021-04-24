using System;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Scratch;
using JsonFormatting = Newtonsoft.Json.Formatting;

namespace CSScratchpad.Script {
    class FormatJson4 : Common, IRunnable {
        public void Run() {
            String xml = @"<?xml version='1.0' encoding='utf-8'?>
                <storage>
                  <item key='a' type='System.Int32' value='15' />
                  <item key='c' type='System.Double' value='14,6' />
                  <item key='d' type='System.Decimal' value='46,89865' />
                  <item key='b' type='System.String' value='arererere' />
                  <item key='e' type='System.Int64' value='23' />
                  <item key='f' type='System.Int32' value='255879886' />
                  <item key='i' type='System.DateTime' value='20210424T020857Z' />
                  <item key='j' type='System.TimeSpan' value='5.03:20:40' />
                  <item key='g' type='System.String[]'>
                    <item key='String' type='System.String' value='aru' />
                    <item key='String' type='System.String' value='no' />
                    <item key='String' type='System.String' value='ka' />
                    <item key='String' type='System.String' value='na' />
                  </item>
                  <item key='h' type='&lt;&gt;f__AnonymousType0`6[System.String,System.String,System.Double,System.Double,System.Int32,System.String[]]'>
                    <item key='Are' type='System.String' value='arererere' />
                    <item key='Ano' type='System.String' value='anonono' />
                    <item key='X' type='System.Double' value='17,5' />
                    <item key='Y' type='System.Double' value='40' />
                    <item key='Z' type='System.Int32' value='-23' />
                    <item key='Arr' type='System.String[]'>
                      <item key='String' type='System.String' value='aru' />
                      <item key='String' type='System.String' value='no' />
                      <item key='String' type='System.String' value='ka' />
                      <item key='String' type='System.String' value='na' />
                    </item>
                  </item>
                  <item key='k' type='System.Collections.Generic.Dictionary`2[System.String,System.String]'>
                    <item key='key1' type='System.String' value='value1' />
                    <item key='key2' type='System.String' value='value2' />
                  </item>
                  <item key='m' type='System.Collections.Generic.Dictionary`2[System.String,System.Object]'>
                    <item key='key1' type='&lt;&gt;f__AnonymousType1`8[System.String,System.String,System.Double,System.Double,System.Int32,System.String[],System.Object,System.String]'>
                      <item key='Are' type='System.String' value='arererere' />
                      <item key='Ano' type='System.String' value='anonono' />
                      <item key='X' type='System.Double' value='17,5' />
                      <item key='Y' type='System.Double' value='40' />
                      <item key='Z' type='System.Int32' value='-23' />
                      <item key='Arr' type='System.String[]'>
                        <item key='String' type='System.String' value='aru' />
                        <item key='String' type='System.String' value='no' />
                        <item key='String' type='System.String' value='ka' />
                        <item key='String' type='System.String' value='na' />
                      </item>
                      <item key='Ignore' type='System.Object' value='null' />
                      <item key='Emp' type='System.String' value='' />
                    </item>
                    <item key='key2' type='System.String[]'>
                      <item key='String' type='System.String' value='aru' />
                      <item key='String' type='System.String' value='no' />
                      <item key='String' type='System.String' value='ka' />
                      <item key='String' type='System.String' value='na' />
                    </item>
                  </item>
                  <item key='l' type='System.Collections.Hashtable'>
                    <item key='o' type='System.Int32' value='723' />
                    <item key='a' type='System.String' value='e' />
                    <item key='e' type='System.String' value='a' />
                  </item>
                </storage>";

            var doc = new XmlDocument();
            doc.LoadXml(xml);

            String json = JsonConvert.SerializeXmlNode(doc, JsonFormatting.Indented);

            Console.WriteLine(json);

            XNode node = JsonConvert.DeserializeXNode(json, "Root");
            Console.WriteLine(node);

            String json2 = @"{
                'storage': [
                    {
                      'item.key': 'a',
                      'item.type': 'System.Int32',
                      'item.value': '15'
                    },
                    {
                      'item.key': 'c',
                      'item.type': 'System.Double',
                      'item.value': '14,6'
                    },
                  ]
                }";

            XNode node2 = JsonConvert.DeserializeXNode(json2, "Root");
            Console.WriteLine(node2);

            Console.ReadLine();
        }
    }
}
