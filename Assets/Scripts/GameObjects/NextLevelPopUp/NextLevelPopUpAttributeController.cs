using UnityEngine;
using TMPro;

public class NextLevelPopUpAttributeController : MonoBehaviour 
{

	private int index;
	private int newPower;
	private int newLevel;
	private bool isHovering;

	public void initialize(int index, int newPower, int newLevel)
	{
		this.index = index;
		this.newPower = newPower;
		this.newLevel = newLevel;
	}

	void OnMouseOver()
	{
		if(!this.isHovering)
		{
			this.isHovering=true;
			gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
			gameObject.transform.FindChild("Text2").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
			gameObject.transform.FindChild("rightArrow").GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
			if(this.index>2)
			{
				gameObject.transform.FindChild("Name").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
				gameObject.transform.FindChild("Name2").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
			}
		}
	}
	void OnMouseExit()
	{
		if(this.isHovering)
		{
			this.isHovering=false;
			gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("Text2").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("rightArrow").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
			if(this.index>2)
			{
				gameObject.transform.FindChild("Name").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
				gameObject.transform.FindChild("Name2").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
			}
		}
	}
	void OnMouseDown()
	{
		gameObject.transform.parent.GetComponent<NextLevelPopUpController> ().clickOnAttribute (this.index, this.newPower, this.newLevel);
	}
}

