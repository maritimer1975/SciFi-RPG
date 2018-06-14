using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using RPG.Characters;
using System;

namespace RPG.CameraUI
{
    public class CameraRaycaster : MonoBehaviour
	{
#region VARIABLES
		[SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D enemyCursor = null;

		[SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);
		
		private const int clickToWalkLayer = 8;
		float maxRaycastDepth = 100f; // Hard coded value

		Rect currentScreenRect;

#endregion

#region	DELEGATES
		// Setup delegates for broadcasting layer changes to other classes
		// mouse over terrain event setup
		public delegate void OnMouseOverTerrain(Vector3 destination);	// declare new delegate type
		public event OnMouseOverTerrain notifyMouseOverTerrain; // instantiate observer set

		// mouse over enemy event setup
		public delegate void OnMouseOverEnemy(EnemyAI enemy);	// declare new delegate type
		public event OnMouseOverEnemy notifyMouseOverEnemy;	// instantiate observer set
#endregion

#region UNITY METHODS
		void Update()
		{
			
			currentScreenRect = new Rect(0, 0, Screen.width, Screen.height); // move inside update to support screen resize
			if(EventSystem.current.IsPointerOverGameObject())
			{
				// implement UI interaction
			}else
			{ 
				PerformRaycasts();
			}
		}
#endregion  

#region CUSTOM METHODS
        
		 private bool IsMouseOverEnemy(Ray ray)
        {
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit, maxRaycastDepth))
			{
				var targetObj = hit.collider.gameObject;
				var enemy = targetObj.GetComponent<EnemyAI>();
				if( enemy )
				{
					Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
					NotifyMouseOverEnemyObservers(enemy);
					return true;
				}
			}
			return false;
        }

		private bool IsMouseOverTerrain(Ray ray)
        {
			RaycastHit hit;

			int layerMask = 1 << clickToWalkLayer;

			if(Physics.Raycast(ray, out hit, maxRaycastDepth, layerMask))
			{
				Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);	
				NotifyMouseOverTerrainObservers(hit.point);
				return true;
			}
			return false;
        }

		private void NotifyMouseOverEnemyObservers(EnemyAI enemy)
        {
            notifyMouseOverEnemy(enemy);
        }
        private void NotifyMouseOverTerrainObservers(Vector3 destination)
        {
            notifyMouseOverTerrain(destination);
        }

		private void PerformRaycasts()
        {
            if(currentScreenRect.Contains(Input.mousePosition))
			{
			
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				
				if( IsMouseOverEnemy(ray) ) { return; }
				if( IsMouseOverTerrain(ray) ) { return; }
			}
        }
#endregion
	}
}