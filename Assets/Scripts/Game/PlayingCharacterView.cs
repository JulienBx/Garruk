using UnityEngine ;

public class PlayingCharacterView : MonoBehaviour
{
	public PlayingCharacterViewModel playingCharacterVM;

	public PlayingCharacterView ()
	{
		this.playingCharacterVM = new PlayingCharacterViewModel();
	}

	public void replace ()
	{
		gameObject.transform.localPosition = playingCharacterVM.position;
		gameObject.transform.localRotation = playingCharacterVM.rotation;
		gameObject.transform.localScale = playingCharacterVM.scale;
	}

	void OnGUI ()
	{
		GUILayout.BeginArea (this.playingCharacterVM.infoRect);
		{
			GUILayout.BeginVertical(this.playingCharacterVM.backgroundStyle);
			{
				GUILayout.Label(this.playingCharacterVM.name, this.playingCharacterVM.nameTextStyle);
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndArea ();
	}

	void OnMouseEnter(){
		gameObject.GetComponentInChildren<PlayingCharacterController>().hoverCharacter();
	}
	
	void OnMouseExit(){
		//gameObject.GetComponentInChildren<PlayingCharacterController>().endHoverCharacter();
	}
	
	void OnMouseDown(){
		gameObject.GetComponentInChildren<PlayingCharacterController>().clickCharacter();
	}
	
	void OnMouseUp(){
		gameObject.GetComponentInChildren<PlayingCharacterController>().releaseClickCharacter();
	}
}


