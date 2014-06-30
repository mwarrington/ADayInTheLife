using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem {
	
	/// <summary>
	/// The conversation trigger component starts a conversation between an actor and this game 
	/// object when the game object receives a specified dialogue trigger. For example, you can use
	/// this component to start a conversation as soon as a game object or level is loaded by 
	/// choosing the OnStart event.
	/// </summary>
	public class CustomConversationTrigger : ConversationStarter {
		
		/// <summary>
		/// The actor to converse with. If not set, the game object that triggered the event.
		/// </summary>
		public Transform actor;
		
		/// <summary>
		/// The trigger that starts the conversation.
		/// </summary>
		public DialogueTriggerEvent trigger = DialogueTriggerEvent.OnUse;
		public bool gameEnd = false;
	
		
		public void OnBarkEnd(Transform actor) {
			if (enabled && (trigger == DialogueTriggerEvent.OnBarkEnd)) TryStartConversation(Tools.Select(this.actor, actor));
		}
		
		public void OnConversationEnd(Transform actor) {
			if (enabled && (trigger == DialogueTriggerEvent.OnConversationEnd)) 
			{
				TryStartConversation(Tools.Select(this.actor, actor));
			}
		}
		
		public void OnSequenceEnd(Transform actor) {
			if (enabled && (trigger == DialogueTriggerEvent.OnSequenceEnd)) TryStartConversation(Tools.Select(this.actor, actor));
		}
		
		public void OnUse(Transform actor) {
			if (enabled && (trigger == DialogueTriggerEvent.OnUse)) TryStartConversation(Tools.Select(this.actor, actor));
		}
		
		public void OnTriggerEnter(Collider other) {
			if (enabled && (trigger == DialogueTriggerEvent.OnTriggerEnter)) TryStartConversation(Tools.Select(this.actor, other.transform));
		}

		
		void Start() {
			// Waits one frame to allow all other components to finish their Start() methods.
			
		}

		void Update()
		{
			

		}

		
		void OnEnable() {
			// Waits one frame to allow all other components to finish their OnEnable() methods.
			
			if (trigger == DialogueTriggerEvent.OnEnable) StartCoroutine(StartConversationAfterOneFrame());
		}
		
		private IEnumerator StartConversationAfterOneFrame() {
			yield return null;
			TryStartConversation(actor);
		}
		
	}

}
