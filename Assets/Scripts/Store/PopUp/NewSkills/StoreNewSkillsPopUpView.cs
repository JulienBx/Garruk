using UnityEngine;

public class StoreNewSkillsPopUpView : MonoBehaviour
{

	public StoreNewSkillsPopUpViewModel storeNewSkillsPopUpVM;
	
	public StoreNewSkillsPopUpView ()
	{
		this.storeNewSkillsPopUpVM = new StoreNewSkillsPopUpViewModel ();
	}
	void OnGUI()
	{
		GUI.depth = storeNewSkillsPopUpVM.guiDepth;
		GUILayout.BeginArea(storeNewSkillsPopUpVM.centralWindow);
		{
			GUILayout.BeginVertical(storeNewSkillsPopUpVM.centralWindowStyle);
			{
				GUILayout.Space(30);
				GUILayout.Label (storeNewSkillsPopUpVM.title,storeNewSkillsPopUpVM.centralWindowTitleStyle);
				for(int i=0;i<storeNewSkillsPopUpVM.skills.Count;i++)
				{
					GUILayout.Label (storeNewSkillsPopUpVM.skills[i],storeNewSkillsPopUpVM.centralWindowTitleStyle);
				}
				GUILayout.Space(30);
			}
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea();
	}
}


