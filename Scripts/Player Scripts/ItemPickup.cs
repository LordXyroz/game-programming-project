using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour {

    public TextMesh text;
    
	void Start () {


        text = GetComponent<TextMesh>();
        text.color = new Vector4(1, 1, 1, 0);

    }
	

    public void SetTextActive(bool visible)
    {
        Color tempColor = text.color;   // Need to set temp for changing color of text in work.
        if (visible)
            text.color = new Vector4(1, 1, 1, 1);
        else
            text.color = new Vector4(1, 1, 1, 0);
    }

    public void RotateText(Vector3 targetDir)
    {

        float step = 5 * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);

        transform.rotation = Quaternion.LookRotation(newDir);
    }
}
