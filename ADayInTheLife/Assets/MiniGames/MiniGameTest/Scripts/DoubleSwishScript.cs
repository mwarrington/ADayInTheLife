using UnityEngine;
using System.Collections;

//This script handles the greatest and best game ever designed or created.
//It is both theoretically and practically the greatest game of all time.
public class DoubleSwishScript : MonoBehaviour
{
    private GUIText _scoreBoard;
    private GameManager _myGameManager;
    private int _swishCount = 0;
    private bool _winMode = false;

    private void Start()
    {
        _myGameManager = FindObjectOfType<GameManager>();
        _scoreBoard = GameObject.Find("ScoreBoard").GetComponent<GUIText>();
        _myGameManager.LastLevelLoaded = Application.loadedLevelName;
    }

    private void Update()
    {
        if((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Mouse0)) && !animation.isPlaying && !_winMode)
        {
            animation.Play();
            Invoke("CountIt", 0.75f);
            Invoke("CountIt", 1f);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("Labrary");
        }
    }

    private void CountIt()
    {
        _swishCount++;
    }

    private void OnGUI()
    {
        if (_swishCount < 10)
            _scoreBoard.text = "0" + _swishCount;
        else if (_swishCount < 100)
            _scoreBoard.text = "" + _swishCount;
        else
        {
            _scoreBoard.text = "You Win!";
            _winMode = true;
            Invoke("ResetGame", 3);
        }
    }

    private void ResetGame()
    {
        _swishCount = 0;
        _winMode = false;
        CancelInvoke();
    }
}
