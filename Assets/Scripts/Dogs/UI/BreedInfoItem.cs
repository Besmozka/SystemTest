using System;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BreedInfoItem : MonoBehaviour
{
    [SerializeField] private Waiter waiter;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI itemIndex;
    
    private string _breedId;
    
    private RectTransform _rectTransform;
    public RectTransform RectTransform => _rectTransform ?? (_rectTransform = GetComponent<RectTransform>());
    
    private Subject<string> _breedInfoClick;
    public Subject<string> BreedInfoClick => _breedInfoClick;

    private void Awake()
    {
        _breedInfoClick = new Subject<string>();
        
        button.OnClickAsObservable()
            .Subscribe(_ => BreedInfoClick?.OnNext(_breedId));
    }

    public void SetItemInfo(string id, string breedName, int index)
    {
        _breedId = id;
        name.text = breedName;
        itemIndex.text = index.ToString();
    }
}
