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
    private List<TypewriterEffect> _typewriterEffects = new List<TypewriterEffect>();
    private List<FadeEffect> _fadeEffects = new List<FadeEffect>();
    private List<SlideEffect> _slideEffects = new List<SlideEffect>();
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

        DialogSystemEffectInitialization();

        for(int i = 0; i < DialogUiSets.Length; i++)
        {
            DialogUiSets[i].panel.gameObject.SetActive(false);
        }
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
        ToggleDialogSystemEffects();
        Close();
        dialogue = _dialogUiSets[setUpName];
        Open();

        if (_showDialog)
        {
            ShowSubtitle(_myGameManager.CurrentSubtitle);
            HideResponses();
        }
        else
        {
            Response[] currentResponses = new Response[_myGameManager.CurrentResponses.Count];

            for (int i = 0; i < _myGameManager.CurrentResponses.Count; i++)
            {
                currentResponses[i] = _myGameManager.CurrentResponses[i];
            }

            ShowResponses(_myGameManager.CurrentSubtitle, currentResponses, 0);
            HideSubtitle(_myGameManager.CurrentSubtitle);
        }
        Invoke("ToggleDialogSystemEffects", 0.5f);
    }

    private void ToggleDialogSystemEffects()
    {
        foreach (TypewriterEffect te in _typewriterEffects)
        {
            if (te.charactersPerSecond < 1000)
                te.charactersPerSecond = 1000;
            else
                te.charactersPerSecond = 50;
        }

        foreach (FadeEffect fe in _fadeEffects)
        {
            if (fe.fadeInDuration > 0.0001f)
                fe.fadeInDuration = 0.0001f;
            else
                fe.fadeInDuration = 0.3f;
        }

        foreach (SlideEffect se in _slideEffects)
        {
            Debug.Log(se.transform.parent.name);

            if (se.duration > 0.0001f)
                se.duration = 0.0001f;
            else
                se.duration = 0.3f;
        }
    }

    private void DialogSystemEffectInitialization()
    {
        foreach (TypewriterEffect te in this.GetComponentsInChildren<TypewriterEffect>())
        {
            _typewriterEffects.Add(te);
        }

        foreach (FadeEffect fe in this.GetComponentsInChildren<FadeEffect>())
        {
            _fadeEffects.Add(fe);
        }

        foreach (SlideEffect se in this.GetComponentsInChildren<SlideEffect>())
        {
            _slideEffects.Add(se);
        }
    }
}