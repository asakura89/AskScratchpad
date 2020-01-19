using System;
using System.Collections.Generic;
using Scratch;

namespace CSScratchpad.Script {
    class TestDecoratorPattern : Common, IRunnable {
        public void Run() {
            var component = new Component();
            var mixer = new ComponentMixerDecorator(component);
            var packager = new ComponentPackagerDecorator(mixer);

            /*
            Client.DecorComponent(packager);
            Client.DecorComponent(mixer);
            packager.DottedAddedBehaviour();
            packager.GradientAddedBehaviour();
            */

            var componentList = new List<IComponent>();
            var packagerMyWay = new ComponentPackagerDecoratorMyWay(mixer);
            packagerMyWay.AddBehaviour(new DottedAddedBehaviour());
            packagerMyWay.AddBehaviour(new GradientAddedBehaviour());
            componentList.Add(packagerMyWay);
            ClientMyWay.DecorComponent(componentList);
        }
    }

    #region decorator pattern

    interface IComponent {
        void Operation();
    }

    class Component : IComponent { // class which need extending
        public void Operation() => Console.WriteLine("I'm original component");
    }

    class ComponentMixerDecorator : IComponent {
        IComponent targetComponent;

        public ComponentMixerDecorator(IComponent component) {
            targetComponent = component;
        }

        public void Operation() {
            targetComponent.Operation();
            Console.WriteLine("and I'm being mixed by decorator");
        }
    }

    class ComponentPackagerDecorator : IComponent {
        IComponent targetComponent;

        public ComponentPackagerDecorator(IComponent component) {
            targetComponent = component;
        }

        public void Operation() {
            targetComponent.Operation();
            Console.WriteLine("and I'm being packaged by decorator");
        }

        public void DottedAddedBehaviour() => Console.WriteLine("I'm packaged using dotted wrap paper");

        public void GradientAddedBehaviour() => Console.WriteLine("I'm packaged using gradient wrap paper");
    }

    class Client {
        public static void DecorComponent(IComponent component) {
            component.Operation();
        }
    }

    #endregion

    #region decorator my way

    interface IAddedBehaviour {
        void AddedBehaviour();
    }

    interface IMultipleBehaviour {
        IList<IAddedBehaviour> GetBehaviourList();
        void AddBehaviour(IAddedBehaviour behaviour);
        void AddRangeBehaviour(IEnumerable<IAddedBehaviour> behaviourList);
    }

    class DottedAddedBehaviour : IAddedBehaviour {
        public void AddedBehaviour() => Console.WriteLine("I'm packaged using dotted wrap paper");
    }

    class GradientAddedBehaviour : IAddedBehaviour {
        public void AddedBehaviour() => Console.WriteLine("I'm packaged using gradient wrap paper");
    }

    class ComponentPackagerDecoratorMyWay : IComponent, IMultipleBehaviour {
        readonly IComponent targetComponent;
        IList<IAddedBehaviour> BehaviourList;

        public ComponentPackagerDecoratorMyWay(IComponent component) {
            targetComponent = component;
            BehaviourList = new List<IAddedBehaviour>();
        }

        public void Operation() {
            targetComponent.Operation();
            Console.WriteLine("and I'm being packaged by decorator");
        }

        public IList<IAddedBehaviour> GetBehaviourList() => BehaviourList;

        public void AddBehaviour(IAddedBehaviour behaviour) => BehaviourList.Add(behaviour);

        public void AddRangeBehaviour(IEnumerable<IAddedBehaviour> behaviourList) {
            foreach (IAddedBehaviour behaviour in behaviourList)
                BehaviourList.Add(behaviour);
        }
    }

    class ClientMyWay {
        public static void DecorComponent(List<IComponent> componentList) {
            foreach (IComponent component in componentList) {
                component.Operation();
                if (component is IMultipleBehaviour) {
                    IList<IAddedBehaviour> behaviourList = ((IMultipleBehaviour) component).GetBehaviourList();
                    foreach (IAddedBehaviour behaviour in behaviourList)
                        behaviour.AddedBehaviour();
                }
            }
        }
    }

    #endregion
}