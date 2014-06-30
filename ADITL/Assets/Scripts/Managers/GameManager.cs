using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class GameManager : MonoBehaviour
{
    public static float timer;
	public static float timerStart = 360;
    public bool startTime = false,
        timeElapsed = false;

    public static int restSeconds, roundedRestSeconds, displaySeconds, displyaMinutes, coutdownSeconds;

   
    bool playOnce = false;
	
	public AudioClip currentClip = null;
	public AudioClip currentStinger = null;
	public bool musicPlaying = false;
	
	public float timeBeforeReset = 30.0f;

	public GUIStyle font;

	public GameObject thoughtCloud, spiral;

	private float  _totalHideTimer = 30f,
                   _hideTimerCountdown,
                   _showTimerSeconds = 5f;
    public bool showTimer = true;
    private bool billMurray = true;
 
	//QuestLog test;
	public string title, description;
	public QuestState state;
	static public int daysRepeated = 0;

	// Use this for initialization
	void Start () {
        
        spiral.renderer.enabled = true;
        thoughtCloud.renderer.enabled = true;
        billMurray = true;
       

        //Debug.Log(QuestLog.IsQuestActive("Test"));

		if (!DreamSpiral.startTimerOnce && billMurray)
        {
        	_hideTimerCountdown = _totalHideTimer;
	        startTime = true;
	        timer = timerStart;
	        playOnce = false;
	        billMurray = false;
        }
        
        QuestLog.AddQuest(title, description, state);	
        
	}

	void Awake()
	{
		 Debug.Log(daysRepeated);
		 if (daysRepeated >0)
		 {
		 	QuestLog.SetQuestState("DaysRepeated", QuestState.Active);
		 }
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!billMurray)
			DreamSpiral.startTimerOnce = true;

        if (startTime)
        {
        	 _hideTimerCountdown -= Time.deltaTime;
            if (_hideTimerCountdown < (_totalHideTimer - _showTimerSeconds))
            {
                spiral.renderer.enabled = false;
                thoughtCloud.renderer.enabled = false;
            }
            if (_hideTimerCountdown <= 0)
            {
                _hideTimerCountdown = _totalHideTimer;
                spiral.renderer.enabled = true;
                thoughtCloud.renderer.enabled = true;
            }
        	
        	spiral.gameObject.transform.Rotate(Vector3.up);
            timer -= Time.deltaTime;
            float guiTime = Time.deltaTime - timer;
            restSeconds = coutdownSeconds - (int)guiTime;
            roundedRestSeconds = Mathf.CeilToInt(restSeconds);
            displaySeconds = roundedRestSeconds % 60;
            displyaMinutes = roundedRestSeconds / 60;

            /*if (!playOnce)
            {
                playOnce = true;
                SoundScript.Play(PrefabLoaderScript.instance.finalHours, .3f);
				currentClip = PrefabLoaderScript.instance.finalHours;
            }*/
            if (timer <= 0)
            {
                ResetTimer();
                DetermineEnding();
                playOnce = false;
            }
			if (!musicPlaying)
			{
				//SoundScript.Unpause(currentClip);
				//SoundScript.Unpause(currentStinger);
				musicPlaying = true;
			}
        }
        else
        {
        	spiral.renderer.enabled = false;
        	thoughtCloud.renderer.enabled = false;
        }

		if (timer < 10)
		{
			//if (currentClip != PrefabLoaderScript.instance.tenSecondsRemain)
				//CueTenSecondSong();
		}
		/*else if (timer < 30)
		{
			if (currentClip != PrefabLoaderScript.instance.thirtySecondsRemain)
				CueThirtySecondSong();
		}*/
		
		if ((!startTime) && musicPlaying && SoundManager.IsPlaying(currentClip))
		{
			/*Debug.Log("Stopped");
			SoundScript.Pause(currentClip);
			SoundScript.Pause(currentStinger);
			musicPlaying = false;*/
            
		}

        
    }

    void OnGUI()
    {
        string text = string.Format("{0:00}:{1:00}", displyaMinutes, displaySeconds);
		
		//Texture2D tmp = PrefabLoaderScript.instance.bioFont.normal.background;
		//PrefabLoaderScript.instance.bioFont.normal.background = null;
    if (showTimer)
        	{
            		if (startTime)
        {
        	if ((_hideTimerCountdown > _totalHideTimer - _showTimerSeconds))
            {
            	
	            	if (Application.loadedLevelName == "hallway")
	                	GUI.Box(new Rect(Screen.width * 0.355f, Screen.height * 0.525f, Screen.width * 0.15f, Screen.height * 0.15f), text, font);
	            	else if (Application.loadedLevelName == "labrary")
	                	GUI.Box(new Rect(Screen.width * 0.275f, Screen.height * 0.545f, Screen.width * 0.15f, Screen.height * 0.15f), text, font);
	            
            }
        }
    }

		//PrefabLoaderScript.instance.bioFont.normal.background = tmp;
    }

    void ResetTimer()
    {
        timer = 360f;
       	daysRepeated +=1;
        Application.LoadLevel("DreamSpiral");
       
    }




    public void DetermineEnding()
    {
        
    }
	
	/*void CueThirtySecondSong()
	{
		SoundScript.Stop(currentClip);
		SoundScript.Play(PrefabLoaderScript.instance.thirtySecondsRemain);
		currentClip = PrefabLoaderScript.instance.thirtySecondsRemain;
	}*/
	
	void CueTenSecondSong()
	{
		/*SoundScript.Stop(currentClip);
		currentClip = PrefabLoaderScript.instance.tenSecondsRemain;
		SoundScript.Play(currentClip);
		
		switch (gameEnd)
		{
		case GameEnd.Good:
			currentStinger = PrefabLoaderScript.instance.stingerGood;
			break;
		case GameEnd.manuOver:
		case GameEnd.ok:
			currentStinger = PrefabLoaderScript.instance.stingerOK;
			break;
		case GameEnd.neutral:
			currentStinger = PrefabLoaderScript.instance.stingerNeutral;
			break;
		case GameEnd.badBoth:
			currentStinger = PrefabLoaderScript.instance.stingerBothBad;
			break;
		case GameEnd.badNat:
			currentStinger = PrefabLoaderScript.instance.stingerBadNat;
			break;
		case GameEnd.badMan:
			currentStinger = PrefabLoaderScript.instance.stingerBadManu;
			break;
		}
		
		SoundScript.Play(currentStinger, 1.0f);*/
	}
	
	
}
