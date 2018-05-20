using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;


namespace RPG.Characters
{
	public class Energy : MonoBehaviour {

		[SerializeField] RawImage energyBarRawImage = null;
		[SerializeField] float maxEnergyPoints = 100f;
		[SerializeField] float pointsPerHit = 10f;
		
		float currentEnergyPoints;
		
		CameraRaycaster cameraRaycaster;

		void Start () {
			currentEnergyPoints = maxEnergyPoints;
			
			cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
			cameraRaycaster.notifyRightClickObservers += ProcessRightClick;			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		void ProcessRightClick(RaycastHit raycastHit, int layerHit)
        {
            float newEnergyPoints = currentEnergyPoints - pointsPerHit;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);

            UpdateEnergyBar();
        }

        private void UpdateEnergyBar()
        {
            float xValue = -(EnergyAsPercent() / 2f) - 0.5f;
            energyBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }

		float EnergyAsPercent()
		{
			return currentEnergyPoints / maxEnergyPoints;
		}
    }
}