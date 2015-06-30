using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndSceneView : MonoBehaviour
{
	public EndSceneScreenViewModel screenVM;
	public EndSceneViewModel endSceneVM;

	public EndSceneView()
	{
		this.screenVM= new EndSceneScreenViewModel();
		this.endSceneVM = new EndSceneViewModel ();
	}

	public void OnGUI() 
	{
		GUILayout.BeginArea(screenVM.mainBlock);
		{
			GUILayout.Label (endSceneVM.title,endSceneVM.titleStyle);
			GUILayout.Space(screenVM.mainBlock.height*5/100);
			GUILayout.Label ("Vous gagnez : "+endSceneVM.creditsToAdd+" crédits ("+endSceneVM.credits+" credits)",endSceneVM.creditStyle);
			if(endSceneVM.collectionPoints>0)
			{
				GUILayout.Label("et "+endSceneVM.collectionPoints+" points de collection (classement : "+endSceneVM.collectionPointsRanking+")",endSceneVM.creditStyle);
			}
			GUILayout.FlexibleSpace();
			if(endSceneVM.newCardType!="")
			{
				GUILayout.Label("Vous avez débloqué la classe : "+endSceneVM.newCardType,endSceneVM.creditStyle);
			}
			if(endSceneVM.newSkills.Count>0)
			{
				GUILayout.Label("Vous débloquez :",endSceneVM.creditStyle);
				for(int i =0;i<endSceneVM.newSkills.Count;i++)
				{
					GUILayout.Label(endSceneVM.newSkills[i],endSceneVM.newSkillsStyle);
				}
			}
			GUILayout.Space(screenVM.mainBlock.height*5/100);
			GUI.enabled = endSceneVM.guiEnabled;
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(screenVM.mainBlock.width*3/10);
				if(GUILayout.Button("Quitter",endSceneVM.buttonStyle,GUILayout.Height(screenVM.mainBlock.height*10/100)))
				{
					EndSceneController.instance.quitEndScene();
				}
				GUILayout.Space(screenVM.mainBlock.width*3/10);
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(screenVM.mainBlock.height*5/100);
		}
		GUILayout.EndArea ();
	}
}
