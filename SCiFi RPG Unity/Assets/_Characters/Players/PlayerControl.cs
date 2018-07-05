using UnityEngine;
using System.Collections;

// TODO: Consider rewiring
using RPG.CameraUI;

//TODO: extract weapon systems
namespace RPG.Characters
{
    public class PlayerControl : MonoBehaviour
    {

#region SERIALIZED VARIABLES        

        [SerializeField] ParticleSystem criticalHitParticle;


        [Header ("Critical Hit Stats")]
        [Range (0.1f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;

        [SerializeField] float criticalHitMultiplier = 1.25f;
#endregion

#region VARIABLES

        SpecialAbilities specialAbilities;

        WeaponSystem weaponSystem;

        EnemyAI currentEnemy = null;

        HealthSystem healthSystem;
        Character character;

#endregion        

#region UNITY METHODS
        void Awake()
        {
            
        }

        void Start()
        {
            InitializeComponents();
            RegisterObservers();
        }

        void Update()
        {
            if(IsPlayerAlive())
            {
                // Scan for key press
                ScanForAbilityKeyDown();
            }
        }

#endregion

#region PROPERTIES SET/GET
        public GameObject getCurrentTarget { get { return currentEnemy.gameObject; } }

#endregion

#region CUSTOM METHODS

        bool IsPlayerAlive()
        {
            return healthSystem.IsCharacterAlive();
        }

        void InitializeComponents()
        {
            specialAbilities = GetComponent<SpecialAbilities>();
            healthSystem = GetComponent<HealthSystem>();
            character = GetComponent<Character>();
            weaponSystem = GetComponent<WeaponSystem>();
        }

        bool IsTargetInRange(Vector3 targetPosition)
        {
            float distanceToTarget = (targetPosition - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.getCurrentWeapon.GetMaxAttackRange();
        }

        IEnumerator MoveToTarget(EnemyAI target)
        {
            character.SetDestination(target.transform.position);

            while(!IsTargetInRange(target.transform.position))
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }

        IEnumerator MoveAndAbility(EnemyAI target)
        {
            yield return StartCoroutine( MoveToTarget(target) );
            specialAbilities.AttemptSpecialAbility(0, target.gameObject);
        }

        IEnumerator MoveAndAttack(EnemyAI target)
        {
            yield return StartCoroutine( MoveToTarget(target) );
            weaponSystem.AttackTarget(target.gameObject);
        }

        void OnMouseOverEnemy(EnemyAI enemy)
        {
            currentEnemy = enemy;
            if( Input.GetMouseButton(0)  && IsTargetInRange(enemy.transform.position) )
            {
                    weaponSystem.AttackTarget(enemy.gameObject);
            }
            else if( Input.GetMouseButton(0)  && !IsTargetInRange(enemy.transform.position) )
            {
                // move and attack
                StartCoroutine( MoveAndAttack(enemy) );    
            }
            else if( Input.GetMouseButtonDown(1) && IsTargetInRange(enemy.transform.position) )
            {
                specialAbilities.AttemptSpecialAbility(0, enemy.gameObject); // targeting the enemy we click on
            }
            else if( Input.GetMouseButtonDown(1) && !IsTargetInRange(enemy.transform.position) )
            {
                // move and ability
                StartCoroutine( MoveAndAbility(enemy) );
            }
        }

        void OnMouseOverTerrain(Vector3 worldPos)
        {
            if(Input.GetMouseButton(0))
            {
                character.SetDestination(worldPos);
            }
        }

        private void RegisterObservers()
        {
            var cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.notifyMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.notifyMouseOverTerrain += OnMouseOverTerrain;
        }

        private void ScanForAbilityKeyDown()
        {
            for (int keyIndex = 1; keyIndex < specialAbilities.GetNumberOfAbilities(); keyIndex++)
            {
                if(Input.GetKeyDown(keyIndex.ToString()))
                {
                    specialAbilities.AttemptSpecialAbility(keyIndex);
                }    
            }
            
            // if(Input.GetKeyDown(KeyCode.Alpha1))
            // {
            //     specialAbilities.AttemptSpecialAbility(1);
            // }

            // if(Input.GetKeyDown(KeyCode.Alpha2))
            // {
            //     specialAbilities.AttemptSpecialAbility(2);
            // }

            // if(Input.GetKeyDown(KeyCode.Alpha3))
            // {
            //     specialAbilities.AttemptSpecialAbility(3);
            // }
        }
#endregion
    }
}