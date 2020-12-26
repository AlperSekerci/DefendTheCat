using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class QiskitHandler : MonoBehaviour
{
    public string host = "127.0.0.1";
    public int port = 15920;    
    private Socket socket;
    private bool connected = false;

    public BlochSphere[] qbits;
    public AgentTeam[] agentTeams;
    public int sampleSize = 100;

    #region Memory & Data Info
    private MemoryStream wStream; // w = write
    private BinaryWriter writer; 
    private MemoryStream rStream; // r = read
    private BinaryReader reader;
    private int stateBytes;
    private int circuitBytes;
    private int totalSendBytes;
    private int receiveBytes;
    private byte[] receiveBuffer;
    private const int EACH_QBIT_FLOAT_CT = 2;
    #endregion

    private void Start()
    {
        ConnectToQiskit();
    }

    private void SetupMemoryStreams()
    {
        if (wStream != null) return; // already done
        stateBytes = sizeof(float) * qbits.Length * EACH_QBIT_FLOAT_CT;
        circuitBytes = agentTeams.Length * qbits.Length;
        totalSendBytes = stateBytes + circuitBytes;
        receiveBytes = sampleSize;
        receiveBuffer = new byte[receiveBytes];
        wStream = new MemoryStream(totalSendBytes);
        writer = new BinaryWriter(wStream);
        rStream = new MemoryStream(receiveBuffer);
        reader = new BinaryReader(rStream);
    }

    private void ConnectToQiskit()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            Debug.Log("QiskitHandler: Trying to connect to " + host + ":" + port + ".");
            socket.Connect(host, port);
            Debug.Log("QiskitHandler: connected to " + host + ":" + port);
            connected = true;
            SetupMemoryStreams();
        }
        catch (Exception e)
        {
            Debug.LogError("[QiskitHandler] " + e + ": " + e.StackTrace);
        }
    }

    public void SampleCircuitOutputs()
    {
        if (!connected)
        {
            Debug.LogError("QiskitHandler: Not connected to the qiskit server yet.");
            return;
        }

        SendData();
        ReceiveData();
    }

    private void SendData()
    {
        
    }

    private void ReceiveData()
    {
        
    }
}