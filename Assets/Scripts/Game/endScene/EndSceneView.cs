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
			GUILayout.Space(screenVM.mainBlock.height*1/10);
			GUILayout.Label ("Vous gagnez : "+endSceneVM.creditsToAdd+" crédits ("+endSceneVM.credits+" credits)",endSceneVM.creditStyle);
			GUILayout.Space(screenVM.mainBlock.height*5/10);
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
		}
		GUILayout.EndArea ();
	}
}
