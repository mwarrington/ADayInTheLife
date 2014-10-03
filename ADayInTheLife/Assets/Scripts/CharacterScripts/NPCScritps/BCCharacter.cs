using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.UnityGUI;

public class BCCharacter : NPCScript
{
    public GameObject MyThoughtCloud;
    public Transform MySpotLightTransform;
    public bool CCharacter;

    private GameObject _dLights,
                       _mySpotLight;
    private GameObject[] _roomPieces;
    private EmpathicEmoticons _myEmpathicEmoticons;
    private SpriteRenderer _emoticonRenderer;
    private Subtitle _currentLine;
    private List<Subtitle> _viewedLines = new List<Subtitle>();
    private string _currentTopic,
                   _currentConversant;
    private float _timeSpentOnLine;
    private int _myProgress;
    private bool _conversationActive;

    protected override void Start()
    {
        base.Start();
        _myEmpathicEmoticons = myGameManager.GetComponent<EmpathicEmoticons>();
        _emoticonRenderer = MyThoughtCloud.GetComponent<SpriteRenderer>();
        _dLights = GameObject.FindGameObjectWithTag("D-Lights");
        _mySpotLight = GameObject.FindGameObjectWithTag("SpotLight");
        _roomPieces = GameObject.FindGameObjectsWithTag("RoomPiece");

        //HACK: Resets the TalkedToVars to allow for single cycle completion
        //_myProgress = DialogueLua.GetVariable(this.name + "Progress").AsInt;
    }

    protected override void Update()
    {
        base.Update();

        if (_conversationActive)
            LineTimer();
    }

    protected override void OnConversationStart(Transform actor)
    {
        base.OnConversationStart(actor);
        _conversationActive = true;

        if (CCharacter)
            ToggleSpotLight(true);
    }

    protected override void OnConversationLine(Subtitle line)
    {
        base.OnConversationLine(line);

        for (int i = 0; i < line.dialogueEntry.fields.Count; i++)
        {
            if (line.dialogueEntry.fields[i].title == "Description")
            {
                if (line.dialogueEntry.fields[i].value != "")
                {
                    _currentConversant = line.dialogueEntry.fields[1].value;
                    Invoke("ChangeNames", 0.001f);
                }
            }
        }

        LineManager(line);
        int moodIndex = 0;
        for (int i = 0; i < line.dialogueEntry.fields.Count; i++)
        {
            if (line.dialogueEntry.fields[i].title == "Mood")
            {
                moodIndex = i;
                break;
            }
        }

        if (moodIndex != 0 && line.dialogueEntry.fields[moodIndex].value != "")
        {
            _emoticonRenderer.sprite = _myEmpathicEmoticons.SpriteDictionary[line.dialogueEntry.fields[12].value];
            _emoticonRenderer.enabled = true;
            CancelInvoke("RemoveThoughtCloud");
            Invoke("RemoveThoughtCloud", 2);
        }
    }

    private void RemoveThoughtCloud()
    {
        _emoticonRenderer.enabled = false;
    }

    protected override void OnConversationEnd(Transform actor)
    {
        base.OnConversationEnd(actor);
        _conversationActive = false;
        DialogSetup();
        RemoveThoughtCloud();

        if (CCharacter)
            ToggleSpotLight(false);
    }

    protected override void RotateTowardPlayer()
    {
        base.RotateTowardPlayer();
    }

    protected override void DialogSetup()
    {
        //Finds the progress Var
        string progressVarName = "";
        for (int i = 0; i < MyDatabase.variables.Count; i++)
        {
            if (MyDatabase.variables[i].fields[2].value == "CharacterProgress")
                progressVarName = MyDatabase.variables[i].fields[0].value;
        }

        //HACK:
        //Resets the TalkedTo vars for test purposes
        //This alows you to complete a level in a single day
        if (_myProgress < DialogueLua.GetVariable(progressVarName).AsInt)
        {
            DialogueLua.SetVariable("TalkedTo" + this.name, false);
            _myProgress = DialogueLua.GetVariable(progressVarName).AsInt;
        }

        //Sets the Conversation to load
        if (PlayerSpacificDialog && myGameManager.IsSarylyn)
            dialogString = this.name.ToString() + "_" + DialogueLua.GetVariable(progressVarName).AsInt + "_Sarylyn";
        else if (PlayerSpacificDialog && !myGameManager.IsSarylyn)
            dialogString = this.name.ToString() + "_" + DialogueLua.GetVariable(progressVarName).AsInt + "_Sanome";
        else
            dialogString = this.name.ToString() + "_" + DialogueLua.GetVariable(progressVarName).AsInt;

        base.DialogSetup();
    }

    private void ToggleSpotLight(bool val)
    {
        for (int i = 0; i < _roomPieces.Length; i++)
        {
            Renderer renderer = _roomPieces[i].GetComponent<Renderer>();

            if (renderer.material != null)
            {
                if (val == true)
                    renderer.material.color = new Color(renderer.material.color.r / 3, renderer.material.color.g / 3, renderer.material.color.b / 3);
                else
                    renderer.material.color = new Color(renderer.material.color.r * 3, renderer.material.color.g * 3, renderer.material.color.b * 3);
            }
        }
        _dLights.SetActive(!val);
        _mySpotLight.transform.position = MySpotLightTransform.position;
        _mySpotLight.SetActive(val);
    }

    private void LineTimer()
    {
        _timeSpentOnLine += Time.deltaTime;
        DialogueLua.SetVariable(this.name + "Timer", (int)_timeSpentOnLine);
    }

    private void LineManager(Subtitle myLine)
    {
        //This sets the current line if it is initally null
        if (_currentLine == null)
            _currentLine = myLine;

        //This keeps track of the dialog lines that have been seen
        foreach (Subtitle s in _viewedLines)
        {
            if (myLine == s)
            {
                _timeSpentOnLine = 50;
                return;
            }
        }
        _viewedLines.Add(myLine);

        //This first checks to see if the dialog line has "Topic" in the description
        //It then checks to see if the topic is the same as it's been if not, it resets the timer
        if (myLine.dialogueEntry.fields[2].value != "")
        {
            if (myLine.dialogueEntry.fields[2].value != _currentTopic)
            {
                _currentTopic = myLine.dialogueEntry.fields[2].value;
                _currentLine = myLine;
                _timeSpentOnLine = 0;
                return;
            }
            else
                return;
        }

        //This checks to see if the line is the same as the last
        if (_currentLine != myLine)// && myLine)
        {
            _currentLine = myLine;
            _timeSpentOnLine = 0;
        }
    }

    private void ChangeNames()
    {
        for (int i = 0; i < FindObjectOfType<DialogUINameHandler>().DisplayNames.Length; i++)
        {
            FindObjectOfType<DialogUINameHandler>().DisplayNames[i].text = _currentConversant;
        }
    }
}