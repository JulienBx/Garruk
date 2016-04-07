using UnityEngine;
using TMPro;

public class NewCardPanelMarketController : SimpleButtonController
{

	private string toolTipTitle;
	private string toolTipDescription;


	public void setToolTipLabels(string toolTipTitle, string toolTipDescription)
	{
		this.toolTipTitle=toolTipTitle;
		this.toolTipDescription=toolTipDescription;
	}
	public override void setInitialState ()
	{
		gameObject.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
		gameObject.transform.FindChild("Cristal").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
	}
	public override void setHoveredState ()
	{
		gameObject.GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
	}
	public override void setForbiddenState ()
	{
		gameObject.GetComponent<SpriteRenderer>().color=new Color(166f/255f,31f/255f,28f/255f);
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(166f/255f,31f/255f,28f/255f);
		gameObject.transform.FindChild("Cristal").GetComponent<SpriteRenderer>().color=new Color(166f/255f,31f/255f,28f/255f);
	}
	public override void mainInstruction ()
	{
		SoundController.instance.playSound(8);
		this.gameObject.transform.parent.GetComponent<NewCardMarketController>().panelMarketHandler();
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(this.toolTipTitle,this.toolTipDescription);
	}
}

