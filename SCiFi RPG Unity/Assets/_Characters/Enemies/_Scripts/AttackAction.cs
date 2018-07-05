using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
	[CreateAssetMenu (menuName="RPG/Enemy AI/Actions/Attack")]
	public class AttackAction : Action {

		public override void Act(EnemyAI enemyAI)
		{
			Attack(enemyAI);
		}

		public void Attack(EnemyAI enemyAI)
		{
			Debug.Log("New Attack Acton");
			
			if(enemyAI.DistanceToPlayer <= enemyAI.CurrentWeaponRange)
			{
				enemyAI.AttackPlayer();
			}
		}
	}
}