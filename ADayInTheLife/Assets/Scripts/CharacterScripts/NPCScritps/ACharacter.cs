using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class ACharacter : NPCScript
{
	public int DialogIndex;

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

	//For now only BCCharacters use this function
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
		dialogString = DialogIndex.ToString();

		base.DialogSetup ();
	}
}
