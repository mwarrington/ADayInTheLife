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
	public SpriteRenderer CurrentThoughtImage;
	public Thoughts MyThoughts;
	public bool AlwaysFacePlayer,
				HasSharedVariables;
	public string DialogString;

	private GameObject _player;
	private Vector3 _orriginalRotation;

	void Start ()
	{
		MyConTrigger.conversation = DialogString;

		_player = GameObject.Find("Player");
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
		CloseUpCamera.enabled = true;
	}

	void OnConversationEnd(Transform actor)
	{
		CloseUpCamera.enabled = false;
		if(HasSharedVariables)
		{
			GameObject.FindGameObjectWithTag("GameManager").GetComponent<SharedVariables>().SyncVariables(DialogString);
		}
	}

	/*void OnConversationLine(Subtitle line)
	{
		for(int i = 0; i < MyThoughts.PlayerDialogueLines.Length; i++)
		{
			if(MyThoughts.PlayerDialogueLines[i] == line.dialogueEntry.fields[6].value)
			{
				MyThoughtCloud.GetComponent<SpriteRenderer>().enabled = true;
				CurrentThoughtImage.sprite = MyThoughts.ThoughtImages[i];
				CurrentThoughtImage.enabled = true;
				Invoke("RemoveThoughtCloud", 3);
			}
		}
	}*/

	private void RemoveThoughtCloud()
	{
		MyThoughtCloud.GetComponent<SpriteRenderer>().enabled = false;
		CurrentThoughtImage.enabled = false;
	}

	private void RotateTowardPlayer()
	{
		this.transform.LookAt(_player.transform);
		Debug.Log (this.transform.rotation.eulerAngles.y + _orriginalRotation.y);
		this.transform.rotation = Quaternion.Euler(new Vector3(0 + _orriginalRotation.x, this.transform.rotation.eulerAngles.y + _orriginalRotation.y, 0 + _orriginalRotation.z));

		//This will maintain rotation with the player
		//this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, _player.transform.rotation, 100);
	}
}
