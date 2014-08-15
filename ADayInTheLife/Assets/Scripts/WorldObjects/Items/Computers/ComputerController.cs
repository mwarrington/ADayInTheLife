using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComputerController : ItemController
{
	private Dictionary<string, Page> Pages = new Dictionary<string, Page>();

	protected Page currentPage
	{
		get
		{
			foreach(Page p in Pages.Values)
			{
				if(p.IsActive)
					_currentPage = p;
			}

			return _currentPage;
		}
	}
	private Page _currentPage;
	
	public Page StartPage;

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

	public void ChangePage(string nextPage)
	{
		currentPage.transform.localPosition = new Vector3 (currentPage.transform.localPosition.x, 0);
		currentPage.IsActive = false;
		Pages[nextPage].IsActive = true;
	}

	protected override void TurnOff ()
	{
		base.TurnOff ();
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			ItemCamera.enabled = false;
		}
	}
}