using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class TutorialController : MonoBehaviour 
{
	public static TutorialController instance;
	private TutorialView view;
	public GUIStyle[] tutorialVMStyle;
	public User player;
	public Texture2D[] mainPictures;
	public string[] titles;
	public string[] descriptions;
	
	void Start ()
	{
		instance = this;
		this.view = Camera.main.gameObject.AddComponent <TutorialView>();
		this.initStyles ();
		this.resize ();
		this.player = new User ();
		this.player.Username = ApplicationModel.username;
		this.selectPage ();
	}
	public void selectPage()
	{
		view.tutorialVM.title = this.titles [view.tutorialVM.selectedPage];
		view.tutorialVM.description = this.descriptions [view.tutorialVM.selectedPage];
		view.tutorialVM.mainPictureStyle.normal.background = this.mainPictures [view.tutorialVM.selectedPage];
	}
	public void displayPrecedentPage()
	{
		if(view.tutorialVM.selectedPage>0)
		{
			view.tutorialVM.selectedPage--;
			this.selectPage();
		}
	}
	public void displayNextPage()
	{
		if(view.tutorialVM.selectedPage==2)
		{
			StartCoroutine(this.quitTutorial());
		}
		else
		{
			view.tutorialVM.selectedPage++;
			this.selectPage();
		}
	}
	private IEnumerator quitTutorial()
	{
		view.tutorialVM.guiEnabled = false;
		yield return StartCoroutine(this.player.setTutorialStep (1));
		Application.LoadLevel("HomePage");
	}
	public void resize()
	{
		view.screenVM.resize ();
		view.tutorialVM.resize (view.screenVM.heightScreen);
	}
	public void initStyles()
	{
		view.tutorialVM.styles=new GUIStyle[this.tutorialVMStyle.Length];
		for(int i=0;i<this.tutorialVMStyle.Length;i++)
		{
			view.tutorialVM.styles[i]=this.tutorialVMStyle[i];
		}
		view.tutorialVM.initStyles();
	}
}
