using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
	public abstract class AbilityBehaviour : MonoBehaviour {
		protected AbilityConfig config;

		const float PARTICLE_CLEAN_UP_DELAY = 20f;

		public abstract void Use(AbilityUseParams useParams);

		protected void PlayParticleEffect()
        {
			Transform particleSystemTransform = GameObject.FindWithTag("Particle Systems").transform;
			GameObject particleObject = Instantiate(config.GetParticle(), particleSystemTransform); 
			particleObject.GetComponent<ParticleSystem>().Play();
			StartCoroutine(DestroyParticleWhenFinished(particleObject));
        }

		IEnumerator DestroyParticleWhenFinished(GameObject particlePrefab)
		{
			while (particlePrefab.GetComponent<ParticleSystem>().isPlaying)
			{
				yield return new WaitForSeconds(PARTICLE_CLEAN_UP_DELAY);
			}
			Destroy(particlePrefab);
			yield return new WaitForEndOfFrame();
		}

		public void SetConfig(AbilityConfig configToSet)
		{
			config = configToSet;
		}
	}
}