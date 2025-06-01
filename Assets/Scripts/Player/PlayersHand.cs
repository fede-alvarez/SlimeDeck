using UnityEngine;


public class PlayersHand : MonoBehaviour
{
    public AnimationCurve handCurvature;
    public int maxCards = 3;
    
    private int _cardsCount = 0;
    private float _cardSize = 1.8f;

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
        
        _startPos = _cardsCount * _cardSize * -0.5f + (_cardSize * 0.5f);
        
        int index = 0;
        foreach (var card in transform)
        {
            var currentCard = (Transform) card;
            var cardPos = currentCard.position;

            //Debug.Log(index + " " + _cardsCount + " " + (float) index / (_cardsCount - 1));
            var curveEvaluation = handCurvature.Evaluate((float) index / (_cardsCount - 1));
            var offPosition = (_cardsCount <= 3) ? 0 : curveEvaluation * 0.0f;
            
            var cardPosX = _startPos + (index * _cardSize);
            
            currentCard.position = new Vector3(cardPosX, cardPos.y + offPosition, cardPos.z);
            currentCard.GetComponent<Card>().SetCardStartPosition();
            
            index++;
        }
    }
}
