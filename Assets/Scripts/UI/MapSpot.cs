using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapSpot : MonoBehaviour
{
    public enum SpotType
    {
        Monster,
        Chest
    };
    
    public Transform iconContainer;
    public GameObject spotSelected;
    
    public SpotType spotType = SpotType.Monster;
    
    private CanvasGroup _iconCanvasGroup;

    private void Awake()
    {
        _iconCanvasGroup = iconContainer.GetComponent<CanvasGroup>();
    }

    void Start()
    {
        SetSelected(false);
        SetType();
    }

    private void SetType()
    {
        foreach (Transform icon in iconContainer)
        {
            icon.gameObject.SetActive(false);
        }
        
        iconContainer.GetChild((int) spotType).gameObject.SetActive(true);
    }

    public void SetSelected(bool selected)
    {
        spotSelected.SetActive(selected);
        _iconCanvasGroup.alpha = selected ? 1.0f : 0.4f;
    }
}
