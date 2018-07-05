using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;


namespace RPG.Characters
{
	[ExecuteInEditMode]
	public class WeaponPickupPoint : MonoBehaviour {

		[SerializeField] WeaponConfig weaponConfig;

		[SerializeField] AudioClip pickupSFX;

		AudioSource audioSource = null;

		// Use this for initialization
		void Start () {
			audioSource = GetComponent<AudioSource>();

			InstantiateWeapon();
		}
		
		// Update is called once per frame
		void Update () {
			
			if( !Application.isPlaying)
			{
				DestroyChildren();
				//InstantiateWeapon();
			}
		}

        private void DestroyChildren()
        {
            foreach(Transform child in transform)
			{
				DestroyImmediate(child.gameObject);
			}
        }

        private void InstantiateWeapon()
        {
            var weapon = weaponConfig.GetWeaponPrefab();
			weapon.transform.position = Vector3.zero;
			Instantiate(weapon, gameObject.transform);
        }

		public void OnTriggerEnter(Collider other)
		{
			PlayerControl playerComponent = other.GetComponent<PlayerControl>();
			if(playerComponent != null)
			{
				Debug.Log("Pick up weapon");
				other.GetComponent<WeaponSystem>().PutWeaponInHand(weaponConfig);
				PlaySoundEffect();
			}
		}

		protected void PlaySoundEffect()
		{
			audioSource.PlayOneShot(pickupSFX);
		}
    }
}