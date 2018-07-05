using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{

[System.Serializable]
	public class CharacterStat
	{
#region VARIABLES
		public float baseValue;
		private readonly List<StatModifier> statModifiers;


#endregion

		public CharacterStat(float _baseValue)
		{
			baseValue = _baseValue;
			statModifiers = new List<StatModifier>();
		}
	}
}