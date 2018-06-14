using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Characters
{
	
	public class WeaponSystem : MonoBehaviour {

#region SERIALIZED VARIABLES
		[SerializeField] float baseDamage = 10f;

        [SerializeField] WeaponConfig currentWeaponConfig;
#endregion

#region VARIABLES
		GameObject weaponObject;

		GameObject target;

		Animator anim;

		Character character;

		float lastTimeHit = 0f;
		
#endregion

#region CONSTANTS
		const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

#endregion

#region UNITY METHODS
		void Start ()
		{
			InitializeComponents();
			
			PutWeaponInHand(currentWeaponConfig);
			SetAttackAnimation();
		}
		
		void Update ()
		{
			
		}
#endregion

#region PROPERTIES SET/GET

		public WeaponConfig getCurrentWeapon { get { return currentWeaponConfig; } }

#endregion

#region CUSTOM METHODS

		public void AttackTarget(GameObject targetToAttack)
		{
			target = targetToAttack;
			Debug.Log("Attacking target: " + targetToAttack);
			// TODO: use a repeat attack co-routine

		}

		// TODO: use co-routing to move and attack
        private void AttackTarget()
        {
            if ( Time.time - lastTimeHit > currentWeaponConfig.GetMinTimeBetweenHits() )
            {
                SetAttackAnimation();
                anim.SetTrigger(ATTACK_TRIGGER);
                var enemyHealth = target.GetComponent<HealthSystem>();
                enemyHealth.TakeDamage(CalculateDamage());
                lastTimeHit = Time.time;
            }
        }

        // TODO: should this be in the weapon system
         private float CalculateDamage()
        {
            float totalDamage = baseDamage + currentWeaponConfig.GetAdditionalDamage();

            return totalDamage;
        }

		private void InitializeComponents()
		{
			anim = GetComponent<Animator>();
			character = GetComponent<Character>();
		}

		public void PutWeaponInHand(WeaponConfig weaponToUse)
        {
            //TODO: refactor weapon and equipment. Make a manager component.
            
            GameObject weaponSocket = RequestDominantHand();

            currentWeaponConfig = weaponToUse;

            // check if a weapon is already in hand
            RemoveEquippedWeapon(weaponObject);

            var weaponPrefab = currentWeaponConfig.GetWeaponPrefab();
            
            weaponObject = Instantiate(weaponPrefab, weaponSocket.transform); 
            weaponObject.transform.localPosition = currentWeaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = currentWeaponConfig.gripTransform.localRotation;
        }

		private void RemoveEquippedWeapon(GameObject weaponObject)
        {
            Destroy(weaponObject);
        }

		private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.AreNotEqual(numberOfDominantHands, 0, "No DominantHand found on player. Please add one.");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on player. Please remove one.");
            return dominantHands[0].gameObject;
        }

		private void SetAttackAnimation()
        {
            var animOverrideController = character.getAnimatorOverrideController;
			anim.runtimeAnimatorController = animOverrideController;
            animOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimClip();
        }

		public void SetWeaponInUse(WeaponConfig weaponConfig)
        {
            currentWeaponConfig = weaponConfig;
        }
#endregion
	}
}