using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DivisionProgressionController : MonoBehaviour
{


	private bool toAnimate;
	private float currentRatio;
	private float nextWinRatio;

	void Update ()
	{
		if(toAnimate)
		{
			float increase = 0.1f * Time.deltaTime;
			this.currentRatio=this.currentRatio+increase;
			if(this.currentRatio>this.nextWinRatio)
			{
				this.currentRatio=this.nextWinRatio;
				this.toAnimate=false;
				NewLobbyController.instance.endGaugeAnimation();
			}
			this.setGaugeCamera(this.currentRatio);
		}
	}
	public void initialize()
	{
		this.gameObject.transform.FindChild("Gauge").FindChild("Bar").GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;
		this.gameObject.transform.FindChild("Gauge").FindChild("TitleBar").GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;
		this.gameObject.transform.FindChild("Gauge").FindChild("PromotionBar").GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;
		this.gameObject.transform.FindChild("Gauge").FindChild("RelegationBar").GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;
		this.gameObject.transform.FindChild("Gauge").FindChild("NbWins").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.blueColor;
		this.gameObject.transform.FindChild("Gauge").FindChild("RelegationNbWins").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.gameObject.transform.FindChild("Gauge").FindChild("PromotionNbWins").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.gameObject.transform.FindChild("Gauge").FindChild("TitleNbWins").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.gameObject.transform.FindChild("Gauge").FindChild("RelegationTitle").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.gameObject.transform.FindChild("Gauge").FindChild("PromotionTitle").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.gameObject.transform.FindChild("Gauge").FindChild("TitleTitle").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		//this.gameObject.transform.FindChild("Gauge").FindChild("RemainingGames").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.gameObject.transform.FindChild("Gauge").FindChild("Status").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.currentRatio = 0f;
	}
	public void setGaugeCamera(float ratio)
	{
		Vector3 gaugeScale = this.gameObject.transform.FindChild("Gauge").localScale;
		Vector3 gaugePosition = this.gameObject.transform.FindChild ("Gauge").FindChild("fullGauge").position;
		Vector2 fullGaugeSize = new Vector2 (593f, 598f);
		Vector2 fullGaugeWorldSize = (fullGaugeSize / ApplicationDesignRules.pixelPerUnit) * gaugeScale.x;
		Rect cameraRect = new Rect ();
		cameraRect.x = (ApplicationDesignRules.worldWidth / 2f + gaugePosition.x - fullGaugeWorldSize.x / 2f) / ApplicationDesignRules.worldWidth;
		cameraRect.y = (ApplicationDesignRules.worldHeight / 2f + gaugePosition.y - fullGaugeWorldSize.y / 2f) / ApplicationDesignRules.worldHeight;
		cameraRect.width = fullGaugeWorldSize.x / ApplicationDesignRules.worldWidth;
		cameraRect.height = ratio * (fullGaugeWorldSize.y / ApplicationDesignRules.worldHeight);

		this.gameObject.transform.FindChild ("GaugeCamera").GetComponent<Camera> ().rect = cameraRect;
		this.gameObject.transform.FindChild ("GaugeCamera").GetComponent<Camera> ().orthographicSize = (ratio * fullGaugeWorldSize.y)/2f;
		this.gameObject.transform.FindChild ("GaugeCamera").position = new Vector3 (gaugePosition.x, gaugePosition.y - fullGaugeWorldSize.y / 2f + ratio * fullGaugeWorldSize.y / 2f, 1f);
	}
	public void resize(Rect parentBlock)
	{
		float emptyGaugeSizeY = 660f;
		float emptyGaugeScale = this.gameObject.transform.FindChild ("Gauge").localScale.x;
		float emptyGaugeWorldSizeY = (emptyGaugeSizeY / ApplicationDesignRules.pixelPerUnit) * emptyGaugeScale;
		this.gameObject.transform.position = new Vector3 (parentBlock.x, parentBlock.y-(parentBlock.height-emptyGaugeWorldSizeY)/2f+0.05f, 0f);
		this.setGaugeCamera (this.currentRatio);
	}
	public void drawGauge(Division d, bool showGaugeStateBeforeLastWin)
	{
		int nbWins = d.NbWins;
		int remainingGames = d.NbGames - d.GamesPlayed;
		if(showGaugeStateBeforeLastWin)
		{
			nbWins=nbWins-1;
			remainingGames=remainingGames+1;
		}

		float gaugeMinXPosition = -0.815f;
		float gaugeMaxXPosition = 1.86f;

		this.currentRatio=nbWins / ((float)d.NbWinsForTitle + 2);
		this.nextWinRatio = (nbWins + 1) / ((float)d.NbWinsForTitle + 2);

//		gameObject.transform.FindChild ("Gauge").FindChild ("NbWins").gameObject.SetActive (true);
//
//		Vector2 nbWinsCirclePosition = this.getCirclePointCoordinates(this.currentRatio);
//		float lineScale = this.getGaugeLineScale(nbWinsCirclePosition.x*2);
		
//		Vector3 barPosition = gameObject.transform.FindChild ("Gauge").FindChild ("Bar").localPosition;
//		barPosition.x = 0f;
//		barPosition.y=nbWinsCirclePosition.y;
//		gameObject.transform.FindChild ("Gauge").FindChild ("Bar").localPosition = barPosition;
//		
//		Vector3 barScale = gameObject.transform.FindChild("Gauge").FindChild("Bar").localScale;
//		barScale.x=lineScale;
//		gameObject.transform.FindChild("Gauge").FindChild("Bar").localScale=barScale;

//		Vector3 nbWinsPosition = gameObject.transform.FindChild ("Gauge").FindChild ("NbWins").localPosition;
//		nbWinsPosition.x = 0f;
//		nbWinsPosition.y = nbWinsCirclePosition.y + 0.2f;
//		gameObject.transform.FindChild ("Gauge").FindChild ("NbWins").localPosition =nbWinsPosition;

//		gameObject.transform.FindChild ("Gauge").FindChild ("NbWins").GetComponent<TextMeshPro>().text=nbWins.ToString()+" Victoires";
//		gameObject.transform.FindChild ("Gauge").FindChild ("RemainingGames").GetComponent<TextMeshPro>().text=remainingGames.ToString()+" matchs restants";

		this.setGaugeCamera(this.currentRatio);

		if(nbWins<d.NbWinsForTitle)
		{
			gameObject.transform.FindChild ("Gauge").FindChild ("TitleBar").gameObject.SetActive(true);
			gameObject.transform.FindChild ("Gauge").FindChild("TitleNbWins").gameObject.SetActive(true);
			gameObject.transform.FindChild ("Gauge").FindChild("TitleTitle").gameObject.SetActive(true);

			float titleBarRatio=(float)d.NbWinsForTitle/((float)d.NbWinsForTitle+2);

			Vector2 titleCirclePosition = this.getCirclePointCoordinates(titleBarRatio);
			float titleLineScale = this.getGaugeLineScale(titleCirclePosition.x*2);

			Vector3 titleBarPosition = gameObject.transform.FindChild ("Gauge").FindChild ("TitleBar").localPosition;
			titleBarPosition.x = 0f;
			titleBarPosition.y=titleCirclePosition.y;
			gameObject.transform.FindChild ("Gauge").FindChild ("TitleBar").localPosition = titleBarPosition;

			Vector3 titleBarScale = gameObject.transform.FindChild("Gauge").FindChild("TitleBar").localScale;
			titleBarScale.x=titleLineScale;
			gameObject.transform.FindChild("Gauge").FindChild("TitleBar").localScale=titleBarScale;

			Vector3 titleNbWinsPosition = gameObject.transform.FindChild ("Gauge").FindChild ("TitleNbWins").localPosition;
			titleNbWinsPosition.x = titleCirclePosition.x+0.5f;
			titleNbWinsPosition.y=titleCirclePosition.y;
			gameObject.transform.FindChild ("Gauge").FindChild ("TitleNbWins").localPosition = titleNbWinsPosition;
			gameObject.transform.FindChild ("Gauge").FindChild ("TitleNbWins").GetComponent<TextMeshPro>().text=d.NbWinsForTitle.ToString()+" Victoires";

			Vector3 titleTitlePosition = gameObject.transform.FindChild ("Gauge").FindChild ("TitleTitle").localPosition;
			titleTitlePosition.x = -titleCirclePosition.x-0.5f;
			titleTitlePosition.y=titleCirclePosition.y;
			gameObject.transform.FindChild ("Gauge").FindChild ("TitleTitle").localPosition = titleTitlePosition;
			gameObject.transform.FindChild ("Gauge").FindChild ("TitleTitle").GetComponent<TextMeshPro>().text="Hégémonie";

		}
		else
		{
			gameObject.transform.FindChild ("Gauge").FindChild ("TitleBar").gameObject.SetActive(false);
			gameObject.transform.FindChild ("Gauge").FindChild("TitleNbWins").gameObject.SetActive(false);
			gameObject.transform.FindChild ("Gauge").FindChild("TitleTitle").gameObject.SetActive(false);
		}
		if (d.NbWinsForPromotion != -1 && nbWins<d.NbWinsForPromotion)
		{
			gameObject.transform.FindChild ("Gauge").FindChild ("PromotionBar").gameObject.SetActive(true);
			gameObject.transform.FindChild ("Gauge").FindChild("PromotionNbWins").gameObject.SetActive(true);
			gameObject.transform.FindChild ("Gauge").FindChild("PromotionTitle").gameObject.SetActive(true);
			
			float promotionBarRatio=(float)d.NbWinsForPromotion/((float)d.NbWinsForTitle+2);

			Vector2 promotionCirclePosition = this.getCirclePointCoordinates(promotionBarRatio);
			float promotionLineScale = this.getGaugeLineScale(promotionCirclePosition.x*2);
			
			Vector3 promotionBarPosition = gameObject.transform.FindChild ("Gauge").FindChild ("PromotionBar").localPosition;
			promotionBarPosition.x = 0f;
			promotionBarPosition.y=promotionCirclePosition.y;
			gameObject.transform.FindChild ("Gauge").FindChild ("PromotionBar").localPosition = promotionBarPosition;
			
			Vector3 promotionBarScale = gameObject.transform.FindChild("Gauge").FindChild("PromotionBar").localScale;
			promotionBarScale.x=promotionLineScale;
			gameObject.transform.FindChild("Gauge").FindChild("PromotionBar").localScale=promotionBarScale;
			
			Vector3 promotionNbWinsPosition = gameObject.transform.FindChild ("Gauge").FindChild ("PromotionNbWins").localPosition;
			promotionNbWinsPosition.x = promotionCirclePosition.x+0.5f;
			promotionNbWinsPosition.y=promotionCirclePosition.y;
			gameObject.transform.FindChild ("Gauge").FindChild ("PromotionNbWins").localPosition = promotionNbWinsPosition;
			gameObject.transform.FindChild ("Gauge").FindChild ("PromotionNbWins").GetComponent<TextMeshPro>().text=d.NbWinsForPromotion.ToString()+" Victoires";
			
			Vector3 promotionTitlePosition = gameObject.transform.FindChild ("Gauge").FindChild ("PromotionTitle").localPosition;
			promotionTitlePosition.x = -promotionCirclePosition.x-0.5f;
			promotionTitlePosition.y=promotionCirclePosition.y;
			gameObject.transform.FindChild ("Gauge").FindChild ("PromotionTitle").localPosition = promotionTitlePosition;
			gameObject.transform.FindChild ("Gauge").FindChild ("PromotionTitle").GetComponent<TextMeshPro>().text="Colonisation";

		}
		else
		{
			gameObject.transform.FindChild ("Gauge").FindChild ("PromotionBar").gameObject.SetActive(false);
			gameObject.transform.FindChild ("Gauge").FindChild("PromotionNbWins").gameObject.SetActive(false);
			gameObject.transform.FindChild ("Gauge").FindChild("PromotionTitle").gameObject.SetActive(false);
		}
		if (d.NbWinsForRelegation != -1 && nbWins<d.NbWinsForRelegation)
		{
			gameObject.transform.FindChild ("Gauge").FindChild ("RelegationBar").gameObject.SetActive(true);
			gameObject.transform.FindChild ("Gauge").FindChild("RelegationNbWins").gameObject.SetActive(true);
			gameObject.transform.FindChild ("Gauge").FindChild("RelegationTitle").gameObject.SetActive(true);

			float relegationBarRatio=(float)d.NbWinsForRelegation/((float)d.NbWinsForTitle+2);

			Vector2 relegationCirclePosition = this.getCirclePointCoordinates(relegationBarRatio);
			float relegationLineScale = this.getGaugeLineScale(relegationCirclePosition.x*2);
			
			Vector3 relegationBarPosition = gameObject.transform.FindChild ("Gauge").FindChild ("RelegationBar").localPosition;
			relegationBarPosition.x = 0f;
			relegationBarPosition.y=relegationCirclePosition.y;
			gameObject.transform.FindChild ("Gauge").FindChild ("RelegationBar").localPosition = relegationBarPosition;
			
			Vector3 relegationBarScale = gameObject.transform.FindChild("Gauge").FindChild("RelegationBar").localScale;
			relegationBarScale.x=relegationLineScale;
			gameObject.transform.FindChild("Gauge").FindChild("RelegationBar").localScale=relegationBarScale;
			
			Vector3 relegationNbWinsPosition = gameObject.transform.FindChild ("Gauge").FindChild ("RelegationNbWins").localPosition;
			relegationNbWinsPosition.x = relegationCirclePosition.x+0.5f;
			relegationNbWinsPosition.y=relegationCirclePosition.y;
			gameObject.transform.FindChild ("Gauge").FindChild ("RelegationNbWins").localPosition = relegationNbWinsPosition;
			gameObject.transform.FindChild ("Gauge").FindChild ("RelegationNbWins").GetComponent<TextMeshPro>().text=d.NbWinsForRelegation.ToString()+" Victoires";
			
			Vector3 relegationTitlePosition = gameObject.transform.FindChild ("Gauge").FindChild ("RelegationTitle").localPosition;
			relegationTitlePosition.x = -relegationCirclePosition.x-0.5f;
			relegationTitlePosition.y=relegationCirclePosition.y;
			gameObject.transform.FindChild ("Gauge").FindChild ("RelegationTitle").localPosition = relegationTitlePosition;
			gameObject.transform.FindChild ("Gauge").FindChild ("RelegationTitle").GetComponent<TextMeshPro>().text="Stabilisation";
		}
		else
		{
			gameObject.transform.FindChild ("Gauge").FindChild ("RelegationBar").gameObject.SetActive(false);
			gameObject.transform.FindChild ("Gauge").FindChild("RelegationNbWins").gameObject.SetActive(false);
			gameObject.transform.FindChild ("Gauge").FindChild("RelegationTitle").gameObject.SetActive(false);
		}

		string remainingGamesText = "";
		string nbWinsText = "";
		if(remainingGames>1)
		{
			remainingGamesText =remainingGames+" combats restants";
		}
		else
		{
			remainingGamesText =remainingGames+" combat restant";
		}
		if(nbWins>1)
		{
			nbWinsText =nbWins+" victoires";
		}
		else
		{
			nbWinsText =nbWins+" victoire";
		}

		if(nbWins>=d.NbWinsForTitle)
		{
			NewLobbyController.instance.updateSubMainBlockTitle("Hégémonie atteinte\n"+remainingGamesText+"\n"+nbWinsText);
		}
		else if(nbWins>=d.NbWinsForPromotion && d.NbWinsForPromotion!=-1)
		{
			NewLobbyController.instance.updateSubMainBlockTitle("Colonisation atteinte\n"+remainingGamesText+"\n"+nbWinsText);
		}
		else if(nbWins>=d.NbWinsForRelegation || d.NbWinsForRelegation==-1)
		{
			NewLobbyController.instance.updateSubMainBlockTitle("Stabilisation atteinte\n"+remainingGamesText+"\n"+nbWinsText);
		}
		else
		{
			NewLobbyController.instance.updateSubMainBlockTitle("Stabilisation en cours\n"+remainingGamesText+"\n"+nbWinsText);
		}
		this.setGaugeCamera (currentRatio);
	}
	public void animateGauge()
	{
		this.toAnimate = true;
		this.gameObject.transform.FindChild ("Gauge").FindChild ("NbWins").gameObject.SetActive (false);
	}
	public Vector2 getCirclePointCoordinates(float ratio)
	{
		Vector3 emptyGaugeScale = this.gameObject.transform.localScale;
		Vector3 emptygaugePosition = this.gameObject.transform.FindChild ("Gauge").position;
		Vector2 emptyGaugeSize = new Vector2 (610f, 610f);
		Vector2 emptyGaugeWorldSize = (emptyGaugeSize / ApplicationDesignRules.pixelPerUnit) * emptyGaugeScale.x;

		float distanceToCenter = Mathf.Abs (emptyGaugeWorldSize.y / 2 - ratio * emptyGaugeWorldSize.y);
		float horizontalDistance = Mathf.Sqrt(Mathf.Pow(emptyGaugeWorldSize.x/2f,2f)-Mathf.Pow(distanceToCenter,2f));

		Vector2 circlePointCoordinates = new Vector2(horizontalDistance,ratio*emptyGaugeWorldSize.y-emptyGaugeWorldSize.y/2f);
		return circlePointCoordinates;
	}
	public float getGaugeLineScale(float distance)
	{
		float lineSize = 603f;
		float scale = this.gameObject.transform.localScale.x;
		float lineWorldSize = (lineSize / ApplicationDesignRules.pixelPerUnit) * scale;

		return distance / lineWorldSize;
	}
}


