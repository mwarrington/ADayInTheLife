using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class VariableManager : MonoBehaviour
{
	private List<string> _eventVariables = new List<string>();
	private DialogueDatabase _myMasterDatabase;

	void Start()
	{
		_myMasterDatabase = DialogueManager.MasterDatabase;
	}

	//This method will set variables in databases that share variables
	public void SyncVariables(string CurrentConvo)
	{
		switch(CurrentConvo)
		{
			case "Darrell_day1":
				DialogueLua.SetVariable("darrell",DialogueLua.GetVariable("pre_darrell").AsBool);
				DialogueLua.SetVariable("picture",DialogueLua.GetVariable("pre_picture").AsBool);
				DialogueLua.SetVariable("glasses_exists",DialogueLua.GetVariable("pre_glasses_exists").AsBool);
				break;
			default:
				Debug.Log("That isn't the correct conversation name");
				break;
		}
		//DialogueLua.GetVariable("Helped Gonzo").AsBool);
	}

	public void ResetEventVars()
	{
		_eventVariables.Clear();

		for(int i = 0; i < _myMasterDatabase.variables.Count; i++)
		{
			if(_myMasterDatabase.variables[i].fields[2].value == "EventVar")
			{
				_eventVariables.Add(_myMasterDatabase.variables[i].fields[0].value);
			}
		}

		for(int i = 0; i < _eventVariables.Count; i++)
		{
			DialogueLua.SetVariable(_eventVariables[i], false);
		}
	}
}
