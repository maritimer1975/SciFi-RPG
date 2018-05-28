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
		
		Player player;

		// Use this for initialization
		void Start () {
			player = gameObject.GetComponent<Player>();
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public override void Use(AbilityUseParams useParams)
        {
            DealRadialDamage(useParams);

			PlayParticleEffect();

			PlaySoundEffect();

        }

		private void PlaySoundEffect()
		{
			AudioSource audioSource = player.GetAudioSource();
			audioSource.clip = config.GetSoundClip();
			audioSource.Play();
		}

        private void DealRadialDamage(AbilityUseParams useParams)
        {
            float damageToDeal = useParams.baseDamage + (config as AreaOfEffectConfig).DamageToEachTarget();

            Vector3 effectOrigin = transform.position;

            RaycastHit[] raycastHits = Physics.SphereCastAll(effectOrigin, (config as AreaOfEffectConfig).GetEffectRadius(), transform.forward, (config as AreaOfEffectConfig).GetEffectRadius());

            foreach (RaycastHit raycastHit in raycastHits)
            {
                var damageable = raycastHit.collider.gameObject.GetComponent<IDamageable>();
                bool hitPlayer = raycastHit.collider.gameObject.GetComponent<Player>();
				if (damageable != null && !hitPlayer)
                {
                    damageable.TakeDamage(damageToDeal);
                }
            }
        }
	}
}