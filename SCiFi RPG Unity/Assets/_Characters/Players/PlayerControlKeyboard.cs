using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: make a simple WASD movement control system
//TODO: add jumping
//TODO: add flying

namespace RPG.Characters
{
	public class PlayerControlKeyboard : MonoBehaviour {

#region SERIALIZED VARIABLES
		[SerializeField] float speed = 10f;

#endregion

#region VARIABLES
		Rigidbody rb;
		HealthSystem healthSystem;

		Character character;


#endregion


		// Use this for initialization
		void Start ()
		{
			InitializeComponents(); 
		}
		
		// Update is called once per frame
		void FixedUpdate ()
		{
			if(healthSystem.IsCharacterAlive())
			{
				ScanForMovementKeyDown();
			}
		}


#region CUSTOM METHODS
	private void InitializeComponents()
	{
		rb = GetComponent<Rigidbody>();
		healthSystem = GetComponent<HealthSystem>();
		character = GetComponent<Character>();

	}

	public void ScanForMovementKeyDown()
	{
		Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed * Time.deltaTime;
		character.MoveKeyboard(movement);
	}

#endregion

	}
}