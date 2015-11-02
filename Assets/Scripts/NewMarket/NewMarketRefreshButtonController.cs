using UnityEngine;
using TMPro;

public class NewMarketRefreshButtonController : TextButtonController
{
	private bool isHoveredColor;

	void OnMouseDown()
	{
		NewMarketController.instance.displayNewCards();	
	}
	public void changeColor()
	{
		if(!base.getIsHovered())
		{
			if(isHoveredColor)
			{
				this.setInitialState();
				this.isHoveredColor=false;
			}
			else
			{
				this.setHoveredState();
				this.isHoveredColor=true;
			}
		}
	}
}

