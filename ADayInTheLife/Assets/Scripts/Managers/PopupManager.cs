using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class PopupManager : MonoBehaviour
{
    public GameObject[] MyPopups;

    private GameObject _currentPopup;

    private void OnConversationEnd(Transform actor)
    {
        for (int i = 0; i < DialogueManager.MasterDatabase.variables.Count; i++)
        {
            if (DialogueManager.MasterDatabase.variables[i].fields[2].value == "WinCondition")
            {
                if (DialogueLua.GetVariable(DialogueManager.MasterDatabase.variables[i].Name).AsBool == true)
                {
                    ShowPopup();
                    Invoke("RemovePopup", 5);
                    break;
                }
            }
        }
    }

    private void ShowPopup()
    {
        Debug.Log("Hello!");
    }

    private void RemovePopup()
    {
        Debug.Log("Goodbye...");
    }
}