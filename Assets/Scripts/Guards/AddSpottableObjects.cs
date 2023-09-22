using System.Collections.Generic;
using UnityEngine;
using static SpottablesList;

/// <summary> Adds all of the objects added to it in the editor to the SpottablesList on game starts </summary>
public class AddSpottableObjects : MonoBehaviour
{
    [SerializeField]
    List<SpottableObject> spottables = new List<SpottableObject>();
    [SerializeField] SpottablesList list;

    void Awake()
    {
        list.spottables.Clear();
        list.spottables.AddRange(spottables);
        Destroy(GetComponent<AddSpottableObjects>());
    }
}