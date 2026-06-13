using UnityEngine;

namespace Clicker
{
    public interface IVFXController
    {
        public void SpawnEffects(RectTransform transform);
        public void HideAllVFX();
    }
}