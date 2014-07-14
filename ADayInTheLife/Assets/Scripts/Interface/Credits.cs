using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour
{
    private Transform[] _credits;
    private int _current = 1;

    void Awake()
    {
        _credits = GetComponentsInChildren<Transform>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (_credits[_current].position.y < -13f)
        {
            _credits[_current].position = new Vector3(_credits[_current].position.x, 14, _credits[_current].position.z);

            _current++;

            if (_current >= _credits.Length)
            {
                _current = 1;
            }
        }
        else
        {
            _credits[_current].position -= new Vector3(0, .025f, 0);
        }
    }
    
    void OnEnable()
    {
        _current = 1;
    }

    void OnDisable()
    {
        for (int i = 1; i < _credits.Length - 1; i++)
        {
            _credits[i].position = new Vector3(_credits[_current].position.x, 14, _credits[_current].position.z);
        }
    }
}

//name all objects in numerical order
//find all child objects and add to array
//for loop thru array and display one at a time at 
//next appears when previous reaches a certain point
//clouds sway back and forth