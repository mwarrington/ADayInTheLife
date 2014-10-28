using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCBlueprint : MonoBehaviour
{
    public Scenes MyScene;
    public List<GameObject> NPCs = new List<GameObject>();
    public Dictionary<string, Vector3> NPCPositions = new Dictionary<string, Vector3>();
}
