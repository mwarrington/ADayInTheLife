using UnityEngine;
using System.Collections;

public class PopupManager : MonoBehaviour
{
    public GameObject[] MyPopups;
    public Transform InstantiationTransform;

    protected GameObject currentPopup
    {
        get
        {
            _currentPopup = MyPopups[0];
            return _currentPopup;
        }
        set
        {
            Debug.Log("You shouldn't be trying to set the value of currentPopup...");
        }
    }
    private GameObject _currentPopup;
    private GameManager _myGameManager;

    void Update()
    {
        _myGameManager = this.GetComponent<GameManager>();
        if (Input.GetKeyDown(KeyCode.P))
        {
            ShowPopup();
        }
    }

    public void ShowPopup()
    {
        GameObject popup = (GameObject)Instantiate(currentPopup, InstantiationTransform.position, InstantiationTransform.rotation);
        popup.transform.parent = _myGameManager.MainCamera.transform;
    }
}