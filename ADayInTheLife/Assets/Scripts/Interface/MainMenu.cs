using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    private enum MenuState
    {
        MAIN,
        NEWGAME,
        CREDITS,
        EXIT,
        OPTIONS
    };
    private MenuState State;
    private Quadrant _initialQuad = Quadrant.TopRight;

    private RaycastHit _hit;
    private Material _startCloudActive,
                     _startCloudInactive,
                     _creditsCloudActive,
                     _creditsCloudInactive,
                     _mouseClick,
                     _mouseHover,
                     _sarylynActive,
                     _sarylynInactive,
                     _sanomeActive,
                     _sanomeInactive,
                     _backCloudActive,
                     _backCloudInactive;
    private GameObject _topArrow,
                       _bottomArrow,
                       _mouse,
                       _topCloud,
                       _bottomCloud,
                       _credits,
                       _backCloud,
                       _sanomePortrait,
                       _sarylynPortrait,
                       _spiralLogo,
                       _spiralLogoText;

    private Vector3 _topArrowOrgPos,
                    _bottomArrowOrgPos;
    private Vector2 _initialClickVec = new Vector2();
    private bool _bounce,
				 _startFade = false,
                 _clickedOnSpiral;
	private float _alpha = 0,
                  _initialAngle,
                  _logoSpinSpeed = 50;
    private CharacterTilt _sanomeTilt,
                          _sarylynTilt;
	private AudioSource _currentAudioSource,
						_secondSFX,
						_mainBGM;
	private SpriteRenderer _fadeMask;

    public bool CreditsDone
    {
        get
        {
            return creditsDone;
        } 
        set
        {
            if (value)
            {
                _credits.GetComponent<Credits>().enabled = false;
                _bottomCloud.renderer.enabled = true;
                _bottomArrow.renderer.enabled = true;
                _topArrow.renderer.enabled = true;
                _topCloud.renderer.enabled = true;
                _mouse.renderer.enabled = true;
                _secondSFX.clip = PrefabLoaderScript.instance.CloudClick;
                _secondSFX.Play();
                State = MenuState.MAIN;
                CreditsDone = false;
            }

            creditsDone = value;
        }
    }
    protected bool creditsDone;

    void Awake()
    {
        LoadResources();
    }

    void Start()
    {
        _topArrowOrgPos = _topArrow.transform.position;
        _bottomArrowOrgPos = _bottomArrow.transform.position;
        _sarylynTilt = new CharacterTilt(_sarylynPortrait, false);
        _sanomeTilt = new CharacterTilt(_sanomePortrait, true);
		_secondSFX = GameObject.FindGameObjectWithTag("SecondSFX").GetComponent<AudioSource>();
		_fadeMask = GameObject.FindGameObjectWithTag("FadeMask").GetComponent<SpriteRenderer>();
		_mainBGM = GameObject.FindGameObjectWithTag("MainBGM").GetComponent<AudioSource>();
    }

    void LoadResources()
    {
        _startCloudActive = Resources.Load("Art/Textures/Interface/MainMenu/Materials/StartCloudActive") as Material;
        _startCloudInactive = Resources.Load("Art/Textures/Interface/MainMenu/Materials/StartCloudInactive") as Material;
        _creditsCloudActive = Resources.Load("Art/Textures/Interface/MainMenu/Materials/CreditsCloudActive") as Material;
        _creditsCloudInactive = Resources.Load("Art/Textures/Interface/MainMenu/Materials/CreditsCloudInactive") as Material;
        _mouseClick = Resources.Load("Art/Textures/Interface/MainMenu/Materials/MouseClick") as Material;
        _mouseHover = Resources.Load("Art/Textures/Interface/MainMenu/Materials/MouseHover") as Material;
        _sarylynActive = Resources.Load("Art/Textures/Interface/MainMenu/Materials/SarylynActive") as Material;
        _sarylynInactive = Resources.Load("Art/Textures/Interface/MainMenu/Materials/SarylynInactive") as Material;
        _sanomeActive = Resources.Load("Art/Textures/Interface/MainMenu/Materials/SanomeActive") as Material;
        _sanomeInactive = Resources.Load("Art/Textures/Interface/MainMenu/Materials/SanomeInactive") as Material;
        _backCloudActive = Resources.Load("Art/Textures/Interface/MainMenu/Materials/BackCloudActive") as Material;
        _backCloudInactive = Resources.Load("Art/Textures/Interface/MainMenu/Materials/BackCloudInactive") as Material;

        _topArrow = GameObject.Find("TopArrow");
        _bottomArrow = GameObject.Find("BottomArrow");
        _mouse = GameObject.Find("Mouse");
        _topCloud = GameObject.Find("TopCloud");
        _bottomCloud = GameObject.Find("BottomCloud");
        _credits = GameObject.Find("CreditsManager");
        _backCloud = GameObject.Find("BackButton");
        _sarylynPortrait = GameObject.Find("SarylynPortrait");
        _sanomePortrait = GameObject.Find("SanomePortrait");
        _spiralLogo = GameObject.Find("LogoSpiral");
        _spiralLogoText = GameObject.Find("LogoText");
    }

    void Update()
    {
        ArrowBounce();
        Menu();
		if(_startFade)
			Fade();

        LogoSpiral();
    }

    void Menu()
    {
        if (State == MenuState.CREDITS)
        {
            _spiralLogo.transform.position = new Vector3(-.85f, .54f, 1);
            _spiralLogoText.transform.position = new Vector3(0, .68f, 0);
        }
        else
        {
             _spiralLogo.transform.position = new Vector3(-6.59f, .54f, 1);
             _spiralLogoText.transform.position = new Vector3(-5.98f, .68f, 0);
        }

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out _hit))
        {
            switch (State)
            {
                case MenuState.MAIN:
                    _backCloud.renderer.enabled = false;
                    _backCloud.collider.enabled = false;

                    switch (_hit.collider.name)
                    {
                        case "TopCloud":
                            _spiralLogo.transform.Rotate(new Vector3(0, 0, _logoSpinSpeed * Time.deltaTime));
                            _hit.collider.renderer.material = _startCloudActive;
                            _bottomCloud.renderer.material = _creditsCloudInactive;
                            _bottomArrow.renderer.enabled = false;
                            _topArrow.renderer.enabled = true;
                            _mouse.renderer.material = _mouseClick;
							_currentAudioSource = _hit.collider.gameObject.GetComponent<AudioSource>();
							_currentAudioSource.clip = PrefabLoaderScript.instance.CloudHover;
							if(!_currentAudioSource.isPlaying)
								_currentAudioSource.Play();

                            if (Input.GetMouseButtonDown(0))
                            {
								_secondSFX.clip = PrefabLoaderScript.instance.CloudClick;
								_secondSFX.Play();
                                State = MenuState.NEWGAME;
                            }
                            break;

                        case "BottomCloud":
                            _spiralLogo.transform.Rotate(new Vector3(0, 0, _logoSpinSpeed * Time.deltaTime));
                            _hit.collider.renderer.material = _creditsCloudActive;
                            _topCloud.renderer.material = _startCloudInactive;
                            _bottomArrow.renderer.enabled = true;
                            _topArrow.renderer.enabled = false;
                            _mouse.renderer.material = _mouseClick;
							_currentAudioSource = _hit.collider.gameObject.GetComponent<AudioSource>();
							_currentAudioSource.clip = PrefabLoaderScript.instance.CloudHover;
							if(!_currentAudioSource.isPlaying)
								_currentAudioSource.Play();

                            if (Input.GetMouseButtonDown(0))
                            {
                                _bottomCloud.renderer.enabled = false;
                                _bottomArrow.renderer.enabled = false;
                                _topArrow.renderer.enabled = false;
                                _topCloud.renderer.enabled = false;
                                _mouse.renderer.enabled = false;
								_secondSFX.clip = PrefabLoaderScript.instance.CloudClick;
								_secondSFX.Play();
                                _currentAudioSource.Stop();

                                State = MenuState.CREDITS;
                            }
                            break;
                    }
                    break;
                case MenuState.NEWGAME:
                    _backCloud.renderer.enabled = true;
                    _backCloud.collider.enabled = true;
                    _sarylynPortrait.renderer.enabled = true;
                    _sanomePortrait.renderer.enabled = true;

                    switch (_hit.collider.name)
                    {
                        case "TopCloud":
                            _spiralLogo.transform.Rotate(new Vector3(0, 0, _logoSpinSpeed * Time.deltaTime));
                            _hit.collider.renderer.material = _sarylynActive;
                            _bottomCloud.renderer.material = _sanomeInactive;
                            _bottomArrow.renderer.enabled = false;
                            _topArrow.renderer.enabled = true;
                            _mouse.renderer.material = _mouseClick;
							_currentAudioSource = _hit.collider.gameObject.GetComponent<AudioSource>();
							_currentAudioSource.clip = PrefabLoaderScript.instance.CloudHover;
							if(!_currentAudioSource.isPlaying)
								_currentAudioSource.Play();

                            _sarylynTilt.Update();

                            if (Input.GetMouseButtonDown(0))
                            {
                                GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().IsSarylyn = true;
								_secondSFX.clip = PrefabLoaderScript.instance.PlayerSelect1;
								_secondSFX.Play();
								_startFade = true;
								Invoke("ProceedToGame",3);
								_currentAudioSource.Stop();
								State = MenuState.EXIT;
                            }
                            break;

                        case "BottomCloud":
                            _spiralLogo.transform.Rotate(new Vector3(0, 0, _logoSpinSpeed * Time.deltaTime));
                            _hit.collider.renderer.material = _sanomeActive;
                            _topCloud.renderer.material = _sarylynInactive;
                            _bottomArrow.renderer.enabled = true;
                            _topArrow.renderer.enabled = false;
                            _mouse.renderer.material = _mouseClick;
							_currentAudioSource = _hit.collider.gameObject.GetComponent<AudioSource>();
							_currentAudioSource.clip = PrefabLoaderScript.instance.CloudHover;
							if(!_currentAudioSource.isPlaying)
								_currentAudioSource.Play();

                            _sanomeTilt.Update();

                            if (Input.GetMouseButtonDown(0))
							{
								GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().IsSarylyn = false;
								_secondSFX.clip = PrefabLoaderScript.instance.PlayerSelect2;
								_secondSFX.Play();
								_startFade = true;
								Invoke("ProceedToGame",3);
								_currentAudioSource.Stop();
								State = MenuState.EXIT;
                            }
                            break;

                        case "BackButton":
                            _spiralLogo.transform.Rotate(new Vector3(0, 0, _logoSpinSpeed * Time.deltaTime));
                            _backCloud.renderer.material = _backCloudActive;
							_currentAudioSource = _hit.collider.gameObject.GetComponent<AudioSource>();
							_currentAudioSource.clip = PrefabLoaderScript.instance.CloudHover;
							if(!_currentAudioSource.isPlaying)
								_currentAudioSource.Play();

                            _sarylynTilt.Update();
                            _sanomeTilt.Update();

                            if (Input.GetMouseButtonDown(0))
                            {
                                _topCloud.renderer.material = _startCloudInactive;
                                _bottomCloud.renderer.material = _creditsCloudInactive;
								_secondSFX.clip = PrefabLoaderScript.instance.CloudClick;
								_secondSFX.Play();
                                State = MenuState.MAIN;
                            }
                            break;
                        case "LogoSpiral":
                            _sarylynTilt.Update();
                            _sanomeTilt.Update();
                            break;
                    }
                    break;
                case MenuState.CREDITS:
                    _backCloud.renderer.enabled = true;
                    _backCloud.collider.enabled = true;
                    _sarylynPortrait.renderer.enabled = false;
                    _sanomePortrait.renderer.enabled = false;
                    _credits.GetComponent<Credits>().enabled = true;

                    if (_hit.collider.name == "BackButton")
                    {
                        _spiralLogo.transform.Rotate(new Vector3(0, 0, _logoSpinSpeed * Time.deltaTime));
                        _backCloud.renderer.material = _backCloudActive;
						_currentAudioSource = _hit.collider.gameObject.GetComponent<AudioSource>();
						_currentAudioSource.clip = PrefabLoaderScript.instance.CloudHover;
						if(!_currentAudioSource.isPlaying)
							_currentAudioSource.Play();

                        if (Input.GetMouseButtonDown(0))
                        {
                            _credits.GetComponent<Credits>().enabled = false;
                            _bottomCloud.renderer.enabled = true;
                            _bottomArrow.renderer.enabled = true;
                            _topArrow.renderer.enabled = true;
                            _topCloud.renderer.enabled = true;
                            _mouse.renderer.enabled = true;
							_secondSFX.clip = PrefabLoaderScript.instance.CloudClick;
							_secondSFX.Play();
                            State = MenuState.MAIN;
                            CreditsDone = false;
                        }
                    }
                    break;
                case MenuState.EXIT:
                    break;
                case MenuState.OPTIONS:
                    break;
                default:
                    break;
            }
        }
        else
        {
            _mouse.renderer.material = _mouseHover;
            _spiralLogo.transform.Rotate(new Vector3(0, 0, _logoSpinSpeed * Time.deltaTime));
			if(_currentAudioSource != null)
				if(_currentAudioSource.clip == PrefabLoaderScript.instance.CloudHover)
					_currentAudioSource.Stop();

            switch (State)
            {
                case MenuState.MAIN:
                    _backCloud.renderer.material = _backCloudInactive;
                    _topCloud.renderer.material = _startCloudInactive;
                    _bottomCloud.renderer.material = _creditsCloudInactive;
                    _sanomePortrait.renderer.enabled = false;
                    _sarylynPortrait.renderer.enabled = false;
                    _topArrow.renderer.enabled = true;
                    _bottomArrow.renderer.enabled = true;
                    break;
                case MenuState.NEWGAME:
                    _backCloud.renderer.material = _backCloudInactive;
                    _topCloud.renderer.material = _sarylynInactive;
                    _bottomCloud.renderer.material = _sanomeInactive;
                    _sanomeTilt.Update();
                    _sarylynTilt.Update();
                    _topArrow.renderer.enabled = true;
                    _bottomArrow.renderer.enabled = true;
                    break;
                case MenuState.CREDITS:
                    _backCloud.renderer.material = _backCloudInactive;
                    break;
                case MenuState.EXIT:
                    break;
                case MenuState.OPTIONS:
                    break;
            }
        }
    }

    void LogoSpiral()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out _hit))
        {
            if (_hit.collider.name == "LogoSpiral")
            {
                switch (State)
                {
                    case MenuState.MAIN:
                        _topCloud.renderer.material = _startCloudInactive;
                        _bottomCloud.renderer.material = _creditsCloudInactive;
                        _topArrow.renderer.enabled = true;
                        _bottomArrow.renderer.enabled = true;
                        _mouse.renderer.material = _mouseHover;
                        break;
                    case MenuState.NEWGAME:
                        _topCloud.renderer.material = _sarylynInactive;
                        _bottomCloud.renderer.material = _sanomeInactive;
                        _topArrow.renderer.enabled = true;
                        _bottomArrow.renderer.enabled = true;
                        _mouse.renderer.material = _mouseHover;
                        break;
                    case MenuState.CREDITS:
                        break;
                    case MenuState.EXIT:
                        break;
                    case MenuState.OPTIONS:
                        break;
                    default:
                        break;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > _spiralLogo.transform.position.x && Camera.main.ScreenToWorldPoint(Input.mousePosition).y > _spiralLogo.transform.position.y)
                    {
                        _initialQuad = Quadrant.TopRight;
                        _initialClickVec = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - _spiralLogo.transform.position.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - _spiralLogo.transform.position.y);
                    }
                    else if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < _spiralLogo.transform.position.x && Camera.main.ScreenToWorldPoint(Input.mousePosition).y > _spiralLogo.transform.position.y)
                    {
                        _initialQuad = Quadrant.TopLeft;
                        _initialClickVec = new Vector2(_spiralLogo.transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - _spiralLogo.transform.position.y);
                    }
                    else if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > _spiralLogo.transform.position.x && Camera.main.ScreenToWorldPoint(Input.mousePosition).y < _spiralLogo.transform.position.y)
                    {
                        _initialQuad = Quadrant.BottomRight;
                        _initialClickVec = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - _spiralLogo.transform.position.x, _spiralLogo.transform.position.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
                    }
                    else if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < _spiralLogo.transform.position.x && Camera.main.ScreenToWorldPoint(Input.mousePosition).y < _spiralLogo.transform.position.y)
                    {
                        _initialQuad = Quadrant.BottomLeft;
                        _initialClickVec = new Vector2(_spiralLogo.transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x, _spiralLogo.transform.position.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
                    }

                    _initialAngle = _spiralLogo.transform.rotation.eulerAngles.z;
                    _clickedOnSpiral = true;
                }
            }
        }

        Vector2 currentClickVec;
        if (_clickedOnSpiral)
        {
            if (_initialQuad == Quadrant.TopRight)
                currentClickVec = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - _spiralLogo.transform.position.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - _spiralLogo.transform.position.y);
            else if (_initialQuad == Quadrant.TopLeft)
                currentClickVec = new Vector2(_spiralLogo.transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - _spiralLogo.transform.position.y);
            else if (_initialQuad == Quadrant.BottomRight)
                currentClickVec = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - _spiralLogo.transform.position.x, _spiralLogo.transform.position.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            else if (_initialQuad == Quadrant.BottomLeft)
                currentClickVec = new Vector2(_spiralLogo.transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x, _spiralLogo.transform.position.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            else
                currentClickVec = Vector2.zero;

            Quadrant currentQuad = Quadrant.TopRight;

            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > _spiralLogo.transform.position.x && Camera.main.ScreenToWorldPoint(Input.mousePosition).y > _spiralLogo.transform.position.y)
            {
                currentQuad = Quadrant.TopRight;
            }
            else if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < _spiralLogo.transform.position.x && Camera.main.ScreenToWorldPoint(Input.mousePosition).y > _spiralLogo.transform.position.y)
            {
                currentQuad = Quadrant.TopLeft;
            }
            else if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > _spiralLogo.transform.position.x && Camera.main.ScreenToWorldPoint(Input.mousePosition).y < _spiralLogo.transform.position.y)
            {
                currentQuad = Quadrant.BottomRight;
            }
            else if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < _spiralLogo.transform.position.x && Camera.main.ScreenToWorldPoint(Input.mousePosition).y < _spiralLogo.transform.position.y)
            {
                currentQuad = Quadrant.BottomLeft;
            }

            float angleInDegrees = (Mathf.Acos((_initialClickVec.x * currentClickVec.x + _initialClickVec.y * currentClickVec.y) / (Mathf.Sqrt(Mathf.Pow(_initialClickVec.x, 2) + Mathf.Pow(_initialClickVec.y, 2)) * Mathf.Sqrt(Mathf.Pow(currentClickVec.x, 2) + Mathf.Pow(currentClickVec.y, 2))))) * (180 / Mathf.PI);
            if (!float.IsNaN(angleInDegrees))
            {
                switch (_initialQuad)
                {
                    case Quadrant.TopRight:
                        if (currentQuad == _initialQuad)
                        {
                            if ((_initialClickVec.x / _initialClickVec.y) > (currentClickVec.x / currentClickVec.y))
                                _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle + angleInDegrees, Vector3.forward);
                            else
                                _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle - angleInDegrees, Vector3.forward);
                        }
                        else if (currentQuad == Quadrant.TopLeft)
                        {
                            _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle + angleInDegrees, Vector3.forward);
                        }
                        else if (currentQuad == Quadrant.BottomRight)
                        {
                            _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle - angleInDegrees, Vector3.forward);
                        }
                        else
                        {
                            if ((_initialClickVec.x / _initialClickVec.y) < (currentClickVec.x / currentClickVec.y))
                                _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle + angleInDegrees, Vector3.forward);
                            else
                                _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle - angleInDegrees, Vector3.forward);
                        }
                        break;
                    case Quadrant.TopLeft:
                        if (currentQuad == _initialQuad)
                        {
                            if ((_initialClickVec.x / _initialClickVec.y) < (currentClickVec.x / currentClickVec.y))
                                _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle + angleInDegrees, Vector3.forward);
                            else
                                _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle - angleInDegrees, Vector3.forward);
                        }
                        else if(currentQuad == Quadrant.BottomLeft)
                        {
                            _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle + angleInDegrees, Vector3.forward);
                        }
                        else if(currentQuad == Quadrant.TopRight)
                        {
                            _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle - angleInDegrees, Vector3.forward);
                        }
                        else
                        {
                            if ((_initialClickVec.x / _initialClickVec.y) > (currentClickVec.x / currentClickVec.y))
                                _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle + angleInDegrees, Vector3.forward);
                            else
                                _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle - angleInDegrees, Vector3.forward);
                        }
                        break;
                    case Quadrant.BottomRight:
                        if (currentQuad == _initialQuad)
                        {
                            if ((_initialClickVec.x / _initialClickVec.y) < (currentClickVec.x / currentClickVec.y))
                                _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle + angleInDegrees, Vector3.forward);
                            else
                                _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle - angleInDegrees, Vector3.forward);
                        }
                        else if (currentQuad == Quadrant.TopRight)
                        {
                            _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle + angleInDegrees, Vector3.forward);
                        }
                        else if (currentQuad == Quadrant.BottomLeft)
                        {
                            _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle - angleInDegrees, Vector3.forward);
                        }
                        else
                        {
                            if ((_initialClickVec.x / _initialClickVec.y) > (currentClickVec.x / currentClickVec.y))
                                _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle + angleInDegrees, Vector3.forward);
                            else
                                _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle - angleInDegrees, Vector3.forward);
                        }
                        break;
                    case Quadrant.BottomLeft:
                        if (currentQuad == _initialQuad)
                        {
                            if ((_initialClickVec.x / _initialClickVec.y) > (currentClickVec.x / currentClickVec.y))
                                _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle + angleInDegrees, Vector3.forward);
                            else
                                _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle - angleInDegrees, Vector3.forward);
                        }
                        else if (currentQuad == Quadrant.BottomRight)
                        {
                            _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle + angleInDegrees, Vector3.forward);
                        }
                        else if (currentQuad == Quadrant.TopLeft)
                        {
                            _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle - angleInDegrees, Vector3.forward);
                        }
                        else
                        {
                            if ((_initialClickVec.x / _initialClickVec.y) < (currentClickVec.x / currentClickVec.y))
                                _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle + angleInDegrees, Vector3.forward);
                            else
                                _spiralLogo.transform.rotation = Quaternion.AngleAxis(_initialAngle - angleInDegrees, Vector3.forward);
                        }
                        break;
                    default:
                        Debug.Log("That Quadrant doesn't exist. I mean there are only four. That's why it's called a QUADrant. Keep this nonsense up and I'll go on a QUAD rant.");
                        break;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
            _clickedOnSpiral = false;
    }

    void ArrowBounce()
    {
        if (!_bounce)
        {
            _topArrow.transform.position += new Vector3(0, 3.5f * Time.deltaTime, 0);
            _bottomArrow.transform.position += new Vector3(0, 3.5f * Time.deltaTime, 0);

            if (_topArrow.transform.position.y >= _topArrowOrgPos.y + .5)
            {
                _bounce = true;
            }
        }
        else
        {
            _topArrow.transform.position -= new Vector3(0, 3.5f * Time.deltaTime, 0);
            _bottomArrow.transform.position -= new Vector3(0, 3.5f * Time.deltaTime, 0);

            if (_bottomArrow.transform.position.y <= _bottomArrowOrgPos.y - .5)
            {
                _bounce = false;
            }
        }
    }

	private void Fade()
	{
		_alpha += Time.deltaTime * 0.35f;
		_mainBGM.volume -= Time.deltaTime * 0.1f;
		_fadeMask.color = new Color(_fadeMask.color.r, _fadeMask.color.g, _fadeMask.color.b, _alpha);
	}

	private void ProceedToGame()
	{
		Application.LoadLevel("DreamSpiral");
	}
}