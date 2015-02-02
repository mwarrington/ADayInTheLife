using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class GameManager : MonoBehaviour
{
    //Static fields with properties that have get and set accessors
    #region Static fields with public accessors
    //Game timer. Day resets after this runs out
    static float timer = 360;
    public float Timer
    {
        get
        {
            return timer;
        }
        set
        {
            Debug.Log("Who told you that you could set this property!?");
        }
    }
    //Whether the player is Sarylyn or Sanome
    static bool isSarylyn = true;
    public bool IsSarylyn
    {
        get
        {
            return isSarylyn;
        }
        set
        {
            isSarylyn = value;
        }
    }
    //Indicates that last level that has been loaded
    static string lastLevelLoaded = "DreamSpiral";
    public string LastLevelLoaded
    {
        get
        {
            return lastLevelLoaded;
        }
        set
        {
            lastLevelLoaded = value;
        }
    }
    //If the player has had an introduction with a hall monitor
    static bool hasBeenIntroduced = false;
    public bool HasBeenIntroduced
    {
        get
        {
            return hasBeenIntroduced;
        }
        set
        {
            hasBeenIntroduced = value;
        }
    }
    //Whether the game timer is running
    static bool gameTimerActive;
    public bool GameTimerActive
    {
        get
        {
            return gameTimerActive;
        }
        set
        {
            gameTimerActive = value;
        }
    }
    //Indicates what level the player is currently on
    static int levelCount = 1;
    public int LevelCount
    {
        get
        {
            return levelCount;
        }
        set
        {
            levelCount = value;
        }
    }
    //Indicates the number of repetitions the player has experienced
    static int dayCount = 0;
    public int DayCount
    {
        get
        {
            return dayCount;
        }
        set
        {
            dayCount = value;
        }
    }
    //Indicates which empathy type the player completed last
    static EmpathyTypes lastEmpathyTypeCompleted;
    public EmpathyTypes LastEmpathyTypeCompleted
    {
        get
        {
            return lastEmpathyTypeCompleted;
        }
        set
        {
            lastEmpathyTypeCompleted = value;
        }
    }
    //Indicates the last NPC the player talked to
    static string lastCharacterTalkedTo;
    public string LastCharacterTalkedTo
    {
        get
        {
            return lastCharacterTalkedTo;
        }
        set
        {
            lastCharacterTalkedTo = value;
        }
    }
    #endregion

    //Simple Static fields
    static bool lvl1DatabasesLoaded = false,
                lvl2DatabasesLoaded = false,
                testDatabasesLoaded = false,
                iTestThereforeIAm = false; //HACK: for testing purposes (iTestTherforeIHack{not an interface}); Make false when not in test mode.

    //Private fields
    private float _alpha = 0;
    private SpriteRenderer _fadeMask;
    private PlayerScript _player;

    //Public fields with accessors
    public bool FadingAway
    {
        get
        {
            return _fadingAway;
        }
        set
        {
            //if the value of FadingAway is changing to true this finds the appropriate Fade Mask
            if (value != _fadingAway && value)
                _fadeMask = MainCamera.GetComponentInChildren<SpriteRenderer>();

            _fadingAway = value;
        }
    }
    private bool _fadingAway;

    //Public fields
    public Camera MainCamera;
    public AudioSource Countdown30,
                       Countdown10,
                       MainBGM;

    void Start()
    {
        //Loads the test list of databases
        if (iTestThereforeIAm && !testDatabasesLoaded)
        {
            DatabaseLoader myDatabaseLoader = this.GetComponent<DatabaseLoader>();

            foreach (DialogueDatabase dd in myDatabaseLoader.TestModeDatabases)
            {
                DialogueManager.AddDatabase(dd);
            }
            testDatabasesLoaded = true;
        }
        else if (LevelCount == 1 && !lvl1DatabasesLoaded && !iTestThereforeIAm) //Loads the databases for lvl 1
        {
            DatabaseLoader myDatabaseLoader = this.GetComponent<DatabaseLoader>();

            foreach (DialogueDatabase dd in myDatabaseLoader.Level1Databases)
            {
                DialogueManager.AddDatabase(dd);
            }
            lvl1DatabasesLoaded = true;
        }
        else if (LevelCount == 2 && !lvl2DatabasesLoaded && !iTestThereforeIAm) //Loads the databases for lvl 2
        {
            DialogueManager.MasterDatabase.Clear();
            DatabaseLoader myDatabaseLoader = this.GetComponent<DatabaseLoader>();

            foreach (DialogueDatabase dd in myDatabaseLoader.Level2Databases)
            {
                DialogueManager.AddDatabase(dd);
            }
            lvl2DatabasesLoaded = true;
        }

        //Sets the main camera; this exists because Camera.main is kinda funky
        MainCamera = Camera.main;

        //Finds and sets the player script for reference
        if (FindObjectOfType<PlayerScript>() != null)
            _player = FindObjectOfType<PlayerScript>();

        //Finds and sets the MainBGM AudioSource
        MainBGM = GameObject.FindGameObjectWithTag("MainBGM").GetComponent<AudioSource>();

        //If we are working in editor then has been introduced will always = true unless we are in the Hallway scene. (for testing purposes)
#if UNITY_EDITOR
        if (Application.loadedLevelName != "Hallway")
            HasBeenIntroduced = true;

        //This line turns vSync off for sure if we're in editor
        QualitySettings.vSyncCount = 0;
#endif
    }

    void Update()
    {
        //Allows the player to load differnt levels for test purposes
        if (iTestThereforeIAm)
        {
            if (Input.GetKey(KeyCode.N) && Input.GetKey(KeyCode.O) && Input.GetKey(KeyCode.T))
            {
                Application.LoadLevel("Lobby");
            }
            if (Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.E))
            {
                Application.LoadLevel("Hallway");
            }
        }
        //Runs the game clock if GameTimerActive is true
        if (GameTimerActive)
        {
            timer -= Time.deltaTime;
        }

        //Runs the Fade method if FadingAway is true
        if (FadingAway)
            Fade();

        //This will be disabled until I have a chance to work on optimization.
        //if(Input.GetKeyDown(KeyCode.F))
        //{
        //	Screen.fullScreen = !Screen.fullScreen;
        //}
    }


    //Handles fade away
    private void Fade()
    {
        //Increases the alpha of the fade mask over time
        _alpha += Time.deltaTime * 0.35f;
        MainBGM.volume -= Time.deltaTime * 0.15f;
        _fadeMask.color = new Color(_fadeMask.color.r, _fadeMask.color.g, _fadeMask.color.b, _alpha);
    }

    //Handles scene loadeding at the end of a day
    public void LoadNextLevel()
    {
        bool loadNewLevel = false;

        //Checks to see if any of the win conditions are met
        //Sets the which win condition is met in LastEmpathyTypeCompleted
        for (int i = 0; i < DialogueManager.MasterDatabase.variables.Count; i++)
        {
            if (DialogueManager.MasterDatabase.variables[i].fields[2].value == "WinCondition")
            {
                if (DialogueLua.GetVariable(DialogueManager.MasterDatabase.variables[i].Name).AsBool == true)
                {
                    if (LevelCount == 1)
                    {
                        switch (DialogueManager.MasterDatabase.variables[i].Name)
                        {
                            case "MarkeshiaHeartWin":
                                LastEmpathyTypeCompleted = EmpathyTypes.Another;
                                break;
                            case "ShawnWin":
                                LastEmpathyTypeCompleted = EmpathyTypes.Self;
                                break;
                            default:
                                Debug.Log("Check your databases, this win condition shouldn't be true.");
                                break;
                        }
                    }
                    loadNewLevel = true;
                }
            }
        }

        //Resets DayCount and adds 1 to LevelCount if a win condition is met
        if (loadNewLevel)
        {
            DayCount = 0;
            LevelCount++;
        }
        else //Adds one to DayCount and resets event vars if a win condition is not met
        {
            this.GetComponent<VariableManager>().ResetEventVars();
            DayCount++;
        }
        //Always resets hasBeenIntroduced and loads the "DreamSpiral" scene
        HasBeenIntroduced = false;
        Application.LoadLevel("DreamSpiral");

        //Resets game timer, starts the fade process, stops game timer, and tells the EmpathicEmoticons script that it will need to load new EEs
        //NOTE: The game time may need to be set to different amounts based on the level but this has not been decided yet
        timer = 360;
        FadingAway = false;
        gameTimerActive = false;
        this.GetComponent<EmpathicEmoticons>().EmoticonsLoaded = false;
    }
}