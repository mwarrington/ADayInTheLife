using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class BCCharacter : NPCScript
{
	public GameObject MyThoughtCloud;
	public SpriteRenderer CurrentThoughtImage;
	private Thoughts _myThoughts;

	protected override void Start ()
	{
		base.Start ();
		_myThoughts = MyThoughtCloud.GetComponent<Thoughts>();
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
		base.OnConversationLine(line);

		for(int i = 0; i < _myThoughts.PlayerDialogueLines.Length; i++)
		{
			if(_myThoughts.PlayerDialogueLines[i] == line.dialogueEntry.fields[6].value)
			{
				MyThoughtCloud.GetComponent<SpriteRenderer>().enabled = true;
				CurrentThoughtImage.sprite = _myThoughts.ThoughtImages[i];
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

		if(PlayerSpacificDialog && myGameManager.IsSarylyn)
			dialogString = this.name.ToString() + "_" + myGameManager.CurrentDay.ToString() + "_Sarylyn";
		else if(PlayerSpacificDialog && !myGameManager.IsSarylyn)
			dialogString = this.name.ToString() + "_" + myGameManager.CurrentDay.ToString() + "_Sanome";
		else
			dialogString = this.name.ToString() + "_" + myGameManager.CurrentDay.ToString();
	}
}
