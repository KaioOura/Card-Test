using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private List<CardData> _availableCards = new List<CardData>(); //Em um cenário real essa variável precisa de uma série de checagens para ser atualizada.
    private DeckData _deckData; //Em um cenário real essa variável precisa de uma série de checagens para ser atualizada. Por hora vai ser alimentada pelo _standardDeckData;
    public List<CardData> AvailableCards => _availableCards;
    private CardsContainer _cardsContainer;


    [SerializeField]
    private DeckData _standardDeckData;

    void Awake()
    {
        if (transform.TryGetComponent(out CardsContainer cardsContainer))
        {
            _cardsContainer = cardsContainer;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        _deckData = GetDeckData(_standardDeckData);
        PopulateHand(GetCards(_deckData));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PopulateHand(List<CardData> cardDatas)
    {
        if (cardDatas == null || cardDatas.Count <= 0)
        {
            Debug.LogWarning("Populate hands failed");
            return;
        }

        _availableCards.Clear();

        foreach (CardData cardData in cardDatas)
        {
            _availableCards.Add(cardData);
        }

        foreach (CardData cardData in _availableCards)
        {
            Card tempCard = Instantiate(cardData.CardPrefab, transform.position, Quaternion.identity);
            tempCard.transform.SetParent(transform);
            tempCard.transform.localScale = new Vector3(1, 1, 1);
            tempCard.InitializeCard(cardData);
        }

        if (_cardsContainer == null)
        {
            Debug.Log("Card Container not found");
            return;
        }

        _cardsContainer.SetCardList(transform);
    }

    public List<CardData> GetCards(List<CardData> cardDatas)
    {
        return cardDatas;
    }
    public List<CardData> GetCards(DeckData deckData)
    {
        return deckData.CardDatas;
    }

    public DeckData GetDeckData(DeckData deckData) //Com as tratativas corretas pode ser usado para atualizar o deck atual
    {
        if (deckData == null)
        {
            Debug.LogWarning("DeckData is null, operation canceled");
            return null;
        }

        return deckData;
    }
}
