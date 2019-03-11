using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallCampScript : MonoBehaviour {

    public AudioClip clip;
    // Bool to make sure we don't continuesly retry to play efx
    public bool played = false;

    private void OnTriggerEnter(Collider other)
    {
        // Fades in a new bgm when a player triggers the collider
        if (other.gameObject.tag == "Player")
        {
            // Only plays for the local player, so online players can't trigger it for others
            if (other.GetComponent<PlayerMultiPlayer>().isLocalPlayer)
            {
                if (!played)
                {
                    other.GetComponentInChildren<SoundManager>().Fade(clip, 0.1f, 0.5f);
                    played = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Fades in default bgm when a player triggers the collider
        if (other.gameObject.tag == "Player")
        {
            // Only plays for the local player, so online players can't trigger it for others
            if (other.GetComponent<PlayerMultiPlayer>().isLocalPlayer)
            {
                var soundManager = other.GetComponentInChildren<SoundManager>();
                if (played && soundManager.bgmSource.clip == clip)
                {
                    played = false;
                    soundManager.Fade(soundManager.defaultClip, 0.1f, 0.5f);
                }
            }
        }
    }
}
