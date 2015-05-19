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
			GUILayout.Label ("Gains de combats",endSceneVM.titleStyle);
			GUILayout.Label ("Crédits :"+endSceneVM.credits,endSceneVM.creditStyle);
			if(GUILayout.Button("Quitter",endSceneVM.buttonStyle))
			{
				EndSceneController.instance.quitEndScene();
			}
		}
		GUILayout.EndArea ();
	}
}
