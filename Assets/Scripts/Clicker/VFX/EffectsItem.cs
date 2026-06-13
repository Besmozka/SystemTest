using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Clicker
{
    public class EffectsItem : MonoBehaviour
    {
        [SerializeField] private RectTransform currencyItem;
        [SerializeField] private ParticleSystem _particleSystem;
        
        private EffectItemsPool _pool;
        private Tween _currentTween;
        
        public RectTransform RectTransform => currencyItem;

        [Inject]
        public void Construct(EffectItemsPool pool)
        {
            _pool = pool;
        }

        public void Launch()
        {
            MoveAsync();
        }

        private void MoveAsync()
        {
            _particleSystem.Play();
            
            Vector3 startPos = RectTransform.position;
            Vector3 targetPos = startPos + Vector3.up * 5f;
            
            _currentTween?.Kill();
            
            _currentTween = RectTransform.DOMove(targetPos, 5f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _particleSystem.Stop();
                    _particleSystem.Clear();
                    _pool.Despawn(this);
                });
        }
        
        private void OnDestroy()
        {
            _currentTween?.Kill();
        }
    }
}