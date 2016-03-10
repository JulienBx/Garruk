using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using System.Text.RegularExpressions;

namespace Xsolla 
{

	public abstract class ScreenBaseConroller<T> : MonoBehaviour 
	{

		public delegate void RecieveError(XsollaError xsollaError);
		
		public event Action<XsollaError> ErrorHandler;

		protected Dictionary<string, GameObject> screenObjects;

		private const string PrefabStatus = 		"Prefabs/SimpleView/Status";
		private const string PrefabStatusWaiting = 	"Prefabs/SimpleView/StatusWaiting";
		private const string PrefabTitle =			"Prefabs/SimpleView/TitleNoImg";
		private const string PrefabtwoTextPlate = 	"Prefabs/SimpleView/_ScreenCheckout/TwoTextGrayPlate";
		private const string PrefabError = 			"Prefabs/SimpleView/Error";
		private const string PrefabListView = 		"Prefabs/SimpleView/ListView";
		private const string PrefabInstructions = 	"Prefabs/SimpleView/Instructions";
		private const string PrefabButton = 		"Prefabs/SimpleView/Button";
		private const string PrefabClose = 			"Prefabs/SimpleView/Close";
		private const string PrefabEmpty = 			"Prefabs/SimpleView/Empty";

		public ScreenBaseConroller(){
			screenObjects = new Dictionary<string, GameObject> ();
		}

		public abstract void InitScreen (XsollaTranslations translations, T model);
		private void DrawScreen (XsollaTranslations translations, T model){
		}

		protected void InitView()
		{
			foreach (KeyValuePair<string, GameObject> go in screenObjects) 
			{
				screenObjects[go.Key] = Instantiate(go.Value) as GameObject;
			}
		}

		protected GameObject GetObjectByTag(string tag)
		{
			if (screenObjects.ContainsKey (tag))
				return screenObjects [tag];
			return null;
		}

		protected GameObject GetOkStatus(string titleText){
			
			if (titleText != null) 
			{
				GameObject statusObj = GetObject(PrefabStatus);
				SetText (statusObj, titleText);
				return statusObj;
			}
			return null;
		}

		protected GameObject GetWaitingStatus(string titleText){
			
			if (titleText != null) 
			{
				GameObject statusObj = GetObject(PrefabStatusWaiting);
				SetText (statusObj, titleText);
				return statusObj;
			}
			return null;
		}

		protected GameObject GetTitle(string titleText){
			
			if (titleText != null) 
			{
				GameObject titleObj =  GetObject(PrefabTitle);
				SetText (titleObj, titleText);
				return titleObj;
			}
			return null;
		}

		protected GameObject GetTwoTextPlate(string titleText, string valueText){
			if (titleText != null) 
			{
				GameObject textPlate = GetObject(PrefabtwoTextPlate);
				Text[] texts = textPlate.GetComponentsInChildren<Text>();
				texts[0].text = titleText;
				texts[1].text = valueText;
				return textPlate;
			}
			return null;
		}

		protected GameObject GetErrorByString(string error)
		{
			bool isError = error != null;
			if (isError)
			{
				GameObject errorObj = GetObject(PrefabError);
				SetText (errorObj, error);
				return errorObj;
			}
			return null;
		}

		protected GameObject GetError(XsollaError error)
		{
			bool isError = error != null;
			if (isError)
			{
				GameObject errorObj = GetObject(PrefabError);
				SetText (errorObj, error.GetMessage());
				return errorObj;
			}
			return null;
		}
		
		protected GameObject GetList(IBaseAdapter adapter)
		{

			if (adapter != null) 
			{
				GameObject listViewObj = GetObject(PrefabListView);
				ListView listView = listViewObj.GetComponent<ListView> ();
				listView.SetAdapter(adapter);
				listView.DrawList ();
				return listViewObj;
			} 
			return null;
		}

		protected GameObject GetTextPlate(string s)
		{
			if (s != null) 
			{
				int start = s.IndexOf("<a");
				int end = s.IndexOf("a>");
				string taggedText = s.Substring(start, end - start + 2);
				string[] linkedText = taggedText.Split(new Char [] {'<', '>'});
				string newString = "<color=#a38dd8>" + linkedText[2] + "</color>";
				s = s.Replace(taggedText, newString);
				GameObject textPlate = GetObject(PrefabInstructions);
				SetText(textPlate, s);
				return textPlate;
			} 
			return null;
		}
		protected GameObject GetButton(string text, UnityAction onClick)
		{
			if (text != null)
			{ 
				GameObject buttonObj = GetObject(PrefabButton);
				SetText (buttonObj, text);
				buttonObj.GetComponentInChildren<Button> ().onClick.AddListener (onClick);
				return buttonObj;
			}
			return null;
		}

		protected GameObject GetHelp(XsollaTranslations translations)
		{
			if (translations != null)
			{ 
				GameObject helpObj = GetObject("Prefabs/SimpleView/Help");
				Text[] texsts = helpObj.GetComponentsInChildren<Text>();
				texsts[0].text = translations.Get(XsollaTranslations.SUPPORT_PHONE);
				texsts[1].text = translations.Get(XsollaTranslations.SUPPORT_NEED_HELP);
				texsts[2].text = "support@xsolla.com";
				texsts[3].text = translations.Get(XsollaTranslations.SUPPORT_CUSTOMER_SUPPORT);
				return helpObj;
			}
			return null;
		}

		protected GameObject GetClose(UnityAction onClick)
		{
			if (onClick != null)
			{ 
				GameObject buttonObj = GetObject(PrefabClose);
				buttonObj.GetComponentInChildren<Button> ().onClick.AddListener (onClick);
				return buttonObj;
			}
			return null;
		}

		protected GameObject GetEmpty()
		{
			return GetObject(PrefabEmpty);
		}

		protected void OnErrorRecived(XsollaError error)
		{
			if(ErrorHandler != null)
				ErrorHandler(error);
		}

		public GameObject GetObject(String pathToPrefab){
			return Instantiate (Resources.Load (pathToPrefab)) as GameObject;
		}

		public string  GetFirstAHrefText(string s){
			int start = s.IndexOf("<a");
			int end = s.IndexOf("a>");
			string taggedText = s.Substring(start, end - start + 2);
			string[] text = taggedText.Split(new Char [] {'<', '>'});
			return text [2];
		}

		protected void SetImage(GameObject go, string imgUrl)
		{
			Image[] i2 = go.GetComponentsInChildren<Image>();
			GetComponent<ImageLoader>().LoadImage(i2[1], imgUrl);
		}

		protected void SetText(GameObject go, string s)
		{
			go.GetComponentInChildren<Text>().text = s;
		}

		protected void SetText(Text text, string s)
		{
			text.text = s;
		}

		protected void ResizeToParent()
		{
			RectTransform containerRectTransform = GetComponent<RectTransform>();
			RectTransform parentRectTransform = transform.parent.gameObject.GetComponent<RectTransform> ();
			float parentHeight = parentRectTransform.rect.height;
			float parentWidth = parentRectTransform.rect.width;
			float parentRatio = parentWidth/parentHeight;// > 1 horizontal
			float width = containerRectTransform.rect.width;
			if (parentRatio < 1) {
				containerRectTransform.offsetMin = new Vector2 (-parentWidth/2, -parentHeight/2);
				containerRectTransform.offsetMax = new Vector2 (parentWidth/2, parentHeight/2);
			} else {
				float newWidth = parentWidth/3;
				if(width < newWidth){
					containerRectTransform.offsetMin = new Vector2 (-newWidth/2, -parentHeight/2);
					containerRectTransform.offsetMax = new Vector2 (newWidth/2, parentHeight/2);
				}
			}
		}

	}

}
