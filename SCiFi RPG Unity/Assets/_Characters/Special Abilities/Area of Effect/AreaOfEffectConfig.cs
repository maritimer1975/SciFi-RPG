using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
	[CreateAssetMenu(menuName="RPG/Special Ability/Area of Effect", fileName="DefaultAreaOfEffect")]
	public class AreaOfEffectConfig : SpecialAbility
	{
		[Header("Area Of Effect Specific")]
		[SerializeField] float damageToEachTarget = 10f;
		[SerializeField] float effectRadius = 5f;

		public override void AttachComponentTo(GameObject gameObjectToAttachTo)
		{
			var behaviourComponent = gameObjectToAttachTo.AddComponent<AreaOfEffectBehaviour>();
			behaviourComponent.SetConfig(this);
			behaviour = behaviourComponent;
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