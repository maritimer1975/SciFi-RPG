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
		
		[SerializeField] AudioClip[] abilitySounds;

		protected AbilityBehaviour behaviour;

		public abstract AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo);

		public void AttachAbilityTo(GameObject objectToAttachTo)
		{
			AbilityBehaviour behaviourComponent = GetBehaviourComponent(objectToAttachTo);
			behaviourComponent.SetConfig(this);
			behaviour = behaviourComponent;
		}

		protected void SetBehaviour(AbilityBehaviour behaviourComponent)
        {
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

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

		public AudioClip GetRandomSoundClip()
		{
			return abilitySounds[Random.Range(0, abilitySounds.Length)];
		}
	}
}
