using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class SocketClient
{
    private Socket socketClient;
    private Thread thread;
    private byte[] data = new byte[1024];
    public string mes;

    public SocketClient(string hostIP, int port) {
        thread = new Thread(() => {
            // while the status is "Disconnect", this loop will keep trying to connect.
            while (true) {
                try {
                    socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socketClient.Connect(new IPEndPoint(IPAddress.Parse(hostIP), port));
                    // while the connection
                    while (true) {
                        /*********************************************************
                         * TODO: you need to modify receive function by yourself *
                         *********************************************************/
                        socketClient.Send(Encoding.UTF8.GetBytes("Hello, world"));
                        if (socketClient.Available < 100) {
                            Thread.Sleep(1);
                            continue;
                        }
                        int length = socketClient.Receive(data);
                        mes = Encoding.UTF8.GetString(data, 0, length).Split("\n")[1];
                        Debug.Log("Recieve message: " + mes);
                        // */
                    }
                } catch (Exception ex) {
                    if (socketClient != null) {
                        socketClient.Close();
                    }
                    Debug.Log(ex.Message);
                }
            }
        });
        thread.IsBackground = true;
        thread.Start();
    }

    public void Close() {
        thread.Abort();
        if (socketClient != null) {
            socketClient.Close();
        }
    }
}
