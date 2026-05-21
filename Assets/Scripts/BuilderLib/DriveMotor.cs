using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveMotor : MonoBehaviour
{
    private const float StallTorque = 7;
    private float _momentOfInertia = 0.0000255f;

    [HideInInspector] public float gearRatio = 5.85f;

    public float motorSpeed;

    private const float Kv = 6000 / 12;

    // Start is called before the first frame update
    void Start()
    {
        motorSpeed = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DriveBroke())
        {
            ResetMotor();
        }
    }

    /// <summary>
    /// Updates the speed of the drive motor values.
    /// Resets the calculated new speed to the real new speed
    /// </summary>
    /// <param name="voltage"></param>
    /// <param name="realSpeed"></param>
    /// <returns></returns>
    public float DriveSimUpdate(float voltage, float realSpeed)
    {
        //reset drive speed to real speed
        motorSpeed = realSpeed;

        if (FMS.RobotState == RobotState.disabled)
        {
            motorSpeed = realSpeed;
        }
        //w = RPM Wm = Max Rpm Ts = stall Torque J = Moi
        //t = -((J*Wm)/Ts) * ln((Wm-w)/Wm)
        //dw = ((Wm-w)/Wm)*(Ts/(J*Wm)) * dt
        //Wm = V*KV
        //V = voltage
        //Kv = RPM per Voltage
        //dw = ((V*Kv-w)/(V*Kv)) * (Ts/(J*V*Kv))
        motorSpeed += ((voltage * Kv - motorSpeed)/(12*Kv)) * (StallTorque/((_momentOfInertia /Mathf.Pow(gearRatio,2))*12*Kv));
        return motorSpeed;
    }

    /// <summary>
    /// resets the motor speed in case of issues.
    /// </summary>
    public void ResetMotor()
    {
        motorSpeed = 0;
    }
    
    /// <summary>
    /// Detects rare NAN casses and resets the motor speed
    /// </summary>
    /// <returns></returns>
    private bool DriveBroke()
    {
        return !float.IsNaN(motorSpeed) && !float.IsInfinity(motorSpeed);
    }
}
