using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class TrainingPopUpController : MonoBehaviour 
{

	private float timer;
	private bool cardTypeUnlocked;
	private bool divisionModeUnlocked;
	private int trainingStatus;


	public void reset(bool hasWon)
	{
		string text = "";
		if(hasWon && ApplicationModel.player.TrainingStatus%10==-1)
		{
			text=WordingTrainingPopUp.getReference(8) + WordingCardTypes.getName(ApplicationModel.player.TrainingPreviousAllowedCardType) + WordingTrainingPopUp.getReference(9);
			this.divisionModeUnlocked=true;
			this.cardTypeUnlocked=false;
			this.trainingStatus=93;
		}
		else if(hasWon && ApplicationModel.player.TrainingStatus%10==0)
		{
			text=WordingTrainingPopUp.getReference(8) + WordingCardTypes.getName(ApplicationModel.player.TrainingPreviousAllowedCardType) + WordingTrainingPopUp.getReference(9);
			this.cardTypeUnlocked=true;
			this.divisionModeUnlocked=false;
			this.trainingStatus=ApplicationModel.player.TrainingStatus-7;
		}
		else if(hasWon && ApplicationModel.player.TrainingStatus==91)
		{
			text=WordingTrainingPopUp.getReference(0) + WordingCardTypes.getName(ApplicationModel.player.TrainingAllowedCardType) + WordingTrainingPopUp.getReference(1) + "2" + WordingTrainingPopUp.getReference(3) + WordingTrainingPopUp.getReference(5);
			this.cardTypeUnlocked=false;
			this.divisionModeUnlocked=false;
			this.trainingStatus=ApplicationModel.player.TrainingStatus;	
		}
		else if(hasWon && ApplicationModel.player.TrainingStatus==92)
		{
			text=WordingTrainingPopUp.getReference(0) + WordingCardTypes.getName(ApplicationModel.player.TrainingAllowedCardType) + WordingTrainingPopUp.getReference(1) + "1" + WordingTrainingPopUp.getReference(2) + WordingTrainingPopUp.getReference(5);
			this.cardTypeUnlocked=false;
			this.divisionModeUnlocked=false;
			this.trainingStatus=ApplicationModel.player.TrainingStatus;	
		}
		else if(hasWon && ApplicationModel.player.TrainingStatus%10==1)
		{
			text=WordingTrainingPopUp.getReference(0) + WordingCardTypes.getName(ApplicationModel.player.TrainingAllowedCardType) + WordingTrainingPopUp.getReference(1) + "2" + WordingTrainingPopUp.getReference(3) + WordingTrainingPopUp.getReference(4);
			this.cardTypeUnlocked=false;
			this.divisionModeUnlocked=false;
			this.trainingStatus=ApplicationModel.player.TrainingStatus;	
		}
		else if(hasWon && ApplicationModel.player.TrainingStatus%10==2)
		{
			text=WordingTrainingPopUp.getReference(0) + WordingCardTypes.getName(ApplicationModel.player.TrainingAllowedCardType) + WordingTrainingPopUp.getReference(1) + "1" + WordingTrainingPopUp.getReference(2) + WordingTrainingPopUp.getReference(4);
			this.cardTypeUnlocked=false;
			this.divisionModeUnlocked=false;
			this.trainingStatus=ApplicationModel.player.TrainingStatus;	
		}
		else if(!hasWon && ApplicationModel.player.TrainingStatus==90)
		{
			text=WordingTrainingPopUp.getReference(6) + WordingCardTypes.getName(ApplicationModel.player.TrainingAllowedCardType) + WordingTrainingPopUp.getReference(1) + "3" + WordingTrainingPopUp.getReference(3) + WordingTrainingPopUp.getReference(4);
			this.cardTypeUnlocked=false;
			this.divisionModeUnlocked=false;
			this.trainingStatus=ApplicationModel.player.TrainingStatus;	
		}
		else if(!hasWon && ApplicationModel.player.TrainingStatus==91)
		{
			text=WordingTrainingPopUp.getReference(6) + WordingCardTypes.getName(ApplicationModel.player.TrainingAllowedCardType) + WordingTrainingPopUp.getReference(1) + "2" + WordingTrainingPopUp.getReference(3) + WordingTrainingPopUp.getReference(4);
			this.cardTypeUnlocked=false;
			this.divisionModeUnlocked=false;
			this.trainingStatus=ApplicationModel.player.TrainingStatus;	
		}
		else if(!hasWon && ApplicationModel.player.TrainingStatus==92)
		{
			text=WordingTrainingPopUp.getReference(6) + WordingCardTypes.getName(ApplicationModel.player.TrainingAllowedCardType) + WordingTrainingPopUp.getReference(1) + "1" + WordingTrainingPopUp.getReference(2) + WordingTrainingPopUp.getReference(4);
			this.cardTypeUnlocked=false;
			this.divisionModeUnlocked=false;
			this.trainingStatus=ApplicationModel.player.TrainingStatus;	
		}
		else if(!hasWon && ApplicationModel.player.TrainingStatus%10==0)
		{
			text=WordingTrainingPopUp.getReference(6) +WordingTrainingPopUp.getReference(1) + "3" + WordingTrainingPopUp.getReference(3) + WordingTrainingPopUp.getReference(4);
			this.cardTypeUnlocked=false;
			this.divisionModeUnlocked=false;
			this.trainingStatus=ApplicationModel.player.TrainingStatus;	
		}
		else if(!hasWon && ApplicationModel.player.TrainingStatus%10==1)
		{
			text=WordingTrainingPopUp.getReference(6) +WordingTrainingPopUp.getReference(1) + "2" + WordingTrainingPopUp.getReference(3) + WordingTrainingPopUp.getReference(4);
			this.cardTypeUnlocked=false;
			this.divisionModeUnlocked=false;
			this.trainingStatus=ApplicationModel.player.TrainingStatus;	
		}
		else if(!hasWon && ApplicationModel.player.TrainingStatus%10==2)
		{
			text=WordingTrainingPopUp.getReference(6) +WordingTrainingPopUp.getReference(1) + "1" + WordingTrainingPopUp.getReference(2) + WordingTrainingPopUp.getReference(4);
			this.cardTypeUnlocked=false;
			this.divisionModeUnlocked=false;
			this.trainingStatus=ApplicationModel.player.TrainingStatus;	
		}
		gameObject.transform.FindChild("Training").gameObject.SetActive(true);
		gameObject.transform.FindChild("Division").gameObject.SetActive(false);
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = text;
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingTrainingPopUp.getReference(7);
		gameObject.transform.FindChild ("Button").GetComponent<TrainingPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild("Training").GetComponent<TrainingController>().draw(this.trainingStatus);
	}
	public void resize()
	{
	}
	public void exitPopUp()
	{
		SoundController.instance.playSound(8);
		if(this.cardTypeUnlocked)
		{
			gameObject.transform.FindChild("Training").GetComponent<TrainingController>().draw(ApplicationModel.player.TrainingStatus);
			gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingTrainingPopUp.getReference(10) + WordingCardTypes.getName(ApplicationModel.player.TrainingAllowedCardType) + WordingTrainingPopUp.getReference(11);
			this.cardTypeUnlocked=false;
		}
		else if(this.divisionModeUnlocked)
		{
			gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingTrainingPopUp.getReference(12) ;
			this.divisionModeUnlocked=false;
			gameObject.transform.FindChild("Training").gameObject.SetActive(false);
			gameObject.transform.FindChild("Division").gameObject.SetActive(true);
		}
		else
		{
			NewHomePageController.instance.hideTrainingPopUp ();
		}
	}
}

