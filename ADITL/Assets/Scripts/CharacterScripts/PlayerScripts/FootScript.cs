using UnityEngine;
using System.Collections;
//This Script will need to be added to PlayerScript.cs and then deleted
public class FootScript : MonoBehaviour {


	public GameObject footstep;
	bool canPlayStep = false;
	public PlayerScript player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (player.isWalkingForward || player.isWalkingLeft || player.isWalkingRight || player.isWalkingBack)
		{
			canPlayStep = true;
		}
	
	
	}



	void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.tag);
		if (other.tag == "Floor" && canPlayStep)
		{
			
			footstep.audio.Play();
		}

	}
}
