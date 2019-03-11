using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePitScript : MonoBehaviour {

    public AudioClip clip;
    // Bool to make sure we don't continuesly retry to play efx
    public bool played = false;
    
    private void OnTriggerEnter(Collider other)
    {
        // Plays a looping fire soundeffect when a player triggers the collider
        if (other.tag == "Player")
        {
            // Only plays for the local player, so online players can't trigger it for others
            if (!played && other.GetComponent<PlayerMultiPlayer>().isLocalPlayer)
            { 
                other.GetComponentInChildren<SoundManager>().PlayEfx(clip, true);
                played = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Stops the looping fire soundeffect when a player triggers the collider
        if (other.tag == "Player")
        {
            // Only stops for the local player, so online players can't trigger it for others
            if (played && other.GetComponent<PlayerMultiPlayer>().isLocalPlayer)
            {
                other.GetComponentInChildren<SoundManager>().loopingEfxSource.Stop();
                other.GetComponentInChildren<SoundManager>().loopingEfxSource.clip = null;
                played = false;
            }
        }
    }
}
