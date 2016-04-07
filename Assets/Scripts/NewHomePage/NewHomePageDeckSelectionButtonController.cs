using UnityEngine;
using TMPro;

public class NewHomePageDeckSelectionButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		NewHomePageController.instance.deckSelectionButtonHandler();	
	}
	public override void OnMouseOver()
	{
		if(base.getIsActive())
		{
			if(!base.getIsSelected())
			{
				if(!base.getIsHovered())
				{
					if(!ApplicationDesignRules.isMobileScreen)
					{
						this.setHoveredState();
					}
					this.setIsHovered(true);
					this.showToolTip();
					NewHomePageController.instance.mouseOnSelectDeckButton(true);
				}
			}
		}
	}
	public override void OnMouseExit()
	{
		if(base.getIsActive())
		{	
			if(!base.getIsSelected())
			{
				if(base.getIsHovered())
				{
					if(!ApplicationDesignRules.isMobileScreen)
					{
						this.setInitialState();
					}
					this.setIsHovered(false);
					this.hideToolTip();
					NewHomePageController.instance.mouseOnSelectDeckButton(false);
				}
			}
		}
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingDeck.getReference(18),WordingDeck.getReference(19));
	}
}

