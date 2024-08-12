using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckData", menuName = "ScriptableObjects/DeckData", order = 2)]
public class DeckData : ScriptableObject
{

    [SerializeField]
    private string _title;

    [SerializeField]
    private string _description;

    [SerializeField]
    private Sprite _deckImage;

    [SerializeField]
    private List<CardData> _cardDatas = new List<CardData>();

    public string Title => _title;
    public string Description => _description;
    public Sprite DeckImage => _deckImage;
    public List<CardData> CardDatas => _cardDatas;

}
