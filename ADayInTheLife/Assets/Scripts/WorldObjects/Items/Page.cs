using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class Page : MonoBehaviour
{
	public SpriteRenderer MySprite;
	public float PageLength;
	public bool ImpartsInfo;
	public string[] KnowledgeStrings; //For Chatmapper Integration
	public bool IsActive
	{
		get
		{
			return isActive;
		}
		set
		{
			if(value)
			{
				MySprite.enabled = true;
				ImpartInformation();
			}
			else
				MySprite.enabled = false;

			isActive = value;
		}
	}
	protected bool isActive;
	protected GameObject myObject;
	protected float autoScrollTimer = 0,
				    lastScrollTime = 0;
	protected bool autoScrollTimerStarted = false;

	protected virtual void Start()
	{
		myObject = this.transform.parent.gameObject;
	}

	protected virtual void Update()
	{
		if(IsActive)
			ScrollControl();
	}

	protected virtual void ScrollControl()
	{
		//Debug.Log (this.transform.position.y);
		if((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && this.transform.position.y <= PageLength)
		{
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f);
			autoScrollTimerStarted = true;
		}
		if((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && this.transform.position.y >= 0)
		{
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.5f);
			autoScrollTimerStarted = true;
		}
		
		//This part handles the auto scroll
		if(autoScrollTimerStarted)
		{
			autoScrollTimer += Time.deltaTime;
			if(autoScrollTimer > 0.7f)
			{
				lastScrollTime += Time.deltaTime;
				if(lastScrollTime > 0.05f)
				{
					lastScrollTime = 0;
					if((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && this.transform.position.y <= PageLength)
						this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f);
					else if((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && this.transform.position.y >= 0)
						this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.5f);
				}
			}
		}
		
		if((Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S)))
		{
			autoScrollTimerStarted = false;
			autoScrollTimer = 0;
			lastScrollTime = 0;
		}
		if((Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W)))
		{
			autoScrollTimerStarted = false;
			autoScrollTimer = 0;
			lastScrollTime = 0;
		}
	}

	protected virtual void ImpartInformation()
	{
		if(ImpartsInfo)
		{
			for(int i = 0; i < KnowledgeStrings.Length; i++)
			{
				DialogueLua.SetVariable(KnowledgeStrings[i], true);
			}
		}
	}
}
