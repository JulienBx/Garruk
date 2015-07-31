using UnityEngine;

public class NewCollectionPointsPopUpView : MonoBehaviour
{
	
	public NewCollectionPointsPopUpViewModel cardCollectionPointsPopUpVM;
	public NewPopUpViewModel popUpVM;
	
	
	public NewCollectionPointsPopUpView ()
	{
		this.cardCollectionPointsPopUpVM = new NewCollectionPointsPopUpViewModel ();
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


