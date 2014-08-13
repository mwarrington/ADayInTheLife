using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class BCCharacter : NPCScript
{
	public GameObject MyThoughtCloud;

	private EmpathicEmoticons _myEmpathicEmoticons;
	private SpriteRenderer _emoticonRenderer;
	private int _myProgress = 1;

	protected override void Start ()
	{
		base.Start ();
		_myEmpathicEmoticons = myGameManager.GetComponent<EmpathicEmoticons>();
		_emoticonRenderer = MyThoughtCloud.GetComponent<SpriteRenderer>();
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

		if(line.dialogueEntry.fields.Count > 12)
		{
			if(line.dialogueEntry.fields[12].value != "")
			{
				_emoticonRenderer.sprite = _myEmpathicEmoticons.SpriteDictionary[line.dialogueEntry.fields[12].value];
				_emoticonRenderer.enabled = true;
				CancelInvoke("RemoveThoughtCloud");
				Invoke("RemoveThoughtCloud", 2);
			}
		}
	}
	
	private void RemoveThoughtCloud()
	{
		_emoticonRenderer.enabled = false;
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

		//Rests the TalkedTo vars for test purposes
		//This alows you to complete a level in a single day
		//if(_myProgress < DialogueLua.GetVariable(progressVarName).AsInt)
		//{
		//	DialogueLua.SetVariable ("TalkedTo" + this.name, false);
		//	_myProgress = DialogueLua.GetVariable(progressVarName).AsInt;
		//}

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
