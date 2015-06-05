using UnityEngine;

public class CardCollectionPointsPopUpView : MonoBehaviour
{
	
	public CardCollectionPointsPopUpViewModel cardCollectionPointsPopUpVM;
	
	public CardCollectionPointsPopUpView ()
	{
		this.cardCollectionPointsPopUpVM = new CardCollectionPointsPopUpViewModel ();
	}
	void OnGUI()
	{
		GUI.depth = cardCollectionPointsPopUpVM.guiDepth;
		GUILayout.BeginArea(cardCollectionPointsPopUpVM.centralWindow,cardCollectionPointsPopUpVM.centralWindowStyle);
		{
			GUILayout.FlexibleSpace();
			GUILayout.Label ("Collections Points : + "+cardCollectionPointsPopUpVM.collectionPoints,cardCollectionPointsPopUpVM.centralWindowTitleStyle);
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea();
	}
}


