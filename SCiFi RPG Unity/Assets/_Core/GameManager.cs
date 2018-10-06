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

		private PlayerStats playerStats;

		[SerializeField] string filePath = @"C:\Test Data\playerInfo.dat";

		private CharacterStat GM_stat;

		private void Start()
		{
			playerObject = GameObject.FindWithTag("Player");
			playerCharacter = playerObject.GetComponent<Character>();
			playerStats = playerObject.GetComponent<PlayerStats>();
		}

		public void Save()
		{
			BinaryFormatter bf = new BinaryFormatter();
//			CharacterStat data = playerCharacter.dexterityStat;

			using( FileStream file = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite) )
			{
				//bf.Serialize(file, data);
				bf.Serialize(file, playerStats);
				file.Close();
			}
		}

		public void Load()
		{
			BinaryFormatter bf = new BinaryFormatter();
			
			using( FileStream file = File.Open(filePath, FileMode.Open, FileAccess.Read))
			{
				//CharacterStat data = (CharacterStat)bf.Deserialize(file);
				playerStats = (PlayerStats)bf.Deserialize(file);
				file.Close();

				//playerCharacter.dexterityStat = data;
				
			} 
		}
	}
}