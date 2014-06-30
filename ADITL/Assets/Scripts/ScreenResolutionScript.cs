using UnityEngine;
using System.Collections;

public class ScreenResolutionScript : MonoBehaviour
{
	public Texture2D borderImage;
	public Camera[] cameras;
	public static float targetaspect = 16.0f / 9.0f;
	
    //public Camera camera;
    // Use this for initialization
    void Start()
    {
        
            foreach (Camera camera in cameras)
            {
                // determine the game window's current aspect ratio
                float windowaspect = (float)Screen.width / (float)Screen.height;

                // current viewport height should be scaled by this amount
                float scaleheight = windowaspect / targetaspect;

                // if scaled height is less than current height, add letterbox
                if (scaleheight < 1.0f)
                {
                    Rect rect = camera.rect;

                    rect.width = camera.rect.width * 1.0f;
                    rect.height = camera.rect.height * scaleheight;
                    rect.x = camera.rect.x + 0;
                    rect.y = (1.0f - rect.height) / 2.0f;

                    camera.rect = rect;
                }
                else // add pillarbox
                {
                    float scalewidth = 1.0f / scaleheight;

                    Rect rect = camera.rect;

                    rect.width = camera.rect.width * scalewidth;
                    rect.height = camera.rect.height * 1.0f;
                    rect.x = (1.0f - rect.width) / 2.0f;
                    rect.y = camera.rect.y + 0;

                    camera.rect = rect;
                }
            }
        
    }
	
	static public void SetGUIProps()
	{
		GUI.matrix = Matrix4x4.TRS(
			new Vector3(1, Screen.height * Camera.main.rect.y, 1),
			Quaternion.identity,
			new Vector3(Camera.main.rect.width, Camera.main.rect.height, 1)
		);
		GUI.depth = 1;
	}
	
	void OnGUI()
	{
		Rect r = new Rect(0, 0, Screen.width, (Screen.height * (1.0f - Camera.main.rect.height)) / 2.0f);
		int tmp = GUI.depth;
		GUI.depth = 10000;
		GUI.DrawTexture(r, borderImage, ScaleMode.StretchToFill, true);
		r.y = Screen.height * (Camera.main.rect.y + Camera.main.rect.height);
		GUI.DrawTexture(r, borderImage, ScaleMode.StretchToFill, true);
		GUI.depth = tmp;
	}
}
