using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.Examples;

public class ItemInteract : MonoBehaviour
{
	private GameObject _currentItem;
	private GameManager _myGameManager;
	
	public GameObject ItemPrefab;
	public Camera ComputerViewCamera;
	public Transform PlaceToInstantiate;
	public bool SimpleItem,
				ItemActive;

	void Start()
	{
		_myGameManager = GameObject.FindObjectOfType<GameManager>();
		
		this.gameObject.GetComponent<Usable>().overrideName = ItemPrefab.GetComponent<ItemController>().OverrideName;
		this.gameObject.GetComponent<Usable>().overrideUseMessage = ItemPrefab.GetComponent<ItemController>().OverrideUseMessage;
	}

	void OnTriggerStay(Collider col)
	{
		if(col.tag == "Player")
		{
			if(Input.GetKeyDown(KeyCode.Space) && !ItemActive)
			{
				if(SimpleItem)
				{
					_currentItem = Instantiate(ItemPrefab, PlaceToInstantiate.position, Quaternion.identity) as GameObject;
					ComputerViewCamera.enabled = true;
					_myGameManager.MainCamera.enabled = false;
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
