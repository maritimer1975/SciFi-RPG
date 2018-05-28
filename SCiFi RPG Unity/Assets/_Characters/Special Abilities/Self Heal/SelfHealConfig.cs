using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
	[CreateAssetMenu(menuName="RPG/Special Ability/Self Heal", fileName="SH LVXX")]
	public class SelfHealConfig : AbilityConfig
	{
		[Header("Self Heal Specific")]
		[SerializeField] float healAmount = 100f;

		public override void AttachComponentTo(GameObject gameObjectToAttachTo)
		{
			var behaviourComponent = gameObjectToAttachTo.AddComponent<SelfHealBehaviour>();
			behaviourComponent.SetConfig(this);
			behaviour = behaviourComponent;
		}

		public float GetHealAmount()
		{
			return healAmount;
		}
	}
}