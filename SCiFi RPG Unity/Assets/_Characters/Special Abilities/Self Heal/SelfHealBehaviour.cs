using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
	public class SelfHealBehaviour : AbilityBehaviour
	{	
		Player player = null;

		// Use this for initialization
		void Start () {
			

			player = gameObject.GetComponent<Player>();
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public override void Use(AbilityUseParams useParams)
        {
            // TODO: find a way to move audio and particle playing to a parent class
			
			ApplyHealing(useParams);

            PlayParticleEffect();

        }

        private void ApplyHealing(AbilityUseParams useParams)
        {
			player.IncreaseHealth( (config as SelfHealConfig).GetHealAmount());  // TODO: consider making it a percent of maximum health heal
			AudioSource audioSource = player.GetAudioSource();
			audioSource.clip = config.GetRandomSoundClip();
			audioSource.Play();
        }
	}
}