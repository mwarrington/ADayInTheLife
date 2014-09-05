using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmpathicEmoticons : MonoBehaviour
{
	public Dictionary<string, Sprite> SpriteDictionary = new Dictionary<string, Sprite>();
	//Replace with a Resource Load thing
	public string[] _emoticons = new string[16];

	void Start()
	{
		_emoticons[0] = "AngryAngry";
		_emoticons[1] = "AngryFearful";
		_emoticons[2] = "AngryHappy";
		_emoticons[3] = "AngrySad";
		_emoticons[4] = "FearfulAngry";
		_emoticons[5] = "FearfulFearful";
		_emoticons[6] = "FearfulHappy";
		_emoticons[7] = "FearfulSad";
		_emoticons[8] = "HappyAngry";
		_emoticons[9] = "HappyFearful";
		_emoticons[10] = "HappyHappy";
		_emoticons[11] = "HappySad";
		_emoticons[12] = "SadAngry";
		_emoticons[13] = "SadFearful";
		_emoticons[14] = "SadHappy";
		_emoticons[15] = "SadSad";

		switch(this.GetComponent<GameManager>().LevelCount)
		{
			case 1:
				for(int i = 0; i < 16; i++)
				{
					SpriteDictionary.Add(_emoticons[i], Resources.Load<Sprite>("NPCs/NPCEmotions/Level1/" + _emoticons[i]));
				}
				break;
			case 2:
				for(int i = 0; i < 16; i++)
				{
					SpriteDictionary.Add(_emoticons[i], Resources.Load<Sprite>("NPCs/NPCEmotions/Level2/" + _emoticons[i]));
				}
				break;
			case 3:
				for(int i = 0; i < 16; i++)
				{
					SpriteDictionary.Add(_emoticons[i], Resources.Load<Sprite>("NPCs/NPCEmotions/Level3/" + _emoticons[i]));
				}
				break;
			default:
				Debug.Log ("What!? The level couldn't possibly be higher then 3 or lower than 1. Your math must be off. This is terribly disappointing...");
				break;
		}
	}
}
