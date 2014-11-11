using UnityEngine;
using System.Collections;

public class WeLearnByRepetition : MonoBehaviour
{
    public AudioClip noseDive,
                     cartoonSpiral,
                     boing;

    private GameManager _myManager;

    void Start()
    {
        _myManager = FindObjectOfType<GameManager>();
    }

    void Rep1()
    {
        //number 0 animation! by repetition
        if (noseDive != null)
            this.gameObject.audio.PlayOneShot(noseDive);
    }

    void Repeti1()
    {
        //number 1 animation! by repetition
        if (cartoonSpiral != null)
            this.gameObject.audio.PlayOneShot(cartoonSpiral);
    }

    void Repetition1()
    {
        //number 2 animation! by repetition
        if (boing != null)
        {
            this.gameObject.audio.PlayOneShot(boing);
            foreach (TextMesh tm in this.GetComponentsInChildren<TextMesh>())
            {
                tm.text = _myManager.DayCount.ToString();
            }
        }
    }
}
