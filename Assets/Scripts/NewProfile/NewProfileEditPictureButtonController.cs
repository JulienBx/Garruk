using UnityEngine;
using TMPro;

public class NewProfileEditPictureButtonController : SimpleButtonController 
{
	public override void mainInstruction()
	{
		NewProfileController.instance.editProfilePictureHandler ();
	}
	public override void OnMouseOver()
	{
		if(!base.getIsHovered())
		{
			base.setIsHovered(true);
		}
	}
	public override void OnMouseExit()
	{
		if(base.getIsHovered())
		{	
			base.setIsHovered(false);
			NewProfileController.instance.endHoveringProfilePicture ();
		}
	}
}

