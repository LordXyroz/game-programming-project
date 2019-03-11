using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour {

    public PlayerStatController playerStats;
    public Image mana;
    public Text manaText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Changes the manabar based on the players mana
        mana.fillAmount = (float)playerStats.mana / (float)playerStats.maxMana;
        manaText.text = playerStats.mana.ToString() + " / " + playerStats.maxMana.ToString();
	}
}
