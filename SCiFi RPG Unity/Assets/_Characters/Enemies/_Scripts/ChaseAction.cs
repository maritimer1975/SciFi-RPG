using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
	[CreateAssetMenu (menuName="RPG/Enemy AI/Actions/Chase")]
	public class ChaseAction : Action 
	{
		public override void Act(EnemyAI enemyAI)
		{
			Chase(enemyAI);
		}

		public void Chase(EnemyAI enemyAI)
		{
			if(enemyAI.DistanceToPlayer <= enemyAI.ChaseRadius)
			{
				enemyAI.ChasePlayer();
			}
		}
	}
}