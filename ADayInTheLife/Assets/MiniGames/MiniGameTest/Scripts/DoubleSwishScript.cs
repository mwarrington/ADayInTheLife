using UnityEngine;
using System.Collections;

public class DoubleSwishScript : MonoBehaviour
{
    private int _swishCount = 0;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            animation.Play();
            Invoke("CountIt", 0.75f);
            Invoke("CountIt", 1f);
        }
    }

    private void CountIt()
    {
        _swishCount++;
    }
}
