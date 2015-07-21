using UnityEngine;

public class NewSkillsPopUpView : MonoBehaviour
{
	
	public NewSkillsPopUpViewModel cardNewSkillsPopUpVM;
	public NewPopUpViewModel popUpVM;
	
	public NewSkillsPopUpView ()
	{
		this.cardNewSkillsPopUpVM = new NewSkillsPopUpViewModel ();
		this.popUpVM = new NewPopUpViewModel ();
	}
	void OnGUI()
	{
		
		GUILayout.BeginArea(popUpVM.centralWindow);
		{
			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
			{
				GUILayout.Space(30);
				GUILayout.Label (cardNewSkillsPopUpVM.title,popUpVM.centralWindowTitleStyle);
				for(int i=0;i<cardNewSkillsPopUpVM.skills.Count;i++)
				{
					GUILayout.Label (cardNewSkillsPopUpVM.skills[i],popUpVM.centralWindowTitleStyle);
				}
				GUILayout.Space(30);
			}
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea();
	}
}


