using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TutorialController : MonoBehaviour 
{
	public static TutorialController instance;
	public GameObject blockObject;

	private GameObject mainBlock;
	private GameObject description;
	private GameObject previousButton;
	private GameObject nextButton;
	private GameObject picture0;
	private GameObject picture1;
	private GameObject background;
	private GameObject title;

	private User player;
	public Sprite[] backgrounds;
	public Sprite[] caracters;
	public string[] titles;
	public string[] descriptions;

	private int widthScreen;
	private int heightScreen;
	private float worldWidth;
	private float worldHeight;
	private float pixelPerUnit;

	private int sequenceId;

	void Update()
	{
		if (Screen.width != this.widthScreen || Screen.height != this.heightScreen) 
		{
			this.resize();
		}
	}
	void Awake()
	{
		this.widthScreen = Screen.width;
		this.heightScreen = Screen.height;
		this.pixelPerUnit = 108f;
		this.sequenceId = 0;
		this.initializeScene ();
	}
	public void initializeScene()
	{
		this.mainBlock = Instantiate(this.blockObject) as GameObject;
		this.description = GameObject.Find ("Description");
		this.background = GameObject.Find ("Background");
		this.picture0 = GameObject.Find ("picture0");
		this.picture1 = GameObject.Find ("picture1");
		this.nextButton = GameObject.Find ("NextButton");
		this.previousButton = GameObject.Find ("PreviousButton");
		this.title = GameObject.Find ("TitleLabel");
		this.nextButton.transform.FindChild("Title").GetComponent<TextMeshPro> ().text = "Suivant";
		this.previousButton.transform.FindChild("Title").GetComponent<TextMeshPro> ().text = "Précédent";
	}
	void Start ()
	{
		instance = this;
		this.resize ();
		this.player = new User ();
		this.player.Username = ApplicationModel.username;
		this.selectSequence (0);
	}
	private IEnumerator quitTutorial()
	{
		yield return StartCoroutine(this.player.setTutorialStep (1));
		Application.LoadLevel("NewHomePage");
	}
	public void resize()
	{
		this.widthScreen=Screen.width;
		this.heightScreen=Screen.height;
		this.worldHeight = 2f*Camera.main.GetComponent<Camera>().orthographicSize;
		this.worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;

		float mainBlockLeftMargin = 0.2f;
		float mainBlockRightMargin = 0.2f;
		float mainBlockUpMargin = 7.5f;
		float mainBlockDownMargin = 0.2f;
		
		float mainBlockHeight = worldHeight - mainBlockUpMargin-mainBlockDownMargin;
		float mainBlockWidth = worldWidth-mainBlockLeftMargin-mainBlockRightMargin;
		Vector2 mainBlockOrigin = new Vector3 (-worldWidth/2f+mainBlockLeftMargin+mainBlockWidth/2f, -worldHeight / 2f + mainBlockDownMargin + mainBlockHeight / 2,0f);
		
		this.mainBlock.GetComponent<BlockController> ().resize(new Rect(mainBlockOrigin.x,mainBlockOrigin.y,mainBlockWidth,mainBlockHeight));

		this.description.GetComponent<TextContainer> ().width = mainBlockWidth - 0.4f;
		this.description.GetComponent<TextContainer> ().height = mainBlockHeight - 1f;
		this.description.transform.position = new Vector3 (mainBlockOrigin.x, mainBlockOrigin.y + 0.3f, 0f);

		this.nextButton.transform.position = new Vector3 (mainBlockWidth / 2f + mainBlockOrigin.x - 1.5f, mainBlockOrigin.y - mainBlockHeight / 2f + 0.4f, 0f);
		this.previousButton.transform.position = new Vector3 (- mainBlockWidth / 2f + mainBlockOrigin.x + 1.5f, mainBlockOrigin.y - mainBlockHeight / 2f + 0.4f, 0f);

		this.picture0.transform.position = new Vector3 (-5f*this.worldWidth / 16f, 0f, 0f);
		this.picture1.transform.position = new Vector3 (5f*this.worldWidth / 16f, 0f, 0f);

		//view.screenVM.resize ();
		//view.tutorialVM.resize (view.screenVM.heightScreen);
	}
	public void selectSequence(int id)
	{
		this.sequenceId = id;
		if(this.sequenceId==0)
		{
			this.previousButton.transform.gameObject.SetActive(false);
			this.background.transform.GetComponent<SpriteRenderer>().sprite=this.backgrounds[0];
			this.picture0.transform.GetComponent<SpriteRenderer>().sprite=this.caracters[0];
			this.picture1.transform.GetComponent<SpriteRenderer>().sprite=this.caracters[1];
			this.title.transform.GetComponent<TextMeshPro>().text=this.titles[0];
			this.description.transform.GetComponent<TextMeshPro>().text=this.descriptions[0];
		}
		else if(this.sequenceId==1)
		{
			this.previousButton.transform.gameObject.SetActive(true);
			this.background.transform.GetComponent<SpriteRenderer>().sprite=this.backgrounds[1];
			this.picture0.transform.GetComponent<SpriteRenderer>().sprite=this.caracters[2];
			this.picture1.transform.GetComponent<SpriteRenderer>().sprite=this.caracters[3];
			this.title.transform.GetComponent<TextMeshPro>().text=this.titles[1];
			this.description.transform.GetComponent<TextMeshPro>().text=this.descriptions[1];
		}
		else if(this.sequenceId==2)
		{
			this.previousButton.transform.gameObject.SetActive(true);
			this.background.transform.GetComponent<SpriteRenderer>().sprite=this.backgrounds[0];
			this.picture0.transform.GetComponent<SpriteRenderer>().sprite=this.caracters[4];
			this.picture1.transform.GetComponent<SpriteRenderer>().sprite=this.caracters[5];
			this.title.transform.GetComponent<TextMeshPro>().text=this.titles[2];
			this.description.transform.GetComponent<TextMeshPro>().text=this.descriptions[2];
		}
	}
	public void nextButtonHandler()
	{
		if(this.sequenceId==0)
		{
			this.selectSequence(1);
		}
		else if(this.sequenceId==1)
		{
			this.selectSequence(2);
		}
		else if(this.sequenceId==2)
		{
			StartCoroutine(this.quitTutorial());
		}
	}
	public void previousButtonHandler()
	{
		if(this.sequenceId==1)
		{
			this.selectSequence(0);
		}
		else if(this.sequenceId==2)
		{
			this.selectSequence(1);
		}
	}
}
