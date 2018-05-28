    using UnityEngine;

// TODO: Consider rewiring
using RPG.Core;
using RPG.Weapons;

namespace RPG.Characters
{
    [RequireComponent (typeof(AICharacterControl))]
    public class Enemy : MonoBehaviour, IDamageable
    {

        [SerializeField] float maxHealthPoints;
        float currentHealthPoints;
        
        AICharacterControl aiCharacterControl = null;
        Player player = null;

        [SerializeField] float attackRadius = 4f;
        [SerializeField] float damagePerShot = 9f;
        [SerializeField] public float firingPeriodInS = 1f;

        [SerializeField] float firingPeriodVariation = 0.1f;

        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;
        [SerializeField] Vector3 aimOffset = new Vector3(0f, 1f, 0f);

        [SerializeField] float chaseRadius = 6f;

        bool isAttacking = false;

        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0, maxHealthPoints);

            if(currentHealthPoints <= 0)
                Destroy(gameObject);

        }

        private void Start()
        {
            aiCharacterControl = GetComponent<AICharacterControl>();
            player = FindObjectOfType<Player>();
            currentHealthPoints = maxHealthPoints;
        }

        private void Update()
        {
            // stops enemies behaviour
            if(player.healthAsPercentage <= Mathf.Epsilon)
            {
                StopAllCoroutines();
                Destroy(this);  // removes the enemy component
            }
            
            Vector3 enemyPosition = transform.position;
            Vector3 playerPosition = player.transform.position;

            float diff = Vector3.Distance(enemyPosition, playerPosition);
            if(diff <= attackRadius && !isAttacking)
            {
                isAttacking = true;

                float randomizedDelay = firingPeriodInS + Random.Range(-firingPeriodVariation, firingPeriodVariation);
                InvokeRepeating("SpawnProjectile", 0f, randomizedDelay);
                transform.LookAt(player.transform);
            }

            if(diff <= attackRadius)
            {
                transform.LookAt(player.transform);
            }

            if(diff > attackRadius)
            {
                isAttacking = false;
                CancelInvoke();
            }
                
        
            if(diff <= chaseRadius)
                aiCharacterControl.SetTarget(player.transform);
            else 
                aiCharacterControl.SetTarget(transform);

        }

        // TODO: separate out Character firing logic
        void SpawnProjectile()
        {
            GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.DamageCaused = damagePerShot;
            projectileComponent.SetShooter(gameObject);

            Vector3 enemyToPlayer = Vector3.Normalize(player.transform.position + aimOffset - projectileSocket.transform.position);
            float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed(); 
            newProjectile.GetComponent<Rigidbody>().velocity = enemyToPlayer * projectileSpeed;

        }

        public float healthAsPercentage
        {
            get { return currentHealthPoints / (float)maxHealthPoints; }

        }

        void OnDrawGizmos()
        {
            // Draw chase gizmos
            Gizmos.color = Color.blue;
            //Gizmos.DrawLine(transform.position, clickPoint);
            //Gizmos.DrawSphere(currentDestination,0.15f);
            Gizmos.DrawWireSphere(transform.position,chaseRadius);

            // Draw attack sphere
            Gizmos.color=new Color(255f,0f, 0f,0.5f);
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
    }
}