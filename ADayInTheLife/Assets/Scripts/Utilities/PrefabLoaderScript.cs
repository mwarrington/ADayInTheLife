using UnityEngine;
using System.Collections;

public class PrefabLoaderScript : MonoBehaviour
{
	public static PrefabLoaderScript instance;

	public AudioClip Countdown30,
					 Countdown10;

	void Awake()
	{
		PrefabLoaderScript.instance = this;
	}
}
