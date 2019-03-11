using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarPosition : MonoBehaviour
{
   public float distY = 1;
   public float distZ = 0;
   public Transform owner;

   // Update is called once per frame
   void Update()
   {
        if (Camera.main == null)
            return;
      transform.eulerAngles = Camera.main.transform.eulerAngles;   // make sure the HP bar is always facing the camera
      if (owner != null)
      {
         Vector3 pos = new Vector3(owner.position.x, owner.position.y + distY, owner.position.z + distZ);
         transform.position = pos;
      }
   }
}
