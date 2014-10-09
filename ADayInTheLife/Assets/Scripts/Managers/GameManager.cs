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
    static float timer = 360;
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
    #endregion

    //Simple Static fields
	static int DayCount = 0;
	static bool lvl1DatabasesLoaded = false,
				lvl2DatabasesLoaded = false,
				lvl1JSONInitialized = false;

	//Private fields
	private float _alpha = 0;
	private SpriteRenderer _fadeMask;
	private PlayerScript _player;

	//Public fields
	public bool	FadingAway,
                ITestThereforeIAm; //HACK: for testing purposes (iTestTherforeIHack{not an interface})
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

		DayEnd();

		if(FadingAway)
			Fade();

		//This will be disabled until I have a chance to work on optimization.
		//if(Input.GetKeyDown(KeyCode.F))
		//{
		//	Screen.fullScreen = !Screen.fullScreen;
		//}
	}

	//This method handles the strange things that happen at the end of a day.
	private void DayEnd()
	{
        if (timer <= 60 && MainBGM.pitch > 0.9f)
		{
            MainBGM.pitch = 0.9f;
		}
		if(timer <= 50 && !this.Countdown30.isPlaying)
		{
            MainBGM.pitch = 0.85f;
            MainBGM.volume = 0.7f;
			Countdown30.Play();
			Countdown30.volume = 0.3f;
		}
        if (timer <= 40 && MainBGM.pitch > 0.8f)
		{
            MainBGM.pitch = 0.8f;
            MainBGM.volume = 0.65f;
			Countdown30.volume = 0.5f;
		}
        if (timer <= 20 && MainBGM.pitch > 0.7f)
		{
            MainBGM.pitch = 0.7f;
            MainBGM.volume = 0.3f;
			Countdown30.volume = 0.75f;
		}
		if (timer <= 10 && !this.Countdown10.isPlaying)
		{
            MainBGM.pitch = 0.5f;
            MainBGM.volume = 0.25f;
			_player.ConfuseMovement();
			this.Countdown10.Play();
		}
		switch(Application.loadedLevelName)
		{
			case "Hallway":
                if (timer <= 30 && MainBGM.pitch > 0.75f)
				{
                    MainBGM.pitch = 0.75f;
                    MainBGM.volume = 0.5f;
					Countdown30.volume = 0.6f;
					GameObject[] _lockerSearch = GameObject.FindGameObjectsWithTag("locker30");    
					foreach(GameObject _locker in _lockerSearch)
					{
						StartCoroutine(StaggeredAnimationPlay(_locker.GetComponent<Animation>(), 0.7f));
					}
				}
				
				if(timer <= 10 && timer > 9.9)
				{
					GameObject[] _lockerSearch = GameObject.FindGameObjectsWithTag("locker30");    
					foreach(GameObject _locker in _lockerSearch)
					{
						_locker.GetComponent<Animation>().Stop ();
						_locker.collider.enabled = true;
						_locker.rigidbody.AddForce(1f, 1f, 1f);
					}
				}
				break;
			case "Labrary":
                if (timer <= 30 && MainBGM.pitch > 0.75f)
				{
                    MainBGM.pitch = 0.75f;
                    MainBGM.volume = 0.5f;
					Countdown30.volume = 0.6f;
					GameObject[ ] _computerSearch = GameObject.FindGameObjectsWithTag("computer60");    
					foreach(GameObject _computer in _computerSearch)
					{
						StartCoroutine(StaggeredAnimationPlay(_computer.GetComponent<Animation>(), 1f));
					}
				}
				break;
			case "Classroom":
                if (timer <= 30 && MainBGM.pitch > 0.75f)
				{
                    MainBGM.pitch = 0.75f;
                    MainBGM.volume = 0.5f;
					Countdown30.volume = 0.6f;
					GameObject[] _deskSearch = GameObject.FindGameObjectsWithTag("desk");    
					foreach(GameObject _desk in _deskSearch)
					{
						_desk.rigidbody.useGravity = true;
						_desk.rigidbody.isKinematic = false;
					}
				}
				break;
			case "Roomclass":
                if (timer <= 30 && MainBGM.pitch > 0.75f)
				{
                    MainBGM.pitch = 0.75f;
                    MainBGM.volume = 0.5f;
					Countdown30.volume = 0.6f;
					GameObject[] _deskSearch = GameObject.FindGameObjectsWithTag("desk");    
					foreach(GameObject _desk in _deskSearch)
					{
						_desk.rigidbody.isKinematic = false;
						_desk.rigidbody.AddForce(0,100,0);
					}
				}
                break;
            case "GreenWorld":
                if (timer <= 30 && MainBGM.pitch > 0.75f)
                {
                    MainBGM.pitch = 0.75f;
                    MainBGM.volume = 0.5f;
                    Countdown30.volume = 0.6f;
                    GameObject[] animationObjects = GameObject.FindGameObjectsWithTag("SetPiece");
                    List<Animation> animations = new List<Animation>();

                    for(int i = 0; i < animationObjects.Length; i++)
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
		if(timer <= 3)
		{
			Invoke("LoadNextLevel", 3);
			DialogueManager.StopConversation();
			_fadeMask = MainCamera.GetComponentInChildren<SpriteRenderer>();
			FadingAway = true;
		}
	}

	private void Fade()
	{
		_alpha += Time.deltaTime * 0.35f;
        MainBGM.volume -= Time.deltaTime * 0.15f;
		_fadeMask.color = new Color(_fadeMask.color.r, _fadeMask.color.g, _fadeMask.color.b, _alpha);

	}

	private IEnumerator StaggeredAnimationPlay(Animation animation, float variance)
	{
		float randFloat = Random.Range(0, variance);
		yield return new WaitForSeconds(randFloat);
		animation.Play();
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

	private void LoadNextLevel()
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
			DayCount = 0;
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
	}
}
