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

	private GameObject[] users;
	private GameObject paginationButtons;
	private IList<int> usersDisplayed;
	
	private Pagination pagination;

	public void launch()
	{
		instance = this;
		this.model = new SearchUsersPopUpModel ();
		this.pagination = new Pagination ();
		this.pagination.nbElementsPerPage= 3;
		this.initializePopUp ();
	}
	public void resize()
	{
		this.paginationButtons.GetComponent<SearchUsersPopUpPaginationController> ().resize ();
	}
	public IEnumerator initialization(string search)
	{
		BackOfficeController.instance.displayLoadingScreen ();
		this.gameObject.transform.FindChild ("Button").GetComponent<SearchUsersPopUpButtonController> ().reset ();
		this.gameObject.transform.FindChild ("CloseButton").GetComponent<SearchUsersPopUpCloseButtonController> ().reset ();
		yield return StartCoroutine (model.searchForUsers (search));
		this.initializeUsers ();
		if(model.users.Count==0)
		{
			this.gameObject.transform.FindChild("NoResults").gameObject.SetActive(true);
		}
		else
		{
			this.gameObject.transform.FindChild("NoResults").gameObject.SetActive(false);
		}
		BackOfficeController.instance.hideLoadingScreen ();
	}
	private void initializeUsers()
	{
		this.pagination.chosenPage = 0;
		this.pagination.totalElements= model.users.Count;
		this.paginationButtons.GetComponent<SearchUsersPopUpPaginationController> ().p = pagination;
		this.paginationButtons.GetComponent<SearchUsersPopUpPaginationController> ().setPagination ();
		this.drawUsers ();
	}
	public void paginationHandler()
	{
		this.drawUsers ();
	}
	public void initializePopUp()
	{
		this.gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingSearchUsersPopUp.getReference(0);
		this.gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingSearchUsersPopUp.getReference(1);
		this.gameObject.transform.FindChild("NoResults").GetComponent<TextMeshPro>().text=WordingSearchUsersPopUp.getReference(2);
		this.paginationButtons = GameObject.Find ("Pagination");
		this.paginationButtons.AddComponent<SearchUsersPopUpPaginationController> ();
		this.paginationButtons.GetComponent<SearchUsersPopUpPaginationController> ().initialize ();
		this.users=new GameObject[3];
		for(int i=0;i<this.users.Length;i++)
		{
			this.users[i]=GameObject.Find ("User"+i);
			this.users[i].GetComponent<SearchUsersPopUpUserController>().setId(i);
			this.users[i].SetActive(false);
		}
	}
	public void exitPopUp()
	{
		NewProfileController.instance.hideSearchUsersPopUp ();
	}
	public void drawUsers()
	{
		this.usersDisplayed = new List<int> ();
		for(int i =0;i<pagination.nbElementsPerPage;i++)
		{
			if(pagination.chosenPage*pagination.nbElementsPerPage+i<model.users.Count)
			{
				this.usersDisplayed.Add (pagination.chosenPage*pagination.nbElementsPerPage+i);
				this.users[i].GetComponent<SearchUsersPopUpUserController>().u=model.users[pagination.chosenPage*pagination.nbElementsPerPage+i];
				this.users[i].GetComponent<SearchUsersPopUpUserController>().show();
				this.users[i].SetActive(true);
			}
			else
			{
				this.users[i].SetActive(false);
			}
		}
	}
}