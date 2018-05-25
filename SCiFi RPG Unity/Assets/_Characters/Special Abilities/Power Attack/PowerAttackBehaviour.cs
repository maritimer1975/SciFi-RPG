using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
	public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbility
	{
		
		PowerAttackConfig config;
		
		// Use this for initialization
		void Start () {
			Debug.Log("Power Attack behaviour attached to " + gameObject.name);
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void Use(AbilityUseParams useParams)
		{
			float damageToDeal = useParams.baseDamage + config.GetExtraDamage();
			useParams.target.TakeDamage(damageToDeal);
		}

		public void SetConfig(PowerAttackConfig configToSet)
		{
			this.config = configToSet;
		}
	}
}