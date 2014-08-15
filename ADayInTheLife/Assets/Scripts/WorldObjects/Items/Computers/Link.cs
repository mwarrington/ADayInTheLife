using UnityEngine;
using System.Collections;

public class Link : MonoBehaviour
{
	private ComputerController _myComputer;

	public string PageName;

	void Start()
	{
		_myComputer = this.transform.parent.parent.GetComponent<ComputerController>();
	}

	void Update()
	{
		if(this.transform.parent.GetComponent<Page>().IsActive)
		{
			this.GetComponent<BoxCollider>().enabled = true;
			LinkControll();
		}
		else
			this.GetComponent<BoxCollider>().enabled = false;
	}

	private void LinkControll()
	{
		Ray ray = _myComputer.ItemCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		if (Physics.Raycast(ray, out hit))
		{
			if(hit.collider.gameObject == this.gameObject)
			{
				if(Input.GetKeyDown(KeyCode.Mouse0))
					_myComputer.ChangePage(PageName);
			}
		}
	}
}
