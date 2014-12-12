using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class HallMonitor : NPCScript
{
    //This int just keeps track of whatever random int we last used for dialog set up
    private int _lastRand = 0;

	protected override void Start ()
	{
		base.Start ();
		DialogSetup();
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
        //Hall monitors start out as OnStart until the player has been introduced
        //After the player has been introduced once the Trigger event is changed back to OnUse
		if(myConTrigger.trigger == DialogueTriggerEvent.OnStart)
		{
			myConTrigger.trigger = DialogueTriggerEvent.OnUse;
			myGameManager.GameTimerActive = true;
			myGameManager.HasBeenIntroduced = true;
		}

        DialogSetup();

		base.OnConversationEnd (actor);
	}
	
	protected override void RotateTowardPlayer ()
	{
		base.RotateTowardPlayer ();
	}
	
	protected override void DialogSetup ()
	{
        //If the player hasn't been introduced
        if (!myGameManager.HasBeenIntroduced)
            dialogString = "Hall_Monitor_Intro";
        else //Otherwise a random dialog will be set
        {
            //Randomizes dialog the Hall Monitor says making sure they don't repeat the same dialog twice in a row.
            int rand = 0;
            rand = Random.Range(1, 7);

            //If the last random number is equal to the one that was just generated...
            if (_lastRand != rand && rand == 6)//And that number is 6, rand will be set to 5
                rand = 5;
            else if (_lastRand != rand && rand == 1)//And that number is 1, rand will be set to 2
                rand = 2;

            _lastRand = rand;
            dialogString = "Hall_Monitor_" + rand;
            myConTrigger.conversation = dialogString;
            myConTrigger.trigger = DialogueTriggerEvent.OnUse;
        }

		base.DialogSetup ();
	}
}
