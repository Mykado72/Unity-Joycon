using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyconAffect : MonoBehaviour
{

    private List<Joycon> joycons;
    private Joycon j;
    // Values made available via Unity
    public float[] stick;
    public Vector3 gyro;
    public Vector3 accel;
    public bool left;
    public Quaternion orientation;

    void Start()
    {
        gyro = new Vector3(0, 0, 0);
        accel = new Vector3(0, 0, 0);
        // get the public Joycon array attached to the JoyconManager in scene
        joycons = JoyconManager.Instance.j;
        foreach (Joycon joy in joycons)
        {
            if (joy.isLeft==this.left)
            {
                j = joy;
            }
        }
        if (j==null)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // make sure the Joycon only gets checked if attached
        if (j !=null)
        {
            // GetButtonDown checks if a button has been pressed (not held)
            if (j.GetButtonDown(Joycon.Button.SHOULDER_2))
            {
                Debug.Log("Shoulder button 2 pressed");
                // GetStick returns a 2-element vector with x/y joystick components
                Debug.Log(string.Format("Stick x: {0:N} Stick y: {1:N}", j.GetStick()[0], j.GetStick()[1]));

                // Joycon has no magnetometer, so it cannot accurately determine its yaw value. Joycon.Recenter allows the user to reset the yaw value.
                j.Recenter();
            }
            // GetButtonDown checks if a button has been released
            if (j.GetButtonUp(Joycon.Button.SHOULDER_2))
            {
                Debug.Log("Shoulder button 2 released");
            }
            // GetButtonDown checks if a button is currently down (pressed or held)
            if (j.GetButton(Joycon.Button.SHOULDER_2))
            {
                Debug.Log("Shoulder button 2 held");
            }

            if (j.GetButtonDown(Joycon.Button.DPAD_DOWN))
            {
                Debug.Log("Rumble");

                // Rumble for 200 milliseconds, with low frequency rumble at 160 Hz and high frequency rumble at 320 Hz. For more information check:
                // https://github.com/dekuNukem/Nintendo_Switch_Reverse_Engineering/blob/master/rumble_data_table.md

                j.SetRumble(160, 320, 0.6f, 200);

                // The last argument (time) in SetRumble is optional. Call it with three arguments to turn it on without telling it when to turn off.
                // (Useful for dynamically changing rumble values.)
                // Then call SetRumble(0,0,0) when you want to turn it off.
            }

            stick = j.GetStick();

            // Gyro values: x, y, z axis values (in radians per second)
            gyro = j.GetGyro();
            if (gyro.y<-1)
                Debug.Log("l'avant tourne vers le bas ");
            if (gyro.y>1)
                Debug.Log("l'avant tourne vers nous ");

            // Accel values:  x, y, z axis values (in Gs)
            accel = j.GetAccel();
            /*
            if (accel.x > 1)
                Debug.Log("pointe vers la haut");
            if (accel.x < -1)
                Debug.Log("pointe vers le bas");
            if (accel.z < -0.99)
                Debug.Log("a plat");
            if (accel.z > 0.99)
                Debug.Log("a l'envers");
            if (accel.y > 1)
                Debug.Log("sur la tranche, posé sur le rail");
            if (accel.y < -1)
                Debug.Log("sur la tranche, rail vers le haut");
            */

            orientation = j.GetVector();
            if (j.GetButton(Joycon.Button.DPAD_UP))
            {
                gameObject.GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                gameObject.GetComponent<Renderer>().material.color = Color.blue;
            }
            gameObject.transform.RotateAround(gameObject.transform.localPosition, transform.forward, -gyro.z);
            gameObject.transform.RotateAround(gameObject.transform.localPosition, transform.right, -gyro.y);
            gameObject.transform.RotateAround(gameObject.transform.localPosition, transform.up, -gyro.x);
            // gameObject.transform.rotation = orientation;
        }
    }
}