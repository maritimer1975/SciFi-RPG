using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
	public class HealthSystem : MonoBehaviour {

#region SERIALIZED VARIABLES
		[SerializeField] float maxHealthPoints = 100f;
		[SerializeField] Image healthBar;
	    [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;
		[SerializeField] float deathVanishSeconds = 2f;

#endregion	

#region VARIABLES
		const string DEATH_TRIGGER = "Death";

		float currentHealthPoints;

		Animator anim;

		AudioSource audioSource;

		Character character;
#endregion

#region UNITY METHODS		
		
		void Awake()
		{
			
		}

		// Use this for initialization
		void Start ()
		{
			InitializeComponents();
			
			currentHealthPoints = maxHealthPoints;
		}
		
		// Update is called once per frame
		void Update ()
		{
			UpdateHealthBar();
		}
#endregion

#region PROERTIES SET/GET
		public float healthAsPercentage { get { return currentHealthPoints / (float)maxHealthPoints; } }

#endregion

#region CUSTOM METHODS
		public AudioSource GetAudioSource()
		{
			return audioSource;
		}

		public void Heal(float healthPoints)
		{
			currentHealthPoints = Mathf.Clamp(currentHealthPoints + healthPoints,0f, maxHealthPoints);
		}

		public void IncreaseHealth(float healthPoints)
		{
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + healthPoints, 0f, maxHealthPoints);
        }

		private void InitializeComponents()
		{
			anim = GetComponent<Animator>();
			audioSource = GetComponent<AudioSource>();
			character = GetComponent<Character>();
		}

		public bool IsCharacterAlive()
        {
            return healthAsPercentage > Mathf.Epsilon;
        }

		IEnumerator KillCharacter()
        {
            StopAllCoroutines();
			
			character.Kill();

			anim.SetTrigger(DEATH_TRIGGER);

			var playerComponent= GetComponent<WeaponSystem>();
			if(playerComponent && playerComponent.isActiveAndEnabled) // lazy evaluation
			{
				PlayDeathSound();
				            
            	// wait reload the scene or death screen SceneManager.something
            	yield return new WaitForSecondsRealtime(audioSource.clip.length);
            	SceneManager.LoadScene(0);   
			}
			else
			{
				Destroy(gameObject, deathVanishSeconds);
			}
        }

		private void PlayDamageSound()
        {
            if(damageSounds.Length > 0)
			{
				int index = UnityEngine.Random.Range(0, damageSounds.Length);
				audioSource.clip = damageSounds[index];
				audioSource.Play();
			}
			else
			{
				Debug.LogError("No damage sounds assinged.");
			}
        }

        private void PlayDeathSound()
        {
            int index = UnityEngine.Random.Range(0, deathSounds.Length);
            audioSource.clip = deathSounds[index];
            audioSource.Play();
        }

		private void ReduceHealth(float healthPoints)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - healthPoints, 0f, maxHealthPoints);
        }

		 public void TakeDamage(float damagePoints)
        {
            bool characterDies = (currentHealthPoints - damagePoints <= 0);  // must happen before we reduce the health
            
            ReduceHealth(damagePoints);

            PlayDamageSound();

            if (characterDies)
            {
                StartCoroutine(KillCharacter());
            }
        }

		private void UpdateHealthBar()
		{
			if(healthBar)
			{
				healthBar.fillAmount = healthAsPercentage;
			}
		}
#endregion
	}
}