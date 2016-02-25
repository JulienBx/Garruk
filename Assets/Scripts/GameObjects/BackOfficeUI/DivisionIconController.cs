using UnityEngine;
using TMPro;

public class DivisionIconController : MonoBehaviour
{	
	private int division;

	public void setDivision(int division)
	{
		this.division=division;
		this.gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().text=division.ToString();
		this.applyColors();
	}
	private void applyColors()
	{
		if(this.division<4)
		{
			this.gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.redColor;
			this.gameObject.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
		}
		else if(this.division<7)
		{
			this.gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
			this.gameObject.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
		}
		else
		{
			this.gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
			this.gameObject.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
		}	
	}
	public void setHoveredState()
	{
		this.gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
		this.gameObject.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
	}
	public void setInitialState()
	{
		this.applyColors();
	}
}

