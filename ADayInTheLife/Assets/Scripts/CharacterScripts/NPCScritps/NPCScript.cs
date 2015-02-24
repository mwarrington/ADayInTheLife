using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
//using PixelCrushers.DialogueSystem.UnityGUI;

public class NPCScript : MonoBehaviour
{
    //Public Fields to be set in inspector
    public Camera CloseUpCamera;
    public AudioSource MyVoice;
	public DialogueDatabase MyDatabase;
    public DialogueVisualUI MyDialogGui;
	public NPCType MyType;
	public bool AlwaysFacePlayer,
				HasSharedVariables,
				PlayerSpacificDialog,
				HasCloseUpCam,
				TimedResponse;

    //Protected fields to be set in Start()
	protected GameObject player;
	protected Vector3 orriginalRotation;
	protected GameManager myGameManager;
	protected ConversationTrigger myConTrigger;
	protected List<string> namesToSync = new List<string>();
	protected string dialogString;
	protected bool hasSharedProgress = false;

	protected virtual void Start ()
	{
        //Initialization of protected fields
		player = GameObject.FindGameObjectWithTag("Player");
		orriginalRotation = this.transform.rotation.eulerAngles;
		MyVoice = this.GetComponent<AudioSource>();
		myGameManager = GameObject.FindObjectOfType<GameManager>();
		myConTrigger = this.GetComponent<ConversationTrigger>();
        //Calls DialogSetup() after a 0.1 second delay
        //This gives the game manager time to load all of the databases before dialogs are set up
		Invoke("DialogSetup", 0.1f);
	}
	
	protected virtual void Update ()
	{
        //Calls RotateTowardPlayer if AlwaysFacePlayer is true
		if(AlwaysFacePlayer)
			RotateTowardPlayer();
	}

    //Called when conversation is initiated
    protected virtual void OnConversationStart(Transform actor)
    {
        //This sets the amount of time for the response timer if TimedResponse is true
        if (TimedResponse)
            DialogueManager.Instance.displaySettings.inputSettings.responseTimeout = 30;
        else //Sets response timer to 0 if TimedResponse is false. This effectivly disables the response timer
            DialogueManager.Instance.displaySettings.inputSettings.responseTimeout = 0;

        //If the player has a close up cam then it will be enabled while turning off the main camera
        if (HasCloseUpCam)
        {
            CloseUpCamera.enabled = true;
            myGameManager.MainCamera.enabled = false;

            //This will handle talk voice but requires a design conversation before it can be implimented
            //if(MyVoice != null)
            //    MyVoice.Play();
        }

        //Sets which convo the player just initiated
        Conversation currentConvo = DialogueManager.MasterDatabase.GetConversation(DialogueManager.LastConversationStarted);
        //If a convorsation is meant to share progress changes in Lua then
        //it's Description field will need to be set to "SharedProgress" followed by the names of every NPC it's sharing with followed by a ' '
        //Example: "SharedProgress Tom Dick Harry "
        for (int i = 0; i < currentConvo.fields.Count; i++)
        {
            if (currentConvo.fields[i].title == "Description" && currentConvo.fields[i].value != "")
            {
                string title = "";
                for (int j = 0; j < 14; j++)
                {
                    title = title + currentConvo.fields[i].value[j];
                }
                if (title == "SharedProgress")
                {
                    string name = "";
                    for (int j = 15; j < (currentConvo.fields[i].value.Length); j++)
                    {
                        if (currentConvo.fields[i].value[j] != ' ')
                        {
                            name = name + currentConvo.fields[i].value[j];
                        }
                        else
                        {
                            namesToSync.Add(name);
                            name = "";
                        }
                    }
                    hasSharedProgress = true;
                    break;
                }
            }
        }

        myGameManager.LastCharacterTalkedTo = this.name;
    }

    //Called per dialog line
	protected virtual void OnConversationLine(Subtitle line)
	{
		if(line.speakerInfo.IsPlayer)
		{
			if(myGameManager.LevelCount == 1)
			{
				//This next line may need to be commented out in editor to avoid a null ref
                //myGameManager.GetComponent<JSONHandler>().JSONOut["Level1"]["Conversations"][DialogueManager.LastConversationStarted.ToString()][myGameManager.JSONOut["Level1"]["Conversations"][DialogueManager.LastConversationStarted.ToString()].Count] = line.dialogueEntry.id.ToString();
				//Debug.Log (myGameManager.JSONOut.ToString());
			}
		}
	}

    //Called at the end of each conversation
	protected virtual void OnConversationEnd(Transform actor)
	{
        //Switches back to main camera if there was a close up cam
		if(HasCloseUpCam)
		{
			CloseUpCamera.enabled = false;
			myGameManager.MainCamera.enabled = true;
		}

        //If the NPC has Shared Variables then it will call the SyncVariables method
		if(HasSharedVariables)
		{
			GameObject.FindGameObjectWithTag("GameManager").GetComponent<VariableManager>().SyncVariables(dialogString);
		}
        //If the NPC has Shared Progress then it will call the SharedProgress method
		if(hasSharedProgress)
		{
			GameObject.FindGameObjectWithTag("GameManager").GetComponent<VariableManager>().ProgressSync(this.name, namesToSync);
			hasSharedProgress = false;
		}

        //JSON component
		//StartCoroutine(myGameManager.GetComponent<JSONHandler>().LogJSON());
        myGameManager.LastCharacterTalkedTo = this.name;
	}

    protected virtual void OnTriggerEnter(Collider col)
    {
        //Changes which DialogUI the game uses for convos
        if (MyDialogGui != null && col.tag == "Player")
        {
            GameObject.Destroy(FindObjectOfType<DialogueVisualUI>().gameObject);
            DialogueManager.DialogueUI = MyDialogGui;
        }
    }

    //This method handles the rotation of NPCs in rotates toward player scenes
	protected virtual void RotateTowardPlayer()
	{
		//This will make sprites turn to look at the player
		//this.transform.LookAt(_player.transform);
		//this.transform.rotation = Quaternion.Euler(new Vector3(0 + _orriginalRotation.x, this.transform.rotation.eulerAngles.y + _orriginalRotation.y, 0 + _orriginalRotation.z));

		//This will maintain rotation with the player
		this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, player.transform.rotation, 100);
	}

	//This method sets up what conversation the NPC will speak.
	protected virtual void DialogSetup()
	{
		myConTrigger.conversation = dialogString;
	}
}