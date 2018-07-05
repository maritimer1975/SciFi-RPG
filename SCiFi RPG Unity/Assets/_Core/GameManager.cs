using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using RPG.Characters;

namespace RPG.Core
{
	public class GameManager : GenericSingletonClass<GameManager> 
	{
		private GameObject playerObject;
		private Character playerCharacter;

		[SerializeField] string filePath = @"C:\Test Data\playerInfo.dat";

		private CharacterStat GM_stat;

		private void Start()
		{
			playerObject = GameObject.FindWithTag("Player");
			playerCharacter = playerObject.GetComponent<Character>();
		}

		public void Save()
		{
			BinaryFormatter bf = new BinaryFormatter();

			FileStream file;
			
			if(!File.Exists(filePath))
			{
				file = File.Create(filePath);
			}
			else
			{
				file = File.Open(filePath, FileMode.Open);
			}

			CharacterStat data = playerCharacter.dexterity;

			bf.Serialize(file, data);
			file.Close();
		}

		public void Load()
		{
			if(File.Exists(filePath))
			{
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(filePath, FileMode.Open);

				CharacterStat data = (CharacterStat)bf.Deserialize(file);
				file.Close();

				playerCharacter.dexterity = data;

			}
		}
	}
}