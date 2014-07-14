using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	static float Timer = 360;
	private bool _gameTimerActive = false;

	public bool Test = false;

	void Start ()
	{
	}

	void Update ()
	{
		if(_gameTimerActive)
		{
			Timer -= Time.deltaTime;
		}
	}
}
