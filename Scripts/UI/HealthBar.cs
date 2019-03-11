using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public PlayerStatController playerStats;
    public Image healthbar;
    public Text healthText;
	
	void Update () {
        // Changes the healthbar based on players health
        healthbar.fillAmount = (float) playerStats.health / (float) playerStats.maxHealth;
        healthText.text = playerStats.health.ToString() + " / " + playerStats.maxHealth.ToString();
    }
}
