using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    public GameObject TeleportLocation;
	public string SceneToTeleport;
	public bool TeleportsToScene;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
			if(!TeleportsToScene)
			{
	            float orriginalY = col.transform.position.y;
	            col.transform.position = new Vector3(TeleportLocation.transform.position.x, orriginalY, TeleportLocation.transform.position.z);
			}
			else
				Application.LoadLevel(SceneToTeleport);
        }
    }
}
