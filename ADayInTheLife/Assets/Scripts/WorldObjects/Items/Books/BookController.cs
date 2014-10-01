using UnityEngine;
using System.Collections;

public class BookController : ItemController
{
    public string BookContent;
    public GameObject PagePrefab;

    private GameObject _currentPage;
    private TextMesh _currentTextMesh;
    private MeshRenderer _currentRenderer;
    private int _pageCount = 1;

    protected override void Start()
    {
        base.Start();

        ItemCamera = GameObject.FindGameObjectWithTag("BookCamera").GetComponent<Camera>();
        _currentPage = this.GetComponentInChildren<Page>().gameObject;
        _currentPage.GetComponent<BookPage>().PageNumber = _pageCount;
        _currentPage.name = _currentPage.name + _pageCount;
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

                if(_currentRenderer.bounds.extents.x > 6.7f)
                {
                    _currentTextMesh.text = _currentTextMesh.text.Remove(_currentTextMesh.text.Length - (currentWord.Length + 1));
                    _currentTextMesh.text = _currentTextMesh.text + "\n" + currentWord;

                    if (_currentRenderer.bounds.extents.y > 8.6f)
                    {
                        _currentTextMesh.text = _currentTextMesh.text.Remove(_currentTextMesh.text.Length - (currentWord.Length + 2));

                        _pageCount++;
                        _currentPage = (GameObject)Instantiate(PagePrefab, _currentPage.transform.position, Quaternion.identity);
                        _currentPage.transform.parent = this.transform;
                        _currentPage.GetComponent<BookPage>().PageNumber = _pageCount;
                        _currentPage.name = _currentPage.name.Remove(_currentPage.name.Length - 7);
                        _currentPage.name = _currentPage.name + _pageCount;
                        Pages.Add(_currentPage.name, _currentPage.GetComponent<Page>());
                        _currentTextMesh = _currentPage.GetComponent<TextMesh>();
                        _currentRenderer = _currentPage.GetComponent<MeshRenderer>();

                        _currentTextMesh.text = _currentTextMesh.text + currentWord;
                    }
                }
                currentWord = "";
            }
        }

        foreach (Page p in Pages.Values)
        {
            if (p.name != StartPage.name)
                p.gameObject.SetActive(false);
        }
    }
}
