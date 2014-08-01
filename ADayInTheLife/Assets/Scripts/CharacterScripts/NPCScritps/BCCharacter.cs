using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class BCCharacter : NPCScript
{


	protected override void Start ()
	{
		base.Start ();
	}
	
	protected override void Update ()
	{
		base.Update ();
	}
	
	protected override void OnConversationStart (Transform actor)
	{
		base.OnConversationStart (actor);
	}
	
	protected override void OnConversationLine (PixelCrushers.DialogueSystem.Subtitle line)
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
	
	
	private void RemoveThoughtCloud()
	{
		MyThoughtCloud.GetComponent<SpriteRenderer>().enabled = false;
		CurrentThoughtImage.enabled = false;
	}
	
	protected override void OnConversationEnd (Transform actor)
	{
		base.OnConversationEnd (actor);
	}
	
	protected override void RotateTowardPlayer ()
	{
		base.RotateTowardPlayer ();
	}
	
	protected override void DialogSetup ()
	{
		base.DialogSetup ();
	}
}
