using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class ClientWiiState
{
    // All the button states of the wiimote. True if button is pressed down, false if released
    public bool A, B, Up, Down, Left, Right, Plus, Minus, One, Two, Home;

    // The accelerometer states
    public float accelX, accelY, accelZ;

    // Infrared states. Floats between (0,1)
    public bool irActive, irVisible;
    public float irMidpointX, irMidpointY, ir1PosX, ir1PosY, ir2PosX, ir2PosY;    

    public bool Rumble;

    // Numchuck data
    public bool ncC, ncZ;
    public float ncJoyX, ncJoyY;

    // Nunchuck accelerometer states
    public float ncAccelX, ncAccelY, ncAccelZ;

    public ClientWiiState()
    {
        // Initialise all data members
        A = false; B = false; Up = false; Down = false; Left = false; Right = false;
        Plus = false; Minus = false; One = false; Two = false; Home = false;

        Rumble = false;

        accelX = (float)0.0; accelY = (float)0.0; accelZ = (float)0.0;

        irMidpointX = (float)0.0; irMidpointY = (float)0.0; ir1PosX = (float)0.0; ir1PosY = (float)0.0; ir2PosX = (float)0.0; ir2PosY = (float)0.0;
        irActive = false; irVisible = false;
        

        ncC = false; ncZ = false;
        ncJoyX = (float)0.0; ncJoyY = (float)0.0;
    }

    public void ButtonsFromString(String sStates)
    {
        String[] values = sStates.Split(' ');
        
        // Assign values to the data members. Bad protocol, i know
        A = Convert.ToBoolean(values[0]); B = Convert.ToBoolean(values[1]);
        Up = Convert.ToBoolean(values[2]); Down = Convert.ToBoolean(values[3]);
        Left = Convert.ToBoolean(values[4]); Right = Convert.ToBoolean(values[5]);
        Plus = Convert.ToBoolean(values[6]); Minus = Convert.ToBoolean(values[7]);
        One = Convert.ToBoolean(values[8]); Two = Convert.ToBoolean(values[9]);
        Home = Convert.ToBoolean(values[10]);
        
    }

    public void AccelFromString(String sStates)
    {
        String[] values = sStates.Split(' ');

        accelX = float.Parse(values[0]);
        accelY = float.Parse(values[1]);
        accelZ = float.Parse(values[2]);
    }

    public void IRFromString(String sStates)
    {
        String[] values = sStates.Split(' ');

        irActive = Convert.ToBoolean(values[0]); irVisible = Convert.ToBoolean(values[1]);
        irMidpointX = float.Parse(values[2]);
        irMidpointY = float.Parse(values[3]);
        ir1PosX = float.Parse(values[4]);
        ir1PosY = float.Parse(values[5]);
        ir2PosX = float.Parse(values[6]);
        ir2PosY = float.Parse(values[7]);  

    }

    public void NunchuckFromString(String sStates)
    {
        String[] values = sStates.Split(' ');

        ncC = Convert.ToBoolean(values[0]); ncZ = Convert.ToBoolean(values[1]);
        ncJoyX = float.Parse(values[2]);
        ncJoyY = float.Parse(values[3]);

        ncAccelX = float.Parse(values[4]);
        ncAccelY = float.Parse(values[5]);
        ncAccelZ = float.Parse(values[6]);
    }

    public void RumbleFromString(String rState)
    {
        Rumble = Convert.ToBoolean(rState); 
    }
}

