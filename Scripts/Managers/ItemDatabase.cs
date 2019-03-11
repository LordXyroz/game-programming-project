using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemDatabase : NetworkBehaviour {

    // Prefabs of objects we want:
    public List<GameObject> prefabs;
    public List<Sprite> sprites;

    // This is a list of all items we have in the game which can be used.
    public List<GameObject> items = new List<GameObject>();

    // Initialize prefab into inventory or ItemDatabase.
    void InitPrefab(ref GameObject item, string name, string storagePosition, ref Sprite image)
    {
        //item = MonoBehaviour.Instantiate((GameObject)Resources.Load("Prefabs/" + name, typeof(GameObject)));
        //item.transform.SetParent(GameObject.Find(storagePosition).transform);
        //item.gameObject.SetActive(false);
        //image = Resources.Load<Sprite>("Images/" + name);
    }
    
    // All item prefabs are set with their data here:
    public void InitPrefabs()
    {
        //InitPrefab(ref axePrefab, "axe", "ItemDatabase", ref axeImage);
        //InitPrefab(ref swordPrefab, "sword", "ItemDatabase", ref swordImage);
        //if (swordImage == null)
        //    Debug.Log("sword image is null");



    }

    public void Start()
    {
        foreach (var prefab in prefabs)
        {
            items.Add(Instantiate(prefab, this.transform));
        }

        for (int i = 0; i < items.Count; i++)
        {
            NetworkServer.Spawn(items[i]);
            items[i].GetComponent<Item>().itemID = i;
            items[i].GetComponent<Item>().sprite = sprites[i];
            items[i].SetActive(false);
        }
    }

    public ItemDatabase ()
    {
        InitPrefabs();
        // Name, power, type, prefab
        //items.Add(new Item("potion",    20,  Item.ItemType.consumable));
        //items.Add(new Item("antidote",  20,  Item.ItemType.consumable));
        //items.Add(new Item("key",       0,  Item.ItemType.consumable));

        //items.Add(new Item("axe", 6, Item.ItemType.weapon, axePrefab, axeImage));
        //items.Add(new Item("sword", 10, Item.ItemType.weapon, swordPrefab, swordImage));
        //items.Add(new Item("simple sword", 5, Item.ItemType.weapon, simpleSwordPrefab, simpleSwordImage));
        //items.Add(new Item("normal sword", 10, Item.ItemType.weapon));
        //items.Add(new Item("super sword", 20, Item.ItemType.weapon));
        //items.Add(new Item("bow",       50, Item.ItemType.weapon));
        //items.Add(new Item("arrow",     0,  Item.ItemType.ammo
    }
   

    public GameObject GetItem(string name)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].GetComponent<Item>().itemName == name)
            {
                return items[i];
            }
        }
        return null;
    }

    public GameObject GetItem(int id)
    {
        foreach (var temp in items)
            if (temp.GetComponent<Item>().itemID == id)
                return temp;
        return null;
    }

    public Sprite GetSprite(string name)
    {
        foreach (var temp in items)
            if (temp.GetComponent<Item>().itemName == name)
                return temp.GetComponent<Item>().sprite;
        return null;
    }

    public Sprite GetSprite(int id)
    {
        foreach (var temp in items)
            if (temp.GetComponent<Item>().itemID == id)
                return temp.GetComponent<Item>().sprite;
        return null;
    }

}
