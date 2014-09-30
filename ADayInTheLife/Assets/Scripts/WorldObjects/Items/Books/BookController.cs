using UnityEngine;
using System.Collections;

public class BookController : ItemController
{
    public string BookContent;

    private GameObject _currentPage;
    private TextMesh _currentTextMesh;
    private MeshRenderer _currentRenderer;

    protected override void Start()
    {
        base.Start();

        ItemCamera = GameObject.FindGameObjectWithTag("BookCamera").GetComponent<Camera>();
        _currentPage = this.GetComponentInChildren<Page>().gameObject;
        _currentTextMesh = _currentPage.GetComponent<TextMesh>();
        _currentRenderer = _currentPage.GetComponent<MeshRenderer>();

        StringFormatter();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void ChangePage(string nextPage)
    {
        base.ChangePage(nextPage);
    }

    protected override void TurnOff()
    {
        base.TurnOff();
    }

    private void StringFormatter()
    {
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

        string currentWord = "";

        for(int i = 0; i < BookContent.Length; i++)
        {
            if(BookContent[i] != ' ')
            {
                currentWord = currentWord + BookContent[i];
            }
            else
            {
                _currentTextMesh.text = _currentTextMesh.text + " " + currentWord;

                if(_currentRenderer.bounds.extents.x > 7.3f)
                {
                    _currentTextMesh.text.Remove(_currentTextMesh.text.Length - (currentWord.Length + 1));
                    _currentTextMesh.text = _currentTextMesh.text + "\n" + currentWord;
                }
            }
        }
    }
}
