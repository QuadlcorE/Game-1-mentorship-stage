using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;  // since its one character that s talking explaining his/her story. Not an array.

    [TextArea(2,8)] // the dimensions of the dialogue box or text area, height and width.
    public string[] sentences; // the array of the whole dialogue conversation or story.

}
