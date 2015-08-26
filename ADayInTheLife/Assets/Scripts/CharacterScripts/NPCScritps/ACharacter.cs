using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class ACharacter : NPCScript
{
    //This indicates which convo the A level character should load
	public int DialogIndex;
    //Indicates whether this AlvlChar is a member of the team
	public bool IsTeamMember;

    //The following six methods don't need unique lines from NPCScripts
	protected override void Start ()
	{
		base.Start ();
	}

	protected override void Update ()
	{
		base.Update ();
	}

	protected override void OnConversationStart (Transform actor)
	{
		base.OnConversationStart (actor);
	}

	protected override void OnConversationLine (Subtitle line)
	{
		base.OnConversationLine (line);
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
        //If the A lvl Char is a team member then it will use the player's name to find which convo to load
		if(IsTeamMember)
			dialogString = this.name + "_" + DialogIndex.ToString();
		else //Else it will use a number set in the inspector
			dialogString = DialogIndex.ToString();

		base.DialogSetup ();
	}
}