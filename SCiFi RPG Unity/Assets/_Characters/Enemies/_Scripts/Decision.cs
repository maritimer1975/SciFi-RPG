using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
	public abstract class Decision : ScriptableObject 
	{
		public abstract bool Decide (EnemyAI enemyAI);
		
	}
}