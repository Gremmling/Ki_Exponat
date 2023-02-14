using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ButtonHoverTwoDesigns : MonoBehaviour
{

	public Button BTN_Button;

	public Boolean smaller;
	public Boolean isChooseMode;
	public Boolean isTipps;


	public Vector2 whileHoverVector;
	public Vector2 bevorHoverVector;

	public ChooseMode CH;
	public UI UI;




	void OnMouseEnter()
	{
		if (isChooseMode)
		{
			if (isTipps)
				BTN_Button.image.sprite = CH.Sprite_Outline_Tipps;
			else
				BTN_Button.image.sprite = CH.Sprite_Outline_FirstTurn;
		}
		else
		{
			BTN_Button.image.sprite = UI.Sprite_Outline_Learn;
		}
		if (smaller)
		{
			BTN_Button.GetComponent<RectTransform>().sizeDelta = whileHoverVector;
			BTN_Button.GetComponent<BoxCollider>().size = whileHoverVector;
		}
	}

	void OnMouseExit()
	{
		if (isChooseMode)
		{
			if (isTipps)
				BTN_Button.image.sprite = CH.Sprite_Tipps;
			else
				BTN_Button.image.sprite = CH.Sprite_FirstTurn;
		}
		else
		{
			BTN_Button.image.sprite = UI.Sprite_Learn;
		}
		if (smaller)
		{
			BTN_Button.GetComponent<RectTransform>().sizeDelta = bevorHoverVector;
			BTN_Button.GetComponent<BoxCollider>().size = bevorHoverVector;
		}
	}
}
