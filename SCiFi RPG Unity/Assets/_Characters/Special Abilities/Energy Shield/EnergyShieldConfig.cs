using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
	[CreateAssetMenu(menuName="RPG/Special Ability/Energy Shield", fileName="ES LVXX")]
	public class EnergyShieldConfig : AbilityConfig
	{
		[Header("Energy Shield Specific")]
		//[SerializeField] float shieldPoints = 10f;
		[SerializeField] float shieldDuration = 10f;

		public override AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
             return objectToAttachTo.AddComponent<EnergyShieldBehaviour>();
        }

		public float GetShieldDuration()
		{
			return shieldDuration;
		}

	}
}