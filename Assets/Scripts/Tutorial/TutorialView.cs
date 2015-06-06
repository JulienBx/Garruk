using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialView : MonoBehaviour
{
	public TutorialScreenViewModel screenVM;
	public TutorialViewModel tutorialVM;
	
	public TutorialView()
	{
		this.screenVM= new TutorialScreenViewModel();
		this.tutorialVM = new TutorialViewModel ();
	}
	void Update()
	{
		if (Screen.width != screenVM.widthScreen || Screen.height != screenVM.heightScreen) 
		{
			TutorialController.instance.resize();
		}
	}
	
	void OnGUI()
	{
		GUI.enabled = tutorialVM.guiEnabled;
		GUILayout.BeginArea (screenVM.mainBlock,tutorialVM.blockBakgroundStyle);
		{
			GUILayout.Space(screenVM.mainBlock.height*0.05f);
			GUILayout.Label(tutorialVM.title,tutorialVM.titleStyle);
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical(GUILayout.Width(screenVM.mainBlock.height*1.2f));
				{
					GUILayout.Label("",tutorialVM.mainPictureStyle,GUILayout.Height(screenVM.mainBlock.height*0.6f));
					GUILayout.FlexibleSpace();
					GUILayout.Label(tutorialVM.description,tutorialVM.descriptionStyle);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						if(tutorialVM.selectedPage>0)
						{
							if(GUILayout.Button("Retour",tutorialVM.buttonStyle,GUILayout.Height(screenVM.mainBlock.height*0.05f),GUILayout.Width(screenVM.mainBlock.width*0.2f)))
							{
								TutorialController.instance.displayPrecedentPage();
							}
							GUILayout.FlexibleSpace();
						}
						else
						{
							GUILayout.FlexibleSpace();
						}
						if(GUILayout.Button("Continuer",tutorialVM.buttonStyle,GUILayout.Height(screenVM.mainBlock.height*0.05f),GUILayout.Width(screenVM.mainBlock.width*0.2f)))
						{
							TutorialController.instance.displayNextPage();
						}
						if(tutorialVM.selectedPage==0)
						{
							GUILayout.FlexibleSpace();
						}
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(screenVM.mainBlock.height*0.075f);
		}
		GUILayout.EndArea ();
	}
}
