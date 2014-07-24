using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	//Static fields with properties that have get and set accessors
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
	static bool IsSarylyn = true;
	public bool isSarylyn
	{
		get
		{
			return IsSarylyn;
		}
		set
		{
			IsSarylyn = value;
		}
	}
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
	static string lastLevelLoaded = "DreamSpiral";
	public string LastLevelLoaded
	{
		get
		{
			return lastLevelLoaded;
		}
		set
		{
			lastLevelLoaded = value;
		}
	}

	//Simple Static fields
	static int DayCount = 0;
	static bool databasesLoadedForHallway = false;
	static bool databasesLoadedForLabrary = false;

	//Public fields
	public bool GameTimerActive;
	public AudioSource Countdown30,
					   Countdown10;

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

		DayEnd();

		if(Input.GetKeyDown(KeyCode.F))
		{
			Screen.fullScreen = !Screen.fullScreen;
		}
	}

	//This method handles the strange things that happen at the end of a day.
	private void DayEnd()
	{
		switch(Application.loadedLevelName)
		{
			case "Hallway":
				if (timer <= 30 && !this.Countdown30.isPlaying)
				{
					GameObject[]_lockerSearch = GameObject.FindGameObjectsWithTag("locker30");    
					foreach(GameObject _locker in _lockerSearch)
					{
						_locker.collider.enabled = true;
						_locker.rigidbody.AddForce(30, 30, 30);
					}
					
					this.Countdown30.Play();
				}
				if (timer <= 10 && !this.Countdown10.isPlaying)
				{
					this.Countdown10.Play();
				}
				break;
			default:
				Debug.Log("That level doesn't exist...");
				break;
		}
		if(timer <= 0)
		{
			DialogueManager.StopConversation();
			Application.LoadLevel("DreamSpiral");
			DayCount++;
			timer = 360;
		}
	}
}
