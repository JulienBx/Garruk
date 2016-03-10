
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace Xsolla {
	public class DataLoader : MonoBehaviour {

		public Action<XsollaUtils> RecieveUtils;
		public Action<XsollaForm>  RecieveForm;
		public Action<XsollaPricepointsManager> RecievePricePoints;
		public Action<XsollaSubscriptions> RecieveSubscriptions;
		public Action<XsollaGoodsManager> RecieveGoods;
		public Action<XsollaGoodsManager> RecieveGoodsGrous;
		public Action<XsollaQuickPayments> RecieveQuickPayments;
		public Action<XsollaPaymentMethods> RecievePaymentsList;
		public Action<XsollaCountries> RecieveCountries;

		void Start (){
//			Debug.Log ("DataLoader Start");
//			Dictionary<string, object> requestParams = new Dictionary<string, object> ();
//			requestParams.Add ("access_token", "fk4lCZfA4CGR6yyUp4WUfekYrBJygVAZ");
//			requestParams.Add ("project", "7521");
//			requestParams.Add ("v1", "Ivan");
//			GetUtils (requestParams);
		}

		public void GetUtils(Dictionary<string, object> requestParams)
		{
			BaseWWWRequest request = RequestFactory.GetUtilsRequest (requestParams);
			request.ObjectsRecived += OnDataLoaded;
			StartCoroutine(request.Execute ());
		}

		public void OnDataLoaded(int type, object[] data)
		{
			switch (type) 
			{
				case RequestFactory.UTILS:
					break;
			}
		}
	}
}
