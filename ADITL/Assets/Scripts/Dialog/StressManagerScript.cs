using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

//attach this script to player object
public class StressManagerScript : MonoBehaviour {

	bool playedOnceA,
	playedOnceB,
	playedOnceC,
	playedOnceD,
	playedOnceE,
	playedOnceF;

	//use an int to indicate stressLevel
	private int stressLevel;
	//stressLevel setter
	//stressChange: the amount of change (+/-) applied to current stressLevel
	//return new stressLevel
	//sound volume range: 0.0 - 1.0. call VolumeChange and pass the stressChanged(+/-)
	public int setStressLevel(int stressChange){
		stressLevel += stressChange;

		SoundManager.VolumeChange (stressChange/100.0f);

		return stressLevel;
	}
	//stressLevel getter
	//return current stressLevel
	public int getStessLevel(){return stressLevel;}

	// Use this for initialization
	void Start () {

		//Add an observer to listen to the player's decision
		//Require variable "stress" in dialogue manager. 
		//Require Lua script for each player decision to change stress level
		DialogueManager.AddLuaObserver("Variable['stress']", LuaWatchFrequency.EveryDialogueEntry, DecisionMade);

		//initialize stress level to 0 to represent neutrual
		stressLevel = 0;

		playedOnceA = false;
		playedOnceB = false;
		playedOnceC = false;
		playedOnceD = false;
		playedOnceE = false;
		playedOnceF = false;
	
	}

	void DecisionMade(LuaWatchItem luaWatchItem, Lua.Result newValue){
		int stressChange = Lua.Run ("return Variable['stress']").AsInt;//get the amount of stress changed from Lua script 
		Debug.Log ("stress change: " + stressChange);//print stress change to Unity console
		setStressLevel (stressChange);
		Debug.Log ("Current stress level: " + this.getStessLevel ());//print current stress level to Unity console
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyUp(KeyCode.A))
		{
			if (!playedOnceA)
			{
				playedOnceA = true;
				SoundManager.Play(PrefabLoaderScript.instance.radiator, 0.5f);
			}
			else
			{
				playedOnceA = false;
				SoundManager.Stop(PrefabLoaderScript.instance.radiator);

			}							
		}

		if (Input.GetKeyUp(KeyCode.B))
		{
			if (!playedOnceB)
			{
				playedOnceB = true;
				SoundManager.Play(PrefabLoaderScript.instance.metalChairScrape, 1.0f);
			}
			else
			{
				playedOnceB = false;
				SoundManager.Stop(PrefabLoaderScript.instance.metalChairScrape);

			}							
		}

		if (Input.GetKeyUp(KeyCode.C))
		{
			if (!playedOnceC)
			{
				playedOnceC = true;
				SoundManager.Play(PrefabLoaderScript.instance.food, 1.5f);
			}
			else
			{
				playedOnceC = false;
				SoundManager.Stop(PrefabLoaderScript.instance.food);

			}							
		}

		if (Input.GetKeyUp(KeyCode.D))
		{
			if (!playedOnceD)
			{
				playedOnceD = true;
				SoundManager.Play(PrefabLoaderScript.instance.fork, 0.5f);
			}
			else
			{
				playedOnceD = false;
				SoundManager.Stop(PrefabLoaderScript.instance.fork);

			}							
		}

		if (Input.GetKeyUp(KeyCode.E))
		{
			if (!playedOnceE)
			{
				playedOnceE = true;
				SoundManager.Play(PrefabLoaderScript.instance.cymbol, 0.5f);
			}
			else
			{
				playedOnceE = false;
				SoundManager.Stop(PrefabLoaderScript.instance.cymbol);

			}							
		}

		if (Input.GetKeyUp(KeyCode.F))
		{
			if (!playedOnceF)
			{
				playedOnceF = true;
				SoundManager.Play(PrefabLoaderScript.instance.gore, 1.0f);
			}
			else
			{
				playedOnceF = false;
				SoundManager.Stop(PrefabLoaderScript.instance.gore);

			}							
		}
	
	}
}
