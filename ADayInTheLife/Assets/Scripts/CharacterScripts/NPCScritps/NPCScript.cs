using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class NPCScript : MonoBehaviour
{
	public Camera CloseUpCamera;
	public DialogueDatabase MyDatabase;
	public SpriteRenderer CurrentThoughtImage;
	public NPCType MyType;
	public bool AlwaysFacePlayer,
				HasSharedVariables,
				HasThoughts,
				PlayerSpacificDialog;
	public string DialogString;
	public int DialogIndex;

	protected GameObject player;
	protected Vector3 orriginalRotation;
	protected AudioSource myVoice;
	protected GameManager myGameManager;
	protected ConversationTrigger myConTrigger;

	//Temporary HACK
	public GameObject MyThoughtCloud;
	public Thoughts MyThoughts;

	protected virtual void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		orriginalRotation = this.transform.rotation.eulerAngles;
		myVoice = this.GetComponent<AudioSource>();
		myGameManager = GameObject.FindObjectOfType<GameManager>();
		myConTrigger = this.GetComponent<ConversationTrigger>();
		DialogSetup();
		myConTrigger.conversation = DialogString;
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

			if(myVoice != null)
				myVoice.Play();
		}
	}

	protected virtual void OnConversationEnd(Transform actor)
	{
		if(MyType != NPCType.ACharacter)
			CloseUpCamera.enabled = false;
		if(MyType == NPCType.HallMonitor)
		{
			if(myConTrigger.trigger == DialogueTriggerEvent.OnStart)
			{
				myConTrigger.trigger = DialogueTriggerEvent.OnUse;
				myGameManager.GameTimerActive = true;
				myGameManager.HasBeenIntroduced = true;
			}
			DialogString = "Hall_Monitor_" + Random.Range(1,MyDatabase.conversations.Count+1).ToString();
			myConTrigger.conversation = DialogString;
		}
		if(HasSharedVariables)
		{
			GameObject.FindGameObjectWithTag("GameManager").GetComponent<SharedVariables>().SyncVariables(DialogString);
		}
	}

	//For now only BCCharacters use this function
	protected virtual void OnConversationLine(Subtitle line)
	{
		//Temporary HACK
		if(HasThoughts)
		{
			for(int i = 0; i < MyThoughts.PlayerDialogueLines.Length; i++)
			{
				if(MyThoughts.PlayerDialogueLines[i] == line.dialogueEntry.fields[6].value)
				{
					MyThoughtCloud.GetComponent<SpriteRenderer>().enabled = true;
					CurrentThoughtImage.sprite = MyThoughts.ThoughtImages[i];
					CurrentThoughtImage.enabled = true;
					CancelInvoke("RemoveThoughtCloud");
					Invoke("RemoveThoughtCloud", 3);
				}
			}
		}
	}

	//Temporary HACK
	private void RemoveThoughtCloud()
	{
		MyThoughtCloud.GetComponent<SpriteRenderer>().enabled = false;
		CurrentThoughtImage.enabled = false;
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
		switch (MyType)
		{
			case NPCType.ACharacter:
				DialogString = DialogIndex.ToString();
				break;
			case NPCType.BCCharacter:
				if(PlayerSpacificDialog && myGameManager.IsSarylyn)
					DialogString = this.name.ToString() + "_" + myGameManager.CurrentDay.ToString() + "_Sarylyn";
				else if(PlayerSpacificDialog && !myGameManager.IsSarylyn)
					DialogString = this.name.ToString() + "_" + myGameManager.CurrentDay.ToString() + "_Sanome";
				else
					DialogString = this.name.ToString() + "_" + myGameManager.CurrentDay.ToString();
				break;
			case NPCType.HallMonitor:
				if(!myGameManager.HasBeenIntroduced)
				{
					Debug.Log ("poop");
					DialogString = "Hall_Monitor_Intro";
				}
				else
					myConTrigger.trigger = DialogueTriggerEvent.OnUse;
				break;
			default:
				Debug.Log ("That NPC type doesn't exist...");
				break;
		}
	}
}
