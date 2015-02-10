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
            OpenDoor();
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

    private void OpenDoor()
    {
        Application.LoadLevel(SceneToLoad);
        //Animation stuff will go here
    }
}
