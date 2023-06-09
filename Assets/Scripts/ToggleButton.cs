using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ToggleButton : ButtonHover
{

	public Sprite[] SpritesDefault;

	public Boolean Activated;

	void Start(){
		Activated = false;
		BTN_Button.onClick.AddListener(() => {
				Activated = !Activated;
				UpdateSprite();
			});
		UpdateSprite();
	}

	override protected void UpdateSprite(){
		if(Activated){
			BTN_Button.image.sprite = Sprites[Hovered ? 1 : 0];
		}
		else{
			BTN_Button.image.sprite = SpritesDefault[Hovered ? 1 : 0];
		}
	}

}