using UnityEngine;

public class CardZone : MonoBehaviour
{
    private GameManager _manager;
    
    private SpriteRenderer _spriteRenderer;
    private ShowDamage _damageFeedback;
    
    private bool _activeZone = false;
    private bool _isDropping = false;
    
    private Card _selectedCard;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _damageFeedback = GetComponent<ShowDamage>();
        
        // Get a reference to Game Manager
        _manager = FindAnyObjectByType<GameManager>();
    }

    private void Start()
    {
        _spriteRenderer.enabled = _manager.DebugMode;
    }

    private void Update()
    {
        if (!_activeZone) return;
        
        // Checks for Mouse Left Button Release (Up)
        _isDropping = Input.GetMouseButtonUp(0);
        DropCard();
    }

    private void DropCard()
    {
        if (!_isDropping) return;
        _activeZone = false;
        
        // Shows DAMAGE number & Use CARD
        ShowFeedback();
        
        // Subtract energy
        _manager.SubtractEnergy(_selectedCard.cardEnergy);
        
        // Destroy the card when played
        if (_selectedCard && _selectedCard.gameObject)
            Destroy(_selectedCard.gameObject);
            
        _manager.ReorderPlayersHand();
    }

    private void ShowFeedback()
    {
        //Debug.Log("Select Card: " + _selectedCard.name);
        
        Vector3 feedbackPosition = transform.position + new Vector3(0, 2f, 0);
        int cardDamage = _selectedCard.cardDamage; 
        _damageFeedback.damage = cardDamage;
        _damageFeedback.ShowAt(feedbackPosition);
    }

    private void SetActiveZone(bool activeZone)
    {
        _activeZone = activeZone;
        
        if (activeZone)
            _spriteRenderer.color = new Color(1,0,0,0.3f);
        else
            _spriteRenderer.color = new Color(1,1,1,0.3f);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        SetActiveZone(true);
        _selectedCard = other.gameObject.GetComponentInParent<Card>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        SetActiveZone(false);
        //_selectedCard = null;
    }
}