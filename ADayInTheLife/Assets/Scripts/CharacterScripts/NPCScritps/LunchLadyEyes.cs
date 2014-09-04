using UnityEngine;
using System.Collections;

public class LunchLadyEyes : MonoBehaviour
{
	protected bool animationPlaying
	{
		get
		{
			return _animationPlaying;
		}
		set
		{
			if(_animationPlaying != value)
			{
				if(value)
					_myAnimation.Play();
				else
					_myAnimation.Stop();

				_animationPlaying = value;
			}
		}
	}
	private bool _animationPlaying = false;

	private GameObject _player;
	private Transform _myLunchLady;
	private Animation _myAnimation;
	private float _startPos,
				  _endPos;
	private bool _initialAnimationStart = false;

	public bool QuickEyeFollow;

	void Start()
	{
		_player = FindObjectOfType<PlayerScript>().gameObject;
		_myLunchLady = this.transform.parent;
		_myAnimation = this.GetComponent<Animation>();
	}

	void Update()
	{
		if(QuickEyeFollow)//This one is the super quick scary version
		{
			_startPos = _myLunchLady.position.x + 0.12f;
			_endPos = _myLunchLady.position.x - 0.1f;

			if(_player.transform.position.x > _endPos && _player.transform.position.x < _startPos)
				this.transform.position = new Vector3(_player.transform.position.x - 0.1f, this.transform.position.y, this.transform.position.z);
		}
		else//This is the slower more gradual version
		{
			if(_player.GetComponent<PlayerScript>().IsMoving && _initialAnimationStart)
			{
				animationPlaying = true;
			}
			else
			{
				animationPlaying = false;
			}
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player" && !QuickEyeFollow)
		{
			_myAnimation.Play();
			_initialAnimationStart = true;
			animationPlaying = true;
		}
	}
}
