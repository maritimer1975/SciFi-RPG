using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;

namespace RPG.Characters
{
    public class Energy : MonoBehaviour {

#region VARIABLES
		
		[SerializeField] float maxEnergyPoints = 100f;

		[SerializeField] float regenPointsPerSecond = 2f;

		[SerializeField] Image energyOrb = null;
		float currentEnergyPoints;
		CameraRaycaster cameraRaycaster;
#endregion

#region UNITY METHODS
		void Awake()
		{
			InitializeComponents();  // make sure components are initialized before start
		}

		void Start () {
			currentEnergyPoints = maxEnergyPoints;
			UpdateEnergyBar();
			
			// RegisterObservers();			
		}

		void Update()
		{
			// energy regen mechanic
			if(currentEnergyPoints < maxEnergyPoints)
			{
				AddEnergy(regenPointsPerSecond * Time.deltaTime);
			}

		}
#endregion
		
#region CUSTOM METHODS

		public void AddEnergy(float energyPoints)
		{
			currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + energyPoints, 0f, maxEnergyPoints);
			UpdateEnergyBar();
		}

		public void ReduceEnergy(float energyPoints)
        {			
			currentEnergyPoints = Mathf.Clamp(currentEnergyPoints - energyPoints, 0f, maxEnergyPoints);
			UpdateEnergyBar();
        }
		float EnergyAsPercent()
		{
			return currentEnergyPoints / maxEnergyPoints;
		}

		private void InitializeComponents()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        }

		public bool IsEnergyAvailable(float energyPoints)
		{
			return energyPoints <= currentEnergyPoints;
		}

        // private void RegisterObservers()
        // {
        //     cameraRaycaster.notifyMouseOverEnemy += OnMouseOverEnemy;
        // }

        private void UpdateEnergyBar()
        {
			// TODO: remove magic numbers
            energyOrb.fillAmount = EnergyAsPercent();
        }
    }
#endregion
}