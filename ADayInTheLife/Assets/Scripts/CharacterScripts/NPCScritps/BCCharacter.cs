using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.UnityGUI;

public class BCCharacter : NPCScript
{
    //Public fields to be set in the inspector
    public Transform MySpotLightTransform;
    public EmpathyTypes MyPastEmpathyType;
    public bool CCharacter,
                HasMultipleThoughtClouds,
                FormerCCharacter;

    //private fields to be set/populated in script
    private Dictionary<string, GameObject> _thoughtCloudDic = new Dictionary<string, GameObject>();
    private GameObject _dLights,
                       _mySpotLight;
    private GameObject[] _roomPieces;
    private EmpathicEmoticons _myEmpathicEmoticons;
    private SpriteRenderer _emoticonRenderer,
                           _lastEmoticonRenderer;
    private Subtitle _currentLine;
    private List<Subtitle> _viewedLines = new List<Subtitle>();
    private string _currentTopic,
                   _currentConversant;
    private float _timeSpentOnLine;
    private int _myProgress = 1;
    private bool _conversationActive;

    //Initializes some fields
    protected override void Start()
    {
        base.Start();
        _myEmpathicEmoticons = myGameManager.GetComponent<EmpathicEmoticons>();
        ThoughtCloudSetUp();
        _dLights = GameObject.FindGameObjectWithTag("D-Lights");
        _mySpotLight = GameObject.FindGameObjectWithTag("SpotLight");
        _roomPieces = GameObject.FindGameObjectsWithTag("RoomPiece");

        //Initializes what animation should be active for the NPC based on this NPCs progress var
        if (this.GetComponentInChildren<Animator>() != null)
        {
            this.GetComponentInChildren<Animator>().SetInteger("Progress", DialogueLua.GetVariable(this.name + "Progress").AsInt);
            if (DialogueLua.GetVariable(this.name + "Progress").AsInt > 1)
                this.GetComponentInChildren<Animator>().SetBool("DefaultIdle", false);
        }

        //HACK: Resets the TalkedToVars to allow for single cycle completion
        //_myProgress = DialogueLua.GetVariable(this.name + "Progress").AsInt;
    }

    protected override void Update()
    {
        base.Update();

        //If a convo is active then LineTimer will be called
        if (_conversationActive)
            LineTimer();
    }

    protected override void OnConversationStart(Transform actor)
    {
        base.OnConversationStart(actor);
        //Sets _conversationActive to true
        _conversationActive = true;

        //If this is a C class NPC then the spot light will appear
        if (CCharacter)
            ToggleSpotLight(true);
    }

    protected override void OnConversationLine(Subtitle line)
    {
        base.OnConversationLine(line);

        //Checks for unique name and sets that name if it's found in the Description of a line
        string characterName = "";
        for (int i = 0; i < line.dialogueEntry.fields.Count; i++)
        {
            if (line.dialogueEntry.fields[i].title == "Description")
            {
                if (line.dialogueEntry.fields[i].value != "")
                {
                    _currentConversant = line.dialogueEntry.fields[1].value;
                    characterName = line.dialogueEntry.fields[1].value;
                    Invoke("ChangeNames", 0.001f);
                }
            }
        }

        LineManager(line);

        EmpathicEmoticonHandler(line, characterName);
    }

    public void EmpathicEmoticonHandler(Subtitle currentLine, string name)
    {
        //Finds which field the Mood field is and records it's index
        int moodIndex = 0;
        for (int i = 0; i < currentLine.dialogueEntry.fields.Count; i++)
        {
            if (currentLine.dialogueEntry.fields[i].title == "Mood")
            {
                moodIndex = i;
                break;
            }
        }

        //Checks to see if there is a value in the "Mood" field
        if (currentLine.dialogueEntry.fields[moodIndex].value != "")
        {
            if (_lastEmoticonRenderer != _emoticonRenderer && HasMultipleThoughtClouds)
                _lastEmoticonRenderer = _emoticonRenderer;
            if (HasMultipleThoughtClouds) //If there are multiple thought clouds then the thought cloud will be named specifically in _thoughtCouldDic
                _emoticonRenderer = _thoughtCloudDic[name].GetComponent<SpriteRenderer>();
            else //Otherwise they will be generically known as NPC in _thoughtCloudDic
                _emoticonRenderer = _thoughtCloudDic["NPC"].GetComponent<SpriteRenderer>();

            //Sets the sprite to whichever the Mood field's value is then turns the renderer on
            _emoticonRenderer.sprite = _myEmpathicEmoticons.SpriteDictionary[currentLine.dialogueEntry.fields[moodIndex].value];
            _emoticonRenderer.enabled = true;
            if (_lastEmoticonRenderer == _emoticonRenderer)
                CancelInvoke("RemoveThoughtCloud");

            //Removes the thought cloud in 3 seconds
            Invoke("RemoveThoughtCloud", 3);
        }
    }

    private void RemoveThoughtCloud()
    {
        //If there are multiple thought coulds this makes sure that the correct thought cloud is removed
        if (_lastEmoticonRenderer != _emoticonRenderer && HasMultipleThoughtClouds)
        {
            _lastEmoticonRenderer.enabled = false;
            _lastEmoticonRenderer = _emoticonRenderer;
        }
        else //Otherwise it just removes the one
        {
            if (_emoticonRenderer != null)
                _emoticonRenderer.enabled = false;
        }
    }

    //Called whenver a conversation ends
    protected override void OnConversationEnd(Transform actor)
    {
        base.OnConversationEnd(actor);

        _conversationActive = false;
        DialogSetup();
        RemoveThoughtCloud();

        if (CCharacter)
        {
            ToggleSpotLight(false);
        }

        //This checks to see if progress has been made. If so, then the progress popup will appear.
        if (_myProgress < DialogueLua.GetVariable(this.name + "Progress").AsInt)
        {
            myGameManager.gameObject.GetComponent<PopupManager>().ShowPopup();
            _myProgress = DialogueLua.GetVariable(this.name + "Progress").AsInt;
            this.GetComponentInChildren<Animator>().SetInteger("Progress", DialogueLua.GetVariable(this.name + "Progress").AsInt);
        }
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
            {
                progressVarName = MyDatabase.variables[i].fields[0].value;
            }
        }

        //HACK:
        //Resets the TalkedTo vars for test purposes
        //This alows you to complete a level in a single day
        //if (_myProgress < DialogueLua.GetVariable(progressVarName).AsInt)
        //{
        //    DialogueLua.SetVariable("TalkedTo" + this.name, false);
        //    _myProgress = DialogueLua.GetVariable(progressVarName).AsInt;
        //}

        //Sets the Conversation to load
        //NOTE: This will need to be further elaborated if we have player spacific former C level NPCs
        if (PlayerSpacificDialog && myGameManager.IsSarylyn) //If NPC has dialog specific to Sarylyn
            dialogString = this.name.ToString() + "_" + DialogueLua.GetVariable(progressVarName).AsInt + "_Sarylyn";
        else if (PlayerSpacificDialog && !myGameManager.IsSarylyn) //If NPC has dialog specific to Sanome
            dialogString = this.name.ToString() + "_" + DialogueLua.GetVariable(progressVarName).AsInt + "_Sanome";
        else if (FormerCCharacter && MyPastEmpathyType == myGameManager.LastEmpathyTypeCompleted) //If NPC has dialog specific to whether the player has completed their past story line
            dialogString = this.name.ToString() + "_Solved_" + DialogueLua.GetVariable(progressVarName).AsInt;
        else if (FormerCCharacter && MyPastEmpathyType != myGameManager.LastEmpathyTypeCompleted) //If NPC has dialog specific to whether the player has not completed their past story line
            dialogString = this.name.ToString() + "_NotSolved_" + DialogueLua.GetVariable(progressVarName).AsInt;
        else //Otherwise the most simple type of dialog string will be used
            dialogString = this.name.ToString() + "_" + DialogueLua.GetVariable(progressVarName).AsInt;

        base.DialogSetup();
    }

    //Toggles Spotlight on or off based on "val"
    private void ToggleSpotLight(bool val)
    {
        //This for loop darkens the room pieces so that everything appears to be darkened when the spotlight comes on
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

    //Updates the dialog timer
    private void LineTimer()
    {
        _timeSpentOnLine += Time.deltaTime;
        DialogueLua.SetVariable(this.name + "Timer", (int)_timeSpentOnLine);
    }

    //This has to do with the Line timer
    //NOTE: It's possible we may never use this but we might.
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

    //This method changes the displayed name of the Dialog UI if there are multiple NPCs
    private void ChangeNames()
    {
        for (int i = 0; i < FindObjectOfType<DialogUINameHandler>().DisplayNames.Length; i++)
        {
            FindObjectOfType<DialogUINameHandler>().DisplayNames[i].text = _currentConversant;
        }
    }

    //This method populates the dictionary _thoughtCloudDic
    private void ThoughtCloudSetUp()
    {
        List<GameObject> myThoughtClouds = new List<GameObject>();
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("ThoughtCloud").Length; i++)
        {
            if (GameObject.FindGameObjectsWithTag("ThoughtCloud")[i].transform.parent == this.transform)
                myThoughtClouds.Add(GameObject.FindGameObjectsWithTag("ThoughtCloud")[i]);
        }

        for (int i = 0; i < myThoughtClouds.Count; i++)
        {
            string thoughtCloudName = "";

            for (int j = 0; j < myThoughtClouds[i].name.Length; j++)
            {
                if (myThoughtClouds[i].name[j] != '_')
                    thoughtCloudName = thoughtCloudName + myThoughtClouds[i].name[j];
                else
                    break;
            }

            _thoughtCloudDic.Add(thoughtCloudName, myThoughtClouds[i]);
        }
    }
}