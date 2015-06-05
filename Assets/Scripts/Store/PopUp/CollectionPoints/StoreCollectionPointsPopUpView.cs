using UnityEngine;

public class StoreCollectionPointsPopUpView : MonoBehaviour
{

	public StoreCollectionPointsPopUpViewModel storeCollectionPointsPopUpVM;
	
	public StoreCollectionPointsPopUpView ()
	{
		this.storeCollectionPointsPopUpVM = new StoreCollectionPointsPopUpViewModel ();
	}
	void OnGUI()
	{
		GUI.depth = storeCollectionPointsPopUpVM.guiDepth;
		GUILayout.BeginArea(storeCollectionPointsPopUpVM.centralWindow,storeCollectionPointsPopUpVM.centralWindowStyle);
		{
			GUILayout.FlexibleSpace();
			GUILayout.Label ("Collections Points : + "+storeCollectionPointsPopUpVM.collectionPoints,storeCollectionPointsPopUpVM.centralWindowTitleStyle);
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea();
	}
}


