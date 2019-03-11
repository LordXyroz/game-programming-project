using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : NPCBaseFSM
{
   // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
   override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      base.OnStateEnter(animator, stateInfo, layerIndex);
      statusFlagMeshRenderer.material.color = Color.black;
   }

   // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
   override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (opponent == null)
        {
            animator.SetBool("InChaseRange", false);
            animator.SetBool("InChase", false);
            animator.SetBool("InAlertState", true);
            return;
        }

        if (healthBar.value <= 0) return;   //if enemy is dead, no updates are run

      ///////////////////////////// For use with NavMesh       /////////////////////////////
      agent.SetDestination(opponent.transform.position);
      ///////////////////////////// Fore use without NavMesh   /////////////////////////////
      /*var direction = opponent.transform.position - NPC.transform.position;
      direction.y = 0;
      NPC.transform.rotation = Quaternion.Slerp(NPC.transform.rotation,
                                                Quaternion.LookRotation(direction),
                                                rotSpeed * Time.deltaTime);
      NPC.transform.Translate(0, 0, Time.deltaTime * enemyMovementSpeed);*/
      /////////////////////////////                            /////////////////////////////

      if (Vector3.Distance(opponent.transform.position, NPC.transform.position) <= attackDistance)
      {
         animator.SetBool("InAttackRange", true);
         animator.SetBool("InChase", false);
      }

      else if (Vector3.Distance(opponent.transform.position, NPC.transform.position) > chaseDistance)
      {
         animator.SetBool("InChaseRange", false);
         animator.SetBool("InChase", false);

         if (Vector3.Distance(opponent.transform.position, NPC.transform.position) < attentionDistance)
         {
            animator.SetBool("InAlertState", true);
         }
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
