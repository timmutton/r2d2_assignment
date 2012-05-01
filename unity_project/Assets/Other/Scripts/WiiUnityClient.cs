using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

class WiiUnityClient
{
    const String buttonsMsg = "updateB";
    const String accelMsg = "updateA";
    const String ncMsg = "updateNC";
    const String rumbleMsg = "toggleR";
    const String irTogMsg = "toggleIR";
    const String irUpdMsg = "updateIR";
    const String exitMsg = "exit";
    const String refuseConnMsg = "refuse";
    const String acceptConnMsg = "accept";
    const String blankMsg = "null";


    private IPEndPoint ipep;
    private Socket serverSocket;
    private EndPoint remote;

    private ClientWiiState[] wiiStates;
	
	public int numWiimotes{
		get;
		private set;
	}

    public WiiUnityClient()
    {
        wiiStates = new ClientWiiState[0];
    }

    public bool StartClient()
    {		
        // start the server
        System.Diagnostics.Process wiiServer = new System.Diagnostics.Process();
        wiiServer.StartInfo.FileName = "Assets\\WiimoteServer.exe";
//		wiiServer.StartInfo.UseShellExecute = false;
		wiiServer.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

        wiiServer.Start();

        //yield WaitForSeconds(5);		
        System.Threading.Thread.Sleep(4000);

        // Servers ip (localhost) and port
        ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);

        // Create a new udp socket on client side
        serverSocket = new Socket(AddressFamily.InterNetwork,
                                        SocketType.Dgram, ProtocolType.Udp);

        // Setup local reference for Server
        remote = (EndPoint)ipep;

        // Establish connection. Sending first message in bytes to serverSocket
        string msg = "Hello, are you there?";
        serverSocket.SendTo(Encoding.ASCII.GetBytes(msg), remote);    

        // Get response from serverSocket
        String response = WaitForMsg();

        String[] msgParts = response.Split(' ');

        if (msgParts[0].Equals(acceptConnMsg))
        {
            Console.WriteLine("Connected to serverSocket at: " + remote.ToString());
            numWiimotes = int.Parse(msgParts[1]);
            wiiStates = new ClientWiiState[numWiimotes];
            for (int i = 0; i < numWiimotes; i++)
                wiiStates[i] = new ClientWiiState();
        }
        else
        {
            Console.WriteLine("Server refused connection. Most likely because no wiimotes exist");
            return false;
        }

        return true;
    }

    public void EndClient()
    {
        // Send serverSocket an exit message
        Console.WriteLine("Sending Exit signal to serverSocket");
        SendMsg(exitMsg + " 1 ");

        // Get serverSocket's exit reply
        String response = WaitForMsg();

        // Check serverSocket response
        if (response.Equals(exitMsg))
            Console.WriteLine("Server responded correctly, exiting now...");
        else
            Console.WriteLine("Unexpected serverSocket response. Exiting anyway...");

        // Close connection
        serverSocket.Close();
    }

    public void UpdateButtons(int wiimoteID)
    {
        SendMsg(buttonsMsg + " " + wiimoteID + " ");

        String response = WaitForMsg();

        if (!response.Equals(blankMsg))
            wiiStates[wiimoteID-1].ButtonsFromString(response);
    }

    public void UpdateAccel(int wiimoteID)
    {
        SendMsg(accelMsg + " " + wiimoteID + " ");

        String response = WaitForMsg();

        if (!response.Equals(blankMsg))
            wiiStates[wiimoteID-1].AccelFromString(response);
    }

    public void ToggleIR(int wiimoteID)
    {
        SendMsg(irTogMsg + " " + wiimoteID + " ");

        String response = WaitForMsg();

        if (!response.Equals(blankMsg))
            wiiStates[wiimoteID - 1].IRFromString(response);
    }

    public void UpdateIR(int wiimoteID)
    {
        SendMsg(irUpdMsg + " " + wiimoteID + " ");

        String response = WaitForMsg();

        if (!response.Equals(blankMsg))
            wiiStates[wiimoteID - 1].IRFromString(response);
    }

    public void UpdateNunchuck(int wiimoteID)
    {
        SendMsg(ncMsg + " " + wiimoteID + " ");

        String response = WaitForMsg();

        if (!response.Equals(blankMsg))
            wiiStates[wiimoteID-1].NunchuckFromString(response);
    }

    public void ToggleRumble(int wiimoteID)
    {
        SendMsg(rumbleMsg + " " + wiimoteID + " ");

        String response = WaitForMsg();

        if (!response.Equals(blankMsg))
            wiiStates[wiimoteID - 1].RumbleFromString(response);
    }

    public ClientWiiState GetWiiState(int wiimoteID)
    {
        int arrayID = wiimoteID - 1;
        if (arrayID >= 0 && arrayID < wiiStates.Length)
            return wiiStates[arrayID];
        else
        {
            Console.WriteLine("Wiimote with ID <" + wiimoteID + "> could not be found");
            return null;
        }
    }

    public void SendMsg(String msg)
    {
        serverSocket.SendTo(Encoding.ASCII.GetBytes(msg), remote);
    }

    public String WaitForMsg()
    {
        byte[] data = new byte[1024];
        int recv = serverSocket.ReceiveFrom(data, ref remote);

        return Encoding.ASCII.GetString(data, 0, recv);
    }
}

