using UnityEngine;
using System.Collections;

public class CharacterTilt
{

    private GameObject _character;
    private bool _tilt,
                 _switchDirections;

    public CharacterTilt(GameObject character, bool switchDir)
    {
        _character = character;
        _switchDirections = switchDir;
    }

    public void Update()
    {
        PortraitTilt();
    }

    void PortraitTilt()
    {

        if (!_tilt)
        {
            if (!_switchDirections)
            {
                _character.transform.eulerAngles += new Vector3(16f * Time.deltaTime, 0, 0);

                if (_character.transform.eulerAngles.x >= 10 && _character.transform.eulerAngles.x <= 100)
                {
                    _tilt = true;
                }
            }
            else
            {
                _character.transform.eulerAngles -= new Vector3(16f * Time.deltaTime, 0, 0);

                if (_character.transform.eulerAngles.x <= 350 && _character.transform.eulerAngles.x > 100)
                {
                    _tilt = true;
                }
            }
        }
        else
        {
            if (!_switchDirections)
            {
                _character.transform.eulerAngles -= new Vector3(16f * Time.deltaTime, 0, 0);

                if (_character.transform.eulerAngles.x <= 350 && _character.transform.eulerAngles.x > 100)
                {
                    _tilt = false;
                }
            }
            else
            {
                _character.transform.eulerAngles += new Vector3(16f * Time.deltaTime, 0, 0);

                if (_character.transform.eulerAngles.x >= 10 && _character.transform.eulerAngles.x <= 100)
                {
                    _tilt = false;
                }
            }

        }
    }
}