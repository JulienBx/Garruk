using UnityEngine;
using TMPro;

public class NewCardMarketController : NewCardController
{
	private int id;
	private GameObject panelMarket;

	public override void Update()
	{
	}
	public override void Awake()
	{
		base.Awake ();
		this.setUpdateSpeed ();
		this.panelMarket = this.gameObject.transform.FindChild ("PanelMarket").gameObject;
	}
	public override void OnMouseOver()
	{
		base.OnMouseOver ();
//		if (Input.GetMouseButton(1)) 
//		{
//			NewMarketController.instance.rightClickedHandler(this.id);
//		}
	}
	void OnMouseDown()
	{
		NewMarketController.instance.leftClickedHandler (this.id);
	}
	public override void OnMouseExit()
	{
		base.OnMouseExit ();
	}
	public void setId(int value)
	{
		this.id = value;
	}
	public override void show()
	{
		base.show ();
		this.setMarketFeatures ();
	}
	public void setMarketFeatures()
	{
		if(this.c.onSale==0)
		{
			base.displayPanelSold ();
			this.hidePanelMarket ();
		}
		else
		{
			base.hidePanelSold ();
			this.displayPanelMarket ();
		}
	}
	public void displayPanelMarket()
	{
		this.panelMarket.SetActive (true);
		this.panelMarket.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = this.c.Price.ToString();
		if(this.c.Price<=ApplicationModel.credits)
		{
			this.panelMarket.GetComponent<NewCardPanelMarketController>().setClickable(true);
		}
		else
		{
			this.panelMarket.GetComponent<NewCardPanelMarketController>().setClickable(false);
		}
	}
	public override void hidePanelMarket()
	{
		this.panelMarket.SetActive (false);
	}
	public void communicateIdCard()
	{
		NewMarketController.instance.retrieveIdCardClicked (this.id);
	}
	public override void actualizePrice()
	{
		this.setMarketFeatures ();
	}
}

