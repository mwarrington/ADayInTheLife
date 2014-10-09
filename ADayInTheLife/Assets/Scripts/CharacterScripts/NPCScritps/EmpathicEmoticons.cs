using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmpathicEmoticons : MonoBehaviour
{
    public bool EmoticonsLoaded
    {
        get
        {
            return emoticonsLoaded;
        }
        set
        {
            emoticonsLoaded = value;
        }
    }
    static bool emoticonsLoaded;
    public Dictionary<string, Sprite> SpriteDictionary
    {
        get
        {
            return spriteDictionary;
        }
    }
	static Dictionary<string, Sprite> spriteDictionary = new Dictionary<string, Sprite>();
	private string[] _emoticons = new string[16];

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

        if (!emoticonsLoaded)
        {
            spriteDictionary.Clear();
            switch (this.GetComponent<GameManager>().DayCount)
            {
                case 1:
                    for (int i = 0; i < 16; i++)
                    {
                        spriteDictionary.Add(_emoticons[i], Resources.Load<Sprite>("NPCs/NPCEmotions/Cycle1/" + _emoticons[i]));
                    }
                    break;
                case 2:
                    for (int i = 0; i < 16; i++)
                    {
                        spriteDictionary.Add(_emoticons[i], Resources.Load<Sprite>("NPCs/NPCEmotions/Cycle2/" + _emoticons[i]));
                    }
                    break;
                case 3:
                    for (int i = 0; i < 16; i++)
                    {
                        spriteDictionary.Add(_emoticons[i], Resources.Load<Sprite>("NPCs/NPCEmotions/Cycle3/" + _emoticons[i]));
                    }
                    break;
                default:
                    Debug.Log("Hmm this shouldn't be happening... Check the public accessor for DayCount in GameManager.cs");
                    break;
            }

            emoticonsLoaded = true;
        }
    }
}
