using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour
{
    void Start()
    {
        Debug.Log(this.GetComponent<MeshRenderer>().bounds);
    }

    //Part 1:
    //Method that reads each character in a long string.
    //Everytime a ' ' appears it will add the string of non ' ' characters to the TextRenderer
    //That string will initially be added as " string" for spacing
    //Method then checks if the new word makes the mesh bounds too wide
    //If the word makes the mesh renderer too wide then it is replaced with the " string" with "\nstring"
    //Part 2:
    //Similarly if the line gets to be too long then the a new page will be instantiated
    //The process in part one will be repeated on the next instantiated page.
    //BAM!
}
