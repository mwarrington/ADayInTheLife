using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCBlueprint : MonoBehaviour
{
    public Scenes MyScene;
    public List<GameObject> NPCs = new List<GameObject>();
    public List<Transform> TransformList = new List<Transform>();
    //public Dictionary<string, Vector3> NPCPositions = new Dictionary<string, Vector3>();

    //void Start()
    //{
    //    for (int i = 0; i < NPCs.Count; i++)
    //    {
    //        NPCPositions.Add(NPCs[i].name, PositionList[i]);
    //    }
    //}
}
