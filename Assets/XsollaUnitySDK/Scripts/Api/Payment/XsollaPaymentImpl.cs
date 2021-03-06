using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System;
using System.Text;

namespace Xsolla 
{
	public class XsollaPaymentImpl : MonoBehaviour, IXsollaPayment {

		private string DOMAIN = "https://secure.xsolla.com";

		private const int TRANSLATIONS = 0;
		private const int DIRECTPAYMENT_FORM = 1;
		private const int DIRECTPAYMENT_STATUS = 2;
		private const int PRICEPOINTS = 3;
		private const int GOODS = 5;
		private const int GOODS_GROUPS = 51;
		private const int GOODS_ITEMS = 52;
		private const int PAYMENT_LIST = 6;
		private const int QUICK_PAYMENT_LIST = 7;
		private const int COUNTRIES = 8;


		public Action<XsollaUtils> UtilsRecieved;
		public Action<XsollaTranslations> TranslationRecieved;

		public Action<XsollaPricepointsManager> PricepointsRecieved;
		public Action<XsollaGroupsManager> GoodsGroupsRecieved;
		public Action<XsollaGoodsManager> GoodsRecieved;

		public Action<XsollaPaymentMethods> PaymentMethodsRecieved;
		public Action<XsollaQuickPayments> QuickPaymentMethodsRecieved;
		public Action<XsollaCountries> CountriesRecieved;

		public Action<XsollaForm> FormReceived;
		public Action<XsollaStatus, XsollaForm> StatusReceived;
		public Action<string, int> StatusChecked;
		public Action<XsollaError> ErrorReceived;

		//TODO CHANGE PARAMS
		protected string _accessToken;
		protected Dictionary<string, object> baseParams;

		public XsollaPaymentImpl(){
		}

		public XsollaPaymentImpl(string accessToken){
			//accessToken = "KhevmS4ddZ3lmzZKU4yBlmKwTeKO3bTY";
			this._accessToken = accessToken;

		}

		public void InitPaystation(XsollaWallet xsollaWallet)
		{
			this._accessToken = xsollaWallet.GetToken ();
			GetUtils (null);
//			GetNextStep (new Dictionary<string, object> ());
		}

		public void InitPaystation(string accessToken)
		{
			this._accessToken = accessToken;
			GetUtils (null);
		}

		public void InitPaystation(Dictionary<string, object> pararams)
		{

//			this._accessToken = pararams[XsollaApiConst.ACCESS_TOKEN].ToString();
			baseParams = new Dictionary<string, object> (pararams);
			GetUtils (pararams);
		}

		public void StartPaymentWithoutUtils(XsollaWallet xsollaWallet)
		{
			this._accessToken = xsollaWallet.GetToken ();
			GetNextStep (new Dictionary<string, object> ());
		}

		public void NextStep(Dictionary<string, object> xpsMap)
		{
			GetNextStep (xpsMap);
		}

		public void Status(string token, long invoice)
		{

		}

		public void SetModeSandbox(bool isSandbox)
		{
			if (!isSandbox) { 
				DOMAIN = "https://secure.xsolla.com";
			} else {
				DOMAIN = "https://sandbox-secure.xsolla.com";
			}
		}

		public void SetDomain(string domain)
		{
			DOMAIN = domain;
		}

		public void SetToken (string token)
		{
			this._accessToken = token;
		}

		
		// ---------------------------------------------------------------------------

		private void OnUtilsRecieved(XsollaUtils utils)
		{
			if(UtilsRecieved != null)
				UtilsRecieved(utils);
		}

		private void OnPricepointsRecieved(XsollaPricepointsManager pricepoints)
		{
			if(PricepointsRecieved != null)
				PricepointsRecieved(pricepoints);
		}

		private void OnGoodsRecieved(XsollaGoodsManager goods)
		{
			if(GoodsRecieved != null)
				GoodsRecieved(goods);
		}

		private void OnGoodsGroupsRecieved(XsollaGroupsManager groups)
		{
			if(GoodsGroupsRecieved != null)
				GoodsGroupsRecieved(groups);
		}

		// ---------------------------------------------------------------------------

		private void OnPaymentMethodsRecieved(XsollaPaymentMethods paymentMethods)
		{
			if(PaymentMethodsRecieved != null)
				PaymentMethodsRecieved(paymentMethods);
		}

		private void OnQuickPaymentMethodsRecieved(XsollaQuickPayments quickPayments)
		{
			if(QuickPaymentMethodsRecieved != null)
				QuickPaymentMethodsRecieved(quickPayments);
		}

		private void OnCountriesRecieved(XsollaCountries countries)
		{
			if(CountriesRecieved != null)
				CountriesRecieved(countries);
		}

		protected virtual void OnTranslationRecieved(XsollaTranslations translations) 
		{
			if (TranslationRecieved != null)
				TranslationRecieved(translations);
		}
		
		// ---------------------------------------------------------------------------

		protected virtual void OnFormReceived(XsollaForm form) 
		{
			if (FormReceived != null)
				FormReceived(form);
		}

		protected virtual void OnStatusReceived(XsollaStatus status, XsollaForm form) 
		{
			if (StatusReceived != null)
				StatusReceived(status, form);
		}

		protected virtual void OnStatusChecked(string status, int elapsedTime)
		{
			if (StatusChecked != null)
				StatusChecked(status, elapsedTime);
		}

		protected virtual void OnErrorReceived(XsollaError error) 
		{
			if (ErrorReceived != null)
				ErrorReceived(error);
		}
		
		// ---------------------------------------------------------------------------

		void GetUtils(Dictionary<string, object> pararams)
		{
			//Dictionary<string, object> pararams = new Dictionary<string, object> ();
			POST (TRANSLATIONS, GetUtilsLink(), pararams);
		}

		void GetNextStep(Dictionary<string, object> nextStepParams)
		{
			if (nextStepParams.Count == 0) {
				nextStepParams.Add ("pid", 26);
			}
			if (!nextStepParams.ContainsKey ("paymentWithSavedMethod")) {
				nextStepParams.Add ("paymentWithSavedMethod", 0);
//				nextStepParams.Add ("returnUrl", "");
			}
			POST (DIRECTPAYMENT_FORM, GetDirectpaymentLink(), nextStepParams);
		}

		public void GetStatus(Dictionary<string, object> statusParams)
		{
			POST (DIRECTPAYMENT_STATUS, GetStatusLink(), statusParams);
		}

		public void GetPricePoints(Dictionary<string, object> requestParams)
		{
//			Dictionary<string, object> requestParams = new Dictionary<string, object>();
			POST (PRICEPOINTS, GetPricepointsUrl(), requestParams);
		}

		public void GetGoods(Dictionary<string, object> requestParams)
		{
//			Dictionary<string, object> requestParams = new Dictionary<string, object>();
			POST (GOODS, GetGoodsUrl(), requestParams);
		}



		public void GetItemsGrous(Dictionary<string, object> requestParams)
		{
//			Dictionary<string, object> requestParams = new Dictionary<string, object>();
			POST (GOODS_GROUPS, GetItemsGroupsUrl(), requestParams);
		}

		public void GetItems(long groupId, Dictionary<string, object> requestParams)
		{
//			Dictionary<string, object> requestParams = new Dictionary<string, object>();
			requestParams.Add ("group_id", groupId );//group_id <- NEW | OLD -> requestParams.Add ("id_group",groupId );
			POST (GOODS_ITEMS, GetItemsUrl(), requestParams);
		}

		public void GetFavorites(Dictionary<string, object> requestParams)
		{
			POST (GOODS_ITEMS, GetFavoritsUrl(), requestParams);
		}

		public void SetFavorite(Dictionary<string, object> requestParams)
		{
			POST (999, SetFavoritsUrl(), requestParams);
		}

		public void GetPaymentsInfo(Dictionary<string, object> requestParams)
		{
//			Dictionary<string, object> requestParams = new Dictionary<string, object>();
			POST (QUICK_PAYMENT_LIST, GetQuickPaymentsUrl(), requestParams);
			POST (PAYMENT_LIST, GetPaymentListUrl(), requestParams);
			POST (COUNTRIES, GetCountriesListUrl(), requestParams);
		}

		public void GetQuickPayments(string countryIso, Dictionary<string, object> requestParams)
		{
//			Dictionary<string, object> requestParams = new Dictionary<string, object>();
			if (countryIso != null && !"".Equals (countryIso)) {
				requestParams["country"] = countryIso;
			}
			POST (QUICK_PAYMENT_LIST, GetQuickPaymentsUrl(), requestParams);
		}

		public void GetPayments(string countryIso, Dictionary<string, object> requestParams)
		{
//			Dictionary<string, object> requestParams = new Dictionary<string, object>();
			if (countryIso != null && !"".Equals (countryIso)) {
				requestParams["country"] = countryIso;
			}
			POST (PAYMENT_LIST, GetPaymentListUrl(), requestParams);
		}

		public void GetCountries(Dictionary<string, object> requestParams)
		{
//			Dictionary<string, object> requestParams = new Dictionary<string, object>();
			POST (COUNTRIES, GetCountriesListUrl(), requestParams);
		}

		public WWW POST(int type, string url, Dictionary<string, object> post)
		{
			WWWForm form = new WWWForm();
			StringBuilder sb = new StringBuilder ();
			if (!post.ContainsKey (XsollaApiConst.ACCESS_TOKEN) && !post.ContainsKey ("project") && !post.ContainsKey ("access_data") && baseParams != null)
				foreach (KeyValuePair<string, object> kv in baseParams)
					post.Add (kv.Key, kv.Value);//.Add (XsollaApiConst.ACCESS_TOKEN, _accessToken);
			if (type == DIRECTPAYMENT_STATUS)
				TransactionHelper.SaveRequest (post);
			post.Add ("alternative_platform", "unity/1.2.6");
			foreach(KeyValuePair<string,object> post_arg in post)
			{
				string argValue = post_arg.Value != null ? post_arg.Value.ToString() : "";
				sb.Append(post_arg.Key).Append("=").Append(argValue).Append("&");
				form.AddField(post_arg.Key, argValue);

			}
//			Debug.Log (url);
//			Debug.Log (sb.ToString());
			WWW www = new WWW(url, form);
			StartCoroutine(WaitForRequest(type, www));
			return www; 
		}
		
		private IEnumerator WaitForRequest(int type, WWW www)
		{
			yield return www;
			// check for errors
			if (www.error == null)
			{
				JSONNode rootNode = JSON.Parse(www.text);
				if(rootNode != null && rootNode.Count > 2 || rootNode["error"] == null) {
					switch(type)
					{
						case TRANSLATIONS:
							{
								if(rootNode.Count > 2){
									XsollaUtils utils = new XsollaUtils().Parse(rootNode) as XsollaUtils;
									projectId = utils.GetProject().id.ToString();
									OnUtilsRecieved(utils);
									OnTranslationRecieved(utils.GetTranslations());
								} else {
									XsollaError error = new XsollaError();
									error.Parse(rootNode);
									OnErrorReceived(error);
								}
							}
							break;
						case DIRECTPAYMENT_FORM:
								{
									if(rootNode.Count > 8) {
										XsollaForm form = new XsollaForm();
										form.Parse(rootNode);
										switch (form.GetCurrentCommand()) {
											case XsollaForm.CurrentCommand.STATUS:
												GetStatus(form.GetXpsMap());
												break;
											case XsollaForm.CurrentCommand.CHECKOUT:
											case XsollaForm.CurrentCommand.CHECK:
											case XsollaForm.CurrentCommand.FORM:
											case XsollaForm.CurrentCommand.CREATE:
											case XsollaForm.CurrentCommand.ACCOUNT:
												OnFormReceived(form);
												break;
											case XsollaForm.CurrentCommand.UNKNOWN:
												if(rootNode.Count > 10)
												{
													OnFormReceived(form);
												} else {
													XsollaError error = new XsollaError();
													error.Parse(rootNode);
													OnErrorReceived(error);
												}
												break;
											default:
												break;
										}
									} else {
										OnStatusChecked(rootNode["status"], rootNode["elapsedTime"].AsInt);
									}
							}
							break;
						case DIRECTPAYMENT_STATUS:
							{
								XsollaForm form = new XsollaForm();
								form.Parse(rootNode);
								XsollaStatus status = new XsollaStatus();
								status.Parse(rootNode);
								OnStatusReceived(status, form);
							}
							break;
						case PRICEPOINTS:
							{
								XsollaPricepointsManager pricepoints = new XsollaPricepointsManager();
								pricepoints.Parse(rootNode);
								OnPricepointsRecieved(pricepoints);
							}
							break;
						case GOODS:
							{
								XsollaGoodsManager goods = new XsollaGoodsManager();
								goods.Parse(rootNode);
								OnGoodsRecieved(goods);
							}
							break;
						case GOODS_GROUPS:
							{
								XsollaGroupsManager groups = new XsollaGroupsManager();
								groups.Parse(rootNode);
								OnGoodsGroupsRecieved(groups);
							}
							break;
						case GOODS_ITEMS:
							{
								XsollaGoodsManager goods = new XsollaGoodsManager();
								goods.Parse(rootNode);
								OnGoodsRecieved(goods);
							}
							break;
						case PAYMENT_LIST:
							{
								XsollaPaymentMethods paymentMethods = new XsollaPaymentMethods();
								paymentMethods.Parse(rootNode);
								OnPaymentMethodsRecieved(paymentMethods);
							}
							break;
						case QUICK_PAYMENT_LIST:
							{
								XsollaQuickPayments quickPayments = new XsollaQuickPayments();
								quickPayments.Parse(rootNode);
								OnQuickPaymentMethodsRecieved(quickPayments);
							}
							break;
						case COUNTRIES:
							{
								XsollaCountries countries = new XsollaCountries();
								countries.Parse(rootNode);
								OnCountriesRecieved(countries);
							}
							break;
						default:
							break;
					}
				} else {
					XsollaError error = new XsollaError();
					error.Parse(rootNode);
					OnErrorReceived(error);
				}
			} else {
				JSONNode errorNode = JSON.Parse(www.text);
				string errorMsg = errorNode["errors"].AsArray[0]["message"].Value 
					+ ". Support code " + errorNode["errors"].AsArray[0]["support_code"].Value;
				int errorCode = 0;
				if(www.error.Length > 3)
					errorCode = int.Parse(www.error.Substring(0, 3));
				else
					errorCode = int.Parse(www.error);
				OnErrorReceived(new XsollaError(errorCode, errorMsg));
			}    
			if(projectId != null && !"".Equals(projectId))
				LogEvent ("UNITY 1.2.6 REQUEST", projectId, www.url);
			else 
				LogEvent ("UNITY 1.2.6 REQUEST", "undefined", www.url);
		}



		private string GetUtilsLink(){
			return DOMAIN + "/paystation2/api/utils";
		}

		private string GetDirectpaymentLink(){
			return DOMAIN + "/paystation2/api/directpayment";
		}

		private string GetStatusLink(){
			return DOMAIN + "/paystation2/api/directpayment";
		}

		private string GetPricepointsUrl(){
			return DOMAIN + "/paystation2/api/pricepoints";
		}
		
		private string GetGoodsUrl(){
			return DOMAIN + "/paystation2/api/digitalgoods";
		}

		private string GetFavoritsUrl(){
			return DOMAIN + "/paystation2/api/virtualitems/favorite";
		}

		private string SetFavoritsUrl(){
			return DOMAIN + "/paystation2/api/virtualitems/setfavorite";
		}
		
		private string GetItemsGroupsUrl(){
			return DOMAIN + "/paystation2/api/virtualitems/groups";
		}


		
		private string GetItemsUrl(){
			return DOMAIN + "/paystation2/api/virtualitems/items";
		}

		private string GetPaymentListUrl(){
			return DOMAIN + "/paystation2/api/paymentlist";
		}

		private string GetQuickPaymentsUrl(){
			return DOMAIN + "/paystation2/api/paymentlist/quick_payments";
		}

		private string GetCountriesListUrl(){
			return DOMAIN + "/paystation2/api/country";
		}

		//*** GA SECTION START ***
		private string propertyID = "UA-62372273-1";
		
		private static XsollaPaymentImpl instance;
		
		private string bundleID = "Unity";
		private string appName;
		private string projectId = "";
		private const string appVersion = "1.2.6";
		
		private string screenRes;
		private string clientID;
		
//		void Awake()
//		{
//			if(instance)
//				DestroyImmediate(gameObject);
//			else
//			{
//				DontDestroyOnLoad(gameObject);
//				instance = this;
//			}
//		}
		
		void Start() 
		{
			appName = GetProjectName ();
			screenRes = Screen.width + "x" + Screen.height;
			
			#if UNITY_IPHONE
			clientID = SystemInfo.deviceUniqueIdentifier;
			#else
			clientID = SystemInfo.deviceUniqueIdentifier;
			#endif
			
		}
		
		public void LogScreen(string title)
		{
//			Debug.Log ("Google Analytics - Screen --> " + title);
			
			title = WWW.EscapeURL(title);
			
			var url = "https://www.google-analytics.com/collect?v=1&ul=en-us&t=appview&sr="+screenRes+"&an="+WWW.EscapeURL(appName)+"&a=448166238&tid="+propertyID+"&aid="+bundleID+"&cid="+WWW.EscapeURL(clientID)+"&_u=.sB&av="+appVersion+"&_v=ma1b3&cd="+title+"&qt=2500&z=185";
			
			StartCoroutine( Process(new WWW(url)) );
		}
		
		/*  MOBILE EVENT TRACKING:  https://developers.google.com/analytics/devguides/collection/protocol/v1/devguide */
		public void LogEvent(string titleCat, string titleAction, string actionLable)
		{
//			Debug.Log ("Google Analytics - Event --> " + titleAction);
			
			titleCat = WWW.EscapeURL(titleCat);
			titleAction = WWW.EscapeURL(titleAction);
			actionLable = WWW.EscapeURL (actionLable);
			
			var url = "https://www.google-analytics.com/collect?v=1&ul=en-us&t=event&sr="+screenRes+"&an="+WWW.EscapeURL(appName)+"&a=448166238&tid="+propertyID+"&aid="+bundleID+"&cid="+WWW.EscapeURL(clientID)+"&_u=.sB&av="+appVersion+"&_v=ma1b3&ec="+titleCat+"&ea="+titleAction+"&el"+actionLable+"&qt=2500&z=185";
			
			StartCoroutine( Process(new WWW(url)) );
		}

		public void LogEvent(string titleCat, string actionLable)
		{
//			Debug.Log ("Google Analytics - Event --> " + actionLable);
			
			titleCat = WWW.EscapeURL(titleCat);
			if (projectId == null) {
				projectId = GetProjectName();//PlayerSettings.companyName + " | " + PlayerSettings.productName;
				projectId = WWW.EscapeURL (projectId);
			}
			actionLable = WWW.EscapeURL (actionLable);
			
			var url = "https://www.google-analytics.com/collect?v=1&ul=en-us&t=event&sr="+screenRes+"&an="+WWW.EscapeURL(appName)+"&a=448166238&tid="+propertyID+"&aid="+bundleID+"&cid="+WWW.EscapeURL(clientID)+"&_u=.sB&av="+appVersion+"&_v=ma1b3&ec="+titleCat+"&ea="+projectId+"&el="+actionLable+"&qt=2500&z=185";
			
			StartCoroutine( Process(new WWW(url)) );
		}
		
		
		
		private IEnumerator Process(WWW www)
		{
			yield return www;
			
			if(www.error == null)
			{
				if (www.responseHeaders.ContainsKey("STATUS"))
				{
					if (www.responseHeaders["STATUS"] == "HTTP/1.1 200 OK")	
					{
//						Debug.Log ("GA Success");
					} else {
//						Debug.LogWarning(www.responseHeaders["STATUS"]);	
					}
				}else{
//					Debug.LogWarning("Event failed to send to Google");	
				}
			}else{
//				Debug.LogWarning(www.error.ToString());	
			}
			
			www.Dispose();
		}
		
		//*** GA SECTION END ***

		public string GetProjectName()
		{
			string[] s = Application.dataPath.Split('/');
			string projectName = s[s.Length - 2];
//			Debug.Log("project = " + projectName);
			return projectName;
		}
		
	}
}
