﻿using UnityEngine;
using System.Collections;

public class DreamSpiral : MonoBehaviour
{
    private RaycastHit _hit;
    private GameObject _dreamSpiral;
    //_startCloud;
	private AudioSource _currentAudioSource,
					  _secondSFX;
    //private Material _startCloudActive,
                     //_startCloudInactive;
	private GameManager _myGameManager;
    private TextMesh[] _repCount = new TextMesh[2];
    private bool _canMove = true;
    public static bool startTimerOnce = false;

    void Awake()
    {
		_myGameManager = FindObjectOfType<GameManager>();
        //_startCloudActive = Resources.Load("Art/Textures/Interface/DreamSpiral/Materials/WakeUpCloudActive") as Material;
        //_startCloudInactive = Resources.Load("Art/Textures/Interface/DreamSpiral/Materials/WakeUpCloudInactive") as Material;

        _dreamSpiral = GameObject.Find("DreamSpiral");
        //_startCloud = GameObject.Find("WakeUpCloud");
        //Kind of hacky. Might want to revise...
        if (_myGameManager.DayCount > 0)
        {
            FindObjectOfType<WeLearnByRepetition>().GetComponentsInChildren<TextMesh>()[0].text = (_myGameManager.DayCount - 1).ToString();
            FindObjectOfType<WeLearnByRepetition>().GetComponentsInChildren<TextMesh>()[1].text = (_myGameManager.DayCount - 1).ToString();
        }
    }

    void Start()
    {
       // _startCloud.renderer.material = _startCloudInactive;
		_secondSFX = GameObject.FindGameObjectWithTag("SecondSFX").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_canMove)
        {
            _dreamSpiral.transform.Rotate(0f, 22.2f * Time.deltaTime, 0);
        }

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out _hit))
        {
            switch (_hit.collider.name)
            {
                case "WakeUpCloud":
                    /*_canMove = true;
                    _startCloud.renderer.material = _startCloudActive;
					_currentAudioSource = _hit.collider.gameObject.GetComponent<AudioSource>();
					_currentAudioSource.clip = PrefabLoaderScript.instance.CloudHover;
					if(!_currentAudioSource.isPlaying)
						_currentAudioSource.Play();

                    if (Input.GetMouseButtonDown(0))
                    {
                        startTimerOnce = false;
						Invoke("LoadLevel", 0.1f);
						_secondSFX.clip = PrefabLoaderScript.instance.CloudClick;
						_secondSFX.Play();
                    }*/
                    break;

                case "DreamSpiral":
					//_startCloud.renderer.material = _startCloudInactive;
					if(_currentAudioSource != null)
						if(_currentAudioSource.isPlaying)
							_currentAudioSource.Stop();
                    break;
            }
        }
        else
        {
            /*_canMove = true;
            _startCloud.renderer.material = _startCloudInactive;
			if(_currentAudioSource != null)
				if(_currentAudioSource.clip == PrefabLoaderScript.instance.CloudHover)
					_currentAudioSource.Stop();*/
        }
    }
}