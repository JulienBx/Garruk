using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class ExternalColliderController : MonoBehaviour
{	
	public void OnMouseEnter(){
		GameView.instance.hitExternalCollider();
	}
}


