using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public Transform spots;

    private void Start()
    {
        UnlockSpot(0);
    }

    private IEnumerator SelectNext(int index)
    {
        UnselectSpots();
        yield return new WaitForEndOfFrame();
        SelectSpot(index);
    }

    public void UnlockSpot(int index)
    {
        StartCoroutine("SelectNext", index);
    }

    private void UnselectSpots()
    {
        foreach (Transform spot in spots)
        {
            spot.GetComponent<Button>().interactable = false;
            spot.GetComponent<MapSpot>().SetSelected(false);
        }
    }

    private void SelectSpot(int spotIndex)
    {
        EnableSelect(spotIndex);
    }

    private void EnableSelect(int spotIndex)
    {
        Transform spot = spots.GetChild(spotIndex);
        
        spot.GetComponent<Button>().interactable = true;
        
        MapSpot mapSpot = spot.GetComponent<MapSpot>();
        mapSpot.gameObject.SetActive(true);
        mapSpot.SetSelected(true);
    }

    public void OnPressed()
    {
        Debug.Log("Go Spot!");
    }
}
