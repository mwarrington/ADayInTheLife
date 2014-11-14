using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class DayEndManager : MonoBehaviour
{
    private GameManager _myGameManager;
    private PlayerScript _player;
    private SpriteRenderer _fadeMask;

    void Start()
    {
        _myGameManager = FindObjectOfType<GameManager>();
        _player = FindObjectOfType<PlayerScript>();
    }
    
    void Update()
    {
        if (_player != null)
            DayEnd();
    }

    //This method handles the strange things that happen at the end of a day.
    private void DayEnd()
    {
        //These things happen no matter what
        if (_myGameManager.Timer <= 60 && _myGameManager.MainBGM.pitch > 0.9f)
        {
            _myGameManager.MainBGM.pitch = 0.9f;
        }
        if (_myGameManager.Timer <= 50 && !_myGameManager.Countdown30.isPlaying)
        {
            _myGameManager.MainBGM.pitch = 0.85f;
            _myGameManager.MainBGM.volume = 0.7f;
            _myGameManager.Countdown30.Play();
            _myGameManager.Countdown30.volume = 0.3f;
        }
        if (_myGameManager.Timer <= 40 && _myGameManager.MainBGM.pitch > 0.8f)
        {
            _myGameManager.MainBGM.pitch = 0.8f;
            _myGameManager.MainBGM.volume = 0.65f;
            _myGameManager.Countdown30.volume = 0.5f;
        }
        if (_myGameManager.Timer <= 20 && _myGameManager.MainBGM.pitch > 0.7f)
        {
            _myGameManager.MainBGM.pitch = 0.7f;
            _myGameManager.MainBGM.volume = 0.3f;
            _myGameManager.Countdown30.volume = 0.75f;
        }
        if (_myGameManager.Timer <= 10 && !_myGameManager.Countdown10.isPlaying)
        {
            _myGameManager.MainBGM.pitch = 0.5f;
            _myGameManager.MainBGM.volume = 0.25f;
            _player.ConfuseMovement();
            _myGameManager.Countdown10.Play();
        }
        //Area specific changes
        switch (Application.loadedLevelName)
        {
            case "Hallway":
                if (_myGameManager.Timer <= 30 && _myGameManager.MainBGM.pitch > 0.75f)
                {
                    _myGameManager.MainBGM.pitch = 0.75f;
                    _myGameManager.MainBGM.volume = 0.5f;
                    _myGameManager.Countdown30.volume = 0.6f;
                    GameObject[] _lockerSearch = GameObject.FindGameObjectsWithTag("locker30");
                    foreach (GameObject _locker in _lockerSearch)
                    {
                        StartCoroutine(StaggeredAnimationPlay(_locker.GetComponent<Animation>(), 0.7f));
                    }
                }

                if (_myGameManager.Timer <= 10 && _myGameManager.Timer > 9.9)
                {
                    GameObject[] _lockerSearch = GameObject.FindGameObjectsWithTag("locker30");
                    foreach (GameObject _locker in _lockerSearch)
                    {
                        _locker.GetComponent<Animation>().Stop();
                        _locker.collider.enabled = true;
                        _locker.rigidbody.AddForce(1f, 1f, 1f);
                    }
                }
                break;
            case "Labrary":
                if (_myGameManager.Timer <= 30 && _myGameManager.MainBGM.pitch > 0.75f)
                {
                    _myGameManager.MainBGM.pitch = 0.75f;
                    _myGameManager.MainBGM.volume = 0.5f;
                    _myGameManager.Countdown30.volume = 0.6f;
                    GameObject[] _computerSearch = GameObject.FindGameObjectsWithTag("computer60");
                    foreach (GameObject _computer in _computerSearch)
                    {
                        StartCoroutine(StaggeredAnimationPlay(_computer.GetComponent<Animation>(), 1f));
                    }
                }
                break;
            case "Classroom":
                if (_myGameManager.Timer <= 30 && _myGameManager.MainBGM.pitch > 0.75f)
                {
                    _myGameManager.MainBGM.pitch = 0.75f;
                    _myGameManager.MainBGM.volume = 0.5f;
                    _myGameManager.Countdown30.volume = 0.6f;
                    GameObject[] _deskSearch = GameObject.FindGameObjectsWithTag("desk");
                    foreach (GameObject _desk in _deskSearch)
                    {
                        _desk.rigidbody.useGravity = true;
                        _desk.rigidbody.isKinematic = false;
                    }
                }
                break;
            case "Roomclass":
                if (_myGameManager.Timer <= 30 && _myGameManager.MainBGM.pitch > 0.75f)
                {
                    _myGameManager.MainBGM.pitch = 0.75f;
                    _myGameManager.MainBGM.volume = 0.5f;
                    _myGameManager.Countdown30.volume = 0.6f;
                    GameObject[] _deskSearch = GameObject.FindGameObjectsWithTag("desk");
                    foreach (GameObject _desk in _deskSearch)
                    {
                        _desk.rigidbody.isKinematic = false;
                        _desk.rigidbody.AddForce(0, 100, 0);
                    }
                }
                break;
            case "GreenWorld":
                if (_myGameManager.Timer <= 30 && _myGameManager.MainBGM.pitch > 0.75f)
                {
                    _myGameManager.MainBGM.pitch = 0.75f;
                    _myGameManager.MainBGM.volume = 0.5f;
                    _myGameManager.Countdown30.volume = 0.6f;
                    GameObject[] animationObjects = GameObject.FindGameObjectsWithTag("SetPiece");
                    List<Animation> animations = new List<Animation>();

                    for (int i = 0; i < animationObjects.Length; i++)
                    {
                        animations.Add(animationObjects[i].GetComponent<Animation>());
                    }
                    for (int i = 0; i < animations.Count; i++)
                    {
                        //This way all of the set pieces don't fall at once
                        StartCoroutine(StaggeredAnimationPlay(animations[i], 1f));

                        //This way all of the set pieces fall at once
                        //animations[i].Play();
                    }
                }
                break;
            default:
                //Nothing should happen here
                //Debug.Log("That level doesn't exist...");
                break;
        }
        if (_myGameManager.Timer <= 3)
        {
            DialogueManager.StopConversation();
            _myGameManager.FadingAway = true;
        }
        if (_myGameManager.Timer <= 0)
            _myGameManager.LoadNextLevel();
    }

    private IEnumerator StaggeredAnimationPlay(Animation animation, float variance)
    {
        float randFloat = Random.Range(0, variance);
        yield return new WaitForSeconds(randFloat);
        animation.Play();
    }
}
