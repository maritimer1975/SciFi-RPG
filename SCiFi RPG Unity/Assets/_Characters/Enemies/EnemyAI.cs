using UnityEngine;
using System.Collections;

// TODO: Consider rewiring
using RPG.Core;

namespace RPG.Characters
{
    [RequireComponent (typeof(WeaponSystem))]
    [RequireComponent (typeof(Character))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] float chaseRadius = 6f;
        [SerializeField] WaypointContainer patrolPath;
        [SerializeField] float patrolStoppingDistance = 1f;

        WeaponConfig currentWeaponConfig;
        float currentWeaponRange;
        int nextWaypointIndex = 0;
        Transform nextWaypointTransform;

        WeaponSystem weaponSystem;

        PlayerControl player;
        Character character;

        float distanceToPlayer;

        // TODO: give more states, like chase, idle, fleeing, etc.
        enum States { idle, attacking, patrolling, chasing }
        States state = States.idle;

        private void Start()
        {
            //aiCharacterControl = GetComponent<AICharacterControl>();
            player = FindObjectOfType<PlayerControl>();
            character = GetComponent<Character>();
            nextWaypointTransform = patrolPath.transform.GetChild(nextWaypointIndex);
        }

        private void Update()
        {   
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponConfig = weaponSystem.getCurrentWeapon;
            currentWeaponRange = currentWeaponConfig.GetMaxAttackRange();

            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if(distanceToPlayer > chaseRadius && state != States.patrolling)
            {
                // stop doing what we are doing
                // start patrolling
                StopAllCoroutines();
                StartCoroutine(Patrol());
            }
            if(distanceToPlayer <= chaseRadius && state != States.chasing)
            {
                StopAllCoroutines();
                StartCoroutine(ChasePlayer());
            }
            if(distanceToPlayer <= currentWeaponRange && state != States.attacking)
            {
                // stop what we are doing
                // attack the player
                StopAllCoroutines();
                state = States.attacking;
            }
        }


        IEnumerator ChasePlayer()
        {
            state = States.chasing;
            while( distanceToPlayer >= currentWeaponRange )
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
        }

        void CycleWaypointWhenClose()
        {
            var distanceToWaypoint = Vector3.Distance(patrolPath.transform.GetChild(nextWaypointIndex).position, transform.position);
            
            Debug.Log("Distance To waypoint [" + nextWaypointIndex + "] = " + distanceToWaypoint);

            if(distanceToWaypoint <= patrolStoppingDistance)
            {
                nextWaypointIndex = (nextWaypointIndex + 1) % patrolPath.transform.childCount;    
            }
        }

        IEnumerator Patrol()
        {
            state = States.patrolling;
            while(true)
            {
                Vector3 nextWaypointPos = patrolPath.transform.GetChild(nextWaypointIndex).position;
                character.SetDestination(nextWaypointPos);
                CycleWaypointWhenClose();
                yield return new WaitForSeconds(0.5f); // TODO: parameterize
            }
        }

        void OnDrawGizmos()
        {
            // Draw chase sphere
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position,chaseRadius);

            // Draw attack sphere
            Gizmos.color=new Color(255f,0f, 0f,0.5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);
        }
    }
}