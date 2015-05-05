using System;
using UnityEngine;

public class StoreView : MonoBehaviour
{

	public StoreViewModel storeVM;
	public StoreScreenViewModel storeScreenVM;

	public StoreView ()
	{
		this.storeVM = new StoreViewModel ();
		this.storeScreenVM = new StoreScreenViewModel ();
	}
	void Update()
	{
		if (Screen.width != storeScreenVM.widthScreen || Screen.height != storeScreenVM.heightScreen) {
			StoreController.instance.resize();
		}
		if(Input.GetKeyDown(KeyCode.Return)) 
		{
			StoreController.instance.returnPressed();
		}
		if(Input.GetKeyDown(KeyCode.Escape)) 
		{
			StoreController.instance.escapePressed();
		}
	}
	void OnGUI()
	{
		GUI.enabled=storeVM.guiEnabled;
		if(!this.storeVM.isCardZoomed)
		{
			GUILayout.BeginArea(new Rect(storeScreenVM.blockMain.min.x,storeScreenVM.blockMain.min.y,storeScreenVM.blockMainWidth*1/10,storeScreenVM.blockMainHeight*15/100));
			{
				if (GUILayout.Button("Créer une nouvelle carte - "+ storeVM.creationCost+" crédits",storeVM.buttonStyle))
				{
					StartCoroutine(StoreController.instance.createRandomCard());
				}
			}
			GUILayout.EndArea();
		}
	}
	
}
