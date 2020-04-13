using System;
using Scratch;

namespace CSScratchpad.Script {
    public class PrintJapFL : Common, IRunnable {
        public void Run() {

        }

        abstract class Yokai { }

        class Ayakashi : Yokai { }

        class Mononoke : Yokai { }

        class Mamono : Yokai { }

        class Oni : Yokai { }

        abstract class Yurei : Yokai { }

        interface IShapeShifter {
            void ShapeShift();
        }

        class Obake : Yokai, IShapeShifter {
            public void ShapeShift() { }
        }

        class AnimalFeature {
            public AnimalFeature(String desc) { }
        }

        interface IPossesAnimalFeatures {
            AnimalFeature[] Features { get; set; }
        }

        class Kappa : Yokai, IPossesAnimalFeatures {
            public AnimalFeature[] Features { get; set; }

            public Kappa() {
                Features = new[] {
                    new AnimalFeature("Turtle exoskeleton")
                };
            }
        }

        class Tengu : Yokai, IPossesAnimalFeatures {
            public AnimalFeature[] Features { get; set; }

            public Tengu() {
                Features = new[] {
                    new AnimalFeature("Left wing"),
                    new AnimalFeature("Right wing")
                };
            }
        }
    }
}
