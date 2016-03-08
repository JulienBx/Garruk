using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class NewPackController : MonoBehaviour 
{

	private NewPackRessources ressources;

	public Pack p;

	private int Id;
	private bool isClickable;

	private Rect centralWindow;
	private Rect collectionPointsWindow;
	private Rect newSkillsWindow;

	public void initialize()
	{
		this.gameObject.transform.FindChild("PackTitle").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.gameObject.transform.FindChild("PackPicture").GetComponent<SpriteRenderer>().color = ApplicationDesignRules.whiteSpriteColor;
	}
	public void resize()
	{
		this.gameObject.transform.FindChild("PackTexture").transform.localScale = ApplicationDesignRules.packScale;
		this.gameObject.transform.FindChild("PackPicture").transform.localScale = ApplicationDesignRules.packPictureScale;
		this.gameObject.transform.FindChild("PackPicture").transform.position = new Vector3 (this.gameObject.transform.position.x + ApplicationDesignRules.packWorldSize.x / 2f - ApplicationDesignRules.packPictureWorldSize.x / 2f, this.gameObject.transform.position.y-0.05f, 0f);
		this.gameObject.transform.FindChild("PackButton").localScale = ApplicationDesignRules.button62Scale;
		this.gameObject.transform.FindChild("PackButton").transform.position  = new Vector3 (this.gameObject.transform.position.x - ApplicationDesignRules.packWorldSize.x / 2f + ApplicationDesignRules.button62WorldSize.x / 2f + 0.15f, this.gameObject.transform.position.y - ApplicationDesignRules.packWorldSize.y / 2f + ApplicationDesignRules.button62WorldSize.y / 2f +0.15f, 0f);
		this.gameObject.transform.FindChild("PackTitle").transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.gameObject.transform.FindChild("PackTitle").transform.position = new Vector3 (this.gameObject.transform.position.x - ApplicationDesignRules.packWorldSize.x / 2f +0.15f, this.gameObject.transform.position.y + ApplicationDesignRules.packWorldSize.y / 2f -0.4f, 0f);
		if(ApplicationDesignRules.isMobileScreen)
		{
			this.gameObject.transform.FindChild("PackTitle").GetComponent<TextMeshPro> ().textContainer.width = ApplicationDesignRules.packWorldSize.x/1.3f;
		}
		else
		{
			this.gameObject.transform.FindChild("PackTitle").GetComponent<TextMeshPro> ().textContainer.width = ApplicationDesignRules.packWorldSize.x/2.1f;
		}
	}
	public void show(Pack p)
	{
		this.gameObject.transform.FindChild("PackButton").FindChild("Title").GetComponent<TextMeshPro> ().text = WordingPacks.getReferences(0)+p.Price.ToString ();
		this.gameObject.transform.FindChild("PackPicture").GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnPackPicture (p.IdPicture);
		this.gameObject.transform.FindChild("PackTitle").GetComponent<TextMeshPro> ().text = p.Name;
	}
	public void activeButton(bool value)
	{
		this.gameObject.transform.FindChild ("PackButton").GetComponent<NewPackButtonController> ().setIsActive (value);
		if(value)
		{
			this.gameObject.transform.FindChild ("PackButton").GetComponent<NewPackButtonController> ().setInitialState ();
		}
		else
		{
			this.gameObject.transform.FindChild ("PackButton").GetComponent<NewPackButtonController> ().setForbiddenState();
		}
	}
	public virtual void buyPackHandler()
	{
	}
	public virtual void buttonHovered(bool value)
	{
	}
	public void setId(int id)
	{
		this.Id = id;
	}
	public int getId()
	{
		return this.Id;
	}
	public Vector3 getBuyButtonPosition()
	{
		return this.gameObject.transform.FindChild ("PackButton").position;
	}
}

