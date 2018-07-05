using System.Collections;
using UnityEngine;


namespace RPG.Characters
{
	[RequireComponent (typeof(Character))]

    public abstract class AbilityBehaviour : MonoBehaviour {
		protected AbilityConfig config;

		const float PARTICLE_CLEAN_UP_DELAY = 20f;
		const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

		public abstract void Use(GameObject target = null);

		protected void PlayAbilityAnimation()
		{
			var character = GetComponent<Character>();
			var anim = GetComponent<Animator>();
			var animOverrideController = character.getAnimOverrideController;
			anim.runtimeAnimatorController = animOverrideController;
			animOverrideController[DEFAULT_ATTACK] = config.AnimClip;
			anim.SetTrigger(ATTACK_TRIGGER);
		}

		protected void PlayParticleEffect()
        {
			GameObject particleObject = Instantiate(config.GetParticle(), gameObject.transform);
			ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
			particleSystem.Play(true);
			StartCoroutine(DestroyParticleWhenFinished(particleObject));
        }

		protected void PlaySoundEffect()
		{
			AudioSource audioSource = GetComponent<AudioSource>();
			var soundEffect = config.GetRandomSoundClip();
			audioSource.PlayOneShot(soundEffect);
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