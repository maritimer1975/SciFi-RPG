using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
	[CreateAssetMenu(menuName="RPG/Enemy AI/Decisions/IsPlayerWithinAttackRange")]
	public class IsPlayerWithinAttackRange : Decision
	{
		public override bool Decide(EnemyAI enemyAI)
		{
			if(enemyAI.DistanceToPlayer <= enemyAI.CurrentWeaponRange)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}

