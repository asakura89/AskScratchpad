using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Security.AntiXss;
using Newtonsoft.Json.Linq;
using Scratch;
using Console = System.Console;

namespace CSScratchpad.Script {
    class TestEmitEvents : Common, IRunnable {
        public void Run() {
            try {
                On_EmptyFunc_ShouldContainsOne();
                On_EmptyFuncWithArg_ShouldHaveArgValue();
                Once_EmptyFunc_ShouldContainsOneThenRemoved();
                Once_EmptyFuncWithArg_ShouldContainsOneThenRemoved();
                Emit_DummyFunc_ShouldContainsTwo();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        public void On_EmptyFunc_ShouldContainsOne() {
            var emitter = new Em();
            emitter.On("test", arg => { });
            As.True(emitter.Count == 1);

            emitter.Emit("test", null);
            As.True(emitter.Count == 1);
        }

        public void On_EmptyFuncWithArg_ShouldHaveArgValue() {
            var emitter = new Em();
            emitter.On("test", arg => { As.NotNull(arg); });
            emitter.Emit("test", new EmEventArgs("Context"));
        }

        public void Once_EmptyFunc_ShouldContainsOneThenRemoved() {
            var emitter = new Em();
            emitter.Once("test", arg => { });
            As.True(emitter.Count == 1);

            emitter.Emit("test", null);
            As.True(emitter.Count == 0);
        }

        public void Once_EmptyFuncWithArg_ShouldContainsOneThenRemoved() {
            var emitter = new Em();
            emitter.Once("test", arg => { As.NotNull(arg); });
            As.True(emitter.Count == 1);

            emitter.Emit("test", new EmEventArgs("Context"));
            As.True(emitter.Count == 0);
        }

        public void Emit_DummyFunc_ShouldContainsTwo() {
            var emitter = new Em();
            emitter.On("test", arg => {
                Console.WriteLine("Start.");
                Thread.Sleep(3000);
                Console.WriteLine(arg.Context);
                Thread.Sleep(3000);
                Console.WriteLine("Done.");
            });

            emitter.On("test-2", arg => {
                Console.WriteLine("Start Logging.");
                Thread.Sleep(1000);
                Console.WriteLine(arg.Context);
                Thread.Sleep(1000);
                Console.WriteLine("Done Logging.");
            });

            As.True(emitter.Count == 2);

            emitter.Emit("test", new EmEventArgs("Context"));
            As.True(emitter.Count == 2);

            emitter.Emit("test-2", new EmEventArgs("Arg"));
            As.True(emitter.Count == 2);
        }

        public void Emit_DummyFuncWithComplexArg_ShouldContainsOne() {
            var emitter = new Em();
            emitter.On("test", arg => {
                Console.WriteLine("Start.");
                Thread.Sleep(3000);
                Console.WriteLine(arg.Context);
                Thread.Sleep(3000);
                Console.WriteLine("Done.");
            });

            emitter.On("test", arg => {
                Console.WriteLine("Start Logging.");
                Thread.Sleep(1000);
                Console.WriteLine(arg.Context);
                Thread.Sleep(1000);
                Console.WriteLine("Done Logging.");
            });

            As.True(emitter.Count == 1);

            emitter.Emit("test", new EmEventArgs(new {  }));
            As.True(emitter.Count == 1);
        }

        #region :: Event Emitter ::

        public class EmException : Exception {
            public EmException(String message) : base(message) { }

            public EmException(String message, Exception innerException) : base(message, innerException) { }
        }

        public class EmEventArgs : EventArgs {
            public Object Context { get; }

            public EmEventArgs(Object context) {
                Context = context;
            }
        }

        public class Em {

            readonly Dictionary<String, IList<Action<EmEventArgs>>> e = new Dictionary<String, IList<Action<EmEventArgs>>>();

            public Int32 Count => e.Count;

            public Em On(String name, Action<EmEventArgs> callback) {
                if (String.IsNullOrEmpty(name))
                    throw new EmException("Name must be specified.");

                if (callback == null)
                    throw new EmException("Invalid callback.");

                if (!e.ContainsKey(name))
                    e.Add(name, new List<Action<EmEventArgs>>());

                e[name].Add(callback);

                return this;
            }

            public Em Off(String name) => Off(name, null);

            public Em Off(String name, Action<EmEventArgs> callback) {
                if (String.IsNullOrEmpty(name))
                    throw new EmException("Name must be specified.");

                if (!e.ContainsKey(name))
                    return this;

                if (callback == null) {
                    e.Remove(name);
                    return this;
                }

                IList<Action<EmEventArgs>> callbacks = e[name];
                IList<Action<EmEventArgs>> liveCallbacks = callbacks
                    .Where(callb => !callb.Equals(callback))
                    .ToList();

                if (liveCallbacks.Any())
                    e[name] = liveCallbacks;
                else
                    e.Remove(name);

                return this;
            }

            public Em Once(String name, Action<EmEventArgs> callback) {
                if (String.IsNullOrEmpty(name))
                    throw new EmException("Name must be specified.");

                if (callback == null)
                    throw new EmException("Invalid callback.");

                Em self = this;
                Action<EmEventArgs> wrapper = null;
                wrapper = arg =>
                {
                    self.Off(name, wrapper);
                    callback.Invoke(arg);
                };

                return self.On(name, wrapper);
            }

            public Em Emit(String name, EmEventArgs arg) {
                if (String.IsNullOrEmpty(name))
                    throw new EmException("Name must be specified.");

                if (!e.ContainsKey(name))
                    return this;

                IList<Action<EmEventArgs>> callbacks = e[name];
                foreach (Action<EmEventArgs> callback in callbacks)
                    callback.Invoke(arg);

                return this;
            }
        }

        #endregion

        #region :: Assert ::

        public class AsException : Exception {
            public AsException(String methodName, String message) : this($"[{methodName}]: {message}") { }

            public AsException(String message) : base(message) { }

            public AsException(String methodName, String message, Exception innerException) : this($"[{methodName}]: {message}", innerException) { }

            public AsException(String message, Exception innerException) : base(message, innerException) { }
        }

        public static class As {
            public static void Null(Object actual) {
                if (actual != null)
                    throw new AsException(nameof(Null), "Actual object was not null");
            }

            public static void NotNull(Object actual) {
                if (actual == null)
                    throw new AsException(nameof(NotNull), "Actual object was null.");
            }

            public static void True(Boolean actual) {
                if (!actual)
                    throw new AsException(nameof(True), "Actual value was false.");
            }

            public static void False(Boolean actual) {
                if (actual)
                    throw new AsException(nameof(True), "Actual value was true.");
            }
        }

        #endregion
    }
}
