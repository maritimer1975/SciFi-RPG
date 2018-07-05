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
		void Start()
		{
			GameObject playerObject = GameObject.FindWithTag("Player");
			playerCharacter = playerObject.GetComponent<Character>();
		}
		
		void OnGUI()
		{
			if(GUI.Button( new Rect( 10, 100, 100, 30), "Dexterity Up"))
			{
				playerCharacter.dexterity.baseValue += 10;
			}

			if(GUI.Button( new Rect( 10, 140, 100, 30), "Dexterity Down"))
			{
				playerCharacter.dexterity.baseValue -= 10;
			}

			if(GUI.Button( new Rect( 10, 180, 100, 30), "Save"))
			{
				GameManager.Instance.Save();
			}

			if(GUI.Button( new Rect( 10, 220, 100, 30), "Load"))
			{
				GameManager.Instance.Load();
			}

		}

	}
}