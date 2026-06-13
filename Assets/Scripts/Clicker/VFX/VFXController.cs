using DG.Tweening;
using UnityEngine;

namespace Clicker
{
    public class VFXController : IVFXController
    {
        private readonly EffectItemsPool _effectItemsPool;
        private bool _isAnimating;

        public VFXController(EffectItemsPool effectItemsPool)
        {
            _effectItemsPool = effectItemsPool;
        }

        public void SpawnEffects(RectTransform transform)
        {
            _effectItemsPool.Spawn();

            if (_isAnimating) return;

            _isAnimating = true;

            var startScale = transform.localScale;
            var targetScale = startScale * 0.9f;

            transform.DOScale(targetScale, 0.1f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    transform.DOScale(startScale, 0.1f)
                        .SetEase(Ease.InQuad)
                        .OnComplete(() => _isAnimating = false);
                });
        }

        public void HideAllVFX()
        {
            _effectItemsPool.ReturnAll();
        }
    }
}