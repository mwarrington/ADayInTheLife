using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	public bool enabled;

    void Start()
    {
        
    }
   
    void Update() {

        if (enabled)
            this.gameObject.camera.active = true;
        else
            this.gameObject.camera.active = false;

    	
	}

    void ToggleCamera()
    {
        if (enabled)
            enabled = false;
        else
            enabled = true;
    }

	
}
