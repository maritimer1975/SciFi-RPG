using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
	public class PowerAttackBehaviour : AbilityBehaviour
	{
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
            DealDamage(useParams);

            PlayParticleEffect();

			PlaySoundEffect();
        }

        private void DealDamage(AbilityUseParams useParams)
        {
            float damageToDeal = useParams.baseDamage + (config as PowerAttackConfig).GetExtraDamage();
            useParams.target.TakeDamage(damageToDeal);


        }
	}
}