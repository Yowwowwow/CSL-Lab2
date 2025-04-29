using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class WirelessMotionController : MonoBehaviour
{

    const string hostIP = "192.168.128.1";
    const int port = 80;

    private SocketClient socketClient;

    public int move = 0;
    public float yaw = 0;
    float baseYaw;
    bool initYaw = false;
    public int vvv;
    bool working = true;

    private void Awake() {
        socketClient = new SocketClient(hostIP, port);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!working) return;
        try
        {
            vvv = int.Parse(socketClient.mes.Split('\t')[0].Split(' ')[3]);
        }
        catch (System.Exception)
        {
            return;
        }
        string mes = socketClient.mes;
        move = mes[3] - '0';
        if (!initYaw && move == 1) { baseYaw = float.Parse(mes.Split('\t')[2].Split(' ')[0]); initYaw = true; }
        if (initYaw)
        {
            yaw = float.Parse(mes.Split('\t')[2].Split(' ')[0]) - baseYaw;
            if (yaw < -180) yaw += 360;
            else if (yaw > 180) yaw -= 360;
        }
    }

    void OnDestroy () {
        socketClient.Close();
    }
}
