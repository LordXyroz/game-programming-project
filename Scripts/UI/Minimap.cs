using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour {

    public GameObject player;
    public Vector3 offset;
    public Shader minimapShader;

    private void Start()
    {
        // Sets a replacement shader, want to use a simpler shader than what the main cam is using
        // for optimization
        gameObject.GetComponent<Camera>().SetReplacementShader(minimapShader, "");
    }
    
	void FixedUpdate () {
        transform.position = player.transform.position + offset;
        transform.rotation = Quaternion.Euler(90, player.transform.rotation.eulerAngles.y, 0);
	}
}
