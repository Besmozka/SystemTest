using System;
using System.Collections.Generic;
using Dogs;
using R3;
using UnityEngine;
using Zenject;

public class DogsView : MonoBehaviour, IDogsView
{ 
    [SerializeField] private List<BreedInfoItem> breedInfoItems;
    [SerializeField] private Vector3 startBreedsPosition;

    private Subject<string> _breedInfoClick = new ();
    public Subject<string> BreedInfoClick => _breedInfoClick;
    
    private CompositeDisposable _compositeDisposable;

    private void Awake()
    {
        _compositeDisposable = new CompositeDisposable();
    }

    public void UpdateBreedInfoItems(List<Breed> breedsInfo)
    {
        if (breedsInfo == null || breedsInfo.Count == 0) return;

        for (int i = 0; i < breedsInfo.Count; i++)
        {
            breedInfoItems[i].SetItemInfo(breedsInfo[i].Id, breedsInfo[i].Info.Name, i + 1);
            
            breedInfoItems[i].BreedInfoClick
                .Subscribe(_breedInfoClick.OnNext)
                .AddTo(_compositeDisposable);
        }
    }

    public void ShowWaiter(int index)
    {
        if (index < 0 || index >= breedInfoItems.Count) return;
        
        // breedInfoItems[index].
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void Clear()
    {
        _compositeDisposable?.Dispose();
    }
}