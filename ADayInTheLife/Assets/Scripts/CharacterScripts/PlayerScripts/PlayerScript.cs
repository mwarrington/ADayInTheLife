using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class PlayerScript : MonoBehaviour
{
	private bool _isWalkingForward = false,
				 _isWalkingBack = false,
				 _isWalkingLeft = false,
				 _isWalkingRight = false,
				 _isRotatingLeft = false,
				 _isRotatingRight = false,
				 _hitWallForward = false,
				 _hitWallBack = false,
				 _hitWallLeft = false,
				 _hitWallRight = false,
				 _playingFootSound = false,
				 _isSarylyn = true;
	private float _bobTimer,
				  _waveslice,
				  _midpoint,
				  _gameTimer;
	private Vector3 _newPos,
					_orriginalRotation;
	private GameManager _myManager;

	public Scenes CurrentScene
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
				case "Classroom":
					_currentScene = Scenes.Classroom;
					break;
				case "Roomclass":
					_currentScene = Scenes.Roomclass;
					break;
				case "Cafeteria":
					_currentScene = Scenes.Cafeteria;
					break;
				case "SecurityCameraRoomTest":
					_currentScene = Scenes.SecCamTemp;
					break;
				case "ConsularOffice":
					_currentScene = Scenes.ConsularOffice;
					break;
				case "ChorusRoom":
					_currentScene = Scenes.ChorusRoom;
					break;
				case "Lobby":
					_currentScene = Scenes.Lobby;
					break;
				case "GreenWorld":
					_currentScene = Scenes.GreenWorld;
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
			_isSarylyn = _myManager.IsSarylyn;
			return _isSarylyn;
		}
	}

	public bool IsMoving
	{
		get
		{
			if(_isWalkingForward || _isWalkingBack || _isWalkingLeft || _isWalkingRight)
				_isMoving = true;
			else
				_isMoving = false;

			return _isMoving;
		}
	}
	private bool _isMoving;

	public GameObject TimerCloud,
					  FootSound;
	public SpriteRenderer SarylynSprite,
						  SanomeSprite;
	public float PlayerVelocity,
				 BobbingSpeed,
				 BobbingAmount,
				 TranslateChange;
	public bool ConfusedMovement,
				DontHideSprite;

	public GameObject SlowBGM, FastBGM;

	void Start()
	{
		_myManager = FindObjectOfType<GameManager>();
		_midpoint = this.gameObject.transform.position.y;
		_orriginalRotation = this.transform.rotation.eulerAngles;
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

		//Sets the player's appropriate postion then
		//Declares current scene as the LastLevelLoaded
		SetPlayerPosition();
		_myManager.LastLevelLoaded = Application.loadedLevelName;
	}

	void Update ()
	{
		InputBasedMovement();
		Headbob();
		if (Application.loadedLevelName == "GreenWorld")
		{
			if (IsMoving)
			{
				SlowBGM.audio.volume = 0;
				FastBGM.audio.volume = 1.0f;

			}
			else
			{
				SlowBGM.audio.volume = 1.0f;
				FastBGM.audio.volume = 0;	
			}
		}
	}

	private void InputBasedMovement()
	{
		switch(CurrentScene)
		{
			case Scenes.Hallway:
				Standard2DMove();
				break;
			case Scenes.Labrary:
				Standard1DMove();
				break;
			case Scenes.Classroom:
				Rotatable2DMove();
				break;
			case Scenes.Roomclass:
				Rotatable2DMove();
				break;
			case Scenes.Cafeteria:
				Auto1DMove();
				break;
			case Scenes.SecCamTemp:
				SecurityCamera2DMove();
				break;
			case Scenes.ConsularOffice:
				ZeroDMove();
				break;
			case Scenes.ChorusRoom:
				ZeroDMove();
				break;
			case Scenes.Lobby:
				Standard2DMove();
				break;
			case Scenes.GreenWorld:
				Standard2DMove();
				break;
			default:
				Debug.Log("That Scene Doesn't Exist!!!");
				break;
		}
	}
	
	//This method handles the Headbobbing
	private void Headbob()
	{
		float horizontal = 1;
		float vertical = 1;
		
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
			TranslateChange = _waveslice * BobbingAmount;
			float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
			totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
			TranslateChange = totalAxes * TranslateChange;
			_newPos = transform.position;
			_newPos.y = _midpoint + TranslateChange;
			transform.position = _newPos;
			if(_newPos.y < _midpoint-(BobbingAmount*0.4f) && !_playingFootSound)
			{
				_playingFootSound = true;
				FootSound.audio.Play();
			}
			if(_newPos.y > _midpoint+(BobbingAmount*0.4f) && _playingFootSound)
				_playingFootSound = false;

		}
		else
		{
			Vector3 findMidpoint = transform.position;
			findMidpoint.y = _midpoint;
			transform.position = findMidpoint;
		}
	}

	//This method sets the player position depending on which scene the player was in last.
	private void SetPlayerPosition()
	{
		if(Application.loadedLevelName == "Hallway")
		{
			switch(_myManager.LastLevelLoaded)
			{
				case "DreamSpiral":
					this.gameObject.transform.position = new Vector3(-1.582302f, 4.500048f, -54.55913f);
					break;
				case "Labrary":
					this.gameObject.transform.position = new Vector3(-1.582302f, 4.500048f, -54.55913f);
					break;
				case "Classroom":
					this.gameObject.transform.position = new Vector3(3.037517f, 4.500048f, -25.66859f);
					break;
				default:
					Debug.Log ("That Scene scene either doesn't exist or you should be coming from there.");
					break;
			}
		}
	}

	#region Movement Methods
	//Movement Types
	private void Standard2DMove()
	{
		//When the player is pressing an arrow or WASD key
		if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{
			if(!ConfusedMovement)
				_isWalkingForward = true;
			else
				_isWalkingRight = true;
		}
		else
		{
			if(!ConfusedMovement)
				_isWalkingForward = false;
			else
				_isWalkingRight = false;
		}
		if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			if(!ConfusedMovement)
				_isWalkingBack = true;
			else
				_isWalkingLeft = true;
		}
		else
		{
			if(!ConfusedMovement)
				_isWalkingBack = false;
			else
				_isWalkingLeft = false;
		}
		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			if(!ConfusedMovement)
				_isWalkingLeft = true;
			else
				_isWalkingForward = true;
		}
		else
		{
			if(!ConfusedMovement)
				_isWalkingLeft = false;
			else
				_isWalkingForward = false;
		}
		if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			if(!ConfusedMovement)
				_isWalkingRight = true;
			else
				_isWalkingBack = true;
		}
		else
		{
			if(!ConfusedMovement)
				_isWalkingRight = false;
			else
				_isWalkingBack = false;
		}
		
		if (Application.loadedLevelName == "GreenWorld" && IsMoving)
				PlayerVelocity = 0.3f;
			else
				PlayerVelocity = 0.1f;

		//This is how the method uses the bools set by pressing the keys
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
	}

	private void Standard1DMove()
	{
		//When the player is pressing an arrow or WASD key
		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			if(!ConfusedMovement)
				_isWalkingLeft = true;
			else
				_isWalkingRight = true;
		}
		else
		{
			if(!ConfusedMovement)
				_isWalkingLeft = false;
			else
				_isWalkingRight = false;
		}
		if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			if(!ConfusedMovement)
				_isWalkingRight = true;
			else
				_isWalkingLeft = true;
		}
		else
		{
			if(!ConfusedMovement)
				_isWalkingRight = false;
			else
				_isWalkingLeft = false;
		}
		
		//This is how the method uses the bools set by pressing the keys
		if(_isWalkingLeft && !_hitWallLeft)
		{
			this.transform.Translate(Vector3.left * PlayerVelocity);
		}
		if(_isWalkingRight && !_hitWallRight)
		{
			this.transform.Translate(Vector3.right * PlayerVelocity);
		}
	}

	private void Rotatable2DMove()
	{
		//When the player is pressing an arrow or WASD key
		if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{
			if(!ConfusedMovement)
				_isWalkingForward = true;
			else
				_isRotatingRight = true;
		}
		else
		{
			if(!ConfusedMovement)
				_isWalkingForward = false;
			else
				_isRotatingRight = false;
		}
		if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			if(!ConfusedMovement)
				_isWalkingBack = true;
			else
				_isRotatingLeft = true;
		}
		else
		{
			if(!ConfusedMovement)
				_isWalkingBack = false;
			else
				_isRotatingLeft = false;
		}
		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			if(!ConfusedMovement)
				_isRotatingLeft = true;
			else
				_isWalkingForward = true;
		}
		else
		{
			if(!ConfusedMovement)
				_isRotatingLeft = false;
			else
				_isWalkingForward = false;
		}
		if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			if(!ConfusedMovement)
				_isRotatingRight = true;
			else
				_isWalkingBack = true;
		}
		else
		{
			if(!ConfusedMovement)
				_isRotatingRight = false;
			else
				_isWalkingBack = false;
		}
		
		//This is how the method uses the bools set by pressing the keys
		if(_isWalkingForward && !_hitWallForward)
		{
			this.transform.Translate(Vector3.forward * PlayerVelocity);
		}
		if(_isWalkingBack && !_hitWallBack)
		{
			this.transform.Translate(Vector3.back * PlayerVelocity);
		}
		if(_isRotatingLeft)
		{
			this.transform.Rotate(Vector3.down * 1.3f);
		}
		if(_isRotatingRight)
		{
			this.transform.Rotate(Vector3.up * 1.3f);
		}
	}

	private void SecurityCamera2DMove()
	{
		//This will maintain rotation with the camera
		this.transform.LookAt(_myManager.MainCamera.transform);
		this.transform.rotation = Quaternion.Euler(new Vector3(0 + _orriginalRotation.x, (this.transform.rotation.eulerAngles.y + _orriginalRotation.y) + 180, 0 + _orriginalRotation.z));

		//When the player is pressing an arrow or WASD key
		if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{
			if(!ConfusedMovement)
				_isWalkingForward = true;
			else
				_isWalkingRight = true;
		}
		else
		{
			if(!ConfusedMovement)
				_isWalkingForward = false;
			else
				_isWalkingRight = false;
		}
		if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			if(!ConfusedMovement)
				_isWalkingBack = true;
			else
				_isWalkingLeft = true;
		}
		else
		{
			if(!ConfusedMovement)
				_isWalkingBack = false;
			else
				_isWalkingLeft = false;
		}
		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			if(!ConfusedMovement)
				_isWalkingLeft = true;
			else
				_isWalkingForward = true;
		}
		else
		{
			if(!ConfusedMovement)
				_isWalkingLeft = false;
			else
				_isWalkingForward = false;
		}
		if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			if(!ConfusedMovement)
				_isWalkingRight = true;
			else
				_isWalkingBack = true;
		}
		else
		{
			if(!ConfusedMovement)
				_isWalkingRight = false;
			else
				_isWalkingBack = false;
		}
		
		//This is how the method uses the bools set by pressing the keys
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
	}

	private void Auto1DMove()
	{
		//When the player is pressing an arrow or WASD key
		if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow))
			_isWalkingLeft = false;
		else
			_isWalkingLeft = true;
		
		//This is how the method uses the bools set by pressing the keys
		if(_isWalkingLeft)
			this.transform.Translate(Vector3.left * PlayerVelocity);
	}

	private void ZeroDMove()
	{
		//Nothing so far
		if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Escape))
		{
			Application.LoadLevel("Lobby");
		}
		
	}

	public void ConfuseMovement()
	{
		ConfusedMovement = true;
		_isWalkingForward = false;
		_isWalkingBack = false;
		_isWalkingRight = false;
		_isWalkingLeft = false;
		_isRotatingRight = false;
		_isRotatingLeft = false;
	}
	#endregion

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
		if(!DontHideSprite)
		{
			SarylynSprite.enabled = false;
			SanomeSprite.enabled = false;
		}

		TimerCloud.GetComponent<SpriteRenderer> ().enabled = false;
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
		TimerCloud.GetComponent<SpriteRenderer> ().enabled = true;
	}
}
