using UnityEngine;

public class MenuHelpController : SpriteButtonController
{
	private bool isHoveredColor;

	public override void mainInstruction()
	{
		MenuController.instance.helpHandler ();
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

