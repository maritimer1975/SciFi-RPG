using UnityEngine;
using System.Collections;

// TODO: Consider rewiring
using RPG.Core;

namespace RPG.Characters
{
    [RequireComponent (typeof(WeaponSystem))]
    [RequireComponent (typeof(Character))]
    [RequireComponent (typeof(HealthSystem))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] float chaseRadius = 6f;
        [SerializeField] WaypointContainer patrolPath;
        [SerializeField] float patrolStoppingDistance = 1f;
        [SerializeField] float waypointDwellTime = 0.5f;

        WeaponConfig currentWeaponConfig;
        float currentWeaponRange;
        int nextWaypointIndex = 0;
        Transform nextWaypointTransform;

        //WeaponSystem weaponSystem;

        PlayerControl player;
        Character character;

        WeaponSystem weaponSystem;

        float distanceToPlayer;

        // TODO: give more states, like chase, idle, fleeing, etc.
        enum States { idle, attacking, patrolling, chasing }
//        States state = States.idle;

        // Pluggable Finite State Macine AI 
        public State currentState;
        public State remainState;

        private void Start()
        {
            InitializeComponents();
            
            if(patrolPath != null)
            {
                nextWaypointTransform = patrolPath.transform.GetChild(nextWaypointIndex);
            }
            
        }

        private void Update()
        {   
            currentWeaponConfig = weaponSystem.getCurrentWeapon;
            currentWeaponRange = currentWeaponConfig.GetMaxAttackRange();

            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            // initial state
            //Debug.Log(gameObject.name + " State: " + state);

            // switch(state)
            // {
            //     case States.attacking:
                    
            //         //Debug.Log(gameObject.name + " State: " + state);
                    
            //         if( distanceToPlayer > currentWeaponRange && distanceToPlayer > chaseRadius)
            //         {
            //             state = States.patrolling;
            //         }
            //         if(distanceToPlayer > currentWeaponRange && distanceToPlayer <= chaseRadius)
            //         {
            //             state = States.chasing;
            //         }

            //         if(state == States.attacking)
            //         {
            //             //Debug.Log("Attacking State");
            //             StopAllCoroutines();
            //             //weaponSystem.AttackTarget(player.gameObject);
            //         }

            //         break;

            //     case States.patrolling:
                    
            //         if(patrolPath == null)
            //         {
            //             state = States.idle;
            //         }
            //         if(distanceToPlayer <= currentWeaponRange)
            //         {
            //             state = States.attacking;
            //         }
            //         if(distanceToPlayer <= chaseRadius && distanceToPlayer > currentWeaponRange)
            //         {
            //             state = States.chasing;
            //         }

            //         if(state == States.patrolling)
            //         {
            //             //Debug.Log("Patrolling State");
            //             StopAllCoroutines();
            //             weaponSystem.StopAttacking();
            //             //StartCoroutine (Patrol() );
            //         }

                    
            //         break;

            //     case States.chasing:
            //         if(distanceToPlayer <= currentWeaponRange)
            //         {
            //             state = States.attacking;
            //         }
            //         if(distanceToPlayer > chaseRadius)
            //         {
            //             state = States.patrolling;
            //         }
                    
                    
            //         if(state == States.chasing)
            //         {
            //             //Debug.Log("Chasing State");
            //             StopAllCoroutines();
            //             weaponSystem.StopAttacking();
            //             //StartCoroutine( ChasePlayer() );
            //         }
                        
            //         break;
                
            //     case States.idle:
            //         if(distanceToPlayer <= currentWeaponRange)
            //         {
            //             state = States.attacking;
            //         }
            //         if(distanceToPlayer <= chaseRadius)
            //         {
            //             state = States.chasing;
            //         }
            //         if(patrolPath != null)
            //         {
            //             state = States.patrolling;
            //         }

            //         //Debug.Log("Idle State");
            //         break;


            //     default:
            //         Debug.Log("No State!!");
            //         break;

            // }


            // new pluggable state AI
            if(currentState != null)
            {
                currentState.UpdateState(this);
            }
        }

#region PROPERTIES SET/GET
        public Character Character { get { return character; } }
        public float DistanceToPlayer { get { return distanceToPlayer; } }
        public float CurrentWeaponRange { get { return currentWeaponRange;} }
        public float ChaseRadius { get { return chaseRadius; } }

#endregion

#region CUSTOM METHODS
        
        public void AttackPlayer()
        {
            weaponSystem.AttackTarget(player.gameObject);
        }


        // IEnumerator ChasePlayer()
        // {
        //     state = States.chasing;
        //     while( distanceToPlayer >= currentWeaponRange )
        //     {
        //         character.SetDestination(player.transform.position);
        //         yield return new WaitForEndOfFrame();
        //     }
        // }

        public void ChasePlayer()
        {
            character.SetDestination(player.transform.position);
        }

        public void CycleWaypointWhenClose()
        {
            var distanceToWaypoint = Vector3.Distance(patrolPath.transform.GetChild(nextWaypointIndex).position, transform.position);

            if(distanceToWaypoint <= patrolStoppingDistance)
            {
                nextWaypointIndex = (nextWaypointIndex + 1) % patrolPath.transform.childCount;    
            }
        }

        private void InitializeComponents()
        {
            player = FindObjectOfType<PlayerControl>();
            character = GetComponent<Character>();
            weaponSystem = GetComponent<WeaponSystem>();
        }

        // IEnumerator Patrol()
        // {
        //     state = States.patrolling;
        //     while(true)
        //     {
        //         Vector3 nextWaypointPos = patrolPath.transform.GetChild(nextWaypointIndex).position;
        //         character.SetDestination(nextWaypointPos);
        //         CycleWaypointWhenClose();
        //         yield return new WaitForSeconds(waypointDwellTime);
        //     }
        // }

        public Vector3 GetNextWaypointPosition()
        {
            Vector3 nextWaypointPos = patrolPath.transform.GetChild(nextWaypointIndex).position;
            return nextWaypointPos;
        }

        public void TransitionToState(State nextState)
        {
            if(nextState != remainState)
            {
                currentState = nextState;
            }
        }

        void OnDrawGizmos()
        {
            // Draw chase sphere
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position,chaseRadius);

            // Draw attack sphere, will not show up in scene mode as the weapon config is not loaded till runtime
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            // draw state machine gizmos
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
#endregion
    }
}