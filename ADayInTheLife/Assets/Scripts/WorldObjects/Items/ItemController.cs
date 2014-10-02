using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem.Examples;

public class ItemController : MonoBehaviour
{
	public Dictionary<string, Page> Pages = new Dictionary<string, Page>();
	
	public Page CurrentPage
	{
		get
		{
			foreach(Page p in Pages.Values)
			{
				if(p.IsActive)
					currentPage = p;
			}
			
			return currentPage;
		}
	}
	protected Page currentPage;

	protected GameManager myGameManager;
	protected GameObject player;
	protected GameObject worldObject;
	
	public Page StartPage;
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

	public virtual void ChangePage(string nextPage)
	{
		CurrentPage.transform.localPosition = new Vector3 (currentPage.transform.localPosition.x, 0);
		CurrentPage.IsActive = false;
		Pages[nextPage].IsActive = true;
	}

	protected virtual void TurnOff()
	{
		myGameManager.MainCamera.enabled = true;
		ItemCamera.enabled = false;
		player.GetComponent<PlayerScript>().enabled = true;
		worldObject.GetComponent<Usable>().overrideName = OverrideName;
		worldObject.GetComponent<Usable>().overrideUseMessage = OverrideUseMessage;
		worldObject.GetComponent<ItemInteract>().ItemActive = false;
		Destroy(this.gameObject);
	}
}
