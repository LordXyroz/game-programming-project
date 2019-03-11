using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : NPCBaseFSM
{
   float timer = 0;

   // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
   override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      base.OnStateEnter(animator, stateInfo, layerIndex);

      statusFlagMeshRenderer.material.color = Color.red;
      timer = 0f;


      if (rangedUnit)
         NPC.GetComponent<EnemyAI>().startFiring();
   }

   // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
   override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        // Returns if no players
        if (opponent == null)
        {
            animator.SetBool("InAttackRange", false);
            animator.SetBool("InChase", true);
            return;
        }

        if (healthBar.value <= 0) return;   //if enemy is dead, no updates are run

      agent.SetDestination(NPC.transform.position);   // set position to self in order to have NavMeshAgent stop moving into the player
      ///////////////////////////// Fore use without NavMesh   /////////////////////////////
      var direction = opponent.transform.position - NPC.transform.position;
      direction.y = 0;
      NPC.transform.rotation = Quaternion.Slerp(NPC.transform.rotation,
                                                Quaternion.LookRotation(direction),
                                                rotSpeed * Time.deltaTime);
      //NPC.transform.Translate(0, 0, Time.deltaTime * enemyMovementSpeed);
      /////////////////////////////                            /////////////////////////////

      if (Vector3.Distance(opponent.transform.position, NPC.transform.position) <= attackDistance)
      {
         animator.SetBool("InAttackRange", true);
         animator.SetBool("InChase", false);

         // Add the time since Update was last called to the timer.
         timer += Time.deltaTime;

         // If the timer exceeds the time between attacks, the player is in range and this enemy is alive: attack
         if (timer >= attackSpeed
            && enemyHealth > 0)
         {
            // Reset the timer.
            timer = 0f;

            // If the player has health to lose...
            if (healthBar.value > 0)
            {
               // ... damage the player.
               takeDamage(attackDamage);

               Debug.Log(NPC.name + " has " + healthBar.value);
               //playerHealth.TakeDamage(attackDamage);
            }
            else if (healthBar.value <= 0)
            {
               animator.SetBool("IsDead", true);
            }
         }
      }

      else if (Vector3.Distance(opponent.transform.position, NPC.transform.position) > attackDistance)
      {
         animator.SetBool("InAttackRange", false);
         if (Vector3.Distance(opponent.transform.position, NPC.transform.position) < chaseDistance)
         {
            animator.SetBool("InChase", true);
         }
      }
   }

   // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
   override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      animator.SetBool("InAttackRange", false);

      if (rangedUnit)
         NPC.GetComponent<EnemyAI>().stopFiring();
   }

   // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
   //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
   //
   //}

   // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
   //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
   //
   //}

   /*void AttackAction()
   {
      // Reset the timer.
      timer = 0f;
      Debug.Log("------Attacked");

      // If the player has health to lose...
      if (healthBar.value > 0)
      {
         // ... damage the player.
         takeDamage(attackDamage);
         //playerHealth.TakeDamage(attackDamage);
      }
   }*/
}
