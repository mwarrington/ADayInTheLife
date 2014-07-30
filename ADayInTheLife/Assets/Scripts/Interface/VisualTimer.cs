using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class VisualTimer : MonoBehaviour
{
	private bool _showingGameTimer = false;
	private int _gameTimer;

	public GUIStyle Font;
	public GameObject TimerCloud;

	void Start()
	{
		if(Application.loadedLevelName == "Labrary" || Application.loadedLevelName == "Classroom" || Application.loadedLevelName == "Roomclass")
			GameObject.Find("GameManager").GetComponent<GameManager>().GameTimerActive = true;
	}

	void OnGUI()
	{
		if(_showingGameTimer && !DialogueManager.IsConversationActive)
			GameTimer();
	}

	public void ShowGameTimer()
	{
		if(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().Timer > 61)
		{
			Invoke ("HideGameTimer", 5);
		}
		
		_showingGameTimer = true;
		TimerCloud.SetActive(true);
	}
	
	private void GameTimer()
	{
		_gameTimer = (int)GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().Timer;
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
	
	private void HideGameTimer()
	{
		_showingGameTimer = false;
		TimerCloud.SetActive(false);
		Invoke ("ShowGameTimer", 25);
	}
}
