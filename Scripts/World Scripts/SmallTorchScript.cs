using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallTorchScript : MonoBehaviour {
    public Light myLight;
    public ParticleSystem myParticle;

    private void Start()
    {
        myLight.enabled = false;
        myParticle.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Enables the light and partical effect for the local player when triggered
        if (other.tag == "Player" && other.GetComponent<PlayerMultiPlayer>().isLocalPlayer)
        {
            myLight.enabled = true;
            myParticle.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Disables the light and partical effect for the local player when triggered
        if (other.tag == "Player" && other.GetComponent<PlayerMultiPlayer>().isLocalPlayer)
        {
            myLight.enabled = false;
            myParticle.Stop();
        }
    }
}
