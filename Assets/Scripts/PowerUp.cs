using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PowerUpType
{
    None,
    Armor,
    Drug,
    Gun
}

public class PowerUp : MonoBehaviour
{
    public PowerUpType powerUpType;
}
