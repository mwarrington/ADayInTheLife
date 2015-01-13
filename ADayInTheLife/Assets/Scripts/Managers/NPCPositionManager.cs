using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class NPCPositionManager : MonoBehaviour
{
    public bool Disabled;
    public int HallMonitorsCount,
               HallMonitorPositions;

    protected NPCBlueprint currentBluePrint
    {
        get
        {
            switch (Application.loadedLevelName)
            {
                case "Hallway":
                    _currentBluePrint = _defaultBluePrint;
                    break;
                case "Labrary":
                    _currentBluePrint = _defaultBluePrint;
                    break;
                case "Classroom":
                    _currentBluePrint = _defaultBluePrint;
                    break;
                case "Roomclass":
                    _currentBluePrint = _defaultBluePrint;
                    break;
                case "Cafeteria":
                    if ((DialogueLua.GetVariable("GonzoProgress").AsInt == 3 && DialogueLua.GetVariable("RobynProgress").AsInt == 2) && (DialogueLua.GetVariable("GonzoAgrees").AsBool || DialogueLua.GetVariable("RobynAgrees").AsBool))
                        _currentBluePrint = (Resources.Load("Prefabs/NPCs/BluePrints/MediationCafeteriaBlueprint") as GameObject).GetComponent<NPCBlueprint>();
                    else
                        _currentBluePrint = _defaultBluePrint;
                    break;
                case "SecurityCameraRoomTest":
                    _currentBluePrint = _defaultBluePrint;
                    break;
                case "ConsularOffice":
                    _currentBluePrint = _defaultBluePrint;
                    break;
                case "ChorusRoom":
                    _currentBluePrint = _defaultBluePrint;
                    break;
                case "Lobby":
                    if ((DialogueLua.GetVariable("GonzoProgress").AsInt == 3 && DialogueLua.GetVariable("RobynProgress").AsInt == 2) && (DialogueLua.GetVariable("GonzoAgrees").AsBool || DialogueLua.GetVariable("RobynAgrees").AsBool))
                        _currentBluePrint = (Resources.Load("Prefabs/NPCs/BluePrints/MediationLobbyBlueprint") as GameObject).GetComponent<NPCBlueprint>();
                    else
                        _currentBluePrint = _defaultBluePrint;
                    break;
                case "GreenWorld":
                    if ((DialogueLua.GetVariable("GonzoProgress").AsInt == 3 && DialogueLua.GetVariable("RobynProgress").AsInt == 2) && (DialogueLua.GetVariable("GonzoAgrees").AsBool || DialogueLua.GetVariable("RobynAgrees").AsBool))
                        _currentBluePrint = (Resources.Load("Prefabs/NPCs/BluePrints/MediationGreenWorldBlueprint") as GameObject).GetComponent<NPCBlueprint>();
                    else
                        _currentBluePrint = _defaultBluePrint;
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
    private NPCBlueprint _currentBluePrint,
                         _defaultBluePrint,
                         _hallMonitorBluePrint;

    private GameManager _myGameManager;

    void Awake()
    {
        if (!Disabled)
        {
            _defaultBluePrint = (Resources.Load("Prefabs/NPCs/BluePrints/Default" + Application.loadedLevelName + "Blueprint") as GameObject).GetComponent<NPCBlueprint>();
            if (HallMonitorsCount > 0)
            {
                Debug.Log("Prefabs/NPCs/BluePrints/HallMonitor" + Application.loadedLevelName + "Blueprint");
                _hallMonitorBluePrint = (Resources.Load("Prefabs/NPCs/BluePrints/HallMonitor" + Application.loadedLevelName + "Blueprint") as GameObject).GetComponent<NPCBlueprint>();
            }

            _myGameManager = FindObjectOfType<GameManager>();

            ClearLevel();
            LevelSetup();
        }
    }

    private void LevelSetup()
    {
        NPCBlueprint lvlBluePrint = currentBluePrint;
        for (int i = 0; i < lvlBluePrint.NPCs.Count; i++)
        {
            GameObject currentNPC = (GameObject)Instantiate(lvlBluePrint.NPCs[i], lvlBluePrint.TransformList[i].position, Quaternion.identity);
            currentNPC.name = lvlBluePrint.NPCs[i].name;
        }

        GameObject hallMonitor;
        if (HallMonitorsCount > 1)
        {
            for (int i = 0; i < HallMonitorsCount; i++)
            {
                hallMonitor = (GameObject)Instantiate(_hallMonitorBluePrint.NPCs[0], _hallMonitorBluePrint.TransformList[i].position, Quaternion.identity);
                hallMonitor.name = _hallMonitorBluePrint.NPCs[0].name;

                if (_myGameManager.HasBeenIntroduced)
                    hallMonitor.GetComponent<ConversationTrigger>().trigger = DialogueTriggerEvent.OnUse;
            }
        }
        else if (HallMonitorsCount > 0)
        {
            hallMonitor = (GameObject)Instantiate(_hallMonitorBluePrint.NPCs[0], _hallMonitorBluePrint.TransformList[Random.Range(0, HallMonitorPositions)].position, Quaternion.identity);
            hallMonitor.name = _hallMonitorBluePrint.NPCs[0].name;

            if (_myGameManager.HasBeenIntroduced)
                hallMonitor.GetComponent<ConversationTrigger>().trigger = DialogueTriggerEvent.OnUse;
        }
    }

    private void ClearLevel()
    {
        List<NPCScript> NPCs = new List<NPCScript>();
        NPCs.AddRange(FindObjectsOfType<NPCScript>());

        for (int i = 0; i < NPCs.Count; i++)
        {
            Destroy(NPCs[i].gameObject);
        }
    }
}