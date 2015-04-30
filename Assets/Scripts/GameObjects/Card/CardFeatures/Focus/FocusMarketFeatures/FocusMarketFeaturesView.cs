using UnityEngine ;

public class FocusMarketFeaturesView : MonoBehaviour
{
	public FocusMarketFeaturesViewModel focusMarketFeaturesVM;
	public CardFeaturesFocusViewModel cardFeaturesFocusVM;
	
	public FocusMarketFeaturesView ()
	{
		this.focusMarketFeaturesVM = new FocusMarketFeaturesViewModel();
		this.cardFeaturesFocusVM = new CardFeaturesFocusViewModel ();
	}
	
	void OnGUI()
	{
		GUI.enabled = cardFeaturesFocusVM.guiEnabled;
		GUILayout.BeginArea (focusMarketFeaturesVM.cardFeaturesFocusRects[0]);
		{
			GUILayout.BeginHorizontal();
			{
				if(ApplicationModel.credits<focusMarketFeaturesVM.price || !focusMarketFeaturesVM.onSale)
				{
					GUI.enabled=false;
				}
				GUILayout.Space (focusMarketFeaturesVM.cardFeaturesFocusRects[0].width*1/20);
				if(GUILayout.Button("Acheter (-"+ focusMarketFeaturesVM.price+" crédits)", cardFeaturesFocusVM.buttonStyle))
				{
					gameObject.GetComponent<CardController>().displayBuyCardPopUp();
				}
				GUI.enabled = cardFeaturesFocusVM.guiEnabled;
			}
			GUILayout.EndHorizontal();
			GUILayout.Space (focusMarketFeaturesVM.cardFeaturesFocusRects[0].height*1/20);
		}
		GUILayout.EndArea();
		GUILayout.BeginArea (focusMarketFeaturesVM.cardFeaturesFocusRects[1]);
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space (focusMarketFeaturesVM.cardFeaturesFocusRects[0].width*1/20);
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label("Victoires : "+focusMarketFeaturesVM.nbWin,cardFeaturesFocusVM.statsStyle);
					GUILayout.Label("Défaites : "+focusMarketFeaturesVM.nbLoose,cardFeaturesFocusVM.statsStyle);
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
			GUILayout.Space (focusMarketFeaturesVM.cardFeaturesFocusRects[1].height*1/20);
		}
		GUILayout.EndArea();
		GUILayout.BeginArea (focusMarketFeaturesVM.cardFeaturesFocusRects[2]);
		{
			GUILayout.Space (focusMarketFeaturesVM.cardFeaturesFocusRects[2].height*1/20);
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space (focusMarketFeaturesVM.cardFeaturesFocusRects[0].width*1/20);
				if(GUILayout.Button("Retour", cardFeaturesFocusVM.buttonStyle))
				{
					gameObject.GetComponent<CardMarketController>().exitFocus();
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();
	}
}