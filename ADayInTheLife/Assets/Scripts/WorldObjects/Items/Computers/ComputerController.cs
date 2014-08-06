using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComputerController : ItemController
{
	public List<Page> Pages = new List<Page>();

	protected Page currentPage
	{
		get
		{
			for(int i = 0; i < Pages.Count; i++)
			{
				if(Pages[i].IsActive)
					_currentPage = Pages[i];
			}

			return _currentPage;
		}
	}
	private Page _currentPage;
	private float _autoScrollTimer = 0,
				  _lastScrollTime = 0;
	private bool _autoScrollTimerStarted = false;

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
		if((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && currentPage.gameObject.transform.position.y <= currentPage.PageLength)
		{
			currentPage.gameObject.transform.position = new Vector3(currentPage.gameObject.transform.position.x, currentPage.gameObject.transform.position.y + 0.5f);
			_autoScrollTimerStarted = true;
		}
		if((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && currentPage.gameObject.transform.position.y >= 0)
		{
			currentPage.gameObject.transform.position = new Vector3(currentPage.gameObject.transform.position.x, currentPage.gameObject.transform.position.y - 0.5f);
			_autoScrollTimerStarted = true;
		}

		//This part handles the auto scroll
		if(_autoScrollTimerStarted)
		{
			_autoScrollTimer += Time.deltaTime;
			if(_autoScrollTimer > 0.7f)
			{
				_lastScrollTime += Time.deltaTime;
				if(_lastScrollTime > 0.1f)
				{
					_lastScrollTime = 0;
					if((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && currentPage.gameObject.transform.position.y <= currentPage.PageLength)
						currentPage.gameObject.transform.position = new Vector3(currentPage.gameObject.transform.position.x, currentPage.gameObject.transform.position.y + 0.5f);
					else if((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && currentPage.gameObject.transform.position.y >= 0)
						currentPage.gameObject.transform.position = new Vector3(currentPage.gameObject.transform.position.x, currentPage.gameObject.transform.position.y - 0.5f);
				}
			}
		}

		if((Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S)))
		{
			_autoScrollTimerStarted = false;
			_autoScrollTimer = 0;
			_lastScrollTime = 0;
		}
		if((Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W)))
		{
			_autoScrollTimerStarted = false;
			_autoScrollTimer = 0;
			_lastScrollTime = 0;
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