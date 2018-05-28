using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
	public struct AbilityUseParams
	{
		public IDamageable target;
		public float baseDamage;

		public AbilityUseParams(IDamageable target, float baseDamage)
		{
			this.target = target;
			this.baseDamage = baseDamage;
		}
	}
	
	
	public abstract class AbilityConfig : ScriptableObject 
	{
		[Header("Ability General")]
		[SerializeField] float energyCost = 10f;
		[SerializeField] GameObject particlePrefab = null;
		
		[SerializeField] AudioClip abilitySound;

		protected AbilityBehaviour behaviour;

		abstract public void AttachComponentTo(GameObject gameObjectToAttachTo);

		public float GetEnergyCost()
		{
			return energyCost;
		}

		public GameObject GetParticle()
		{	
			return particlePrefab;
		}

		public void Use(AbilityUseParams useParams)
		{
			behaviour.Use(useParams);
		}

		public AudioClip GetSoundClip()
		{
			return abilitySound;
		}
	}
}
