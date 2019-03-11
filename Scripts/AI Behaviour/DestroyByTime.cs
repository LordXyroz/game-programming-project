using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
   //TODO create a spawn controller instead of creating and destroying

   public float lifetime;
   GameObject player;

   void Start()
   {
      player = GameObject.FindGameObjectWithTag("Player");
      Destroy(gameObject, lifetime);
   }

   private void OnCollisionEnter(Collision collision)
   {
      //Destroy(collision.collider.gameObject);
      if (collision.gameObject.tag == "Player")
      {
         Destroy(gameObject);
      }
   }
}
