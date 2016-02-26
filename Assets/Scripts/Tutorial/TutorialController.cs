using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour 
{
	public static TutorialController instance;
	public GameObject blockObject;

	private GameObject backOfficeController;
	private GameObject mainBlock;
	private GameObject description;
	private GameObject nextButton;
	private GameObject picture0;
	private GameObject picture1;
	private GameObject background;
	private GameObject mainLogo;

	private GameObject mainCamera;
	private GameObject sceneCamera;

	void Start ()
	{
		instance = this;
		this.initializeBackOffice();
		this.initializeScene ();
		this.resize ();
		BackOfficeController.instance.hideLoadingScreen();
	}
	private void initializeBackOffice()
	{
		this.backOfficeController = GameObject.Find ("BackOfficeController");
		this.backOfficeController.AddComponent<BackOfficeTutorialController>();
		this.backOfficeController.GetComponent<BackOfficeTutorialController>().initialize();
	}
	public void initializeScene()
	{
		this.mainCamera = gameObject;
		this.sceneCamera = GameObject.Find ("sceneCamera");
		this.mainBlock = Instantiate(this.blockObject) as GameObject;
		this.description = GameObject.Find ("Description");
		this.background = GameObject.Find ("Background");
		this.picture0 = GameObject.Find ("picture0");
		this.picture1 = GameObject.Find ("picture1");
		this.nextButton = GameObject.Find ("NextButton");
		this.nextButton.transform.FindChild("Title").GetComponent<TextMeshPro> ().text = WordingTutorialScene.getReference(0);
		this.description.transform.GetComponent<TextMeshPro>().text=WordingTutorialScene.getReference(2);
		this.mainLogo = GameObject.Find("mainLogo");
	}
	private IEnumerator quitTutorial()
	{
		BackOfficeController.instance.displayLoadingScreen();
		yield return StartCoroutine(ApplicationModel.player.setTutorialStep (1));
		SceneManager.LoadScene("NewStore");
	}
	public void resize()
	{
		Vector2 backgroundSize = new Vector2 (1920f,1080f);
		Vector2 leftPictureSize = new Vector2(634f,1080f);
		Vector2 rightPictureSize=new Vector2(638f,1080f);
		Vector2 backgroundWorldSize = new Vector2();
		Vector2 leftPictureWorldSize = new Vector2();
		Vector2 rightPictureWorldSize = new Vector2();

		this.mainCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.mainCamera.transform.position = ApplicationDesignRules.mainCameraPosition;
		this.sceneCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.sceneCamera.transform.position = new Vector3(0f,0f,-10f);

		float mainBlockHeight=3f-(ApplicationDesignRules.screenRatio-1f);
		float mainBlockUpMargin=ApplicationDesignRules.worldHeight-mainBlockHeight;

		float scale=ApplicationDesignRules.worldHeight/(backgroundSize.y/ApplicationDesignRules.pixelPerUnit);
		backgroundWorldSize = new Vector2((backgroundSize.x*scale)/108f,(backgroundSize.y*scale)/108f);
		this.background.transform.localScale=new Vector3(scale,scale,scale);
		this.background.transform.position=new Vector3(0f,-ApplicationDesignRules.worldHeight/2f+mainBlockHeight+backgroundWorldSize.y/2f,0f);

		this.mainBlock.GetComponent<NewBlockController> ().resize(0f,mainBlockUpMargin,ApplicationDesignRules.worldWidth,mainBlockHeight);

		leftPictureWorldSize = new Vector2((leftPictureSize.x*scale)/108f,(leftPictureSize.y*scale)/108f);
		rightPictureWorldSize = new Vector2((rightPictureSize.x*scale)/108f,(rightPictureSize.y*scale)/108f);

		if(ApplicationDesignRules.worldWidth-leftPictureWorldSize.x-rightPictureWorldSize.x<0)
		{
			this.picture1.SetActive(false);
		}
		else
		{
			this.picture1.SetActive(true);
		}

		this.mainLogo.transform.position=new Vector3(0f,-ApplicationDesignRules.worldHeight/2f+mainBlockHeight,0f);
		this.description.GetComponent<TextContainer>().height=mainBlockHeight-1.2f;
		this.description.transform.position=new Vector3(0f,-ApplicationDesignRules.worldHeight/2f+mainBlockHeight/2f+0.1f,0f);
		this.picture0.transform.localScale=new Vector3(scale,scale,scale);
		this.picture0.transform.position=new Vector3(-ApplicationDesignRules.worldWidth/2f+leftPictureWorldSize.x/2f,-ApplicationDesignRules.worldHeight/2f+mainBlockHeight+leftPictureWorldSize.y/2f,0f);
		this.picture1.transform.localScale=new Vector3(scale,scale,scale);
		this.picture1.transform.position=new Vector3(ApplicationDesignRules.worldWidth/2f-rightPictureWorldSize.x/2f,-ApplicationDesignRules.worldHeight/2f+mainBlockHeight+rightPictureWorldSize.y/2f,0f);
		this.description.GetComponent<TextContainer>().width=ApplicationDesignRules.worldWidth-2f*ApplicationDesignRules.blockHorizontalSpacing;
		this.nextButton.transform.position = new Vector3 (0f, -4.6f);
	}
	public void nextButtonHandler()
	{
		StartCoroutine(this.quitTutorial());
	}
}
