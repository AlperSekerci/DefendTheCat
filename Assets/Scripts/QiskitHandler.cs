using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using Gates = AgentTeam.Gates;

public class QiskitHandler : MonoBehaviour
{
    public static QiskitHandler Instance { get; private set; }

    public string host = "127.0.0.1";
    public int port = 15920;    
    private Socket socket;
    private bool connected = false;

    public BlochSphere[] qbits;
    public AgentTeam[] agentTeams;
    
    #region Memory & Data Info
    private MemoryStream wStream; // w = write
    private BinaryWriter writer;     
    private int stateBytes;
    private int circuitBytes;
    private int totalSendBytes;
    private int receiveBytes;
    private byte[] receiveBuffer;
    private const int EACH_QBIT_FLOAT_CT = 2;
    #endregion

    #region UI
    public GameObject errorObj;
    #endregion

    private void Start()
    {
        Instance = this;
        ConnectToQiskit();
    }

    private void SetupMemoryStreams()
    {
        if (wStream != null) return; // already done
        stateBytes = sizeof(float) * qbits.Length * EACH_QBIT_FLOAT_CT;
        circuitBytes = agentTeams.Length * qbits.Length;
        totalSendBytes = stateBytes + circuitBytes;
        receiveBytes = Mathf.RoundToInt(Mathf.Pow(2, qbits.Length));
        receiveBuffer = new byte[receiveBytes];        
        wStream = new MemoryStream(totalSendBytes);
        writer = new BinaryWriter(wStream);        
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
            errorObj.SetActive(true);
        }
    }

    public byte[] SampleCircuitOutputs()
    {
        if (!connected)
        {
            Debug.LogError("QiskitHandler: Not connected to the qiskit server yet.");
            return null;
        }

        SendData();
        ReceiveData();
        return receiveBuffer;
    }

    private void SendData()
    {
        wStream.Position = 0;
        WriteQBits();
        WriteCircuit();
        socket.Send(wStream.GetBuffer());
    }

    private void WriteQBits()
    {
        foreach (BlochSphere qbit in qbits)
        {
            writer.Write(qbit.theta);
            writer.Write(qbit.phi);
        }
    }

    private void WriteCircuit()
    {
        foreach (AgentTeam team in agentTeams)
        {
            int gateCount = team.gates.Length;
            for (int i = gateCount - 1; i >= 0; --i)
            {
                int gate = (int)team.gates[i];
                writer.Write((byte)gate);
            }                    
        }
    }

    private void ReceiveData()
    {
        socket.Receive(receiveBuffer);        
    }

    private void OnApplicationQuit()
    {
        if (socket != null && socket.Connected)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}