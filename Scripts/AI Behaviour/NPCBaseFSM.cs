using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCBaseFSM : StateMachineBehaviour
{
    // Debug
    [HideInInspector] public MeshRenderer statusFlagMeshRenderer = null;

    // Game object for self and opponent (player)
    [HideInInspector] protected GameObject NPC;
    [HideInInspector] public GameObject opponent;   //i.e. the player
    // Waypoint for patrolling and returning to patrol
    [HideInInspector] public GameObject[] waypointsPatrol;
    [HideInInspector] public int currentWaypoint = 0;
    //public Transform chaseTarget;

    // NavMesh elements
    [HideInInspector] public UnityEngine.AI.NavMeshAgent agent;

    // UI elements
    [HideInInspector] public Slider healthBar;
    [HideInInspector] public float fadeTime;
    [HideInInspector] public GameObject floatingText;

    // FoV parameters for only noticing opponents within field of view
    [HideInInspector] public float NPCFoV;
    [HideInInspector] public Vector3 opponentDirection;


    // parameters for enemy behaviour
    [HideInInspector] public bool randomPatrolRoute;
    [HideInInspector] public bool rangedUnit;
    [HideInInspector] protected float fovAngle;
    [HideInInspector] public float attentionDistance;
    [HideInInspector] public float chaseDistance;
    [HideInInspector] public float attackDistance;
    [HideInInspector] public int attackDamage;
    [HideInInspector] public float attackSpeed;
    [HideInInspector] public float enemyMovementSpeed;
    [HideInInspector] public float enemyHealth;
    [HideInInspector] public float rotSpeed;
    [HideInInspector] public float accuracyWaypoint;
    [HideInInspector] public float distanceToOpponent;


    int tempHP;

    



    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        // get the game objects 
        NPC = animator.gameObject;
        EnemyAI enemyAI = NPC.GetComponent<EnemyAI>();
        //player = GameObject.FindGameObjectWithTag("Player");
        //opponent = NPC.GetComponent<EnemyAI>().GetPlayer();
        //opponent = GameObject.FindGameObjectWithTag("Player");
        agent = NPC.GetComponent<UnityEngine.AI.NavMeshAgent>();


        // get values for the parameters for enemy behaviour
        opponent = enemyAI.player;

        randomPatrolRoute = enemyAI.randomPatrolRoute;
        rangedUnit = enemyAI.rangedUnit;
        fovAngle = enemyAI.fovAngle;
        attentionDistance = enemyAI.attentionDistance;
        chaseDistance = enemyAI.chaseDistance;
        attackDistance = enemyAI.attackDistance;
        attackDamage = enemyAI.attackDamage;
        attackSpeed = enemyAI.attackSpeed;
        enemyMovementSpeed = enemyAI.enemyMovementSpeed;
        enemyHealth = enemyAI.enemyHealth;
        rotSpeed = enemyAI.rotSpeed;
        accuracyWaypoint = enemyAI.accuracyWaypoint;

        waypointsPatrol = enemyAI.waypoints;

        // get UI elements
        healthBar = enemyAI.healthBar;
        floatingText = enemyAI.floatingText;
        fadeTime = enemyAI.fadeTime;


        // setting up start values 
        healthBar.maxValue = enemyHealth;
        healthBar.value = enemyHealth;
        animator.SetBool("IsDead", false);
        healthBar.gameObject.SetActive(false); // makes healthbar invisible if enemy is at full health

        // Debug ////////////////
        statusFlagMeshRenderer = new MeshRenderer();
        statusFlagMeshRenderer = enemyAI.statusFlagMeshRenderer;
        ////////////////////////
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        // Need to check for player in update incase another player is closer
        opponent = NPC.GetComponent<EnemyAI>().player;
        if (opponent == null)
            return;

        opponentDirection = opponent.transform.position - NPC.transform.position;
        distanceToOpponent = Vector3.Distance(opponent.transform.position, NPC.transform.position);
        NPCFoV = Vector3.Angle(opponentDirection, NPC.transform.forward);      // check if player is within FoV of enemy

        /*// if damage has been taken
        if (healthBar.value < tempHP)
        {
           takeDamage(tempHP - (int)healthBar.value);
           tempHP = (int)healthBar.value;
           }*/
        if (healthBar.value <= 0)
        {
           animator.SetBool("IsDead", true);
        }

    }

    public int closesWaypoint(GameObject[] waypointsToCheck)
    {
        float closestWaypointSqr = Mathf.Infinity;
        int closestWaypointTemp = 0;

        for (int i = 0; i < waypointsToCheck.Length; i++)
        {
            Vector3 directionToWaypoint = waypointsToCheck[i].transform.position - NPC.transform.position;
            float sqrToWaypoint = directionToWaypoint.sqrMagnitude;
            if (sqrToWaypoint < closestWaypointSqr)
            {
                closestWaypointSqr = sqrToWaypoint;
                closestWaypointTemp = i;
            }
        }
        //Debug.Log("----" + NPC.GetComponent<EnemyAI>().name + "Closest waypoint: " + currentWaypoint);
        return closestWaypointTemp;
    }

    public void takeDamage(int damage)
    {
        //damage particles   TODO

        //HP bar
        healthBar.gameObject.SetActive(true); //  set to true so HP bar gets displayed after damage is taken
        //healthBar.value -= damage;


        // Floating text
        Vector3 textLocation = new Vector3(NPC.transform.position.x, NPC.transform.position.y + 0.5f, NPC.transform.position.z);
        GameObject displayDmg = Instantiate(floatingText, textLocation, Quaternion.identity) as GameObject;
        displayDmg.GetComponent<FloatingTextScript>().damage = damage.ToString();
    }
}
