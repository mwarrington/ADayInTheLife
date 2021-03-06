﻿using UnityEngine;
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
                 _isSarylyn = true,
                 _hasSpeedControls = false,
                 _blurStarted = false,
                 _fastRotateStarted = false;
    private float _bobTimer,
                  _waveslice,
                  _midpoint,
                  _gameTimer,
                  _playerVelocity;
    private Vector3 _newPos,
                    _orriginalRotation;
    private GameManager _myManager;
    private AudioClip[] _mySpeedClips = new AudioClip[5];

    //This property will set itself based on which scene we are currently in
    //We also set the whether the player is able to move fast
    public Scenes CurrentScene
    {
        get
        {
            switch (Application.loadedLevelName)
            {
                case "Hallway":
                    _currentScene = Scenes.Hallway;
                    _hasSpeedControls = true;
                    break;
                case "Labrary":
                    _currentScene = Scenes.Labrary;
                    _hasSpeedControls = true;
                    break;
                case "Classroom":
                    _currentScene = Scenes.Classroom;
                    _hasSpeedControls = true;
                    break;
                case "Roomclass":
                    _currentScene = Scenes.Roomclass;
                    _hasSpeedControls = true;
                    break;
                case "Cafeteria":
                    _currentScene = Scenes.Cafeteria;
                    _hasSpeedControls = true;
                    break;
                case "SecurityCameraRoomTest":
                    _currentScene = Scenes.SecCamTemp;
                    _hasSpeedControls = true;
                    break;
                case "ConsularOffice":
                    _currentScene = Scenes.ConsularOffice;
                    _hasSpeedControls = false;
                    break;
                case "ChorusRoom":
                    _currentScene = Scenes.ChorusRoom;
                    _hasSpeedControls = false;
                    break;
                case "Lobby":
                    _currentScene = Scenes.Lobby;
                    _hasSpeedControls = true;
                    break;
                case "GreenWorld":
                    _currentScene = Scenes.GreenWorld;
                    _hasSpeedControls = true;
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

    //This will set itself to true or false depending on a field form the GameManager
    protected bool isSarylyn
    {
        get
        {
            _isSarylyn = _myManager.IsSarylyn;
            return _isSarylyn;
        }
    }

    //Looks to see if any of the isWalking bools is true and sets IsMoving to true if so
    public bool IsMoving
    {
        get
        {
            if (_isWalkingForward || _isWalkingBack || _isWalkingLeft || _isWalkingRight)
                _isMoving = true;
            else
                _isMoving = false;

            return _isMoving;
        }
    }
    private bool _isMoving,
                 _spedUp = false;

    public GameObject TimerCloud,
                      FootSound;
    public SpriteRenderer SarylynSprite,
                          SanomeSprite;
    public AudioSource SpeedSFX;
    public LayerMask CollisionLayers;
    public Transform ForwardCollisionPoint,
                     BackwardCollisionPoint;
    public float StandardVelocity,
                 BobbingSpeed,
                 BobbingAmount,
                 TranslateChange;
    public bool ConfusedMovement, //At 5 seconds to day end the movements of the player is messed up
                DontHideSprite,
                CanMove = true;

    public GameObject SlowBGM, FastBGM;

    void Start()
    {
        _myManager = FindObjectOfType<GameManager>();
        _midpoint = this.gameObject.transform.position.y;
        _orriginalRotation = this.transform.rotation.eulerAngles;

        //Depending on whether the player is Sarylyn or not the correct sprite will be set to true
        if (isSarylyn)
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
        SetPlayerPosition();

        //Declares current scene as the LastLevelLoaded
        _myManager.LastLevelLoaded = Application.loadedLevelName;

        //Population of _mySpeedClips
        _mySpeedClips[0] = PrefabLoaderScript.instance.Boing;
        _mySpeedClips[1] = PrefabLoaderScript.instance.Pop;
        _mySpeedClips[2] = PrefabLoaderScript.instance.Werp;
        _mySpeedClips[3] = PrefabLoaderScript.instance.Wheee;
        _mySpeedClips[4] = PrefabLoaderScript.instance.Xylophone;
    }

    void Update()
    {
        //This method handles all types of movement
        if (CanMove)
            InputBasedMovement();

        //Some scenes alow you move at a faster speed
        if (_hasSpeedControls)
            WalkSpeedHandler();

        //This method handles the head bob
        Headbob();

        //In the GreenWorld the music changes depending on if you're moving or not
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
        //This checks to see which scene is active then uses the appropriate method
        switch (CurrentScene)
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

        //If the player is neither walking or 
        if (!IsMoving || _hitWallForward || _hitWallBack || _hitWallLeft || _hitWallRight)
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
            if (_newPos.y < _midpoint - (BobbingAmount * 0.4f) && !_playingFootSound)
            {
                _playingFootSound = true;
                FootSound.audio.Play();
            }
            if (_newPos.y > _midpoint + (BobbingAmount * 0.4f) && _playingFootSound)
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
    //This might be scrapped since we want to have a sort of loop effect whenever the player changes scenes.
    private void SetPlayerPosition()
    {
        if (Application.loadedLevelName == "Hallway")
        {
            switch (_myManager.LastLevelLoaded)
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
                    Debug.Log("That Scene scene either doesn't exist or you should be coming from there.");
                    break;
            }
        }
    }

    //Movement Types
    #region Movement Methods
    #region Movement Types
    //This movement type is the most basic four directional movement
    private void Standard2DMove()
    {
        //When the player is pressing an arrow or WASD key it changes one of the four direction bools
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            if (!ConfusedMovement)
                _isWalkingForward = true;
            else
                _isWalkingRight = true;
        }
        else
        {
            if (!ConfusedMovement)
                _isWalkingForward = false;
            else
                _isWalkingRight = false;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            if (!ConfusedMovement)
                _isWalkingBack = true;
            else
                _isWalkingLeft = true;
        }
        else
        {
            if (!ConfusedMovement)
                _isWalkingBack = false;
            else
                _isWalkingLeft = false;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (!ConfusedMovement)
                _isWalkingLeft = true;
            else
                _isWalkingForward = true;
        }
        else
        {
            if (!ConfusedMovement)
                _isWalkingLeft = false;
            else
                _isWalkingForward = false;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (!ConfusedMovement)
                _isWalkingRight = true;
            else
                _isWalkingBack = true;
        }
        else
        {
            if (!ConfusedMovement)
                _isWalkingRight = false;
            else
                _isWalkingBack = false;
        }

        //This is how the method uses the bools set by pressing the keys
        if (_isWalkingForward && !_hitWallForward)
        {
            this.transform.Translate(Vector3.forward * _playerVelocity * Time.deltaTime);
        }
        if (_isWalkingBack && !_hitWallBack)
        {
            this.transform.Translate(Vector3.back * _playerVelocity * Time.deltaTime);
        }
        if (_isWalkingLeft && !_hitWallLeft)
        {
            this.transform.Translate(Vector3.left * _playerVelocity * Time.deltaTime);
        }
        if (_isWalkingRight && !_hitWallRight)
        {
            this.transform.Translate(Vector3.right * _playerVelocity * Time.deltaTime);
        }
    }

    //This is used for scenes where the player can only move left or right
    private void Standard1DMove()
    {
        //When the player is pressing an arrow or WASD key it changes one of the four direction bools
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (!ConfusedMovement)
                _isWalkingLeft = true;
            else
                _isWalkingRight = true;
        }
        else
        {
            if (!ConfusedMovement)
                _isWalkingLeft = false;
            else
                _isWalkingRight = false;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (!ConfusedMovement)
                _isWalkingRight = true;
            else
                _isWalkingLeft = true;
        }
        else
        {
            if (!ConfusedMovement)
                _isWalkingRight = false;
            else
                _isWalkingLeft = false;
        }

        //This is how the method uses the bools set by pressing the keys
        if (_isWalkingLeft && !_hitWallLeft)
        {
            this.transform.Translate(Vector3.left * _playerVelocity * Time.deltaTime);
        }
        if (_isWalkingRight && !_hitWallRight)
        {
            this.transform.Translate(Vector3.right * _playerVelocity * Time.deltaTime);
        }
    }

    //This is used for scenes where the player rotates with the left(or A) or right(or D) key
    private void Rotatable2DMove()
    {
        //When the player is pressing an arrow or WASD key it changes one of the two direction bools or one of the two rotation bools
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            if (!ConfusedMovement)
                _isWalkingForward = true;
            else
                _isRotatingRight = true;
        }
        else
        {
            if (!ConfusedMovement)
                _isWalkingForward = false;
            else
                _isRotatingRight = false;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            if (!ConfusedMovement)
                _isWalkingBack = true;
            else
                _isRotatingLeft = true;
        }
        else
        {
            if (!ConfusedMovement)
                _isWalkingBack = false;
            else
                _isRotatingLeft = false;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (!ConfusedMovement)
                _isRotatingLeft = true;
            else
                _isWalkingForward = true;
        }
        else
        {
            if (!ConfusedMovement)
                _isRotatingLeft = false;
            else
                _isWalkingForward = false;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (!ConfusedMovement)
                _isRotatingRight = true;
            else
                _isWalkingBack = true;
        }
        else
        {
            if (!ConfusedMovement)
                _isRotatingRight = false;
            else
                _isWalkingBack = false;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!ConfusedMovement)
                FastTurn(true);
            else
                FastTurn(false);

            _fastRotateStarted = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!ConfusedMovement)
                FastTurn(false);
            else
                FastTurn(true);

            _fastRotateStarted = true;
        }

        //This is how the method uses the bools set by pressing the keys
        if (_isWalkingForward && !_hitWallForward)
        {
            this.transform.Translate(Vector3.forward * _playerVelocity * Time.deltaTime);
        }
        if (_isWalkingBack && !_hitWallBack)
        {
            this.transform.Translate(Vector3.back * _playerVelocity * Time.deltaTime);
        }
        if (_isRotatingLeft)
        {
            this.transform.Rotate(Vector3.down * 50f * Time.deltaTime);
        }
        if (_isRotatingRight)
        {
            this.transform.Rotate(Vector3.up * 50f * Time.deltaTime);
        }

        //Increases the area of screen that the blur covers based on how close we are to the target amount
        //In other words: the farther we are to the target the faster the blur area will increase and vice versa
        if (_fastRotateStarted)
        {
            if (_myManager.MainCamera.GetComponent<CameraFilterPack_Blur_Focus>()._Eyes > 6.2f)
                _myManager.MainCamera.GetComponent<CameraFilterPack_Blur_Focus>()._Eyes -= Time.deltaTime * (25 * (_myManager.MainCamera.GetComponent<CameraFilterPack_Blur_Focus>()._Eyes - 6));
            else //Once the target is reached the value is set to 4 to accomidate any over shootting
            {
                _myManager.MainCamera.GetComponent<CameraFilterPack_Blur_Focus>()._Eyes = 6;
                _fastRotateStarted = false;
            }
        }

        //These lines handle whether we are at a wall in front of us or behind us.
        //We create a very small sphere in front and behind the player to see if anything other than the player is overlaping the sphere.
        _hitWallForward = (Physics.OverlapSphere(ForwardCollisionPoint.position, 0.1f, CollisionLayers).Length > 0);
        _hitWallBack = (Physics.OverlapSphere(BackwardCollisionPoint.position, 0.1f, CollisionLayers).Length > 0);
    }

    //This is used for rooms that have a second person view as if the player is being watched by a security camera
    private void SecurityCamera2DMove()
    {
        //This will maintain player rotation with the camera
        this.transform.LookAt(_myManager.MainCamera.transform);
        this.transform.rotation = Quaternion.Euler(new Vector3(0 + _orriginalRotation.x, (this.transform.rotation.eulerAngles.y + _orriginalRotation.y) + 180, 0 + _orriginalRotation.z));

        //When the player is pressing an arrow or WASD key it changes one of the four direction bools
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            if (!ConfusedMovement)
                _isWalkingForward = true;
            else
                _isWalkingRight = true;
        }
        else
        {
            if (!ConfusedMovement)
                _isWalkingForward = false;
            else
                _isWalkingRight = false;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            if (!ConfusedMovement)
                _isWalkingBack = true;
            else
                _isWalkingLeft = true;
        }
        else
        {
            if (!ConfusedMovement)
                _isWalkingBack = false;
            else
                _isWalkingLeft = false;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (!ConfusedMovement)
                _isWalkingLeft = true;
            else
                _isWalkingForward = true;
        }
        else
        {
            if (!ConfusedMovement)
                _isWalkingLeft = false;
            else
                _isWalkingForward = false;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (!ConfusedMovement)
                _isWalkingRight = true;
            else
                _isWalkingBack = true;
        }
        else
        {
            if (!ConfusedMovement)
                _isWalkingRight = false;
            else
                _isWalkingBack = false;
        }

        //This is how the method uses the bools set by pressing the keys
        if (_isWalkingForward && !_hitWallForward)
        {
            this.transform.Translate(Vector3.forward * _playerVelocity * Time.deltaTime);
        }
        if (_isWalkingBack && !_hitWallBack)
        {
            this.transform.Translate(Vector3.back * _playerVelocity * Time.deltaTime);
        }
        if (_isWalkingLeft && !_hitWallLeft)
        {
            this.transform.Translate(Vector3.left * _playerVelocity * Time.deltaTime);
        }
        if (_isWalkingRight && !_hitWallRight)
        {
            this.transform.Translate(Vector3.right * _playerVelocity * Time.deltaTime);
        }
    }

    //This is used for rooms where the player automatically moves as if on a conveyor belt
    private void Auto1DMove()
    {
        //When the player is pressing an arrow or WASD key in the opposite direction of the auto movement it changes the one relavent bool
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            _isWalkingLeft = false;
        else
            _isWalkingLeft = true;

        //This is how the method uses the bool set by pressing the keys
        if (_isWalkingLeft)
            this.transform.Translate(Vector3.left * _playerVelocity * Time.deltaTime);
    }

    //This is for scenes where the player can't move
    private void ZeroDMove()
    {
        //When the player presses the escape key they leave the room
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_myManager.LevelCount == 1)
                Application.LoadLevel("Hallway");
            else if (_myManager.LevelCount == 2)
                Application.LoadLevel("Lobby");
        }
    }
    #endregion Movement Types

    #region Movement Utilities
    //This turns ConfusedMovement to true and halts all player movement
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

    //This method handles the player's ability to walk faster or slower
    private void WalkSpeedHandler()
    {
        if (((_isWalkingLeft && !_hitWallLeft) || (_isWalkingRight && !_hitWallRight) || (_isWalkingForward && !_hitWallForward) || (_isWalkingBack && !_hitWallBack)) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            //Hold left shift to speed up player velocity and bobbing speed, adds a motion blur, and plays a speed sfx
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (!_blurStarted)
                {
                    _myManager.MainCamera.GetComponent<CameraFilterPack_Blur_Focus>().enabled = true;
                    _blurStarted = true;
                }

                _playerVelocity = 15f;
                BobbingSpeed = 20 * Time.deltaTime;

                //Increases the area of screen that the blur covers based on how close we are to the target amount
                //In other words: the farther we are to the target the faster the blur area will increase and vice versa
                if (_myManager.MainCamera.GetComponent<CameraFilterPack_Blur_Focus>()._Eyes > 4)
                    _myManager.MainCamera.GetComponent<CameraFilterPack_Blur_Focus>()._Eyes -= Time.deltaTime * (25 * (_myManager.MainCamera.GetComponent<CameraFilterPack_Blur_Focus>()._Eyes - 4));
                else //Once the target is reached the value is set to 4 to accomidate any over shootting
                    _myManager.MainCamera.GetComponent<CameraFilterPack_Blur_Focus>()._Eyes = 4;
                if (!SpeedSFX.isPlaying && !_spedUp)
                {
                    SpeedSFX.clip = _mySpeedClips[Random.Range(0, 5)];
                    SpeedSFX.Play();
                    _spedUp = true;
                }
            }
            else if (Input.GetKey(KeyCode.RightShift)) //Hold right shift to slow player velocity and bobbing speed, adds a motion blur, and lowers pitch of the BGM
            {
                if (!_blurStarted)
                {
                    _myManager.MainCamera.GetComponent<CameraFilterPack_Blur_Focus>().enabled = true;
                    _blurStarted = true;
                }

                _playerVelocity = 2.4f;
                BobbingSpeed = 5.6f * Time.deltaTime;

                //Increases the area of screen that the blur covers based on how close we are to the target amount
                //In other words: the farther we are to the target the faster the blur area will increase and vice versa
                if (_myManager.MainCamera.GetComponent<CameraFilterPack_Blur_Focus>()._Eyes > 6)
                    _myManager.MainCamera.GetComponent<CameraFilterPack_Blur_Focus>()._Eyes -= Time.deltaTime * (25 * (_myManager.MainCamera.GetComponent<CameraFilterPack_Blur_Focus>()._Eyes - 6));
                else //Once the target is reached the value is set to 4 to accomidate any over shootting
                    _myManager.MainCamera.GetComponent<CameraFilterPack_Blur_Focus>()._Eyes = 6;
                if (_myManager.Timer > 60)
                    _myManager.MainBGM.pitch = 0.7f;
            }
        }
        else if(!_fastRotateStarted) //Otherwise the motion blur is turned off and all values are set to their default
        {
            //Decreases the area of screen that the blur covers based on how close we are to the target amount
            //In other words: the farther we are to the target the slower the blur area will decrease and vice versa
            if (_myManager.MainCamera.GetComponent<CameraFilterPack_Blur_Focus>()._Eyes < 50 && _blurStarted)
                _myManager.MainCamera.GetComponent<CameraFilterPack_Blur_Focus>()._Eyes += Time.deltaTime * (5 * (64 - _myManager.MainCamera.GetComponent<CameraFilterPack_Blur_Focus>()._Eyes));
            else //Once the target is reached the blur effect is turned off
            {
                _myManager.MainCamera.GetComponent<CameraFilterPack_Blur_Focus>().enabled = false;
                _blurStarted = false;
            }

            _playerVelocity = StandardVelocity;
            BobbingSpeed = 12 * Time.deltaTime;

            if (_myManager.Timer > 60)
                _myManager.MainBGM.pitch = 1f;
            _spedUp = false;
        }
    }

    //When called this method flips the player 180 degrees
    private void FastTurn(bool left)
    {
        if (left)
            iTween.RotateTo(this.gameObject, new Vector3(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y - 179, this.transform.rotation.eulerAngles.z), 0.5f);
        else
            iTween.RotateTo(this.gameObject, new Vector3(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y + 179, this.transform.rotation.eulerAngles.z), 0.5f);

        if (!_blurStarted)
        {
            _myManager.MainCamera.GetComponent<CameraFilterPack_Blur_Focus>().enabled = true;
            _blurStarted = true;
        }
    }
    #endregion Movement Utilities
    #endregion

    //When player collides with something
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

        //HACK: This is a temp fix until I figure out how to get collision working for Rotatable2DMove
        if (CurrentScene != Scenes.Classroom || CurrentScene != Scenes.Roomclass)
        {
            //Checks to see if the centermost point of collision is on the same side as the player is moving.
            //If so, then the bool associated with hitting the wall in that direction is set to true.
            if (_isWalkingForward && centerColPoint.z > this.transform.position.z + 0.7f)
                _hitWallForward = true;
            if (_isWalkingBack && centerColPoint.z < this.transform.position.z - 0.7f)
                _hitWallBack = true;
            if (_isWalkingLeft && centerColPoint.x < this.transform.position.x - 0.7f)
                _hitWallLeft = true;
            if (_isWalkingRight && centerColPoint.x > this.transform.position.x + 0.7f)
                _hitWallRight = true;
        }
    }

    //When the player stops colliding with something
    void OnCollisionExit(Collision col)
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

        if (centerColPoint.x > this.transform.position.x + 0.4f && _hitWallRight)
            _hitWallRight = false;
        else if (centerColPoint.x < this.transform.position.x - 0.4f && _hitWallLeft)
            _hitWallLeft = false;

        if (centerColPoint.y > this.transform.position.z + 0.4f && _hitWallForward)
            _hitWallForward = false;
        else if (centerColPoint.z < this.transform.position.z - 0.4f && _hitWallBack)
            _hitWallBack = false;
    }

    void OnConversationStart(Transform actor)
    {
        //In most situations we want the player's sprite to go invisible during convos. This does that as long as DontHideSprite is false
        if (!DontHideSprite)
        {
            SarylynSprite.enabled = false;
            SanomeSprite.enabled = false;
        }

        //The time cloud is always enabled after a convo
        TimerCloud.GetComponent<VisualTimer>().ToggleTimerReminder(false);
    }

    void OnConversationEnd(Transform actor)
    {
        //Once the convo is over the appropriate sprite is turned back on
        if (isSarylyn)
        {
            SarylynSprite.enabled = true;
            SanomeSprite.enabled = false;
        }
        else
        {
            SanomeSprite.enabled = true;
            SarylynSprite.enabled = false;
        }

        //The time cloud is always enabled after a convo
        TimerCloud.GetComponent<VisualTimer>().ToggleTimerReminder(true);

        //For Win Condition true
        //Right now we don't have a pop up for win conditions but when we do we can use this
        //for (int i = 0; i < DialogueManager.MasterDatabase.variables.Count; i++)
        //{
        //    if (DialogueManager.MasterDatabase.variables[i].fields[2].value == "WinCondition")
        //    {
        //        if (DialogueLua.GetVariable(DialogueManager.MasterDatabase.variables[i].Name).AsBool == true)
        //        {
        //            _myManager.gameObject.GetComponent<PopupManager>().ShowPopup();
        //            break;
        //        }
        //    }
        //}
    }
}