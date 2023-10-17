using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPoses : MonoBehaviour
{
    [SerializeField]
    public Dictionary<string, int> namesToIndices = new Dictionary<string, int>();
    [SerializeField]
    public Dictionary<int, string> indicesToNames = new Dictionary<int, string>();
    public int Count = 0;

    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            string name = transform.GetChild(i).name;
            namesToIndices.Add(name, i);
            indicesToNames.Add(i, name);
        }

        Count = namesToIndices.Count;

        if (Count != indicesToNames.Count || Count != namesToIndices.Count || Count == 0)
        {
            Debug.LogError("CameraPoses has an invalid number of children");
            return;
        }

        // print
        foreach (KeyValuePair<string, int> kvp in namesToIndices)
        {
            Debug.Log("namesToIndices CameraPoses: " + kvp.Key + " " + kvp.Value);
        }
        foreach (KeyValuePair<int, string> kvp in indicesToNames)
        {
            Debug.Log("indicesToNames CameraPoses: " + kvp.Key + " " + kvp.Value);
        }
    }


    public int GetIndex(string name)
    {
        return namesToIndices[name];
    }

    public string GetName(int index)
    {
        return indicesToNames[index];
    }

    public Transform GetTransform(int index)
    {
        return transform.GetChild(index).transform;
    }

    public Transform GetTransform(string name)
    {
        return transform.GetChild(GetIndex(name)).transform;
    }


    public int Next(int current)
    {
        return (current + 1) % Count;
    }

    public int Previous(int current)
    {
        int previous = (current - 1) % Count;
        if (previous < 0) previous += Count;
        return previous;
    }
}



// next camera
// previous camera


