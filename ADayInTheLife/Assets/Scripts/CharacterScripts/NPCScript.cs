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
	public bool AlwaysFacePlayer,
				HasSharedVariables,
				HasThoughts,
				CanTalk,
				PlayerSpacificDialog,
				ACharacter;
	public string DialogString;
	public int DialogIndex;

	private GameObject _player;
	private Vector3 _orriginalRotation;

	void Start ()
	{
		if(ACharacter)
		{
			DialogString = DialogIndex.ToString();
		}
		else if(CanTalk)
		{
			if(PlayerSpacificDialog && MyGameManager.isSarylyn)
				DialogString = this.name.ToString() + "_" + MyGameManager.CurrentDay.ToString() + "_Sarylyn";
			else if(PlayerSpacificDialog && !MyGameManager.isSarylyn)
				DialogString = this.name.ToString() + "_" + MyGameManager.CurrentDay.ToString() + "_Sanome";
			else
				DialogString = this.name.ToString() + "_" + MyGameManager.CurrentDay.ToString();
		}
		MyConTrigger.conversation = DialogString;
		_player = GameObject.FindGameObjectWithTag("Player");
		_orriginalRotation = this.transform.rotation.eulerAngles;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(AlwaysFacePlayer)
			RotateTowardPlayer();
	}

	void OnConversationStart(Transform actor)
	{
		if(!ACharacter)
			CloseUpCamera.enabled = true;
	}

	void OnConversationEnd(Transform actor)
	{
		if(!ACharacter)
			CloseUpCamera.enabled = false;
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
		this.transform.LookAt(_player.transform);
		this.transform.rotation = Quaternion.Euler(new Vector3(0 + _orriginalRotation.x, this.transform.rotation.eulerAngles.y + _orriginalRotation.y, 0 + _orriginalRotation.z));

		//This will maintain rotation with the player
		this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, _player.transform.rotation, 100);
	}
}
