using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNPCCheck : MonoBehaviour {

    public InputManager inputManager;
    public DialogManager dialogManager;

    public GameObject optionsMenu;

    private void OnTriggerEnter(Collider other)
    {
        // If the collider is triggered by an NPC with text
        if (other.gameObject.tag == "NPCwText")
        {
            // Tells the inputmanager that we can talk to the NPC with the interract key
            // Adds the NPC's talk function to the interract key's dictionary with some parameters,
            // and uses the NPC's instance ID as a key so we can remove it if the player goes out of range.
            inputManager.RegisterAction(InputManager.Keys.interract, () => other.GetComponent<NPCTextScript>().Talk(this.gameObject, dialogManager, optionsMenu), other.GetInstanceID());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If the collider is triggered by an NPC with text
        if (other.gameObject.tag == "NPCwText")
        {
            // Removes the talk function from the interract key's dicitonary
            // The NPC's instance ID is used as a key for removing the function
            inputManager.RemoveAction(InputManager.Keys.interract, other.GetInstanceID());

        }
    }
}
