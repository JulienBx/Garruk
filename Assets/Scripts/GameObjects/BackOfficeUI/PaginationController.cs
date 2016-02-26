using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class PaginationController : MonoBehaviour 
{	
	public Pagination p;
	public GameObject[] buttons;

	public void initialize()
	{
		buttons=new GameObject[2];
		for (int i=0;i<buttons.Length;i++)
		{
			this.buttons[i]=gameObject.transform.FindChild("Button"+i).gameObject;
			this.buttons[i].AddComponent<PaginationButtonController>();
			this.buttons[i].GetComponent<PaginationButtonController>().setId(i);
		}
	}
	public virtual void resize()
	{
		for (int i=0;i<buttons.Length;i++)
		{
			this.buttons[i].transform.localScale=ApplicationDesignRules.paginationButtonScale;
		}
		this.buttons[0].transform.localPosition=new Vector3 (-ApplicationDesignRules.paginationButtonWorldSize.x / 2f, 0f, 0f);
		this.buttons[1].transform.localPosition=new Vector3 (ApplicationDesignRules.paginationButtonWorldSize.x / 2f, 0f, 0f);
	}
	public void setVisible(bool value)
	{
		for (int i=0;i<buttons.Length;i++)
		{
			this.buttons[i].SetActive(value);
		}
	}
	public void setPagination()
	{
		this.p.elementDebut = 0;
		this.p.elementFin = 0;
		this.p.nbPages=Mathf.CeilToInt((float) p.totalElements / ((float)p.nbElementsPerPage));
		if(this.p.chosenPage>this.p.nbPages-1 && this.p.chosenPage>0)
		{
			this.p.chosenPage=this.p.nbPages-1;
		}
		if(this.p.chosenPage==0)
		{
			this.p.elementDebut=1;
			this.buttons[0].GetComponent<PaginationButtonController>().reset();
			this.buttons[0].SetActive(false);
			if(this.p.nbPages>1)
			{
				this.buttons[1].SetActive(true);
				p.elementFin=p.nbElementsPerPage;
			}
			else
			{
				this.buttons[1].GetComponent<PaginationButtonController>().reset ();
				this.buttons[1].SetActive(false);
				if(this.p.nbPages!=0)
				{
					this.p.elementFin=this.p.totalElements;
				}
			}
		}
		else
		{
			this.p.elementDebut=this.p.chosenPage*this.p.nbElementsPerPage+1;
			this.buttons[0].SetActive(true);
			if(this.p.chosenPage!=this.p.nbPages-1)
			{
				this.p.elementFin=this.p.elementDebut+this.p.nbElementsPerPage-1;
				this.buttons[1].SetActive(true);
			}
			else
			{
				this.p.elementFin=this.p.totalElements;
				this.buttons[1].GetComponent<PaginationButtonController>().reset ();
				this.buttons[1].SetActive(false);
			}
		}
	}
	public virtual void paginationHandler(int id)
	{
		if(id==0)
		{
			this.p.chosenPage--;
		}
		else
		{
			this.p.chosenPage++;
		}
		this.setPagination ();
	}
}

