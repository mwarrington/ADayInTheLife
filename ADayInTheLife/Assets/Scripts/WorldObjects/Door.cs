using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
	public string SceneToLoad;

	void OnTriggerStay(Collider col)
	{
		if(col.gameObject.name == "Player")
		{
			if(Input.GetKeyDown(KeyCode.Space))
				Application.LoadLevel(SceneToLoad);
		}
	}
}
