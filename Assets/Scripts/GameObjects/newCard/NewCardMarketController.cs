using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
	void OnMouseUp()
	{
		NewMarketController.instance.leftClickReleaseHandler ();
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
		if(this.c.onSale==0 && this.c.IdOWner!=NewMarketController.instance.returnUserId())
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
		if(this.c.onSale==0)
		{
			this.panelMarket.GetComponent<NewCardPanelMarketController>().setClickable(true);
			this.panelMarket.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingStore.getReference(8);
			this.panelMarket.transform.FindChild("Cristal").gameObject.SetActive(false);
		}
		else 
		{
			this.panelMarket.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = ApplicationDesignRules.priceToString(this.c.Price);
			this.panelMarket.transform.FindChild("Cristal").gameObject.SetActive(true);
			if(this.c.IdOWner!=NewMarketController.instance.returnUserId())
			{
				if(this.c.Price<=ApplicationModel.credits)
				{
					this.panelMarket.GetComponent<NewCardPanelMarketController>().setClickable(true);
				}
				else
				{
					this.panelMarket.GetComponent<NewCardPanelMarketController>().setClickable(false);
				}
			}
			else
			{
				this.panelMarket.GetComponent<NewCardPanelMarketController>().setClickable(true);
			}
		}
	}
	public void panelMarketHandler()
	{
		StartCoroutine (panelMarketFunction());
	}
	private IEnumerator panelMarketFunction()
	{
		yield return new WaitForSeconds (0.1f);
		if(NewMarketController.instance.canClick())
		{
			NewMarketController.instance.communicateCardIndex (this.id);
			if(this.c.onSale==0)
			{
				base.displayputOnMarketCardPopUp();
			}
			else if(this.c.IdOWner!=NewMarketController.instance.returnUserId())
			{
				base.displayBuyCardPopUp();
			}
			else
			{
				base.displayEditSellCardPopUp();
			}
		}
	}
	public override void hidePanelMarket()
	{
		this.panelMarket.SetActive (false);
	}
	public override void actualizePrice()
	{
		this.setMarketFeatures ();
	}
	public override void updateScene()
	{
		NewMarketController.instance.updateScene ();
	}
	public override void deleteCard()
	{
		NewMarketController.instance.deleteCard ();
	}
	public override Camera getCurrentCamera()
	{
		return NewMarketController.instance.returnCurrentCamera ();
	}
}

