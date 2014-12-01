using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

//Much of this script is incomplete because the hint system has not been fully designed
//I will revisit this once we have finished designing how this will work
public class Consular : NPCScript
{
	private string _convoModifier = "Intro";

    //This property is the meat and potatoes of the script
    //It determins what dialog the Consular will say
	protected EmpathyTypes currentEmpathyType
	{
		get
        {
			switch (myGameManager.LevelCount)
			{
				case 1:
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
                    //Hack: This is made specifically for the Markeshia Demo.
                    //That being said, each empathy type will work much like this one does
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

    //Whenever the progress value is different the value of "Another"(there will be one for "Self" and "Community" in the future)
    //is reverted back to 1
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

    //If ever we need Sam to say a special line we'll use this string
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
        //ShowHint is a variable in Sam's DialogDatabase. When it's set to true the next dialog shown is as follows.
        if (DialogueLua.GetVariable("ShowHint").AsBool)
            dialogString = "Sam" + "_" + _convoModifier + "_" + progress;
        else
            dialogString = "Sam_Intro";

		base.DialogSetup ();
	}
}