using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
	public class SelfHealBehaviour : AbilityBehaviour
	{	
		HealthSystem playerHealth;

		// Use this for initialization
		void Start () {
			

			playerHealth = gameObject.GetComponent<HealthSystem>();
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public override void Use(GameObject target)
        {
            // TODO: find a way to move audio and particle playing to a parent class
			
			ApplyHealing();
            PlayParticleEffect();
			PlayAbilityAnimation();
        }

        private void ApplyHealing()
        {
			playerHealth.Heal( (config as SelfHealConfig).GetHealAmount());  // TODO: consider making it a percent of maximum health heal
			AudioSource audioSource = playerHealth.GetAudioSource();
			audioSource.clip = config.GetRandomSoundClip();
			audioSource.Play();
        }
	}
}