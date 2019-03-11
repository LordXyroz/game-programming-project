using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLocationTrigger : MonoBehaviour {
    
    public TextMesh text;
    
    public void UpdateText(string txt)
    {
        text.text = txt;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Tells the colliding player's questmanager to handle the trigger from this location
        if (other.gameObject.tag == "Player")
        {
            var questManager = other.GetComponentInChildren<QuestManager>();
            questManager.HandleTrigger(this.gameObject);
        }
    }
}
