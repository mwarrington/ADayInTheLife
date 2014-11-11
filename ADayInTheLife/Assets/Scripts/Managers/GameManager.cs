using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class GameManager : MonoBehaviour
{
    //Static fields with properties that have get and set accessors
    #region Static fields with public accessors
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
        if (iTestThereforeIAm && !testDatabasesLoaded)
        {
            DatabaseLoader myDatabaseLoader = this.GetComponent<DatabaseLoader>();

            foreach (DialogueDatabase dd in myDatabaseLoader.TestModeDatabases)
            {
                DialogueManager.AddDatabase(dd);
            }
            testDatabasesLoaded = true;
        }
        else if (LevelCount == 1 && !lvl1DatabasesLoaded && !iTestThereforeIAm)
        {
            DatabaseLoader myDatabaseLoader = this.GetComponent<DatabaseLoader>();

            foreach (DialogueDatabase dd in myDatabaseLoader.Level1Databases)
            {
                DialogueManager.AddDatabase(dd);
            }
            lvl1DatabasesLoaded = true;
        }
        else if (LevelCount == 2 && !lvl2DatabasesLoaded && !iTestThereforeIAm)
        {
            DialogueManager.MasterDatabase.Clear();
            DatabaseLoader myDatabaseLoader = this.GetComponent<DatabaseLoader>();

            foreach (DialogueDatabase dd in myDatabaseLoader.Level2Databases)
            {
                DialogueManager.AddDatabase(dd);
            }
            lvl2DatabasesLoaded = true;
        }

        MainCamera = Camera.main;
        if (FindObjectOfType<PlayerScript>() != null)
            _player = FindObjectOfType<PlayerScript>();

        MainBGM = GameObject.FindGameObjectWithTag("MainBGM").GetComponent<AudioSource>();
    }

    void Update()
    {
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
        if (GameTimerActive)
        {
            timer -= Time.deltaTime;
        }

        if (FadingAway)
            Fade();

        //This will be disabled until I have a chance to work on optimization.
        //if(Input.GetKeyDown(KeyCode.F))
        //{
        //	Screen.fullScreen = !Screen.fullScreen;
        //}
    }


    //Handles fade away
    //I might move this to DayEndManager.cs but haven't decided yet
    private void Fade()
    {
        _alpha += Time.deltaTime * 0.35f;
        MainBGM.volume -= Time.deltaTime * 0.15f;
        _fadeMask.color = new Color(_fadeMask.color.r, _fadeMask.color.g, _fadeMask.color.b, _alpha);
    }

    public void LoadNextLevel()
    {
        bool loadNewLevel = false;

        for (int i = 0; i < DialogueManager.MasterDatabase.variables.Count; i++)
        {
            if (DialogueManager.MasterDatabase.variables[i].fields[2].value == "WinCondition")
            {
                if (DialogueLua.GetVariable(DialogueManager.MasterDatabase.variables[i].Name).AsBool == true)
                    loadNewLevel = true;
            }
        }

        if (loadNewLevel)
        {
            Application.LoadLevel("MainMenu");
            DayCount = 1;
            //Something to clear the database for new databases.
        }
        else
        {
            this.GetComponent<VariableManager>().ResetEventVars();
            Application.LoadLevel("DreamSpiral");
            DayCount++;
            hasBeenIntroduced = false;
        }

        timer = 360;
        FadingAway = false;
        gameTimerActive = false;
        this.GetComponent<EmpathicEmoticons>().EmoticonsLoaded = false;
    }
}
