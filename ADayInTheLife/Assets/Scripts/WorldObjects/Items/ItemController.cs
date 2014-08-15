using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.Examples;

public class ItemController : MonoBehaviour
{
	protected GameManager myGameManager;
	protected GameObject player;
	protected GameObject worldObject;
	
	public Camera ItemCamera;
	public string OverrideName,
				  OverrideUseMessage;

	protected virtual void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		foreach (ItemInteract ii in GameObject.FindObjectsOfType<ItemInteract>())
		{
			string iiName = ii.name + "(Clone)";
			if(iiName == this.name)
			{
				worldObject = ii.gameObject;
			}
		}
		myGameManager = GameObject.FindObjectOfType<GameManager>();
	}

	protected virtual void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			TurnOff ();
		}
	}

	protected virtual void TurnOff()
	{
		myGameManager.MainCamera.enabled = true;
		player.GetComponent<PlayerScript>().enabled = true;
		worldObject.GetComponent<Usable>().overrideName = OverrideName;
		worldObject.GetComponent<Usable>().overrideUseMessage = OverrideUseMessage;
		worldObject.GetComponent<ItemInteract>().ItemActive = false;
		Destroy(this.gameObject);
	}
}
