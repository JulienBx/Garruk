using UnityEngine ;

public class FocusLobbyFeaturesView : MonoBehaviour
{
	public FocusLobbyFeaturesViewModel focusLobbyFeaturesVM;
	public CardFeaturesFocusViewModel cardFeaturesFocusVM;
	
	public FocusLobbyFeaturesView ()
	{
		this.focusLobbyFeaturesVM = new FocusLobbyFeaturesViewModel();
		this.cardFeaturesFocusVM = new CardFeaturesFocusViewModel ();
	}
	
	void OnGUI()
	{
		GUI.enabled = cardFeaturesFocusVM.guiEnabled;
		GUILayout.BeginArea (focusLobbyFeaturesVM.cardFeaturesFocusRects[0]);
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space (focusLobbyFeaturesVM.cardFeaturesFocusRects[0].width*1/20);
				if(focusLobbyFeaturesVM.cardLevel<10)
				{
					if(ApplicationModel.credits<focusLobbyFeaturesVM.nextLevelCost)
					{
						GUI.enabled=false;
					}
					if(GUILayout.Button("Passer au niveau suivant (-"+ focusLobbyFeaturesVM.nextLevelCost+" crédits)", cardFeaturesFocusVM.buttonStyle))
					{
						gameObject.GetComponent<CardController>().displayBuyXpCardPopUp();
					}
					GUI.enabled = cardFeaturesFocusVM.guiEnabled;
				}
				else
				{
					GUI.enabled=false;
					if(GUILayout.Button("La carte a atteint son niveau max", cardFeaturesFocusVM.buttonStyle))
					{
					}
					GUI.enabled = cardFeaturesFocusVM.guiEnabled;
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space (focusLobbyFeaturesVM.cardFeaturesFocusRects[0].height*1/20);
		}
		GUILayout.EndArea();
		GUILayout.BeginArea (focusLobbyFeaturesVM.cardFeaturesFocusRects[1]);
		{
			GUILayout.BeginHorizontal();
			{
				if(ApplicationModel.credits<focusLobbyFeaturesVM.renameCost)
				{
					GUI.enabled=false;
				}
				GUILayout.Space (focusLobbyFeaturesVM.cardFeaturesFocusRects[1].width*1/20);
				if(GUILayout.Button("Renommer la carte pour (-"+ focusLobbyFeaturesVM.renameCost+" crédits)", cardFeaturesFocusVM.buttonStyle))
				{
					gameObject.GetComponent<CardController>().displayRenameCardPopUp();
				}
				GUI.enabled = cardFeaturesFocusVM.guiEnabled;
			}
			GUILayout.EndHorizontal();
			GUILayout.Space (focusLobbyFeaturesVM.cardFeaturesFocusRects[1].height*1/20);
		}
		GUILayout.EndArea();
		GUILayout.BeginArea (focusLobbyFeaturesVM.cardFeaturesFocusRects[2]);
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space (focusLobbyFeaturesVM.cardFeaturesFocusRects[2].width*1/20);
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label("Victoires : "+focusLobbyFeaturesVM.nbWin,cardFeaturesFocusVM.statsStyle);
					GUILayout.Label("Défaites : "+focusLobbyFeaturesVM.nbLoose,cardFeaturesFocusVM.statsStyle);
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
			GUILayout.Space (focusLobbyFeaturesVM.cardFeaturesFocusRects[2].height*1/20);
		}
		GUILayout.EndArea();
		GUILayout.BeginArea (focusLobbyFeaturesVM.cardFeaturesFocusRects[3]);
		{
			GUILayout.Space (focusLobbyFeaturesVM.cardFeaturesFocusRects[3].height*1/20);
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space (focusLobbyFeaturesVM.cardFeaturesFocusRects[3].width*1/20);
				if(GUILayout.Button("Retour", cardFeaturesFocusVM.buttonStyle))
				{
					gameObject.GetComponent<CardLobbyController>().exitFocus();
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();
	}
}