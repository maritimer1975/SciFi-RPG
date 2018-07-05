using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{	
	public abstract class AbilityConfig : ScriptableObject 
	{
#region SERIALIZED VARIABLES		
		[Header("Ability General")]
		[SerializeField] float energyCost = 10f;
		[SerializeField] GameObject particlePrefab = null;
		
		[SerializeField] AudioClip[] abilitySounds;
		[SerializeField] AnimationClip animClip;
#endregion

#region VARIABLES
		protected AbilityBehaviour behaviour;

		public abstract AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo);
#endregion

#region PROPERTIES GET/SET
		public AnimationClip AnimClip { get { return animClip; }}
#endregion

#region CUSTOM METHODS
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

		public void Use(GameObject target)
		{
			behaviour.Use(target);
		}

		public AudioClip GetRandomSoundClip()
		{
			return abilitySounds[Random.Range(0, abilitySounds.Length)];
		}
#endregion
	}
}
