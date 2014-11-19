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
                    //Hack: This is made specifically for the Markeshia Demo.
                    //if(DialogueLua.GetVariable("GlassesProgress").AsInt > progress)
                    //{
                    //    _currentEmpathyType = EmpathyTypes.Community;
                    //}
                    //else
                    //if(DialogueLua.GetVariable("ShawnProgress").AsInt > progress)
                    //{
                    //    _currentEmpathyType = EmpathyTypes.Self;
                    //}
                    //else if(DialogueLua.GetVariable("MarkeshiaProgress").AsInt > progress)
                    //{
                    //    _currentEmpathyType = EmpathyTypes.Another;
                    //}
                    if (DialogueLua.GetVariable("KnowsAboutDali_MarkeshiaHeart").AsBool)
                        progress = 3;
                    else if (DialogueLua.GetVariable("MarkeshiaProgress").AsInt == 1)
                        progress = 1;
                    else if (DialogueLua.GetVariable("MarkeshiaProgress").AsInt == 2)
                        progress = 2;

                    _currentEmpathyType = EmpathyTypes.Another;
					break;
				case 2:
                    if (DialogueLua.GetVariable("GonzoProgress").AsInt == 3 || DialogueLua.GetVariable("RobynProgress").AsInt == 2)
                    {
                        _currentEmpathyType = EmpathyTypes.Special;
                    }
                    else if (DialogueLua.GetVariable("GonzoProgress").AsInt > progress || DialogueLua.GetVariable("RobynProgress").AsInt > progress)
                    {
                        _currentEmpathyType = EmpathyTypes.Community;
                    }
                    else if (DialogueLua.GetVariable("ShawnProgress").AsInt > progress)
                    {
                        _currentEmpathyType = EmpathyTypes.Self;
                    }
                    else if (DialogueLua.GetVariable("MarkeshiaProgress").AsInt > progress)
                    {
                        _currentEmpathyType = EmpathyTypes.Another;
                    }
					break;
				case 3:
					Debug.Log ("NothingYet");
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
    protected int progress
    {
        get
        {
            return _progress;
        }
        set
        {
            if (_progress != value)
                DialogueLua.SetVariable("Another", 1);

            _progress = value;
        }
    }
    private int _progress = 0;

    public string SpecialConvoModifier;

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
            case EmpathyTypes.Special:
                _convoModifier = SpecialConvoModifier;
                break;
			default:
				Debug.Log ("Listen pal, there are only three types of empathy that we're exploring in this game. How did you even end up here?");
				break;
		}

		DialogSetup();
	}

	protected override void DialogSetup ()
	{
        if (DialogueLua.GetVariable("ShowHint").AsBool)
            dialogString = "Sam" + "_" + _convoModifier + "_" + progress;
        else
            dialogString = "Sam_Intro";

		base.DialogSetup ();
	}
}