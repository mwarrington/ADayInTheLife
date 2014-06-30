using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Character info item - used to store information about a character.
/// Whether the information is known to the player is set by looking up the value of the 
/// variable that is keyed with this item's id. The id should be equal to the name of the conversation 
/// concatenated with the line number of the conversation that the rumor is learned in.
/// 
/// E.g. A conversation named "WeirdHorseDream" has a rumor that should be learned in line 7, the id should be "WeirdHorseDream7"
/// </summary>
public class CharacterInfoItem {
    public string Id { get; set; }
    public string Text { get; set; }
    private string isKnown;

    public CharacterInfoItem(string Id, string Text){
        this.Id = Id;
        this.Text = Text;
        this.isKnown = "Variable['" + Id + "']";
        Lua.Run(isKnown + " = false");
    }

    public bool IsKnown(){
        return Lua.IsTrue(isKnown);
    }
}
