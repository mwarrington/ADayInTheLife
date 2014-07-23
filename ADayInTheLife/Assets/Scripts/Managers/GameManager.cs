﻿using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class GameManager : MonoBehaviour
{
	static float timer = 360;
	public float Timer
	{
		get
		{
			return timer;
		}
		set
		{
			Debug.Log ("Who told you that you could set this property!?");
		}
	}
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
	static bool databasesLoadedForHallway = false;
	static bool databasesLoadedForLabrary = false;
	static Days currentDay;
	public Days CurrentDay
	{
		get
		{
			return currentDay;
		}
		set
		{
			currentDay = value;
		}
	}

	void Start ()
	{
		if(Application.loadedLevelName == "Hallway" && !databasesLoadedForHallway)
		{
			foreach(NPCScript n in GameObject.FindObjectsOfType(typeof(NPCScript)))
			{
				DialogueManager.AddDatabase(n.MyDatabase);
			}
			databasesLoadedForHallway = true;
		}
		else if(Application.loadedLevelName == "Labrary" && !databasesLoadedForLabrary)
		{
			foreach(NPCScript n in GameObject.FindObjectsOfType(typeof(NPCScript)))
			{
				DialogueManager.AddDatabase(n.MyDatabase);
			}
			databasesLoadedForLabrary = true;
		}
	}

	void Update ()
	{
		if(GameTimerActive)
		{
			timer -= Time.deltaTime;
		}
		if(timer <= 0)
		{
			DialogueManager.StopConversation();
			Application.LoadLevel("DreamSpiral");
			DayCount++;
			timer = 360;
		}
		if(Input.GetKeyDown(KeyCode.F))
		{
			Screen.fullScreen = !Screen.fullScreen;
		}
	}

	public void SelectPlayer(bool value)
	{
		IsSarylyn = value;
	}
}
