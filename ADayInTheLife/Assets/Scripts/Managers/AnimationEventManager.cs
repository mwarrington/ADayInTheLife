using UnityEngine;
using System.Collections;

public class AnimationEventManager : MonoBehaviour
{
    private NPCScript _myNPCScript;

    void Start()
    {
        //Finds the NPC script for this object
        if (this.transform.parent.GetComponent<NPCScript>() != null)
            _myNPCScript = this.transform.parent.GetComponent<NPCScript>();
    }

    //This method sets and plays the NPCs "Call Out" sound. This sound is used to get player attention. ei: "Your Majesty"
    private void CallOut()
    {
        _myNPCScript.MyVoice.clip = Resources.Load("Sounds/SFX/NPCCalls/" + this.transform.parent.name + "Call") as AudioClip;
        _myNPCScript.MyVoice.Play();
    }
}