using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;

namespace RPG.Characters
{
    public class SpecialAbilities : MonoBehaviour {

#region ERIALIZED VARIABLES
		
		[SerializeField] AbilityConfig[] abilities;

		[SerializeField] float maxEnergyPoints = 100f;

		[SerializeField] float regenPointsPerSecond = 2f;

		[SerializeField] Image energyBar = null;
		[SerializeField] AudioClip outOfEnergySound;
#endregion		

#region VARIABLES
		float currentEnergyPoints;

		AudioSource audioSource;
		
		PlayerControl player;

#endregion

#region UNITY METHODS
		void Awake()
		{
			
		}

		void Start ()
		{
			InitializeComponents();
			
			currentEnergyPoints = maxEnergyPoints;
			
			UpdateEnergyBar();
			
			AttachAbilities();
			
			// RegisterObservers();			
		}

		void Update()
		{
			// energy regen mechanic
			if(currentEnergyPoints < maxEnergyPoints)
			{
				IncreaseEnergy(regenPointsPerSecond * Time.deltaTime);
			}

		}
#endregion

#region  PROPERTIES SET/GET

		float energyAsPercent { get { return currentEnergyPoints / maxEnergyPoints; } }

#endregion

#region CUSTOM METHODS

		public void AttemptSpecialAbility(int abilityIndex, GameObject target = null)
        {
            var energyCost = abilities[abilityIndex].GetEnergyCost();
            
            if(energyCost <= currentEnergyPoints)
            {
                ReduceEnergy(energyCost);
                
				//TODO: make abilities work
				// var abilityUseParams = new AbilityUseParams(currentEnemy, baseDamage);
                abilities[abilityIndex].Use(target);
            }
			else
			{
				audioSource.PlayOneShot(outOfEnergySound);
			}
        }
		
		private void AttachAbilities()
        {
            for( int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex ++)
            {
                abilities[abilityIndex].AttachAbilityTo(gameObject);
            }
        }

		public void IncreaseEnergy(float energyPoints)
		{
			currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + energyPoints, 0f, maxEnergyPoints);
			UpdateEnergyBar();
		}

		private void InitializeComponents()
        {
            //cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
			audioSource = GetComponent<AudioSource>();
			player = GetComponent<PlayerControl>();
        }

		public bool IsEnergyAvailable(int abilityIndex)
		{
			return abilities[abilityIndex].GetEnergyCost() <= currentEnergyPoints;
		}
		
		public bool IsEnergyAvailable(float energyPoints)
		{	
			return energyPoints <= currentEnergyPoints;
		}

		public int GetNumberOfAbilities()
		{
			return abilities.Length;
		}

		public void ReduceEnergy(float energyPoints)
        {			
			currentEnergyPoints = Mathf.Clamp(currentEnergyPoints - energyPoints, 0f, maxEnergyPoints);
			UpdateEnergyBar();
        }

        // private void RegisterObservers()
        // {
        //     cameraRaycaster.notifyMouseOverEnemy += OnMouseOverEnemy;
        // }

        private void UpdateEnergyBar()
        {
			// TODO: remove magic numbers
            energyBar.fillAmount = energyAsPercent;
        }
    }
#endregion
}