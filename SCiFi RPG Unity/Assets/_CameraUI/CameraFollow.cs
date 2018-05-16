using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;  // TODO: Consider rewiring

namespace RPG.CameraUI
{
	public class CameraFollow : MonoBehaviour {

		[SerializeField]
		TransformVariable target;

		GameObject player;

		// Use this for initialization
		void Start () {
			player = GameObject.FindGameObjectWithTag("Player");
		}
		
		// Update is called once per frame
		void Update () {
			transform.position = player.transform.position;
		}
	}
}
