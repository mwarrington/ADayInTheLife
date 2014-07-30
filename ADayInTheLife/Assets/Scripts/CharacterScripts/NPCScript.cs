using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class NPCScript : MonoBehaviour
{
	public Camera CloseUpCamera;
	public DialogueDatabase MyDatabase;
	public ConversationTrigger MyConTrigger;
	public GameObject MyThoughtCloud;
	public GameManager MyGameManager;
	public SpriteRenderer CurrentThoughtImage;
	public Thoughts MyThoughts;
	public NPCType MyType;
	public bool AlwaysFacePlayer,
				HasSharedVariables,
				HasThoughts,
				PlayerSpacificDialog;
	public string DialogString;
	public int DialogIndex;

	private GameObject _player;
	private Vector3 _orriginalRotation;
	private AudioSource _myVoice;

	void Start ()
	{
		DialogSetup();

		MyConTrigger.conversation = DialogString;
		_player = GameObject.FindGameObjectWithTag("Player");
		_orriginalRotation = this.transform.rotation.eulerAngles;
		_myVoice = this.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(AlwaysFacePlayer)
			RotateTowardPlayer();
	}

	void OnConversationStart(Transform actor)
	{
		if(MyType != NPCType.ACharacter)
		{
			CloseUpCamera.enabled = true;

			if(_myVoice != null)
				_myVoice.Play();
		}
	}

	void OnConversationEnd(Transform actor)
	{
		if(MyType != NPCType.ACharacter)
			CloseUpCamera.enabled = false;
		if(MyType == NPCType.HallMonitor)
		{
			if(MyConTrigger.trigger == DialogueTriggerEvent.OnStart)
			{
				MyConTrigger.trigger = DialogueTriggerEvent.OnUse;
				MyGameManager.GameTimerActive = true;
			}
			DialogString = "Hall_Monitor_" + Random.Range(1,MyDatabase.conversations.Count+1).ToString();
			MyConTrigger.conversation = DialogString;
		}
		if(HasSharedVariables)
		{
			GameObject.FindGameObjectWithTag("GameManager").GetComponent<SharedVariables>().SyncVariables(DialogString);
		}
	}

	void OnConversationLine(Subtitle line)
	{
		if (HasThoughts)
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

	private void RemoveThoughtCloud()
	{
		MyThoughtCloud.GetComponent<SpriteRenderer>().enabled = false;
		CurrentThoughtImage.enabled = false;
	}

	private void RotateTowardPlayer()
	{
		//This will make sprites turn to look at the player
		//this.transform.LookAt(_player.transform);
		//this.transform.rotation = Quaternion.Euler(new Vector3(0 + _orriginalRotation.x, this.transform.rotation.eulerAngles.y + _orriginalRotation.y, 0 + _orriginalRotation.z));

		//This will maintain rotation with the player
		this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, _player.transform.rotation, 100);
	}

	//This method sets up what conversation the NPC will speak.
	private void DialogSetup()
	{
		switch (MyType)
		{
			case NPCType.ACharacter:
				DialogString = DialogIndex.ToString();
				break;
			case NPCType.BCCharacter:
				if(PlayerSpacificDialog && MyGameManager.IsSarylyn)
					DialogString = this.name.ToString() + "_" + MyGameManager.CurrentDay.ToString() + "_Sarylyn";
				else if(PlayerSpacificDialog && !MyGameManager.IsSarylyn)
					DialogString = this.name.ToString() + "_" + MyGameManager.CurrentDay.ToString() + "_Sanome";
				else
					DialogString = this.name.ToString() + "_" + MyGameManager.CurrentDay.ToString();
				break;
			case NPCType.HallMonitor:
				if(!MyGameManager.HasBeenIntroduced)
				{
					DialogString = "Hall_Monitor_Intro";
					MyGameManager.HasBeenIntroduced = true;
				}
				else
					MyConTrigger.trigger = DialogueTriggerEvent.OnUse;
				break;
			default:
				Debug.Log ("That NPC type doesn't exist...");
				break;
		}
	}
}
