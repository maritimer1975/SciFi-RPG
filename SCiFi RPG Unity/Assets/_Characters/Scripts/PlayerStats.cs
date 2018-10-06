using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
	[System.Serializable]
	public class PlayerStats : MonoBehaviour {

		[SerializeField] public CharacterStat[] stats;


		// Use this for initialization
		void Start () {
			for(int i = 0; i < stats.Length; i++)
			{
				stats[i] = new CharacterStat(stats[i].baseValue);
			}
		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}