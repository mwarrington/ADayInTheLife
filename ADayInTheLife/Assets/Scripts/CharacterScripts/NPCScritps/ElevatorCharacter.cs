using UnityEngine;
using System.Collections;

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

    private void MoveElevator()
    {
        if (!_elevatorPrince && !animation.isPlaying)
        {
            animation.Play("OfficeElevatorDown");
            _elevatorPrince = true;
        }
        if (_elevatorPrince && !animation.isPlaying)
        {
            animation.Play("OfficeElevatorUp");
            _elevatorPrince = false;
        }
    }
}
