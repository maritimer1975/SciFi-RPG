using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Characters
{

	[System.Serializable]
	public class CharacterStat
	{
#region VARIABLES
		[SerializeField] string statName;
		public float baseValue;
		private readonly List<StatModifier> statModifiers;
		private bool isDirty = true;
		private float finalValue;

#endregion

		public CharacterStat(float _baseValue)
		{
			baseValue = _baseValue;
			statModifiers = new List<StatModifier>();
		}

#region PROPERTIES GET / SET

		public float Value 
		{ 
			get 
			{ 
				if(isDirty)
				{
					finalValue = CalculateFinalValue();
					isDirty = false;
				}
				return finalValue;
			}
		}

#endregion

#region METODS
		public void AddModifier(StatModifier mod)
		{
			isDirty = true;
			statModifiers.Add(mod);
			Debug.Log("Stat: " + this);
		}

		public float CalculateFinalValue()
		{
			float finalValue = baseValue;

			for (int i = 0; i < statModifiers.Count; i++)
			{
				finalValue += statModifiers[i].value;
			}
			return (float)Math.Round(finalValue, 4);
		}

		public bool RemoveModifier(StatModifier mod)
		{
			isDirty = true;
			return statModifiers.Remove(mod);
		}

#endregion

	}
}