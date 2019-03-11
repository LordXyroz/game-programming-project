using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatController : MonoBehaviour {

    // Script for controlling the different stats of the player character
    // TODO: Use the script for combat, healing, leveling, death etc
    public InputManager input;

    [Header("Main Stats")]
    public int strength;
    public int agility;
    public int stamina;
    public int intellect;

    [Header("Health")]
    public int health;
    public int minHealth;
    public int maxHealth;

    [Header("Mana")]
    public int mana;
    public int minMana;
    public int maxMana;

    [Header("Combat")]
    public int meleeDamage;
    public int rangedDamage;
    public int magicDamage;

    [Header("Other")]
    public int movementSpeed;

    // Use this for initialization
    void Start () {
        maxHealth = 10 * stamina;
        health = maxHealth;

        meleeDamage = 10 * strength;
        rangedDamage = 10 * agility;
        magicDamage = 10 * intellect;

        maxMana = 10 * intellect;
        mana = maxMana;

        // Adds a way to check that the player takes damage and uses mana for debugging purpose
        input.RegisterAction(InputManager.Keys.mouse0, TakeDamage, GetInstanceID());
        input.RegisterAction(InputManager.Keys.mouse1, LooseMana, GetInstanceID());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage()
    {
        health--;
    }

    public void LooseMana()
    {
        mana--;
    }
}
