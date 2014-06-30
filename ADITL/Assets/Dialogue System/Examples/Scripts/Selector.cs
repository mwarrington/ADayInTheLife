using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.UnityGUI;

namespace PixelCrushers.DialogueSystem.Examples {

	/// <summary>
	/// This component implements a selector that allows the player to target and use a usable 
	/// object. 
	/// 
	/// To mark an object usable, add the Usable component and a collider to it. The object's
	/// layer should be in the layer mask specified on the Selector component.
	/// 
	/// The selector can be configured to target items under the mouse cursor or the middle of
	/// the screen. When a usable object is targeted, the selector displays a targeting reticle
	/// and information about the object. If the target is in range, the inRange reticle 
	/// texture is displayed; otherwise the outOfRange texture is displayed.
	/// 
	/// If the player presses the use button (which defaults to spacebar and Fire2), the targeted
	/// object will receive an "OnUse" message.
	/// </summary>
	public class Selector : MonoBehaviour {
		
		/// <summary>
		/// This class defines the textures and size of the targeting reticle.
		/// </summary>
		[System.Serializable]
		public class Reticle {
			public Texture2D inRange;
			public Texture2D outOfRange;
			public float width = 64f;
			public float height = 64f;
		}
		
		/// <summary>
		/// Specifies how to target: center of screen or under the mouse cursor.
		/// </summary>
		public enum SelectAt { CenterOfScreen, MousePosition };
		
		/// <summary>
		/// Specifies whether to compute range from the targeted object (distance to the camera
		/// or distance to the selector's game object).
		/// </summary>
		public enum DistanceFrom { Camera, GameObject };
		
		/// <summary>
		/// The default layermask is just the Default layer.
		/// </summary>
		private static LayerMask DefaultLayer = 1;
		
		/// <summary>
		/// The layer mask to use when targeting objects. Objects on others layers are ignored.
		/// </summary>
		public LayerMask layerMask = DefaultLayer;
		
		/// <summary>
		/// How to target (center of screen or under mouse cursor). Default is center of screen.
		/// </summary>
		public SelectAt selectAt = SelectAt.CenterOfScreen;
		
		/// <summary>
		/// How to compute range to targeted object. Default is from the camera.
		/// </summary>
		public DistanceFrom distanceFrom = DistanceFrom.Camera;
		
		/// <summary>
		/// The max selection distance. The selector won't target objects farther than this.
		/// </summary>
		public float maxSelectionDistance = 30f;
		
		/// <summary>
		/// The GUI skin to use for the target's information (name and use message).
		/// </summary>
		public GUISkin guiSkin;
		
		/// <summary>
		/// The color of the information labels when the target is in range.
		/// </summary>
		public Color inRangeColor = Color.yellow;
		
		/// <summary>
		/// The color of the information labels when the target is out of range.
		/// </summary>
		public Color outOfRangeColor = Color.gray;
		
		/// <summary>
		/// The reticle images.
		/// </summary>
		public Reticle reticle;
		
		/// <summary>
		/// The key that sends an OnUse message.
		/// </summary>
		public KeyCode useKey = KeyCode.Space;
		
		/// <summary>
		/// The button that sends an OnUse message.
		/// </summary>
		public string useButton = "Fire2";
		
		/// <summary>
		/// The default use message. This can be overridden in the target's Usable component.
		/// </summary>
		public string defaultUseMessage = "(spacebar to interact)";
		
		private GameObject selection = null;
		private Usable usable = null;
		private float distance = 0;
		private GUIStyle guiStyle = null;
		
		void Update() {
			// Exit if disabled or paused:
			if (!enabled || (Time.timeScale <= 0)) return;
			
			// Exit if there's no camera:
			if (Camera.main == null) return;
			
			// Cast a ray and see what we hit:
			Ray ray = Camera.main.ScreenPointToRay(GetSelectionPoint());
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, maxSelectionDistance, layerMask)) {
				distance = (distanceFrom == DistanceFrom.Camera) ? hit.distance : Vector3.Distance(gameObject.transform.position, hit.collider.transform.position);
				if (selection != hit.collider.gameObject) {
					Usable hitUsable = hit.collider.gameObject.GetComponent<Usable>();
					if (hitUsable != null) {
						usable = hitUsable;
						selection = hit.collider.gameObject;
					} else {
						usable = null;
						selection = null;
					}
				}
			} else {
				usable = null;
				selection = null;
			}
			
			// If the player presses the use key/button, send the OnUse message:
			if (IsUseButtonDown() && (usable != null) && (distance <= usable.maxUseDistance)) {
				usable.gameObject.BroadcastMessage("OnUse", this.transform, SendMessageOptions.DontRequireReceiver);
			}
		}
		
		private bool IsUseButtonDown() {
			return ((useKey != KeyCode.None) && Input.GetKeyDown(useKey))
				|| (!string.IsNullOrEmpty(useButton)  && Input.GetButtonUp(useButton));
		}
		
		private Vector3 GetSelectionPoint() {
			switch (selectAt) {
			case SelectAt.MousePosition: return Input.mousePosition;
			default:
			case SelectAt.CenterOfScreen: return new Vector3(Screen.width / 2, Screen.height / 2);
			}
		}
		
		void OnGUI() {
			GUI.skin = UnityGUITools.GetValidGUISkin(guiSkin);
			if (guiStyle == null) {
				guiStyle = new GUIStyle(GUI.skin.label);
				guiStyle.alignment = TextAnchor.UpperCenter;
			}
			Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
			if (usable != null) {
				bool inUseRange = (distance <= usable.maxUseDistance);
				guiStyle.normal.textColor = inUseRange ? inRangeColor : outOfRangeColor;
				string heading = string.IsNullOrEmpty(usable.overrideName) ? usable.name : usable.overrideName;
				string useMessage = string.IsNullOrEmpty(usable.overrideUseMessage) ? defaultUseMessage : usable.overrideUseMessage;
				UnityGUITools.DrawText(screenRect, heading, guiStyle, TextStyle.Shadow);
				UnityGUITools.DrawText(new Rect(0, guiStyle.CalcSize(new GUIContent("Ay")).y, Screen.width, Screen.height), useMessage, guiStyle, TextStyle.Shadow);
				Texture2D reticleTexture = inUseRange ? reticle.inRange : reticle.outOfRange;
				if (reticleTexture != null) GUI.Label(new Rect(0.5f * (Screen.width - reticle.width), 0.5f * (Screen.height - reticle.height), reticle.width, reticle.height), reticleTexture);
			}
		}
		
	}

}
