using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
	public class WaypointContainer : MonoBehaviour {

		[SerializeField] Color waypointColor = new Color(255f ,0f ,0f, 1f);
		[SerializeField] float waypointRadius = .2f;
		
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		void OnDrawGizmos()
		{
			Vector3 startWaypoint = transform.GetChild(0).position;
			Vector3 previousWaypoint = startWaypoint;

			Gizmos.color = waypointColor;

			foreach(Transform waypoint in transform)
			{
				Gizmos.DrawSphere(waypoint.position, waypointRadius);
				Gizmos.DrawLine(previousWaypoint, waypoint.position);
				
				previousWaypoint = waypoint.position;
			}
			Gizmos.DrawLine(previousWaypoint, startWaypoint);
		}
	}
}