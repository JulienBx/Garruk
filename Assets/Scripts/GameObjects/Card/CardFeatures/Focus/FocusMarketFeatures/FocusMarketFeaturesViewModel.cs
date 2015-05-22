using UnityEngine;

public class FocusMarketFeaturesViewModel
{
	public Rect[] cardFeaturesFocusRects;

	public string usernameOwner;
	public int price;
	public int idOwner;
	public int nbWin;
	public int nbLoose;
	public bool onSale;

	
	public FocusMarketFeaturesViewModel ()
	{
		this.cardFeaturesFocusRects=new Rect[3];
	}

}

