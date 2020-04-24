using System;

namespace CSScratchpad.Others.Duck {
    public class MallardDuck : IDuck {
        public void Fly() => Console.WriteLine($"{nameof(MallardDuck)} Flying.");

        public void Quack() => Console.WriteLine($"{nameof(MallardDuck)} Quacking.");
    }
}
