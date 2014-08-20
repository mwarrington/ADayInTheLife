using UnityEngine;
using System.Collections;

public class BookPage : Page
{
	public int PageNumber;
	public string BookName;

	private BookController _myBook;

	protected override void Start ()
	{
		base.Start();
		_myBook = myObject.GetComponent<BookController>();
	}

	protected override void Update ()
	{
		base.Update();

		if(IsActive)
			FlipPage();
	}

	protected override void ScrollControl ()
	{
		base.ScrollControl ();
	}

	private void FlipPage()
	{
		if((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && PageNumber > 1)
		{
			_myBook.ChangePage(BookName + (PageNumber - 1));
		}
		if((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && PageNumber < _myBook.Pages.Count)
		{
			_myBook.ChangePage(BookName + (PageNumber + 1));
		}
	}
}
