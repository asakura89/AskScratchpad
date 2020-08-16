using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using Scratch;

namespace CSScratchpad.Script {
    public class HostAWcf : Common, IRunnable {
        public void Run() {
            // custom binding
            var b = new CustomBinding();
            var msgEncoder = new TextMessageEncodingBindingElement { MessageVersion = MessageVersion.None };
            //var http = new HttpsTransportBindingElement();
            var http = new HttpTransportBindingElement();
            b.Elements.Add(msgEncoder);
            b.Elements.Add(http);

            var sh = new ServiceHost(typeof(SimpleHTTPService));
            ServiceEndpoint se = sh.AddServiceEndpoint(typeof(SimpleHTTPService), b, "http://localhost:5004/testHTTP");
            //ServiceEndpoint se = sh.AddServiceEndpoint(typeof(SimpleHTTPService), b, "http://localhost:5004/");
            sh.Open();

            Console.WriteLine("Simple HTTP Service");
            Console.WriteLine("Press key to stop service");
            Console.ReadLine();
        }

        [ServiceContract]
        public class SimpleHTTPService {
            [OperationContract(Action = "*", ReplyAction = "*")]
            Message GetAllUris(Message msg) {
                HttpRequestMessageProperty httpProps;
                String propName;
                propName = HttpRequestMessageProperty.Name;
                httpProps = msg.Properties[propName] as HttpRequestMessageProperty;
                String uri;
                uri = msg.Headers.To.AbsolutePath;
                Console.WriteLine("Request to {0}", uri);
                if (httpProps.Method != "GET")
                    Console.WriteLine("Incoming Message {0} with method of {1}",
                        msg.GetReaderAtBodyContents().ReadOuterXml(),
                        httpProps.Method);
                else
                    Console.WriteLine("GET Request - no message Body");

                //print the query string if any
                if (httpProps.QueryString != null)
                    Console.WriteLine("QueryString = {0}", httpProps.QueryString);

                var response = Message.CreateMessage(MessageVersion.None, "*", "Simple response string");
                var responseProp = new HttpResponseMessageProperty();
                responseProp.Headers.Add("CustomHeader", "Value");
                return response;
            }
        }
    }
}
