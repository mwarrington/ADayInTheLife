using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class GameManager : MonoBehaviour
{
	static float Timer = 360;
	static int DayCount = 0;
	public static bool IsSarylyn = true;
	public bool isSarylyn
	{
		get
		{
			return IsSarylyn;
		}
		set
		{
			Debug.Log("You shouldn't be trying to set this property.");
		}
	}
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

	public void SelectPlayer(bool value)
	{
		IsSarylyn = value;
	}
}
