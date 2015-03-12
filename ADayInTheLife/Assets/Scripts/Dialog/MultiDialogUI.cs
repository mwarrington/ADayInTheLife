using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.UnityGUI;

//[System.Serializable]
public class MultiDialogUI : DialogueVisualUI
{
    public UnityDialogueControls[] DialogUiSets;
    public string[] DialogUiNames;

    private Dictionary<string, UnityDialogueControls> _dialogUiSets = new Dictionary<string, UnityDialogueControls>();
    private GameManager _myGameManager;
    private bool _showDialog = true;

    void Start()
    {
        _myGameManager = FindObjectOfType<GameManager>();

        for(int i  = 0; i < DialogUiSets.Length; i++)
        {
            _dialogUiSets.Add(DialogUiNames[i], DialogUiSets[i]);
        }

        dialogue = DialogUiSets[0];
    }

    public override void ShowResponses(Subtitle subtitle, Response[] responses, float timeout)
    {
        base.ShowResponses(subtitle, responses, timeout);

        _showDialog = false;

        _myGameManager.CurrentResponses.Clear();

        for (int i = 0; i < responses.Length; i++)
        {
            _myGameManager.CurrentResponses.Add(responses[i]);
        }
    }

    public override void ShowSubtitle(Subtitle subtitle)
    {
        _showDialog = true;

        base.ShowSubtitle(subtitle);
    }

    public void ChangeDialog(string setUpName)
    {
        Close();
        dialogue = _dialogUiSets[setUpName];
        Open();
        if (_showDialog)
            ShowSubtitle(_myGameManager.CurrentSubtitle);
        else
        {
            Response[] currentResponses = new Response[_myGameManager.CurrentResponses.Count];

            for (int i = 0; i < _myGameManager.CurrentResponses.Count; i++)
            {
                currentResponses[i] = _myGameManager.CurrentResponses[i];
            }

            ShowResponses(_myGameManager.CurrentSubtitle, currentResponses, 0);
        }
    }
}