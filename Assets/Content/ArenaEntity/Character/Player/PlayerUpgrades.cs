using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerUpgrades
{
    public float HealthIncrease { get; private set; } = 0;

    public float ManaIncrease { get; private set; } = 0;

    public float CooldownReduction { get; private set; } = 0;
}
