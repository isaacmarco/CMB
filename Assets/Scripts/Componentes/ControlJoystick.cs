using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlJoystick : MonoBehaviour
{
    
    private Vector2 axisJoystick; 

    public Vector2 AxisJoystick {
        get { return this.axisJoystick; }
    }

        
    void Update()
    {
        // el vector esta normalizado 
        axisJoystick = new Vector2(
            Input.GetAxis("Vertical"), 
            Input.GetAxis("Horizontal")
        );

    }
}
