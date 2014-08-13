using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmpathicEmoticons : MonoBehaviour
{
	public Dictionary<string, Sprite> SpriteDictionary = new Dictionary<string, Sprite>();
	public Sprite[] Level1Sprites,
					Level2Sprites,
					Level3Sprites;

	void Start()
	{
		switch(this.GetComponent<GameManager>().LevelCount)
		{
			case 1:
				for(int i = 0; i < Level1Sprites.Length; i++)
				{
					SpriteDictionary.Add(Level1Sprites[i].name, Level1Sprites[i]);
				}
				break;
			case 2:
				for(int i = 0; i < Level2Sprites.Length; i++)
				{
					SpriteDictionary.Add(Level2Sprites[i].name, Level2Sprites[i]);
				}
				break;
			case 3:
				for(int i = 0; i < Level3Sprites.Length; i++)
				{
					SpriteDictionary.Add(Level3Sprites[i].name, Level3Sprites[i]);
				}
				break;
			default:
				Debug.Log ("What!? The level couldn't possibly be higher then 3 or lower than 1. Your math must be off.");
				break;
		}
	}
}
