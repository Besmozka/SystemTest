using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Clicker
{
    public class EffectItemsPool : MemoryPool<EffectsItem>
    {
        private readonly List<EffectsItem> _activeItems = new List<EffectsItem>();
    
        protected override void OnSpawned(EffectsItem item)
        {
            base.OnSpawned(item);
            
            item.gameObject.SetActive(true);
            item.Launch();
            
            _activeItems.Add(item);
        }

        protected override void OnDespawned(EffectsItem item)
        {
            base.OnDespawned(item);
            
            item.gameObject.SetActive(false);
            
            _activeItems.Remove(item);
        }

        protected override void Reinitialize(EffectsItem item)
        {
            base.Reinitialize(item);
            
            item.RectTransform.position = Vector3.zero + Random.insideUnitSphere;
            item.Launch();
        }

        public void ReturnAll()
        {
            foreach (var item in _activeItems.ToArray())
            {
                Despawn(item);
            }
            
            _activeItems.Clear();
        }
    }
}