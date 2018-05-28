using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

// TODO: Consider rewiring
using RPG.CameraUI;
using RPG.Core;
using RPG.Weapons;
using System;

namespace RPG.Characters
{
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] float maxHealthPoints;
        [SerializeField] float baseDamage = 10f;

        [SerializeField] Weapon weaponInUse;

        [SerializeField] AnimatorOverrideController animatorOverrideController;

        [SerializeField] ParticleSystem criticalHitParticle;

        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;

        // temporarily serializing for debugging
        [SerializeField] AbilityConfig[] abilities;


        [Header ("Critical Hit Stats")]
        [Range (0.1f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;

        [SerializeField] float criticalHitMultiplier = 1.25f;

        Animator animator;
        float currentHealthPoints;
        float lastTimeHit = 0f;

        const string DEATH_TRIGGER = "Death";
        const string ATTACK_TRIGGER = "Attack";

        CameraRaycaster cameraRaycaster;

        Energy energyComponent = null;

        AudioSource audioSource = null;

        Enemy currentEnemy = null;

        

#region UNITY METHODS
        void Awake()
        {
            InitializeComponents();
        }

        void Start()
        {
            RegisterObservers();
            SetCurrentMaxHealth();
            PutWeaponInHand();
            SetupRuntimeAnimator();
            AttachAbilities();
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


#region CUSTOM METHODS

        private void AttachAbilities()
        {
            for( int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex ++)
            {
                abilities[abilityIndex].AttachComponentTo(gameObject);
            }
        }

        private void AttackTarget()
        {
            if ( Time.time - lastTimeHit > weaponInUse.GetMinTimeBetweenHits() )
            {
                animator.SetTrigger(ATTACK_TRIGGER);
                currentEnemy.TakeDamage(CalculateDamage());
                lastTimeHit = Time.time;
            }
        }

        private float CalculateDamage()
        {
            float totalDamage = baseDamage + weaponInUse.GetAdditionalDamage();

            bool isCriticalHit = UnityEngine.Random.Range(0f,1f) <= criticalHitChance;

            if(isCriticalHit)
            {
                criticalHitParticle.Play();
                totalDamage *= criticalHitMultiplier;
            }

            Debug.Log ("Damage = " + totalDamage);

            return totalDamage;

        }

         private void AttemptSpecialAbility(int abilityIndex)
        {
            var energyCost = abilities[abilityIndex].GetEnergyCost();
            
            if(energyComponent.IsEnergyAvailable(energyCost))
            {
                energyComponent.ReduceEnergy(energyCost);
                
                var abilityUseParams = new AbilityUseParams(currentEnemy, baseDamage);
                abilities[abilityIndex].Use(abilityUseParams);
            }
        }

        public AudioSource GetAudioSource()
        {
            return audioSource;
        }


        public float healthAsPercentage
        {
            get { return currentHealthPoints / (float)maxHealthPoints; }
        }

        public void IncreaseHealth(float health)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + health, 0f, maxHealthPoints);

        }

        void InitializeComponents()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            energyComponent = GetComponent<Energy>();
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        bool IsTargetInRange(Vector3 targetPosition)
        {
            float distanceToTarget = (targetPosition - transform.position).magnitude;
            return distanceToTarget <= weaponInUse.GetMaxAttackRange();
        }

        bool IsPlayerAlive()
        {
            return healthAsPercentage > Mathf.Epsilon;
        }

        IEnumerator KillPlayer()
        {
            animator.SetTrigger(DEATH_TRIGGER);
            
            PlayDeathSound();
            
            // wait reload the scene or death screen SceneManager.something
            yield return new WaitForSecondsRealtime(audioSource.clip.length);
            SceneManager.LoadScene(0);   
        }

        private void MoveToTarget(GameObject target)
        {
            Debug.Log("Move Too"); // TODO: call player movement to move to target
        }

        void OnMouseOverEnemy(Enemy enemy)
        {
            currentEnemy = enemy;
            if(Input.GetMouseButton(0)  && IsTargetInRange(enemy.transform.position))
            {
                    AttackTarget();
            }
            else if(Input.GetMouseButtonDown(1))
            {
                AttemptSpecialAbility(0);
            }
        }

         private void PlayDamageSound()
        {
            int index = UnityEngine.Random.Range(0, damageSounds.Length);
            audioSource.clip = damageSounds[index];
            audioSource.Play();
        }

        private void PlayDeathSound()
        {
            int index = UnityEngine.Random.Range(0, deathSounds.Length);
            audioSource.clip = deathSounds[index];
            audioSource.Play();
        }

        private void PutWeaponInHand()
        {
            var weaponPrefab = weaponInUse.GetWeaponPrefab();
            GameObject weaponSocket = RequestDominantHand();
            var weapon = Instantiate(weaponPrefab, weaponSocket.transform); 
            weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
        }

        private void RegisterObservers()
        {
            cameraRaycaster.notifyMouseOverEnemy += OnMouseOverEnemy;
        }

        private void ReduceHealth(float health)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - health, 0f, maxHealthPoints);
            // play hit sound and animation
        }

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.AreNotEqual(numberOfDominantHands, 0, "No DominantHand found on player. Please add one.");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on player. Please remove one.");
            return dominantHands[0].gameObject;
        }

        private void ScanForAbilityKeyDown()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                AttemptSpecialAbility(1);
            }

            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                AttemptSpecialAbility(2);
            }

            if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                AttemptSpecialAbility(3);
            }

        }

        private void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }

        private void SetupRuntimeAnimator()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimClip();  //TODO: remove string
            
            //throw new NotImplementedException();
        }

        public void TakeDamage(float damage)
        {
            bool playerDies = (currentHealthPoints - damage <= 0);  // must happen before we reduce the health
            
            ReduceHealth(damage);

            PlayDamageSound();

            if (playerDies)
            {
                StartCoroutine(KillPlayer());
            }
        }

       
        #endregion
    }
}