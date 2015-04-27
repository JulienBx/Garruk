using UnityEngine;

public class MarketFeaturesViewModel
{
	public Rect rect;
	public string usernameOwner;
	public int price;
	public int idOwner;
	public GUIStyle[] styles;
	public GUIStyle cantBuyPriceStyle;
	public GUIStyle canBuyPriceStyle;
	public GUIStyle titleStyle;
	public GUIStyle ownerButtonStyle;
	public GUIStyle buyButtonStyle;
	public bool guiEnabled;


	public MarketFeaturesViewModel ()
	{
		this.guiEnabled = true;
		this.rect = new Rect ();
		this.styles=new GUIStyle[0];
		this.titleStyle = new GUIStyle ();
		this.canBuyPriceStyle=new GUIStyle();
		this.cantBuyPriceStyle = new GUIStyle ();
		this.ownerButtonStyle = new GUIStyle ();
		this.buyButtonStyle = new GUIStyle ();
	}
	public void initStyles()
	{
		this.cantBuyPriceStyle = this.styles [0];
		this.canBuyPriceStyle = this.styles [1];
		this.titleStyle = this.styles [2];
		this.ownerButtonStyle = this.styles [3];
		this.buyButtonStyle = this.styles [4];
	}
	public void resize(float cardHeight)
	{
		this.titleStyle.fontSize = (int)cardHeight * 8 / 100;
		this.canBuyPriceStyle.fontSize = (int)cardHeight * 8 / 100;
		this.cantBuyPriceStyle.fontSize = (int)cardHeight * 8 / 100;
		this.buyButtonStyle.fontSize = (int)cardHeight * 8 / 100;
		this.ownerButtonStyle.fontSize = (int)cardHeight * 8 / 100;
	}
}

