using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationManager : MonoBehaviour
{
    private List<string> animations = new List<string>();

    private GameObject currentGameObject, LookForGameObject;
    private GameManager _myGameManager;
    public string currentGameObjectName;

    private float lengthOfCurrentClip;

    public int dayCountCheck = 0;
    public AudioClip sfxClip;

    // Use this for initialization
    void Start()
    {
        _myGameManager = FindObjectOfType<GameManager>();
        //dayCountCheck = GameObject.Find("GameManger").GetComponent<GameManager>().DayCount;
        switch (currentGameObjectName)
        {
            case ("Clock"):
                if (dayCountCheck == 0)
                {
                    foreach (AnimationState state in this.animation)
                    {
                        animations.Add(state.clip.name);
                    }

                    StartCoroutine(ADayInTheLifeReset());
                }
                else if (dayCountCheck == 1)
                {
                    foreach (AnimationState state in this.animation)
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
        this.gameObject.animation.clip.name = this.gameObject.animation[animations[0]].name;

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
        this.gameObject.animation.clip.name = this.gameObject.animation[animations[0]].name;

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
        this.gameObject.animation.clip.name = this.gameObject.animation[animations[1]].name;
        this.gameObject.animation[animations[1]].speed = 1;

        //lengthOfCurrentClip = this.gameObject.animation.clip.length;

        lengthOfCurrentClip = this.gameObject.animation.clip.length;

        this.gameObject.animation[animations[1]].time = lengthOfCurrentClip;
        if (!playedOnce)
        {
            playedOnce = true;
            animation.Play(this.gameObject.animation.clip.name);
        }

        yield return new WaitForSeconds(1.5f);

        this.gameObject.animation[animations[1]].speed *= -1;

        yield return new WaitForSeconds(1.5f);

        this.gameObject.animation[animations[1]].speed *= -1;
        FindObjectOfType<GameManager>().FadingAway = true;

        yield return new WaitForSeconds(1.5f);

        this.gameObject.animation[animations[1]].speed *= -1;

        yield return new WaitForSeconds(1.5f);

        LoadLevel();
    }


    IEnumerator ADayInTheLifeReset()
    {
        bool playedOnce = false;

        //this.gameObject.animation.clip = currentClip;
        this.gameObject.animation.clip.name = this.gameObject.animation[animations[0]].name;

        lengthOfCurrentClip = this.gameObject.animation.clip.length;

        if (!playedOnce)
        {
            playedOnce = true;
            animation.Play(this.gameObject.animation.clip.name);
        }

        yield return new WaitForSeconds(lengthOfCurrentClip);

        animation.Stop();

        //this.gameObject.animation.clip = currentClip;
        this.gameObject.animation.clip.name = this.gameObject.animation[animations[1]].name;

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

        this.gameObject.animation.clip.name = this.gameObject.animation[animations[2]].name;

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

        this.gameObject.animation.clip.name = this.gameObject.animation[animations[3]].name;

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

    private void LoadLevel()
    {
        switch (_myGameManager.LevelCount)
        {
            case 1:
                Application.LoadLevel("Hallway");
                break;
            case 2:
                Debug.Log("Sup");
                Application.LoadLevel("Lobby");
                break;
            case 3:
                Debug.Log("sdf");
                break;
            default:
                Debug.Log("There are only three levels so why are you on " + _myGameManager.LevelCount + "?");
                break;
        }
    }
}
