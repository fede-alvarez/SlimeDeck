using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Card : MonoBehaviour
{
    [Header("Props")] 
    public int cardDamage = 1;
    public int cardEnergy = 1;
    
    [Header("UI Elements")]
    public TMP_Text cardText;
    public TMP_Text cardEnergyText;
    
    [Header("Visuals")]
    public SpriteRenderer spriteRenderer;
    public Canvas textCanvas;
    public SpriteRenderer debugRenderer;
    
    private Animator _animator;
    private Vector3 _startPosition;
    
    private const float ReturnSpeed = 5.0f;
    
    private bool _isDragging = false;
    private bool _isReturning = false;
    
    private Camera _mainCamera;
    
    private GameManager _gameManager;
    
    private void Awake()
    {
        _gameManager = FindAnyObjectByType<GameManager>();
        
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

        if (transform.position == _startPosition)
        {
            _isReturning = false;
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
