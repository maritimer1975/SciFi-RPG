using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
	public abstract class Action : ScriptableObject 
	{
		public abstract void Act(EnemyAI enemyAI);
	}
}