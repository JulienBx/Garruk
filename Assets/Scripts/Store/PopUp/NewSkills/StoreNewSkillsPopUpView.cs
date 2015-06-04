using UnityEngine;

public class StoreNewSkillsPopUpView : MonoBehaviour
{
	
	public StoreCollectionPopUpViewModel popUpVM;
	public StoreNewSkillsPopUpViewModel storeNewSkillsPopUpVM;
	
	public StoreNewSkillsPopUpView ()
	{
		this.popUpVM = new StoreCollectionPopUpViewModel ();
		this.storeNewSkillsPopUpVM = new StoreNewSkillsPopUpViewModel ();
	}
	void OnGUI()
	{
		GUI.depth = popUpVM.guiDepth;
		GUILayout.BeginArea(popUpVM.centralWindow);
		{
			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
			{
				GUILayout.Space(30);
				GUILayout.Label (storeNewSkillsPopUpVM.title,popUpVM.centralWindowTitleStyle);
				for(int i=0;i<storeNewSkillsPopUpVM.skills.Count;i++)
				{
					GUILayout.Label (storeNewSkillsPopUpVM.skills[i],popUpVM.centralWindowTitleStyle);
				}
				GUILayout.Space(30);
			}
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea();
	}
}


