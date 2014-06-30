using UnityEngine;
using System.Collections;

public class CharacterScript : MonoBehaviour {

	public AnimationManager spriteAnim;

	// Use this for initialization
	void Start () {
		SetAnimation("Idle");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetAnimation(string anim, float delay = 0.0f)
	{
		StartCoroutine( SetAnimationRoutine(anim, delay) );
	}
	public IEnumerator SetAnimationRoutine(string anim, float delay = 0.0f)
	{
		if (spriteAnim != null)
		{
			yield return new WaitForSeconds(delay);
			StopCoroutine("SetAnimationRoutine");
			spriteAnim.SetState(anim);
		}	
	}
}
