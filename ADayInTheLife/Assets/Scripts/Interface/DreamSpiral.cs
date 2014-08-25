using UnityEngine;
using System.Collections;

public class DreamSpiral : MonoBehaviour
{
    private RaycastHit _hit;
    private GameObject _dreamSpiral,
                   _startCloud;
	private AudioSource _currentAudioSource,
					  _secondSFX;
    private Material _startCloudActive,
                     _startCloudInactive;
    private bool _canMove = true;
    public static bool startTimerOnce = false;

    void Awake()
    {
		
        _startCloudActive = Resources.Load("Interface/DreamSpiral/Materials/WakeUpCloudActive") as Material;
		_startCloudInactive = Resources.Load("Interface/DreamSpiral/Materials/WakeUpCloudInactive") as Material;

        _dreamSpiral = GameObject.Find("DreamSpiral");
        _startCloud = GameObject.Find("WakeUpCloud");

    }

    // Use this for initialization
    void Start()
    {
        _startCloud.renderer.material = _startCloudInactive;
		_secondSFX = GameObject.FindGameObjectWithTag("SecondSFX").GetComponent<AudioSource>();
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
                case "WakeUpCloud":
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
					_startCloud.renderer.material = _startCloudInactive;
					if(_currentAudioSource != null)
						if(_currentAudioSource.isPlaying)
							_currentAudioSource.Stop();
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
        //_canMove = false;

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