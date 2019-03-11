using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine;

public class EnemyAI : NetworkBehaviour
{
    // Debug
    public MeshRenderer statusFlagMeshRenderer;

    // UI elements
    public Slider healthBar;
    public float fadeTime = 6;
    public GameObject floatingText;

    // Waypoints this enemy is patrolling
    public GameObject[] waypoints;
    public bool randomPatrolRoute = true;
    public bool rangedUnit = false;

    // extra for ranged units
    public GameObject arrow;
    public GameObject bow;

    // distance parameters for enemy behaviour
    [Range(45, 180)]
    public float fovAngle = 120;
    [Range(5, 20)]
    public float attentionDistance = 10;
    [Range(1, 20)]
    public float chaseDistance = 7;
    [Range(0.6f, 20)]
    public float attackDistance = 0.7f;
    [Range(1, 100)]
    public int attackDamage = 10;
    [Range(0.1f, 10)]
    public float attackSpeed = 0.5f;
    [Range(1f, 1000.0f)]
    public float projectileSpeed = 50f;
    [Range(1f, 20.0f)]
    public float enemyMovementSpeed = 2.0f;
    [Range(1f, 100.0f)]
    public float enemyHealth = 50.0f;
    [Range(1.0f, 20.0f)]
    public float rotSpeed = 10f;
    [Range(1, 10)]
    public float accuracyWaypoint = 2.5f;  // must be higher than enemy movement speed

    Animator anim;

    [SyncVar]
    public GameObject player;

    void Fire()
    {
        GameObject projectile = Instantiate(arrow, bow.transform.position - (bow.transform.right), bow.transform.rotation);  // spawn arrow in front of the bat to avoid collider issues
        projectile.GetComponent<Rigidbody>().AddForce(-bow.transform.right * projectileSpeed * Time.deltaTime);
    }

    public void startFiring()
    {
        InvokeRepeating("Fire", attackSpeed, attackSpeed);
    }

    public void stopFiring()
    {
        CancelInvoke("Fire");
    }


    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        // disable renderer for debug items in play mode
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i].GetComponent<Renderer>().enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Set distance to player for animator
        var players = GameObject.FindGameObjectsWithTag("Player");
        float distSqrd = 1000000f;
        foreach (var p in players)
        {
            var offset = transform.position - p.transform.position;
            float sqrLen = offset.sqrMagnitude;
            if (sqrLen < distSqrd)
            {
                distSqrd = sqrLen;
                player = p;
            }
        }

        if (player == null)
            return;

        anim.SetFloat("distance", Vector3.Distance(transform.position, player.transform.position));

        //////////////////////////////////  Debug distance representation
        Vector3 forwardAttention = this.transform.TransformDirection(Vector3.forward) * attentionDistance;
        Debug.DrawRay(this.transform.position, forwardAttention, Color.green);

        Vector3 forwardChase = this.transform.TransformDirection(Vector3.forward) * chaseDistance;
        Debug.DrawRay(this.transform.position, forwardChase, Color.red);
        //////////////////////////////////
        Vector3 rightSidedAttention = (Quaternion.Euler(0, fovAngle / 2, 0) * transform.forward) * attentionDistance;
        Vector3 leftSidedAttention = (Quaternion.Euler(0, -fovAngle / 2, 0) * transform.forward) * attentionDistance;
        Debug.DrawRay(this.transform.position, rightSidedAttention, Color.green);
        Debug.DrawRay(this.transform.position, leftSidedAttention, Color.green);

        Vector3 rightSideChase = (Quaternion.Euler(0, fovAngle / 2, 0) * transform.forward) * chaseDistance;
        Vector3 leftSideChase = (Quaternion.Euler(0, -fovAngle / 2, 0) * transform.forward) * chaseDistance;
        Debug.DrawRay(this.transform.position, rightSideChase, Color.red);
        Debug.DrawRay(this.transform.position, leftSideChase, Color.red);
        //////////////////////////////////
    }
}
