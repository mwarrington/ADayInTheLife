using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCPositionManager : MonoBehaviour
{
    protected NPCBlueprint currentBluePrint
    {
        get
        {
            switch (Application.loadedLevelName)
            {
                case "Hallway":
                    break;
                case "Labrary":
                    break;
                case "Classroom":
                    break;
                case "Roomclass":
                    break;
                case "Cafeteria":
                    break;
                case "SecurityCameraRoomTest":
                    break;
                case "ConsularOffice":
                    break;
                case "ChorusRoom":
                    break;
                case "Lobby":
                    break;
                case "GreenWorld":
                    break;
                default:
                    Debug.Log("That Scene Doesn't Exist!!!");
                    break;
            }

            return _currentBluePrint;
        }
        set
        {
            _currentBluePrint = value;
        }
    }
    private NPCBlueprint _currentBluePrint;

    private GameManager _myGameManager;

    void Start()
    {
        _myGameManager = FindObjectOfType<GameManager>();
        
    }

    void Update()
    {

    }
}
