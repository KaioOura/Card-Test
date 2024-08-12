using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardsContainer : MonoBehaviour
{

    [SerializeField]
    private CardContainerConfig _cardContainerConfig;
    [SerializeField]
    private RectTransform _noAreaTransform;

    [SerializeField] 
    private CardEventChannel _cardEventChannelDownListener;
    [SerializeField] 
    private CardEventChannel _cardEventChannelUpListener;

    [SerializeField]
    private List<Card> _cards = new List<Card>();

    private RectTransform _rectTransform;
    private Card _currentCardDragged;


    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        SetCardsPosition();
        UpdateCardOrder();
    }

    public void SetCardList(Transform transform)
    {
        _cards.Clear();
        foreach (Transform tr in transform)
        {
            if (!tr.TryGetComponent(out Card card))
                return;
            
            _cards.Add(card);
        }
    }

    private bool IsCursorInPlayArea() {
       
        var screenPoint = Input.mousePosition;
        screenPoint.z = 100; //distance of the plane from the camera
        var cursorPosition = Camera.main.ScreenToWorldPoint(screenPoint);

        var playArea = _rectTransform;
        var playAreaCorners = new Vector3[4];
        playArea.GetWorldCorners(playAreaCorners);
        return cursorPosition.x > playAreaCorners[0].x &&
               cursorPosition.x < playAreaCorners[2].x &&
               cursorPosition.y > playAreaCorners[0].y &&
               cursorPosition.y < playAreaCorners[2].y;
    }

    void HandleCardDraggedParenting()
    {
        if (IsCursorInPlayArea())
            _currentCardDragged = CardMoverHandler.CardBeingDragged;

        if (!_cards.Contains(_currentCardDragged) && IsCursorInPlayArea())
        {
            AddCardToContainer(_currentCardDragged);
        }
        else if (_cards.Contains(_currentCardDragged) && !IsCursorInPlayArea())
        {
            RemoveCardFromContainer(_currentCardDragged);
            _currentCardDragged = null;
        }
    }

    private void UpdateCardOrder() 
    {
        if (CardMoverHandler.CardBeingDragged == null)
            return;

        HandleCardDraggedParenting();

        if (_currentCardDragged == null || _cards.Count < 1) return;

        var newCardIdx = _cards.Count(card => _currentCardDragged.transform.localPosition.x > card.transform.localPosition.x);
        var originalCardIdx = _cards.IndexOf(_currentCardDragged);
        if (newCardIdx != originalCardIdx) 
        {
            _cards.RemoveAt(originalCardIdx);
            if (newCardIdx > originalCardIdx && newCardIdx < _cards.Count - 1) {
                newCardIdx--;
            }

            _cards.Insert(newCardIdx, _currentCardDragged);
        }

        _currentCardDragged.transform.SetSiblingIndex(newCardIdx);
    }

    private void SetCardsPosition() {
        // Soma a largura total das cartas
        var cardsTotalWidth = _cards.Sum(card => card.Width * card.transform.lossyScale.x);

        DistributeChildrenToFitContainer(cardsTotalWidth);
    }

    private void DistributeChildrenToFitContainer(float childrenTotalWidth) {
        // Tamanho total do rect
        var width = _rectTransform.rect.width * transform.lossyScale.x;
        var distanceBetweenChildren = (width - childrenTotalWidth) / (_cards.Count - 1);
        // Posiciona as childs em distâncias iguais
        var currentX = transform.position.x - width / 2;
        foreach (Card child in _cards) {
            var adjustedChildWidth = child.Width * child.transform.lossyScale.x;
            UpdateCardPosition(child, new Vector3(currentX + adjustedChildWidth / 2, transform.position.y, 0));

            currentX += adjustedChildWidth + distanceBetweenChildren;
        }
    }

    void UpdateCardPosition(Card card, Vector3 _targetPosition)
    {
        if (!card.IsDragged)
        {
            var target = new Vector3(_targetPosition.x, _targetPosition.y + _cardContainerConfig.DisplacementY, transform.position.z);

            var distance = Vector3.Distance(card.RectTransform.position, target);
            var repositionSpeed = _cardContainerConfig.PositionTime;
            card.transform.position = Vector3.Lerp(card.RectTransform.position, target,
            repositionSpeed / distance * Time.deltaTime);
        }
         
    }

    public void AddCardToContainer(Card card)
    {
        if (card == null || _cards.Contains(card))
            return;
        
        _cards.Add(card);
        card.transform.SetParent(transform);
    }

    void RemoveCardFromContainer(Card card)
    {
        if (!_cards.Contains(card))
            return;

        _cards.Remove(card);
    }

    public void OnCardReleasedInsideContainer(Card card)
    {
        AddCardToContainer(card);
        _currentCardDragged = null;
    }

}
