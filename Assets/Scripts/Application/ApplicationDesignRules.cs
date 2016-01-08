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
	static public float mobileScreenRatio =1.5f;
	static public float cameraSize=5f;
	static public float backgroundCameraSize=5f;
	static public bool isMobileScreen;
	static public float viewHeight;
	static public float reductionRatio;
	static public float leftMargin=0.5f;
	static public float rightMargin=0.5f;
	static public float upMargin;
	static public float downMargin;
	static public float blockWidth;
	static public float largeBlockHeight;
	static public float mediumBlockHeight;
	static public float smallBlockHeight;
	static public float gapBetweenBlocks=0.05f;
	static public Color blackColor=new Color(0f,0f,0f);
	static public Color blueColor=new Color(75f/255f,163f/255f,174f/255f);
	static public Color redColor = new Color (218f / 255f, 70f / 255f, 70f / 255f);
	static public Color whiteTextColor = new Color(218f/255f,218f/255f,218f/255f);
	static public Color whiteSpriteColor = new Color(1f,1f,1f);
	static public Color greySpriteColor = new Color(100/255f,100f/255f,100f/255f);
	static public Color greyTextColor = new Color(171/255f,171f/255f,171f/255f);
	static public Color[] cardsColor = {new Color(75f/255f,163f/255f,174f/255f),new Color (196f/255f,196f/255f,196f/255f),new Color(171/255f,171f/255f,171f/255f)};

	static private Vector2 transparentBackgroundSize=new Vector2(1920f,1080f);
	static public Vector2 transparentBackgroundWorldSize;
	static private Vector3 transparentBackgroundOriginalScale=new Vector3(1f,1f,1f);
	static public Vector3 transparentBackgroundScale;
	static private Vector2 roundButtonSize=new Vector2(122f,122f);
	static public Vector2 roundButtonWorldSize;
	static private Vector3 roundButtonOriginalScale=new Vector3(0.85f,0.85f,0.85f);
	static public Vector3 roundButtonScale;
	static private Vector2 button31Size=new Vector2(199f,81f);
	static public Vector2 button31WorldSize;
	static private Vector3 button31OriginalScale=new Vector3(0.6f,0.6f,0.6f);
	static public Vector3 button31Scale;
	static private Vector2 button62Size=new Vector2(376f,122f);
	static public Vector2 button62WorldSize;
	static private Vector3 button62OriginalScale=new Vector3(0.6f,0.6f,0.6f);
	static public Vector3 button62Scale;
	static private Vector2 tabSize=new Vector2(355f,119f);
	static public Vector2 tabWorldSize;
	static private Vector3 tabOriginalScale=new Vector3(0.6f,0.6f,0.6f);
	static public Vector3 tabScale;
	static private Vector2 button61Size = new Vector2 (376f, 81f);
	static public Vector2 button61WorldSize;
	static private Vector3 button61OriginalScale=new Vector3(0.6f,0.6f,0.6f);
	static public Vector3 button61Scale;
	static private Vector2 button51Size = new Vector2 (317f, 81f);
	static public Vector2 button51WorldSize;
	static private Vector3 button51OriginalScale=new Vector3(0.6f,0.6f,0.6f);
	static public Vector3 button51Scale;
	static private Vector2 listElementSize = new Vector2 (415f, 89f);
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
	static private Vector2 competitionSize = new Vector2(325f,325f);
	static public Vector2 competitionWorldSize;
	static private Vector3 competitionOriginalScale=new Vector3(0.5f,0.5f,0.5f);
	static public Vector3 competitionScale;
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
	static private Vector3 paginationButtonMobileScale = new Vector3 (0.85f, 0.85f, 0.85f);
	static public Vector3 paginationButtonScale;
	static private Vector2 cardFocusedSize = new Vector3 (970f, 1060f);
	static public Vector2 cardFocusedWorldSize;
	static public Vector3 cardFocusedScale;
	static private Vector2 nextLevelPopUpSize = new Vector2 (720f, 1004f);
	static public Vector2 nextLevelPopUpWorldSize;
	static public Vector3 nextLevelPopUpScale = new Vector3(1.014f, 1.014f,1.014f);
	static private Vector2 cardTypeFilterSize = new Vector2(386f,386f);
	static public Vector2 cardTypeFilterWorldSize;
	static private Vector3 cardTypeFilterOriginalScale=new Vector3(0.17f,0.17f,0.17f);
	static public Vector3 cardTypeFilterScale;
	static private Vector2 skillTypeFilterSize = new Vector2(101f,101f);
	static public Vector2 skillTypeFilterWorldSize;
	static private Vector3 skillTypeFilterOriginalScale=new Vector3(0.7f,0.7f,0.7f);
	static public Vector3 skillTypeFilterScale;
	static private Vector2 inputTextSize = new Vector2(406f,80f);
	static public Vector2 inputTextWorldSize;
	static private Vector3 inputTextOriginalScale=new Vector3(0.6f,0.6f,0.6f);
	static public Vector3 inputTextScale;
	static private Vector2 largeInputTextSize = new Vector2(731f,81f);
	static public Vector2 largeInputTextWorldSize;
	static private Vector3 largeInputTextOriginalScale=new Vector3(0.6f,0.6f,0.6f);
	static public Vector3 largeInputTextScale;
	static private Vector3 valueFilterOriginalScale = new Vector3 (1f, 1f, 1f);
	static public Vector3 valueFilterScale;
	static private Vector2 cursorSize=new Vector2(78f,78f);
	static public Vector2 cursorWorldSize;
	static private Vector3 cursorOriginalScale=new Vector3(0.3f,0.3f,0.3f);
	static public Vector3 cursorScale;
	static private Vector2 packSize=new Vector2(688f,242f);
	static public Vector2 packWorldSize;
	static public Vector3 packScale;
	static public Vector2 packPictureSize = new Vector3 (375f, 200f);
	static public Vector2 packPictureWorldSize;
	static public Vector3 packPictureOriginalScale = new Vector3 (1.2f, 1.2f, 1.2f);
	static public Vector3 packPictureScale;
	static private Vector2 skillSize=new Vector2(1599f,355f);
	static public Vector2 skillWorldSize;
	static private Vector3 skillOriginalScale=new Vector3(0.53f,0.53f,0.53f);
	static public Vector3 skillScale;

	static private Vector3 mainTitleOriginalScale=new Vector3(1f,1f,1f);
	static public Vector3 mainTitleScale;
	static private Vector3 subMainTitleOriginalScale = new Vector3(1f,1f,1f);
	static public Vector3 subMainTitleScale;

	static public Vector3 menuPosition = new Vector3(0f,40f,0f);
	static public Vector3 tutorialPosition = new Vector3(0f,100f,0f);
	static public Vector3 backgroundPosition = new Vector3(0f,20f,0f);
	static public Vector3 focusedCardPosition;
	static public Vector3 nextLevelPopUpPosition;
	static public Vector3 randomCardsPosition;

	static public Vector3 mainCameraPosition = new Vector3 (menuPosition.x, menuPosition.y, -10f);
	static public Vector3 sceneCameraStandardPosition = new Vector3 (0f,0f, -10f);
	static public Vector3 sceneCameraFocusedCardPosition;
	static public Vector3 sceneCameraRandomCardsPosition;
	static public Vector3 scrollCameraStartPosition;
	static public Vector3 tutorialCameraPositiion = new Vector3 (tutorialPosition.x, tutorialPosition.y, -10f);
	static public Vector3 backgroundCameraPosition = new Vector3 (backgroundPosition.x, backgroundPosition.y, -10f);

	static private Vector2 topBarSize = new Vector2(1454f,181f);
	static private Vector3 topBarScale = new Vector3(0.75f,0.75f,0.75f);
	static public Vector2 topBarWorldSize;

	static private Vector2 bottomBarSize = new Vector3(1599f,199f);
	static private Vector3 bottomBarScale = new Vector3(0.68f,0.68f,0.68f);
	static public Vector2 bottomBarWorldSize;

	static public float gapBetweenCardsLine = 0.25f;
	static public float gapBetweenMarketCardsLine = 0.55f;
	static public float gapBetweenPacksLine = 0.25f;
	static public float gapBetweenSkillsLine = 0.25f;
	
	static public void computeDesignRules()
	{
		widthScreen=Screen.width;
		heightScreen=Screen.height;
		screenRatio = (float)widthScreen/(float)heightScreen;

		if(screenRatio<1f)
		{
			cameraSize=5f*((float)heightScreen/(float)widthScreen);
		}
		else
		{
			cameraSize=5f;
		}

		worldHeight = 2f*cameraSize;
		worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;

		topBarWorldSize = toWorldSize (topBarSize, topBarScale);
		bottomBarWorldSize = toWorldSize (bottomBarSize, bottomBarScale);

		if(screenRatio<=mobileScreenRatio)
		{
			reductionRatio=1f;
			isMobileScreen=true;
			leftMargin = (worldWidth-10.1f)/2f;
			rightMargin = leftMargin;
			upMargin=topBarWorldSize.y-0.3f;
			downMargin=bottomBarWorldSize.y-0.1f;
			blockWidth=worldWidth-leftMargin-rightMargin;

			largeBlockHeight = 10f - 2.1f;
			mediumBlockHeight = (10f-2.1f-gapBetweenBlocks)*(2.8f/5f);
			smallBlockHeight = (10f-2.1f-gapBetweenBlocks)*(2.2f/5f);
			scrollCameraStartPosition=new Vector3(0f,(upMargin + downMargin)/2f,-10f);

		}
		else
		{
			isMobileScreen=false;
			leftMargin = worldWidth / 80f;
			rightMargin = leftMargin;
			upMargin=1.9f;
			downMargin=0.2f;
			blockWidth=(worldWidth-leftMargin-rightMargin-gapBetweenBlocks)/2f;
			if(screenRatio>=optimalScreenRatio)
			{
				reductionRatio=1f;
			}
			else 
			{
				reductionRatio=1f-(optimalScreenRatio-screenRatio)/optimalScreenRatio;
			}

			largeBlockHeight = 10f - upMargin - downMargin;
			mediumBlockHeight = (10f-upMargin-downMargin-gapBetweenBlocks)*(2.8f/5f);
			smallBlockHeight = (10f-upMargin-downMargin-gapBetweenBlocks)*(2.2f/5f);
			scrollCameraStartPosition=new Vector3(0f,0f,-10f);
		}

		cardFocusedWorldSize.y = worldHeight - upMargin - downMargin;
		float focusedCardScale = cardFocusedWorldSize.y / (cardFocusedSize.y / pixelPerUnit);
		cardFocusedScale = new Vector3 (focusedCardScale, focusedCardScale, focusedCardScale);
		if(worldWidth-leftMargin-rightMargin<focusedCardScale*(cardFocusedSize.x/pixelPerUnit))
		{
			cardFocusedWorldSize.x = worldWidth - leftMargin - rightMargin;
			focusedCardScale = cardFocusedWorldSize.x / (cardFocusedSize.x / pixelPerUnit);
			cardFocusedScale = new Vector3 (focusedCardScale, focusedCardScale, focusedCardScale);
		}

		focusedCardPosition = new Vector3 (0f, -200f - (upMargin - downMargin)/2f, 0f);
		randomCardsPosition = new Vector3 (0f, -300f - (upMargin - downMargin) / 2f, 0f);
		sceneCameraFocusedCardPosition = new Vector3 (0f, -200f, -10f);
		sceneCameraRandomCardsPosition = new Vector3 (0f, -300f, -10f);

		transparentBackgroundWorldSize.y = worldHeight;
		float backgroundTransparentScale = transparentBackgroundWorldSize.y / (transparentBackgroundSize.y / pixelPerUnit);
		transparentBackgroundScale = new Vector3 (backgroundTransparentScale, backgroundTransparentScale, backgroundTransparentScale);
		transparentBackgroundWorldSize.x = backgroundTransparentScale * (transparentBackgroundSize.x / pixelPerUnit);

		viewHeight = worldHeight - upMargin - downMargin;

		roundButtonScale = toNewScale (roundButtonOriginalScale, roundButtonOriginalScale);
		roundButtonWorldSize = toWorldSize (roundButtonSize, roundButtonScale);

		button31Scale = toNewScale (button31OriginalScale, button31OriginalScale);
		button31WorldSize = toWorldSize (button31Size, button31Scale);

		button62Scale = toNewScale (button62OriginalScale, button62OriginalScale);
		button62WorldSize = toWorldSize (button62Size, button62Scale);

		tabScale = toNewScale (tabOriginalScale, tabOriginalScale);
		tabWorldSize = toWorldSize (tabSize, tabScale);

		button61Scale = toNewScale (button61OriginalScale, button61OriginalScale);
		button61WorldSize = toWorldSize (button61Size, button61Scale);

		button51Scale = toNewScale (button51OriginalScale,button51OriginalScale);
		button51WorldSize = toWorldSize (button51Size, button51Scale);

		listElementScale = toNewScale (listElementOriginalScale,listElementOriginalScale);
		listElementWorldSize = toWorldSize (listElementSize, listElementScale);

		cardHaloScale = toNewScale (cardHaloOriginalScale,cardHaloOriginalScale);
		cardHaloWorldSize = toWorldSize (cardHaloSize, cardHaloScale);

		cardScale = toNewScale (cardOriginalScale,cardOriginalScale);
		cardWorldSize = toWorldSize (cardSize, cardScale);

		competitionScale = toNewScale (competitionOriginalScale,competitionOriginalScale);
		competitionWorldSize = toWorldSize (competitionSize, competitionScale);

		thumbScale = toNewScale (thumbOriginalScale,thumbOriginalScale);
		thumbWorldSize = toWorldSize (thumbSize, thumbScale);

		cardTypeThumbScale = toNewScale (cardTypeThumbOriginalScale,cardTypeThumbOriginalScale);
		cardTypeThumbWorldSize = toWorldSize (cardTypeThumbSize, cardTypeThumbScale);

		skillTypeThumbScale = toNewScale (skillTypeThumbOriginalScale,skillTypeThumbOriginalScale);
		skillTypeThumbWorldSize = toWorldSize (skillTypeThumbSize, skillTypeThumbScale);

		profilePictureScale = toNewScale (profilePictureOriginalScale,profilePictureOriginalScale);
		profilePictureWorldSize = toWorldSize (profilePictureSize, profilePictureScale);

		paginationButtonScale = toNewScale (paginationButtonOriginalScale,paginationButtonMobileScale);
		paginationButtonWorldSize = toWorldSize (paginationButtonSize, paginationButtonScale);

		cardTypeFilterScale = toNewScale (cardTypeFilterOriginalScale,cardTypeFilterOriginalScale);
		cardTypeFilterWorldSize = toWorldSize (cardTypeFilterSize, cardTypeFilterScale);

		skillTypeFilterScale = toNewScale (skillTypeFilterOriginalScale,skillTypeFilterOriginalScale);
		skillTypeFilterWorldSize = toWorldSize (skillTypeFilterSize, skillTypeFilterScale);

		inputTextScale = toNewScale (inputTextOriginalScale,inputTextOriginalScale);
		inputTextWorldSize = toWorldSize (inputTextSize, inputTextScale);

		largeInputTextScale = toNewScale (largeInputTextOriginalScale,largeInputTextOriginalScale);
		largeInputTextWorldSize = toWorldSize (largeInputTextSize, largeInputTextScale);

		subMainTitleScale = toNewScale (subMainTitleOriginalScale,subMainTitleOriginalScale);
		mainTitleScale = toNewScale (mainTitleOriginalScale, mainTitleOriginalScale);

		packPictureScale = toNewScale (packPictureOriginalScale,packPictureOriginalScale);
		packPictureWorldSize = toWorldSize (packPictureSize,packPictureScale);

		valueFilterScale = toNewScale (valueFilterOriginalScale,valueFilterOriginalScale);

		cursorScale = toNewScale (cursorOriginalScale,cursorOriginalScale);
		cursorWorldSize = toWorldSize (cursorSize, cursorScale);

		skillScale = toNewScale (skillOriginalScale,skillOriginalScale);
		skillWorldSize = toWorldSize (skillSize, skillScale);

		packWorldSize.x = blockWidth - 2f * 0.3f;
		packWorldSize.y = 2.5f;
		packScale.x = packWorldSize.x/(packSize.x / ApplicationDesignRules.pixelPerUnit);
		packScale.y = packWorldSize.y / (packSize.y / ApplicationDesignRules.pixelPerUnit);
	}
	static private Vector2 toWorldSize(Vector2 originalSize, Vector3 scale)
	{
		Vector2 toWorldSize = new Vector2 ();
		toWorldSize.x=scale.x* (originalSize.x / pixelPerUnit);
		toWorldSize.y=scale.y* (originalSize.y / pixelPerUnit);
		return toWorldSize;
	}
	static private Vector3 toNewScale(Vector3 originalScale, Vector3 mobileScale)
	{
		Vector3 newScale = new Vector3 ();
		if(isMobileScreen)
		{
			newScale=mobileScale;
		}
		else
		{
			newScale=new Vector3(originalScale.x*reductionRatio,originalScale.y*reductionRatio,originalScale.z*reductionRatio);
		}
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
	static public string priceToString(int price)
	{
		int divisionRest;
		int unitNumber;
		string priceToString;
		if(price>1000000)
		{
			divisionRest=price%1000000;
			unitNumber = (price-divisionRest)/1000000;
			priceToString=unitNumber.ToString();
			if(divisionRest>0 && unitNumber<100)
			{
				priceToString=priceToString+".";
				for(int i=0;i<3-unitNumber.ToString().Length;i++)
				{
					if(divisionRest.ToString().Substring(i,1)!="0")
					{
						priceToString=priceToString+divisionRest.ToString().Substring(i,1);
					}
					else
					{
						break;
					}
				}
			}
			priceToString=priceToString+" M";
		}
		else if(price>1000)
		{
			divisionRest=price%1000;
			unitNumber = (price-divisionRest)/1000;
			priceToString=unitNumber.ToString();
			if(divisionRest>0 && unitNumber<100)
			{
				priceToString=priceToString+".";
				for(int i=0;i<3-unitNumber.ToString().Length;i++)
				{
					if(divisionRest.ToString().Substring(i,1)!="0")
					{
						priceToString=priceToString+divisionRest.ToString().Substring(i,1);
					}
					else
					{
						break;
					}
				}
			}
			priceToString=priceToString+" k";
		}
		else
		{
			priceToString=price.ToString();
		}
		return priceToString;
	}
}