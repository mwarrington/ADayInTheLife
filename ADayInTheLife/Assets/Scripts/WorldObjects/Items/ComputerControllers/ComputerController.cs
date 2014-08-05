using UnityEngine;
using System.Collections;

public class ComputerController : ItemController
{
	public GameObject Page;
	public float PageLength;

	protected override void Start ()
	{
		base.Start ();
		
		itemCamera = GameObject.FindGameObjectWithTag("ComputerCamera").GetComponent<Camera>();
	}

	protected override void Update ()
	{
		base.Update ();

		ScrollPage();
	}

	private void ScrollPage()
	{
		if(Input.GetKeyDown(KeyCode.DownArrow) && Page.transform.position.y <= PageLength)
		{
			Page.transform.position = new Vector3(Page.transform.position.x, Page.transform.position.y + 0.5f);
		}
		if(Input.GetKeyDown(KeyCode.UpArrow) && Page.transform.position.y >= 0)
		{
			Page.transform.position = new Vector3(Page.transform.position.x, Page.transform.position.y - 0.5f);
		}
	}

	protected override void TurnOff ()
	{
		base.TurnOff ();
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			itemCamera.enabled = false;
		}
	}
}
