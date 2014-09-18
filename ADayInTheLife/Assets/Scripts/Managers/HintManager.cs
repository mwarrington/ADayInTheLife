using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.Examples;

public class HintManager : MonoBehaviour
{
	public GameObject SceneCurtains,
					  DeskCurtains;

	protected bool colliderActive
	{
		get
		{
			if((_myCameraManager.ZoomingIn || SceneCurtains.GetComponent<Animation>().isPlaying))
				_colliderActive = false;
			else
				_colliderActive = true;

			return _colliderActive;
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
		_playerProxSelector.enabled = false;
	}

	void Update()
	{
		if(colliderActive)
		{
			_player.transform.position = new Vector3(_player.transform.position.x + 1, _player.transform.position.y, _player.transform.position.z);
			_playerProxSelector.enabled = true;
			_player.transform.position = new Vector3(_player.transform.position.x - 1, _player.transform.position.y, _player.transform.position.z);
		}
		else
			_playerProxSelector.enabled = false;

		if(!_myCameraManager.ZoomingIn)
		{
			DeskCurtains.SampleAnimation(DeskCurtains.GetComponent<Animation>().clip, 0);
		}
	}

	void OnConversationEnd (Transform actor)
	{
		if(DialogueLua.GetVariable("ShowHint").AsBool)
		{
			ShowHint();
			DialogueLua.SetVariable("ShowHint", false);
		}
	}

	private void ShowHint()
	{
		DeskCurtains.GetComponent<Animation>().Play();
		_myCameraManager.ZoomingIn = true;
	}
}
