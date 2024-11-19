using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class AttackState : BaseState
{
    // move enemy slightly to make them harder to hit
    private float moveTimer; 
    // how long enemy stays in attack before searching for player
    private float losePlayerTimer; 
    private float shotTimer;
    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer()) { // player can be seen
            // lock the lose player timer and increment the move and shot timers
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;
            // Calculate the direction to the player
            Vector3 directionToPlayer = enemy.Player.transform.position - enemy.transform.position;
            directionToPlayer.y = 0; // Ignore the Y-axis so the enemy doesn't tilt up/down

            // Calculate the rotation needed to face the player on the Y-axis
            Quaternion rotation = Quaternion.LookRotation(directionToPlayer);

            // Apply the rotation to the enemy, maintaining its current X and Z rotation
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, rotation, Time.deltaTime * 10f); // Adjust speed as needed


            // if shot timer > fireRate
            if (shotTimer > enemy.fireRate) {
                Shoot();
            }

            // move the enemy to a random position after a random time
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

    public void Shoot()
    {
    // store reference to gun barrel
    Transform gunbarrel = enemy.gunBarrel;

    // instantiate new bullet
    GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, gunbarrel.position, enemy.transform.rotation);

    // calculate the direction to the player
    Vector3 shootDirection = (enemy.Player.transform.position - gunbarrel.transform.position).normalized;

    // add small random variation for bullet spread (adjust the range as needed)
    Vector3 randomSpread = Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) * shootDirection;

    // Adjust the bullet's rotation to match the shoot direction
    bullet.transform.rotation = Quaternion.FromToRotation(bullet.transform.up, shootDirection);

    // set bullet velocity (using Rigidbody velocity)
    Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
    bulletRigidbody.velocity = randomSpread * 40f;  // Set the velocity of the bullet

    // Debug message
    Debug.Log("Shoot");

    // Reset the shot timer or cooldown (if applicable)
    shotTimer = 0;
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
