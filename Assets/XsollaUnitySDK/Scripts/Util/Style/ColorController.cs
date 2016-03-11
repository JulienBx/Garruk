using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace Xsolla {
	public class ColorController : MonoBehaviour {
		
		public Elem[] itemsToColor;

		public void ChangeColor(int elemNo, StyleManager.BaseColor color){
			itemsToColor [elemNo].color = color;
			foreach (Elem e in itemsToColor) {
				if(e.whatToColor != null)
					e.whatToColor.color = StyleManager.Instance.GetColor (e.color);
			}
		}

		// Use this for initialization
		void Start () {
			foreach (Elem e in itemsToColor) {
				if(e.whatToColor != null)
					e.whatToColor.color = StyleManager.Instance.GetColor (e.color);
			}
		}
		
		// Update is called once per frame
		void Update () {
		
		}

	}

	[Serializable]
	public struct Elem {
		public StyleManager.BaseColor color;
		public Graphic whatToColor;
	}
}
