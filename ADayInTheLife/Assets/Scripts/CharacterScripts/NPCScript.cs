using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class NPCScript : MonoBehaviour
{
	public Camera CloseUpCamera;
	public DialogueDatabase MyDatabase;
	public ConversationTrigger MyConTrigger;
	public bool AlwaysFacePlayer,
				HasSharedVariables;
	public string DialogString;

	private GameObject _player;
	private Vector3 _orriginalRotation;

	void Awake()
	{
		DialogueManager.AddDatabase(MyDatabase);
	}

	void Start ()
	{
		MyConTrigger.conversation = DialogString;

		_player = GameObject.Find("Player");
		_orriginalRotation = this.transform.rotation.eulerAngles;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(AlwaysFacePlayer)
			RotateTowardPlayer();
	}

	void OnConversationStart(Transform actor)
	{
		CloseUpCamera.enabled = true;
	}

	void OnConversationEnd(Transform actor)
	{
		CloseUpCamera.enabled = false;
		if(HasSharedVariables)
		{
			GameObject.FindGameObjectWithTag("GameManager").GetComponent<SharedVariables>().SyncVariables(DialogString);
		}
	}

	private void RotateTowardPlayer()
	{
		this.transform.LookAt(_player.transform);
		Debug.Log (this.transform.rotation.eulerAngles.y + _orriginalRotation.y);
		this.transform.rotation = Quaternion.Euler(new Vector3(0 + _orriginalRotation.x, this.transform.rotation.eulerAngles.y + _orriginalRotation.y, 0 + _orriginalRotation.z));

		//This will maintain rotation with the player
		//this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, _player.transform.rotation, 100);
	}
}
