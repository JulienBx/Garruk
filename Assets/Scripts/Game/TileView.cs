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

	public void changeBorder() 
	{
		renderer.materials[0].mainTexture = this.tileVM.border;
	}

	public void changeBackground() 
	{
		renderer.materials[1].mainTexture = this.tileVM.background;
	}

	void OnMouseEnter(){
		gameObject.GetComponentInChildren<TileController>().hoverTile();
	}
	
	void OnMouseUp(){
		gameObject.GetComponentInChildren<TileController>().releaseClickTile();
		//gameObject.GetComponentInChildren<TileController>().release();
		//		PlayingCharacterController pcc = gameObject.GetComponentInChildren<PlayingCharacterController>();
//		
//		pcc.release();
//		if (GameController.instance.onGoingAttack)
//		{
//			pcc.getDamage();
//			GameController.instance.setStateOfAttack(false);
//			//Debug.Log("total damage : " + pcc.damage);
//		}
	}
}

