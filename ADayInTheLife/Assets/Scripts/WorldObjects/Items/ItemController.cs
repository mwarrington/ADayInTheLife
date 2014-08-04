using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour
{
	protected GameObject player;
	protected Camera itemCamera;

	protected virtual void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

	protected virtual void Update()
	{
		TurnOff();
	}

	protected virtual void TurnOff()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Camera.main.depth = 5;
			player.GetComponent<PlayerScript>().enabled = true;
			Destroy(this.gameObject);
		}
	}
}
