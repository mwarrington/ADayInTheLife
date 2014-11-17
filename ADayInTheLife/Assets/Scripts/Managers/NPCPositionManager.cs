﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class NPCPositionManager : MonoBehaviour
{
    public bool Disabled;

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
                         _defaultBluePrint;

    private GameManager _myGameManager;

    void Start()
    {
        if (!Disabled)
        {
            _defaultBluePrint = (Resources.Load("Prefabs/NPCs/BluePrints/Default" + Application.loadedLevelName + "Blueprint") as GameObject).GetComponent<NPCBlueprint>();
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
