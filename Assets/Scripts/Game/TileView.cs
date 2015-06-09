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
				Vector3 scrScale = Utils.getGOScreenPosition(tileVM.scale);
				if (Input.mousePosition.x > (scrPos.x - scrScale.x / 2f) && Input.mousePosition.x < (scrPos.x + scrScale.x / 2f) && (height - Input.mousePosition.y) > (scrPos.y - scrScale.y / 2f) && (height - Input.mousePosition.y) < scrPos.y + scrScale.y / 2f)
				{
					gameObject.GetComponentInChildren<TileController>().addTileTarget();
				}
			}
		}
		
		if (this.tileVM.toDisplayHalo)
		{		
			if (Input.GetMouseButtonDown(0))
			{
				int height = Screen.height;
				if (Input.mousePosition.x > this.tileVM.haloRect.xMin && Input.mousePosition.x < this.tileVM.haloRect.xMax && (height - Input.mousePosition.y) > this.tileVM.haloRect.yMin && (height - Input.mousePosition.y) < this.tileVM.haloRect.yMax)
				{
					gameObject.GetComponentInChildren<TileController>().clickTarget();
				}
			}
		}
	}

	public void resize()
	{
		gameObject.transform.localPosition = tileVM.position;
		gameObject.transform.localScale = tileVM.scale;
	}
	
	public void OnGUI(){
		if (this.tileVM.toDisplayHalo)
		{
			GUI.Box(this.tileVM.haloRect, this.tileVM.halo, this.tileVM.haloStyle);
		}
		else if (this.tileVM.toDisplayTrap)
		{
			GUI.Box(this.tileVM.haloRect, this.tileVM.trap, this.tileVM.trapStyle);
		}
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
	}
	
	void OnMouseUp()
	{
		gameObject.GetComponentInChildren<TileController>().releaseClickTile();
	}
}

