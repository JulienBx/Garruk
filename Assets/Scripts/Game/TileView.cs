using UnityEngine;

public class TileView : MonoBehaviour
{
	public TileViewModel tileVM;

	public TileView()
	{
		this.tileVM = new TileViewModel();
	}

	public void Update()
	{
		
		if (this.tileVM.isPotentialTarget)
		{
			if (Input.GetMouseButton(0))
			{
				int height = Screen.height;
				Vector3 scrPos = Utils.getGOScreenPosition(tileVM.position);
				Vector3 min = Utils.getGOScreenPosition(gameObject.renderer.bounds.min);
				Vector3 max = Utils.getGOScreenPosition(gameObject.renderer.bounds.max);

				if (Input.mousePosition.x > min.x && Input.mousePosition.x < max.x && Input.mousePosition.y > min.y && Input.mousePosition.y < max.y)
				{
					gameObject.GetComponentInChildren<TileController>().addTileTarget();
				}
			}
		}
	}

	public void resize()
	{
		gameObject.transform.localPosition = tileVM.position;
		gameObject.transform.localScale = tileVM.scale;
	}

	public void changeBorder()
	{
		renderer.materials [0].mainTexture = this.tileVM.border;
	}

	public void changeBackground()
	{
		renderer.materials [1].mainTexture = this.tileVM.background;
	}

	void OnMouseEnter()
	{
		gameObject.GetComponentInChildren<TileController>().hoverTile();
		GameController.instance.testX = tileVM.position.x;
		GameController.instance.testY = tileVM.position.y;

		GameController.instance.test2X = Input.mousePosition.x;
		GameController.instance.test2Y = Input.mousePosition.y;

		Vector3 min = Utils.getGOScreenPosition(gameObject.renderer.bounds.min);
		Vector3 max = Utils.getGOScreenPosition(gameObject.renderer.bounds.max);

		GameController.instance.test3X = min.x;
		GameController.instance.test3Y = min.y;
	}
	
	void OnMouseUp()
	{
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

	void OnGUI()
	{
		if (this.tileVM.toDisplayIcon)
		{
			GUI.Box(this.tileVM.iconRect, this.tileVM.icon, this.tileVM.iconStyle);
		}
	}
}

