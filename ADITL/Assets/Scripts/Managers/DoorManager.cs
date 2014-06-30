using UnityEngine;
using System.Collections;

public class DoorManager : MonoBehaviour {

	//Simple script to load the next scene when the player collides with the GameObject


	public bool canTeleport = false;
	// Use this for initialization
	void Start () {
		canTeleport = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (canTeleport)
		{
			Application.LoadLevel("hallway");
		}

	}

	void OnTriggerEnter(Collider collision)
	{

			
		

	}
}
