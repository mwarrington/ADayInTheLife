using UnityEngine;
using System.Collections;

public class BookController : ItemController
{
	protected override void Start ()
	{
		base.Start ();
		
		ItemCamera = GameObject.FindGameObjectWithTag("BookCamera").GetComponent<Camera>();
	}

	protected override void Update ()
	{
		base.Update ();
	}

	public override void ChangePage (string nextPage)
	{
		base.ChangePage (nextPage);
	}

	protected override void TurnOff ()
	{
		base.TurnOff ();
	}
}
