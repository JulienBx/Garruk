using UnityEngine;
using TMPro;

public class NewFocusedCardHomePageController : NewFocusedCardController
{
	public override void initializeFocusFeatures()
	{
		this.gameObject.transform.FindChild ("FocusFeature2").gameObject.SetActive (false);
		this.gameObject.transform.FindChild ("FocusFeature3").gameObject.SetActive (false);
	}
	public override void updateFocusFeatures()
	{
		if(this.c.ExperienceLevel!=10)
		{
			this.gameObject.transform.FindChild("FocusFeature0").FindChild("Title").GetComponent<TextMeshPro>().text="Passer la carte au niveau suivant \n( -"+this.c.NextLevelPrice+ " crédits)";
			this.gameObject.transform.FindChild("FocusFeature0").GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
		}
		else
		{
			this.gameObject.transform.FindChild("FocusFeature0").FindChild("Title").GetComponent<TextMeshPro>().text="Niveau maximum atteint";
			this.gameObject.transform.FindChild("FocusFeature0").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(false);
		}
		this.gameObject.transform.FindChild("FocusFeature1").transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Renommer la carte \n( -" + this.c.RenameCost + " crédits)";
		this.gameObject.transform.FindChild("FocusFeature4").transform.GetComponent<TextMeshPro>().text=this.c.nbWin+" Victoires \n" + this.c.nbLoose+" Défaites";
		this.gameObject.transform.FindChild("FocusFeature5").transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Quitter";
	}
	public override void refreshCredits()
	{
		NewHomePageController.instance.refreshCredits ();
	}
	public override void exitFocus()
	{
		base.exitFocus ();
		NewHomePageController.instance.hideCardFocused ();
	}
	public override void focusFeaturesHandler(int id)
	{
		switch(id)
		{
		case 0:
			base.displayBuyXpCardPopUp();
			break;
		case 1:
			base.displayRenameCardPopUp();
			break;
		case 5:
			this.exitFocus();
			break;
		}
	}
}

