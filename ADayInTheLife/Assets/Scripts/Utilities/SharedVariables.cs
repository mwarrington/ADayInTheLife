using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class SharedVariables : MonoBehaviour
{
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
}
