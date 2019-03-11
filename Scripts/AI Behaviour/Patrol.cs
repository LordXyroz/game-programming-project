using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : NPCBaseFSM
{
   // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
   override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      animator.SetBool("InPatrol", true);
      base.OnStateEnter(animator, stateInfo, layerIndex);
      currentWaypoint = closesWaypoint(waypointsPatrol);
      //player = GameObject.FindGameObjectWithTag("Player");

      statusFlagMeshRenderer.material.color = Color.green;
   }

   // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
   override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        if (healthBar.value <= 0) return;   //if enemy is dead, no updates are run

      if (waypointsPatrol.Length == 0) return;
      {
         if (Vector3.Distance(waypointsPatrol[currentWaypoint].transform.position, NPC.transform.position) < accuracyWaypoint)
         {
            if (randomPatrolRoute)
            {
               // for random patrol pattern
               currentWaypoint = Random.Range(0, waypointsPatrol.Length);
            }
            else
            {
               // for fixed patrol route pattern
               currentWaypoint++;
               if (currentWaypoint >= waypointsPatrol.Length)
               {
                  currentWaypoint = 0;
                  Debug.Log("Waypoint array reset - " + waypointsPatrol[currentWaypoint], waypointsPatrol[currentWaypoint]);
               }
            }
         }

         ///////////////////////////// For use with NavMesh       /////////////////////////////
         agent.SetDestination(waypointsPatrol[currentWaypoint].transform.position);
         ///////////////////////////// Fore use without NavMesh   /////////////////////////////
         // rotate towards target
         /*var waypointDirection = waypoints[currentWaypoint].transform.position - NPC.transform.position;
         NPC.transform.rotation = Quaternion.Slerp(NPC.transform.rotation,
                                                   Quaternion.LookRotation(waypointDirection),
                                                   rotSpeed * Time.deltaTime);
         NPC.transform.Translate(0, 0, Time.deltaTime * enemyMovementSpeed);*/
         /////////////////////////////                         /////////////////////////////
      }
        // Returns if no players
        if (opponent == null)
            return;

        if (Vector3.Distance(opponent.transform.position, NPC.transform.position) < attentionDistance
         && (NPCFoV < fovAngle))
      {
         // Raycast after the opponent (player) is within range and FoV in orther to not raycast for all enemies for every frame
         if (withinFoV())
         {
            animator.SetBool("InAlertState", true);
            animator.SetBool("InPatrol", false);
         }
      }
      else if (Vector3.Distance(opponent.transform.position, NPC.transform.position) > chaseDistance)
      {
         animator.SetBool("InAlertState", false);
      }
   }

   // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
   override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      animator.SetBool("InPatrol", false);
   }

   // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
   //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
   //
   //}

   // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
   //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
   //
   //}

   //public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
   //{
   //   base.OnStateMachineEnter(animator, stateMachinePathHash);
   //
   //}

   private bool withinFoV()
   {
      // Bit shift the index of the layer (2) to get a bit mask
      int layerMask = 1 << 2;

      // This would cast rays only against colliders in layer 2.
      // But instead we want to collide against everything except layer 2. The ~ operator does this, it inverts a bitmask.
      layerMask = ~layerMask;


      RaycastHit sightRay = new RaycastHit();
      Vector3 directionToTarget = (opponent.transform.position - NPC.transform.position);
      float angle = Vector3.Angle(directionToTarget, NPC.transform.forward);

      if (angle < fovAngle * 0.5f)  // if player within FoV (half on each side of "forward")
      {
         if (Physics.Raycast(NPC.transform.position, directionToTarget, out sightRay, NPC.GetComponent<EnemyAI>().attentionDistance, layerMask)) //true if raycast collides with player
         {
            if (sightRay.collider.CompareTag("Player"))
            {
               Debug.DrawRay(NPC.transform.position, directionToTarget * sightRay.distance, Color.yellow);
               return true;
            }
            else
            {
               Debug.DrawRay(NPC.transform.position, directionToTarget * 1, Color.blue);
               return false;
            }
         }
         else
         {
            Debug.DrawRay(NPC.transform.position, directionToTarget * 1, Color.blue);
            return false;
         }
      }
      return false;
   }
}
