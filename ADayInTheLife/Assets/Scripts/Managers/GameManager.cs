using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using SimpleJSON;

public class GameManager : MonoBehaviour
{
    //Static fields with properties that have get and set accessors
    #region Static fields with public accessors
    static float timer = 5;
	public float Timer
	{
		get
		{
			return timer;
		}
		set
		{
			Debug.Log ("Who told you that you could set this property!?");
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
	static JSONNode jSONOut = new JSONNode();
	public JSONNode JSONOut
	{
		get
		{
			return jSONOut;
		}
		set
		{
			jSONOut = value;
		}
	}
	static WWWForm formJSON = new WWWForm();
	public WWWForm FormJSON
	{
		get
		{
			return formJSON;
		}
		set
		{
			formJSON = value;
		}
	}
    static int dayCount = 1;
    public int DayCount
    {
        get
        {
            return dayCount;
        }
        set
        {
            if (value > 2)
                dayCount = 3;
            else
                dayCount = value;
        }
    }
    #endregion

    //Simple Static fields
	static bool lvl1DatabasesLoaded = false,
				lvl2DatabasesLoaded = false,
				lvl1JSONInitialized = false;

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
            if(value != _fadingAway && value)
                _fadeMask = MainCamera.GetComponentInChildren<SpriteRenderer>();

            _fadingAway = value;
        }
    }
    private bool _fadingAway;

	//Public fields
	public bool ITestThereforeIAm; //HACK: for testing purposes (iTestTherforeIHack{not an interface})
	public Camera MainCamera;
	public AudioSource Countdown30,
					   Countdown10,
                       MainBGM;

	void Start ()
	{
		if(LevelCount == 1 && !lvl1DatabasesLoaded)
		{
			DatabaseLoader myDatabaseLoader = this.GetComponent<DatabaseLoader>();

			foreach(DialogueDatabase dd in myDatabaseLoader.Level1Databases)
			{
				DialogueManager.AddDatabase(dd);
			}
			lvl1DatabasesLoaded = true;
		}
		else if(LevelCount == 2 && !lvl2DatabasesLoaded)
		{
            DialogueManager.MasterDatabase.Clear();
			DatabaseLoader myDatabaseLoader = this.GetComponent<DatabaseLoader>();
			
			foreach(DialogueDatabase dd in myDatabaseLoader.Level2Databases)
			{
				DialogueManager.AddDatabase(dd);
			}
			lvl2DatabasesLoaded = true;
		}

		//JSON set up
		StartCoroutine (JSONSetUp());

		MainCamera = Camera.main;
		if(FindObjectOfType<PlayerScript>() != null)
			_player = FindObjectOfType<PlayerScript>();

		MainBGM = GameObject.FindGameObjectWithTag("MainBGM").GetComponent<AudioSource>();
	}

	void Update ()
	{
		if (ITestThereforeIAm)
		{
			if(Input.GetKey(KeyCode.N) && Input.GetKey(KeyCode.O) && Input.GetKey(KeyCode.T))
			{
				Application.LoadLevel("Lobby");
			}
			if(Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.E))
			{
				Application.LoadLevel("Hallway");
			}

		}
		if(GameTimerActive)
		{
			timer -= Time.deltaTime;
		}

		if(FadingAway)
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

	private IEnumerator JSONSetUp()
	{
		if(!lvl1JSONInitialized && Application.loadedLevelName == "DreamSpiral")
		{
			JSONOut = JSONNode.Parse("{\"Player\":\"Sarylyn\"},\"Gamestate\":{}");
			if(IsSarylyn)
				JSONOut["Player"] = "Sarylyn";
			else
				JSONOut["Player"] = "Sanome";
			
			for (int i = 0; i < DialogueManager.MasterDatabase.conversations.Count; i++)
			{
				string conversationString = DialogueManager.MasterDatabase.conversations[i].Title.ToString();
				JSONOut["Level1"]["Conversations"][conversationString][0] = "";
			}
			lvl1JSONInitialized = true;
			string url = "http://localhost:3000/gamestates/create";
			FormJSON.AddField("gamestate", WWW.EscapeURL(jSONOut.ToString()));
			Hashtable headers = new Hashtable();
			headers.Add("Content-Type", "application/json");

			//Debug.Log (JSONOut.ToString());
			yield return new WWW(url, Encoding.Default.GetBytes(FormJSON.data.ToString()), headers);
		}
	}

	public IEnumerator LogJSON()
	{
		string url = "http://localhost:3000/gamestates/create";
		FormJSON.AddField("gamestate", jSONOut.ToString());
		Hashtable headers = new Hashtable();
		headers.Add("Content-Type", "application/json");
		yield return new WWW(url, Encoding.Default.GetBytes(FormJSON.data.ToString()), headers);
	}

	public void LoadNextLevel()
	{
		bool loadNewLevel = false;

		for(int i = 0; i < DialogueManager.MasterDatabase.variables.Count; i++)
		{
			if(DialogueManager.MasterDatabase.variables[i].fields[2].value == "WinCondition")
			{
				if(DialogueLua.GetVariable(DialogueManager.MasterDatabase.variables[i].Name).AsBool == true)
					loadNewLevel = true;
			}
		}

		if(loadNewLevel)
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
