﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComputerController : ItemController
{
	protected override void Start ()
	{
		base.Start ();

        ItemCamera = GameObject.FindGameObjectWithTag("ComputerCamera").GetComponent<Camera>();
        
        foreach (Page p in GetComponentsInChildren<Page>())
        {
            Pages.Add(p.name, p);
        }
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