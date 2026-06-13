using System;
using System.Collections;
using System.Collections.Generic;
using Dogs;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpPanel : MonoBehaviour, IPopUpPanel
{
    [SerializeField] private Button closeButton;
    [SerializeField] private RectTransform dialogTransform;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private void Awake()
    {
        closeButton.OnClickAsObservable()
            .Subscribe(_ => ClosePanel())
            .AddTo(this);
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void SetDecriptionText(string description)
    {
        gameObject.SetActive(true);
        
        descriptionText.text = description;
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(dialogTransform);
    }
}
