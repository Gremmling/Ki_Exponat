using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ButtonHover : MonoBehaviour
{


	public Button BTN_Button;

	public Sprite[] Sprites;
	protected Boolean Hovered;
	public Boolean smaller;


	public Vector2 whileHoverVector;
	public Vector2 bevorHoverVector;


	virtual protected void UpdateSprite(){
		BTN_Button.image.sprite = Sprites[Hovered ? 1 : 0];
	}

    void OnMouseEnter(){
		Hovered = true;
		UpdateSprite();
		if(smaller){
			BTN_Button.GetComponent<RectTransform>().sizeDelta = whileHoverVector;
			BTN_Button.GetComponent<BoxCollider>().size = whileHoverVector;
		}
	}

    void OnMouseExit(){
		Hovered = false;
		UpdateSprite();
		if(smaller){
	    	BTN_Button.GetComponent<RectTransform>().sizeDelta = bevorHoverVector;
			BTN_Button.GetComponent<BoxCollider>().size = bevorHoverVector;
	    }
    }
}
