using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardContainerConfigData", menuName = "ScriptableObjects/CardContainerConfigData", order = 1)]
public class CardContainerConfig : ScriptableObject
{
    [SerializeField]
    private float _displacementY;

    [SerializeField]
    private float _positionTime;


    public float DisplacementY => _displacementY;
    public float PositionTime => _positionTime;

}
