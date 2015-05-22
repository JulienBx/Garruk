using UnityEngine;

public class FocusLobbyFeaturesViewModel
{
	public Rect[] cardFeaturesFocusRects;
	
	public string title;
	public int renameCost;
	public int nextLevelCost;
	public int cardLevel;
	public int nbWin;
	public int nbLoose;
	
	public FocusLobbyFeaturesViewModel ()
	{
		this.cardFeaturesFocusRects=new Rect[4];
	}
}

