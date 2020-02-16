using System;

namespace CSScratchpad.Others.Turkey {
    public class WildTurkey : ITurkey {
        public void Fly() => Console.WriteLine($"{nameof(WildTurkey)} Flying.");

        public void Gobble() => Console.WriteLine($"{nameof(WildTurkey)} Gobble Gobble.");
    }
}
