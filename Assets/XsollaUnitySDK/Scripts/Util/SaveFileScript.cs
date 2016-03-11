using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using SimpleJSON;

namespace Xsolla {
	public class SaveFileScript : MonoBehaviour {

		void Start () {
			XsollaPaymentImpl Payment = gameObject.AddComponent <XsollaPaymentImpl>() as XsollaPaymentImpl;
			Dictionary<string, object> newDict = TransactionHelper.LoadRequest ();
			if (newDict != null) {
				Payment.GetStatus (newDict);
			} else {
				Debug.Log ("Have no Unfinished requests");
			}
		}

	}
}
