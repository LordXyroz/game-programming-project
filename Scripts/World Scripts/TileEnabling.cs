using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TileEnabling : MonoBehaviour {
    
    [SerializeField]
    private List<GameObject> objects = new List<GameObject>();
    private Vector3 tileSize = new Vector3(1000, 1000, 1000);
    private Vector3 myPos;

	// Use this for initialization
	void Start () {
        // Defaults TerrainCollider to false
        this.gameObject.GetComponent<TerrainCollider>().enabled = false;

        this.myPos = this.transform.position + (tileSize / 2f);

        // Loops through all root GameObjects
        foreach (var rootObjs in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            // Ignore the itemdatabase
            if (rootObjs.name == "ItemDatabase")
                continue;

            // Loops through child objects
            // We have a parent object for the different parts of the scene, so we can't only loop through root objects
            foreach (Transform child in rootObjs.transform)
            {
                var obj = child.gameObject;

                // We want to ignore objects that should never be disabled
                if (obj.tag != "Player" && obj.tag != "MainCamera" && obj.tag != "AlwaysEnabled")
                {
                    float xDistance = obj.transform.position.x;
                    float zDistance = obj.transform.position.z;

                    // If the object is within the tile's boundaries, add it to the list of objects for this tile
                    if ((xDistance >= this.myPos.x - (tileSize.x / 2f) && zDistance >= this.myPos.z - (tileSize.z / 2f)) &&
                        (xDistance >= this.myPos.x - (tileSize.x / 2f) && zDistance <= this.myPos.z + (tileSize.z / 2f)) &&
                        (xDistance <= this.myPos.x + (tileSize.x / 2f) && zDistance >= this.myPos.z - (tileSize.z / 2f)) &&
                        (xDistance <= this.myPos.x + (tileSize.x / 2f) && zDistance <= this.myPos.z + (tileSize.z / 2f))
                        )
                        this.objects.Add(obj);
                }
            }
        }
	}

    public void ToggleTile(bool active)
    {
        // Toggles collider, trees and foliage, and all objects within the tile
        // Helps reducing CPU and GPU load reducing objects rendered, and code that's running
        this.gameObject.GetComponent<TerrainCollider>().enabled = active;
        this.gameObject.GetComponent<Terrain>().drawTreesAndFoliage = active;
        foreach (var obj in this.objects)
        {
            if (obj != null)
                obj.SetActive(active);
        }
    }
}
