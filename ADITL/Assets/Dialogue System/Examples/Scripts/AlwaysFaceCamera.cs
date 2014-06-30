using UnityEngine;

namespace PixelCrushers.DialogueSystem.Examples {

	/// <summary>
	/// Component that keeps its game object always facing the main camera.
	/// </summary>
	public class AlwaysFaceCamera : MonoBehaviour {
		
		private Transform myTransform = null;
		
		void Awake() {
			myTransform = transform;
		}
	
		void Update() {
			if ((myTransform != null) && (Camera.main != null)) {
				myTransform.LookAt(Camera.main.transform);
			}
		}
		
	}

}
