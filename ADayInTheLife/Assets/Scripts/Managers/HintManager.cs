using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class HintManager : MonoBehaviour
{
	public GameObject SceneCurtains,
					  DeskCurtains;

	protected bool colliderActive
	{
		get
		{
			if(_myCameraManager.ZoomingIn && !SceneCurtains.GetComponent<Animation>().isPlaying)
				_colliderActive = false;
			else
				_colliderActive = true;

			return _colliderActive;
		}
	}
	private bool _colliderActive;

	private CameraManager _myCameraManager;
	private BoxCollider _samBoxCollider;

	void Start()
	{
		_myCameraManager = FindObjectOfType<CameraManager>();
		_samBoxCollider = this.GetComponent<BoxCollider>();
		_samBoxCollider.enabled = false;
	}

	void Update()
	{
		if(colliderActive)
		{
			_samBoxCollider.enabled = true;
		}
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
