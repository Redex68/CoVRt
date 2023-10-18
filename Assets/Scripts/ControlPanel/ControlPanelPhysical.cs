using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO.Ports;
using UnityEditor.Experimental.GraphView;

public class ControlPanelPhysical : ControlPanel
{

    private UnityEvent cameraSelect = new();
    private UnityEvent<int> floorSwitch = new();

    private SerialPort data_stream;
    private bool reading = false;

    private int floor = 0;
    private Vector2 joystick = Vector2.zero;
    private bool joystickWasHeldDown = false;
    private float dial1 = 0;
    private float dial2 = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        }

        StartCoroutine(readDataReceived());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Data format:
    //f|xxxx|yyyy|b|dddd|eeee
    //f - floor (0 or 1)
    //xxxx - joystick x axis [0, 1023]
    //yyyy - joystick y axis [0, 1023]
    //b - joystick button (0 or 1)
    //dddd - dial 1 [0, 1023]
    //eeee - dial 2 [0, 1023]
    private IEnumerator readDataReceived()
    {
        while(true)
        {
            if(reading)
            {
                string newData = data_stream.ReadLine();
                string[] segments = newData.Split('|');
                if(segments.Length != 6) continue;

                int newFloor = int.Parse(segments[0]);
                int _joystickX = int.Parse(segments[1]);
                int _joystickY = int.Parse(segments[2]);
                bool joystickDown = int.Parse(segments[3]) == 1;
                int _dial1 = int.Parse(segments[4]);
                int _dial2 = int.Parse(segments[5]);

                joystick = new Vector2((_joystickX - 512) / 512.0f, (_joystickY - 512) / 512.0f);
                dial1 = (_dial1 - 512) / 512.0f;
                dial2 = (_dial2 - 512) / 512.0f;

                if(newFloor != floor)
                {
                    floorSwitch.Invoke(newFloor);
                    floor = newFloor;
                }
                if(joystickDown && !joystickWasHeldDown)
                {
                    cameraSelect.Invoke();
                    joystickWasHeldDown = true;
                }
                else if(!joystickDown)
                {
                    joystickWasHeldDown = false;
                }
            }
            else yield return new WaitForSeconds(1);
        }
    }


    public override void AddCameraSelectListener(UnityAction action)
    {
        cameraSelect.AddListener(action);
    }

    public override void AddFloorSwitchListener(UnityAction<int> action)
    {
        floorSwitch.AddListener(action);
    }

    public override float GetDial1Position()
    {
        return dial1;
    }

    public override float GetDial2Position()
    {
        return dial2;
    }

    public override Vector2 GetJoystickDirection()
    {
        return joystick;
    }
}
