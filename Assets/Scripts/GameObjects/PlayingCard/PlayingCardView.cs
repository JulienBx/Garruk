using UnityEngine ;
using System.Collections;
using System.Collections.Generic;

public class PlayingCardView : MonoBehaviour
{
	public PlayingCardViewModel playingCardVM;
	
	public PlayingCardView()
	{
		this.playingCardVM = new PlayingCardViewModel();
	}

	public void replace()
	{
		gameObject.transform.localPosition = playingCardVM.position;
		gameObject.transform.localScale = playingCardVM.scale;
	}

	void Update()
	{
		int height = Screen.height;
		if (this.playingCardVM.toDisplayHalo)
		{		
			if (Input.GetMouseButtonDown(0))
			{
				if (Input.mousePosition.x > this.playingCardVM.haloRect.xMin && Input.mousePosition.x < this.playingCardVM.haloRect.xMax && (height - Input.mousePosition.y) > this.playingCardVM.haloRect.yMin && (height - Input.mousePosition.y) < this.playingCardVM.haloRect.yMax)
				{
					gameObject.GetComponentInChildren<PlayingCardController>().addTarget();
				}
			}
		} else
		{
			this.playingCardVM.toDisplayDescriptionIcon = -1;
			for (int i = 0; i < this.playingCardVM.icons.Count; i++)
			{
				if (Input.mousePosition.x > this.playingCardVM.iconsRect [i].xMin && Input.mousePosition.x < this.playingCardVM.iconsRect [i].xMax && (height - Input.mousePosition.y) > this.playingCardVM.iconsRect [i].yMin && (height - Input.mousePosition.y) < this.playingCardVM.iconsRect [i].yMax)
				{
					this.playingCardVM.toDisplayDescriptionIcon = i;
				} 
			}
		}
		
		this.playingCardVM.skillResultTimer -= Time.deltaTime;
		if (this.playingCardVM.skillResultTimer < 0)
		{
			this.playingCardVM.toDisplaySkillResult = false;
		}
	}
	
	public void show()
	{
		transform.renderer.materials [1].mainTexture = playingCardVM.face; 
		transform.Find("LifeArea").FindChild("Life").renderer.materials [0].mainTexture = playingCardVM.lifeGauge;
		transform.Find("MoveArea").FindChild("Move").GetComponent<TextMesh>().text = playingCardVM.move;
		transform.Find("AttackArea").FindChild("Attack").GetComponent<TextMesh>().text = playingCardVM.attack;
	}

	public void setTextResolution(float resolution)
	{
		transform.Find("MoveArea").FindChild("Move").GetComponent<TextMesh>().fontSize = Mathf.RoundToInt(resolution * 30);	
		transform.Find("MoveArea").FindChild("Move").localScale = new Vector3(0.3f / resolution, 0.3f / resolution, 0);
		transform.Find("AttackArea").FindChild("Attack").GetComponent<TextMesh>().fontSize = Mathf.RoundToInt(resolution * 30);	
		transform.Find("AttackArea").FindChild("Attack").localScale = new Vector3(0.3f / resolution, 0.3f / resolution, 0);
	}
	public void drawLifeGauge(float percentage)
	{
		Vector3 scale = new Vector3(0.95f * percentage, 0.6f, 1f);
		Vector3 position = new Vector3(0.5f * 0.95f * (percentage - 1f), 0, -0.01f);
		gameObject.transform.Find("LifeArea").FindChild("Life").localScale = scale;
		gameObject.transform.Find("LifeArea").FindChild("Life").localPosition = position;
	}

	public void changeBorder()
	{
		renderer.materials [0].mainTexture = this.playingCardVM.border;
	}
	
	public void changeBackground()
	{
		renderer.materials [1].mainTexture = this.playingCardVM.background;
	}
	
	void OnMouseEnter()
	{
		if (this.playingCardVM.isActive)
		{
			gameObject.GetComponentInChildren<PlayingCardController>().hoverPlayingCard();
		}
	}

	void OnMouseDown()
	{
		if (this.playingCardVM.isActive)
		{
			if (this.playingCardVM.toDisplayHalo)
			{
				gameObject.GetComponentInChildren<PlayingCardController>().addTarget();
			} else
			{
				gameObject.GetComponentInChildren<PlayingCardController>().clickPlayingCard();
			}
		}
	}

	void OnMouseUp()
	{
		if (this.playingCardVM.isActive)
		{
			if (this.playingCardVM.toDisplayHalo)
			{
			
			} else
			{
				gameObject.GetComponentInChildren<PlayingCardController>().releaseClickPlayingCard();
			}
		}
	}

	void OnGUI()
	{
		if (this.playingCardVM.icons.Count > 0)
		{
			for (int i = 0; i < this.playingCardVM.icons.Count; i++)
			{
				GUI.Box(this.playingCardVM.iconsRect [i], this.playingCardVM.icons [i], this.playingCardVM.iconStyle);
			}
			if (this.playingCardVM.toDisplayDescriptionIcon!=-1)
			{
				Rect newRect = new Rect(this.playingCardVM.iconsRect [this.playingCardVM.toDisplayDescriptionIcon].x, this.playingCardVM.iconsRect [this.playingCardVM.toDisplayDescriptionIcon].y - this.playingCardVM.iconsRect [this.playingCardVM.toDisplayDescriptionIcon].height * 3, this.playingCardVM.iconsRect [this.playingCardVM.toDisplayDescriptionIcon].width * 8, this.playingCardVM.iconsRect [this.playingCardVM.toDisplayDescriptionIcon].height * 3);
				
				GUILayout.BeginArea(newRect, this.playingCardVM.descriptionRectStyle);
				{
					GUILayout.BeginVertical();
					{
						GUILayout.Label(this.playingCardVM.titlesIcon [this.playingCardVM.toDisplayDescriptionIcon], this.playingCardVM.titleStyle);
						GUILayout.FlexibleSpace();
						GUILayout.Label(this.playingCardVM.descriptionIcon [this.playingCardVM.toDisplayDescriptionIcon], this.playingCardVM.descriptionStyle);
						GUILayout.FlexibleSpace();
						GUILayout.Label(this.playingCardVM.additionnalInfoIcon [this.playingCardVM.toDisplayDescriptionIcon], this.playingCardVM.additionnalInfoStyle);
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndArea();
			}
		}
		if (this.playingCardVM.toDisplayHalo)
		{
			GUI.Box(this.playingCardVM.haloRect, this.playingCardVM.halo, this.playingCardVM.iconStyle);
		}
		if (this.playingCardVM.toDisplaySkillResult)
		{
			GUI.Label(this.playingCardVM.haloRect, this.playingCardVM.skillResult.text, this.playingCardVM.skillResult.style);
		}
	}
}


