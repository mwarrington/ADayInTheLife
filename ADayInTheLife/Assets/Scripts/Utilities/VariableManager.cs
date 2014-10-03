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
		SetWorldVarToLuaVar();
	}

	//This method will set variables in databases that share variables
    public void SyncVariables(string CurrentConvo)
    {
        switch (CurrentConvo)
        {
            case "Darrell_1":
                DialogueLua.SetVariable("KnowsAboutRumor_Jack", DialogueLua.GetVariable("KnowsAboutRumor").AsBool);
                DialogueLua.SetVariable("KnowsAboutRumor_Glasses", DialogueLua.GetVariable("KnowsAboutRumor").AsBool);
                DialogueLua.SetVariable("KnowsAboutPicture_Jack", DialogueLua.GetVariable("KnowsAboutPicture").AsBool);
                DialogueLua.SetVariable("KnowsAboutPicture_Glasses", DialogueLua.GetVariable("KnowsAboutPicture").AsBool);
                break;
            case "Jack_1":
                DialogueLua.SetVariable("JackWantsHelp_Darrell", DialogueLua.GetVariable("JackWantsHelp").AsBool);
                DialogueLua.SetVariable("JackWantsHelp_Glasses", DialogueLua.GetVariable("JackWantsHelp").AsBool);
                break;
            case "Markeshia_2":
                DialogueLua.SetVariable("Dali_Phone", DialogueLua.GetVariable("Dali").AsBool);
                break;
            default:
                Debug.Log("That isn't the correct conversation name");
                break;
        }
        //DialogueLua.GetVariable("Helped Gonzo").AsBool);
    }

	public void ProgressSync(string myName, List<string> namesToSync)
	{
		for(int i = 0; i < namesToSync.Count; i++)
		{
			DialogueLua.SetVariable(namesToSync[i] + "Progress", DialogueLua.GetVariable(myName + "Progress").AsInt);
		}
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
			if(DialogueLua.GetVariable(_eventVariables[i]).luaValue.GetType().ToString() == "Language.Lua.LuaBoolean")
				DialogueLua.SetVariable(_eventVariables[i], false);
			else
				DialogueLua.SetVariable(_eventVariables[i], 0);
		}
	}

	public void SetWorldVarToLuaVar()
	{
		if (Application.loadedLevelName == "Classroom")
		{
			if(DialogueLua.GetVariable("ShawnProgress").AsInt == 4)
			{
				DialogueLua.SetVariable("CompletedFavor", true);
			}
		}
	}
}
