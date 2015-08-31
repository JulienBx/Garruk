using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class BlockController : MonoBehaviour 
{

	public GameObject shadowObject;
	private GameObject shadow;
	private bool isShadowAlreadyExists;
	
	public void display(bool value)
	{
		if(shadow)
		{
			this.shadow.SetActive(value);
		}
		this.gameObject.SetActive (value);
	}
	public void destroyShadow()
	{
		if(shadow)
		{
			Destroy (this.shadow);
		}
	}
	public void resize(Rect block, bool shadow=true)
	{
		float pixelPerUnit = 108f;
		
		Vector3 cornerSize = new Vector3 (200f, 200f,0)/pixelPerUnit;
		Vector3 lineSize = new Vector3 (500f, 10f,0)/pixelPerUnit;
		Vector3 areaSize = new Vector3 (500f, 500f,0)/pixelPerUnit;
		
		Vector3 newCornerScale = new Vector3 (0.25f / (cornerSize.x), 0.25f / (cornerSize.y), 1f);
		Vector3 newCornerSize = new Vector3((newCornerScale.x) * (cornerSize.x),(newCornerScale.y) * (cornerSize.y),0);
		
		Vector3 newHorizontalLineScale = new Vector3 ((block.width-2f*newCornerSize.x) / lineSize.x, 1f, 1f);
		Vector3 newHorizontalLineSize = new Vector3(newHorizontalLineScale.x * (lineSize.x),newHorizontalLineScale.y * (lineSize.y),0f);
		
		Vector3 newVerticalLineScale = new Vector3 ((block.height-2f*newCornerSize.x) / lineSize.x, 1f, 1f);
		Vector3 newVerticalLineSize = new Vector3(newVerticalLineScale.x * (lineSize.x),newVerticalLineScale.y * (lineSize.y),0f);
		
		Vector3 newHorizontalAreaScale = new Vector3 (newHorizontalLineSize.x / areaSize.x, (newCornerSize.y - newHorizontalLineSize.y) / areaSize.y, 1f);
		Vector3 newHorizontalAreaSize = new Vector3(newHorizontalAreaScale.x * (areaSize.x),newHorizontalAreaScale.y * (areaSize.y),0f);
		
		Vector3 newVerticalAreaScale = new Vector3 ((newCornerSize.x - newVerticalLineSize.y) / areaSize.x, newVerticalLineSize.x / areaSize.y, 1f);
		Vector3 newVerticalAreaSize = new Vector3(newVerticalAreaScale.x * (areaSize.x),newVerticalAreaScale.y * (areaSize.y),0f);
		
		Vector3 newBigAreaScale = new Vector3 ((block.width- 2f * newVerticalLineSize.y -2*newVerticalAreaSize.x) /(areaSize.x), (block.height - 2f * newHorizontalLineSize.y - 2f * newHorizontalAreaSize.y) / (areaSize.y), 1f);
		Vector3 newBigAreaSize = new Vector3((newBigAreaScale.x) * (areaSize.x),(newBigAreaScale.y) * (areaSize.y),0f);
		
		this.gameObject.transform.position = new Vector3(block.x,block.y,0);
		
		Vector3 topRectanglePosition = new Vector3 (0f, newBigAreaSize.y / 2f + newHorizontalAreaSize.y / 2f, 0f);
		Vector3 bottomRectanglePosition = new Vector3 (0f, -newBigAreaSize.y / 2f - newHorizontalAreaSize.y / 2f, 0f);
		Vector3 leftRectanglePosition = new Vector3 (-newVerticalAreaSize.x/2f-newBigAreaSize.x/2f, 0f, 0f);
		Vector3 rightRectanglePosition = new Vector3 (newVerticalAreaSize.x/2f+newBigAreaSize.x/2f, 0f, 0f);
		Vector3 centerRectanglePosition = new Vector3 (0f, 0f, 0f);
		Vector3 bottomLeftCornerPosition = new Vector3 (-newCornerSize.x/2f-newHorizontalLineSize.x / 2f, -newBigAreaSize.y / 2f - newCornerSize.y / 2f, 0f);
		Vector3 bottomRightCornerPosition = new Vector3 (newCornerSize.x/2f+newHorizontalLineSize.x / 2f, -newBigAreaSize.y / 2f - newCornerSize.y / 2f, 0f);
		Vector3 topRightCornerPosition = new Vector3 (newCornerSize.x/2f+newHorizontalLineSize.x / 2f, newBigAreaSize.y / 2f + newCornerSize.y / 2f, 0f);
		Vector3 topLeftCornerPosition = new Vector3 (-newCornerSize.x/2f-newHorizontalLineSize.x / 2f, newBigAreaSize.y / 2f + newCornerSize.y / 2f, 0f);
		Vector3 bottomLinePosition = new Vector3 (0f, - newBigAreaSize.y / 2f - newHorizontalAreaSize.y - newHorizontalLineSize.y / 2f, 0f);
		Vector3 topLinePosition = new Vector3 (0f, newBigAreaSize.y / 2f + newHorizontalAreaSize.y  + newHorizontalLineSize.y / 2f, 0f);
		Vector3 leftLinePosition = new Vector3 (- newBigAreaSize.x / 2f - newVerticalAreaSize.x - newVerticalLineSize.y / 2f, 0f, 0f);
		Vector3 rightLinePosition = new Vector3 (newBigAreaSize.x / 2f + newVerticalAreaSize.x + newVerticalLineSize.y / 2f,0f, 0f);
		
		this.gameObject.transform.FindChild ("TopRectangle").localPosition = topRectanglePosition;
		this.gameObject.transform.FindChild ("TopRectangle").localScale = newHorizontalAreaScale;
		this.gameObject.transform.FindChild ("BottomRectangle").localPosition = bottomRectanglePosition;
		this.gameObject.transform.FindChild ("BottomRectangle").localScale = newHorizontalAreaScale;
		this.gameObject.transform.FindChild ("LeftRectangle").localPosition = leftRectanglePosition;
		this.gameObject.transform.FindChild ("LeftRectangle").localScale = newVerticalAreaScale;
		this.gameObject.transform.FindChild ("RightRectangle").localPosition = rightRectanglePosition;
		this.gameObject.transform.FindChild ("RightRectangle").localScale = newVerticalAreaScale;
		this.gameObject.transform.FindChild ("CenterRectangle").localPosition = centerRectanglePosition;
		this.gameObject.transform.FindChild ("CenterRectangle").localScale = newBigAreaScale;
		this.gameObject.transform.FindChild ("BottomLeftCorner").localPosition = bottomLeftCornerPosition;
		this.gameObject.transform.FindChild ("BottomLeftCorner").localScale = newCornerScale;
		this.gameObject.transform.FindChild ("BottomRightCorner").localPosition = bottomRightCornerPosition;
		this.gameObject.transform.FindChild ("BottomRightCorner").localScale = newCornerScale;
		this.gameObject.transform.FindChild ("TopRightCorner").localPosition = topRightCornerPosition;
		this.gameObject.transform.FindChild ("TopRightCorner").localScale = newCornerScale;
		this.gameObject.transform.FindChild ("TopLeftCorner").localPosition = topLeftCornerPosition;
		this.gameObject.transform.FindChild ("TopLeftCorner").localScale = newCornerScale;
		this.gameObject.transform.FindChild ("TopLine").localPosition = topLinePosition;
		this.gameObject.transform.FindChild ("TopLine").localScale = newHorizontalLineScale;
		this.gameObject.transform.FindChild ("BottomLine").localPosition = bottomLinePosition;
		this.gameObject.transform.FindChild ("BottomLine").localScale = newHorizontalLineScale;
		this.gameObject.transform.FindChild ("LeftLine").localPosition = leftLinePosition;
		this.gameObject.transform.FindChild ("LeftLine").localScale = newVerticalLineScale;
		this.gameObject.transform.FindChild ("RightLine").localPosition = rightLinePosition;
		this.gameObject.transform.FindChild ("RightLine").localScale = newVerticalLineScale;
		
		if(shadow)
		{
			if(!isShadowAlreadyExists)
			{
				this.shadow = Instantiate(this.shadowObject) as GameObject;
				this.isShadowAlreadyExists=true;
			}
			this.shadow.GetComponent<BlockController> ().resize (new Rect (block.x, block.y, block.width + 0.075f, block.height + 0.075f),false);
		}
		
	}
//	public void resizeASymetric(Rect block)
//	{
//
//		this.gameObject.transform.FindChild ("LeftRectangle").gameObject.SetActive (false);
//		this.gameObject.transform.FindChild ("RightRectangle").gameObject.SetActive (false);
//		this.gameObject.transform.FindChild ("BottomRightCorner").gameObject.SetActive (false);
//		this.gameObject.transform.FindChild ("TopLeftCorner").gameObject.SetActive (false);
//		this.gameObject.transform.FindChild ("LeftLine").gameObject.SetActive (false);
//		this.gameObject.transform.FindChild ("RightLine").gameObject.SetActive (false);
//
//		float pixelPerUnit = 108f;
//		Vector3 cornerSize = new Vector3 (200f, 200f,0)/pixelPerUnit;
//		Vector3 lineSize = new Vector3 (500f, 10f,0)/pixelPerUnit;
//		Vector3 areaSize = new Vector3 (500f, 500f,0)/pixelPerUnit;
//
//		Vector3 newCornerScale = new Vector3 (0.25f / (cornerSize.x), 0.25f / (cornerSize.y), 1f);
//		Vector3 newCornerSize = new Vector3((newCornerScale.x) * (cornerSize.x),(newCornerScale.y) * (cornerSize.y),0);
//
//		Vector3 newLineScale = new Vector3 ((block.width-newCornerSize.x) / lineSize.x, 1f, 1f);
//		Vector3 newLineSize = new Vector3(newLineScale.x * (lineSize.x),newLineScale.y * (lineSize.y),0f);
//
//		Vector3 newSmallAreaScale = new Vector3 (newLineSize.x / areaSize.x, (newCornerSize.y - newLineSize.y) / areaSize.y, 1f);
//		Vector3 newSmallAreaSize = new Vector3(newSmallAreaScale.x * (areaSize.x),newSmallAreaScale.y * (areaSize.y),0f);
//
//		Vector3 newBigAreaScale = new Vector3 (block.width /(areaSize.x), (block.height - 2f * newLineSize.y - 2f * newSmallAreaSize.y) / (areaSize.y), 1f);
//		Vector3 newBigAreaSize = new Vector3((newBigAreaScale.x) * (areaSize.x),(newBigAreaScale.y) * (areaSize.y),0f);
//
//		this.gameObject.transform.position = new Vector3(block.x,block.y,0);
//
//		Vector3 topRectanglePosition = new Vector3 (-newCornerSize.x / 2f, newBigAreaSize.y / 2f + newSmallAreaSize.y / 2f, 0f);
//		Vector3 bottomRectanglePosition = new Vector3 (newCornerSize.x / 2f, -newBigAreaSize.y / 2f - newSmallAreaSize.y / 2f, 0f);
//		Vector3 centerRectanglePosition = new Vector3 (0f, 0f, 0f);
//		Vector3 bottomLeftCornerPosition = new Vector3 (-newLineSize.x / 2f, -newBigAreaSize.y / 2f - newCornerSize.y / 2f, 0f);
//		Vector3 topRightCornerPosition = new Vector3 (newLineSize.x / 2f, newBigAreaSize.y / 2f + newCornerSize.y / 2f, 0f);
//		Vector3 bottomLinePosition = new Vector3 (newCornerSize.x / 2f, - newBigAreaSize.y / 2f - newSmallAreaSize.y - newLineSize.y / 2f, 0f);
//		Vector3 topLinePosition = new Vector3 (-newCornerSize.x / 2f, newBigAreaSize.y / 2f + newSmallAreaSize.y  + newLineSize.y / 2f, 0f);
//		
//		this.gameObject.transform.FindChild ("TopRectangle").localPosition = topRectanglePosition;
//		this.gameObject.transform.FindChild ("TopRectangle").localScale = newSmallAreaScale;
//		this.gameObject.transform.FindChild ("BottomRectangle").localPosition = bottomRectanglePosition;
//		this.gameObject.transform.FindChild ("BottomRectangle").localScale = newSmallAreaScale;
//		this.gameObject.transform.FindChild ("CenterRectangle").localPosition = centerRectanglePosition;
//		this.gameObject.transform.FindChild ("CenterRectangle").localScale = newBigAreaScale;
//		this.gameObject.transform.FindChild ("BottomLeftCorner").localPosition = bottomLeftCornerPosition;
//		this.gameObject.transform.FindChild ("BottomLeftCorner").localScale = newCornerScale;
//		this.gameObject.transform.FindChild ("TopRightCorner").localPosition = topRightCornerPosition;
//		this.gameObject.transform.FindChild ("TopRightCorner").localScale = newCornerScale;
//		this.gameObject.transform.FindChild ("TopLine").localPosition = topLinePosition;
//		this.gameObject.transform.FindChild ("TopLine").localScale = newLineScale;
//		this.gameObject.transform.FindChild ("BottomLine").localPosition = bottomLinePosition;
//		this.gameObject.transform.FindChild ("BottomLine").localScale = newLineScale;
//
//	}
//	public void resizeSymetric2(Rect block)
//	{
//		this.gameObject.transform.FindChild ("LeftLine").gameObject.SetActive (false);
//		this.gameObject.transform.FindChild ("RightLine").gameObject.SetActive (false);
//
//		float pixelPerUnit = 108f;
//
//		Vector3 cornerSize = new Vector3 (200f, 200f,0)/pixelPerUnit;
//		Vector3 lineSize = new Vector3 (500f, 10f,0)/pixelPerUnit;
//		Vector3 areaSize = new Vector3 (500f, 500f,0)/pixelPerUnit;
//		
//		Vector3 newCornerScale = new Vector3 (0.25f / (cornerSize.x), 0.25f / (cornerSize.y), 1f);
//		Vector3 newCornerSize = new Vector3((newCornerScale.x) * (cornerSize.x),(newCornerScale.y) * (cornerSize.y),0);
//		
//		Vector3 newHorizontalLineScale = new Vector3 ((block.width-2f*newCornerSize.x) / lineSize.x, 1f, 1f);
//		Vector3 newHorizontalLineSize = new Vector3(newHorizontalLineScale.x * (lineSize.x),newHorizontalLineScale.y * (lineSize.y),0f);
//
//		Vector3 newHorizontalAreaScale = new Vector3 (newHorizontalLineSize.x / areaSize.x, (newCornerSize.y - newHorizontalLineSize.y) / areaSize.y, 1f);
//		Vector3 newHorizontalAreaSize = new Vector3(newHorizontalAreaScale.x * (areaSize.x),newHorizontalAreaScale.y * (areaSize.y),0f);
//
//		Vector3 newVerticalAreaScale = new Vector3 ((newCornerSize.x) / areaSize.x, (block.height-2f*newCornerSize.y) / areaSize.y, 1f);
//		Vector3 newVerticalAreaSize = new Vector3(newVerticalAreaScale.x * (areaSize.x),newVerticalAreaScale.y * (areaSize.y),0f);
//
//		Vector3 newBigAreaScale = new Vector3 ((block.width-2*newVerticalAreaSize.x) /(areaSize.x), (block.height - 2f * newHorizontalLineSize.y - 2f * newHorizontalAreaSize.y) / (areaSize.y), 1f);
//		Vector3 newBigAreaSize = new Vector3((newBigAreaScale.x) * (areaSize.x),(newBigAreaScale.y) * (areaSize.y),0f);
//
//		Vector3 newShadowScale = new Vector3 ((block.width+0.05f) /(areaSize.x), (block.height + 0.05f) / (areaSize.y), 1f);
//		Vector3 newShadowSize = new Vector3((newShadowScale.x) * (areaSize.x),(newShadowScale.y) * (areaSize.y),0f);
//		
//		this.gameObject.transform.position = new Vector3(block.x,block.y,0);
//
//		Vector3 topRectanglePosition = new Vector3 (0f, newBigAreaSize.y / 2f + newHorizontalAreaSize.y / 2f, 0f);
//		Vector3 bottomRectanglePosition = new Vector3 (0f, -newBigAreaSize.y / 2f - newHorizontalAreaSize.y / 2f, 0f);
//		Vector3 leftRectanglePosition = new Vector3 (-newVerticalAreaSize.x/2f-newBigAreaSize.x/2f, 0f, 0f);
//		Vector3 rightRectanglePosition = new Vector3 (newVerticalAreaSize.x/2f+newBigAreaSize.x/2f, 0f, 0f);
//		Vector3 centerRectanglePosition = new Vector3 (0f, 0f, 0f);
//		Vector3 shadowPosition = new Vector3 (0f, 0f, 0f);
//		Vector3 bottomLeftCornerPosition = new Vector3 (-newCornerSize.x/2f-newHorizontalLineSize.x / 2f, -newBigAreaSize.y / 2f - newCornerSize.y / 2f, 0f);
//		Vector3 bottomRightCornerPosition = new Vector3 (newCornerSize.x/2f+newHorizontalLineSize.x / 2f, -newBigAreaSize.y / 2f - newCornerSize.y / 2f, 0f);
//		Vector3 topRightCornerPosition = new Vector3 (newCornerSize.x/2f+newHorizontalLineSize.x / 2f, newBigAreaSize.y / 2f + newCornerSize.y / 2f, 0f);
//		Vector3 topLeftCornerPosition = new Vector3 (-newCornerSize.x/2f-newHorizontalLineSize.x / 2f, newBigAreaSize.y / 2f + newCornerSize.y / 2f, 0f);
//		Vector3 bottomLinePosition = new Vector3 (0f, - newBigAreaSize.y / 2f - newHorizontalAreaSize.y - newHorizontalLineSize.y / 2f, 0f);
//		Vector3 topLinePosition = new Vector3 (0f, newBigAreaSize.y / 2f + newHorizontalAreaSize.y  + newHorizontalLineSize.y / 2f, 0f);
//		Vector3 leftLinePosition = new Vector3 (- newBigAreaSize.x / 2f - newVerticalAreaSize.x , 0f, 0f);
//		Vector3 rightLinePosition = new Vector3 (newBigAreaSize.x / 2f + newVerticalAreaSize.x ,0f, 0f);
//		
//		this.gameObject.transform.FindChild ("TopRectangle").localPosition = topRectanglePosition;
//		this.gameObject.transform.FindChild ("TopRectangle").localScale = newHorizontalAreaScale;
//		this.gameObject.transform.FindChild ("BottomRectangle").localPosition = bottomRectanglePosition;
//		this.gameObject.transform.FindChild ("BottomRectangle").localScale = newHorizontalAreaScale;
//		this.gameObject.transform.FindChild ("LeftRectangle").localPosition = leftRectanglePosition;
//		this.gameObject.transform.FindChild ("LeftRectangle").localScale = newVerticalAreaScale;
//		this.gameObject.transform.FindChild ("RightRectangle").localPosition = rightRectanglePosition;
//		this.gameObject.transform.FindChild ("RightRectangle").localScale = newVerticalAreaScale;
//		this.gameObject.transform.FindChild ("CenterRectangle").localPosition = centerRectanglePosition;
//		this.gameObject.transform.FindChild ("CenterRectangle").localScale = newBigAreaScale;
//		this.gameObject.transform.FindChild ("BottomLeftCorner").localPosition = bottomLeftCornerPosition;
//		this.gameObject.transform.FindChild ("BottomLeftCorner").localScale = newCornerScale;
//		this.gameObject.transform.FindChild ("BottomRightCorner").localPosition = bottomRightCornerPosition;
//		this.gameObject.transform.FindChild ("BottomRightCorner").localScale = newCornerScale;
//		this.gameObject.transform.FindChild ("TopRightCorner").localPosition = topRightCornerPosition;
//		this.gameObject.transform.FindChild ("TopRightCorner").localScale = newCornerScale;
//		this.gameObject.transform.FindChild ("TopLeftCorner").localPosition = topLeftCornerPosition;
//		this.gameObject.transform.FindChild ("TopLeftCorner").localScale = newCornerScale;
//		this.gameObject.transform.FindChild ("TopLine").localPosition = topLinePosition;
//		this.gameObject.transform.FindChild ("TopLine").localScale = newHorizontalLineScale;
//		this.gameObject.transform.FindChild ("BottomLine").localPosition = bottomLinePosition;
//		this.gameObject.transform.FindChild ("BottomLine").localScale = newHorizontalLineScale;
//		this.gameObject.transform.FindChild ("Shadow").localPosition = shadowPosition;
//		this.gameObject.transform.FindChild ("Shadow").localScale = newShadowScale;
//		
//	}
//	public void resize(Rect block)
//	{
//		float pixelPerUnit = 108f;
//		Vector3 areaSize = new Vector3 (500f, 500f,0)/pixelPerUnit;
//
//		Vector3 newBigAreaScale = new Vector3 (block.width /(areaSize.x), (block.height / (areaSize.y)), 1f);
//		Vector3 newBigAreaSize = new Vector3((newBigAreaScale.x) * (areaSize.x),(newBigAreaScale.y) * (areaSize.y),0f);
//		
//		this.gameObject.transform.position = new Vector3(block.x,block.y,0);
//		
//		Vector3 centerRectanglePosition = new Vector3 (0f, 0f, 0f);
//		
//		this.gameObject.transform.FindChild ("TopRectangle").gameObject.SetActive (false);
//		this.gameObject.transform.FindChild ("BottomRectangle").gameObject.SetActive (false);
//		this.gameObject.transform.FindChild ("CenterRectangle").localPosition = centerRectanglePosition;
//		this.gameObject.transform.FindChild ("CenterRectangle").localScale = newBigAreaScale;
//		this.gameObject.transform.FindChild ("BottomLeftCorner").gameObject.SetActive (false);
//		this.gameObject.transform.FindChild ("TopRightCorner").gameObject.SetActive (false);
//		this.gameObject.transform.FindChild ("TopLine").gameObject.SetActive (false);
//		this.gameObject.transform.FindChild ("BottomLine").gameObject.SetActive (false);
//		
//	}
}

