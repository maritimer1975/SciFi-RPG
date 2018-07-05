using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
	public class AreaOfEffectBehaviour : AbilityBehaviour
	{
		
		//AreaOfEffectConfig config;
	
#region UNITY METHODS    
		void Start () {
			//player = gameObject.GetComponent<Player>();
		}
		
		// Update is called once per frame
		void Update () {
			
		}
#endregion

#region CUSTOM METHODS
		public override void Use(GameObject target)
        {
            DealRadialDamage();

			PlayParticleEffect();

			PlaySoundEffect();

            PlayAbilityAnimation();

        }
		
        private void DealRadialDamage()
        {
            float damageToDeal = (config as AreaOfEffectConfig).DamageToEachTarget();

            Vector3 effectOrigin = transform.position;

            RaycastHit[] raycastHits = Physics.SphereCastAll(effectOrigin, (config as AreaOfEffectConfig).GetEffectRadius(), transform.forward, (config as AreaOfEffectConfig).GetEffectRadius());

            foreach (RaycastHit raycastHit in raycastHits)
            {
                var healthSystem = raycastHit.collider.gameObject.GetComponent<HealthSystem>();
                bool hitPlayer = raycastHit.collider.gameObject.GetComponent<PlayerControl>();
				if (healthSystem != null && !hitPlayer)
                {
                    healthSystem.TakeDamage(damageToDeal);
                }
            }
        }
#endregion

	}
}