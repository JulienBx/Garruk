using UnityEngine;
using TMPro;

public class NewProfilePictureButtonController : SpriteButtonController 
{
	public override void OnMouseOver()
	{
		if(!base.getIsHovered())
		{
			NewProfileController.instance.startHoveringProfilePicture ();
			base.setIsHovered(true);
		}
	}
	public override void OnMouseExit()
	{
		if(base.getIsHovered())
		{	
			base.setIsHovered(false);
		}
	}
	public override void mainInstruction()
	{
		if(ApplicationDesignRules.isMobileScreen)
		{
			NewProfileController.instance.editProfilePictureHandler ();
		}
	}
}

