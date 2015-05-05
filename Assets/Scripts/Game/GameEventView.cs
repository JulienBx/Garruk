using UnityEngine;
using System.Collections;

public class GameEventView : MonoBehaviour
{
	public GameEventViewModel gameEventVM;
	bool isHovered = false;

	// Use this for initialization
	void Awake()
	{
		gameEventVM = new GameEventViewModel();
	}
	
	// Update is called once per frame
	void Update()
	{

	}
	void OnGUI()
	{
//		GUILayout.BeginArea(this.gameEventVM.infoRect);
//		{
//			GUILayout.BeginVertical(this.gameEventVM.backgroundStyle);
//			{
//				GUILayout.Label(this.gameEventVM.name, this.gameEventVM.nameTextStyle, GUILayout.Height(this.gameEventVM.infoRect.height * 12 / 100));
//			}
//		}
		if (isHovered)
		{
			GUILayout.BeginArea(this.gameEventVM.infoRect);
			{
				GUILayout.BeginVertical(this.gameEventVM.backgroundStyle);
				{
					GUILayout.Label(this.gameEventVM.characterName + " " + this.gameEventVM.action, this.gameEventVM.nameTextStyle, GUILayout.Height(this.gameEventVM.infoRect.height));
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
		
		}
	}

	void OnMouseOver()
	{
		isHovered = true;
	}
	void OnMouseExit()
	{
		isHovered = false;
	}

}
