using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/CardEventChannel")]
public class CardEventChannel : ScriptableObject
{
    public event Action<Card> OnEventRaised;

    public void RaiseEvent(Card card)
    {
        OnEventRaised?.Invoke(card);
    }
}
