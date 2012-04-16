using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
    
class WiiUnityClient
{
    private const String buttonsMsg = "updateB";
    private const String accelMsg = "updateA";
    private const String ncMsg = "updateNC";
    private const String exitMsg = "exit";
    private const String refuseConnMsg = "refuse";
    private const String acceptConnMsg = "accept";
    private const String blankMsg = "null";


    private IPEndPoint ipep;
    private Socket server;
    private EndPoint Remote;

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
		wiiServer.Start();
		
		//yield WaitForSeconds(5);		
		System.Threading.Thread.Sleep(1500);
		
        byte[] data = new byte[1024];

        // Servers ip (localhost) and port
        ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);

        // Create a new udp socket on client side
        server = new Socket(AddressFamily.InterNetwork,
                                        SocketType.Dgram, ProtocolType.Udp);

        // Establish connection. Sending first message in bytes to server
        string confirmConn = "Hello, are you there?";
        data = Encoding.ASCII.GetBytes(confirmConn);
        server.SendTo(data, data.Length, SocketFlags.None, ipep);

        // Setup reciever for messages from server
        IPEndPoint sender = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);
        Remote = (EndPoint)sender;

        // Get response from server
        data = new byte[1024];
        int recv = server.ReceiveFrom(data, ref Remote);

        String connReply = Encoding.ASCII.GetString(data, 0, recv);
        String[] msgParts = connReply.Split(' ');

        if (msgParts[0].Equals(acceptConnMsg))
        {
            Debug.Log("Connected to server at: " + Remote.ToString());
            numWiimotes = int.Parse(msgParts[1]);
            wiiStates = new ClientWiiState[numWiimotes];
            for (int i = 0; i < numWiimotes; i++)
                wiiStates[i] = new ClientWiiState();
        }
        else
        {
            Debug.Log("Wiimote server refused connection. Most likely because no wiimotes were found");
            return false;
        }

        return true;
    }

    public void EndClient()
    {
        // Send server an exit message
        Debug.Log("Sending Exit signal to Wiimote server");
        String msg = exitMsg + " 1 ";
        server.SendTo(Encoding.ASCII.GetBytes(msg), Remote);

        // Get server's exit reply
        byte[] data = new byte[1024];
        int recv = server.ReceiveFrom(data, ref Remote);
        String stringData = Encoding.ASCII.GetString(data, 0, recv);

        // Check server response
        if (stringData.Equals(exitMsg))
            Debug.Log("Wiimote Server responded correctly, exiting now...");
        else
            Debug.Log("Unexpected wiimote server response. Exiting anyway...");

        // Close connection
        server.Close();
    }

    public void UpdateButtons(int wiimoteID)
    {
        String msg = buttonsMsg + " " + wiimoteID + " ";
        server.SendTo(Encoding.ASCII.GetBytes(msg), Remote);

        // Get server's exit reply
        byte[] data = new byte[1024];
        int recv = server.ReceiveFrom(data, ref Remote);

        // The server will return a blank message if the wiimoteID doesnt exist
        String response = Encoding.ASCII.GetString(data, 0, recv);
        if (!response.Equals(blankMsg))
            wiiStates[wiimoteID-1].ButtonsFromString(response);
    }

    public void UpdateAccel(int wiimoteID)
    {
        String msg = accelMsg + " " + wiimoteID + " ";
        server.SendTo(Encoding.ASCII.GetBytes(msg), Remote);

        // Get server's exit reply
        byte[] data = new byte[1024];
        int recv = server.ReceiveFrom(data, ref Remote);

        // The server will return a blank message if the wiimoteID doesnt exist
        String response = Encoding.ASCII.GetString(data, 0, recv);
        if (!response.Equals(blankMsg))
            wiiStates[wiimoteID-1].AccelFromString(response);
    }

    public void UpdateNunchuck(int wiimoteID)
    {
        String msg = ncMsg + " " + wiimoteID + " ";
        server.SendTo(Encoding.ASCII.GetBytes(msg), Remote);

        byte[] data = new byte[1024];
        int recv = server.ReceiveFrom(data, ref Remote);

        String response = Encoding.ASCII.GetString(data, 0, recv);
        if (!response.Equals(blankMsg))
            wiiStates[wiimoteID-1].NunchuckFromString(response);
    }

    public ClientWiiState GetWiiState(int wiimoteID)
    {
        int arrayID = wiimoteID - 1;
        if (arrayID >= 0 && arrayID < wiiStates.Length)
            return wiiStates[arrayID];
        else
        {
            Debug.Log("Wiimote with ID <" + wiimoteID + "> could not be found");
            return null;
        }
    }
}

