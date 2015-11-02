using UnityEngine;
using TMPro;

public class NewFocusedCardStoreController : NewFocusedCardController
{
	public void displayFocusFeatures(bool value)
	{
		this.gameObject.transform.FindChild ("FocusFeature0").gameObject.SetActive (value);
		this.gameObject.transform.FindChild ("FocusFeature1").gameObject.SetActive (value);
		this.gameObject.transform.FindChild ("FocusFeature2").gameObject.SetActive (value);
		this.gameObject.transform.FindChild ("FocusFeature3").gameObject.SetActive (value);
		this.gameObject.transform.FindChild ("FocusFeature4").gameObject.SetActive (value);
		this.gameObject.transform.FindChild ("FocusFeature5").gameObject.SetActive (value);
	}
	public override void updateFocusFeatures()
	{
		this.gameObject.transform.FindChild("FocusFeature0").FindChild("Title").GetComponent<TextMeshPro>().text="Bannir \n( +"+this.c.destructionPrice+" cristaux)";
		this.gameObject.transform.FindChild("FocusFeature0").GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
		if(this.c.ExperienceLevel!=10)
		{
			this.gameObject.transform.FindChild("FocusFeature1").FindChild("Title").GetComponent<TextMeshPro>().text="Passer l'unité au niveau suivant \n( -"+this.c.NextLevelPrice+ " cristaux)";
			this.gameObject.transform.FindChild("FocusFeature1").GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
		}
		else
		{
			this.gameObject.transform.FindChild("FocusFeature1").FindChild("Title").GetComponent<TextMeshPro>().text="Niveau maximum atteint";
			this.gameObject.transform.FindChild("FocusFeature1").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(false);
		}
		this.gameObject.transform.FindChild("FocusFeature2").transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Renommer l'unité \n( -" + this.c.RenameCost + " cristaux)";
		this.gameObject.transform.FindChild("FocusFeature2").GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
		if(this.c.onSale==1)
		{
			this.gameObject.transform.FindChild("FocusFeature3").transform.FindChild("Title").GetComponent<TextMeshPro>().text="Unité en vente sur le marché pour "+this.c.Price+" cristaux. \nModifier ?";
			this.gameObject.transform.FindChild("FocusFeature3").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
		}
		else
		{
			this.gameObject.transform.FindChild("FocusFeature3").transform.FindChild("Title").GetComponent<TextMeshPro>().text="Mettre l'unité en vente sur le marché";
			this.gameObject.transform.FindChild("FocusFeature3").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
		}
		this.gameObject.transform.FindChild("FocusFeature5").transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Retour";
		this.gameObject.transform.FindChild("FocusFeature5").GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
	}
	public override void refreshCredits()
	{
		StartCoroutine(MenuController.instance.getUserData ());
	}
	public override void deleteCard ()
	{
		base.exitFocus ();
		NewStoreController.instance.deleteCard ();
	}
	public override void goBackToScene()
	{
		NewStoreController.instance.hideCardFocused ();
	}
	public override void selectAFeature(int feature)
	{
		switch(feature)
		{
		case 0:
			base.displaySellCardPopUp();
			break;
		case 1:
			base.displayBuyXpCardPopUp();
			break;
		case 2:
			base.displayRenameCardPopUp();
			break;
		case 3:
			if(c.onSale==1)
			{
				base.displayEditSellCardPopUp();
			}
			else
			{
				base.displayputOnMarketCardPopUp();
			}
			break;
		case 5:
			if(NewStoreController.instance.getIsTutorialLaunched())
			{
				TutorialObjectController.instance.actionIsDone();
			}
			this.exitFocus();
			break;
		}
	}
}

