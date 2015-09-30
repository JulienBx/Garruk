using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DivisionProgressionController : MonoBehaviour
{
	
	public Color[] gaugeColors;

	private float nextWinGaugeRatio;
	private float activeGaugeRatio;
	private bool toAnimate;
	private float scaleSpeed;
	
	void Update ()
	{
		if(toAnimate)
		{
			float increase = scaleSpeed * Time.deltaTime;
			this.activeGaugeRatio=this.activeGaugeRatio+increase;
			if(this.activeGaugeRatio>this.nextWinGaugeRatio)
			{
				this.activeGaugeRatio=this.nextWinGaugeRatio;
				this.toAnimate=false;
				NewLobbyController.instance.endGaugeAnimation();
			}
			this.drawActiveGauge(this.activeGaugeRatio);
		}
	}
	public void resize(Rect parentBlock)
	{
		float pixelPerUnit = 108f;
		float gaugeWidth = 402f;
		float gaugeWorldWidth = gaugeWidth / pixelPerUnit;
		float gaugeXScale = (parentBlock.width - 2f) / gaugeWorldWidth;

		Vector3 gaugeScale = gameObject.transform.FindChild ("Gauge").transform.localScale;
		gaugeScale.x = gaugeXScale;
		gameObject.transform.FindChild ("Gauge").transform.localScale = gaugeScale;

		gameObject.transform.position = new Vector3 (parentBlock.x, parentBlock.y, 0f);

		gameObject.transform.FindChild ("NbWins").transform.position = gameObject.transform.FindChild ("Gauge").FindChild ("StartGauge").position;

		Vector3 relegationNbWinsPosition = gameObject.transform.FindChild ("Gauge").FindChild ("RelegationBar").transform.position;
		relegationNbWinsPosition.x = relegationNbWinsPosition.x + gaugeXScale * 0.08f;
		gameObject.transform.FindChild ("RelegationNbWins").transform.position = relegationNbWinsPosition;

		Vector3 promotionNbWinsPosition = gameObject.transform.FindChild ("Gauge").FindChild ("PromotionBar").transform.position;
		promotionNbWinsPosition.x = promotionNbWinsPosition.x + gaugeXScale * 0.08f;
		gameObject.transform.FindChild ("PromotionNbWins").transform.position = promotionNbWinsPosition;

		Vector3 titleNbWinsPosition = gameObject.transform.FindChild ("Gauge").FindChild ("TitleBar").transform.position;
		titleNbWinsPosition.x = titleNbWinsPosition.x + gaugeXScale * 0.08f;
		gameObject.transform.FindChild ("TitleNbWins").transform.position = titleNbWinsPosition;

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

		this.activeGaugeRatio = nbWins / ((float)d.NbWinsForTitle + 2);
		this.nextWinGaugeRatio = (nbWins + 1) / ((float)d.NbWinsForTitle + 2);

		gameObject.transform.FindChild ("NbWins").GetComponent<TextMeshPro>().text=nbWins.ToString()+" V";
		gameObject.transform.FindChild ("RemainingGames").GetComponent<TextMeshPro>().text=remainingGames.ToString()+" matchs restants";

		this.drawActiveGauge (activeGaugeRatio);

		float relegationBarRatio = 0f;
		float promotionBarRatio = 0f;

		if(nbWins<d.NbWinsForTitle)
		{
			gameObject.transform.FindChild ("Gauge").FindChild ("TitleBar").gameObject.SetActive(true);
			gameObject.transform.FindChild("TitleNbWins").gameObject.SetActive(true);

			float titleBarRatio=(float)d.NbWinsForTitle/((float)d.NbWinsForTitle+2);
			Vector3 titleBarPosition = gameObject.transform.FindChild ("Gauge").FindChild ("PromotionBar").localPosition;
			titleBarPosition.x = gaugeMinXPosition + titleBarRatio * (gaugeMaxXPosition - gaugeMinXPosition);
			gameObject.transform.FindChild ("Gauge").FindChild ("TitleBar").localPosition = titleBarPosition;
			gameObject.transform.FindChild("Gauge").FindChild("TitleBar").GetComponent<SpriteRenderer>().color=this.gaugeColors[3];

			Vector3 titleNbWinsPosition = gameObject.transform.FindChild ("Gauge").FindChild ("TitleBar").transform.position;
			titleNbWinsPosition.x = titleNbWinsPosition.x + gameObject.transform.FindChild ("Gauge").transform.localScale.x * 0.08f;
			gameObject.transform.FindChild ("TitleNbWins").transform.position = titleNbWinsPosition;
			gameObject.transform.FindChild ("TitleNbWins").GetComponent<TextMeshPro>().text=d.NbWinsForTitle.ToString()+" V";


		}
		else
		{
			gameObject.transform.FindChild ("Gauge").FindChild ("TitleBar").gameObject.SetActive(false);
			gameObject.transform.FindChild("TitleNbWins").gameObject.SetActive(false);
		}
		if (d.NbWinsForPromotion != -1 && nbWins<d.NbWinsForPromotion)
		{
			gameObject.transform.FindChild ("Gauge").FindChild ("PromotionBar").gameObject.SetActive(true);
			gameObject.transform.FindChild("PromotionNbWins").gameObject.SetActive(true);
			
			promotionBarRatio=(float)d.NbWinsForPromotion/((float)d.NbWinsForTitle+2);
			Vector3 promotionBarPosition = gameObject.transform.FindChild ("Gauge").FindChild ("PromotionBar").localPosition;
			promotionBarPosition.x = gaugeMinXPosition + promotionBarRatio * (gaugeMaxXPosition - gaugeMinXPosition);
			gameObject.transform.FindChild ("Gauge").FindChild ("PromotionBar").localPosition = promotionBarPosition;
			gameObject.transform.FindChild("Gauge").FindChild("PromotionBar").GetComponent<SpriteRenderer>().color=this.gaugeColors[2];
			
			Vector3 titleNbWinsPosition = gameObject.transform.FindChild ("Gauge").FindChild ("PromotionBar").transform.position;
			titleNbWinsPosition.x = titleNbWinsPosition.x + gameObject.transform.FindChild ("Gauge").transform.localScale.x * 0.08f;
			gameObject.transform.FindChild ("PromotionNbWins").transform.position = titleNbWinsPosition;
			gameObject.transform.FindChild ("PromotionNbWins").GetComponent<TextMeshPro>().text=d.NbWinsForPromotion.ToString()+" V";
		}
		else
		{
			gameObject.transform.FindChild ("Gauge").FindChild ("PromotionBar").gameObject.SetActive(false);
			gameObject.transform.FindChild("PromotionNbWins").gameObject.SetActive(false);
		}
		if (d.NbWinsForRelegation != -1 && nbWins<d.NbWinsForRelegation)
		{
			gameObject.transform.FindChild ("Gauge").FindChild ("RelegationBar").gameObject.SetActive(true);
			gameObject.transform.FindChild("RelegationNbWins").gameObject.SetActive(true);

			relegationBarRatio=(float)d.NbWinsForRelegation/((float)d.NbWinsForTitle+2);
			Vector3 relegationBarPosition = gameObject.transform.FindChild ("Gauge").FindChild ("RelegationBar").localPosition;
			relegationBarPosition.x = gaugeMinXPosition + relegationBarRatio * (gaugeMaxXPosition - gaugeMinXPosition);
			gameObject.transform.FindChild ("Gauge").FindChild ("RelegationBar").localPosition = relegationBarPosition;
			gameObject.transform.FindChild("Gauge").FindChild("RelegationBar").GetComponent<SpriteRenderer>().color=this.gaugeColors[1];

			Vector3 titleNbWinsPosition = gameObject.transform.FindChild ("Gauge").FindChild ("RelegationBar").transform.position;
			titleNbWinsPosition.x = titleNbWinsPosition.x + gameObject.transform.FindChild ("Gauge").transform.localScale.x * 0.08f;
			gameObject.transform.FindChild ("RelegationNbWins").transform.position = titleNbWinsPosition;
			gameObject.transform.FindChild ("RelegationNbWins").GetComponent<TextMeshPro>().text=d.NbWinsForRelegation.ToString()+" V";
		}
		else
		{
			gameObject.transform.FindChild ("Gauge").FindChild ("RelegationBar").gameObject.SetActive(false);
			gameObject.transform.FindChild("RelegationNbWins").gameObject.SetActive(false);
		}

		if(nbWins>=d.NbWinsForTitle)
		{
			gameObject.transform.FindChild ("Gauge").FindChild ("ActiveGauge").GetComponent<SpriteRenderer> ().color = this.gaugeColors [3];
			gameObject.transform.FindChild("Status").GetComponent<TextMeshPro>().text="Statut actuel : Champion de division";
		}
		else if(nbWins>=d.NbWinsForPromotion && d.NbWinsForPromotion!=-1)
		{
			gameObject.transform.FindChild ("Gauge").FindChild ("ActiveGauge").GetComponent<SpriteRenderer> ().color = this.gaugeColors [2];
			gameObject.transform.FindChild("Status").GetComponent<TextMeshPro>().text="Statut actuel : monte de division";
		}
		else if(nbWins>=d.NbWinsForRelegation || d.NbWinsForRelegation==-1)
		{
			gameObject.transform.FindChild ("Gauge").FindChild ("ActiveGauge").GetComponent<SpriteRenderer> ().color = this.gaugeColors [1];
			gameObject.transform.FindChild("Status").GetComponent<TextMeshPro>().text="Statut actuel : reste dans la division";
		}
		else
		{
			gameObject.transform.FindChild ("Gauge").FindChild ("ActiveGauge").GetComponent<SpriteRenderer> ().color = this.gaugeColors [0];
			gameObject.transform.FindChild("Status").GetComponent<TextMeshPro>().text="Statut actuel : descend de division";
		}
	}
	public void drawActiveGauge(float activeGaugeRatio)
	{
		float activeGaugeMinXPosition = -0.815f;
		float activeGaugeMinXScale = 0f;
		float activeGaugeMaxXPosition = 0.524f;
		float activeGaugeMaxXScale = 4.87f;

		Vector3 activeGaugePosition = gameObject.transform.FindChild ("Gauge").FindChild ("ActiveGauge").localPosition;
		activeGaugePosition.x = activeGaugeMinXPosition + activeGaugeRatio * (activeGaugeMaxXPosition - activeGaugeMinXPosition);
		gameObject.transform.FindChild ("Gauge").FindChild ("ActiveGauge").localPosition = activeGaugePosition;
		
		Vector3 activeGaugeScale = gameObject.transform.FindChild ("Gauge").FindChild ("ActiveGauge").localScale;
		activeGaugeScale.x = activeGaugeMinXScale + activeGaugeRatio * (activeGaugeMaxXScale - activeGaugeMinXScale);
		gameObject.transform.FindChild ("Gauge").FindChild ("ActiveGauge").localScale = activeGaugeScale;
	}
	public void animateGauge()
	{
		this.toAnimate = true;
		this.scaleSpeed = 0.1f;
	}
}


