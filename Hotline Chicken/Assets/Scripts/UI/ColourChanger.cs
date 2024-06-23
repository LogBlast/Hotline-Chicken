using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class ColourChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_Text text;
    public TMP_FontAsset FontAssetA;
    public TMP_FontAsset FontAssetB;

    public void Start()
    {
        text = GetComponent<TMP_Text>();
        text.font = FontAssetA;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.font = FontAssetB;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.font = FontAssetA;
    }
}
