using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextScript : MonoBehaviour
{

   public string damage;
   public TextMesh myTextMesh;


   void Start()
   {
      myTextMesh.text = damage;
      myTextMesh.transform.eulerAngles = Camera.main.transform.eulerAngles;   // make the text face the camera direction in order to be visible on the screen
   }
}
