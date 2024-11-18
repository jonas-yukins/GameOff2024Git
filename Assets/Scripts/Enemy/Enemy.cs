using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private GameObject player;

    private NavMeshAgent agent;
    public NavMeshAgent Agent { get => agent; }

    public GameObject Player { get => player; }

    // animations
    public Animator animator;
    public bool isMoving;
    
    
    public AIPath path;
    [Header("Sight Values")]
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    public float eyeHeight;

    [Header("weapon Values")]
    public Transform gunBarrel;
    [Range(0.1f, 10)]
    public float fireRate;

    [SerializeField] // for debugging purposes
    private string currentState;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialize();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        CanSeePlayer();
        currentState = stateMachine.activeState.ToString();

        // Update the Animator's isMoving parameter based on AI state or movement
        animator.SetBool("isMoving", isMoving);
    }

    public bool CanSeePlayer()
    {
        // calculates angle, and checks if player is within distance and angle
        if (player != null) {
            // is the player close enough to be seen
            if (Vector3.Distance(transform.position, player.transform.position) < sightDistance) {
                Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
                if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView) {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();
                    if (Physics.Raycast(ray, out hitInfo, sightDistance)) {
                        if (hitInfo.transform.gameObject == player) {
                            Debug.DrawRay(ray.origin, ray.direction * sightDistance);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
}
