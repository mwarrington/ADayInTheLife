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
	private Animation _myAnimation;
	private float _startPos = -3.44f,
				  _endPos = -3.68f;
	private bool _initialAnimationStart = false;

	void Start()
	{
		_player = FindObjectOfType<PlayerScript>().gameObject;
		_myAnimation = this.GetComponent<Animation>();
	}

	void Update()
	{
		//This one is the super quick scary version
		//if(_player.transform.position.x > _endPos && _player.transform.position.x < _startPos)
		//	this.transform.position = new Vector3(_player.transform.position.x, this.transform.position.y, this.transform.position.z);
		if(_player.GetComponent<PlayerScript>().IsMoving && _initialAnimationStart)
		{
			Debug.Log ("Poopoo");
			animationPlaying = true;
		}
		else
		{
			Debug.Log ("Kaka");
			animationPlaying = false;
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player")
		{
			_myAnimation.Play();
			_initialAnimationStart = true;
			animationPlaying = true;
		}
	}
}
