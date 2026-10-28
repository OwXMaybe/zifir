using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zifir.Core
{
    /// <summary>
    /// Minimal allocation-free component pool. Instances are created by a
    /// factory delegate, deactivated on release, reused on get. Prevents GC
    /// spikes from Instantiate/Destroy during gameplay.
    /// </summary>
    public sealed class ObjectPool<T> where T : Component
    {
        private readonly Stack<T> _free = new();
        private readonly Func<T> _factory;

        public ObjectPool(Func<T> factory, int prewarm = 0)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            for (int i = 0; i < prewarm; i++)
            {
                T item = _factory();
                item.gameObject.SetActive(false);
                _free.Push(item);
            }
        }

        public T Get()
        {
            T item = _free.Count > 0 ? _free.Pop() : _factory();
            item.gameObject.SetActive(true);
            return item;
        }

        public void Release(T item)
        {
            if (item == null) return;
            item.gameObject.SetActive(false);
            _free.Push(item);
        }
    }
}
