using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class HintManager : MonoBehaviour
{
	public GameObject HintToInstantiate;

	void OnConversationEnd (Transform actor)
	{
		Debug.Log ("Kil");
	}
}
