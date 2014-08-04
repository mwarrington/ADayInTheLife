using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.Examples;

public class ItemInteract : MonoBehaviour
{
	private GameObject _currentItem;
	
	public GameObject ItemPrefab;
	public Camera ComputerViewCamera;
	public Transform PlaceToInstantiate;
	public bool SimpleItem;

	void OnTriggerStay(Collider col)
	{
		if(col.tag == "Player")
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				if(SimpleItem)
				{
					_currentItem = Instantiate(ItemPrefab, PlaceToInstantiate.position, Quaternion.identity) as GameObject;
					ComputerViewCamera.depth = 5;
					Camera.main.depth = -5;
					col.gameObject.GetComponent<PlayerScript>().enabled = false;
					//this.gameObject.GetComponent<Usable>().overrideName = " ";
					//this.gameObject.GetComponent<Usable>().overrideUseMessage = " ";
				}
				else
				{
					//This is for items that load a new scenes.
				}
			}
		}
	}
}
