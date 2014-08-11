using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class BCCharacter : NPCScript
{
	public GameObject MyThoughtCloud;

	private Thoughts _myThoughts;
	private SpriteRenderer _thoughtRenderer;
	private int _myProgress = 1;

	protected override void Start ()
	{
		base.Start ();
		_myThoughts = MyThoughtCloud.GetComponent<Thoughts>();
		_thoughtRenderer = MyThoughtCloud.GetComponent<SpriteRenderer>();
		Invoke("DialogSetup", 0.1f);
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
				_thoughtRenderer.sprite = _myThoughts.ThoughtImages[i];
				_thoughtRenderer.enabled = true;
				CancelInvoke("RemoveThoughtCloud");
				Invoke("RemoveThoughtCloud", 3);
			}
		}
	}
	
	private void RemoveThoughtCloud()
	{
		_thoughtRenderer.enabled = false;
	}
	
	protected override void OnConversationEnd (Transform actor)
	{
		base.OnConversationEnd (actor);
		DialogSetup ();
	}
	
	protected override void RotateTowardPlayer ()
	{
		base.RotateTowardPlayer ();
	}
	
	protected override void DialogSetup ()
	{
		//Finds the progress Var
		string progressVarName = "";
		for(int i = 0; i < MyDatabase.variables.Count; i++)
		{
			if(MyDatabase.variables[i].fields[2].value == "CharacterProgress")
				progressVarName = MyDatabase.variables[i].fields[0].value;
		}

		//Rests the TalkedTo vars
		if(_myProgress < DialogueLua.GetVariable(progressVarName).AsInt)
		{
			DialogueLua.SetVariable ("TalkedTo" + this.name, false);
			_myProgress = DialogueLua.GetVariable(progressVarName).AsInt;
		}

		//Sets the Conversation to load
		if(PlayerSpacificDialog && myGameManager.IsSarylyn)
			dialogString = this.name.ToString() + "_" + DialogueLua.GetVariable(progressVarName).AsInt + "_Sarylyn";
		else if(PlayerSpacificDialog && !myGameManager.IsSarylyn)
			dialogString = this.name.ToString() + "_" + DialogueLua.GetVariable(progressVarName).AsInt + "_Sanome";
		else
			dialogString = this.name.ToString() + "_" + DialogueLua.GetVariable(progressVarName).AsInt;

		base.DialogSetup ();
	}
}
