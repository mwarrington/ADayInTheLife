using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class PlayerDecisionLogger : MonoBehaviour {
	void Start () {
		// Option 1: Listen to change in Lua variables -- requires having Lua variables set for every dialogue branch
		// DialogueManager.AddLuaObserver("Variable['decision']", LuaWatchFrequency.EveryDialogueEntry, OnDecision);
	}

	//Option 1 Event Handler
	void OnDecision	(LuaWatchItem luaWatchItem, Lua.Result newValue){
		StartCoroutine(LogDecision("unityPlayerId", "conversationId", newValue.AsString));
	}

	//Option 2: listen to when conversation lines are spoken
	//Question: Does this have the information we want?
	void OnConversationLine(Subtitle subtitle){
		if (ShouldLog(subtitle)) {
			StartCoroutine (LogDecision (subtitle.speakerInfo.nameInDatabase, subtitle.listenerInfo.nameInDatabase, subtitle.formattedText.text));
		}
	}
	
	private bool ShouldLog(Subtitle subtitle){
		return subtitle.speakerInfo.IsPlayer && subtitle.formattedText != null && subtitle.formattedText.text != null && subtitle.formattedText.text.Length > 0;
	}

	IEnumerator LogDecision(string user, string conversation, string decision){
		//TODO make the url configurable
//		string url = "http://localhost:3000/decisions";
		string url = "http://limitless-chamber-6577.herokuapp.com/decisions";
		WWWForm form = new WWWForm();
		form.AddField("decision[user]", user);
		form.AddField("decision[conversation]", conversation);
		form.AddField("decision[decision]", decision);
		yield return new WWW(url, form);  
	} 

	void Update () {}
}
