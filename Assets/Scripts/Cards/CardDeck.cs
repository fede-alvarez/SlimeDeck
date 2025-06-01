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
        if (!card) return;
        card.transform.localPosition = Vector3.zero;
        
        Card currentCard = card.GetComponent<Card>();
        currentCard.cardEnergy = Random.Range(0,4);
        
        float randomChance = Random.Range(0.0f, 1.0f);
        if (currentCard.cardEnergy == 0 && randomChance > 0.5f)
            currentCard.cardEnergy = Random.Range(1,4);
        
        int damageRange = 0;
        switch (currentCard.cardEnergy)
        {
            case 0:
                damageRange = Random.Range(1, 3);
                break;
            case 1:
                damageRange = Random.Range(1, 5);
                break;
            case 2:
                damageRange = Random.Range(4, 6);
                break;
            case 3:
                damageRange = Random.Range(6, 8);
                break;
            default:
                damageRange = Random.Range(1, 3);
                break;
        }
        currentCard.cardDamage = damageRange;
        
        currentCard.Pop();
        
        _gameManager.ReorderPlayersHand();
    }
}
