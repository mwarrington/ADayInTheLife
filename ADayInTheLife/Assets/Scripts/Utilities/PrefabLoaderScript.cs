using UnityEngine;
using System.Collections;

public class PrefabLoaderScript : MonoBehaviour
{
	public static PrefabLoaderScript instance;

	public AudioClip Countdown30,
					 Countdown10,
					 CloudHover,
					 CloudClick,
					 PlayerSelect1,
					 PlayerSelect2,
					 SpiralHover,
					 SpiralTurn;

	void Awake()
	{
		PrefabLoaderScript.instance = this;
	}
}
