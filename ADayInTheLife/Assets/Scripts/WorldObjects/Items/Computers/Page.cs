using UnityEngine;
using System.Collections;

public class Page : MonoBehaviour
{
	public SpriteRenderer MySprite;
	public float PageLength;
	public bool IsActive
	{
		get
		{
			return _isActive;
		}
		set
		{
			if(value)
				MySprite.enabled = true;
			else
				MySprite.enabled = false;

			_isActive = value;
		}
	}
	private bool _isActive;
	private GameObject _myComputer;
	private float _autoScrollTimer = 0,
				  _lastScrollTime = 0;
	private bool _autoScrollTimerStarted = false;

	void Start()
	{
		_myComputer = this.transform.parent.gameObject;
	}

	void Update()
	{
		if(IsActive)
			ScrollControl();
	}

	private void ScrollControl()
	{
		if((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && this.transform.position.y <= PageLength)
		{
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f);
			_autoScrollTimerStarted = true;
		}
		if((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && this.transform.position.y >= 0)
		{
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.5f);
			_autoScrollTimerStarted = true;
		}
		
		//This part handles the auto scroll
		if(_autoScrollTimerStarted)
		{
			_autoScrollTimer += Time.deltaTime;
			if(_autoScrollTimer > 0.7f)
			{
				_lastScrollTime += Time.deltaTime;
				if(_lastScrollTime > 0.05f)
				{
					_lastScrollTime = 0;
					if((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && this.transform.position.y <= PageLength)
						this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f);
					else if((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && this.transform.position.y >= 0)
						this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.5f);
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
}
