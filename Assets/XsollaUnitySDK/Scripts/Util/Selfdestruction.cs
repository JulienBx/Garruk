using UnityEngine;
using System.Collections;
using Xsolla;

public class Selfdestruction : MonoBehaviour {

	public void DestroyRoot(){
		XsollaPaystationController controller = gameObject.GetComponentInParent<XsollaPaystationController> ();
		Destroy (controller.gameObject);
		TransactionHelper.Clear ();
	}

	public void Selfdestroy(){
		Destroy (gameObject);
	}

	public void DestroyObject(GameObject go){
		Destroy (go);
	}


}
