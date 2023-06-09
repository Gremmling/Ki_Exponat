using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor;

public class ChooseMode : MonoBehaviour
{

	//Buttons for the Scene.
	public Button BTN_Exit;
	public Button BTN_PerfectAI;
	public Button BTN_Learn;
	public Button BTN_Tipps;
	public Button BTN_FirstTurn;
	public Button BTN_Continue;
	public Button BTN_Back;

	public GameObject PopUpSettings_PerfectMode;

	public GameObject PopUpSettings_LearnMode;
	public Button DummyMode;



	//static Variables to manage the game Mode and setting
	//PerfectMode = true => Perfect AI Mode, PerfectMode = false => Learn Mode
	//NotesSwitchPerfectAI = true => Show All Notes in the Perfect AI mode, NotesSwitchPerfectAI = false => Just show the possible Notes for the Perfect Ai Mode
	//FirstTurnSwitch = true => AI starts the game, FirstTurnSwitch = false => Player starts the Game
	public static bool PerfectMode;
	public static bool ProgressMode = false;
	public static bool NotesSwitchPerfectAI;
	public static bool FirstTurnSwitch;

	public GameObject BTN_NotesSwitchPerfectAI;
	public GameObject BTN_FirstTurnSwitch;


	void Start(){
		//BTN_Back.onClick.AddListener(() => ClosePopUp());
	}


	//Enum for the two Modes
	public enum Mode
	{
		Perfect = 0,
		Learn = 1,
		Progress = 2
	}

	//Load Main menu Scene
	public void GoBack(){
		SceneManager.LoadScene("MainMenu");
	}
	//Load Game Scene
    public void LoadMode(int GameMode){
        if(GameMode == (int)Mode.Learn){
			PerfectMode = false;
		}
        if(GameMode == (int)Mode.Perfect){
			PerfectMode = true;
        }

		if(GameMode == (int) Mode.Progress){
			PerfectMode = false;
			ProgressMode = true;
		}
		NotesSwitchPerfectAI = BTN_NotesSwitchPerfectAI.GetComponent<ToggleButton>().Activated;
		FirstTurnSwitch = BTN_FirstTurnSwitch.GetComponent<ToggleButton>().Activated;
		SceneManager.LoadScene("LearnGame");
	}

	public void OpenPopUp(int whichPopUp){
		if(whichPopUp == 0){
			PopUpSettings_PerfectMode.SetActiveRecursively(true);
		}
		else{
			PopUpSettings_LearnMode.SetActiveRecursively(true);
		}
		BTN_Exit.interactable = false;
		BTN_FirstTurn.interactable = false;
		DummyMode.interactable = false;
		BTN_Learn.interactable = false;
		BTN_PerfectAI.interactable = false;
	}

	public void ClosePopUp(int whichPopUp){
		if(whichPopUp == 0){
			PopUpSettings_PerfectMode.SetActiveRecursively(false);
		}
		else{
			PopUpSettings_LearnMode.SetActiveRecursively(false);
		}
		BTN_Exit.interactable = true;
		BTN_FirstTurn.interactable = true;
		DummyMode.interactable = true;
		BTN_Learn.interactable = true;
		BTN_PerfectAI.interactable = true;
	}
}