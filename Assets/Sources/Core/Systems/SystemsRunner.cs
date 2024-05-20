using System;
using System.Collections.Generic;
using Asteroids.Utils;
using UnityEngine;

namespace Asteroids.Core.Systems
{
    public class SystemsRunner : MonoBehaviour
    {
        private readonly DisposableCollection _subscriptions = new();
        private readonly List<IUpdateable> _regularUpdates = new();
        private readonly List<IUpdateable> _physicalUpdates = new();
        private readonly List<IInitializeable> _initializeables = new();

        public void AddDisposable(IDisposable disposable) => _subscriptions.Add(disposable);
        public void AddRegularUpdateable(IUpdateable regularUpdateable) => _regularUpdates.Add(regularUpdateable);
        public void AddPhysicsUpdateable(IUpdateable physicsUpdateable) => _physicalUpdates.Add(physicsUpdateable);
        public void AddInitializeable(IInitializeable initializeable) => _initializeables.Add(initializeable);

        private void Start()
        {
            for (var i = 0; i < _initializeables.Count; i++)
            {
                _initializeables[i].Init();
            }
        }

        private void Update()
        {
            for (var i = 0; i < _regularUpdates.Count; i++)
            {
                _regularUpdates[i].Update(Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            for (var i = 0; i < _physicalUpdates.Count; i++)
            {
                _physicalUpdates[i].Update(Time.fixedDeltaTime);
            }
        }
    }
}