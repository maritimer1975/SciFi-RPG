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

		public override AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
             return objectToAttachTo.AddComponent<SelfHealBehaviour>();
        }

		public float GetHealAmount()
		{
			return healAmount;
		}
	}
}