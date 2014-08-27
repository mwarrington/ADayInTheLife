using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
	private float _playerPos,
				  _endPos;

	public float PlayerPosOffset;

	void Start()
	{
		_endPos = -20f;
	}

	void Update()
	{
		_playerPos = FindObjectOfType<PlayerScript>().transform.position.x;

		if(_playerPos > _endPos)
		{
			this.transform.position = new Vector3(_playerPos + PlayerPosOffset, this.transform.position.y);
		}
	}
}
