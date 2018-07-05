using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
	[CreateAssetMenu(menuName="RPG/Weapon")]
	public class WeaponConfig : ScriptableObject {

		public Transform gripTransform;
		[SerializeField] GameObject weaponPrefab;
		[SerializeField] AnimationClip attackAnimation;
        [SerializeField] float minTimeBetweenHits = 0.5f;
        [SerializeField] float maxAttackRange = 2f;
		[SerializeField] float additionalDamage = 10f;

		[SerializeField] float damageDelay = 0.5f;

#region PROPERTIES SET/GET
		public float DamageDelay { get { return damageDelay; } set { damageDelay  = value; } }


#endregion

		
		public float GetMinTimeBetweenHits()
		{
			// TODO: Consinder if animation time should be taken into account
			return minTimeBetweenHits;
		}

		public float GetAdditionalDamage()
		{
			return additionalDamage;
		}

		public float GetMaxAttackRange()
		{
			return maxAttackRange;
		}

		public GameObject GetWeaponPrefab()
		{
			return weaponPrefab;
		}

		public AnimationClip GetAttackAnimClip()
        {
            RemoveAnimationEvents();

            return attackAnimation;
        }

        private void RemoveAnimationEvents()
        {
            // remove all animation events
			attackAnimation.events = new AnimationEvent[0];
        }
    }
}