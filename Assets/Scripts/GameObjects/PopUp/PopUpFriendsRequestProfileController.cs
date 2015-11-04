//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//using TMPro;
//
//public class PopUpFriendsRequestProfileController : PopUpController
//{
//	
//	public override void startHoveringPopUp()
//	{
//		NewProfileController.instance.startHoveringPopUp();
//	}
//	public override void endHoveringPopUp()
//	{
//		NewProfileController.instance.endHoveringPopUp();
//	}
//	public void show(FriendsRequest f)
//	{
//		gameObject.transform.FindChild ("date").gameObject.SetActive (false);
//		if(f.IsInvitingPlayer)
//		{
//			gameObject.transform.FindChild("Button").gameObject.SetActive(true);
//			gameObject.transform.FindChild ("Button").gameObject.AddComponent <PopUpAcceptFriendsRequestButtonController>();
//			gameObject.transform.FindChild("Button2").gameObject.SetActive(true);
//			gameObject.transform.FindChild ("Button2").gameObject.AddComponent <PopUpDeclineFriendsRequestButtonController>();
//			gameObject.transform.FindChild ("Button").transform.localPosition = new Vector3 (-0.9f, -0.6f, -1f);
//			gameObject.transform.FindChild ("Button2").transform.localPosition = new Vector3 (0.9f, -0.6f, -1f);
//			gameObject.transform.FindChild("content").GetComponent<TextMeshPro>().text="vous a invité à devenir son ami";
//			gameObject.transform.FindChild("Button").FindChild("Title").GetComponent<TextMeshPro>().text="Accepter";
//			gameObject.transform.FindChild("Button2").FindChild("Title").GetComponent<TextMeshPro>().text="Rejeter";
//		}
//		else
//		{
//			gameObject.transform.FindChild("Button").gameObject.SetActive(true);
//			gameObject.transform.FindChild ("Button").gameObject.AddComponent <PopUpCancelFriendsRequestButtonController>();
//			gameObject.transform.FindChild("Button2").gameObject.SetActive(false);
//			gameObject.transform.FindChild ("Button").transform.localPosition = new Vector3 (0f, -0.6f, -1f);
//			gameObject.transform.FindChild("content").GetComponent<TextMeshPro>().text="n'a pas encore répondu à votre invitation";
//			gameObject.transform.FindChild("Button").FindChild("Title").GetComponent<TextMeshPro>().text="Retirer";
//		}
//
//		gameObject.transform.FindChild ("user").GetComponent<PopUpUserController> ().show (f.User);
//	}
//}
//
