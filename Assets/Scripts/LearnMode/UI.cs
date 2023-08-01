using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{

	//Objects for the two Pop up windows
	public GameObject FinishScreen;
	public GameObject PausePopUp;
	public GameObject ProgressBar;

	//Objects for the Circle
	[Range(0, 100)]
	public Image uiFill;

	//Sprites for turn player or turn AI
	public Sprite PlayerTurnSprite;
	public Sprite KiTurnSprite;

	//slider and _sliderText
	[SerializeField] private Slider _slider;
	[SerializeField] private TextMeshProUGUI _sliderText;
	private static double SliderNumber;
	private static bool firstRound;
	private static bool KI_Lose = false;
	private static bool KI_CanLearn;
	private double finalValue;

	//Objects Images which Turn
	public Image TurnImg;

	//All Objects for the Buttons
	public Button BTN_Ammount_One;
	public Button BTN_Ammount_Two;
	public Button BTN_Ammount_Three;
	public Button BTN_ActivateLearning;
	public Button BTN_Pause;
	public Button BTN_Resume;
	public Button BTN_Exit;
	public Button BTN_Home;
	public Button BTN_NewGame;

	//An Array of all Number Buttons. These Buttons manage whiche ammount of matches the player wants to remove
	private Button[] AmmountMatchesChooseButtons = new Button[3];

	//Objects for the Text window on the bottom right
	public GameObject ChatPanel, TextObject;

	// List for all Texts to explain the game
	[SerializeField]
	List<Message> messageList = new List<Message>();

	//Variable to save how much Matches are choosen
	public int Matches;

	//int for turn player or ai 0 = Player and 1 = AI
	public int WhosTurn;

	//booleans for diffrent moments
	public bool isClicked;//when player has choosen a number
	public static bool LearningOnOff;//to switch the learn mode on or off
	public bool FinishScreenIsActivaed = false; //to hide or show the pop up for the finish screen
	private bool PauseIsClicked = true;//if button pause is clicked
	public bool GameIsPaused = false;//when game is paused

	private static string fileName = "SaveAi.s";
	private static string path;

									   // Start is called before the first frame update
	void Start()
	{
		//fill the three Buttons into the array
		Debug.Log(LearningOnOff);
		AmmountMatchesChooseButtons[0] = BTN_Ammount_One;
		AmmountMatchesChooseButtons[1] = BTN_Ammount_Two;
		AmmountMatchesChooseButtons[2] = BTN_Ammount_Three;
		isClicked = false;
		path = Path.Combine(Application.dataPath, "Save");
		//add on click to buttons
		BTN_Ammount_One.onClick.AddListener(() => PlayerChooseNumber(1));
		BTN_Ammount_Two.onClick.AddListener(() => PlayerChooseNumber(2));
		BTN_Ammount_Three.onClick.AddListener(() => PlayerChooseNumber(3));
		BTN_ActivateLearning.onClick.AddListener(() => ChangeLearning());
		//add on click to pause button
		if(LearningOnOff)
			BTN_ActivateLearning.GetComponent<ToggleButton>().Activated = true;
		else if(!LearningOnOff)
			BTN_ActivateLearning.GetComponent<ToggleButton>().Activated = false;
		BTN_Pause.onClick.AddListener(() =>
		{
			Debug.Log(PauseIsClicked);
			//	PauseIsClicked = !PauseIsClicked;
			if (PauseIsClicked)
			{// if PauseIsClicked == true change value to false and call Pause game
				PauseGame();
			}
			else
			{// if PauseIsClicked == false change value to true and call Pause game
				ResumeGame();
			}
		});
		BTN_Resume.onClick.AddListener(() => ResumeGame());
		BTN_NewGame.onClick.AddListener(() => NewGame());
		BTN_Home.onClick.AddListener(() => GoHome());
		BTN_Exit.onClick.AddListener(() => GoHome());
		//if(KI_Lose && !LearningOnOff){
			if(!firstRound)
				SliderNumber = SliderScript.valueSlider;
			else if(KI_Lose && !LearningOnOff && KI_CanLearn)
			{
				++SliderNumber;
			}
			finalValue = SliderNumber * 5.882352941176471;

			Debug.Log("Final Number" + finalValue);
			if(SliderNumber == 0){
					_sliderText.text = SliderNumber.ToString();
				}
				else if(SliderNumber == 17){
					_sliderText.text = finalValue.ToString();
				}
				else{
					//_sliderText.text = finalValue.ToString("#.0000");
					_sliderText.text = finalValue.ToString("#.0");
				}
				Debug.Log("slider" + _sliderText.text);
			_slider.value = (int)SliderNumber;
			Debug.Log("slider value" + _slider.value);
		//}
	}

	void Update()
	{
		//Check if Player is pressing escape while he plays the game.
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (GameIsPaused)
			{//if GameIsPaused = true resume game
				ResumeGame();
			}
			else
			{//if GameIsPaused = false pause game
				PauseGame();
			}
		}
	}

	public void Set_Ki_Lose(bool lose){
		KI_Lose = lose;
	}

		public void Set_Ki_CanLearn(bool learn){
		KI_CanLearn = learn;
	}

	public void Disable_ProgressBar(bool active){
		ProgressBar.SetActive(active);
	}

	//Function to show the finish screen
	public void ShowFinishScreen()
	{
		GameIsPaused = true;
		FinishScreenIsActivaed = true;
		//disable all Buttons
		Button_EnableOrDisable(false, 3);
		BTN_ActivateLearning.interactable = false;
		BTN_Pause.interactable = false;
		//set timescale to 0 so the time is stopped
		Time.timeScale = 0f;
		//activate the finish screen pop up
		FinishScreen.SetActiveRecursively(true);
	}

	private void GoHome()
	{

		firstRound = false;
		if(File.Exists(Path.Combine(path, fileName))){
			//File.Delete(path);
			SaveGameManager.ResetData();
		}
		GameManager.allreadyProgessed = false;
		setLearning(false);
		SceneManager.LoadScene("MainMenu");

	}

	private void NewGame()
	{
		firstRound = true;
		SceneManager.LoadScene("LearnGame");
	}

	//Function to pause game
	private void PauseGame()
	{
		PauseIsClicked = !PauseIsClicked;
		GameIsPaused = true;
		//activate pause popup
		PausePopUp.SetActiveRecursively(true);
		//disable all buttons
		Button_EnableOrDisable(false, 3);
		BTN_ActivateLearning.interactable = false;
		//pause time with timescale = 0
		Time.timeScale = 0f;
	}

	private void ResumeGame()
	{
		PauseIsClicked = !PauseIsClicked;
		GameIsPaused = false;
		//hide pause popup
		PausePopUp.SetActiveRecursively(false);
		//is it the players turn so activate all three number buttons for him
		if (WhosTurn != 1)
		{
			Button_EnableOrDisable(true, 3);
			Debug.Log("Buttons aktiviert");
		}
		//activate the Other Button
		BTN_ActivateLearning.interactable = true;
		//set timescale to 1 so its not frozen
		Time.timeScale = 1f;
	}

	//switch between learning mode on or off
	public void ChangeLearning()
	{
		LearningOnOff = !LearningOnOff;

		Debug.Log(LearningOnOff);
	}

	public void setLearning(bool setLearning){
		LearningOnOff = setLearning;
	}

	//set value of the fill ammount of the Circle
	public void FillCircleValue(float value)
	{
		uiFill.fillAmount = value;
	}

	public bool getLearnStatus(){
		return LearningOnOff;
	}

	//enabel or dissable or enable the number buttons
	public void Button_EnableOrDisable(bool ButtonSwitch, int AmmountBTN)
	{
		for (int i = 0; i < AmmountMatchesChooseButtons.Length; i++)
		{
			if (i < AmmountBTN)
			{
				AmmountMatchesChooseButtons[i].interactable = ButtonSwitch;
				AmmountMatchesChooseButtons[i].GetComponent<BoxCollider>().enabled = ButtonSwitch;
			}
		}
	}

	//save the number the player is choosing and set isClicked to true
	void PlayerChooseNumber(int AmmountMatches)
	{
		Matches = AmmountMatches;
		isClicked = true;
	}

	//switch img to show whos turn is acitve
	public void ChangeTurn(string turn)
	{
		if (turn.Equals("Player"))
		{
			TurnImg.sprite = PlayerTurnSprite;
		}
		else if (turn.Equals("AI"))
		{
			TurnImg.sprite = KiTurnSprite;
		}
	}

	//create message in the chat window
	public void SendMassageToChat(string text)
	{
		Message newMessage = new Message();

		newMessage.text = text;

		GameObject newText = Instantiate(TextObject, ChatPanel.transform);//create new game object of the text part

		newMessage.textObject = newText.GetComponent<TextMeshProUGUI>();

		newMessage.textObject.text = newMessage.text;

		messageList.Add(newMessage);//add message to list

		messageList[messageList.Count - 1].textObject.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);//set color to white

		for (int i = 0; i < messageList.Count - 1; i++)//set othere messages to a grey
		{
			messageList[i].textObject.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 100);
		}
	}
}
//class for message Objects
[System.Serializable]
public class Message
{
	public String text;
	public TextMeshProUGUI textObject;
}