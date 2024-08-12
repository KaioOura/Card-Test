using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    private CardData _cardData;

    [SerializeField]
    private TextMeshProUGUI _titleText;
    [SerializeField]
    private TextMeshProUGUI _costText;
    [SerializeField]
    private TextMeshProUGUI _descriptionText;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private CardEventChannel _cardEventChannelDownBroadcaster;
    [SerializeField]
    private CardEventChannel _cardEventChannelUpBroadcaster;


    private RectTransform _rectTransform;
    private bool _isDragged;


    public float Width => _rectTransform.rect.width * _rectTransform.localScale.x;
    public RectTransform RectTransform => _rectTransform;
    public bool IsDragged => _isDragged;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitializeCard(CardData cardData)
    {
        if (cardData == null)
        {
            Debug.LogWarning("Card initialization failed, stopping action");
            return;
        }

        _cardData = cardData;

        _titleText.text = cardData.Title;
        _costText.text = cardData.Cost.ToString();
        _descriptionText.text = cardData.Description;
        gameObject.name = cardData.Title;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragged = false;
        _titleText.maskable = true;
        _costText.maskable = true;
        _descriptionText.maskable = true;
        _image.maskable = true;

        _cardEventChannelUpBroadcaster.RaiseEvent(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragged = true;
        _titleText.maskable = false;
        _costText.maskable = false;
        _descriptionText.maskable = false;
        _image.maskable = false;
        
        _cardEventChannelDownBroadcaster.RaiseEvent(this);
    }
}
