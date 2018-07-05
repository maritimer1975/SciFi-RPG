using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
	[CreateAssetMenu(menuName="RPG/Enemy AI/Decisions/IsPlayerWithinChaseRange")]
	public class IsPlayerWithinChaseRange : Decision 
	{
		public override bool Decide(EnemyAI enemyAI)
		{
			if(enemyAI.DistanceToPlayer <= enemyAI.ChaseRadius)
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