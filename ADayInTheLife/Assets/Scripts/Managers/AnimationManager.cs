using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationManager : MonoBehaviour
{
    private List<string> animations = new List<string>();

    private GameObject currentGameObject, LookForGameObject;
    public string currentGameObjectName;

    private float lengthOfCurrentClip;

    public int dayCountCheck = 0;
    public AudioClip sfxClip;


    // Use this for initialization
    void Start()
    {
        //dayCountCheck = GameObject.Find("GameManger").GetComponent<GameManager>().DayCount;
        switch (currentGameObjectName)
        {
            case ("Clock"):
                if (dayCountCheck == 0)
                {
                    currentGameObject = GameObject.Find("Clock_out");


                    foreach (AnimationState state in currentGameObject.animation)
                    {
                        animations.Add(state.clip.name);
                    }

                    StartCoroutine(ADayInTheLifeReset());


                }
                else if (dayCountCheck == 1)
                {
                    currentGameObject = GameObject.Find("Clock_out");


                    foreach (AnimationState state in currentGameObject.animation)
                    {
                        animations.Add(state.clip.name);
                    }

                    StartCoroutine(ADayInTheLifeLVL1Repeat());
                }
                break;
            case ("Loop"):
                animation.wrapMode = WrapMode.Loop;
                break;
        }
    }

    IEnumerator ADayInTheLifeLVL1Repeat()
    {
        bool playedOnce = false;

        //this.gameObject.animation.clip = currentClip;
        this.gameObject.animation.clip.name = this.gameObject.animation[animations[0].ToString()].name;


        lengthOfCurrentClip = this.gameObject.animation.clip.length;

        if (!playedOnce)
        {
            playedOnce = true;
            animation.Play(this.gameObject.animation.clip.name);
        }


        yield return new WaitForSeconds(lengthOfCurrentClip);

        animation.Stop();
        playedOnce = false;

        //this.gameObject.animation.clip = currentClip;
        this.gameObject.animation.clip.name = this.gameObject.animation[animations[0].ToString()].name;

        this.gameObject.animation[animations[0]].speed = -1;
        //lengthOfCurrentClip = this.gameObject.animation.clip.length;

        lengthOfCurrentClip = this.gameObject.animation.clip.length;

        this.gameObject.animation[animations[0]].time = lengthOfCurrentClip;
        if (!playedOnce)
        {
            playedOnce = true;
            animation.Play(this.gameObject.animation.clip.name);
        }


        yield return new WaitForSeconds(lengthOfCurrentClip);

        animation.Stop();
        playedOnce = false;
        animation.wrapMode = WrapMode.Loop;
        //this.gameObject.animation.clip = currentClip;
        this.gameObject.animation.clip.name = this.gameObject.animation[animations[1].ToString()].name;
        this.gameObject.animation[animations[1]].speed = 1;


        //lengthOfCurrentClip = this.gameObject.animation.clip.length;

        lengthOfCurrentClip = this.gameObject.animation.clip.length;


        this.gameObject.animation[animations[1]].time = lengthOfCurrentClip;
        if (!playedOnce)
        {
            playedOnce = true;
            animation.Play(this.gameObject.animation.clip.name);
        }

        yield return new WaitForSeconds(6);
        Application.LoadLevel("Hallway");
    }


    IEnumerator ADayInTheLifeReset()
    {
        bool playedOnce = false;

        //this.gameObject.animation.clip = currentClip;
        this.gameObject.animation.clip.name = this.gameObject.animation[animations[0].ToString()].name;


        lengthOfCurrentClip = this.gameObject.animation.clip.length;

        if (!playedOnce)
        {
            playedOnce = true;
            animation.Play(this.gameObject.animation.clip.name);

        }


        yield return new WaitForSeconds(lengthOfCurrentClip);

        animation.Stop();

        //this.gameObject.animation.clip = currentClip;
        this.gameObject.animation.clip.name = this.gameObject.animation[animations[1].ToString()].name;

        Debug.Log(this.gameObject.animation.clip.name);
        lengthOfCurrentClip = this.gameObject.animation.clip.length;

        playedOnce = false;

        if (!playedOnce)
        {
            playedOnce = true;
            animation.Play(this.gameObject.animation.clip.name);

        }

        yield return new WaitForSeconds(lengthOfCurrentClip);

        animation.Stop();

        this.gameObject.animation.clip.name = this.gameObject.animation[animations[2].ToString()].name;

        Debug.Log(this.gameObject.animation.clip.name);
        lengthOfCurrentClip = this.gameObject.animation.clip.length;

        playedOnce = false;

        if (!playedOnce)
        {
            playedOnce = true;
            animation.Play(this.gameObject.animation.clip.name);

        }

        yield return new WaitForSeconds(lengthOfCurrentClip);

        animation.Stop();

        this.gameObject.animation.clip.name = this.gameObject.animation[animations[3].ToString()].name;

        Debug.Log(this.gameObject.animation.clip.name);
        lengthOfCurrentClip = this.gameObject.animation.clip.length;

        playedOnce = false;

        if (!playedOnce)
        {
            playedOnce = true;
            animation.Play(this.gameObject.animation.clip.name);

        }

        yield return new WaitForSeconds(lengthOfCurrentClip);

        dayCountCheck = 1;
    }

    void TickTock()
    {
        if (sfxClip != null)
            this.gameObject.audio.PlayOneShot(sfxClip);
    }
}
