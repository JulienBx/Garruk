using UnityEngine ;

public class MarketFeaturesView : MonoBehaviour
{
	public MarketFeaturesViewModel marketFeaturesVM;
	
	public MarketFeaturesView ()
	{
		this.marketFeaturesVM = new MarketFeaturesViewModel();
	}

	void OnGUI()
	{
		GUI.enabled = marketFeaturesVM.guiEnabled;
		GUILayout.BeginArea (marketFeaturesVM.rect);
		{
			if(marketFeaturesVM.price<ApplicationModel.credits)
			{
				GUILayout.Label (marketFeaturesVM.price+" credits",marketFeaturesVM.canBuyPriceStyle,GUILayout.Height(marketFeaturesVM.rect.height*3/10));
			}
			else
			{
				GUILayout.Label (marketFeaturesVM.price+" credits",marketFeaturesVM.cantBuyPriceStyle,GUILayout.Height(marketFeaturesVM.rect.height*3/10));
			}
			GUILayout.Space(marketFeaturesVM.rect.height*1/20);
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label ("Vendeur",marketFeaturesVM.titleStyle,
				                 GUILayout.Width(marketFeaturesVM.rect.width*2/5),
				                 GUILayout.Height(marketFeaturesVM.rect.height*3/10));
				if(GUILayout.Button(marketFeaturesVM.usernameOwner,marketFeaturesVM.ownerButtonStyle,
				                    GUILayout.Height(marketFeaturesVM.rect.height*3/10),
				                    GUILayout.Width(marketFeaturesVM.rect.width*3/5)))
				{
					ApplicationModel.profileChosen=marketFeaturesVM.usernameOwner;
					Application.LoadLevel("Profile");
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(marketFeaturesVM.rect.height*1/20);
			if(marketFeaturesVM.price<=ApplicationModel.credits && marketFeaturesVM.onSale)
			{
				if(GUILayout.Button ("Acheter",marketFeaturesVM.buyButtonStyle,GUILayout.Height(marketFeaturesVM.rect.height*3/10)))
				{
					gameObject.GetComponent<CardController> ().displayBuyCardPopUp();
				}
			}
		}
		GUILayout.EndArea();
	}
}