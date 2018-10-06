using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Characters;

namespace RPG.Core
{
	public class TestGameManager : MonoBehaviour
	{
		private Character playerCharacter;
		private PlayerStats playerStats;
		void Start()
		{
			GameObject playerObject = GameObject.FindWithTag("Player");
			playerCharacter = playerObject.GetComponent<Character>();
			playerStats = playerObject.GetComponent<PlayerStats>();
		}
		
		void OnGUI()
		{
			if(GUI.Button( new Rect( 10, 100, 200, 30), "Dexterity: Add Modifier +10"))
			{
				//playerCharacter.dexterityStat.AddModifier(new StatModifier(10));
				playerStats.stats[1].AddModifier( new StatModifier(10));
			}

			if(GUI.Button( new Rect( 10, 140, 200, 30), "Dexterity: Remove Modifier +10"))
			{
				playerCharacter.dexterityStat.baseValue -= 10;
			}

			if(GUI.Button( new Rect( 10, 180, 100, 30), "Save"))
			{
				GameManager.Instance.Save();
			}

			if(GUI.Button( new Rect( 10, 220, 100, 30), "Load"))
			{
				GameManager.Instance.Load();
			}

			if(GUI.Button( new Rect( 10, 260, 100, 30), "Update Dexterity"))
			{
				playerCharacter.dexterityValue = playerCharacter.dexterityStat.CalculateFinalValue();
			}

		}

	}
}