﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapons
{
	[CreateAssetMenu(menuName="RPG/Weapon")]
	public class Weapon : ScriptableObject {

		public Transform gripTransform;
		[SerializeField] GameObject weaponPrefab;
		[SerializeField] AnimationClip attackAnimation;
        [SerializeField] float minTimeBetweenHits = 0.5f;
        [SerializeField] float maxAttackRange = 2f;
		
		public float GetMinTimeBetweenHits()
		{
			// TODO: Consinder if animation time should be taken into account
			return minTimeBetweenHits;
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