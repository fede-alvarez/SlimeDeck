using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardDeck : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform playersHand;
    
    private GameManager _gameManager;
    private Animator _animator;

    private int _maxCardsCount;
    
    private void Awake()
    {
        _gameManager = FindAnyObjectByType<GameManager>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _maxCardsCount = playersHand.GetComponent<PlayersHand>().maxCards;
    }

    private void OnMouseDown()
    {
        //InstantiateCard();
    }

    public void InstantiateCard()
    {
        if (playersHand.childCount >= _maxCardsCount) return;
        
        _animator.SetTrigger("Press");
        GameObject card = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, playersHand);
        if (card == null) return;
        card.transform.localPosition = Vector3.zero;
        
        Card currentCard = card.GetComponent<Card>();
        currentCard.cardDamage = Random.Range(2, 6);
        currentCard.cardEnergy = Random.Range(1,3);
        currentCard.Pop();
        
        _gameManager.ReorderPlayersHand();
    }
}
