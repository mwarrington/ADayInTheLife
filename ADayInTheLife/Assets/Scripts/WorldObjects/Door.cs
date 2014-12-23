using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
	public string SceneToLoad;

    private bool _nearDoor;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _nearDoor)
        {
            Application.LoadLevel(SceneToLoad);
        }
    }

	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Player")
            _nearDoor = true;
	}

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
            _nearDoor = false;
    }
}
