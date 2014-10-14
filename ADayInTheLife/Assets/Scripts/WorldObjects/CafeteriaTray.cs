using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		_player = FindObjectOfType<PlayerScript>().gameObject;
        List<GameObject> cafeteriaFoods = new List<GameObject>();
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("CafeteriaFood"))
        {
            cafeteriaFoods.Add(go);
        }

        //This populates the _foodTable Dictionary
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
                    _foodTable.Add(go.name, Resources.Load<Sprite>("Environment/Served" + go.name));
                }
            }
            else
            {
                _foodTable.Add(go.name, Resources.Load<Sprite>("Environment/Served" + go.name));
            }
		}
	}

	void Update()
	{
		if(this.transform.position.x > _endPos)
			this.transform.position = new Vector3 (_player.transform.position.x + PlayerPosOffset, this.transform.position.y, this.transform.position.z);

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			if(hit.collider.tag == "CafeteriaFood" && Input.GetKeyDown(KeyCode.Mouse0) && _trayIndex < 5)
			{
				FoodTraySections[_trayIndex].sprite = _foodTable[hit.collider.name];
				_trayIndex++;
			}
		}
	}
}