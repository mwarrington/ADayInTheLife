using UnityEngine;
using System.Collections;

public class NPCThoughtCloud : MonoBehaviour
{
    
    PlayerScript _player;

    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.FindChild("CLOSEUP").gameObject.activeSelf)
        {
            transform.FindChild("Cloud").gameObject.SetActive(true);
        }
        else
        {
            transform.FindChild("Cloud").gameObject.SetActive(false);
        }
    }
}