using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class BCCharacter : NPCScript
{
	public GameObject MyThoughtCloud,
					  MySpotLight;
	public bool CCharacter;

	private GameObject _dLights;
	private EmpathicEmoticons _myEmpathicEmoticons;
	private SpriteRenderer _emoticonRenderer;
	private Subtitle _currentLine;
	private List<Subtitle> _viewedLines = new List<Subtitle>();
	private string _currentTopic;
	private float _timeSpentOnLine;
	private bool _conversationActive;

	protected override void Start ()
	{
		base.Start ();
		_myEmpathicEmoticons = myGameManager.GetComponent<EmpathicEmoticons>();
		_emoticonRenderer = MyThoughtCloud.GetComponent<SpriteRenderer>();
		_dLights = GameObject.FindGameObjectWithTag("D-Lights");
		Invoke("DialogSetup", 0.1f);
	}
	
	protected override void Update ()
	{
		base.Update ();

		if(_conversationActive)
			LineTimer();
	}
	
	protected override void OnConversationStart (Transform actor)
	{
		base.OnConversationStart (actor);
		_conversationActive = true;

		if(CCharacter)
			ToggleSpotLight();
	}

	protected override void OnConversationLine (Subtitle line)
	{
		base.OnConversationLine(line);

		LineManager (line);

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
		_conversationActive = false;
		DialogSetup ();
		RemoveThoughtCloud();

		if(CCharacter)
			ToggleSpotLight();
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

		//Resets the TalkedTo vars for test purposes
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

	private void ToggleSpotLight()
	{
		_dLights.SetActive(!_dLights.activeSelf);
		MySpotLight.SetActive(!MySpotLight.activeSelf);
	}

	private void LineTimer()
	{
		_timeSpentOnLine += Time.deltaTime;
		DialogueLua.SetVariable (this.name + "Timer", (int)_timeSpentOnLine);
	}

	private void LineManager(Subtitle myLine)
	{
		//This sets the current line if it is initally null
		if(_currentLine == null)
			_currentLine = myLine;

		//This keeps track of the dialog lines that have been seen
		foreach(Subtitle s in _viewedLines)
		{
			if(myLine == s)
			{
				_timeSpentOnLine = 50;
				return;
			}
		}
		_viewedLines.Add (myLine);

		//This first checks to see if the dialog line has "Topic" in the description
		//It then checks to see if the topic is the same as it's been if not, it resets the timer
		if (myLine.dialogueEntry.fields[2].value != "")
		{
			if(myLine.dialogueEntry.fields[2].value != _currentTopic)
			{
				_currentTopic = myLine.dialogueEntry.fields[2].value;
				_currentLine = myLine;
				_timeSpentOnLine = 0;
				return;
			}
			else
				return;
		}

		//This checks to see if the line is the same as the last
		if(_currentLine != myLine)// && myLine)
		{
			_currentLine = myLine;
			_timeSpentOnLine = 0;
		}
	}
}
