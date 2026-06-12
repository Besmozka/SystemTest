
using UnityEngine;

namespace Clicker
{
    public class VFXController
    {
        private EffectItemsPool _effectItemsPool;
        private ParticleSystem _particleSystem;

        public VFXController(EffectItemsPool effectItemsPool)
        {
            _effectItemsPool = effectItemsPool;
        }

        public void SpawnEffects(Vector3 position)
        {
            _effectItemsPool.Spawn(position);
        }
    }
}