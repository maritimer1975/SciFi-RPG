using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
	public class AreaOfEffectBehaviour : MonoBehaviour, ISpecialAbility
	{
		
		AreaOfEffectConfig config;

		// Use this for initialization
		void Start () {
			Debug.Log("Area of Effect behaviour attached to " + gameObject.name);
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void Use(AbilityUseParams useParams)
		{
			float damageToDeal = useParams.baseDamage + config.DamageToEachTarget();

			Vector3 effectOrigin = transform.position;

			RaycastHit[] raycastHits = Physics.SphereCastAll(effectOrigin, config.GetEffectRadius(), transform.forward, config.GetEffectRadius());

			foreach(RaycastHit raycastHit in raycastHits)
			{
				var damageable = raycastHit.collider.gameObject.GetComponent<IDamageable>();
				if(damageable != null)
				{
					damageable.TakeDamage(damageToDeal);
				}
			}
		}

		public void SetConfig(AreaOfEffectConfig configToSet)
		{
			this.config = configToSet;
		}
	}
}