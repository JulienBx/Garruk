using UnityEngine;
using TMPro;

public class MenuButtonController : MonoBehaviour 
{
	
	private bool isHovered;
	private bool isActive;

	void OnMouseOver()
	{
		if(!isHovered && !isActive)
		{
			this.isHovered=true;
			this.gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color=new Color(75f/255f,163f/255f,174f/255f);
		}
	}
	void OnMouseExit()
	{
		if(isHovered && !isActive)
		{
			this.isHovered=false;
			this.gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color=new Color(228f/255f,228f/255f,228f/255f);
		}
	}
	void OnMouseDown()
	{
		MenuController.instance.changePage (System.Convert.ToInt32(gameObject.name.Substring (6)));
	}
	public void setActive(bool value)
	{
		this.isActive = value;
		this.gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color=new Color(75f/255f,163f/255f,174f/255f);
	}
}

