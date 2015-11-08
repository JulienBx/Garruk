using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
/**
 ** Classe permettant de stocker les informations de charte graphique.
*/
public class ApplicationDesignRules : MonoBehaviour
{ 
	static public int widthScreen;
	static public int heightScreen;
	static public float worldWidth;
	static public float worldHeight;
	static public float pixelPerUnit=108f;
	static public float screenRatio;
	static public float optimalScreenRatio=1.77f;
	static public float reductionRatio;
	static public float leftMargin=0.5f;
	static public float rightMargin=0.5f;
	static public float upMargin=1.9f;
	static public float downMargin=0.2f;
	static public float gapBetweenBlocks=0.05f;
	static public Color blueColor=new Color(75f/255f,163f/255f,174f/255f);
	static public Color redColor = new Color (218f / 255f, 70f / 255f, 70f / 255f);
	static public Color whiteTextColor = new Color(218f/255f,218f/255f,218f/255f);
	static public Color whiteSpriteColor = new Color(1f,1f,1f);
	static public Color greySpriteColor = new Color(100/255f,100f/255f,100f/255f);
	static public Color greyTextColor = new Color(171/255f,171f/255f,171f/255f);
	static public Color[] cardsColor = {new Color(75f/255f,163f/255f,174f/255f),new Color (196f/255f,196f/255f,196f/255f),new Color(171/255f,171f/255f,171f/255f)};

	static private Vector2 button62Size=new Vector2(357f,120f);
	static public Vector2 button62WorldSize;
	static private Vector3 button62OriginalScale=new Vector3(0.6f,0.6f,0.6f);
	static public Vector3 button62Scale;
	static private Vector2 button61Size = new Vector2 (357f, 62f);
	static public Vector2 button61WorldSize;
	static private Vector3 button61OriginalScale=new Vector3(0.6f,0.6f,0.6f);
	static public Vector3 button61Scale;
	static private Vector2 button51Size = new Vector2 (298f, 62f);
	static public Vector2 button51WorldSize;
	static private Vector3 button51OriginalScale=new Vector3(0.6f,0.6f,0.6f);
	static public Vector3 button51Scale;
	static private Vector2 listElementSize = new Vector2 (415f, 90f);
	static public Vector2 listElementWorldSize;
	static private Vector3 listElementOriginalScale=new Vector3(0.6f,0.6f,0.6f);
	static public Vector3 listElementScale;
	static private Vector2 cardHaloSize = new Vector2(200f,279f);
	static public Vector2 cardHaloWorldSize;
	static private Vector3 cardHaloOriginalScale=new Vector3(1f,1f,1f);
	static public Vector3 cardHaloScale;
	static private Vector2 cardSize = new Vector2(720f,1004f);
	static public Vector2 cardWorldSize;
	static private Vector3 cardOriginalScale=new Vector3(0.2694445f,0.2694445f,0.2694445f);
	static public Vector3 cardScale;
	static private Vector2 lineSize = new Vector2 (1500f, 2f);
	static public Vector2 lineWorldSize;
	static private Vector3 lineOriginalScale=new Vector3(1f,1f,1f);
	static public Vector3 contentLineScale;
	static private Vector2 thumbSize = new Vector2(63f,63f);
	static public Vector2 thumbWorldSize;
	static private Vector3 thumbOriginalScale=new Vector3(1.2f,1.2f,1.2f);
	static public Vector3 thumbScale;
	static private Vector2 cardTypeThumbSize = new Vector2(386f,386f);
	static public Vector2 cardTypeThumbWorldSize;
	static private Vector3 cardTypeThumbOriginalScale=new Vector3(0.19f,0.19f,0.19f);
	static public Vector3 cardTypeThumbScale;
	static private Vector2 skillTypeThumbSize = new Vector2(101f,101f);
	static public Vector2 skillTypeThumbWorldSize;
	static private Vector3 skillTypeThumbOriginalScale=new Vector3(0.75f,0.75f,0.75f);
	static public Vector3 skillTypeThumbScale;
	static private Vector2 profilePictureSize = new Vector2(190f,190f);
	static public Vector2 profilePictureWorldSize;
	static private Vector3 profilePictureOriginalScale=new Vector3(1f,1f,1f);
	static public Vector3 profilePictureScale;
	static private Vector2 paginationButtonSize = new Vector2(125f,125f);
	static public Vector2 paginationButtonWorldSize;
	static private Vector3 paginationButtonOriginalScale=new Vector3(0.3f,0.3f,0.3f);
	static public Vector3 paginationButtonScale;
	static private Vector2 cardFocusedSize = new Vector3 (932f * 0.7287152f, 1402f * 0.7287152f);
	static public Vector2 cardFocusedWorldSize;
	static public Vector3 cardFocusedScale;
	static private Vector2 cardTypeFilterSize = new Vector2(386f,386f);
	static public Vector2 cardTypeFilterWorldSize;
	static private Vector3 cardTypeFilterOriginalScale=new Vector3(0.17f,0.17f,0.17f);
	static public Vector3 cardTypeFilterScale;
	static private Vector2 skillTypeFilterSize = new Vector2(101f,101f);
	static public Vector2 skillTypeFilterWorldSize;
	static private Vector3 skillTypeFilterOriginalScale=new Vector3(0.7f,0.7f,0.7f);
	static public Vector3 skillTypeFilterScale;
	static private Vector2 inputTextSize = new Vector2(386f,62f);
	static public Vector2 inputTextWorldSize;
	static private Vector3 inputTextOriginalScale=new Vector3(0.6f,0.6f,0.6f);
	static public Vector3 inputTextScale;
	static private Vector2 largeInputTextSize = new Vector2(711f,62f);
	static public Vector2 largeInputTextWorldSize;
	static private Vector3 largeInputTextOriginalScale=new Vector3(0.6f,0.6f,0.6f);
	static public Vector3 largeInputTextScale;
	static private Vector3 valueFilterOriginalScale = new Vector3 (1f, 1f, 1f);
	static public Vector3 valueFilterScale;
	static private Vector2 cursorSize=new Vector2(78f,78f);
	static public Vector2 cursorWorldSize;
	static private Vector3 cursorOriginalScale=new Vector3(0.3f,0.3f,0.3f);
	static public Vector3 cursorScale;

	static private Vector3 mainTitleOriginalScale=new Vector3(1f,1f,1f);
	static public Vector3 mainTitleScale;
	static private Vector3 subMainTitleOriginalScale = new Vector3(1f,1f,1f);
	static public Vector3 subMainTitleScale;

	static public void computeDesignRules()
	{
		widthScreen=Screen.width;
		heightScreen=Screen.height;
		screenRatio = (float)widthScreen/(float)heightScreen;
		if(screenRatio>=optimalScreenRatio)
		{
			reductionRatio=1f;
		}
		else
		{
			reductionRatio=1f-(optimalScreenRatio-screenRatio)/optimalScreenRatio;
		}
		worldHeight = 2f*Camera.main.GetComponent<Camera>().orthographicSize;
		worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;
		screenRatio = (float)widthScreen / (float)heightScreen;

		button62Scale = toNewScale (button62OriginalScale);
		button62WorldSize = toWorldSize (button62Size, button62Scale);

		button61Scale = toNewScale (button61OriginalScale);
		button61WorldSize = toWorldSize (button61Size, button61Scale);

		button51Scale = toNewScale (button51OriginalScale);
		button51WorldSize = toWorldSize (button51Size, button51Scale);

		listElementScale = toNewScale (listElementOriginalScale);
		listElementWorldSize = toWorldSize (listElementSize, listElementScale);

		cardHaloScale = toNewScale (cardHaloOriginalScale);
		cardHaloWorldSize = toWorldSize (cardHaloSize, cardHaloScale);

		cardScale = toNewScale (cardOriginalScale);
		cardWorldSize = toWorldSize (cardSize, cardScale);

		thumbScale = toNewScale (thumbOriginalScale);
		thumbWorldSize = toWorldSize (thumbSize, thumbScale);

		cardTypeThumbScale = toNewScale (cardTypeThumbOriginalScale);
		cardTypeThumbWorldSize = toWorldSize (cardTypeThumbSize, cardTypeThumbScale);

		skillTypeThumbScale = toNewScale (skillTypeThumbOriginalScale);
		skillTypeThumbWorldSize = toWorldSize (skillTypeThumbSize, skillTypeThumbScale);

		profilePictureScale = toNewScale (profilePictureOriginalScale);
		profilePictureWorldSize = toWorldSize (profilePictureSize, profilePictureScale);

		paginationButtonScale = toNewScale (paginationButtonOriginalScale);
		paginationButtonWorldSize = toWorldSize (paginationButtonSize, paginationButtonScale);

		cardTypeFilterScale = toNewScale (cardTypeFilterOriginalScale);
		cardTypeFilterWorldSize = toWorldSize (cardTypeFilterSize, cardTypeFilterScale);

		skillTypeFilterScale = toNewScale (skillTypeFilterOriginalScale);
		skillTypeFilterWorldSize = toWorldSize (skillTypeFilterSize, skillTypeFilterScale);

		inputTextScale = toNewScale (inputTextOriginalScale);
		inputTextWorldSize = toWorldSize (inputTextSize, inputTextScale);

		largeInputTextScale = toNewScale (largeInputTextOriginalScale);
		largeInputTextWorldSize = toWorldSize (largeInputTextSize, largeInputTextScale);

		subMainTitleScale = toNewScale (subMainTitleOriginalScale);
		mainTitleScale = toNewScale (mainTitleOriginalScale);

		valueFilterScale = toNewScale (valueFilterOriginalScale);

		cursorScale = toNewScale (cursorOriginalScale);
		cursorWorldSize = toWorldSize (cursorSize, cursorScale);

		cardFocusedWorldSize.y = worldHeight - upMargin - downMargin;
		float focusedCardScale = cardFocusedWorldSize.y / (cardFocusedSize.y / pixelPerUnit);
		cardFocusedScale = new Vector3 (focusedCardScale, focusedCardScale, focusedCardScale);
	}
	static private Vector2 toWorldSize(Vector2 originalSize, Vector3 scale)
	{
		Vector2 toWorldSize = new Vector2 ();
		toWorldSize.x=scale.x* (originalSize.x / pixelPerUnit);
		toWorldSize.y=scale.y* (originalSize.y / pixelPerUnit);
		return toWorldSize;
	}
	static private Vector3 toNewScale(Vector3 originalScale)
	{
		Vector3 newScale=new Vector3(originalScale.x*reductionRatio,originalScale.y*reductionRatio,originalScale.z*reductionRatio);
		return newScale;
	}
	static public float getLineScale(float worldSize)
	{
		float scale = (worldSize * pixelPerUnit) / lineSize.x;
		return scale;
	}
	static public float getScale(float originalSize, float worldSize)
	{
		float scale = (worldSize * pixelPerUnit) / originalSize ;
		return scale;
	}
	static public Vector2 getCardOriginalSize()
	{
		return cardSize;
	}
}