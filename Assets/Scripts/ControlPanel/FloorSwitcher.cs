using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorSwitcher : MonoBehaviour
{
    [SerializeField] List<Toggle> floors;

    void Start()
    {
        ControlPanel.Instance.AddFloorSwitchListener(FloorSwitched);
    }

    private void FloorSwitched(int floor)
    {
        if(floor >= 0 && floor < floors.Count)
        {
            floors[floor].isOn = true;
        }
        else
        {
            Debug.LogError($"There is no floor with index {floor}");
        }
    }
}