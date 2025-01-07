using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIdleState : StateMachineBehaviour
{
    Transform player;
    
    float timer;
    public float idleTime = 0f;
    public float detectionAreaRadius = 18f;

    // Called on enter to state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
       timer = 0;
       player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Called every frame
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        

        // -- Transition to Patrol State -- //

        timer += Time.deltaTime;
        if (timer > idleTime)
        {
            animator.SetBool("isPatrolling", true);
        }

        // -- Transition to Chase State -- //

        float distanceFromPlayer =  Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionAreaRadius)
        {
            animator.SetBool("isChasing", true);
        }
       
    }
}
