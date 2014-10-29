using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCPositionManager : MonoBehaviour
{
    public NPCBlueprint TestBluePrint;

    protected NPCBlueprint currentBluePrint
    {
        get
        {
            switch (Application.loadedLevelName)
            {
                case "Hallway":
                    _currentBluePrint = TestBluePrint;
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
        LevelSetup();
    }

    private void LevelSetup()
    {
        for (int i = 0; i < currentBluePrint.NPCs.Count; i++)
        {
            //GameObject currentNPC = Resources.Load<GameObject>("Prefabs/NPCs/Level" + _myGameManager.LevelCount + "/" + _currentBluePrint.NPCs[i].name);
            Instantiate(currentBluePrint.NPCs[i], currentBluePrint.TransformList[i].position, Quaternion.identity);
        }
    }
}
