using System.Collections;
using System.Collections.Generic;
//using Unity.Mathematics;
using UnityEngine;

public class CardMoverHandler : MonoBehaviour
{

    public static Card CardBeingDragged; 

    private Card _currentCard;

    private CardsContainer _lastCardsContainer;
    private CardsContainer _candidateCardsContainer;

    [SerializeField] 
    private LayerMask _layerMask;

    [SerializeField] 
    private CardEventChannel _cardEventChannelDownListener;
    [SerializeField] 
    private CardEventChannel _cardEventChannelUpListener;


    // Start is called before the first frame update
    void Start()
    {
        _cardEventChannelUpListener.OnEventRaised += OnCardReleased;
        _cardEventChannelDownListener.OnEventRaised += OnCardSelected;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCardDraggedPosition();
    }

    void UpdateCardDraggedPosition()
    {
        if (_currentCard != null)
        {
            var screenPoint = Input.mousePosition;
            screenPoint.z = 100; //distance of the plane from the camera
            _currentCard.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        }
        
    }

    void OnCardSelected(Card card)
    {
        CardBeingDragged = card;
        _currentCard = CardBeingDragged;

        Collider collider = IsColliding();

        if (collider == null)
            return;

        if (collider.TryGetComponent(out CardsContainer component))
        {
            _lastCardsContainer = component;
        }


    }

    void OnCardReleased(Card card)
    {
        Collider collider = IsColliding();

        if (collider == null)
        {
            _lastCardsContainer.OnCardReleasedInsideContainer(_currentCard);
            CardBeingDragged = null;
            _currentCard = CardBeingDragged;
            return;
        }


        if (collider.TryGetComponent(out CardsContainer component))
        {
            _candidateCardsContainer = component;
        }

        if (_candidateCardsContainer != null)
        {
            _candidateCardsContainer.OnCardReleasedInsideContainer(_currentCard);
        }
        else
        {
            _lastCardsContainer.OnCardReleasedInsideContainer(_currentCard);
        }

        CardBeingDragged = null;
        _currentCard = CardBeingDragged;
    }

    Collider IsColliding()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = 100f;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000, _layerMask))
        {
            return hit.collider;
        }
        return null;   
    }
}
