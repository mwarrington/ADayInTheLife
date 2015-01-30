using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour
{
    private Transform[] _credits;
    private float _creditMoveSpeed = 2.5f;
    private int _current = 1,
                _last = 99;
    private bool _finsihed;

    void Awake()
    {
        _credits = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        //Debug.Log(_credits[_current].position.y);

        if (_credits[_current].position.y < -7)
        {
            _last = _current;
            _current++;

            if (_current >= _credits.Length)
            {
                _current--;
                _finsihed = true;
            }
        }
        else
        {
            _credits[_current].position -= new Vector3(0, _creditMoveSpeed * Time.deltaTime, 0);
        }

        if (_last != 99 && _finsihed && _credits[_last].position.y < -13)
        {
            _credits[_last].position = new Vector3(_credits[_last].position.x, 14, _credits[_last].position.z);
            _last = 99;
            FindObjectOfType<MainMenu>().CreditsDone = true;
        }
        else if (_last != 99)
        {
            _credits[_last].position -= new Vector3(0, _creditMoveSpeed * Time.deltaTime, 0);
        }
    }

    void OnEnable()
    {
        _current = 1;
        _last = 99;
    }

    void OnDisable()
    {
        for (int i = 1; i < _credits.Length; i++)
        {
            _credits[i].position = new Vector3(_credits[_current].position.x, 14, _credits[_current].position.z);
        }
        _finsihed = false;
    }
}

//name all objects in numerical order
//find all child objects and add to array
//for loop thru array and display one at a time at 
//next appears when previous reaches a certain point
//clouds sway back and forth