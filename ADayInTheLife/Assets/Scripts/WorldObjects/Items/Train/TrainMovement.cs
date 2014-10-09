using UnityEngine;
using System.Collections;

public class TrainMovement : MonoBehaviour
{
    public bool AutoTrain;

    void Start()
    {
        if (this.gameObject.tag == "train" && AutoTrain)
        {
            InvokeRepeating("SendTrain", 1f, Random.Range(5f, 10f));
        }
    }

    private void SendTrain()
    {
        if (!animation.isPlaying)
        {
            animation["Train"].speed = Random.Range(.222f, 1.666f);
            animation.Play("Train");
            animation["TrainFall"].speed = 2.22f;
            animation.PlayQueued("TrainFall", QueueMode.CompleteOthers);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player" && !AutoTrain && !this.transform.GetChild(0).animation.isPlaying)
        {
            this.transform.GetChild(0).animation["Train"].speed = 1.666f;
            this.transform.GetChild(0).animation.Play("Train");
            this.transform.GetChild(0).animation["TrainFall"].speed = 2.22f;
            this.transform.GetChild(0).animation.PlayQueued("TrainFall", QueueMode.CompleteOthers);
        }
    }
}