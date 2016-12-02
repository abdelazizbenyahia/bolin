using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class NetSet : NetworkBehaviour {
    


    private Vector3 fixProj;
    private Quaternion facing;
    List<string> sList = new List<string>();
    
    public static bool theServer;
    public static bool assigned = true;


	// Use this for initialization
	void Start () {
        if(isLocalPlayer)
            GetComponent<BallMotor>().joystick = GameObject.Find("VirtualJoystick").GetComponentInChildren<VirtualJoystick>();

        if (isServer)
            theServer = true;

        
    }
    /*void OnPlayerDisconnected(NetworkPlayer player)
    {
        print("Clean up after player " + player);
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
        NetworkServer.connections[1].Disconnect();
    }*/
    //test connection
    NetworkConnection ClientConnection;

    // Update is called once per frame
    void Update () {

        print(theServer);
        foreach (string temp in sList)
            print(temp);
        if (isServer)
        {
            //print(NetworkServer.connections.Count);
            if(NetworkServer.connections.Count == 2 && ClientConnection == null)
            {
                ClientConnection = NetworkServer.connections[1];
            }
            //print(NetworkServer.connections.Contains(ClientConnection));

        }

        if (!isLocalPlayer)
        {
            return;
        }

    }

    public void assignName()
    {
        if (assigned)
            return;
        GameObject[] ss = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < ss.Length; ++i)
        {
            bool flag = false;
            foreach(string temp in sList)
                flag = (ss[i].name == temp);
            if (!flag)
            {
                ss[i].GetComponent<BallMotor>().joystick = GameObject.Find("VirtualJoystick").GetComponentInChildren<VirtualJoystick>();
                CmdAssign(ss[i].name);
                assigned = true;
            }

        }

    }

    [Command]
    public void CmdAssign(string assigned) { RpcAssign(assigned); }

    [ClientRpc]
    public void RpcAssign(string assigned) {
        sList.Add(assigned);
    }

}
