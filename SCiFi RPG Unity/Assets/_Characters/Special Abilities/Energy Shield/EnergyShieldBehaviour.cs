using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
	public class EnergyShieldBehaviour : AbilityBehaviour
	{
		
		//EnergyShieldConfig config;

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
			PlaySoundEffect();
			
			PlayParticleEffect();
        }
	}
}
