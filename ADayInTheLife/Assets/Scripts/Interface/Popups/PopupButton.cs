using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class PopupButton : MonoBehaviour
{
    private GameManager _myGameManager;

    public GameObject PopupToLoad;
    public bool CloseButton;

    void Start()
    {
        _myGameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        //When player left clicks
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = _myGameManager.MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (collider.Raycast(ray, out hit, 100))
            {
                //If the click happens while the mouse is over this button's collider
                if (hit.collider.name == this.name)
                {
                    //Popup buttons either instantiate new popups or closes existing ones
                    //This checks to see if it's the close type and either closes the popup or instantiates PopupToLoad
                    if (!CloseButton)
                        Instantiate(PopupToLoad, this.transform.parent.position, this.transform.parent.rotation); //It would be nice to have the popups instantiate slightly down and to the right in the way that multiple popups do
                    else if (GameObject.Find(_myGameManager.LastCharacterTalkedTo).GetComponentInChildren<Animator>() != null) //Makes sure the LastCharacterTalkedTo has an Animator
                    {
                        //Sets the ProgressAchieved bool for the last character talked to's Animator to true once the popup is closed
                        //This makes the progress animation happen. This won't work if we have multiple popups open at once. That version would look like this:
                        //bool lastPopup = false;
                        //for (int i = 0; i < FindObjectsOfType<PopupButton>().Length; i++)
                        //{
                        //    if (FindObjectsOfType<PopupButton>()[i].transform.parent == this.transform.parent)
                        //        lastPopup = true;
                        //    else
                        //    {
                        //        lastPopup = false;
                        //        break;
                        //    }
                        //}
                        //if(lastPopup)
                        //{
                        GameObject.Find(_myGameManager.LastCharacterTalkedTo).GetComponentInChildren<Animator>().SetBool("ProgressAchieved", true);
                        GameObject.Find(_myGameManager.LastCharacterTalkedTo).GetComponent<BCCharacter>().StartCoroutine("ResetProgressForAnimator"); //After three seconds the ProgressAchieved bool is reset to false
                        //Destroy(this.transform.parent.gameObject);
                        //}
                        FindObjectOfType<PlayerScript>().CanMove = true;
                    }
                    else
                        FindObjectOfType<PlayerScript>().CanMove = true;

                    //This destorys the popup if any button is clicked.
                    //If we go with multiple popups this will be commented out or deleted.
                    Destroy(this.transform.parent.gameObject);
                }
            }
        }
    }
}
