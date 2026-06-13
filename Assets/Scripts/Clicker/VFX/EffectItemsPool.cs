using UnityEngine;
using Zenject;

namespace Clicker
{
    public class EffectItemsPool : MemoryPool<Vector3, EffectsItem>
    {
        protected override void OnSpawned(EffectsItem item)
        {
            base.OnSpawned(item);
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(EffectsItem item)
        {
            base.OnDespawned(item);
            item.gameObject.SetActive(false);
        }

        protected override void Reinitialize(Vector3 position, EffectsItem item)
        {
            item.RectTransform.position = Vector3.zero + Random.insideUnitSphere;
            item.Launch();
        }
    }
}