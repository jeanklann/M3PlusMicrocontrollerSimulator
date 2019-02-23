using System;
using System.Collections.Generic;

namespace CircuitSimulator
{
    public class Circuit
    {
        private bool _builtCircuit;
        private readonly Queue<Component> _executionQueue;
        private int _lastId = -1;
        private readonly List<Component> _starterComponents;

        public List<Component> Components;


        public Circuit(float timePerTick = 0.01666666666666666666f)
        {
            Components = new List<Component>();
            _starterComponents = new List<Component>();
            _executionQueue = new Queue<Component>();
            TimePerTick = timePerTick;
        }

        public int SimulationId { get; private set; }

        public float TimePerTick { get; }

        public float Time { get; private set; }

        protected internal void AddToExecution(Component component)
        {
            _executionQueue.Enqueue(component);
        }

        /// <summary>
        ///     Adds a component to the circuit
        /// </summary>
        /// <param name="component">The component</param>
        /// <returns>The same component</returns>
        public T AddComponent<T>(T component) where T : Component
        {
            if (component == null) throw new Exception("Component is null");
            if (component.Circuit != null && component.Circuit != this)
                throw new Exception("The component is attached to another circuit");
            component.Circuit = this;
            component.Id = ++_lastId;
            if (!Components.Contains(component)) Components.Add(component);

            return component;
        }

        /// <summary>
        ///     Removes the component from the circuit
        /// </summary>
        /// <param name="component">The component</param>
        /// <returns>The same component</returns>
        public T RemoveComponent<T>(T component) where T : Component
        {
            if (component == null) return null;
            Components.Remove(component);
            return component;
        }

        /// <summary>
        ///     Finds a component specified by name
        /// </summary>
        /// <param name="name">The name of the component</param>
        /// <returns>The component</returns>
        public Component Find(string name)
        {
            foreach (var item in Components)
            {
                if (item.Name.Equals(name))
                    return item;
            }
            return null;
        }

        /// <summary>
        ///     Build the circuit, need to be before the first Tick()
        /// </summary>
        private void Build()
        {
            foreach (var item in Components)
            {
                if (item.CanStart)
                    _starterComponents.Add(item);
            }
            _builtCircuit = true;
        }

        /// <summary>
        ///     Prepare the Tick() method
        /// </summary>
        private void PrepareTick()
        {
            _executionQueue.Clear();
            foreach (var item in _starterComponents) _executionQueue.Enqueue(item);
            if (_executionQueue.Count == 0)
                throw new Exception("There is no sources in the circuit.");
            ++SimulationId;
        }

        /// <summary>
        ///     Ticks a quantity
        /// </summary>
        /// <param name="ticks">The quantity of Tick()</param>
        public void Ticks(int ticks)
        {
            for (var i = 0; i < ticks; i++) Tick();
        }

        /// <summary>
        ///     This need to be called every frame. Every circuit pass it's a Tick()
        /// </summary>
        public void Tick()
        {
            if (!_builtCircuit)
                Build();
            PrepareTick();
            while (_executionQueue.Count > 0)
            {
                var component = _executionQueue.Dequeue();
                if (component != null)
                {
                    //Failsafe
                    component.SimulationIdInternal = SimulationId;
                    component.Execute();
                }
            }

            Time += TimePerTick;
        }
    }
}