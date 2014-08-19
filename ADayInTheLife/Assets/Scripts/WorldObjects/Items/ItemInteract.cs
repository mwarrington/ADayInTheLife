using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.Examples;

public class ItemInteract : MonoBehaviour
{
	protected GameManager myGameManager;
	protected GameObject currentItem;
	
	public GameObject ItemPrefab;
	public Camera ItemViewCamera;
	public Transform PlaceToInstantiate;
	public bool SimpleItem,
				ItemActive;

	protected virtual void Start()
	{
		myGameManager = GameObject.FindObjectOfType<GameManager>();
		
		this.gameObject.GetComponent<Usable>().overrideName = ItemPrefab.GetComponent<ItemController>().OverrideName;
		this.gameObject.GetComponent<Usable>().overrideUseMessage = ItemPrefab.GetComponent<ItemController>().OverrideUseMessage;
	}

	protected virtual void OnTriggerStay(Collider col)
	{
		if(col.tag == "Player")
		{
			if(Input.GetKeyDown(KeyCode.Space) && !ItemActive)
			{
				if(SimpleItem)
				{
					currentItem = Instantiate(ItemPrefab, PlaceToInstantiate.position, Quaternion.identity) as GameObject;
					currentItem.GetComponent<ItemController>().StartPage.IsActive = true;
					ItemViewCamera.enabled = true;
					myGameManager.MainCamera.enabled = false;
					col.gameObject.GetComponent<PlayerScript>().enabled = false;
					this.gameObject.GetComponent<Usable>().overrideName = " ";
					this.gameObject.GetComponent<Usable>().overrideUseMessage = "Press the Escape Key to exit";
					ItemActive = true;
				}
				else
				{
					//This is for items that load a new scenes.
				}
			}
		}
	}
}
