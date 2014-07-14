﻿using UnityEngine;
using System.Collections;

public class LabraryTeleporter : MonoBehaviour
{
    public GameObject TeleportLocation;

    void OnTriggerEnter(Collider col)
    {
		Debug.Log (col.gameObject.tag);
        if (col.gameObject.tag == "Player")
        {
			Debug.Log ("Kill");
            float orriginalY = col.transform.position.y;
            col.transform.position = new Vector3(TeleportLocation.transform.position.x, orriginalY, TeleportLocation.transform.position.z);
        }
    }
}