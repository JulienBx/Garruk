using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

namespace Xsolla {
	public class StatusViewController : ScreenBaseConroller<XsollaStatus> {
		
		public event Action<XsollaStatus.Group, XsollaStatusData> StatusHandler;
		public GameObject status;
		public Transform checkListTransform;
		public Button statusViewExitButton;

		// prefabs for checklist linearLayout
		public GameObject rowTitlePrefab;
		public GameObject rowStatusPrefab;
		public GameObject rowLinePrefab;
		public GameObject rowInfoElementPrefab;
		public GameObject rowInfoElementBigPrefab;

		public override void InitScreen (XsollaTranslations translations, XsollaStatus xsollaStatus)
		{
			ResizeToParent ();
			string input = translations.Get (XsollaTranslations.STATUS_PURCHASED_DESCRIPTION);
			XsollaStatusText statusText = xsollaStatus.GetStatusText ();
			XsollaStatus.Group currentStatus = xsollaStatus.GetGroup ();
			if (input != null) {
				string pattern = "{{.*?}}";
				Regex regex = new Regex (pattern);
				input = regex.Replace (input, statusText.GetPurchsaeValue(), 1);
				input = regex.Replace (input, statusText.Get ("sum").GetValue (), 1);
			} else {
				input = "";
			}
			PrepareStatus (currentStatus, xsollaStatus.GetStatusText().GetState (), input, xsollaStatus.GetInvoice());
			AddTitle (statusText.GetProjectString());
			if(currentStatus == XsollaStatus.Group.DONE)
				AddStatus (translations.Get (XsollaTranslations.VIRTUALSTATUS_DONE_DESCRIPTIONS));

			AddElement (statusText.GetPurchsaeValue(), statusText.Get ("sum").GetValue ());
			
			XsollaStatusText.StatusTextElement element = statusText.Get ("out");
			if(element != null)
				AddElement (statusText.Get("out").GetPref(), statusText.Get("out").GetValue());
			element = statusText.Get ("invoice");
			if(element != null)
				AddElement (statusText.Get("invoice").GetPref(), statusText.Get("invoice").GetValue());
			element = statusText.Get ("details");
			if(element != null)
				AddElement (statusText.Get("details").GetPref(), statusText.Get("details").GetValue());
			element = statusText.Get ("time");
			if(element != null)
				AddElement (statusText.Get("time").GetPref(), statusText.Get("time").GetValue());
			element = statusText.Get ("merchant");
			if(element != null)
				AddElement (statusText.Get("merchant").GetPref(), statusText.Get("merchant").GetValue());
			AddLine ();
			AddBigElement (statusText.Get("sum").GetPref(), statusText.Get("sum").GetValue());
//			Debug.Log ("statusText.backUrlCaption " + statusText.backUrlCaption);

			if(statusText.backUrlCaption != null && !"".Equals(statusText.backUrlCaption))
				statusViewExitButton.gameObject.GetComponent<Text> ().text = statusText.backUrlCaption;
			else
				statusViewExitButton.gameObject.GetComponent<Text> ().text = "< Back";
			statusViewExitButton.onClick.AddListener (delegate {OnClickExit(currentStatus, xsollaStatus.GetStatusData());});

		}

		public void PrepareStatus(XsollaStatus.Group group, string state, string purchase, string invoice){
			Text[] texts = status.GetComponentsInChildren<Text> ();// [0] Icon [1] Title [2] purchse
//  			Image bg = status.GetComponent<Image> ();
			ColorController colorController = GetComponent<ColorController> ();
			texts[1].text = state;
//			texts[2].text = purchase;
			switch (group){
				case XsollaStatus.Group.DONE:
					texts[0].text = "";
					colorController.ChangeColor(1, StyleManager.BaseColor.bg_ok);
					break;
				case XsollaStatus.Group.TROUBLED:
					texts[0].text = "";
					colorController.ChangeColor(1, StyleManager.BaseColor.bg_error);
//					bg.color = new Color (0.980392157f, 0.454901961f, 0.392156863f);
					break;
				case XsollaStatus.Group.INVOICE:
				case XsollaStatus.Group.DELIVERING:
				case XsollaStatus.Group.UNKNOWN:
				default:
					texts[0].text = "";
					texts[0].gameObject.AddComponent<MyRotation>();
					colorController.ChangeColor(1, StyleManager.BaseColor.selected);
//					bg.color = new Color (0.639215686f, 0.552941176f, 0.847058824f);
					StartCoroutine(UpdateStatus(invoice));
					break;
			}
		}

		private void AddTitle(string s){
			GameObject titleInstance = Instantiate (rowTitlePrefab) as GameObject;
			titleInstance.transform.SetParent (checkListTransform);
			titleInstance.GetComponentInChildren<Text> ().text = s;
		}

		private void AddStatus(string s){
			GameObject statusInstance = Instantiate (rowStatusPrefab) as GameObject;
			statusInstance.transform.SetParent (checkListTransform);
			statusInstance.GetComponentInChildren<Text> ().text = s;
		}

		private void AddLine(){
			GameObject lineInstance = Instantiate (rowLinePrefab) as GameObject;
			lineInstance.transform.SetParent (checkListTransform);
		}

		private void AddElement(string s, string s1){
			GameObject elementInstance = Instantiate (rowInfoElementPrefab) as GameObject;
			elementInstance.transform.SetParent (checkListTransform);
			Text[] texts = elementInstance.GetComponentsInChildren<Text> ();
			texts [0].text = s;
			texts [1].text = s1;
		}

		private void AddBigElement(string s, string s1){
			GameObject elementInstance = Instantiate (rowInfoElementBigPrefab) as GameObject;
			elementInstance.transform.SetParent (checkListTransform);
			Text[] texts = elementInstance.GetComponentsInChildren<Text> ();
			texts [0].text = s;
			texts [1].text = s1;
		}
		
		private IEnumerator UpdateStatus(string invoice)
		{
			yield return new WaitForSeconds(5);
			Dictionary<string, object> map = new Dictionary<string, object> ();
			map.Add ("section", "getstatus");
			map.Add ("action", "getstatus");
			map.Add ("invoice", invoice);
			gameObject.GetComponentInParent<XsollaPaystationController> ().GetStatus (map);//DoPayment (new Dictionary<string, object>());
		}

		private void OnClickExit(XsollaStatus.Group group ,XsollaStatusData xsollaStatusData)
		{
			if (StatusHandler != null)
				StatusHandler (group, xsollaStatusData);
			GetComponentInParent<XsollaPaystationController> ().gameObject.GetComponentInChildren<Selfdestruction> ().DestroyRoot ();
		}

		private void OnClickBack(XsollaStatus.Group group ,XsollaStatusData xsollaStatusData)
		{
			if (StatusHandler != null)
				StatusHandler (group, xsollaStatusData);
		}

	}

}
