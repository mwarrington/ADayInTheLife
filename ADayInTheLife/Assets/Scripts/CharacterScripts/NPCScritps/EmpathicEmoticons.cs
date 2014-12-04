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

    //This is a dictionary of all of the EE sprites orginized by name.
    //It's a static field with a public accessor
    public Dictionary<string, Sprite> SpriteDictionary
    {
        get
        {
            return spriteDictionary;
        }
    }
	static Dictionary<string, Sprite> spriteDictionary = new Dictionary<string, Sprite>();

    //An array of strings to used to populate the spriteDictionary
	private string[] _emoticons = new string[16];

    void Start()
    {
        //Each EE name that we use to populate the spriteDictionary
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

        //Populates spriteDictionary if we haven't already populated the spriteDictionary
        if (!emoticonsLoaded)
        {
            //Clears the old spriteDictionary
            spriteDictionary.Clear();

            if (this.GetComponent<GameManager>().DayCount == 0) //If there hasn't been a day repetition yet.
            {
                for (int i = 0; i < 16; i++)
                {
                    spriteDictionary.Add(_emoticons[i], Resources.Load<Sprite>("Art/Textures/NPCs/NPCEmotions/Cycle1/" + _emoticons[i]));
                }
            }
            else if (this.GetComponent<GameManager>().DayCount == 1) //If there has been 1 day repetition.
            {
                for (int i = 0; i < 16; i++)
                {
                    spriteDictionary.Add(_emoticons[i], Resources.Load<Sprite>("Art/Textures/NPCs/NPCEmotions/Cycle2/" + _emoticons[i]));
                }
            }
            else if (this.GetComponent<GameManager>().DayCount >= 2) //If there has been 2 or more day repetitions.
            {
                for (int i = 0; i < 16; i++)
                {
                    spriteDictionary.Add(_emoticons[i], Resources.Load<Sprite>("Art/Textures/NPCs/NPCEmotions/Cycle3/" + _emoticons[i]));
                }
            }

            //Makes sure this only happens once per day
            emoticonsLoaded = true;
        }
    }
}
