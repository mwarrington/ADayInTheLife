﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class NPCScript : MonoBehaviour
{
	public Camera CloseUpCamera;
	public DialogueDatabase MyDatabase;
	public NPCType MyType;
	public bool AlwaysFacePlayer,
				HasSharedVariables,
				PlayerSpacificDialog;

	protected GameObject player;
	protected Vector3 orriginalRotation;
	protected AudioSource myVoice;
	protected GameManager myGameManager;
	protected ConversationTrigger myConTrigger;
	protected string dialogString;

	protected virtual void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		orriginalRotation = this.transform.rotation.eulerAngles;
		myVoice = this.GetComponent<AudioSource>();
		myGameManager = GameObject.FindObjectOfType<GameManager>();
		myConTrigger = this.GetComponent<ConversationTrigger>();
	}
	
	// Update is called once per frame
	protected virtual void Update ()
	{
		if(AlwaysFacePlayer)
			RotateTowardPlayer();
	}

	protected virtual void OnConversationStart(Transform actor)
	{
		if(MyType != NPCType.ACharacter)
		{
			CloseUpCamera.enabled = true;
			myGameManager.MainCamera.enabled = false;

			if(myVoice != null)
				myVoice.Play();
		}
	}

	protected virtual void OnConversationLine(Subtitle line)
	{

	}

	protected virtual void OnConversationEnd(Transform actor)
	{
		if(MyType != NPCType.ACharacter)
		{
			CloseUpCamera.enabled = false;
			myGameManager.MainCamera.enabled = true;
		}

		if(HasSharedVariables)
		{
			GameObject.FindGameObjectWithTag("GameManager").GetComponent<VariableManager>().SyncVariables(dialogString);
		}
	}

	protected virtual void RotateTowardPlayer()
	{
		//This will make sprites turn to look at the player
		//this.transform.LookAt(_player.transform);
		//this.transform.rotation = Quaternion.Euler(new Vector3(0 + _orriginalRotation.x, this.transform.rotation.eulerAngles.y + _orriginalRotation.y, 0 + _orriginalRotation.z));

		//This will maintain rotation with the player
		this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, player.transform.rotation, 100);
	}

	//This method sets up what conversation the NPC will speak.
	protected virtual void DialogSetup()
	{
		myConTrigger.conversation = dialogString;
	}
}