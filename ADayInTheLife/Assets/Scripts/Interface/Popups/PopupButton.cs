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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = _myGameManager.MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (collider.Raycast(ray, out hit, 100))
            {
                if (hit.collider.name == this.name)
                {
                    if (!CloseButton)
                        Instantiate(PopupToLoad, this.transform.parent.position, this.transform.parent.rotation);
                    else if (GameObject.Find(_myGameManager.LastCharacterTalkedTo).GetComponentInChildren<Animator>() != null)
                    {
                        GameObject.Find(_myGameManager.LastCharacterTalkedTo).GetComponentInChildren<Animator>().SetBool("ProgressAchieved", true);
                        Invoke("ResetProgressForAnimator", 3);
                    }

                    Destroy(this.transform.parent.gameObject);
                }
            }
        }
    }

    private void ResetProgressForAnimator()
    {
        GameObject.Find(_myGameManager.LastCharacterTalkedTo).GetComponentInChildren<Animator>().SetBool("ProgressAchieved", false);
    }
}
