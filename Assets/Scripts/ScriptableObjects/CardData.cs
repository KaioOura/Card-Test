using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/CardData", order = 1)]
public class CardData : ScriptableObject
{

    [SerializeField]
    private string _title;

    [SerializeField]
    private string _description;

    [SerializeField]
    private int _cost;

    [SerializeField]
    private Sprite _frontImage;

    [SerializeField]
    private Card _cardPrefab; 

    public string Title => _title;
    public string Description => _description;
    public int Cost => _cost;
    public Sprite FrontImage => _frontImage;
    public Card CardPrefab => _cardPrefab;

}
