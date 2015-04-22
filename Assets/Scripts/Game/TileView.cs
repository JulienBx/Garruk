using UnityEngine;

public class TileView : MonoBehaviour
{
	public TileViewModel tileVM;

	public TileView ()
	{
		this.tileVM = new TileViewModel();
	}

	public void resize() 
	{
		gameObject.transform.localPosition = tileVM.position;
		gameObject.transform.localScale = tileVM.scale;
	}

	public void ShowFace() 
	{
		renderer.materials[1].mainTexture = this.tileVM.background;
	}

	void OnMouseEnter(){
		gameObject.GetComponentInChildren<TileController>().mouseEnter();
	}
}

