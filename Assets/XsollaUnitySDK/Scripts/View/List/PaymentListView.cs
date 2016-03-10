using UnityEngine;
using System.Collections;
using Xsolla;
using SimpleJSON;
using System.Collections.Generic;

namespace Xsolla 
{
	public class PaymentListView : MonoBehaviour {

		XsollaPaymentMethods _paymentMethods;
		PaymentMethodsAdapter adapter;
		GridView gridView;

		// Use this for initialization
		public void SetPaymentMethods (XsollaPaymentMethods paymentMethods) {
			_paymentMethods = paymentMethods;
			if(adapter == null)
				adapter = GetComponent<PaymentMethodsAdapter>();// gameObject.AddComponent <PaymentMethodsAdapter>() as PaymentMethodsAdapter;
			adapter.SetManager (_paymentMethods);
			gridView = GetComponent<GridView> ();
			gridView.SetAdapter (adapter, 6);
		}

		public void Sort(string s)
		{
			List<XsollaPaymentMethod> arr = _paymentMethods.GetSortedItems (s);
			if (arr.Count > 0) {
				adapter.UpdateElements (arr);
				gridView.SetAdapter (adapter, 6);
			} else {
			
			}
		}

		// Update is called once per frame
		void Update () {
		
		}
	}
}
