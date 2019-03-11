using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    
    // Different types can be used differently, items of type weapon can be wielded to attack,
    // a stack of capacity of ammo/consumables will disappear after being used once.
    // Spells can not be wielded nor "used up" like ammo/consumables, but it requires mana?

    // ItemPower has different uses for different itemTypes, for weapon/spells it gives dmg,
    // for ammo it gives a multiplier to the dmg from weapon? For consumable it could be amount of hp/mana gained from using the item,
    // or amount of strength gained permanently or for a minute or something.
    public int itemID;

    public string itemName;
    public ItemType itemType;
    public int itemPower;

    public Sprite sprite;

    //public Sprite image { get; }   // This image is for recognizing the item in the inventory UI. If no image is sent to the database a temp image will be set instead.

    public enum ItemType
    {
        weapon,
        consumable,
        ammo,
        quest,
        //hat
        //armor/clothing
        //shoes
        //idk
    }
}
