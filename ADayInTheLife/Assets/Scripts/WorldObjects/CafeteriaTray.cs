using UnityEngine;
using System.Collections;

public class CafeteriaTray : MonoBehaviour
{
	private GameObject _player;
	private float _endPos = -25;
	
	public SpriteRenderer[] FoodTraySections = new SpriteRenderer[5];
	public Sprite[] Foods;
	public float PlayerPosOffset;

	void Start()
	{
		_player = FindObjectOfType<PlayerScript>().gameObject;
	}

	void Update()
	{
		if(this.transform.position.x > _endPos)
			this.transform.position = new Vector3 (_player.transform.position.x + PlayerPosOffset, this.transform.position.y, this.transform.position.z);
	}
}
