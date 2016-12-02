using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class BallMotor : NetworkBehaviour {
    public float moveSpeed = 5.0f;
    public float jumpHeight = 0.1f;

    
    public float terminalRotationSpeed = 25.0f;
    public Vector3 MoveVector { set; get; }
    private Rigidbody thisRigidbody;
    public VirtualJoystick joystick;
    NetSet NS;
	// Use this for initialization
	void Start () {
        name = GetComponent<NetworkIdentity>().netId.ToString();
        thisRigidbody = gameObject.GetComponent<Rigidbody>();
        thisRigidbody.maxAngularVelocity = terminalRotationSpeed;
        NS = GetComponent<NetSet>();
        NS.assignName();
	}
	
	// Update is called once per frame
	void Update () {
        if (joystick == null)
        {
            print("adsa:   "+name);
            return;
        }
        MoveVector = PoolInput();
        CmdMove(MoveVector);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            thisRigidbody.velocity = Vector3.zero;
            thisRigidbody.AddForce(MoveVector* 10, ForceMode.VelocityChange);
        }

	}
    [Command]
    public void CmdBoost()
    {
        RpcBoost();
    }

    [ClientRpc]
    public void RpcBoost()
    {
        thisRigidbody.velocity = Vector3.zero;
        thisRigidbody.AddForce((MoveVector + Vector3.up * jumpHeight) * 10, ForceMode.VelocityChange);
    }

    [Command]
    public void CmdMove(Vector3 v3)
    {
        RpcMove(v3);
    }


    [ClientRpc]
    public void RpcMove (Vector3 v3)
    {
        thisRigidbody.AddForce((v3 * moveSpeed));
    }
    private Vector3 PoolInput()
    {
        Vector3 dir = Vector3.zero;
        // dir.x = Input.GetAxis("Horizontal");
        // dir.z = Input.GetAxis("Vertical");
        dir.x = joystick.Horizontal();
        dir.z = joystick.Vertical();
        if (dir.magnitude > 1)
            dir.Normalize();
        return dir;
    }
}
