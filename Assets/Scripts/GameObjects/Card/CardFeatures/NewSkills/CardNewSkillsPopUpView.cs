using UnityEngine;

public class CardNewSkillsPopUpView : MonoBehaviour
{
	
	public CardNewSkillsPopUpViewModel cardNewSkillsPopUpVM;
	
	public CardNewSkillsPopUpView ()
	{
		this.cardNewSkillsPopUpVM = new CardNewSkillsPopUpViewModel ();
	}
	void OnGUI()
	{
		GUI.depth = cardNewSkillsPopUpVM.guiDepth;
		GUILayout.BeginArea(cardNewSkillsPopUpVM.centralWindow);
		{
			GUILayout.BeginVertical(cardNewSkillsPopUpVM.centralWindowStyle);
			{
				GUILayout.Space(30);
				GUILayout.Label (cardNewSkillsPopUpVM.title,cardNewSkillsPopUpVM.centralWindowTitleStyle);
				for(int i=0;i<cardNewSkillsPopUpVM.skills.Count;i++)
				{
					GUILayout.Label (cardNewSkillsPopUpVM.skills[i],cardNewSkillsPopUpVM.centralWindowTitleStyle);
				}
				GUILayout.Space(30);
			}
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea();
	}
}


