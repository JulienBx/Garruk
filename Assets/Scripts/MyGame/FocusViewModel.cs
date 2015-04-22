using UnityEngine;
using System.Collections;

public class FocusViewModel
{
	public int focusedCard;
	public int focusedCardPrice;

	public GUIStyle[] styles;
	public GUIStyle focusButtonStyle;
	public GUIStyle cantBuyStyle;

	public FocusViewModel()
	{
		focusedCard = -1;
	}
	
	public void initStyles()
	{
		focusButtonStyle                = styles[0];
		cantBuyStyle                    = styles[1];
	}
}
