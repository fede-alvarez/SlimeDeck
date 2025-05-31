using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum CurrentTurn
    {
        Player,
        Enemy
    }
    
    [Header("Game")]
    public CurrentTurn currentTurn = CurrentTurn.Player;

    public HPBar playerHp;
    public PlayersHand playersHand;
    public CardDeck cardDeck;
    
    [Header("Enemies Container")]
    public Transform enemiesContainer;
    
    [Header("UI")]
    public GameObject pausePanel;
    public GameObject mapPanel;
    public TMP_Text energyText;
    public TMP_Text turnText;
    public CanvasGroup damageVision;
    
    [Header("Debugging")]
    public bool debugMode = true;

    private int _currentMapSpot = 0;
    private int _currentEnergy = 3;

    private IEnumerator Start()
    {
        mapPanel?.SetActive(true);
        pausePanel.SetActive(false);
        
        yield return new WaitUntil(() => !mapPanel.activeInHierarchy);

        StartCoroutine("DealCards");
    }

    private IEnumerator DealCards()
    {
        for (int i = 0; i < playersHand.maxCards; i++)
        {
            cardDeck.InstantiateCard();
            yield return new WaitForSeconds(0.15f);
        }
    }

    public void SetDebugMode(bool value)
    {
        debugMode = value;
    }
    
    public void SubtractEnergy(int value)
    {
        _currentEnergy -= value;
        UpdateEnergy();
    }

    public void NextTurn()
    {
        if (currentTurn == CurrentTurn.Player)
        {
            CleanPlayersHand();
            StartCoroutine("EnemysTurn");
        }
        else
        {
            StartCoroutine("DealCards");
            PlayersTurn();
        }

        UpdateTurn();
    }

    private void CleanPlayersHand()
    {
        foreach (Transform child in playersHand.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private IEnumerator EnemysTurn()
    {
        currentTurn = CurrentTurn.Enemy;
        yield return new WaitForSeconds(0.15f);
        damageVision.alpha = 0.7f;
        
        for (int i = 0; i < enemiesContainer.childCount; i++)
        {
            Animator animator = enemiesContainer.GetChild(i).GetComponent<Animator>();
            if (animator) animator.SetTrigger("Attack");
            
            playerHp.Damage(UnityEngine.Random.Range(4,8));
            yield return new WaitForSeconds(1.0f);
        }
        
        yield return new WaitForSeconds(0.15f);
        damageVision.alpha = 0;
        yield return new WaitForSeconds(2.0f);
        NextTurn();
    }

    private void PlayersTurn()
    {
        currentTurn = CurrentTurn.Player;
        
        _currentEnergy = 3;
        UpdateEnergy();
    }
    
    public void ReorderPlayersHand()
    {
        StartCoroutine("ReorderPlayersHandCoroutine");
    }

    private IEnumerator ReorderPlayersHandCoroutine()
    {
        yield return new WaitForEndOfFrame();
        playersHand.Recalculate();
    }

    private void UpdateEnergy()
    {
        energyText.text = _currentEnergy.ToString() + "/3";
    }

    private void UpdateTurn()
    {
        if (currentTurn == CurrentTurn.Player)
            turnText.text = "Player's Turn";
        else
            turnText.text = "Enemy's Turn";
    }

    public bool IsEnergyEnough(int energy)
    {
        return _currentEnergy != 0 && energy <= _currentEnergy;
    }

    public bool DebugMode => debugMode;
    public bool GameStarted { get; set; } = false;
    
    public int CurrentEnergy => _currentEnergy;
    public int CurrentMapSpot => _currentMapSpot;
}
