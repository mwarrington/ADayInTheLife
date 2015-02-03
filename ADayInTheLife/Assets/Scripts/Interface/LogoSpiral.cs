using UnityEngine;
using System.Collections;

public class LogoSpiral : MonoBehaviour
{
    private bool _canRotate = true;

    // Use this for initialization 
    void Start()
    {

    }

    void Update()
    {
        if (_canRotate)
        {
            transform.Rotate(new Vector3(0, 50 * Time.deltaTime, 0));
        }
    }

    void OnMouseDrag()
    {
        _canRotate = false;

        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
        float x = -Input.GetAxis("Mouse X");
        float y = -Input.GetAxis("Mouse Y");
        float speed = 10;

        transform.rotation *= Quaternion.AngleAxis(x * speed, Vector3.up);
        transform.rotation *= Quaternion.AngleAxis(y * speed, Vector3.up);
    }

    void OnMouseEnter()
    {
        //_canRotate = false;
    }

    void OnMouseExit()
    {
        _canRotate = true;
    }
}