using UnityEngine;
using TMPro;

public class newMyGameDeckSelectionButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		newMyGameController.instance.displayDeckList();	
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
					newMyGameController.instance.mouseOnSelectDeckButton(true);
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
					newMyGameController.instance.mouseOnSelectDeckButton(false);
				}
			}
		}
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingDeck.getReference(18),WordingDeck.getReference(19));
	}
}

