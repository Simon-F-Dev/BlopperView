using UnityEngine;
using System.Collections;



public class NetworkManager : MonoBehaviour {
	private const string typeName = "Blopper";
	private const string gameName = "BlopperRoom";
	//om vi ska kora servern lokalt


	//Starta server
	private void StartServer()
	{
        MasterServer.ipAddress = "127.0.0.1";
		Network.InitializeServer(3, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}

	//Feedback om den startat
	void OnServerInitialized()
	{
		Debug.Log("Server Initializied");
	}

	//GUI: knappar
	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				StartServer();
			
			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
				RefreshHostList();
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}
		}
	}

	//joining server
	private HostData[] hostList;
	
	private void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}

	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}
	
	void OnConnectedToServer()
	{
		Debug.Log("Server Joined");
	}

	[RPC]
	void PrintText (string text)
	{
		//let's move the head!
		//Debug.Log("BEFORE FLOAT PARSING text: " + text);

		//"3,4,5" -> {3,4,5}
		string[] inValues = text.Split (',');

		float x = float.Parse (inValues[0]);
		float y = float.Parse (inValues[1]);
		float z = float.Parse (inValues[2]);

		Debug.Log("text: " + text + "x/y/z: " + x.ToString() + ", " + y.ToString() + ", " + z.ToString());

		//Cardboard.SDK.HeadPose.Position;
		//Cardboard.SDK.HeadPose.Position.Set(0.0F, 0.0F, 0.0F);
		Cardboard.SDK.HeadPose.Position.Set(x, y, z);

	}

	// Use this for initialization
	void Start () {
        MasterServer.ipAddress = "127.0.0.1";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
