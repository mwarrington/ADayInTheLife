using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class VisualTimer : MonoBehaviour
{
    protected bool showingGameTimer
    {
        get
        {
            return _showingGameTimer;
        }

        set
        {
            if (value != _showingGameTimer)
            {
                if (value)
                {
                    if (DialogueManager.IsConversationActive)
                        _showingGameTimer = false;
                    else if (isItemActive)
                        _showingGameTimer = false;
                    else
                    {
                        foreach (Renderer r in this.GetComponentsInChildren<Renderer>())
                        {
                            r.enabled = true;
                        }
                    }
                }
                else
                {
                    foreach (Renderer r in this.GetComponentsInChildren<Renderer>())
                    {
                        r.enabled = false;
                    }
                }
                _showingGameTimer = value;
            }
        }
    }
    private bool _showingGameTimer = false;

    protected bool isItemActive
    {
        get
        {
            foreach (ItemInteract ii in GameObject.FindObjectsOfType<ItemInteract>())
            {
                if (ii.ItemActive)
                {
                    _isItemActive = true;
                    return _isItemActive;
                }
                else
                    _isItemActive = false;
            }
            return _isItemActive;
        }
    }
    private bool _isItemActive;

    private GameManager _myGameManager;
    private TextMesh _myTextMesh;
    private int _gameTimer;

    void Start()
    {
        _myGameManager = GameObject.FindObjectOfType<GameManager>();
        _myTextMesh = this.GetComponentInChildren<TextMesh>();

        //This handles turning off the the Timer Cloud renderers
        if (_myGameManager.Timer % 30 > 1)
        {
            showingGameTimer = false;
        }
    }

    void OnGUI()
    {
        if (showingGameTimer)
        {
            GameTimer();
        }
    }

    void Update()
    {
        //This handles when to turn on the Timer Cloud renderers
        if (!showingGameTimer)
        {
            if (_myGameManager.Timer < 61)
            {
                _myGameManager.GameTimerActive = true;
                showingGameTimer = true;
            }
            else if (_myGameManager.Timer % 30 < 0.1f && _myGameManager.HasBeenIntroduced)
            {
                if (!showingGameTimer)
                {
                    _myGameManager.GameTimerActive = true;
                    showingGameTimer = true;
                }
            }
            else if (Input.GetKey(KeyCode.LeftControl) && !DialogueManager.IsConversationActive)
                showingGameTimer = true;
        }
        else if (_myGameManager.Timer % 30 < 25 && _myGameManager.Timer % 30 > 0.1f && showingGameTimer && !Input.GetKey(KeyCode.LeftControl))
            showingGameTimer = false;
    }

    private void GameTimer()
    {
        _gameTimer = (int)_myGameManager.Timer;
        int displaySeconds = Mathf.CeilToInt(_gameTimer) % 60;
        int displayMinutes = Mathf.CeilToInt(_gameTimer) / 60;
        _myTextMesh.text = string.Format("{0:00}:{1:00}", displayMinutes, displaySeconds);
    }
}
