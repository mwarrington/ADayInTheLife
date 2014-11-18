using UnityEngine;
using System.Collections;

public class AnimationEventManager : MonoBehaviour
{
    private NPCScript _myNPCScript;

    void Start()
    {
        if (this.transform.parent.GetComponent<NPCScript>() != null)
            _myNPCScript = this.transform.parent.GetComponent<NPCScript>();
    }

    private void CallOut()
    {
        _myNPCScript.MyVoice.clip = Resources.Load("Sounds/SFX/NPCCalls/" + this.transform.parent.name + "Call") as AudioClip;
        _myNPCScript.MyVoice.Play();
    }
}