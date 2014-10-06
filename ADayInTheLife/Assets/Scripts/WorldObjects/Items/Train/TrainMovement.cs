using UnityEngine;
using System.Collections;

public class TrainMovement : MonoBehaviour
{
    private bool _elevatorPrince = false; //if true, the elevator shows the Prince Apall
    public bool AutoTrain;

    // Use this for initialization
    void Start()
    {
        if (this.gameObject.tag == "train" && AutoTrain)
        {
            InvokeRepeating("SendTrain", 1f, Random.Range(5f, 10f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.tag == "frontOffice")
        {
            //If press 1 and Admin Office is showing and animation is not playing.
            //=> Should be changed to use Dialogue System Animation feature so the animation is 
            //=> played based on the Player asking the Administrator to see the Prince Apall 
            if (Input.GetKey(KeyCode.Alpha1) && !_elevatorPrince && !animation.isPlaying)
            {
                animation.Play("OfficeElevatorDown");
                _elevatorPrince = true;
            }
            if (Input.GetKey(KeyCode.Alpha1) && _elevatorPrince && !animation.isPlaying)
            {
                animation.Play("OfficeElevatorUp");
                _elevatorPrince = false;
            }
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
        if(col.tag == "Player" && !AutoTrain)
        {
            this.transform.GetChild(0).animation["Train"].speed = 1.666f;
            this.transform.GetChild(0).animation.Play("Train");
            this.transform.GetChild(0).animation["TrainFall"].speed = 2.22f;
            this.transform.GetChild(0).animation.PlayQueued("TrainFall", QueueMode.CompleteOthers);
        }
    }
}
