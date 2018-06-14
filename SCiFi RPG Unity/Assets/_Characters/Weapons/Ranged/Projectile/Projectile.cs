using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;	// TODO: consider rewiring

namespace RPG.Characters
{
	public class Projectile : MonoBehaviour 
	{

		[SerializeField] float projectileSpeed = 10f;
		[SerializeField] GameObject shooter;  // so we can inspect when paused
		
		float damageCaused = 10f;

		const float DESTROY_DELAY = 0.05f;

		public float DamageCaused { 
			get { return damageCaused; }
			set { damageCaused = value; }
		}

		void OnCollisionEnter(Collision col)
		{
			var layerCollidedWith = col.gameObject.layer;
			if (shooter && layerCollidedWith != shooter.layer)
				DamageIfDamageable(col);
		}

		private void DamageIfDamageable(Collision col)
		{
			var healthSystem = col.gameObject.GetComponent<HealthSystem>();

			if (healthSystem)
			{
				healthSystem.TakeDamage(damageCaused);
			}
			
			Destroy(gameObject, DESTROY_DELAY);
		}

		public void SetShooter(GameObject shooter)
		{
			this.shooter = shooter;
		}

		public float GetDefaultLaunchSpeed()
		{
			return projectileSpeed;
		}
	}
}