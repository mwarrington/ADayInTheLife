using UnityEngine;
using System.Text;
using System.Collections;
using PixelCrushers.DialogueSystem;
using SimpleJSON;

public class JSONHandler : MonoBehaviour
{
    static JSONNode jSONOut = new JSONNode();
    public JSONNode JSONOut
    {
        get
        {
            return jSONOut;
        }
        set
        {
            jSONOut = value;
        }
    }
    static WWWForm formJSON = new WWWForm();
    public WWWForm FormJSON
    {
        get
        {
            return formJSON;
        }
        set
        {
            formJSON = value;
        }
    }

    static bool lvl1JSONInitialized = false;

    private GameManager _myGameManager;

    void Start()
    {
        _myGameManager = FindObjectOfType<GameManager>();

        //JSON set up
        StartCoroutine(JSONSetUp());
    }

    private IEnumerator JSONSetUp()
    {
        if (!lvl1JSONInitialized && Application.loadedLevelName == "DreamSpiral")
        {
            JSONOut = JSONNode.Parse("{\"Player\":\"Sarylyn\"},\"Gamestate\":{}");
            if (_myGameManager.IsSarylyn)
                JSONOut["Player"] = "Sarylyn";
            else
                JSONOut["Player"] = "Sanome";

            for (int i = 0; i < DialogueManager.MasterDatabase.conversations.Count; i++)
            {
                string conversationString = DialogueManager.MasterDatabase.conversations[i].Title.ToString();
                JSONOut["Level1"]["Conversations"][conversationString][0] = "";
            }
            lvl1JSONInitialized = true;
            string url = "http://localhost:3000/gamestates/create";
            FormJSON.AddField("gamestate", WWW.EscapeURL(jSONOut.ToString()));
            Hashtable headers = new Hashtable();
            headers.Add("Content-Type", "application/json");

            //Debug.Log (JSONOut.ToString());
            yield return new WWW(url, Encoding.Default.GetBytes(FormJSON.data.ToString()), headers);
        }
    }

    public IEnumerator LogJSON()
    {
        string url = "http://localhost:3000/gamestates/create";
        FormJSON.AddField("gamestate", jSONOut.ToString());
        Hashtable headers = new Hashtable();
        headers.Add("Content-Type", "application/json");
        yield return new WWW(url, Encoding.Default.GetBytes(FormJSON.data.ToString()), headers);
    }
}
