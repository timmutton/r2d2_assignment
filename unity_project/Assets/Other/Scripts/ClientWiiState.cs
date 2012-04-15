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

    // Numchuck data
    public bool ncC, ncZ;
    public float ncJoyX, ncJoyY;

    public ClientWiiState()
    {
        // Initialise all data members
        A = false; B = false; Up = false; Down = false; Left = false; Right = false;
        Plus = false; Minus = false; One = false; Two = false; Home = false;

        accelX = (float)0.0; accelY = (float)0.0; accelZ = (float)0.0;

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

    public void NunchuckFromString(String sStates)
    {
        String[] values = sStates.Split(' ');

        ncC = Convert.ToBoolean(values[0]); ncZ = Convert.ToBoolean(values[1]);
        ncJoyX = float.Parse(values[2]);
        ncJoyY = float.Parse(values[3]);
    }
}

