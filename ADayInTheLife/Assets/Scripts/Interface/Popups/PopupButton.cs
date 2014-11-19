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
                    else if (DialogueLua.GetVariable("MarkeshiaProgress").AsInt == 2)
                    {
                        GameObject.Find("Markeshia").GetComponentInChildren<Animator>().SetBool("ProgressAchieved", true);
                    }

                    Destroy(this.transform.parent.gameObject);
                }
            }
        }
    }
}
