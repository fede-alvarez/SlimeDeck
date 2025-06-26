using System;
using UnityEngine;


public class PlayersHand : MonoBehaviour
{
    public int maxCards = 3;
    
    private const float CardSize = 1.8f;
    private int _cardsCount = 0;
    
    private float _startPos;
    
    private void Start()
    {
        Recalculate();
    }

    public void Recalculate()
    {
        _cardsCount = transform.childCount;
        //Debug.Log("Cartas: " + _cardsCount);
        
        SetCardsOrder();
        SetPositions();
    }

    private void SetCardsOrder()
    {
        if (transform.childCount == 0) return;
        int index = 0;
        
        foreach (var card in transform)
        {
            Transform currentCard = (Transform) card;
            currentCard.GetComponent<Card>().SetCardOrder(index * 2);
            
            index++;
        }
    }

    private void SetPositions()
    {
        if (_cardsCount <= 0) return;
        
        _startPos = _cardsCount * CardSize * -0.5f + (CardSize * 0.5f);
        
        int index = 0;
        foreach (var card in transform)
        {
            var currentCard = (Transform) card;
            var cardPos = currentCard.position;
            var cardPosX = _startPos + (index * CardSize);
            
            currentCard.localPosition = new Vector3(cardPosX, 0, cardPos.z);
            currentCard.GetComponent<Card>().SetCardStartPosition();
            
            index++;
        }
    }

    public int GetCardIndex(Card card)
    {
        int index = 0;
        foreach (Transform t in transform)
        {
            var currentCard = t.GetComponent<Card>();
            if (card == currentCard)
                return index;
            index += 1;
        }
        
        return -1;
    }
}
