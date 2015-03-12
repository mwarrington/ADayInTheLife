using UnityEngine;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.UnityGUI;

public class DialogueVisualUI : UnityDialogueUI
{
    public bool ScrambleResponses;
    protected bool responsesScrabled;

	// This is a reference to the NPC subtitle fade effect. If the subtitle panel is already
	// visible, we want to manually disable the fade effect so the panel stays visible.
	public FadeEffect npcBorderFadeEffect;

	// We use this bool to remember whether the subtitle panel is already visible:
	private bool isSubtitleVisible = false;

	// Make sure npcBorderFadeEffect is assigned:
	public override void Awake () {
		base.Awake();
		if (npcBorderFadeEffect == null) Debug.LogError(string.Format("{0}: NPC Border Fade Effect is unassigned!", DialogueDebug.Prefix));
	}

	// When the UI has just been opened, the subtitle panel is not visible:
	public override void Open()	{
		base.Open();
		isSubtitleVisible = false;
	}

	// If the subtitle panel is already visible, disable the fade effect:
	public override void ShowSubtitle(Subtitle subtitle) {
		if (npcBorderFadeEffect != null) npcBorderFadeEffect.enabled = !isSubtitleVisible;
		isSubtitleVisible = true;

        responsesScrabled = false;

		base.ShowSubtitle(subtitle);
	}

    public override void ShowResponses(Subtitle subtitle, Response[] responses, float timeout)
    {
        //Response Scrambler
        //NOTE: This currently doesn't work for the the multi dialog ui (Then again, the multi dialog ui doesn't work yet anyway...)
        if (!responsesScrabled && ScrambleResponses)
        {
            List<int> responseIndex = new List<int>();
            Response[] responseRef = new Response[responses.Length];

            for (int i = 0; i < responses.Length; i++)
            {
                responseIndex.Add(i);
                responseRef[i] = responses[i];
            }

            for (int i = 0; i < responseRef.Length; i++)
            {
                int randIndex;

                if (responseIndex.Count > 1)
                    randIndex = Random.Range(0, responseIndex.Count);
                else
                    randIndex = 0;

                responses[i] = responseRef[responseIndex[randIndex]];
                responseIndex.RemoveAt(randIndex);
            }

            responsesScrabled = true;
        }

        base.ShowResponses(subtitle, responses, timeout);
    }
}
