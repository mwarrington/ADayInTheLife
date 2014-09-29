using UnityEngine;
using System.Collections;

public class RareMetalsScript : MonoBehaviour
{

    public GameObject RareMetalFemaleSFX,
                      RareMetalMaleSFX;

    public GameObject BackRow,
                      MidForeRow,
                      ForeRow,
                      MidBackRow;

    void Start()
    {
        BackRow = GameObject.Find("Back");
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!RareMetalFemaleSFX.audio.isPlaying)
            {
                RareMetalFemaleSFX.audio.Play();
                BackRow.animation.Play("RareMetalFemales");
            }

        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!RareMetalMaleSFX.audio.isPlaying)
                RareMetalMaleSFX.audio.Play();
            MidForeRow.animation.Play("RareMetalMales");
        }

    }
}
