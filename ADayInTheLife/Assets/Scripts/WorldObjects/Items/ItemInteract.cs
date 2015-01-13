using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.Examples;

public class ItemInteract : MonoBehaviour
{
    static protected Vector3 myLastPos;

    private PlayerScript _myPlayer;
    private GameObject _subject;
    private bool _nearItem;

	protected GameManager myGameManager;
	protected GameObject currentItem;
	
	public GameObject ItemPrefab;
	public Camera ItemViewCamera;
	public Transform PlaceToInstantiate;
    public string SceneToLoad;
	public bool SimpleItem,
				ItemActive;

	protected virtual void Start()
	{
		myGameManager = GameObject.FindObjectOfType<GameManager>();
        _myPlayer = FindObjectOfType<PlayerScript>();

        if (SimpleItem)
        {
            this.gameObject.GetComponent<Usable>().overrideName = ItemPrefab.GetComponent<ItemController>().OverrideName;
            this.gameObject.GetComponent<Usable>().overrideUseMessage = ItemPrefab.GetComponent<ItemController>().OverrideUseMessage;
        }
        else if (myGameManager.LastLevelLoaded == SceneToLoad)
        {
            FindObjectOfType<PlayerScript>().transform.position = myLastPos;
        }
	}

    void Update()
    {
        if (_nearItem)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !ItemActive)
            {
                //Simple items don't load new scenes
                if (SimpleItem)
                {
                    currentItem = Instantiate(ItemPrefab, PlaceToInstantiate.position, Quaternion.identity) as GameObject;
                    currentItem.GetComponent<ItemController>().StartPage.IsActive = true;
                    ItemViewCamera.enabled = true;
                    myGameManager.MainCamera.enabled = false;
                    _subject.GetComponent<PlayerScript>().enabled = false;
                    this.gameObject.GetComponent<Usable>().overrideName = " ";
                    this.gameObject.GetComponent<Usable>().overrideUseMessage = "Press the Escape Key to exit";
                    ItemActive = true;
                }
                else //Saves the position of the player before loading the new scene
                {
                    //This is for items that load a new scenes.
                    myLastPos = _myPlayer.transform.position;
                    Application.LoadLevel(SceneToLoad);
                }
            }
        }
    }

	protected virtual void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player")
		{
            _nearItem = true;
            _subject = col.gameObject;
		}
	}

    protected virtual void OnTriggerExit(Collider col)
    {
        if (col.gameObject == _subject)
            _nearItem = false;
    }
}
