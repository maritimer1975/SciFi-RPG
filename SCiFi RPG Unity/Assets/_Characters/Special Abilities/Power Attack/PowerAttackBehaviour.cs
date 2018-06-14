using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
	public class PowerAttackBehaviour : AbilityBehaviour
	{
		//Player player;
		
		// Use this for initialization
		void Start () {
			//player = gameObject.GetComponent<Player>();
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public override void Use(GameObject target)
        {
            DealDamage(target);

            PlayParticleEffect();

			PlaySoundEffect();
        }

        private void DealDamage(GameObject target)
        {
            float damageToDeal = (config as PowerAttackConfig).GetExtraDamage();
            // TODO: Fix power useParams. Not idamagable, maybe HealthSystem?
			var targetHealth = target.GetComponent<HealthSystem>();
			targetHealth.TakeDamage(damageToDeal);
        }
	}
}