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
				Vector3 min = Utils.getGOScreenPosition(gameObject.renderer.bounds.min);
				Vector3 max = Utils.getGOScreenPosition(gameObject.renderer.bounds.max);

				if (Input.mousePosition.x > min.x && Input.mousePosition.x < max.x && Input.mousePosition.y > min.y && Input.mousePosition.y < max.y)
				{
					//gameObject.GetComponentInChildren<TileController>().addTileTarget();
				}
			}
		}
		
		if (this.tileVM.toDisplayHalo)
		{		
			int height = Screen.height;
			
			if (Input.GetMouseButtonDown(0))
			{
				if (Input.mousePosition.x > this.tileVM.haloRect.xMin && Input.mousePosition.x < this.tileVM.haloRect.xMax && (height - Input.mousePosition.y) > this.tileVM.haloRect.yMin && (height - Input.mousePosition.y) < this.tileVM.haloRect.yMax)
				{
					//gameObject.GetComponentInChildren<TileController>().clickTarget();
				}
			} else if (Input.mousePosition.x > this.tileVM.haloRect.xMin && Input.mousePosition.x < this.tileVM.haloRect.xMax && (height - Input.mousePosition.y) > this.tileVM.haloRect.yMin && (height - Input.mousePosition.y) < this.tileVM.haloRect.yMax)
			{
				gameObject.GetComponentInChildren<TileController>().hoverTarget();
			}	
		}
	}

	public void resize()
	{
		gameObject.transform.localPosition = tileVM.position;
		gameObject.transform.localScale = tileVM.scale;
	}
	
	public void OnGUI()
	{
		if (this.tileVM.toDisplayIcon)
		{
			GUI.Box(this.tileVM.iconRect, this.tileVM.icon, this.tileVM.iconStyle);
		}
		
		if (this.tileVM.toDisplayHalo)
		{
			GUI.Box(this.tileVM.haloRect, this.tileVM.halo, this.tileVM.haloStyle);
		} else if (this.tileVM.toDisplayTrap)
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

