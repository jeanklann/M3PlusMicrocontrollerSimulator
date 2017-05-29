using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator {
    public class Circuit {
        private bool builtCircuit = false;
        private int simulationId = 0;
        private List<Component> starterComponents;
        private Queue<Component> executionQueue;
        private float timePerTick;
        private float time = 0f;
        private int lastId = -1;

        public int SimulationId { get { return simulationId; } }

        public List<Component> Components;
        public float TimePerTick { get { return timePerTick; } }
        public float Time { get { return time; } }


        public Circuit(float timePerTick = 0.01666666666666666666f) {
            Components = new List<Component>();
            starterComponents = new List<Component>();
            executionQueue = new Queue<Component>();
            this.timePerTick = timePerTick;
        }

        protected internal void AddToExecution(Component component) {
            executionQueue.Enqueue(component);
        }
        /// <summary>
        /// Adds a component to the circuit
        /// </summary>
        /// <param name="component">The component</param>
        /// <returns>The same component</returns>
        public T AddComponent<T> (T component) where T : Component {
            if(component == null) throw new Exception("Component is null");
            if(component.circuit != null && component.circuit != this) throw new Exception("The component is attached to another circuit");
            component.circuit = this;
            component.Id = ++lastId;
            if(!Components.Contains(component)) {
                Components.Add(component);
            }

            return component;
        }
        /// <summary>
        /// Removes the component from the circuit
        /// </summary>
        /// <param name="component">The component</param>
        /// <returns>The same component</returns>
        public T RemoveComponent<T>(T component) where T : Component {
            if(component == null) return null;
            Components.Remove(component);
            return component;
        }
        /// <summary>
        /// Finds a component specified by name
        /// </summary>
        /// <param name="name">The name of the component</param>
        /// <returns>The component</returns>
        public Component Find(string name) {
            foreach(Component item in Components) {
                if(item.Name.Equals(name)) return item;
            }
            return null;
        }

        /// <summary>
        /// Build the circuit, need to be before the first Tick()
        /// </summary>
        private void Build() {
            foreach(Component item in Components) {
                if(item.canStart) starterComponents.Add(item);
            }
            builtCircuit = true;
        }
        /// <summary>
        /// Prepare the Tick() method
        /// </summary>
        private void PrepareTick() {
            executionQueue.Clear();
            foreach(Component item in starterComponents) {
                executionQueue.Enqueue(item);
            }
            if(executionQueue.Count == 0)
                throw new Exception("There is no sources in the circuit.");
            ++simulationId;
        }
        /// <summary>
        /// Ticks a quantity
        /// </summary>
        /// <param name="ticks">The quantity of Tick()</param>
        public void Ticks(int ticks) {
            for(int i = 0; i < ticks; i++) {
                Tick();
            }
        }
        /// <summary>
        /// This need to be called every frame. Every circuit pass it's a Tick()
        /// </summary>
        public void Tick() {
            if(!builtCircuit)
                Build();
            PrepareTick();
            while(executionQueue.Count > 0) {
                Component component = executionQueue.Dequeue();
                if (component != null) { //Failsafe
                    component.simulationId = simulationId;
                    component.Execute();
                }
            }
            time += timePerTick;
        }
    }
    
}
