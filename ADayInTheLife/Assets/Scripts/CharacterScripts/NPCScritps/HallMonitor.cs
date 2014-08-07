﻿using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class HallMonitor : NPCScript
{
	protected override void Start ()
	{
		base.Start ();
		DialogSetup();
	}
	
	protected override void Update ()
	{
		base.Update ();
	}
	
	protected override void OnConversationStart (Transform actor)
	{
		base.OnConversationStart (actor);
	}
	
	//For now only BCCharacters use this function
	protected override void OnConversationLine (Subtitle line)
	{
		base.OnConversationLine (line);
	}
	
	protected override void OnConversationEnd (Transform actor)
	{
		if(myConTrigger.trigger == DialogueTriggerEvent.OnStart)
		{
			myConTrigger.trigger = DialogueTriggerEvent.OnUse;
			myGameManager.GameTimerActive = true;
			myGameManager.HasBeenIntroduced = true;
		}
		dialogString = "Hall_Monitor_" + Random.Range(1,MyDatabase.conversations.Count+1).ToString();
		myConTrigger.conversation = dialogString;

		base.OnConversationEnd (actor);
	}
	
	protected override void RotateTowardPlayer ()
	{
		base.RotateTowardPlayer ();
	}
	
	protected override void DialogSetup ()
	{
		if(!myGameManager.HasBeenIntroduced)
			dialogString = "Hall_Monitor_Intro";
		else
			myConTrigger.trigger = DialogueTriggerEvent.OnUse;

		base.DialogSetup ();
	}

	private void DayEndBehavior()
	{

	}
}