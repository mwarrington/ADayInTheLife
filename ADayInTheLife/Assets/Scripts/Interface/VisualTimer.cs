using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class VisualTimer : MonoBehaviour
{
	protected bool showingGameTimer
	{
		get
		{
			if(_showingGameTimer)
			{
				if(DialogueManager.IsConversationActive)
					_showingGameTimer = false;
				else if(isItemActive)
					_showingGameTimer = false;
				else
				{
					foreach (SpriteRenderer sr in TimerCloud.GetComponentsInChildren<SpriteRenderer>())
					{
						sr.enabled = true;
					}
				}
			}
			else
			{
				foreach (SpriteRenderer sr in TimerCloud.GetComponentsInChildren<SpriteRenderer>())
				{
					sr.enabled = false;
				}
			}

			return _showingGameTimer;
		}

		set
		{
			_showingGameTimer = value;
		}
	}
	private bool _showingGameTimer = false;

	protected bool isItemActive
	{
		get
		{
			foreach (ItemInteract ii in GameObject.FindObjectsOfType<ItemInteract>())
			{
				if(ii.ItemActive)
				{
					_isItemActive = true;
					return _isItemActive;
				}
				else
					_isItemActive = false;
			}
			return _isItemActive;
		}
	}
	private bool _isItemActive;

	private GameManager _myGameManager;
	private int _gameTimer;

	public GUIStyle Font;
	public GameObject TimerCloud;

	void Start()
	{
		_myGameManager = GameObject.FindObjectOfType<GameManager>();

		//This handles turning off the the Timer Cloud renderers
		if(_myGameManager.Timer % 30 > 1)
		{
			showingGameTimer = false;
		}
	}

	void OnGUI()
	{
		if(showingGameTimer)
		{
			GameTimer();
		}
	}

	void Update()
	{
		//This handles when to turn on the Timer Cloud renderers
		if(Application.loadedLevelName == "Labrary" || Application.loadedLevelName == "Classroom" || Application.loadedLevelName == "Roomclass")
		{
			if(_myGameManager.Timer < 61)
			{
				showingGameTimer = true;
			}
			else if(_myGameManager.Timer % 30 < 1)
			{
				if(!showingGameTimer)
				{
					_myGameManager.GameTimerActive = true;
					showingGameTimer = true;
				}
			}
			else if(Input.GetKey(KeyCode.LeftShift))
			{
				showingGameTimer = true;
			}
			else if(_myGameManager.Timer % 30 < 25 && showingGameTimer)
			{
				showingGameTimer = false;
			}
		}
		else
		{
			if(_myGameManager.Timer < 61)
			{
				showingGameTimer = true;
			}
			else if(_myGameManager.Timer % 30 < 1 && _myGameManager.HasBeenIntroduced == true)
			{
				if(!showingGameTimer)
				{
					_myGameManager.GameTimerActive = true;
					showingGameTimer = true;
				}
			}
			else if(Input.GetKey(KeyCode.LeftShift))
			{
				showingGameTimer = true;
			}
			else if(_myGameManager.Timer % 30 < 25 && showingGameTimer)
			{
				showingGameTimer = false;
			}
		}

		/*
		else if(_myGameManager.Timer % 30 < 25 && showingGameTimer)
		{
			showingGameTimer = false;
		}*/
	}
	
	private void GameTimer()
	{
		_gameTimer = (int)_myGameManager.Timer;
		int displaySeconds = Mathf.CeilToInt(_gameTimer) % 60;
		int displayMinutes = Mathf.CeilToInt(_gameTimer) / 60;
		string text = string.Format ("{0:00}:{1:00}", displayMinutes, displaySeconds);
		
		if (Application.loadedLevelName == "Hallway")
			GUI.Box(new Rect(Screen.width * 0.14f, Screen.height * 0.4f, Screen.width * 0.15f, Screen.height * 0.15f), text, Font);
		else if (Application.loadedLevelName == "Labrary")
			GUI.Box(new Rect(Screen.width * 0.14f, Screen.height * 0.4f, Screen.width * 0.15f, Screen.height * 0.15f), text, Font);
		else if (Application.loadedLevelName == "Classroom" || Application.loadedLevelName == "Roomclass")
			GUI.Box(new Rect(Screen.width * 0.07f, Screen.height * 0.367f, Screen.width * 0.15f, Screen.height * 0.15f), text, Font);
	}
}
