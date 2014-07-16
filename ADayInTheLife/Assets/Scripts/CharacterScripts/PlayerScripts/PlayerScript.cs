using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class PlayerScript : MonoBehaviour
{
	private bool _isWalkingForward = false,
				 _isWalkingBack = false,
				 _isWalkingLeft = false,
				 _isWalkingRight = false,
				 _hitWallForward = false,
				 _hitWallBack = false,
				 _hitWallLeft = false,
				 _hitWallRight = false;
	private float _bobTimer,
				  _waveslice,
				  _midpoint;
	private Vector3 _newPos;

	protected Scenes currentScene
	{
		get
		{
			switch (Application.loadedLevelName)
			{
				case "Hallway":
					_currentScene = Scenes.Hallway;
					break;
				case "Labrary":
					_currentScene = Scenes.Labrary;
					break;
				default:
					Debug.Log("That Scene Doesn't Exist!!!");
					break;
			}
			return _currentScene;
		}
		set
		{
			Debug.Log("You shouldn't be trying to set this property");
		}
	}
	private Scenes _currentScene;

	protected bool isSarylyn
	{
		get
		{
			_isSarylyn = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().isSarylyn;
			return _isSarylyn;
		}

	}
	private bool _isSarylyn = true;

	public SpriteRenderer SarylynSprite,
						  SanomeSprite;
	public float PlayerVelocity,
				 BobbingSpeed,
				 BobbingAmount;

	void Start()
	{
		_midpoint = this.gameObject.transform.position.y;
		if(isSarylyn)
		{
			SarylynSprite.enabled = true;
			SanomeSprite.enabled = false;
		}
		else
		{
			SanomeSprite.enabled = true;
			SarylynSprite.enabled = false;
		}
	}

	void Update ()
	{
		InputBasedMovement();
		Headbob();
		if(Input.GetKeyDown(KeyCode.A))
		{
			Application.LoadLevel("Test");
		}
		if(Input.GetKeyDown(KeyCode.B))
		{
			Application.LoadLevel("Hallway");
		}
	}

	private void InputBasedMovement()
	{
		switch(currentScene)
		{
			case Scenes.Hallway:
				//When the player presses an arrow key
				if(Input.GetKeyDown(KeyCode.UpArrow))
				{
					_isWalkingForward = true;
				}
				if(Input.GetKeyDown(KeyCode.DownArrow))
				{
					_isWalkingBack = true;
				}
				if(Input.GetKeyDown(KeyCode.LeftArrow))
				{
					_isWalkingLeft = true;
				}
				if(Input.GetKeyDown(KeyCode.RightArrow))
				{
					_isWalkingRight = true;
				}
				
				//Once the player has pressed an arrow key but hasn't let go of an arrow key
				if(_isWalkingForward && !_hitWallForward)
				{
					this.transform.Translate(Vector3.forward * PlayerVelocity);
				}
				if(_isWalkingBack && !_hitWallBack)
				{
					this.transform.Translate(Vector3.back * PlayerVelocity);
				}
				if(_isWalkingLeft && !_hitWallLeft)
				{
					this.transform.Translate(Vector3.left * PlayerVelocity);
				}
				if(_isWalkingRight && !_hitWallRight)
				{
					this.transform.Translate(Vector3.right * PlayerVelocity);
				}

				//When the player lets go of an arrow key
				if(Input.GetKeyUp(KeyCode.UpArrow))
				{
					_isWalkingForward = false;
				}
				if(Input.GetKeyUp(KeyCode.DownArrow))
				{
					_isWalkingBack = false;
				}
				if(Input.GetKeyUp(KeyCode.LeftArrow))
				{
					_isWalkingLeft = false;
				}
				if(Input.GetKeyUp(KeyCode.RightArrow))
				{
					_isWalkingRight = false;
				}
			break;
		case Scenes.Labrary:
			//When the player presses an arrow key
			if(Input.GetKeyDown(KeyCode.LeftArrow))
			{
				_isWalkingLeft = true;
			}
			if(Input.GetKeyDown(KeyCode.RightArrow))
			{
				_isWalkingRight = true;
			}
			
			//Once the player has pressed an arrow key but hasn't let go of an arrow key
			if(_isWalkingLeft && !_hitWallLeft)
			{
				this.transform.Translate(Vector3.left * PlayerVelocity);
			}
			if(_isWalkingRight && !_hitWallRight)
			{
				this.transform.Translate(Vector3.right * PlayerVelocity);
			}
			
			//When the player lets go of an arrow key
			if(Input.GetKeyUp(KeyCode.LeftArrow))
			{
				_isWalkingLeft = false;
			}
			if(Input.GetKeyUp(KeyCode.RightArrow))
			{
				_isWalkingRight = false;
			}
				break;
			default:
				Debug.Log("That Scene Doesn't Exist!!!");
				break;
		}
	}

	private void Headbob()
	{
		//Headbobbing script
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		
		if ((!_isWalkingForward && !_isWalkingBack && !_isWalkingLeft && !_isWalkingRight) || _hitWallForward || _hitWallBack || _hitWallLeft || _hitWallRight)
		{
			_bobTimer = 0.0f;
			horizontal = 0;
			vertical = 0;
		}
		else
		{
			_waveslice = Mathf.Sin(_bobTimer);
			_bobTimer = _bobTimer + BobbingSpeed;
			if (_bobTimer > Mathf.PI * 2)
			{
				_bobTimer = _bobTimer - (Mathf.PI * 2);
			}
		}
		if (_waveslice != 0)
		{
			float translateChange = _waveslice * BobbingAmount;
			float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
			totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
			translateChange = totalAxes * translateChange;
			_newPos = transform.position;
			_newPos.y = _midpoint + translateChange;
			transform.position = _newPos;
		}
		else
		{
			Vector3 findMidpoint = transform.position;
			findMidpoint.y = _midpoint;
			transform.position = findMidpoint;
		}
	}

	void OnCollisionEnter(Collision col)
	{
		Vector3 centerColPoint = new Vector3();
		
		//Finds centermost point of collision
		foreach (ContactPoint colPoint in col.contacts)
		{
			float centerDistance = 50;
			if (Vector3.Distance(colPoint.point, transform.position) < centerDistance)
			{
				centerDistance = Vector3.Distance(colPoint.point, transform.position);
				centerColPoint = colPoint.point;
			}
		}
		
		//Checks to see if the centermost point of collision is on the same side as the player is moving.
		if (_isWalkingForward && centerColPoint.z > this.transform.position.z + 0.7f)
			_hitWallForward = true;
		if (_isWalkingBack && centerColPoint.z < this.transform.position.z - 0.7f)
			_hitWallBack = true;
		if (_isWalkingLeft && centerColPoint.x < this.transform.position.x - 0.7f)
			_hitWallLeft = true;
		if (_isWalkingRight && centerColPoint.x > this.transform.position.x + 0.7f)
			_hitWallRight = true;
	}

	void OnCollisionExit(Collision col)
	{
		_hitWallForward = false;
		_hitWallBack = false;
		_hitWallLeft = false;
		_hitWallRight = false;
	}

	void OnConversationStart(Transform actor)
	{
		SarylynSprite.enabled = false;
		SanomeSprite.enabled = false;
	}

	void OnConversationEnd(Transform actor)
	{
		if(isSarylyn)
		{
			SarylynSprite.enabled = true;
			SanomeSprite.enabled = false;
		}
		else
		{
			SanomeSprite.enabled = true;
			SarylynSprite.enabled = false;
		}
	}
}
