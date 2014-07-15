using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class GameManager : MonoBehaviour
{
	static float Timer = 360;
	static int DayCount = 0;
	public bool GameTimerActive;

	void Start ()
	{

	}

	void Update ()
	{
		if(GameTimerActive)
		{
			Timer -= Time.deltaTime;
		}
		if(Timer <= 0)
		{
			Application.LoadLevel("DreamSpiral");
			DayCount++;
			Timer = 360;
		}
	}
}
