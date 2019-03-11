using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable]
public class Inventory : NetworkBehaviour {
    private NetworkIdentity objNetId;

    public GameObject player;

    public InputManager inputManager;
    public GameObject optionsMenu;

    [System.Serializable]
    public class ListItem
    {
        [SerializeField]
        public string name { get; set; }
        [SerializeField]
        public int amount { get; set; }

        public ListItem(string n, int a)
        {
            name = n;
            amount = a;
        }
    }
    
    public ItemDatabase itemDatabase;      // We can use itemDatabase.getItem(string name) to get prefab of item with that name
    public List<ListItem> itemInventory;     // List of items the user have.

    public GameObject inventoryHUD;
    

    void Start ()
    {
        // Turn big inventory UI off to begin with.
        inventoryHUD.transform.Find("Inventory").gameObject.SetActive(false);

        itemDatabase = GameObject.FindObjectOfType<ItemDatabase>();
        //itemDatabase.initPrefabs(); 

        itemInventory = new List<ListItem>();

        inputManager.RegisterAction(InputManager.Keys.inventory, OpenInventory, GetInstanceID());

        //TODO If the user has saved his inventory data in a file, those can be loaded from here:
        //itemInventory.Add("axe");

        SetupHUD(); // Setup inventory HUD after items has been loaded.
    }

    public void OpenInventory()
    {
        if (!optionsMenu.activeSelf)
        {
            // Enable/disable UI for inventory:
            GameObject InventoryUI = inventoryHUD.transform.Find("Inventory").gameObject;
            if (InventoryUI.activeSelf)
                InventoryUI.SetActive(false);
            else
                InventoryUI.SetActive(true);
        }
    }

    void Update ()
    {
        UpdateHUD();// TODO make this one be called less often. :P

        if (Input.GetKeyDown(KeyCode.G))    // Throw away item on ground.
        {
            Transform handContainer = player.transform.Find("Player").Find("HandContainer").transform;
            try
            {
                //Get item:
                GameObject item = handContainer.transform.GetChild(0).gameObject;

                //Take item out of hands:
                item.transform.parent = null;

                // Put item on ground:
                item.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.43f, this.transform.position.z) + this.transform.forward * 0.5f + this.transform.right * 0.3f;

                // Enable stuff again:
                item.GetComponent<Collider>().enabled = true;
                item.transform.Find("ItemText").GetComponent<MeshRenderer>().enabled = true;
            } 
            catch
            {
                //Debug.Log("Exception throwing away item: " + e);
            }
        }

        // If player is holding anything, put it in the inventory:
        if (Input.GetKeyDown(KeyCode.Alpha0))   
        {
            PutItemIn();
        }

        // Equip item in inventory spot nr 1.   
        if (Input.GetKeyDown(KeyCode.Alpha1))   
        {
            PutItemIn();  // If current item in hands is something else than item in spot nr 1, it will be taken into inventory.
            try
            {
                PutItemOut(itemInventory[0].name, 0);   // Puts item from inventory out to hands.
            } catch
            {
                // Don't do anything if there is items in inventory spot 0.
                Debug.Log("No item in inventory spot 0");
            }
        }
        // Equip item in inventory spot nr 2.   
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PutItemIn();  // If current item in hands is something else than item in spot nr 1, it will be removed.
            try
            {
                PutItemOut(itemInventory[1].name, 1);   // Puts item from inventory out to hands.
            }
            catch
            {
                // Don't do anything if there is items in inventory spot 1.
                Debug.Log("No item in inventory spot 1");
            }
        }
        // Equip item in inventory spot nr 3.   
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PutItemIn();  // If current item in hands is something else than item in spot nr 1, it will be removed.
            try
            {
                PutItemOut(itemInventory[2].name, 2);   // Puts item from inventory out to hands.
            }
            catch
            {
                // Don't do anything if there is items in inventory spot 2.
                Debug.Log("No item in inventory spot 2");
            }
        }
        // Equip item in inventory spot nr 4.
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PutItemIn();  // If current item in hands is something else than item in spot nr 1, it will be removed.
            try
            {
                PutItemOut(itemInventory[3].name, 3);   // Puts item from inventory out to hands.
            }
            catch
            {
                // Don't do anything if there is items in inventory spot 3.
                Debug.Log("No item in inventory spot 3");
            }
        }
        // Equip item in inventory spot nr 5.
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PutItemIn();  // If current item in hands is something else than item in spot nr 1, it will be removed.
            try
            {
                PutItemOut(itemInventory[4].name, 4);   // Puts item from inventory out to hands.
            }
            catch
            {
                // Don't do anything if there is items in inventory spot 4.
                Debug.Log("No item in inventory spot 4");
            }
        }
        // Equip item in inventory spot nr 6.
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            PutItemIn();  // If current item in hands is something else than item in spot nr 1, it will be removed.
            try
            {
                PutItemOut(itemInventory[5].name, 5);   // Puts item from inventory out to hands.
            }
            catch
            {
                // Don't do anything if there is items in inventory spot 5.
                Debug.Log("No item in inventory spot 5");
            }
        }
        // Equip item in inventory spot nr 7.
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            PutItemIn();  // If current item in hands is something else than item in spot nr 1, it will be removed.
            try
            {
                PutItemOut(itemInventory[6].name, 6);   // Puts item from inventory out to hands.
            }
            catch
            {
                // Don't do anything if there is items in inventory spot 6.
                Debug.Log("No item in inventory spot 6");
            }
        }
        // Equip item in inventory spot nr 8.
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            PutItemIn();  // If current item in hands is something else than item in spot nr 1, it will be removed.
            try
            {
                PutItemOut(itemInventory[7].name, 7);   // Puts item from inventory out to hands.
            }
            catch
            {
                // Don't do anything if there is items in inventory spot 7.
                Debug.Log("No item in inventory spot 7");
            }
        }
        // Equip item in inventory spot nr 9.   
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            PutItemIn();  // If current item in hands is something else than item in spot nr 1, it will be removed.
            try
            {
                PutItemOut(itemInventory[8].name, 8);   // Puts item from inventory out to hands.
            }
            catch
            {
                // Don't do anything if there is items in inventory spot 8.
                Debug.Log("No item in inventory spot 8");
            }
        }
    }

    void PutItemOut(string itemName, int itemNr)   // Puts given item OUT of inventory into hands
    {
        // TODO check itemType, for different purposes. 
        try
        {
            Vector3 position = player.transform.position;
            Vector3 forward = player.transform.forward;

            GameObject inventory = player.transform.Find("Player").Find("InventoryContainer").gameObject;  // Need to work more with inventory system...
            GameObject handObject = null;
            try // Get item in inventory, and set it into hands:
            {
                // Go through all items to find item with name == itemName:
                for (int i = 0; i < inventory.transform.childCount; i++)
                {
                    GameObject item = inventory.transform.GetChild(i).gameObject;
                    if (item.GetComponent<Item>().itemName == itemName)
                    {
                        handObject = (GameObject)item;
                        // If there is more than 1 amount of an item only "use up" 1 at a time.

                        //Get position of itemIn inventory with name:
                        if (itemInventory[itemNr].amount > 1)
                        {
                            itemInventory[itemNr].amount--;
                            break;
                        }
                        else
                        {
                            itemInventory.RemoveAt(itemNr); // Item is no longer in inventory when taking out of hands, if taking item back into inventory = put back in.
                            break;
                        }
                    }
                }

                if (isLocalPlayer)
                {
                    CmdTakeOutHeldItem(handObject, player);
                }

                //handObject.transform.parent = player.transform.Find("Player").Find("HandContainer").transform;
                //handObject.transform.position = player.transform.position + 0.5f * player.transform.forward + 0.39f * player.transform.right + 0.15f * player.transform.up;
                //handObject.transform.forward = player.transform.forward;
                //// Make the object visible again:
                //handObject.gameObject.SetActive(true);
                //player.gameObject.GetComponent<PlayerMultiPlayer>().handObject = handObject;
            }
            catch
            {
                handObject = null;//hmmmTODO fix
                itemName = itemName + "(Clone)";
                // Go through all items to find item with name == itemName:
                for (int i = 0; i < inventory.transform.childCount; i++)
                {
                    GameObject item = inventory.transform.GetChild(i).gameObject;
                    if (item.GetComponent<Item>().itemName == itemName)
                    {
                        handObject = (GameObject)item;
                        // If there is more than 1 amount of an item only "use up" 1 at a time.
                        Debug.Log("name:" + itemInventory[i].name + " amount: " + itemInventory[i].amount);
                        if (itemInventory[itemNr].amount > 1)
                        {
                            itemInventory[itemNr].amount--;
                            break;
                        }
                        else
                        {
                            itemInventory.RemoveAt(itemNr); // Item is no longer in inventory when taking out of hands, if taking item back into inventory = put back in.
                            break;
                        }
                    }
                }

                if (isLocalPlayer)
                {
                    CmdTakeOutHeldItem(handObject, player);
                }

                //handObject.transform.parent = player.transform.Find("Player").Find("HandContainer").transform;
                //handObject.transform.position = player.transform.position + 0.5f * player.transform.forward + 0.39f * player.transform.right + 0.15f * player.transform.up;
                //handObject.transform.forward = player.transform.forward;
                //// Make the object visible again:
                //handObject.gameObject.SetActive(true);
                //player.gameObject.GetComponent<PlayerMultiPlayer>().handObject = handObject;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log("No item in spot nr 1.");
        }
    }

    void PutItemIn()  // Puts any item INto inventory from hands.
    {
        Transform handContainer = player.transform.Find("Player").Find("HandContainer").transform;
        try
        {
            Transform item = handContainer.transform.GetChild(0);
            if (isLocalPlayer)
            {
                CmdPutInHeldItem(item.gameObject, player);
            }
            //item.parent = player.transform.Find("Player").Find("InventoryContainer").gameObject.transform;
            //item.gameObject.SetActive(false); // Make the object "invisible" as if it was in the backpack.

            // Set item into inventory again:

            // Add item to inventory:
            bool inList = false;

            // Check if already owning that kind of item:
            for (int i = 0; i < itemInventory.Count; i++) 
            {
                if (itemInventory[i].name == item.GetComponent<Item>().itemName)
                {
                    inList = true;
                    itemInventory[i].amount += 1;
                    break;
                }
            }

            if (!inList)    // Add new item to inventory:
            {
                itemInventory.Add(new ListItem(item.GetComponent<Item>().itemName, 1));  // Possibly add x amount when getting an item. default to 1.
            }

            item.GetComponent<Collider>().enabled = false;
        }
        catch
        {
            // No item in hands to remove, don't do anything then.
        }
    }
    
    void SetupHUD() // If the player has items in inventory when starting the game they will be set in inventory HUD here.
    {
        // Get amount of inventoryUI slots to fill:
        int amountOfUISlots = inventoryHUD.transform.Find("Inventory").gameObject.transform.childCount;

        // loop through each of them and try-catch getting itemInventory[i].image:  
        for (int i = 0; i < amountOfUISlots; i++)
        {
            string itemName;
            int itemAmount;

            if (i < itemInventory.Count)
            {
                itemName = itemInventory[i].name;
                itemAmount = itemInventory[i].amount;

                Sprite currentImage = itemDatabase.GetSprite(itemName);
                inventoryHUD.transform.Find("Inventory").gameObject.transform.GetChild(i).gameObject.transform.Find("Border").gameObject.transform.Find("ItemImage").GetComponent<Image>().sprite = currentImage;
                inventoryHUD.transform.Find("SmallInventory").gameObject.transform.GetChild(i).gameObject.transform.Find("Border").gameObject.transform.Find("Amount").GetComponent<Text>().text = itemAmount.ToString();
            }
        }
        
        // Setup items for the inventory HUD that shows all the time.
        for (int i = 0; i < 9; i++)
        {
            string itemName;
            int itemAmount;
            if (i < itemInventory.Count)
            {
                itemName = itemInventory[i].name;
                itemAmount = itemInventory[i].amount;

                Sprite currentImage = itemDatabase.GetSprite(itemName);
                inventoryHUD.transform.Find("SmallInventory").gameObject.transform.GetChild(i).gameObject.transform.Find("Border").gameObject.transform.Find("ItemImage").GetComponent<Image>().sprite = currentImage;
                inventoryHUD.transform.Find("SmallInventory").gameObject.transform.GetChild(i).gameObject.transform.Find("Border").gameObject.transform.Find("Amount").GetComponent<Text>().text = itemAmount.ToString();
            }
        }
    }

    void UpdateHUD()    // add/remove items from inventory hud depending on what is in the itemInventory list.
    {
        // Get amount of inventoryUI slots to fill:
        int amountOfUISlots = inventoryHUD.transform.Find("Inventory").gameObject.transform.childCount;

        // loop through each of them and try-catch getting itemInventory[i].image:
        for (int i = 0; i < amountOfUISlots; i++)
        {
            string itemName;
            int itemAmount;
            if (i < itemInventory.Count)
            {
                itemName = itemInventory[i].name;
                itemAmount = itemInventory[i].amount;

                Sprite currentImage = itemDatabase.GetSprite(itemName);
                inventoryHUD.transform.Find("Inventory").gameObject.transform.GetChild(i).gameObject.transform.Find("Border").gameObject.transform.Find("ItemImage").GetComponent<Image>().sprite = currentImage;
                inventoryHUD.transform.Find("Inventory").gameObject.transform.GetChild(i).gameObject.transform.Find("Border").gameObject.transform.Find("Amount").GetComponent<Text>().text = itemAmount.ToString();
            }
            else
            {
                inventoryHUD.transform.Find("Inventory").gameObject.transform.GetChild(i).gameObject.transform.Find("Border").gameObject.transform.Find("ItemImage").GetComponent<Image>().sprite = null;
                inventoryHUD.transform.Find("Inventory").gameObject.transform.GetChild(i).gameObject.transform.Find("Border").gameObject.transform.Find("Amount").GetComponent<Text>().text = "";
            }
        }
        
        // Setup items for the inventory HUD that shows all the time.
        for (int i = 0; i < 9; i++)
        {
            string itemName;
            int itemAmount;
            if (i < itemInventory.Count)
            {
                itemName = itemInventory[i].name;
                itemAmount = itemInventory[i].amount;

                Sprite currentImage = itemDatabase.GetSprite(itemName);
                inventoryHUD.transform.Find("SmallInventory").gameObject.transform.GetChild(i).gameObject.transform.Find("Border").gameObject.transform.Find("ItemImage").GetComponent<Image>().sprite = currentImage;
                inventoryHUD.transform.Find("SmallInventory").gameObject.transform.GetChild(i).gameObject.transform.Find("Border").gameObject.transform.Find("Amount").GetComponent<Text>().text = itemAmount.ToString();
            }
            else
            {
                inventoryHUD.transform.Find("SmallInventory").gameObject.transform.GetChild(i).gameObject.transform.Find("Border").gameObject.transform.Find("ItemImage").GetComponent<Image>().sprite = null;
                inventoryHUD.transform.Find("SmallInventory").gameObject.transform.GetChild(i).gameObject.transform.Find("Border").gameObject.transform.Find("Amount").GetComponent<Text>().text = "";
            }
        }

    }

    void OnTriggerEnter(Collider other)   // for popping up text for itemName.
    {
        if (other.tag == "weapon")
        {
            ItemPickup pickup = other.gameObject.transform.Find("ItemText").GetComponent<ItemPickup>();
            pickup.SetTextActive(true);
        }
    }

    void OnTriggerExit(Collider other)    // remove text popping up for itemName.
    {
        if (other.tag == "weapon")
        {
            ItemPickup pickup = other.gameObject.transform.Find("ItemText").GetComponent<ItemPickup>();
            pickup.SetTextActive(false);
        }
    }

    void OnTriggerStay(Collider other)
    {
        // Gives user ability to pick up item:
        if (other.GetComponent<Collider>().gameObject.layer == LayerMask.NameToLayer("Item"))   
        {
            // Rotate text towards player:
            Vector3 targetDirection = other.gameObject.transform.position - transform.position;
            other.gameObject.transform.Find("ItemText").GetComponent<ItemPickup>().RotateText(targetDirection);

            if (Input.GetKeyDown(KeyCode.F))    // Pick up items with button F.
            {
                // Add item to inventory:
                bool inList = false;
                
                for (int i = 0; i < itemInventory.Count; i++)   // Check if already owning that kind of item:
                {
                    if (itemInventory[i].name == other.GetComponent<Item>().itemName)
                    {
                        inList = true;
                        itemInventory[i].amount += 1;   
                    }
                }

                if (!inList)    // Add new item to inventory:
                {
                    itemInventory.Add(new ListItem(other.GetComponent<Item>().itemName, 1));  // Possibly add x amount when getting an item. default to 1.
                }
                
                CmdSpawnItem(other.GetComponent<Item>().itemName, player);

                Destroy(other.gameObject);
            }
        }
    }

    public void OnInventoryButtonPress(int buttonId)
    {
        Debug.Log("You pressed button: " + buttonId);
    }

    // Tells all clients connected to add the item to the players inventory
    [ClientRpc]
    void RpcSpawnItem(GameObject item, GameObject player)
    {
        item.transform.parent = player.transform.Find("Player").Find("InventoryContainer").gameObject.transform;
        item.gameObject.SetActive(false);
        
        item.gameObject.GetComponent<Collider>().enabled = false; // Disable collider until item is not in inventory.
        item.gameObject.transform.Find("ItemText").GetComponent<MeshRenderer>().enabled = false; // Disable itemText.
    }

    // Tells the server to spawn an item, and to tell the other players what happened
    [Command]
    void CmdSpawnItem(string name, GameObject player)
    {
        GameObject item = Instantiate(FindObjectOfType<ItemDatabase>().GetItem(name));
        NetworkServer.SpawnWithClientAuthority(item, connectionToClient);
        RpcSpawnItem(item, player);
    }

    // Tells all connected clients that the player has put an item into its hand, so other players can see the item
    [ClientRpc]
    void RpcTakeOutHeldItem(GameObject handObject, GameObject player)
    {
        handObject.transform.parent = player.transform.Find("Player").Find("HandContainer").transform;
        handObject.transform.position = player.transform.position + 0.5f * player.transform.forward + 0.39f * player.transform.right + 0.15f * player.transform.up;
        handObject.transform.forward = player.transform.forward;

        handObject.SetActive(true);
    }

    // Tells all connected clients that the player has put away the item from its hand, so other players stop seeing the item
    [ClientRpc]
    void RpcPutInHeldItem(GameObject handObject, GameObject player)
    {
        handObject.transform.parent = player.transform.Find("Player").Find("InventoryContainer").gameObject.transform;
        handObject.SetActive(false);   
    }

    // Tells the server that a player wants to move an item to its hand
    [Command]
    void CmdTakeOutHeldItem(GameObject handObject, GameObject player)
    {
        objNetId = handObject.GetComponent<NetworkIdentity>();
        objNetId.AssignClientAuthority(connectionToClient);
        RpcTakeOutHeldItem(handObject, player);
        objNetId.RemoveClientAuthority(connectionToClient);
    }

    // Tells the server that a player wants to move an item from its hand to its inventory
    [Command]
    void CmdPutInHeldItem(GameObject handOject, GameObject player)
    {
        objNetId = handOject.GetComponent<NetworkIdentity>();
        objNetId.AssignClientAuthority(connectionToClient);
        RpcPutInHeldItem(handOject, player);
        objNetId.RemoveClientAuthority(connectionToClient);
    }
}
