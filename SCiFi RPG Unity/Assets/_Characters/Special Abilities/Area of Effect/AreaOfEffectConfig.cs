using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
	[CreateAssetMenu(menuName="RPG/Special Ability/Area of Effect", fileName="AOE LVXX")]
	public class AreaOfEffectConfig : AbilityConfig
	{
		[Header("Area Of Effect Specific")]
		[SerializeField] float damageToEachTarget = 10f;
		[SerializeField] float effectRadius = 5f;

		public override AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
             return objectToAttachTo.AddComponent<AreaOfEffectBehaviour>();
        }

        public float DamageToEachTarget()
		{
			return damageToEachTarget;
		}

		public float GetEffectRadius()
		{
			return effectRadius;
		}
	}
}