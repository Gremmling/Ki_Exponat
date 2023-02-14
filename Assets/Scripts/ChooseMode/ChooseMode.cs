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

	//Spirts for the Button Background
	public Sprite Sprite_On;
	public Sprite Sprite_Off;
	public Sprite Sprite_On_Outline;
	public Sprite Sprite_Off_Outline;
	public Sprite Sprite_FirstTurnPlayer_Outline;
	public Sprite Sprite_FirstTurnKI_Outline;

	public Sprite Sprite_Tipps;
	public Sprite Sprite_FirstTurn;

	public Sprite Sprite_Outline_Tipps;
	public Sprite Sprite_Outline_FirstTurn;
	public Sprite Sprite_FirstTurnKI;
	public Sprite Sprite_FirstTurnPlayer;


	//static Variables to manage the game Mode and setting
	//PerfectMode = true => Perfect AI Mode, PerfectMode = false => Learn Mode
	//NotesSwitchPerfectAI = true => Show All Notes in the Perfect AI mode, NotesSwitchPerfectAI = false => Just show the possible Notes for the Perfect Ai Mode
	//FirstTurnSwitch = true => AI starts the game, FirstTurnSwitch = false => Player starts the Game
	public static bool PerfectMode;
	public static bool NotesSwitchPerfectAI;
	public static bool FirstTurnSwitch;

	//Enum for the two Modes
	public enum Mode
	{
		Perfect = 0,
		Learn = 1
	}

	// Start is called before the first frame update
	void Start()
    {
		NotesSwitchPerfectAI = false;
		FirstTurnSwitch = false;
		//Add on Click Function to buttons
		BTN_Exit.onClick.AddListener(() => GoBack());
		BTN_PerfectAI.onClick.AddListener(() => LoadMode(Mode.Perfect));
		BTN_Learn.onClick.AddListener(() => LoadMode(Mode.Learn));
		BTN_FirstTurn.onClick.AddListener(() => SwitchTurn());
		BTN_Tipps.onClick.AddListener(() => SwitchTipps());
	}

	//Load Main menu Scene
    private void GoBack(){
		SceneManager.LoadScene("MainMenu");
	}

	//Switch the sprite of the Button BTN_FirstTurn and Change the beginning player
	private void SwitchTurn(){
		FirstTurnSwitch = !FirstTurnSwitch;
		if(FirstTurnSwitch){
			BTN_FirstTurn.image.sprite = Sprite_FirstTurnPlayer;
			Sprite_FirstTurn = Sprite_FirstTurnPlayer;
			Sprite_Outline_FirstTurn = Sprite_FirstTurnPlayer_Outline;
		}
		else{
			BTN_FirstTurn.image.sprite = Sprite_FirstTurnKI;
			Sprite_FirstTurn = Sprite_FirstTurnKI;
			Sprite_Outline_FirstTurn = Sprite_FirstTurnKI_Outline;

		}
	}

	//Switch between show all notes or show just the possible notes
	private void SwitchTipps(){
		NotesSwitchPerfectAI = !NotesSwitchPerfectAI;
		if(NotesSwitchPerfectAI){
			BTN_Tipps.image.sprite = Sprite_On;
			Sprite_Tipps = Sprite_On;
			Sprite_Outline_Tipps = Sprite_On_Outline;
		}
		else{
			BTN_Tipps.image.sprite = Sprite_Off;
			Sprite_Tipps = Sprite_Off;
			Sprite_Outline_Tipps = Sprite_Off_Outline;
		}
	}

	//Load Game Scene
    private void LoadMode(Mode GameMode){
        if(GameMode == Mode.Learn){
			PerfectMode = false;
		}
        if(GameMode == Mode.Perfect){
			PerfectMode = true;
        }
		SceneManager.LoadScene("LearnGame");
	}
}
