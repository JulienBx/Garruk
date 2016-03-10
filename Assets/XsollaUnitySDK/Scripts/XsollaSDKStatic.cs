using UnityEngine;
using System.Collections;
using System;

namespace Xsolla {

	public static class XsollaSDKStatic {
	
		public static string InitPaystation(string token)
		{
			var url = "https://secure.xsolla.com/paystation2/?access_token=" + token;
			Application.OpenURL (url);
			return url;
		}

		public static void CreatePaymentForm(string data, bool isSandbox)
		{
			XsollaPaystationController formController = GetPaystationController ();
			formController.OkHandler 	+= (status) => {Debug.Log("OkHandler 1 " + status);};
			formController.ErrorHandler += (error) => {Debug.Log("ErrorHandler 2 " + error);};
			formController.OpenPaystation (data, isSandbox);
		}
		
		public static void CreatePaymentForm(string data, Action<XsollaResult> actionOk, Action<XsollaError> actionError, bool isSandbox)
		{
			XsollaPaystationController formController = GetPaystationController();
			formController.OkHandler += actionOk;
			formController.ErrorHandler += actionError;
			formController.OpenPaystation (data, isSandbox);
		}

		public static void CreatePaymentForm(XsollaJsonGenerator generator, Action<XsollaResult> actionOk, Action<XsollaError> actionError, bool isSandbox)
		{
			XsollaPaystationController formController = GetPaystationController();
			formController.OkHandler += actionOk;
			formController.ErrorHandler += actionError;
			formController.OpenPaystation (generator.GetPrepared(), isSandbox);
		}
		
		public static void DirectPayment(CCPayment payment)
		{
			payment.InitPaystation ();
		}

		
		private static XsollaPaystationController GetPaystationController() {
			GameObject paystationobject = MonoBehaviour.Instantiate(Resources.Load("Prefabs/XsollaPaystation")) as GameObject;
			XsollaPaystationController formController = paystationobject.GetComponent<XsollaPaystationController> ();
			return formController;
		}

	}

}
