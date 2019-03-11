using System.Collections;
using UnityEngine;

public class DestroyTextScript : MonoBehaviour
{
   public GameObject myBase;

   public void destroyObject()
   {
      myBase.gameObject.SetActive(false);
      Destroy(myBase);
      Destroy(gameObject);
   }
}
