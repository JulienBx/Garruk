using UnityEngine;

public class FocusMyGameFeaturesViewModel
{
	public Rect[] cardFeaturesFocusRects;

	public string title;
	public int renameCost;
	public int cardCost;
	public int nextLevelCost;
	public int cardLevel;
	public bool canBeSold;
	public bool isOnSale;
	public int price;
	public int nbWin;
	public int nbLoose;
	public int idOwner;
	public bool[] buttonsEnabled;

	public FocusMyGameFeaturesViewModel ()
	{
		this.cardFeaturesFocusRects=new Rect[6];
		this.buttonsEnabled=new bool[2];
		for(int i=0;i<this.buttonsEnabled.Length;i++)
		{
			this.buttonsEnabled[i]=true;
		}
	}
}

