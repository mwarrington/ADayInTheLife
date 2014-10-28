using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Popup : MonoBehaviour
{
    public List<GameObject> PopupElements = new List<GameObject>();

    private List<BoxCollider> _clickableElements = new List<BoxCollider>();
    private List<Animation> _animatableElements = new List<Animation>();

    void Start()
    {
        for (int i = 0; i < PopupElements.Count; i++)
        {
            if(PopupElements[i].GetComponent<BoxCollider>() != null)
                _clickableElements.Add(PopupElements[i].GetComponent<BoxCollider>());

            if (PopupElements[i].GetComponent<Animation>() != null)
                _animatableElements.Add(PopupElements[i].GetComponent<Animation>());
        }
    }

    void Update()
    {

    }
}
