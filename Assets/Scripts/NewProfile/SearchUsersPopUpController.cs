using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class SearchUsersPopUpController : MonoBehaviour
{
	public static SearchUsersPopUpController instance;
	private SearchUsersPopUpModel model;

	public GameObject paginationButtonObject;

	private GameObject[] users;
	private GameObject[] paginationButtonsUsers;
	private IList<int> usersDisplayed;

	private int nbPagesUsers;
	private int nbPaginationButtonsLimitUsers;
	private int elementsPerPageUsers;
	private int chosenPageUsers;
	private int pageDebutUsers;
	private int activePaginationButtonIdUsers;

	public void launch(string search)
	{
		instance = this;
		this.model = new SearchUsersPopUpModel ();
		this.elementsPerPageUsers = 3;
		this.initializePopUp ();
		StartCoroutine (initialization (search));
	}
	private IEnumerator initialization(string search)
	{
		newMenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine (model.searchForUsers (search));
		if(model.users.Count>0)
		{
			this.initializeUsers ();
		}
		else
		{
			this.gameObject.transform.FindChild("NoResults").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("NoResults").GetComponent<TextMeshPro>().text="Désolé, aucun résultat ne correspond à votre recherche !";
		}
		newMenuController.instance.hideLoadingScreen ();
	}
	private void initializeUsers()
	{
		this.chosenPageUsers = 0;
		this.pageDebutUsers = 0 ;
		this.drawPaginationUsers();
		this.drawUsers ();
	}
	public void initializePopUp()
	{
		this.gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Résultats";
		this.gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Quitter";
		this.paginationButtonsUsers = new GameObject[0];
		this.users=new GameObject[3];
		for(int i=0;i<this.users.Length;i++)
		{
			this.users[i]=GameObject.Find ("User"+i);
			this.users[i].GetComponent<SearchUsersPopUpUserController>().setId(i);
			this.users[i].SetActive(false);
		}
	}
	public void drawUsers()
	{
		this.usersDisplayed = new List<int> ();
		for(int i =0;i<elementsPerPageUsers;i++)
		{
			if(this.chosenPageUsers*this.elementsPerPageUsers+i<model.users.Count)
			{
				this.usersDisplayed.Add (this.chosenPageUsers*this.elementsPerPageUsers+i);
				this.users[i].GetComponent<SearchUsersPopUpUserController>().u=model.users[this.chosenPageUsers*this.elementsPerPageUsers+i];
				this.users[i].GetComponent<SearchUsersPopUpUserController>().show();
				this.users[i].SetActive(true);
			}
			else
			{
				this.users[i].SetActive(false);
			}
		}
	}
	private void drawPaginationUsers()
	{
		for(int i=0;i<this.paginationButtonsUsers.Length;i++)
		{
			Destroy (this.paginationButtonsUsers[i]);
		}
		this.paginationButtonsUsers = new GameObject[0];
		this.activePaginationButtonIdUsers = -1;
		float paginationButtonWidth = 0.34f;
		float gapBetweenPaginationButton = 0.2f * paginationButtonWidth;
		this.nbPagesUsers = Mathf.CeilToInt((float)model.users.Count / ((float)this.elementsPerPageUsers));
		if(this.nbPagesUsers>1)
		{
			this.nbPaginationButtonsLimitUsers = Mathf.CeilToInt((2.4f)/(paginationButtonWidth+gapBetweenPaginationButton));
			int nbButtonsToDraw=0;
			bool drawBackButton=false;
			if (this.pageDebutUsers !=0)
			{
				drawBackButton=true;
			}
			bool drawNextButton=false;
			if (this.pageDebutUsers+nbPaginationButtonsLimitUsers-System.Convert.ToInt32(drawBackButton)<this.nbPagesUsers-1)
			{
				drawNextButton=true;
				nbButtonsToDraw=nbPaginationButtonsLimitUsers;
			}
			else
			{
				nbButtonsToDraw=this.nbPagesUsers-this.pageDebutUsers;
				if(drawBackButton)
				{
					nbButtonsToDraw++;
				}
			}
			this.paginationButtonsUsers = new GameObject[nbButtonsToDraw];
			for(int i =0;i<nbButtonsToDraw;i++)
			{
				this.paginationButtonsUsers[i] = Instantiate(this.paginationButtonObject) as GameObject;
				this.paginationButtonsUsers[i].transform.parent=this.gameObject.transform;
				this.paginationButtonsUsers[i].AddComponent<SearchUsersPopUpPaginationController>();
				this.paginationButtonsUsers[i].transform.localPosition=new Vector3((0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),-1f,0f);
				this.paginationButtonsUsers[i].name="PaginationUsers"+i.ToString();
			}
			for(int i=System.Convert.ToInt32(drawBackButton);i<nbButtonsToDraw-System.Convert.ToInt32(drawNextButton);i++)
			{
				this.paginationButtonsUsers[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=(this.pageDebutUsers+i-System.Convert.ToInt32(drawBackButton)).ToString();
				this.paginationButtonsUsers[i].GetComponent<SearchUsersPopUpPaginationController>().setId(i);
				if(this.pageDebutUsers+i-System.Convert.ToInt32(drawBackButton)==this.chosenPageUsers)
				{
					this.paginationButtonsUsers[i].GetComponent<SearchUsersPopUpPaginationController>().setActive(true);
					this.activePaginationButtonIdUsers=i;
				}
			}
			if(drawBackButton)
			{
				this.paginationButtonsUsers[0].GetComponent<SearchUsersPopUpPaginationController>().setId(-2);
				this.paginationButtonsUsers[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
			if(drawNextButton)
			{
				this.paginationButtonsUsers[nbButtonsToDraw-1].GetComponent<SearchUsersPopUpPaginationController>().setId(-1);
				this.paginationButtonsUsers[nbButtonsToDraw-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
		}
	}
	public void paginationHandlerUsers(int id)
	{
		if(id==-2)
		{
			this.pageDebutUsers=this.pageDebutUsers-this.nbPaginationButtonsLimitUsers+1+System.Convert.ToInt32(this.pageDebutUsers-this.nbPaginationButtonsLimitUsers+1!=0);
			this.drawPaginationUsers();
		}
		else if(id==-1)
		{
			this.pageDebutUsers=this.pageDebutUsers+this.nbPaginationButtonsLimitUsers-1-System.Convert.ToInt32(this.pageDebutUsers!=0);
			this.drawPaginationUsers();
		}
		else
		{
			if(activePaginationButtonIdUsers!=-1)
			{
				this.paginationButtonsUsers[this.activePaginationButtonIdUsers].GetComponent<SearchUsersPopUpPaginationController>().setActive(false);
			}
			this.activePaginationButtonIdUsers=id;
			this.chosenPageUsers=this.pageDebutUsers-System.Convert.ToInt32(this.pageDebutUsers!=0)+id;
			this.drawUsers();
		}
	}
}