using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BoidScript : MonoBehaviour {
    // Individual velocity
    public float velocity = 10f;

    // Storage of vectors, needed for multithreading
    public Vector3 oldPos;
    public Vector3 oldForward;
    
    public Vector3 newForward;
    
    public Vector3 stearing = Vector3.zero;

    // The different flocking rules
    Vector3 cohesion;
    Vector3 allignment;
    Vector3 separation;
    Vector3 goal;
    
    private void FixedUpdate()
    {
        // Updates transform with new values
        transform.forward = newForward;
        transform.position = transform.position + transform.forward * velocity * Time.fixedDeltaTime;

        // Updates the old vectors
        oldPos = transform.position;
        oldForward = transform.forward;
    }

    public void BoidFunc(FlockInfo flockInfo)
    {
        // zeroing out vectors
        cohesion = Vector3.zero;
        allignment = Vector3.zero;
        separation = Vector3.zero;
        stearing = Vector3.zero;

        // Loops through every boids positions, forward and checks the distance
        for (int i = 0; i < flockInfo.count; i++)
        {
            cohesion += flockInfo.boidsPos[i];
            allignment += flockInfo.boidsForward[i];
            separation += Separation(flockInfo.boidsPos[i], flockInfo.distance);
        }

        // Calculates cohesion rule vector
        cohesion = cohesion / flockInfo.count;
        cohesion = (cohesion - oldPos) / 50f;

        // Calculates allignment rule vector
        allignment = allignment / (flockInfo.count - 1);
        allignment = (allignment - oldForward) / 4f;

        // Calculates seperation rule vector
        separation = separation / 5f;

        // Calculates avoidance rule vector
        stearing = stearing / 2f;

        // Calculates goal rule vector
        goal = flockInfo.flockPos - oldPos;
        goal = goal / 50f;

        // Multiplies every rule with a weight for changing behaviour
        cohesion = cohesion * flockInfo.cohesionWeight;
        allignment = allignment * flockInfo.allignmentWeight;
        separation = separation * flockInfo.separationWeight;
        stearing = stearing * flockInfo.stearingWeight;
        goal = goal * flockInfo.goalWeight;

        // Lerps between the old direction and the new direction over time to reduce jitter and snapping
        newForward = Vector3.Lerp(oldForward, cohesion + allignment + separation + stearing + goal, flockInfo.dt);
    }

    // Function for calculating seperation vector
    private Vector3 Separation(Vector3 t, float dist)
    {
        // Uses SqrMagnitude for a slight performance boost
        if (Vector3.SqrMagnitude(oldPos - t) < dist)
            return (oldPos - t);

        return Vector3.zero;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Boids")
        {
            // Tries to avoid anything with a mesh collider to avoid passing through objects
            if (other.gameObject.GetComponents<MeshCollider>() == null)
            {
                Vector3 hit = other.ClosestPoint(transform.position);
                Debug.DrawLine(transform.position, hit);
                stearing = transform.position - hit;
            }
        }
    }
}