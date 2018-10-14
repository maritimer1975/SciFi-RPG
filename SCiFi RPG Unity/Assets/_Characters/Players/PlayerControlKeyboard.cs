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

		Camera playerCamera;


#endregion


		// Use this for initialization
		void Start ()
		{
			InitializeComponents(); 
		}
		
		void Update()
		{
			// get the camera forward vector
			// update player to look at the camera vector

		}

		// Update is called once per frame
		void FixedUpdate ()
		{
			if(healthSystem.IsCharacterAlive())
			{
				ScanForMovementKeyDown();

				ScanForMouseDirection();
			}
		}


#region CUSTOM METHODS
	private void InitializeComponents()
	{
		rb = GetComponent<Rigidbody>();
		healthSystem = GetComponent<HealthSystem>();
		character = GetComponent<Character>();
		playerCamera = Camera.main;

	}

	public void ScanForMovementKeyDown()
	{
		Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed * Time.deltaTime;
		character.MoveKeyboard(movement);
	}

	public void ScanForMouseDirection()
	{
		if(Input.GetButton("Fire2"))
		{
			transform.localEulerAngles = playerCamera.transform.localEulerAngles;
		}
	}

#endregion

	}
}