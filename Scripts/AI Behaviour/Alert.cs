using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : NPCBaseFSM
{

   // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
   override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      base.OnStateEnter(animator, stateInfo, layerIndex);
      statusFlagMeshRenderer.material.color = Color.yellow;
   }

   // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
   override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        // Returns if no player
        if (opponent == null)
        {
            animator.SetBool("InAlertState", false);
            animator.SetBool("ReturnToPatrol", true);
            return;
        }

        if (healthBar.value <= 0) return;   //if enemy is dead, no updates are run

      agent.SetDestination(NPC.transform.position);   //stop the movement of the enemy

      var direction = opponent.transform.position - NPC.transform.position;
      NPC.transform.rotation = Quaternion.Slerp(NPC.transform.rotation,
                                                Quaternion.LookRotation(direction),
                                                rotSpeed * Time.deltaTime);

      if (Vector3.Distance(opponent.transform.position, NPC.transform.position) <= chaseDistance)
      {
         animator.SetBool("InChaseRange", true);
         animator.SetBool("InChase", true);
      }
      else if (Vector3.Distance(opponent.transform.position, NPC.transform.position) > chaseDistance
               && Vector3.Distance(opponent.transform.position, NPC.transform.position) < attentionDistance)
      {
         animator.SetBool("InChaseRange", false);
         animator.SetBool("InChase", false);
         animator.SetBool("InAlertState", true);
      }

      else if (Vector3.Distance(opponent.transform.position, NPC.transform.position) > attentionDistance)
      {
         animator.SetBool("InAlertState", false);
         animator.SetBool("ReturnToPatrol", true);
      }


   }

   // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
   override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {

   }

   // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
   //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
   //
   //}

   // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
   //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
   //
   //}
}
