﻿using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	//Static fields with properties that have get and set accessors
	static float timer = 10;
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
	static bool isSarylyn = true;
	public bool IsSarylyn
	{
		get
		{
			return isSarylyn;
		}
		set
		{
			isSarylyn = value;
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
	static bool hasBeenIntroduced = false;
	public bool HasBeenIntroduced
	{
		get
		{
			return hasBeenIntroduced;
		}
		set
		{
			hasBeenIntroduced = value;
		}
	}

	//private fields with properties that have get and set accessors
	static bool gameTimerActive;
	public bool GameTimerActive
	{
		get
		{
			return gameTimerActive;
		}
		set
		{
			gameTimerActive = value;
		}
	}

	//Simple Static fields
	static int DayCount = 0;
	static bool databasesLoadedForHallway = false;
	static bool databasesLoadedForLabrary = false;

	//Private fields
	private AudioSource _mainBGM;
	private float _alpha = 0;
	private SpriteRenderer _fadeMask;

	//Public fields
	public bool	FadingAway;
	public Camera MainCamera;
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
		_mainBGM = GameObject.FindGameObjectWithTag("MainBGM").GetComponent<AudioSource>();
		MainCamera = Camera.main;
	}

	void Update ()
	{
		if(GameTimerActive)
		{
			timer -= Time.deltaTime;
		}

		DayEnd();

		if(FadingAway)
			Fade();

		if(Input.GetKeyDown(KeyCode.F))
		{
			Screen.fullScreen = !Screen.fullScreen;
		}
	}

	//This method handles the strange things that happen at the end of a day.
	private void DayEnd()
	{
		if(timer <= 60 && _mainBGM.pitch > 0.9f)
		{
			_mainBGM.pitch = 0.9f;
		}
		if(timer <= 50 && !this.Countdown30.isPlaying)
		{
			_mainBGM.pitch = 0.85f;
			_mainBGM.volume = 0.7f;
			Countdown30.Play();
			Countdown30.volume = 0.3f;
		}
		if(timer <= 40 && _mainBGM.pitch > 0.8f)
		{
			_mainBGM.pitch = 0.8f;
			_mainBGM.volume = 0.65f;
			Countdown30.volume = 0.5f;
		}
		if(timer <= 20 && _mainBGM.pitch > 0.7f)
		{
			_mainBGM.pitch = 0.7f;
			_mainBGM.volume = 0.3f;
			Countdown30.volume = 0.75f;
		}
		if (timer <= 10 && !this.Countdown10.isPlaying)
		{
			_mainBGM.pitch = 0.5f;
			_mainBGM.volume = 0.25f;
			this.Countdown10.Play();
		}
		switch(Application.loadedLevelName)
		{
			case "Hallway":
				if (timer <= 30 && _mainBGM.pitch > 0.75f)
				{
					_mainBGM.pitch = 0.75f;
					_mainBGM.volume = 0.5f;
					Countdown30.volume = 0.6f;
					GameObject[]_lockerSearch = GameObject.FindGameObjectsWithTag("locker30");    
					foreach(GameObject _locker in _lockerSearch)
					{
						StartCoroutine(StaggeredAnimationPlay(_locker.transform.parent.GetComponent<Animation>()));
					}
					
					//GameObject[]_lockerSearch = GameObject.FindGameObjectsWithTag("locker30");    
					//foreach(GameObject _locker in _lockerSearch)
					//{
					//	_locker.collider.enabled = true;
					//	_locker.rigidbody.AddForce(10f, 10f, 10f);
					//}
				}
				break;
			case "Labrary":
				if (timer <= 30 && _mainBGM.pitch > 0.75f)
				{
					_mainBGM.pitch = 0.75f;
					_mainBGM.volume = 0.5f;
					Countdown30.volume = 0.6f;
					GameObject[]_computerSearch = GameObject.FindGameObjectsWithTag("computer60");    
					foreach(GameObject _computer in _computerSearch)
					{
						_computer.GetComponent<Animation>().Play();
					}
				}
				break;
			case "Classroom":
				if (timer <= 30 && _mainBGM.pitch > 0.75f)
				{
					_mainBGM.pitch = 0.75f;
					_mainBGM.volume = 0.5f;
					Countdown30.volume = 0.6f;
					GameObject[]_deskSearch = GameObject.FindGameObjectsWithTag("desk");    
					foreach(GameObject _desk in _deskSearch)
					{
						_desk.rigidbody.useGravity = true;
						_desk.rigidbody.isKinematic = false;
					}
				}
				break;
			case "Roomclass":
				if (timer <= 30 && _mainBGM.pitch > 0.75f)
				{
					_mainBGM.pitch = 0.75f;
					_mainBGM.volume = 0.5f;
					Countdown30.volume = 0.6f;
					GameObject[]_deskSearch = GameObject.FindGameObjectsWithTag("desk");    
					foreach(GameObject _desk in _deskSearch)
					{
						_desk.rigidbody.isKinematic = false;
						_desk.rigidbody.AddForce(0,100,0);
					}
				}
				break;
			default:
				//Nothing should happen here
				//Debug.Log("That level doesn't exist...");
				break;
		}
		if(timer <= 3)
		{
			Invoke("LoadNextLevel", 3);
			if(!FadingAway)
				_fadeMask = Camera.current.GetComponentInChildren<SpriteRenderer>();
			FadingAway = true;
		}
	}

	private void Fade()
	{
		_alpha += Time.deltaTime * 0.35f;
		_mainBGM.volume -= Time.deltaTime * 0.15f;
		_fadeMask.color = new Color(_fadeMask.color.r, _fadeMask.color.g, _fadeMask.color.b, _alpha);

	}

	private IEnumerator StaggeredAnimationPlay(Animation animation)
	{
		float randFloat = Random.Range(0, 0.7f);
		yield return new WaitForSeconds(randFloat);
		animation.Play();
	}

	private void LoadNextLevel()
	{
		FadingAway = false;
		DialogueManager.StopConversation();
		this.GetComponent<VariableManager>().ResetEventVars();
		Application.LoadLevel("DreamSpiral");
		DayCount++;
		gameTimerActive = false;
		timer = 360;
		hasBeenIntroduced = false;
	}
}
