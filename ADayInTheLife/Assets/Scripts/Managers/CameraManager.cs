using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
	private GameManager _myManager;
	private GameObject _player;
	private float _playerPos,
				  _endPos;

	public float PlayerPosOffset;

	void Start()
	{
		_myManager = FindObjectOfType<GameManager>();
		_player = FindObjectOfType<PlayerScript> ().gameObject;
		_endPos = -20f;
	}

	void Update()
	{
		CameraControl ();
	}

	private void CameraControl()
	{
		switch(FindObjectOfType<PlayerScript>().CurrentScene)
		{
			case Scenes.Cafeteria:
				_playerPos = FindObjectOfType<PlayerScript>().transform.position.x;
				
				if(_playerPos > _endPos)
				{
					this.transform.position = new Vector3(_playerPos + PlayerPosOffset, this.transform.position.y);
				}
				break;
			case Scenes.SecCamTemp:
				this.transform.LookAt(_player.transform);
				break;
			default:
				Debug.Log ("There shouldn't be a CameraManager in this scene");
				break;
		}
	}
}