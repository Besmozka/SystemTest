using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BreedInfoItemsPool : MemoryPool<BreedInfoItem>
{
    protected override void OnSpawned(BreedInfoItem item)
    {
        base.OnSpawned(item);
        
        item.gameObject.SetActive(true);
    }

    protected override void OnDespawned(BreedInfoItem item)
    {
        base.OnDespawned(item);
        
        item.gameObject.SetActive(false);
    }

    protected override void Reinitialize(BreedInfoItem item)
    {
        base.Reinitialize(item);
        
        item.RectTransform.position = Vector3.zero;
    }
}