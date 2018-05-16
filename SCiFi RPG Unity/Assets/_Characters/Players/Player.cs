using UnityEngine;
using UnityEngine.Assertions;

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
        [SerializeField] int enemyLayer = 9;
        [SerializeField] float damagePerHit = 10f;


        [SerializeField] Weapon weaponInUse;

        [SerializeField] AnimatorOverrideController animatorOverrideController;

        Animator animator;
        float currentHealthPoints;
        float lastTimeHit = 0f;

        CameraRaycaster cameraRaycaster;

        void Start()
        {
            RegisterForMouseClicks();
            SetCurrentMaxHealth();
            PutWeaponInHand();
            SetupRuntimeAnimator();
        }

        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);

            //if(currentHealthPoints <=0)
            //    Destroy(gameObject);
        }

        private void SetupRuntimeAnimator()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimClip();  //TODO: remove string
            
            //throw new NotImplementedException();
        }

        private void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }

        private void PutWeaponInHand()
        {
            var weaponPrefab = weaponInUse.GetWeaponPrefab();
            GameObject weaponSocket = RequestDominantHand();
            var weapon = Instantiate(weaponPrefab, weaponSocket.transform); 
            weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
        }

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.AreNotEqual(numberOfDominantHands, 0, "No DominantHand found on player. Please add one.");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on player. Please remove one.");
            return dominantHands[0].gameObject;
        }

        private void RegisterForMouseClicks()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
        }

        void OnMouseClick(RaycastHit raycastHit, int layerHit)
        {
            if(layerHit == enemyLayer)
            {
                var enemy = raycastHit.collider.gameObject;
                if(IsTargetInRange(enemy))
                {
                    AttackTarget(enemy);
                }
            }
        }

        bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponInUse.GetMaxAttackRange();
        }


        private void AttackTarget(GameObject target)
        {
            var enemyComponent = target.GetComponent<Enemy>();
            if ( Time.time - lastTimeHit > weaponInUse.GetMinTimeBetweenHits() )
            {
                animator.SetTrigger("Attack");
                enemyComponent.TakeDamage(damagePerHit);
                lastTimeHit = Time.time;
            }
        }

        public float healthAsPercentage
        {
            get { return currentHealthPoints / (float)maxHealthPoints; }
        }

        
    }
}