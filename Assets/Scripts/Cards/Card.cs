using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("Props")] 
    public int cardDamage = 1;
    public int cardEnergy = 1;

    public AnimationCurve returnAnimationCurve;
    
    [Header("UI Elements")]
    public TMP_Text cardText;
    public TMP_Text cardEnergyText;
    
    [Header("Visuals")]
    public SpriteRenderer spriteRenderer;
    public Canvas textCanvas;
    public SpriteRenderer debugRenderer;
    
    private const float ReturnSpeed = 0.2f;
    
    private Vector3 _startPosition;
    private int _lastHierarchyPosition = 0;
    
    private bool _isDragging = false;
    private bool _isReturning = false;

    private PlayersHand _playersHand;
    private Camera _mainCamera;
    private Animator _animator;
    
    private GameManager _gameManager;
    
    private void Awake()
    {
        _gameManager = FindAnyObjectByType<GameManager>();
        _playersHand = _gameManager.GetPlayersHand;
        
        _animator = GetComponent<Animator>();
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        _startPosition = transform.position;
        
        debugRenderer.enabled = _gameManager.DebugMode;
        
        cardText.text = cardDamage.ToString();
        cardEnergyText.text = cardEnergy.ToString();
    }

    public void SetCardStartPosition()
    {
        _startPosition = transform.position;
    }

    public void SetCardOrder(int cardOrder)
    {
        spriteRenderer.sortingOrder = cardOrder;
        textCanvas.sortingOrder = cardOrder + 1;
    }
    
    private void Update()
    {
        if (!_gameManager.GameStarted) return;
        
        if (_isReturning)
        {
            ReturnStartPosition();
        }

        if (_isDragging)
        {
            FollowMousePosition();
        }
    }

    private void ReturnStartPosition()
    {
        transform.position = Vector3.Lerp(transform.position, _startPosition, ReturnSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _startPosition) < 0.2f)
        {
            _isReturning = false;
            transform.SetParent(_gameManager.GetPlayersHandTransform);
            transform.SetSiblingIndex(_lastHierarchyPosition);
            _gameManager.ReorderPlayersHand();
            
        }
    }

    public void Pop()
    {
        _animator.SetTrigger("Pop");
    }

    private void FollowMousePosition()
    {
        Vector3 newPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = 0;
        transform.position = newPos;
    }

    public void OnMouseEnter()
    {
        _animator.SetBool("IsOver", true);
    }

    public void OnMouseExit()
    {
        _animator.SetBool("IsOver", false);
    }

    public void OnMouseDown()
    {
        if (!_gameManager.IsEnergyEnough(cardEnergy)) return;

        _lastHierarchyPosition = _playersHand.GetCardIndex(this);
        
        transform.SetParent(null);
        _gameManager.ReorderPlayersHand();
        
        _isDragging = true;
        _isReturning = false;
    }

    public void OnMouseUp()
    {
        if (!_gameManager.IsEnergyEnough(cardEnergy)) return;
        
        _isDragging = false;
        _isReturning = true;
    }
}
