using UnityEngine;

namespace PixelCrushers.DialogueSystem.Examples {
	
	/// <summary>
	/// This component implements a very simple third person shooter-style controller.
	/// The mouse rotates the character, and vertical axis (up/down or W/S keys) moves
	/// the character forward and back.
	/// </summary>
	public class SimpleController : MonoBehaviour {
		
		public AnimationClip idle;
		public AnimationClip runForward;
		public AnimationClip runBack;
		public float runSpeed = 5f;
		public float mouseSensitivityX = 15f;
		public float mouseSensitivityY = 10f;
		public float mouseMinimumY = -60f;
	    public float mouseMaximumY = 60f;
		
		private CharacterController controller = null;
		private float speed = 0;
		private float velocity = 0;
	    private float cameraRotationY = 0F;
	    private Quaternion originalCameraRotation;
		
		void Awake() {
			controller = GetComponent<CharacterController>();
		}
		
		void Start() {
			originalCameraRotation = Camera.main.transform.localRotation;
		}
	
		void Update() {
			if (Time.timeScale <= 0) return;
			
			// Mouse rotation:
			transform.Rotate(0, Input.GetAxis("Mouse X") * mouseSensitivityX, 0);
			Camera.main.transform.rotation = transform.rotation;
			cameraRotationY += Input.GetAxis("Mouse Y") * mouseSensitivityY;
			cameraRotationY = ClampAngle (cameraRotationY, mouseMinimumY, mouseMaximumY);
			Quaternion yQuaternion = Quaternion.AngleAxis (cameraRotationY, -Vector3.right);
			Camera.main.transform.localRotation = originalCameraRotation * yQuaternion;
			
			// Movement:
			float targetSpeed = Input.GetAxis("Vertical");
			speed = Mathf.SmoothDamp(speed, targetSpeed, ref velocity, 0.3f);
			if (targetSpeed > 0.1f) {
				animation[runForward.name].speed = 1;
				animation.CrossFade(runForward.name);
			} else if (targetSpeed < -0.1f) {
				if (runBack != null) {
					animation.CrossFade(runBack.name);
				} else {
					animation[runForward.name].speed = -1;
					animation.CrossFade(runForward.name);
				}
			} else {
				animation.CrossFade(idle.name);
			}
			
			// Move, including gravity:
			controller.Move(transform.rotation * (Vector3.forward * runSpeed * speed * Time.deltaTime) + Vector3.down * 20f * Time.deltaTime);
		}
		
		/// <summary>
		/// When the character is involved in a conversation, stop moving and play the idle animation.
		/// </summary>
		void OnConversationStart() {
			animation.CrossFade(idle.name);
			speed = 0;
			velocity = 0;
		}
		
		public static float ClampAngle (float angle, float min, float max) {
			if (angle < -360f) angle += 360f;
			if (angle > 360f) angle -= 360f;
			return Mathf.Clamp(angle, min, max);
		}
			 
	}
	
}
