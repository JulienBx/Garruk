using UnityEngine;

public class NewFocusedCardCollectionPointsPopUpView : MonoBehaviour
{
	
	public NewFocusedCardCollectionPointsPopUpViewModel cardCollectionPointsPopUpVM;
	public NewPopUpViewModel popUpVM;

	
	public NewFocusedCardCollectionPointsPopUpView ()
	{
		this.cardCollectionPointsPopUpVM = new NewFocusedCardCollectionPointsPopUpViewModel ();
		this.popUpVM = new NewPopUpViewModel ();
	}
	void OnGUI()
	{
		GUILayout.BeginArea(popUpVM.centralWindow,popUpVM.centralWindowStyle);
		{
			GUILayout.FlexibleSpace();
			GUILayout.Label ("Collections Points : + "+cardCollectionPointsPopUpVM.collectionPoints,popUpVM.centralWindowTitleStyle);
			GUILayout.Label ("(classement : "+cardCollectionPointsPopUpVM.collectionPointsRanking+")",popUpVM.centralWindowTitleStyle);
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea();
	}
}


