using UnityEngine;
using System.Collections;

public class PrefabLoaderScript : MonoBehaviour {

	public static PrefabLoaderScript instance;

	public AudioClip
	radiator,
	metalChairScrape,
	food,
	fork,
	cymbol,
	gore;

	void Awake()
	{
		PrefabLoaderScript.instance = this;
	}

	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {

	
	}
}
