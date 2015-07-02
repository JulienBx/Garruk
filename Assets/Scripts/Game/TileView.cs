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
				Vector3 min = Utils.getGOScreenPosition(gameObject.GetComponent<Renderer>().bounds.min);
				Vector3 max = Utils.getGOScreenPosition(gameObject.GetComponent<Renderer>().bounds.max);

				if (Input.mousePosition.x > min.x && Input.mousePosition.x < max.x && Input.mousePosition.y > min.y && Input.mousePosition.y < max.y)
				{
					//gameObject.GetComponentInChildren<TileController>().addTileTarget();
				}
			}
		}
		int height = Screen.height;
		
		if (this.tileVM.toDisplayHalo)
		{		
			
			if (Input.GetMouseButtonDown(0))
			{
				if (Input.mousePosition.x > this.tileVM.haloRect.xMin && Input.mousePosition.x < this.tileVM.haloRect.xMax && (height - Input.mousePosition.y) > this.tileVM.haloRect.yMin && (height - Input.mousePosition.y) < this.tileVM.haloRect.yMax)
				{
					if(!tileVM.isHaloDisabled)
					{
						gameObject.GetComponentInChildren<TileController>().addTileTarget();
					}
					else
					{
						gameObject.GetComponentInChildren<TileController>().releaseClickTile();
					}
				}
			} else if (Input.mousePosition.x > this.tileVM.haloRect.xMin && Input.mousePosition.x < this.tileVM.haloRect.xMax && (height - Input.mousePosition.y) > this.tileVM.haloRect.yMin && (height - Input.mousePosition.y) < this.tileVM.haloRect.yMax)
			{
				gameObject.GetComponentInChildren<TileController>().hoverTarget();
			}	
		}
		else{
			this.tileVM.toDisplayDescriptionIcon = false;
			if (Input.mousePosition.x > this.tileVM.iconRect.xMin && Input.mousePosition.x < this.tileVM.iconRect.xMax && (height - Input.mousePosition.y) > this.tileVM.iconRect.yMin && (height - Input.mousePosition.y) < this.tileVM.iconRect.yMax)
			{
				this.tileVM.toDisplayDescriptionIcon = true;
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
			if (this.tileVM.toDisplayDescriptionIcon)
			{
				Rect newRect = new Rect(this.tileVM.iconRect.x, this.tileVM.iconRect.y - this.tileVM.iconRect.height * 3, this.tileVM.iconRect.width * 8, this.tileVM.iconRect.height * 3);
				
				GUILayout.BeginArea(newRect, this.tileVM.descritionIconStyle);
				{
					GUILayout.BeginVertical();
					{
						GUILayout.FlexibleSpace();
						GUILayout.Label(this.tileVM.title, this.tileVM.titleStyle);
						GUILayout.FlexibleSpace();
						GUILayout.Label(this.tileVM.description, this.tileVM.descriptionStyle);
						GUILayout.FlexibleSpace();
						GUILayout.Label(this.tileVM.additionnalInfo, this.tileVM.additionnalInfoStyle);
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndArea();
			}
		}
		
		if (this.tileVM.toDisplayHalo)
		{
			GUILayout.BeginArea(this.tileVM.haloRect, this.tileVM.haloStyle);
			{
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					for (int i = 0 ; i < this.tileVM.haloTexts.Count ; i++){
						GUILayout.Label(this.tileVM.haloTexts[i], this.tileVM.haloStyles[i]);
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
		}
	}

	public void changeBorder()
	{
		GetComponent<Renderer>().materials [0].mainTexture = this.tileVM.border;
	}

	public void changeBackground()
	{
		GetComponent<Renderer>().materials [1].mainTexture = this.tileVM.background;
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

