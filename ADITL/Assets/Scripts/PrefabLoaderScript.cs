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

	//I changed that shit. WHATS GOING TO HAPPEN?!?!?!
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {

	
	}
}
