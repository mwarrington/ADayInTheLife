using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class CafeteriaTray : MonoBehaviour
{
	private GameObject _player;
	private Dictionary<string, Sprite> _foodTable = new Dictionary<string, Sprite>();
	private float _endPos = -62;
	private int _trayIndex = 0;
	
	public SpriteRenderer[] FoodTraySections = new SpriteRenderer[5];
	public float PlayerPosOffset;

	void Start()
	{
        //Finds player script for reference
        _player = FindObjectOfType<PlayerScript>().gameObject;

        //Adds all Game Objects tagged as "CafeteriaFood" to a list "cafeteriaFoods"
        List<GameObject> cafeteriaFoods = new List<GameObject>();
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("CafeteriaFood"))
        {
            cafeteriaFoods.Add(go);
        }

        //This populates the _foodTable Dictionary based on unique object names found in "cafeteriaFoods"
		foreach (GameObject go in cafeteriaFoods)
		{
            if (_foodTable.Count > 0)
            {
                bool isUnique = true;
                foreach (string s in _foodTable.Keys)
                {
                    if (s == go.name)
                    {
                        isUnique = false;
                        break;
                    }
                }

                if(isUnique)
                {
                    _foodTable.Add(go.name, Resources.Load<Sprite>("Art/Textures/Environment/Served" + go.name));
                }
            }
            else
            {
                _foodTable.Add(go.name, Resources.Load<Sprite>("Art/Textures/Environment/Served" + go.name));
            }
		}
	}

	void Update()
	{
        //This handles the movement of the food tray
		if(this.transform.position.x > _endPos)
			this.transform.position = new Vector3 (_player.transform.position.x + PlayerPosOffset, this.transform.position.y, this.transform.position.z);

        //These lines of code are only called if the player is NOT in an active convo
        if (!DialogueManager.IsConversationActive)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //Checks to see if the player clicks on a game object tagged "CafeteriaFood" then it shows that food on the tray.
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "CafeteriaFood" && Input.GetKeyDown(KeyCode.Mouse0) && _trayIndex < 5)
                {
                    FoodTraySections[_trayIndex].sprite = _foodTable[hit.collider.name];
                    _trayIndex++;
                }
            }
        }
	}
}