using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu (fileName = "Player Stats", menuName = "Player/Stats")]
public class PlayerStatScript : ScriptableObject {

    // Unused scriptable object, changed to a monobehaviour instead
    [Header("Main Stats")]
    public int Strength;
    public int Agility;
    public int Stamina;
    public int Intellect;

    [Header("Health")]
    public int Health;
    public int MinHealth;
    public int MaxHealth;

    [Header("Mana")]
    public int Mana;
    public int MinMana;
    public int MaxMana;

    [Header("Combat")]
    public int MeleeDamage;
    public int RangedDamage;
    public int MagicDamage;

    [Header("Other")]
    public int MovementSpeed;

}
