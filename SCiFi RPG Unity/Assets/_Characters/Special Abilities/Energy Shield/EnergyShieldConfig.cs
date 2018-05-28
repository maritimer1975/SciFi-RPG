using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
	[CreateAssetMenu(menuName="RPG/Special Ability/Energy Shield", fileName="ES LVXX")]
	public class EnergyShieldConfig : AbilityConfig
	{
		[Header("Energy Shield Specific")]
		[SerializeField] float shieldPoints = 10f;
		[SerializeField] float shieldDuration = 10f;

		public override void AttachComponentTo(GameObject gameObjectToAttachTo)
		{
			var behaviourComponent = gameObjectToAttachTo.AddComponent<EnergyShieldBehaviour>();
			behaviourComponent.SetConfig(this);
			behaviour = behaviourComponent;
		}

		public float GetShieldDuration()
		{
			return shieldDuration;
		}

	}
}