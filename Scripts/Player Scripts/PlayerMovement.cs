using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerMovement : NetworkBehaviour
{
    [Header("Important")]
    public InputManager inputManager;


    [Header("Transforms")]
    public Transform myTransform;
    public Transform mainCamTransform;
    public Transform uiCamTransform;


    [Header ("Movement")]
    public float jumpForce;
    private bool normalJump;
    private bool wallJump;

    [Header("Components")]
    public Rigidbody rb;
    public BoxCollider col;
    public PlayerStatController stats;
    public LayerMask groundLayer;

    [Header("Camera")]
    public Camera mainCam;
    public Camera uiCam;
    private Vector3 thirdPersonOffset;
    private Vector3 firstPersonOffset;
    private float turnSpeed = 90.0f;

    private bool view;      // Toggle FP, TP 

    [Header("Other")]
    public bool rotate = true;
    public bool move = true;
    public Animator animator;
   

    void Start()
    {
        // Animator
        //Debug.Log(GameObject.Find("playerCharacter").GetComponent<Animator>());
        //animator = GameObject.Find("playerCharacter").GetComponent<Animator>();

        // Player
        inputManager.Init();

        myTransform = this.transform;
        mainCamTransform = mainCam.transform;
        uiCamTransform = uiCam.transform;
        
        view = false;
        wallJump = true;

        // Camera:

        if (isLocalPlayer)
        {
            foreach (var temp in GetComponentsInChildren<MeshRenderer>())
            {
                if (temp.tag == "AlwaysEnabled" || temp.renderingLayerMask == LayerMask.NameToLayer("Item"))
                    continue;

                temp.enabled = view;
            }

            foreach (var temp in GetComponentsInChildren<SkinnedMeshRenderer>())
                temp.enabled = view;
        }

        thirdPersonOffset = new Vector3(0f, 1.5f, -3.0f);
        firstPersonOffset = new Vector3(0.0f, 0.5f, 0.0f);


        // Mouse:
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Input registration
        inputManager.RegisterAction(InputManager.Keys.toggleView, ToggleView, GetInstanceID());
        inputManager.RegisterAction(InputManager.Keys.jump, Jump, GetInstanceID());
        inputManager.RegisterAction(InputManager.Keys.forward, () => Move(inputManager.data), GetInstanceID());
        inputManager.RegisterAction(InputManager.Keys.backward, () => Move(inputManager.data), GetInstanceID());
        inputManager.RegisterAction(InputManager.Keys.left, () => Move(inputManager.data), GetInstanceID());
        inputManager.RegisterAction(InputManager.Keys.right, () => Move(inputManager.data), GetInstanceID());
        inputManager.RegisterAction(InputManager.Keys.stopMovement, OnStopMovement, GetInstanceID());
    }

    public void ToggleView()
    {
        // Toggles between 1st person and 3rd person
        // Disables player model in 1st person so you don't see inside the model
        if (isLocalPlayer)
        {
            view = !view;

            foreach (var temp in GetComponentsInChildren<MeshRenderer>())
            {
                if (!(temp.tag == "AlwaysEnabled" || temp.gameObject.layer == LayerMask.NameToLayer("Item")))
                    temp.enabled = view;   
            }
            foreach (var temp in GetComponentsInChildren<SkinnedMeshRenderer>())
                temp.enabled = view;

            if (view)
                mainCamTransform.localRotation = Quaternion.identity;
            else
                mainCamTransform.localRotation = Quaternion.identity;
        }
    }
    
    // Function to be called when all movement keys have been released
    // Only used to go to an idle animation
    public void OnStopMovement()
    {
        if (isLocalPlayer)
        {
            animator.ResetTrigger("Walk");
            animator.SetTrigger("Idle");
        }
    }

    // Function to be called when the jump key has been pressed
    public void Jump()
    {
        if (isLocalPlayer)
        {
            if (normalJump)
                rb.AddForce(new Vector3(0, 13.5f, 0));
        }
    }

    // Function to be called when movement keys have been pressed
    public void Move(InputManager.CallbackData data)
    {
        if (isLocalPlayer)
        {
            // If player is allowed to move
            if (move)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
                {
                    animator.ResetTrigger("Idle");
                    animator.SetTrigger("Walk");
                }
                
                var moveDir = Time.deltaTime * stats.movementSpeed * myTransform.forward * data.xAxis + Time.deltaTime * stats.movementSpeed * myTransform.right * data.yAxis;
                
                moveDir *= 1; //walkSpeed;

                //Player movement
                myTransform.position += moveDir;
            }
        }
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            // If player is allowed to rotate
            if (rotate)
            {
                float y = Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime;
                float x = Input.GetAxis("Mouse Y") * turnSpeed * Time.deltaTime;

                // Rotates the camera and player gameobject
                myTransform.Rotate(0f, y, 0f);
                mainCamTransform.Rotate(-x, 0f, 0f);

                // Creates a new offset based on the new rotation and previous offset
                thirdPersonOffset = Quaternion.AngleAxis(y, Vector3.up) * thirdPersonOffset;

                // Applies a camera offset based on view
                if (view)
                {
                    mainCamTransform.position = myTransform.position + thirdPersonOffset;
                }
                else
                {
                    mainCamTransform.position = myTransform.position + firstPersonOffset;
                }

                // Updates the UI camera to main camera's position and rotation
                uiCamTransform.position = mainCamTransform.position;
                uiCamTransform.rotation = mainCamTransform.rotation;
            }
            

            if (CheckGround())
            {
                wallJump = true;
                normalJump = true;
            }
            else
                normalJump = false;

            // Boundary:         // Should set spawn points.
            if (myTransform.position.y < -10)
                myTransform.position = new Vector3(myTransform.position.x, 1f, myTransform.position.z);
        }
    }


    void OnCollisionStay(Collision col)
    {
        ContactPoint contact = col.contacts[0];             //Walljump:

        if (Input.GetKey(KeyCode.Space) && contact.normal.y < 0.3 && wallJump       // walljump if a wall
            && contact.otherCollider.material.name != "invisibleWall (Instance)")  // && not invisible wall
        {
            rb.velocity = new Vector3(0, 0, 0); // These are for making sure the player doesn't rocket into the air a few times. Happens 1/10 tries when trying to walljump as high as possible in normal jump.
            RaycastHit hit;
            Physics.Raycast(contact.point, -Vector3.up, out hit);
            if (hit.distance > 0.1)
            {
                rb.velocity = new Vector3(0, 0, 0);
                Debug.DrawRay(contact.point, contact.normal, Color.red, 3f, true);
                rb.velocity += Vector3.up * 7.5f;
                wallJump = false;
            }
        }
    }

    bool CheckGround()
    {
        var temp1 = myTransform.position; temp1.x -= col.size.x / 2; temp1.z -= col.size.z / 2;
        var temp2 = myTransform.position; temp2.x += col.size.x / 2; temp2.z -= col.size.z / 2;
        var temp3 = myTransform.position; temp3.x -= col.size.x / 2; temp1.z += col.size.z / 2;
        var temp4 = myTransform.position; temp4.x += col.size.x / 2; temp1.z += col.size.z / 2;
        if (Physics.Raycast(temp1, Vector3.down, col.bounds.extents.y + 0.4f) || Physics.Raycast(temp2, Vector3.down, col.bounds.extents.y + 0.4f)
            || Physics.Raycast(temp3, Vector3.down, col.bounds.extents.y + 0.4f) || Physics.Raycast(temp4, Vector3.down, col.bounds.extents.y + 0.4f))
            return true;
        else
            return false;
    }

}