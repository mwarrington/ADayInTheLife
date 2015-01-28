using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class CameraManager : MonoBehaviour
{
    //private GameManager _myManager;
    private GameObject _player;
    private Vector3 _startPoint,
                    _endPoint;
    private float _playerPos,
                  _endPos,
                  _startTime,
                  _journeyLength;
    private bool _lerpStarted = false;

    public float PlayerPosOffset;
    public bool ZoomingIn;

    void Start()
    {
        //_myManager = FindObjectOfType<GameManager>();
        _player = FindObjectOfType<PlayerScript>().gameObject;
        _endPos = -55f;
        _startPoint = new Vector3(-3.853245f, 5.765692f, -50.30481f);
        _endPoint = new Vector3(-2.861506f, 2.752161f, -43.38896f);
        _startTime = Time.time;
        _journeyLength = Vector3.Distance(_startPoint, _endPoint);
    }

    void Update()
    {
        CameraControl();
    }

    private void CameraControl()
    {
        switch (FindObjectOfType<PlayerScript>().CurrentScene)
        {
            case Scenes.Cafeteria:
                _playerPos = FindObjectOfType<PlayerScript>().transform.position.x;

                if (_playerPos > _endPos)
                {
                    this.transform.position = new Vector3(_playerPos + PlayerPosOffset, this.transform.position.y, this.transform.position.z);
                }
                break;
            case Scenes.SecCamTemp:
                this.transform.LookAt(_player.transform);
                break;
            case Scenes.ConsularOffice:
                if (ZoomingIn)
                {
                    if (this.transform.position == _endPoint && _lerpStarted == true)
                    {
                        HintManager myHintManager = FindObjectOfType<HintManager>();
                        this.transform.position = _startPoint;
                        _lerpStarted = false;
                        ZoomingIn = false;
                        myHintManager.DeskCurtains.SampleAnimation(myHintManager.DeskCurtains.GetComponent<Animation>().clip, 0);
                        DialogueManager.StartConversation(FindObjectOfType<Consular>().GetComponent<ConversationTrigger>().conversation);

                        break;
                    }
                    float distCovered = (Time.time - _startTime) * 5f;
                    float fracJourney = distCovered / _journeyLength;
                    this.transform.position = Vector3.Lerp(_startPoint, _endPoint, fracJourney);
                    _lerpStarted = true;
                }
                else
                {
                    _startTime = Time.time;
                }
                break;
            default:
                Debug.Log("There shouldn't be a CameraManager in this scene");
                break;
        }
    }
}