using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class ElevatorCharacter : NPCScript
{
    //For now there are only two Elevator Characters: The Prince(ipal) and the Administrative Assistant and this bool indicates which one we're seeing
    //NOTE: This may need to be changed to an enum in the future if we get any new elevator characters
    private bool _elevatorPrince;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnConversationEnd(Transform actor)
    {
        base.OnConversationEnd(actor);

        //Any time a conversation with an elevator character ends we call the MoveElevator method
        MoveElevator();
    }

    protected override void DialogSetup()
    {
        //Checks to see which elevator character we're talking to and sets dialog accordingly
        if (_elevatorPrince)
            dialogString = "Prince";
        else
            dialogString = "Administrative_Assistant";

        base.DialogSetup();
    }

    private void MoveElevator()
    {
        //If this is the Administrative Assistant and the dialog calls for a switch to happen,
        //then the change animation is played, the lua variable is reset, _elevatorPrince is set to true, and DialogSetup is called.
        if (!_elevatorPrince && !animation.isPlaying && DialogueLua.GetVariable("SwitchToPrince").AsBool)
        {
            DialogueLua.SetVariable("SwitchToPrince", false);
            animation.Play("OfficeElevatorDown");
            _elevatorPrince = true;
            DialogSetup();
        }
        //If this is the Prince(ipal) and the dialog calls for a switch to happen,
        //then the change animation is played, the lua variable is reset, _elevatorPrince is set to false, and DialogSetup is called.
        if (_elevatorPrince && !animation.isPlaying && DialogueLua.GetVariable("SwitchToAA").AsBool)
        {
            DialogueLua.SetVariable("SwitchToAA", false);
            animation.Play("OfficeElevatorUp");
            _elevatorPrince = false;
            DialogSetup();
        }
    }
}