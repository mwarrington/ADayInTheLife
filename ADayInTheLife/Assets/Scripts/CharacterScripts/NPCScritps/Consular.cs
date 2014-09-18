using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class Consular : NPCScript
{
	private string _convoModifier = "Intro";

	protected EmpathyTypes currentEmpathyType
	{
		get
		{
			switch (myGameManager.LevelCount)
			{
				case 1:
					int progress = 0;
					
					if(DialogueLua.GetVariable("GlassesProgress").AsInt > progress)
					{
						_currentEmpathyType = EmpathyTypes.Community;
					}
					else if(DialogueLua.GetVariable("ShawnProgress").AsInt > progress)
					{
						_currentEmpathyType = EmpathyTypes.Self;
					}
					else if(DialogueLua.GetVariable("MarkeshiaProgress").AsInt > progress)
					{
						_currentEmpathyType = EmpathyTypes.Another;
					}
					break;
				case 2:
					Debug.Log ("Poop");
					break;
				case 3:
					Debug.Log ("Poop");
					break;
				default:
					Debug.Log ("That level doesn't even exist...");
					break;
			}

			return _currentEmpathyType;
		}
		set
		{
			_currentEmpathyType = value;
		}
	}
	private EmpathyTypes _currentEmpathyType;

	protected override void Start ()
	{
		base.Start ();
	}

	protected override void Update ()
	{
		base.Update ();
	}

	protected override void OnConversationEnd (Transform actor)
	{
		base.OnConversationEnd (actor);

		switch(currentEmpathyType)
		{
			case EmpathyTypes.Community:
				_convoModifier = "Community";
				break;
			case EmpathyTypes.Self:
				_convoModifier = "Self";
				break;
			case EmpathyTypes.Another:
				_convoModifier = "Another";
				break;
			default:
				Debug.Log ("Listen pal, there are only three types of empathy that we're exploring in this game. How did you even end up here?");
				break;
		}

		DialogSetup();
	}

	protected override void DialogSetup ()
	{
		dialogString = "Sam_" + myGameManager.LevelCount + "_" + _convoModifier;

		base.DialogSetup ();
	}
}