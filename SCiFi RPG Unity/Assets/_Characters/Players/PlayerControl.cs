using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

// TODO: Consider rewiring
using RPG.CameraUI;
using RPG.Core;
using System;

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

        CameraRaycaster cameraRaycaster;

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
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
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

        private void MoveToTarget(GameObject target)
        {
            Debug.Log("Move Too"); // TODO: call player movement to move to target
        }

        void OnMouseOverEnemy(EnemyAI enemy)
        {
            currentEnemy = enemy;
            if(Input.GetMouseButton(0)  && IsTargetInRange(enemy.transform.position))
            {
                    weaponSystem.AttackTarget(enemy.gameObject);
            }
            else if(Input.GetMouseButtonDown(1))
            {
                specialAbilities.AttemptSpecialAbility(0);
            }
        }

        void OnMouseOverTerrain(Vector3 worldPos)
        {
            if(Input.GetMouseButtonDown(0))
            {
                character.SetDestination(worldPos);
            }
        }

        private void RegisterObservers()
        {
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