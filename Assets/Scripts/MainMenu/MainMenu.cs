using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor;

public class MainMenu : MonoBehaviour
{

	//buttons for main menu
    public Button BTN_PlayGame;
    public Button BTN_Tutorial;
    public Button BTN_ResetLearn;
	public Button BTN_OpenCredits;
	public Button BTN_Quit;

	//Camera and Center of Map to rotate Camera around
	public Camera Camera;
	public GameObject CenterObject;
	public GameObject CreditsPopUp;


	//File name and Path to find file
	private static string fileName = "SaveAi.s";
	private static string path;

    // Start is called before the first frame update
    void Start()
    {
		//set timescale to 1 so the time of the game is normal
		Time.timeScale = 1f;
		//get path of save folder
    	path = Path.Combine(Application.dataPath, "Save");
		//add on click to buttons
		BTN_PlayGame.onClick.AddListener(() => PlayGame());
		BTN_Tutorial.onClick.AddListener(() => OpenTutorial());
		BTN_ResetLearn.onClick.AddListener(() => ResetFile());
		BTN_OpenCredits.onClick.AddListener(() => {
			CreditsPopUp.SetActive(true);
		});
		BTN_Quit.onClick.AddListener(() => QuitGame());
	}

	//rotate Camera around the center
	void Update(){
		Camera.transform.LookAt(CenterObject.transform.position);
		Camera.transform.Translate(Vector3.right * Time.deltaTime);
		if(Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0)){
			CreditsPopUp.SetActive(false);
		}
	}

    private void PlayGame(){
		SceneManager.LoadScene("ChooseMode");
    }

    private void OpenTutorial(){
		SceneManager.LoadScene("Tutorial");
    }

	//if file exists call reset function
    private void ResetFile(){
        if(File.Exists(Path.Combine(path, fileName))){
			//File.Delete(path);
			SaveGameManager.ResetData();
		}
    }

	//Close game
    private void QuitGame(){
		//UnityEditor.EditorApplication.isPlaying = false;
		Application.Quit();
	}

}