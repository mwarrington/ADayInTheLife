using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class GameManager : MonoBehaviour
{
	static float Timer = 360;
	public bool GameTimerActive;

	public bool Test = false;

	void Start ()
	{

	}

	void Update ()
	{
		Debug.Log (Timer);
		if(GameTimerActive)
		{
			Timer -= Time.deltaTime;
		}
		if(Timer <= 0)
		{
			Application.LoadLevel("DreamSpiral");
			Timer = 360;
		}
	}
}
