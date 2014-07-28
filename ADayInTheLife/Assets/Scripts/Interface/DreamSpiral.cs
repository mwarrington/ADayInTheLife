using UnityEngine;
using System.Collections;

public class DreamSpiral : MonoBehaviour
{
    private RaycastHit _hit;
    private GameObject _dreamSpiral,
                   _startCloud,
                   _textCloud;
	private AudioSource _currentAudioSource,
					  _secondSFX,
					  _mainBGM;
    private Material[] _textMatertials = new Material[6];
    private Material _startCloudActive,
                     _startCloudInactive;
    private bool _canMove = true;
    public static bool startTimerOnce = false;

    void Awake()
    {
        _textMatertials[0] = Resources.Load("Interface/MainMenu/Materials/DreamSpiral/DreamSpiralText1") as Material;
        _textMatertials[1] = Resources.Load("Interface/MainMenu/Materials/DreamSpiral/DreamSpiralText2") as Material;
        _textMatertials[2] = Resources.Load("Interface/MainMenu/Materials/DreamSpiral/DreamSpiralText3") as Material;
        _textMatertials[3] = Resources.Load("Interface/MainMenu/Materials/DreamSpiral/DreamSpiralText4") as Material;
        _textMatertials[4] = Resources.Load("Interface/MainMenu/Materials/DreamSpiral/DreamSpiralText5") as Material;
        _textMatertials[5] = Resources.Load("Interface/MainMenu/Materials/DreamSpiral/DreamSpiralText6") as Material;
        _startCloudActive = Resources.Load("Interface/MainMenu/Materials/StartCloudActive") as Material;
        _startCloudInactive = Resources.Load("Interface/MainMenu/Materials/StartCloudInactive") as Material;

        _dreamSpiral = GameObject.Find("DreamSpiral");
        _startCloud = GameObject.Find("StartCloud");
        _textCloud = GameObject.Find("TextCloud");
    }

    // Use this for initialization
    void Start()
    {
        _startCloud.renderer.material = _startCloudInactive;
		_textCloud.renderer.material = _textMatertials[Random.Range(0, 5)];
		_secondSFX = GameObject.FindGameObjectWithTag("SecondSFX").GetComponent<AudioSource>();
		_mainBGM = GameObject.FindGameObjectWithTag("MainBGM").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_canMove)
        {
            _dreamSpiral.transform.Rotate(0f, 1, 0);
        }

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out _hit))
        {
            switch (_hit.collider.name)
            {
                case "StartCloud":
                    _canMove = true;
                    _startCloud.renderer.material = _startCloudActive;
					_currentAudioSource = _hit.collider.gameObject.GetComponent<AudioSource>();
					_currentAudioSource.clip = PrefabLoaderScript.instance.CloudHover;
					if(!_currentAudioSource.isPlaying)
						_currentAudioSource.Play();

                    if (Input.GetMouseButtonDown(0))
                    {
                        startTimerOnce = false;
                        Application.LoadLevel("hallway");
						_secondSFX.clip = PrefabLoaderScript.instance.CloudClick;
						_secondSFX.Play();
                    }
                    break;

                case "DreamSpiral":
                    LogoSpiral();
                    break;
            }
        }
        else
        {
            _canMove = true;
            _startCloud.renderer.material = _startCloudInactive;
			if(_currentAudioSource != null)
				if(_currentAudioSource.clip == PrefabLoaderScript.instance.CloudHover)
					_currentAudioSource.Stop();
        }
    }

    void LogoSpiral()
    {
        _canMove = false;

        if (Input.GetMouseButton(0))
        {
            float x = -Input.GetAxis("Mouse X");
            float y = -Input.GetAxis("Mouse Y");
            float speed = 10;

            _hit.transform.rotation *= Quaternion.AngleAxis(x * speed, Vector3.up);
            _hit.transform.rotation *= Quaternion.AngleAxis(y * speed, Vector3.up);
        }
    }
}