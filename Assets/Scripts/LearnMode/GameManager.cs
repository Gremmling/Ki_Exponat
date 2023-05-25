using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GameManager : MonoBehaviour
{
	//Object to manage the User Interface
	public UI Ui;

	//The finished rotation and position for the matches after they need to move
	public Vector3 targetPosition;
	public Quaternion targetRotation;

	//whos is playing the player or the AI
	private static Turn ActivePlayer;

	//Object for the saved list with numbers
	private SaveData saveD;
	private SaveData ListForProgressMode;

	//enumeration for turns
	public enum Turn
	{
		Player = 0,
		AI = 1
	}

	// all cups to change outline
	public GameObject[] Cups;

	//Lists for matches and old matches => Old matches needs to be deleted
	public List<GameObject> Matches = new List<GameObject>();
	public List<GameObject> OldMatches = new List<GameObject>();

	//lists for all 8 cups
	public List<GameObject> Note_Cup_10;
	public List<GameObject> Note_Cup_9;
	public List<GameObject> Note_Cup_8;
	public List<GameObject> Note_Cup_7;
	public List<GameObject> Note_Cup_6;
	public List<GameObject> Note_Cup_5;
	public List<GameObject> Note_Cup_4;
	public List<GameObject> Note_Cup_3;
	public List<GameObject> Note_Cup_2;

	//list for all lists of cups
	public List<List<GameObject>> AllCupsWithNotes;

	//list for the numbers if the ai is perfect trained
	private List<List<int>> PerfectAINumbers = new List<List<int>>{
		new List<int>{1},
		new List<int>{2},
		new List<int>{3},
		new List<int>{1, 2, 3},
		new List<int>{1},
		new List<int>{2},
		new List<int>{3},
		new List<int>{1, 2, 3},
		new List<int>{1}
	};

	private List<int> ProgressModeDeleteFromCups = new List<int> {0, 1, 2, 3};

	//string for the text what happens in the game
	private string explanation = "";

	//which player draws the last match
	private Turn lastDrawer;

	//array to save numbers the ai draws
	public int[] drawnNumbers = new int[8];

	private int lastCup;
	private int lastDrawnNumber;
	private int AmountMatches = 10;
	private int RestAmmountOfCups = 8;
	private int numberMatch = 0;

	//Position and rotation for the notes to move them back
	private Vector3[] LetterStartPos = { new Vector3(-0.154f, -0.833f, -0.225f), new Vector3(0.005f, -0.78f, -0.225f), new Vector3(0.171f, -0.837f, -0.225f) };

	private Quaternion[] LetterStartRotation = { new Quaternion(0, 0, 15, 0), new Quaternion(0, 0, 0, 0), new Quaternion(0, 0, -18.41f, 0) };

	//variables to rotate and move matches
	private float ElapsedAnimationTime = 0f;
	private float rotationTime = 2f;


	private bool LearnBool = true;
	private bool NeedsToMove = false;
	private bool NeedsToRotate = false;
	private bool PauseGame = false;
	private bool FinishScreenIsOpen = false;

	//variables to check whiche mode and which settings
	public static bool perfectMode;
	public static bool ProgressMode;
	public static int SliderValue;
	public bool firstTurn;
	public bool AllNotesForPerfectAiMode;

	// Start is called before the first frame update
	void Start()
	{
		ListForProgressMode = new SaveData();
		//set the settings variables
		firstTurn = ChooseMode.FirstTurnSwitch;
		AllNotesForPerfectAiMode = ChooseMode.NotesSwitchPerfectAI;
		perfectMode = ChooseMode.PerfectMode;
		ProgressMode = ChooseMode.ProgressMode;
		SliderValue = SliderScript.valueSlider;
		//set who starts the game
		if(firstTurn){
			ActivePlayer = Turn.Player;
			explanation = "Den ersten Zug macht der Mensch.";
			//Ui.ChangeTurn(Enum.GetName(typeof(Turn), ActivePlayer));
		}
		if(!firstTurn){
			ActivePlayer = Turn.AI;
			explanation = "Den ersten Zug macht der Computer.";
		}
		Ui.ChangeTurn(Enum.GetName(typeof(Turn), ActivePlayer));
		//load the data
		saveD = SaveGameManager.Load();
		//dissable buttons if the game is in the perfect ai mode
		MakeList();
		//SetNumberPapers();
		if (perfectMode)
		{
			Ui.BTN_ActivateLearning.interactable = false;
			Ui.LearningOnOff = false;
			Ui.ChangeLearning();
			DontShowNotesAndCups(PerfectAINumbers);
		}
		else{
			if(!ProgressMode)
				SetNumberPapers(saveD.RemainingCups);
			else{
				ProgressModeRemoveNumbers();
				SetNumberPapers(ListForProgressMode.RemainingCups);
			}
		}
		Time.timeScale = 1f;//set time to 1 so its not frozen
		PauseGame = Ui.GameIsPaused;
		Ui.ChangeTurn(Enum.GetName(typeof(Turn), ActivePlayer));
		Set_Outline_Cups(Cups, -1);//dissabel outlines of cups
		if(ActivePlayer == Turn.AI)
			Set_Outline_Cups(Cups, RestAmmountOfCups);
		if (!perfectMode)//if not in perfect mode send message to chat window
			Ui.SendMassageToChat(explanation);
		CheckGame();
	}

	// Update is called once per frame
	void Update()
	{
		//get value is finishScreen open
		FinishScreenIsOpen = Ui.FinishScreenIsActivaed;
		if (FinishScreenIsOpen)//disable all outlines
		{
			Set_Outline_Cups(Cups, -1);
		}
		PauseGame = Ui.GameIsPaused;
		if (!PauseGame)//game not pause then it needs to check diffrent things
		{
			LearnBool = Ui.LearningOnOff;
			float speedMultiplier = Mathf.Clamp((ElapsedAnimationTime * ElapsedAnimationTime), 0, 2);//set speed multiplier with elapsedAnimationTime
			if (NeedsToRotate || NeedsToMove)
				ElapsedAnimationTime += Time.deltaTime;//while the match needs to move add time on elapsed time so the matches moves faster the longer they move
			else
				ElapsedAnimationTime = 0f;//if not reset to 0
			if (NeedsToRotate)//if match needs to rotate rotate every choosen match
			{
				for (int i = 0; i < numberMatch; i++)
				{
					Matches[i].transform.rotation = Quaternion.RotateTowards(Matches[i].transform.rotation, targetRotation, 90 / rotationTime * Time.deltaTime * speedMultiplier);
					if (Matches[i].transform.rotation == targetRotation)
					{
						NeedsToRotate = false;
					}
				}
			}
			if (NeedsToMove)//move every choosen match to the left
			{
				for (int i = 0; i < numberMatch; i++)
				{
					Matches[i].transform.localPosition = Vector3.MoveTowards(Matches[i].transform.localPosition, targetPosition, 1.5f * Time.deltaTime * speedMultiplier);

				}
				if (Matches[0].transform.localPosition == targetPosition)
				{
					NeedsToMove = false;
				}
			}
		}
	}
	//creates list with all notes
	private void MakeList()
	{
		AllCupsWithNotes = new List<List<GameObject>>{
			Note_Cup_2,
			Note_Cup_3,
			Note_Cup_4,
			Note_Cup_5,
			Note_Cup_6,
			Note_Cup_7,
			Note_Cup_8,
			Note_Cup_9,
			Note_Cup_10
		};
	}

	void ProgressModeRemoveNumbers(){
		Debug.Log("SliderVlaue:" + SliderValue);
		for (int i = 0; i < SliderValue; i++)
		{
			(List<List<int>> CupsForProgressMode, List<int> CupIndices) = GetCupsWithNotes(3);
			int random = UnityEngine.Random.Range(0, CupsForProgressMode.Count - 1);
			int index = CupIndices[random];
			List<int> SelectedCup = CupsForProgressMode[random];
			RemoveNote(SelectedCup, index);
		}
	}

	(List<List<int>> Cups, List<int> Indices) GetCupsWithNotes(int cups){
		List<List<int>> res = new List<List<int>>();
		List<int> Indices = new List<int>();
		for (int i = 0; i < ListForProgressMode.RemainingCups.Count; i++)
		{
			if(ListForProgressMode.RemainingCups[i].Count > 1){
				res.Add(ListForProgressMode.RemainingCups[i]);
				Indices.Add(i);
			}
			if(res.Count == cups)
				return (Cups:res, Indices:Indices);
		}
		return (Cups:res, Indices:Indices);
	}

	void RemoveNote(List<int> Cup, int WhicheCup){
		List<int> CopyCup = new List<int>(Cup);
		if(PerfectAINumbers[WhicheCup].Count != 3){
			CopyCup.Remove(PerfectAINumbers[WhicheCup][0]);
		}
		int removedNumber = CopyCup[UnityEngine.Random.Range(0, CopyCup.Count - 1)];
		Cup.Remove(removedNumber);
	}

	//show just the notes the player needs to see. If in every cup is just one note then just show them
	void SetNumberPapers(List<List<int>> NumberList){
		Debug.Log(AllNotesForPerfectAiMode);
		if (!perfectMode || ProgressMode){
			foreach (var cups in AllCupsWithNotes)
			{
				foreach (var item in cups)
				{
					item.SetActive(false);
				}
			}
		}
		for (int i = 0; i < NumberList.Count; i++)
			{
				for (int j = 0; j < NumberList[i].Count; j++)
				{
					int cache = NumberList[i][j] - 1;
					AllCupsWithNotes[i][cache].SetActive(true);
				}
		}
	}

	//activate or deactivates Cups and Notes in the Perfect AI mode
	void DontShowNotesAndCups(List<List<int>> NumberList){
		if (!AllNotesForPerfectAiMode)
		{
			foreach (var cups in AllCupsWithNotes)
			{
				foreach (var item in cups)
				{
					item.SetActive(false);
				}
			}
			foreach (var item in Cups)
			{
				item.SetActive(false);
			}
		}
		else{
			foreach (var cups in AllCupsWithNotes)
			{
				foreach (var item in cups)
				{
					item.SetActive(false);
				}
			}
						for (int i = 0; i < NumberList.Count; i++)
			{
				for (int j = 0; j < NumberList[i].Count; j++)
				{
					int cache = NumberList[i][j] - 1;
					AllCupsWithNotes[i][cache].SetActive(true);
				}
			}
		}
	}

	// check if the game is paused or not
	void CheckGame()
	{
		if (AmountMatches == 0)
		{
			StartCoroutine(EndGame());
		}
		else if (AmountMatches >= 0)
		{
			StartCoroutine(DoTurn());
		}
	}

	//starts when game is over
	private IEnumerator EndGame()
	{
		Debug.Log(lastDrawer);
		if (lastDrawer == Turn.Player)//if palyer draws last match ai wins
		{
			yield return StartCoroutine(Timer(5));//wait for the timer
			explanation = "Der Computer hat gewonnen.";
			Ui.SendMassageToChat(explanation);
			yield return StartCoroutine(Timer(5));
			Ui.ShowFinishScreen();
		}
		else if (lastDrawer == Turn.AI)//player wins game
		{
			explanation = "Der Mensch hat gewonnen.";
			Ui.SendMassageToChat(explanation);
			yield return StartCoroutine(Timer(5));
			if (LearnBool && !perfectMode)//ai should learn
			{
				explanation = "Der Computer beginnt zu lernen.";
				Ui.SendMassageToChat(explanation);
				for (int i = 0; i < drawnNumbers.Length; i++)//check ever drawn number
				{
					if (drawnNumbers[i] != 0)//if there is no 0 on a positon its a drawn number
					{
						if (saveD.RemainingCups[i].Count > 1)//if the cup has more than one number i should be deleted
						{
							explanation = "Aus dem Becher mit der Nummer: " + (i + 2) + " wird die Nummer " + drawnNumbers[i] + " entfernt.";
							Ui.SendMassageToChat(explanation);
							yield return StartCoroutine(Timer(5));
							AllCupsWithNotes[i][drawnNumbers[i] - 1].SetActive(false);
							saveD.RemainingCups[i].Remove(drawnNumbers[i]);
							break;
						}
						else //if the cups has one number and not more it needs to go to the next cup
						{
							explanation = "Aus dem Becher " + (i + 2) + " wird keine Nummer entfernt, weil es nur noch eine Ziehmöglichkeit gibt.";
							Ui.SendMassageToChat(explanation);
							yield return StartCoroutine(Timer(5));
						}
					}
				}
			}
			Ui.ShowFinishScreen();
		}
		foreach (var item in saveD.RemainingCups.Select((x, i) => new { value = x, index = i }))
		{
			string b = $"Cup {item.index + 2}: ";
			foreach (var a in item.value)
			{
				b += a + ", ";
			}
			Debug.Log(b);
		}
		SaveGameManager.Save(saveD);//save the new list so the ai learned
		Move_Notes_Back();
		yield return StartCoroutine(Timer(5));
		Ui.ShowFinishScreen();
		Debug.Log("Ende");
		//UnityEditor.EditorApplication.isPlaying = false;
	}

	//moves the notes back
	private void Move_Notes_Back()
	{
		for (int i = 0; i < drawnNumbers.Length; i++)
		{
			int noteNumber = drawnNumbers[i];
			Vector3 FinishPos = new Vector3(0, 0.683f, 0);
			if (noteNumber != 0)
			{
				//Da in Becher 2 nur 2 Zettel dann muss der Zettel mit der Nummer 2 an die Position von Zettel nummer 3 bewegt werden.
				if ( noteNumber == 3 || (i == 0 && noteNumber == 2) )
				{
					AllCupsWithNotes[i][noteNumber - 1].transform.rotation = new Quaternion(0, 0, -0.13053f, 0.99144f);
					FinishPos.x = -0.134f;
				}
				if (noteNumber == 1)
				{
					AllCupsWithNotes[i][noteNumber - 1].transform.rotation = new Quaternion(0, 0, 0.13053f, 0.99144f);
					FinishPos.x = +0.134f;
				}
				// if (noteNumber == 3)
				// {
				// 	AllCupsWithNotes[i][noteNumber - 1].transform.rotation = new Quaternion(0, 0, -0.13053f, 0.99144f);
				// 	FinishPos.x = -0.134f;
				// }
				AllCupsWithNotes[i][noteNumber - 1].transform.position -= FinishPos;
			}
		}
	}
	//function to draw a random number from the cup
	public int KI_Draw_Random_Number(List<int> cup)
	{
		int random = UnityEngine.Random.Range(0, cup.Count);//get random number between 0 and 2
		return cup[random]; //get number at position 0, 1 or 2
	}

	//set outline to cups
	public void Set_Outline_Cups(GameObject[] Cups, int SelectedCup)
	{
		for (int i = 0; i < Cups.Length; i++)
		{
			if (SelectedCup == i)
			{
				Cups[SelectedCup].GetComponent<cakeslice.Outline>().enabled = true;
				continue;
			}
			Cups[i].GetComponent<cakeslice.Outline>().enabled = false;
		}
	}

	//move note to the top
	public void Move_Drawn_Number(int Cup_Number, int DrawnNumber)
	{
		Debug.Log(AllCupsWithNotes[Cup_Number][DrawnNumber - 1].transform.rotation);
		Quaternion FinishRotation = new Quaternion(0, 0, 0, 0);
		Vector3 FinishPos = new Vector3(0, 0.683f, 0);
		if (DrawnNumber == 3 || (Cup_Number == 0 && DrawnNumber == 2) )
		{
			FinishPos.x = -0.134f;
		}
		if (DrawnNumber == 1)
		{
			FinishPos.x = +0.134f;
		}
		AllCupsWithNotes[Cup_Number][DrawnNumber - 1].transform.rotation = FinishRotation;
		AllCupsWithNotes[Cup_Number][DrawnNumber - 1].transform.position += FinishPos;
	}

	//a timer to wait between turns
	private IEnumerator Timer(float TimeToWait)
	{
		float fillValue = 1;
		float finishedTime = TimeToWait + Time.time;
		yield return new WaitUntil(() =>
			{
				Ui.FillCircleValue(fillValue);
				fillValue = ((finishedTime - Time.time) / TimeToWait);
				return finishedTime < Time.time || (Input.GetMouseButtonDown(1) && !PauseGame);//if timer waits 5 seconds return or if the player press the right mouse button return out so he can skip the timer. But only if the game is not paused
			});

	}

	//manage the turns
	private IEnumerator DoTurn()
	{
		int number = -1;
		int cupText = RestAmmountOfCups + 2;
		if (ActivePlayer == Turn.Player)//Players turn
		{
			Ui.WhosTurn = (int)ActivePlayer;
			//if and elses for the player if there are only two or one match disable the other buttons
			if (AmountMatches == 1)
				Ui.Button_EnableOrDisable(true, 1);
			else if (AmountMatches == 2)
				Ui.Button_EnableOrDisable(true, 2);
			else
				Ui.Button_EnableOrDisable(true, 3);
			yield return new WaitUntil(() => Ui.isClicked);
			Ui.Button_EnableOrDisable(false, 3);
			number = Ui.Matches;
			int newCup = cupText - number;
			if (!perfectMode){//disable the message for perfect ai mode
				explanation = "Der Mensch wählt die Zahl " + number + ".";
				Ui.SendMassageToChat(explanation);
			}
			yield return StartCoroutine(Timer(1));
			if(AmountMatches == 1 || AmountMatches - number == 1)
				explanation = "Es bleibt noch: 1 Streichholz übrig.";
			else
				explanation = "Es bleiben noch: " + newCup + " Streichhölzer.";
			if(!perfectMode)
				Ui.SendMassageToChat(explanation);
			//Ui.WhosTurn = (int)Turn.AI;
			lastDrawer = Turn.Player;
			ActivePlayer = Turn.AI;//switch turn to ai
		}
		else//ai turn
		{
			Ui.WhosTurn = (int)ActivePlayer;
			Ui.Button_EnableOrDisable(false, 3);
			if (AmountMatches == 1)// if only one match is there can just draw one match
			{
				number = 1;
				//yield return StartCoroutine(Timer(5));
				explanation = "Der Computer zieht das letzte Streichholz.";
				Ui.SendMassageToChat(explanation);
			}
			else
			{
				//yield return StartCoroutine(Timer(5));
				Set_Outline_Cups(Cups, RestAmmountOfCups);
				if (!perfectMode){// disable message for perfect ai mode
					explanation = "Der Computer wählt aus Becher " + (RestAmmountOfCups + 2) + " eine Zahl aus.";
					Ui.SendMassageToChat(explanation);
				}
				//if and else to draw from the right list
				if (perfectMode)
					number = KI_Draw_Random_Number(PerfectAINumbers[RestAmmountOfCups]);
				else if(ProgressMode)
					number = KI_Draw_Random_Number(ListForProgressMode.RemainingCups[RestAmmountOfCups]);
				else
					number = KI_Draw_Random_Number(saveD.RemainingCups[RestAmmountOfCups]);
				lastDrawnNumber = number;
				drawnNumbers[RestAmmountOfCups] = number;//save drawn number
				yield return StartCoroutine(Timer(1));
				explanation = "Der Computer wählt die Zahl " + drawnNumbers[RestAmmountOfCups] + " aus.";
				Ui.SendMassageToChat(explanation);
				int newCup = cupText - number;//rest ammount of matches but named cup because the ai draws the number in a cup
				Move_Drawn_Number(RestAmmountOfCups, number);
				//yield return StartCoroutine(Timer(5));
				if(AmountMatches == 1 || AmountMatches - number == 1)
					explanation = "Es bleibt noch: 1 Streichholz übrig.";
				else
					explanation = "Es bleiben noch: " + newCup + " Streichhölzer.";
				Ui.SendMassageToChat(explanation);
			}
			lastDrawer = Turn.AI;
			ActivePlayer = Turn.Player;
		}
		if (AmountMatches > 1)
			RestAmmountOfCups -= number;
		AmountMatches -= number;
		Ui.isClicked = false;
		yield return StartCoroutine(Timer(5));
		RotateMatches(number);
		yield return StartCoroutine(Timer(5));
		MoveMatches(number);
		//yield return new WaitForSeconds(5);
		yield return StartCoroutine(Timer(5));
		//yield return new WaitUntil(() => !NeedsToMove);
		DeleteMatches();
		if(AmountMatches != 0)
			Ui.ChangeTurn(Enum.GetName(typeof(Turn), ActivePlayer));
		Set_Outline_Cups(Cups, -1);
		CheckGame();
	}
	//rotate matches
	private void RotateMatches(int number)
	{
		numberMatch = number;
		NeedsToRotate = true;
	}
	//move matches
	private void MoveMatches(int number)
	{
		numberMatch = number;
		NeedsToMove = true;
	}

	//Delete matches
	private void DeleteMatches()
	{
		foreach (var item in Matches.GetRange(0, numberMatch))
		{
			Destroy(item);
		}
		Matches.RemoveRange(0, numberMatch);
		NeedsToMove = false;
		NeedsToRotate = false;
	}
}