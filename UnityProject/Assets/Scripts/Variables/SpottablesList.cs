using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpottablesList : ScriptableObject
{
    [Serializable]
    public struct SpottableObject {
        [SerializeField] public GameObject obj;
        /// <summary> How long it takes to spot the object when in LOS. Cannot be 0. <summary>
        [SerializeField] public float spotTime;
    }

    public List<SpottableObject> spottables;
}