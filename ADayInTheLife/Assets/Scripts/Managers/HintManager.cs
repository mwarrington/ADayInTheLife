using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.Examples;

//For now the only thing this script does is handle the things that happen in the Consular's Office
//Eventually this script might do more but I won't know that until we finish designing the hint system
public class HintManager : MonoBehaviour
{
	public GameObject SceneCurtains,
					  DeskCurtains;

    //This property determins whether the player sees the instructions for dialog or leaving the room
	protected bool colliderActive
	{
		get
		{
            //If the camera is zooming in, if the curtains are animating, or if a conversation is active
            //then the instructions wont be shown.
			if((_myCameraManager.ZoomingIn || SceneCurtains.GetComponent<Animation>().isPlaying || DialogueManager.IsConversationActive))
				_colliderActive = false;
			else
				_colliderActive = true;

			return _colliderActive;
		}
        set
        {
            Debug.Log("You shouldn't be trying to set this property");
        }
	}
	private bool _colliderActive;

	private CameraManager _myCameraManager;
	private ProximitySelector _playerProxSelector;
	private PlayerScript _player;

	void Start()
	{
		_myCameraManager = FindObjectOfType<CameraManager>();
		_player = FindObjectOfType<PlayerScript>();
		_playerProxSelector = _player.GetComponent<ProximitySelector>();
        //Disables the proximity selector so that it doesn't initially show the talk to options
		_playerProxSelector.enabled = false;
	}

	void Update()
	{
        //If the collider is active then the proximity selector becomes true.
		if(colliderActive)
			_playerProxSelector.enabled = true;
		else
			_playerProxSelector.enabled = false;

        //This resets the curtains to thier 0 position if the camera isn't zooming in
		if(!_myCameraManager.ZoomingIn)
		{
			DeskCurtains.SampleAnimation(DeskCurtains.GetComponent<Animation>().clip, 0);
		}
	}

	void OnConversationEnd (Transform actor)
	{
        //Checks to see if the player has requested a hint
		if(DialogueLua.GetVariable("ShowHint").AsBool)
		{
			ShowHint();
			DialogueLua.SetVariable("ShowHint", false);
		}
	}

    //Plays the desk curtain animation and zooms in
	private void ShowHint()
	{
		DeskCurtains.GetComponent<Animation>().Play();
		_myCameraManager.ZoomingIn = true;
	}
}