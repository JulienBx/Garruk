//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//using TMPro;
//
//public class ProfilePictureButtonController : MonoBehaviour 
//{
//	private bool isHovering;
//	
//	void OnMouseDown()
//	{
//		NewProfileController.instance.editProfilePictureHandler ();
//	}
//	void OnMouseOver()
//	{
//		if(!isHovering)
//		{
//			this.isHovering=true;
//		}
//	}
//	void OnMouseExit()
//	{
//		if(isHovering)
//		{
//			NewProfileController.instance.endHoveringProfilePicture();
//			this.isHovering=false;
//		}
//	}
//}
//
