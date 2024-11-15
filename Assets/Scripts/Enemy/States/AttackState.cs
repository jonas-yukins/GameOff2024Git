using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    // move enemy slightly to make them harder to hit
    private float moveTimer; 
    // how long enemy stays in attack before searching for player
    private float losePlayerTimer; 
    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer()) {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            if (moveTimer > Random.Range(3, 7)) {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            } 
        } else { // can't see player
                losePlayerTimer += Time.deltaTime;
                if (losePlayerTimer > 8) {
                    // change to search state
                    stateMachine.ChangeState(new PatrolState());
                }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
