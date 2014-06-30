using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AnimationManager : MonoBehaviour
{
	public int colCount;
	public int rowCount;
	public int direction = 1;
	public string state;
	static int left = -1;
	static int right = 1;
	public int fps = 10;
	private Renderer reference;
	private float timeFrame = 0.0f;
	
	public List<string> frameKeys;
	public MasterList frameValues;
	public List<Vector2> frameSizeValues;
	public List<AnimType> frameAnimTypes;
	
	public Dictionary<string, List<int>> frames;
	public Dictionary<string, Vector2> frameSizes;
	public Dictionary<string, AnimationManager.AnimType> animationTypes;
	
	private bool tripwire = false; 
	private float startTime = 0;
	
	void Init(Renderer referencePar) {
		reference = referencePar;
		
		frames = new Dictionary<string, List<int>>();
		frameSizes = new Dictionary<string, Vector2>();
		animationTypes = new Dictionary<string, AnimType>();
		for(int i = 0; i < frameKeys.Count; i++) {
			frames.Add(frameKeys[i], frameValues.values[i].values);
			frameSizes.Add(frameKeys[i], frameSizeValues[i]);
			//Debug.Log(i);
			//Debug.Log(frameAnimTypes.Count);
			animationTypes.Add(frameKeys[i], (i < frameAnimTypes.Count) ? frameAnimTypes[i] : AnimType.Loop);
		}
		timeFrame = 0.0f;
	}
	
	void Start()
	{
		Init(this.renderer);
	}
	
	void Update()
	{
		if (frames != null && frames.ContainsKey(state) && frames[state].Count > 0)
		{
			if (timeFrame == 0 && animationTypes[state] == AnimationManager.AnimType.RandomStart)
			{
				timeFrame = UnityEngine.Random.Range(0, 5.0f);	
			}
			
			// Calculate index
			timeFrame += Time.deltaTime;
			int index = (int)(timeFrame * fps);
			
			if (animationTypes != null && animationTypes.ContainsKey(state))
				if (animationTypes[state] == AnimationManager.AnimType.Once || animationTypes[state] == AnimationManager.AnimType.RandomPlay)
			{
				if (tripwire)
				{
					return;
				}
				if (index >= frames[state].Count)
				{
					tripwire = true;
					if (animationTypes[state] == AnimationManager.AnimType.RandomPlay)
					{
						Invoke("CancelTripwire", UnityEngine.Random.Range(1.5f, 4.0f));
					}
				}
			}
			
			// Repeat when exhausting all cells
			if (direction == AnimationManager.left)
				index = (frames[state].Count-1) - index % frames[state].Count;
			else if (direction == AnimationManager.right)
				index = index % frames[state].Count;
			
			index = frames[state][index];
			
			// Size of every cell
			Vector2 size = Vector2.zero;
			if (frameSizes[state].magnitude > 0) {
				size = new Vector2(frameSizes[state].x * reference.material.mainTexture.width, frameSizes[state].y * reference.material.mainTexture.height);
			} else {
				size = new Vector2 (1.0f / colCount, 1.0f / rowCount);
			}
			size.x *= direction;
			
			// split into horizontal and vertical index
			int uIndex = (int)(index % colCount);
			int vIndex = (int)(index / colCount);
			
			// build offset
			// v coordinate is the bottom of the image in opengl so we need to invert.
			Vector2 offset = new Vector2 ((uIndex) * size.x, (1.0f - size.y) - (vIndex) * size.y);
			reference.material.SetTextureOffset ("_MainTex", offset);
			reference.material.SetTextureScale  ("_MainTex", size);
		}
	}
	
	void CancelTripwire()
	{
		tripwire = false;
		timeFrame = 0.0f;
	}
	
	public void AddAnimation()
	{
		if (frameKeys == null || frameValues.values == null) {
			frameKeys = new List<string>();
			frameSizeValues = new List<Vector2>();
		}
		if (!frameKeys.Contains("")) {
			frameKeys.Add("");
			frameSizeValues.Add(Vector2.zero);
			frameValues.values.Add(new ListContainer());
		}
	}
	
	public void SetState(string statePar) {
		CancelInvoke("CancelTripwire");
		tripwire = false;
		if (state != statePar) {
			state = statePar;
			timeFrame = 0.0f;
		}
	}
	
	public bool CheckForAnimation(string animation) {
		return frames.ContainsKey(animation);
	}
	
	public void SetDirection(int directionPar) {
		CancelInvoke("CancelTripwire");
		tripwire = false;
		direction = directionPar;
		timeFrame = 0.0f;
	}

	public enum AnimType { Loop, Once, RandomPlay, RandomStart };
}

[System.Serializable]
public class ListContainer
{
	public List<int> values = new List<int>();
}

[System.Serializable]
public class MasterList
{
	public List<ListContainer> values = new List<ListContainer>();
}
