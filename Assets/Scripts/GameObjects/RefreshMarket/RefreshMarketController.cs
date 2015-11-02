//using UnityEngine;
//using TMPro;
//
//public class RefreshMarketController : MonoBehaviour 
//{
//
//	private bool isHovered;
//	private bool isHoveredColor;
//
//	public void changeColor()
//	{
//		if(!isHovered)
//		{
//			if(isHoveredColor)
//			{
//				this.setNonHoverColor();
//				this.isHoveredColor=false;
//			}
//			else
//			{
//				this.setHoverColor();
//				this.isHoveredColor=true;
//			}
//		}
//	}
//
//	void setHoverColor()
//	{
//		gameObject.transform.FindChild("Button").GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
//		gameObject.transform.FindChild ("Button").FindChild("Title").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
//		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
//	}
//	void setNonHoverColor()
//	{
//		gameObject.transform.FindChild("Button").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
//		gameObject.transform.FindChild ("Button").FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
//		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
//	}
//
//	void OnMouseOver()
//	{
//		if(!isHovered)
//		{
//			this.isHovered=true;
//			this.setHoverColor();
//		}
//	}
//	void OnMouseExit()
//	{
//		if(isHovered)
//		{
//			this.isHovered=false;
//			this.setNonHoverColor();
//		}
//	}
//	void OnMouseDown()
//	{
//		NewMarketController.instance.displayNewCards ();
//	}
//}
//
