using UnityEngine ;

public class CardGameView : MonoBehaviour
{	

	public CardGameViewModel cardGameVM;

	public CardGameView ()
	{
		this.cardGameVM = new CardGameViewModel ();
	}
	
	void OnGUI()
	{
		GUILayout.BeginArea (cardGameVM.nextLevelRect);
		{
			GUILayout.Label(cardGameVM.nextLevel,cardGameVM.nextLevelStyle);
		}
		GUILayout.EndArea();
	}
}