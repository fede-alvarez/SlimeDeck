using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Cameras")] public Camera mainCamera;
    
    [Header("Enemies")]
    public Transform enemiesContainer;
    public GameObject[] monsterPrefabs = new GameObject[2];
    
    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject mapPanel;
    public GameObject gameOverPanel;
    public GameObject gameWinPanel;

    [Header("UI")] 
    public Button endTurnButton;
    public TMP_Text energyText;
    public TMP_Text turnText;
    public Image turnTextBackground;
    public CanvasGroup damageVision;
    
    [Header("Debugging")]
    public bool debugMode = true;

    private int _currentMapSpot = 0;
    private int _currentEnergy = 3;
    private bool _gameOver = false;
    private bool _isShaking = false;

    private Vector3 _startCameraPosition;
    
    private Animator _damageVisionAnimator;
    
    private void Awake()
    {
        _damageVisionAnimator = damageVision.transform.GetComponent<Animator>();
        _startCameraPosition = mainCamera.transform.position;
    }

    private IEnumerator Start()
    {
        mapPanel?.SetActive(true);
        pausePanel?.SetActive(false);
        gameOverPanel?.SetActive(false);
        gameWinPanel?.SetActive(false);
        
        yield return new WaitUntil(() => !mapPanel.activeInHierarchy);
        
        StartCoroutine("DealCards");
    }

    private void Update()
    {
        if (!_isShaking) return;
        
        Vector3 newCameraPosition = mainCamera.transform.position;
        newCameraPosition.x = Mathf.Lerp(_startCameraPosition.x, newCameraPosition.x + UnityEngine.Random.Range(1.0f, 2.0f), Time.deltaTime * 10.0f);
        newCameraPosition.y = Mathf.Lerp(_startCameraPosition.y, newCameraPosition.y + UnityEngine.Random.Range(1.0f, 2.0f), Time.deltaTime * 10.0f);
            
        mainCamera.transform.position = new Vector3(newCameraPosition.x, newCameraPosition.y, mainCamera.transform.position.z);
    }

    public void StartShake()
    {
        StartCoroutine("ShakeCamera", 0.2f);
    }

    private IEnumerator ShakeCamera(float duration)
    {
        _isShaking = true;
        yield return new WaitForSeconds(duration);
        _isShaking = false;
        mainCamera.transform.position = _startCameraPosition;
    }
    
    private IEnumerator DealCards()
    {
        for (int i = 0; i < playersHand.maxCards; i++)
        {
            cardDeck.InstantiateCard(i + 1);
            yield return new WaitForSeconds(0.15f);
        }
        
        endTurnButton.interactable = true;
    }

    public void SetDebugMode(bool value)
    {
        debugMode = value;
    }

    private void AreEnemiesDead()
    {
        if (enemiesContainer.childCount == 0)
        {
            _gameOver = true;
            StartCoroutine("ShowWinPanel");
        }
    }

    private IEnumerator ShowWinPanel()
    {
        endTurnButton.interactable = false;
        yield return new WaitForSeconds(1.0f);
        gameWinPanel?.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        gameWinPanel?.SetActive(false);
        ShowMap();
    }

    private void ShowMap()
    {
        _currentMapSpot += 1;
        mapPanel.SetActive(true);
        mapPanel.GetComponent<Map>().UnlockSpot(_currentMapSpot);
    }

    public void GetNewEnemies()
    {
        foreach (var enemy in enemiesContainer)
        {
            Destroy(enemy as GameObject);
        }
        CleanPlayersHand();

        StartCoroutine("NewEnemiesSetup");
    }

    private IEnumerator NewEnemiesSetup()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < monsterPrefabs.Length; i++)
        {
            GameObject monster = (GameObject)Instantiate(monsterPrefabs[i], Vector3.zero, Quaternion.identity, enemiesContainer);
            if (monster)
            {
                monster.transform.position = new Vector3(-3 * i + 1.0f, monster.transform.position.y - 1.3f, monster.transform.position.z);
            }
        }

        mapPanel.SetActive(false);
        _gameOver = false;
        yield return new WaitForSeconds(0.3f);
        StartCoroutine("DealCards");
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
        endTurnButton.interactable = false;
        yield return new WaitForSeconds(0.2f);
        
        int enemiesCount = enemiesContainer.childCount;
        for (int i = 0; i < enemiesCount; i++)
        {
            // Execute Attack animation
            StartAttackOnEnemy(i);
            
            // Do Random Damage to Player
            playerHp.Damage(UnityEngine.Random.Range(3,6));
            ShowDamageFeedback();
            yield return new WaitForSeconds(1.0f);
        }
        
        yield return new WaitForSeconds(1.0f);
        endTurnButton.interactable = true;
        NextTurn();
    }

    private void StartAttackOnEnemy(int index)
    {
        Animator animator = enemiesContainer.GetChild(index).GetComponent<Animator>();
        if (animator) animator.SetTrigger("Attack");
    }
    
    private void ShowDamageFeedback()
    {
        _damageVisionAnimator.SetTrigger("Flash");
        //ScreenShake
    }

    private void PlayersTurn()
    {
        currentTurn = CurrentTurn.Player;
        
        _currentEnergy = 3;
        UpdateEnergy();
        
        endTurnButton.interactable = true;
    }
    
    public void ReorderPlayersHand()
    {
        StartCoroutine("ReorderPlayersHandCoroutine");
    }

    private IEnumerator ReorderPlayersHandCoroutine()
    {
        yield return new WaitForEndOfFrame();
        playersHand.Recalculate();
        AreEnemiesDead();
    }

    private void UpdateEnergy()
    {
        energyText.text = _currentEnergy.ToString() + "/3";
    }

    private void UpdateTurn()
    {
        if (currentTurn == CurrentTurn.Player)
        {
            turnTextBackground.color = Color.white;
            turnText.text = "Player's Turn";
        }
        else
        {
            turnTextBackground.color = Color.red;
            turnText.text = "Enemy's Turn";
        }
    }

    public bool IsEnergyEnough(int energy)
    {
        //return _currentEnergy != 0 && energy <= _currentEnergy;
        return energy <= _currentEnergy;
    }

    public bool DebugMode => debugMode;
    public Transform GetPlayersHandTransform => playersHand.gameObject.transform;
    public PlayersHand GetPlayersHand => playersHand;
    public bool GameStarted { get; set; } = false;
    public bool GameOver => _gameOver;
    public int CurrentEnergy => _currentEnergy;
    public int CurrentMapSpot => _currentMapSpot;
}
