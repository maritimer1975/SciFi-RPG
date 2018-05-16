using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
// Transform variables

    [CreateAssetMenu]
    public class TransformVariable : ScriptableObject {

        [SerializeField]
        public Transform value { get; set; }

    }
}
