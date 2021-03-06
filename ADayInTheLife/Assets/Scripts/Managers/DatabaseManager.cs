﻿using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class DatabaseManager : MonoBehaviour
{
	public DialogueDatabase MasterDatabase;
	public DialogueDatabase MyDatabase;

	private static bool _databaseLoaded = false;

	void Awake()
	{

	}

	void Start ()
	{
		if(!_databaseLoaded)
		{
			MyDatabase = (DialogueDatabase)DialogueDatabase.Instantiate (MasterDatabase);// = MyDatabase;
			_databaseLoaded = true;
			MyDatabase.actors[1].Name = "Clarissa";
		}
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
}
