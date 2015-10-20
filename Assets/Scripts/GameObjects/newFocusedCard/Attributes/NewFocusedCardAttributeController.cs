using UnityEngine;
using TMPro;

public class NewFocusedCardAttributeController : MonoBehaviour 
{
	
	public int attributeIndex;
	private bool isHovered;

	void OnMouseOver()
	{
		if(!this.isHovered)
		{
			this.gameObject.transform.parent.GetComponent<NewFocusedCardController>().startHoveringAttribute(this.attributeIndex);
			this.isHovered=true;
		}

	}
	void OnMouseExit()
	{
		if(this.isHovered)
		{
			this.gameObject.transform.parent.GetComponent<NewFocusedCardController>().endHoveringAttribute(this.attributeIndex);
			this.isHovered=false;
		}
	}
	void OnMouseDown()
	{
		this.gameObject.transform.parent.GetComponent<NewFocusedCardController>().clickOnAttribute(this.attributeIndex);			
	}
	public void highlightAttribute(bool value)
	{
		if(value)
		{
			this.gameObject.transform.FindChild("Picto").GetComponent<SpriteRenderer>().color=new Color(218f/255f, 70f/255f, 70f/255f);
			this.gameObject.transform.FindChild("Text").GetComponent<TextMeshPro> ().color=new Color(218f/255f, 70f/255f, 70f/255f);
		}
		else
		{
			this.gameObject.transform.FindChild("Picto").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
			this.gameObject.transform.FindChild("Text").GetComponent<TextMeshPro> ().color=new Color(1f,1f,1f);
		}
	}
}

