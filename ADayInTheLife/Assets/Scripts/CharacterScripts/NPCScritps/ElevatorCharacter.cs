using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class ElevatorCharacter : NPCScript
{
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

        MoveElevator();
    }

    protected override void DialogSetup()
    {
        if (_elevatorPrince)
            dialogString = "Prince";
        else
            dialogString = "Administrative_Assistant";

        base.DialogSetup();
    }

    private void MoveElevator()
    {
        if (!_elevatorPrince && !animation.isPlaying && DialogueLua.GetVariable("SwitchToPrince").AsBool)
        {
            DialogueLua.SetVariable("SwitchToPrince", false);
            animation.Play("OfficeElevatorDown");
            _elevatorPrince = true;
            DialogSetup();
        }
        if (_elevatorPrince && !animation.isPlaying && DialogueLua.GetVariable("SwitchToAA").AsBool)
        {
            DialogueLua.SetVariable("SwitchToAA", false);
            animation.Play("OfficeElevatorUp");
            _elevatorPrince = false;
            DialogSetup();
        }
    }
}