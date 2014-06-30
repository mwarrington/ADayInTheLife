using UnityEngine;

namespace PixelCrushers.DialogueSystem.Examples {

	/// <summary>
	/// When you attach this script to an actor, conversations involving that actor will be
	/// logged to the console.
	/// </summary>
	public class ConversationLogger : MonoBehaviour {

		public GameObject closeCamera;
		PlayerScript player;

		void Start()
		{
			player = GameObject.Find("Player").GetComponent<PlayerScript>();
		}
		
		public void OnConversationStart(Transform actor) {
			Debug.Log(string.Format("{0}: Starting conversation with {1}", name, actor.name));
			closeCamera.active = true;	
			player.thoughtCloud.renderer.enabled = false;
			player.spiral.renderer.enabled = false;
			player.inConversation = true;
		}
		
		public void OnConversationLine(Subtitle subtitle) {
			if (string.IsNullOrEmpty(subtitle.formattedText.text)) return;
			Debug.Log(string.Format("<color={0}>{1}: {2}</color>", GetActorColor(subtitle), subtitle.speakerInfo.transform.name, subtitle.formattedText.text));
		}
		
		public void OnConversationEnd(Transform actor) {
			Debug.Log(string.Format("{0}: Ending conversation with {1}", name, actor.name));
			closeCamera.active = false;	
			player.thoughtCloud.renderer.enabled = true;
			player.spiral.renderer.enabled = true;
			player.inConversation = false;

			if (this.gameObject.tag == "destroy")
				this.gameObject.GetComponent<ConversationTrigger>().active =false;
		}
		
		private string GetActorColor(Subtitle subtitle) {
			return subtitle.speakerInfo.IsPlayer ? "blue" : "red";
		}
		
	}

}
