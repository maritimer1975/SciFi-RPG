using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
	[CreateAssetMenu (menuName="RPG/Enemy AI/Actions/Patrol")]
	public class PatrolAction : Action
	{
		public override void Act(EnemyAI enemyAI)
		{
			Patrol(enemyAI);
		}

		private void Patrol(EnemyAI enemyAI)
        {
            Debug.Log("New Patrol Action");
			Vector3 nextWaypointPos = enemyAI.GetNextWaypointPosition();
			enemyAI.Character.SetDestination(nextWaypointPos);
			enemyAI.CycleWaypointWhenClose();
        }
	}
}