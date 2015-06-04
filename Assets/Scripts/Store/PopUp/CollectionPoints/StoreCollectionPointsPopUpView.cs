using UnityEngine;

public class StoreCollectionPointsPopUpView : MonoBehaviour
{
	
	public StoreCollectionPopUpViewModel popUpVM;
	public StoreCollectionPointsPopUpViewModel storeCollectionPointsPopUpVM;
	
	public StoreCollectionPointsPopUpView ()
	{
		this.popUpVM = new StoreCollectionPopUpViewModel ();
		this.storeCollectionPointsPopUpVM = new StoreCollectionPointsPopUpViewModel ();
	}
	void OnGUI()
	{
		GUI.depth = popUpVM.guiDepth;
		GUILayout.BeginArea(popUpVM.centralWindow,popUpVM.centralWindowStyle);
		{
			GUILayout.FlexibleSpace();
			GUILayout.Label ("Collections Points : + "+storeCollectionPointsPopUpVM.collectionPoints,popUpVM.centralWindowTitleStyle);
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea();
	}
}


