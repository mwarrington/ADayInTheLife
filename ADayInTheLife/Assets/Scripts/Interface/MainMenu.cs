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
    private bool _bounce;
    private CharacterTilt _sanomeTilt,
                          _sarylynTilt;

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
    }

    void LoadResources()
    {
        _startCloudActive = Resources.Load("Interface/MainMenu/Materials/StartCloudActive") as Material;
        _startCloudInactive = Resources.Load("Interface/MainMenu/Materials/StartCloudInactive") as Material;
        _creditsCloudActive = Resources.Load("Interface/MainMenu/Materials/CreditsCloudActive") as Material;
        _creditsCloudInactive = Resources.Load("Interface/MainMenu/Materials/CreditsCloudInactive") as Material;
        _mouseClick = Resources.Load("Interface/MainMenu/Materials/MouseClick") as Material;
        _mouseHover = Resources.Load("Interface/MainMenu/Materials/MouseHover") as Material;
        _sarylynActive = Resources.Load("Interface/MainMenu/Materials/SarylynActive") as Material;
        _sarylynInactive = Resources.Load("Interface/MainMenu/Materials/SarylynInactive") as Material;
        _sanomeActive = Resources.Load("Interface/MainMenu/Materials/SanomeActive") as Material;
        _sanomeInactive = Resources.Load("Interface/MainMenu/Materials/SanomeInactive") as Material;
        _backCloudActive = Resources.Load("Interface/MainMenu/Materials/BackCloudActive") as Material;
        _backCloudInactive = Resources.Load("Interface/MainMenu/Materials/BackCloudInactive") as Material;

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
            LogoSpiral();

            switch (State)
            {
                case MenuState.MAIN:
                    _backCloud.renderer.enabled = false;
                    _backCloud.collider.enabled = false;

                    switch (_hit.collider.name)
                    {
                        case "TopCloud":
                            _hit.collider.renderer.material = _startCloudActive;
                            _bottomCloud.renderer.material = _creditsCloudInactive;
                            _bottomArrow.renderer.enabled = false;
                            _topArrow.renderer.enabled = true;
                            _mouse.renderer.material = _mouseClick;

                            if (Input.GetMouseButtonDown(0))
                            {
                                State = MenuState.NEWGAME;
                            }
                            break;

                        case "BottomCloud":
                            _hit.collider.renderer.material = _creditsCloudActive;
                            _topCloud.renderer.material = _startCloudInactive;
                            _bottomArrow.renderer.enabled = true;
                            _topArrow.renderer.enabled = false;
                            _mouse.renderer.material = _mouseClick;

                            if (Input.GetMouseButtonDown(0))
                            {
                                _bottomCloud.renderer.enabled = false;
                                _bottomArrow.renderer.enabled = false;
                                _topArrow.renderer.enabled = false;
                                _topCloud.renderer.enabled = false;
                                _mouse.renderer.enabled = false;

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
                            _hit.collider.renderer.material = _sarylynActive;
                            _bottomCloud.renderer.material = _sanomeInactive;
                            _bottomArrow.renderer.enabled = false;
                            _topArrow.renderer.enabled = true;
                            _mouse.renderer.material = _mouseClick;

                            _sarylynTilt.Update();

                            if (Input.GetMouseButtonDown(0))
                            {
                                GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().isSarylyn = true;
                                Application.LoadLevel("DreamSpiral");
                            }
                            break;

                        case "BottomCloud":
                            _hit.collider.renderer.material = _sanomeActive;
                            _topCloud.renderer.material = _sarylynInactive;
                            _bottomArrow.renderer.enabled = true;
                            _topArrow.renderer.enabled = false;
                            _mouse.renderer.material = _mouseClick;

                            _sanomeTilt.Update();

                            if (Input.GetMouseButtonDown(0))
							{
								GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().isSarylyn = false;
                                Application.LoadLevel("DreamSpiral");
                            }
                            break;

                        case "BackButton":
                            _backCloud.renderer.material = _backCloudActive;
                            if (Input.GetMouseButtonDown(0))
                            {
                                _topCloud.renderer.material = _startCloudInactive;
                                _bottomCloud.renderer.material = _creditsCloudInactive;
                                State = MenuState.MAIN;
                            }
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
                        _backCloud.renderer.material = _backCloudActive;

                        if (Input.GetMouseButtonDown(0))
                        {
                            _credits.GetComponent<Credits>().enabled = false;
                            _bottomCloud.renderer.enabled = true;
                            _bottomArrow.renderer.enabled = true;
                            _topArrow.renderer.enabled = true;
                            _topCloud.renderer.enabled = true;
                            _mouse.renderer.enabled = true;
                            State = MenuState.MAIN;
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
            _spiralLogo.transform.Rotate(new Vector3(0, 1, 0));

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

            if (Input.GetMouseButton(0))
            {
                float x = -Input.GetAxis("Mouse X");
                float y = -Input.GetAxis("Mouse Y");
                float speed = 10;

                _spiralLogo.transform.rotation *= Quaternion.AngleAxis(x * speed, Vector3.up);
                _spiralLogo.transform.rotation *= Quaternion.AngleAxis(y * speed, Vector3.up);
            }
        }
        else
        {
            _spiralLogo.transform.Rotate(new Vector3(0, 1, 0));
        }
    }

    void ArrowBounce()
    {
        if (!_bounce)
        {
            _topArrow.transform.position += new Vector3(0, .055f, 0);
            _bottomArrow.transform.position += new Vector3(0, .055f, 0);

            if (_topArrow.transform.position.y >= _topArrowOrgPos.y + .5)
            {
                _bounce = true;
            }
        }
        else
        {
            _topArrow.transform.position -= new Vector3(0, .055f, 0);
            _bottomArrow.transform.position -= new Vector3(0, .055f, 0);

            if (_bottomArrow.transform.position.y <= _bottomArrowOrgPos.y - .5)
            {
                _bounce = false;
            }
        }
    }
}