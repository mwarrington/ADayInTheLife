using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using System.Linq;

/// <summary>
/// Character - the data model that holds information about characters in the game
/// To create Character instances use the CharacterFactory script
/// Information from here gets used to generate the Contacts GUI in the Contacts script
/// </summary>
public class Character {
    public int Id { get; set; }
    public Texture2D Portrait { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    private Dictionary<string, CharacterInfoItem> Rumors = new Dictionary<string, CharacterInfoItem>();
    private Dictionary<string, CharacterInfoItem> Facts  = new Dictionary<string, CharacterInfoItem>();
    private Dictionary<string, CharacterInfoItem> Lies  = new Dictionary<string, CharacterInfoItem>();
    private string isKnown;

    public Character(string name){
        this.Name = name;
        this.isKnown = "Variable['" + name + "']";
    }

    public CharacterInfoItem[] GetAllRumors(){
        return Rumors.Values.ToArray();
    }

    public CharacterInfoItem[] GetKnownRumors(){
        return GetAllRumors().Where(rumor => rumor.IsKnown()).ToArray();
    }

    public CharacterInfoItem[] GetFacts(){
        return Facts.Values.ToArray();
    }

    public CharacterInfoItem[] GetLies(){
        return Lies.Values.ToArray();
    }

    public void confirmRumor(string id){
        CharacterInfoItem rumor;
        Rumors.TryGetValue(id, out rumor);
        Facts.Add(id, rumor);
        Rumors.Remove(id);
    }

    public void debunkRumor(string id){
        CharacterInfoItem rumor;
        Rumors.TryGetValue(id, out rumor);
        Lies.Add(id, rumor);
        Rumors.Remove(id);
    }

    public void AddRumors(List<CharacterInfoItem> rumors){
        foreach(CharacterInfoItem rumor in rumors) {
            AddRumor(rumor);
        }
    }

    public void AddRumor(CharacterInfoItem rumor){
        Rumors.Add(rumor.Id, rumor);
    }

    public bool IsKnown(){
        return Lua.IsTrue(isKnown);
    }
}
