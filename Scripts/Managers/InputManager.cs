using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class InputManager : MonoBehaviour {

    // Enum for storing all our custom keys
    public enum Keys
    {
        escape,
        forward,
        backward,
        left,
        right,
        jump,
        mouse0,
        mouse1,
        toggleView,
        movement,
        interract,
        inventory,

        stopMovement,

        actionbar1,
        actionbar2,
        actionbar3,
        actionbar4,
        actionbar5,
        actionbar6,
        actionbar7,
        actionbar8,
        actionbar9,
    };

    // Class for storing all the functions/actions to be called on a key press
    [System.Serializable]
    public class Actions
    {
        // Utilizing Dictionaries where int is the InstanceID of the object adding an action
        public Dictionary<int, Action> escape;
        public Dictionary<int, Action> forward;
        public Dictionary<int, Action> backward;
        public Dictionary<int, Action> left;
        public Dictionary<int, Action> right;
        public Dictionary<int, Action> jump;
        public Dictionary<int, Action> mouse0;
        public Dictionary<int, Action> mouse1;
        public Dictionary<int, Action> toggleView;
        public Dictionary<int, Action> movement;
        public Dictionary<int, Action> interract;
        public Dictionary<int, Action> inventory;

        public Dictionary<int, Action> stopMovement;

        public Dictionary<int, Action> actionbar1;
        public Dictionary<int, Action> actionbar2;
        public Dictionary<int, Action> actionbar3;
        public Dictionary<int, Action> actionbar4;
        public Dictionary<int, Action> actionbar5;
        public Dictionary<int, Action> actionbar6;
        public Dictionary<int, Action> actionbar7;
        public Dictionary<int, Action> actionbar8;
        public Dictionary<int, Action> actionbar9;
    };

    // Data for if some other code needs info of what key has been pressed etc
    public struct CallbackData
    {
        public float xAxis;
        public float yAxis;

        public Keys key;
        public string button;
    };

    // Dictionary binding our internal keys with Unity's KeyCodes
    public Dictionary<Keys, KeyCode> keybindings = new Dictionary<Keys, KeyCode>();

    [SerializeField]
    public Actions actions;
    public CallbackData data;

    public void Init()
    {
        // Loops through every list in actions, and initializes them as new dictionaries.
        // Instead of writing many similar lines of code to initialize every dictionaries.
        foreach (var temp in actions.GetType().GetFields())
        {
            temp.SetValue(actions, new Dictionary<int, Action>());
        }

        // Sets default keybindings. TODO: load/save keybindings from file
        keybindings.Add(Keys.escape, KeyCode.Escape);
        keybindings.Add(Keys.forward, KeyCode.W);
        keybindings.Add(Keys.backward, KeyCode.S);
        keybindings.Add(Keys.left, KeyCode.A);
        keybindings.Add(Keys.right, KeyCode.D);
        keybindings.Add(Keys.jump, KeyCode.Space);
        keybindings.Add(Keys.mouse0, KeyCode.Mouse0);
        keybindings.Add(Keys.mouse1, KeyCode.Mouse1);
        keybindings.Add(Keys.toggleView, KeyCode.Z);
        keybindings.Add(Keys.interract, KeyCode.F);
        keybindings.Add(Keys.inventory, KeyCode.I);

        keybindings.Add(Keys.actionbar1, KeyCode.Alpha1);
        keybindings.Add(Keys.actionbar2, KeyCode.Alpha2);
        keybindings.Add(Keys.actionbar3, KeyCode.Alpha3);
        keybindings.Add(Keys.actionbar4, KeyCode.Alpha4);
        keybindings.Add(Keys.actionbar5, KeyCode.Alpha5);
        keybindings.Add(Keys.actionbar6, KeyCode.Alpha6);
        keybindings.Add(Keys.actionbar7, KeyCode.Alpha7);
        keybindings.Add(Keys.actionbar8, KeyCode.Alpha8);
        keybindings.Add(Keys.actionbar9, KeyCode.Alpha9);
    }


    /* Adds a function to be called when a key has been pressed:
       key is our internal key, action is the function to be called,
       and id is the registering object's GetInstanceID() value.
       The id is used in the dictionary as a key for finding the correct function, 
       to be removed if needed. 
       TODO: implement it through try/catch to catch exceptions */
    public void RegisterAction(Keys key, Action action, int id)
    {
        switch (key)
        {
            case Keys.escape:
                actions.escape.Add(id, action);
                break;
            case Keys.forward:
                actions.forward.Add(id, action);
                break;
            case Keys.backward:
                actions.backward.Add(id, action);
                break;
            case Keys.left:
                actions.left.Add(id, action);
                break;
            case Keys.right:
                actions.right.Add(id, action);
                break;
            case Keys.jump:
                actions.jump.Add(id, action);
                break;
            case Keys.mouse0:
                actions.mouse0.Add(id, action);
                break;
            case Keys.mouse1:
                actions.mouse1.Add(id, action);
                break;
            case Keys.toggleView:
                actions.toggleView.Add(id, action);
                break;
            case Keys.movement:
                actions.movement.Add(id, action);
                break;
            case Keys.interract:
                actions.interract.Add(id, action);
                break;
            case Keys.inventory:
                actions.inventory.Add(id, action);
                break;

            case Keys.stopMovement:
                actions.stopMovement.Add(id, action);
                break;

            case Keys.actionbar1:
                actions.actionbar1.Add(id, action);
                break;
            case Keys.actionbar2:
                actions.actionbar2.Add(id, action);
                break;
            case Keys.actionbar3:
                actions.actionbar3.Add(id, action);
                break;
            case Keys.actionbar4:
                actions.actionbar4.Add(id, action);
                break;
            case Keys.actionbar5:
                actions.actionbar5.Add(id, action);
                break;
            case Keys.actionbar6:
                actions.actionbar6.Add(id, action);
                break;
            case Keys.actionbar7:
                actions.actionbar7.Add(id, action);
                break;
            case Keys.actionbar8:
                actions.actionbar8.Add(id, action);
                break;
            case Keys.actionbar9:
                actions.actionbar9.Add(id, action);
                break;
            default:
                break;
        }
    }

    /* Removes a function added to a key's dictionary:
       key is our internal key, and id is the registering 
       object's GetInstanceID() value.
       TODO: implement it through try/catch to catch exceptions
     */
    public bool RemoveAction(Keys key, int id)
    {
        switch (key)
        {
            case Keys.escape:
                actions.escape.Remove(id);
                return true;
            case Keys.forward:
                actions.forward.Remove(id);
                return true;
            case Keys.backward:
                actions.backward.Remove(id);
                return true;
            case Keys.left:
                actions.left.Remove(id);
                return true;
            case Keys.right:
                actions.right.Remove(id);
                return true;
            case Keys.jump:
                actions.jump.Remove(id);
                return true;
            case Keys.mouse0:
                actions.mouse0.Remove(id);
                return true;
            case Keys.mouse1:
                actions.mouse1.Remove(id);
                return true;
            case Keys.toggleView:
                actions.toggleView.Remove(id);
                return true;
            case Keys.movement:
                actions.movement.Remove(id);
                return true;
            case Keys.interract:
                actions.interract.Remove(id);
                return true;
            case Keys.inventory:
                actions.inventory.Remove(id);
                return true;

            case Keys.stopMovement:
                actions.stopMovement.Remove(id);
                return true;

            case Keys.actionbar1:
                actions.actionbar1.Remove(id);
                return true;
            case Keys.actionbar2:
                actions.actionbar2.Remove(id);
                return true;
            case Keys.actionbar3:
                actions.actionbar3.Remove(id);
                return true;
            case Keys.actionbar4:
                actions.actionbar4.Remove(id);
                return true;
            case Keys.actionbar5:
                actions.actionbar5.Remove(id);
                return true;
            case Keys.actionbar6:
                actions.actionbar6.Remove(id);
                return true;
            case Keys.actionbar7:
                actions.actionbar7.Remove(id);
                return true;
            case Keys.actionbar8:
                actions.actionbar8.Remove(id);
                return true;
            case Keys.actionbar9:
                actions.actionbar9.Remove(id);
                return true;
            default:
                return false;
        }
    }

    // Updates our data struct with new values
    private void CreateData(Keys key)
    {
        
        if (key == Keys.forward)
            data.xAxis = 1.0f;
        else if (key == Keys.backward)
            data.xAxis = -1.0f;
        else if (key == Keys.left)
            data.yAxis = -1.0f;
        else if (key == Keys.right)
            data.yAxis = 1.0f;

        data.key = key;
    }

    // Deprecated version of previous function
    private void CreateData(string key)
    {
        data.xAxis = Input.GetAxis("Vertical");
        data.yAxis = Input.GetAxis("Horizontal");

        data.button = key;
    }

    // Function for changing a keybinding:
    // key us our internal key, keycode is Unity's keys.
    // Checks if the keycode has already been used so you don't get duplicate keybindings
    public bool AddKey(Keys key, KeyCode keycode)
    {
        if (keybindings.ContainsValue(keycode))
            return false;
        keybindings.Add(key, keycode);
        return true;
    }

    // Removes the current keybinding to our internal key,
    // and returns the old value bound to that.
    public KeyCode RemoveKey(Keys key)
    {
        KeyCode temp;

        temp = keybindings[key];
        keybindings.Remove(key);
        return temp;
    }

    // Update is called once per frame
    void Update () {
        // Try catch in case of missing functions or keybindings
        try
        {
            // Many if/else if statements to check for each button
            // Creates data based on keypress
            // Loops through every function bound to that key backwards,
            // incase of a function being removed as a result of a function call
            if (Input.GetKey(keybindings[Keys.forward]))
            {
                CreateData(Keys.forward);
                for (int i = actions.forward.Count - 1; i >= 0; i--)
                {
                    actions.forward.ElementAt(i).Value();
                }
            }
            else if (Input.GetKey(keybindings[Keys.backward]))
            {
                CreateData(Keys.backward);
                for (int i = actions.backward.Count - 1; i >= 0; i--)
                {
                    actions.backward.ElementAt(i).Value();
                }
            }
            else if (Input.GetKey(keybindings[Keys.left]))
            {
                CreateData(Keys.left);
                for (int i = actions.left.Count - 1; i >= 0; i--)
                {
                    actions.left.ElementAt(i).Value();
                }
            }
            else if (Input.GetKey(keybindings[Keys.right]))
            {
                CreateData(Keys.right);
                for (int i = actions.right.Count - 1; i >= 0; i--)
                {
                    actions.right.ElementAt(i).Value();
                }
            }
            // Special edge-case used to stop running animation
            else if ((!Input.GetKey(keybindings[Keys.forward])  && Input.GetKeyUp(keybindings[Keys.forward]))   ||
                     (!Input.GetKey(keybindings[Keys.backward]) && Input.GetKeyUp(keybindings[Keys.backward]))  ||
                     (!Input.GetKey(keybindings[Keys.left])     && Input.GetKeyUp(keybindings[Keys.left]))      ||
                     (!Input.GetKey(keybindings[Keys.right])    && Input.GetKeyUp(keybindings[Keys.right])))
            {
                for (int i = actions.stopMovement.Count - 1; i >= 0; i--)
                {
                    actions.stopMovement.ElementAt(i).Value();
                }
            }

            if (Input.GetKeyDown(keybindings[Keys.escape]))
            {
                CreateData(Keys.escape);

                for (int i = actions.escape.Count - 1; i >= 0; i--)
                {
                    actions.escape.ElementAt(i).Value();
                }
            }
            if (Input.GetKeyDown(keybindings[Keys.jump]))
            {
                CreateData(Keys.jump);
                for (int i = actions.jump.Count - 1; i >= 0; i--)
                {
                    actions.jump.ElementAt(i).Value();
                }
            }
            if (Input.GetKeyDown(keybindings[Keys.mouse0]))
            {
                CreateData(Keys.mouse0);
                for (int i = actions.mouse0.Count - 1; i >= 0; i--)
                {
                    actions.mouse0.ElementAt(i).Value();
                }
            }
            if (Input.GetKeyDown(keybindings[Keys.mouse1]))
            {
                CreateData(Keys.mouse1);
                for (int i = actions.mouse1.Count - 1; i >= 0; i--)
                {
                    actions.mouse1.ElementAt(i).Value();
                }
            }
            if (Input.GetKeyDown(keybindings[Keys.toggleView]))
            {
                CreateData(Keys.toggleView);
                for (int i = actions.toggleView.Count - 1; i >= 0; i--)
                {
                    actions.toggleView.ElementAt(i).Value();
                }
            }
            if (Input.GetKeyDown(keybindings[Keys.interract]))
            {
                CreateData(Keys.interract);
                for (int i = actions.interract.Count - 1; i >= 0; i--)
                {
                    actions.interract.ElementAt(i).Value();
                }
            }
            if (Input.GetKeyDown(keybindings[Keys.inventory]))
            {
                CreateData(Keys.inventory);
                for (int i = actions.inventory.Count - 1; i >= 0; i--)
                {
                    actions.inventory.ElementAt(i).Value();
                }
            }


            if (Input.GetKeyDown(keybindings[Keys.actionbar1]))
            {
                CreateData(Keys.actionbar1);
                for (int i = actions.actionbar1.Count - 1; i >= 0; i--)
                {
                    actions.actionbar1.ElementAt(i).Value();
                }
            }
            if (Input.GetKeyDown(keybindings[Keys.actionbar2]))
            {
                CreateData(Keys.actionbar2);
                for (int i = actions.actionbar2.Count - 1; i >= 0; i--)
                {
                    actions.actionbar2.ElementAt(i).Value();
                }
            }
            if (Input.GetKeyDown(keybindings[Keys.actionbar3]))
            {
                CreateData(Keys.actionbar3);
                for (int i = actions.actionbar3.Count - 1; i >= 0; i--)
                {
                    actions.actionbar3.ElementAt(i).Value();
                }
            }
            if (Input.GetKeyDown(keybindings[Keys.actionbar4]))
            {
                CreateData(Keys.actionbar4);
                for (int i = actions.actionbar4.Count - 1; i >= 0; i--)
                {
                    actions.actionbar4.ElementAt(i).Value();
                }
            }
            if (Input.GetKeyDown(keybindings[Keys.actionbar5]))
            {
                CreateData(Keys.actionbar5);
                for (int i = actions.actionbar5.Count - 1; i >= 0; i--)
                {
                    actions.actionbar5.ElementAt(i).Value();
                }
            }
            if (Input.GetKeyDown(keybindings[Keys.actionbar6]))
            {
                CreateData(Keys.actionbar6);
                for (int i = actions.actionbar6.Count - 1; i >= 0; i--)
                {
                    actions.actionbar6.ElementAt(i).Value();
                }
            }
            if (Input.GetKeyDown(keybindings[Keys.actionbar7]))
            {
                CreateData(Keys.actionbar7);
                for (int i = actions.actionbar7.Count - 1; i >= 0; i--)
                {
                    actions.actionbar7.ElementAt(i).Value();
                }
            }
            if (Input.GetKeyDown(keybindings[Keys.actionbar8]))
            {
                CreateData(Keys.actionbar8);
                for (int i = actions.actionbar8.Count - 1; i >= 0; i--)
                {
                    actions.actionbar8.ElementAt(i).Value();
                }
            }
            if (Input.GetKeyDown(keybindings[Keys.actionbar9]))
            {
                CreateData(Keys.actionbar9);
                for (int i = actions.actionbar9.Count - 1; i >= 0; i--)
                {
                    actions.actionbar9.ElementAt(i).Value();
                }
            }

            // Deprecated way of checking for the player moving
            if (Input.GetKey(keybindings[Keys.forward]) ||
                Input.GetKey(keybindings[Keys.backward]) ||
                Input.GetKey(keybindings[Keys.left]) ||
                Input.GetKey(keybindings[Keys.right]))
            {
                CreateData("Horizontal Vertical");
                
                for (int i = actions.movement.Count - 1; i >= 0; i--)
                {
                    actions.movement.ElementAt(i).Value();
                }
            }
        }
        catch (KeyNotFoundException e)
        {
            Debug.Log(e.Message);
        }
    }
}
