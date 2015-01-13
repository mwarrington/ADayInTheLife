using UnityEngine;
using System.Collections;

public class ComputerInteract : ItemInteract
{
	protected override void Start ()
	{
		base.Start ();
	}

    protected override void OnTriggerEnter(Collider col)
	{
        base.OnTriggerEnter(col);
	}
}
