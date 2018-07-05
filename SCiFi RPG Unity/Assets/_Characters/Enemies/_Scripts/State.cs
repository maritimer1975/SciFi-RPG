using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Characters
{
	[CreateAssetMenu (menuName = "RPG/Enemy AI/State")]
	public class State : ScriptableObject
	{
		
		public Color sceneGizmoColor = Color.grey;
		public Action[] actions;
		public Transition[] transitions;

		public void UpdateState(EnemyAI enemyAI)
		{
			DoActions(enemyAI);
			CheckTransitions(enemyAI);
		}

		private void DoActions(EnemyAI enemyAI)
		{
			for( int i = 0; i < actions.Length; i++)
			{
				actions[i].Act(enemyAI);
			}
		}

		private void CheckTransitions(EnemyAI enemyAI)
		{
			for(int i = 0; i < transitions.Length; i++)
			{
				bool decisionSucceded = transitions[i].decision.Decide(enemyAI);

				if(decisionSucceded)
				{
					enemyAI.TransitionToState(transitions[i].trueState);
				}
				else
				{
					enemyAI.TransitionToState(transitions[i].falseState);
				}
			}
		}
	}
}