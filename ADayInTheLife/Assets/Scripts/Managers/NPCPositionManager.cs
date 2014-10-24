using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCPositionManager : MonoBehaviour
{
    public NPCBluePrint MyBluePrint;

    void Start()
    {

    }

    void Update()
    {

    }
}

public class NPCBluePrint
{
    public Scenes MyScene;
    public List<GameObject> NPCs = new List<GameObject>();
    public Dictionary<string, Vector3> NPCPositions = new Dictionary<string, Vector3>();

    public NPCBluePrint()
    {
        MyScene = Scenes.Hallway;
    }
}
