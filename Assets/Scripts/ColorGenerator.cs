﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator : MonoBehaviour
{

    public Color selectedColor;
    public Color interactableColor;
     
    void Start()
    {
        RecolorWorld();
        Material interactableMaterial = (Material)Resources.Load("Materials/Prototyping/Interactable", typeof(Material));
        interactableColor = interactableMaterial.color;
        Material selectedMaterial = (Material)Resources.Load("Materials/Prototyping/Selected", typeof(Material));
        selectedColor = selectedMaterial.color;
    }

    /// <summary>
    /// Naive procedural coloring of stuff (intial random color, offset, and complement)
    /// </summary>
    public void RecolorWorld()
    {
        //Wall = Random Base
        Color randomWallColorHSV = Random.ColorHSV(0f, 1f, 0.45f, 0.45f, 1.0f, 1.0f, 0.30f, 0.35f);
        Color randomWallColorRGB = new Color(randomWallColorHSV.r, randomWallColorHSV.g, randomWallColorHSV.b, randomWallColorHSV.a);
        float wallHue, wallSat, wallVal;
        Color.RGBToHSV(randomWallColorRGB, out wallHue, out wallSat, out wallVal);
        Material wallMaterial = (Material)Resources.Load("Materials/Prototyping/Wall", typeof(Material));
        wallMaterial.SetColor("_Color", randomWallColorRGB);

        //Interactable = Offset Hue of Wall
        float colorOffset = 25f;
        float matchWall = (wallHue * 360) >= 360 - colorOffset ? (wallHue * 360 - colorOffset) / 360 : (wallHue * 360 + colorOffset) / 360;
        Color randomInteractableColorHSV = Random.ColorHSV(matchWall, matchWall, 0.83f, 0.83f, 1.0f, 1.0f, 0.85f, 0.87f);
        Color randomInteractableColorRGB = new Color(randomInteractableColorHSV.r, randomInteractableColorHSV.g, randomInteractableColorHSV.b, randomInteractableColorHSV.a);
        float interactableHue, interactableSat, interactableVal;
        Color.RGBToHSV(randomInteractableColorRGB, out interactableHue, out interactableSat, out interactableVal);
        Material interactableMaterial = (Material)Resources.Load("Materials/Prototyping/Interactable", typeof(Material));
        interactableMaterial.SetColor("_Color", new Color(randomInteractableColorHSV.r, randomInteractableColorHSV.g, randomInteractableColorHSV.b, randomInteractableColorHSV.a));

        //Interactable-No-Decay = Offset Interactable
        float noDecayColorOffset = 35f;
        float matchInteractable = (interactableHue * 360) >= 360 - noDecayColorOffset ? (interactableHue * 360 - noDecayColorOffset) / 360 : (interactableHue * 360 + noDecayColorOffset) / 360;
        Color randomInteractableNoDecayColorHSV = Random.ColorHSV(matchInteractable, matchInteractable, 0.83f, 0.83f, 1.0f, 1.0f, 0.85f, 0.87f);
        Material interactableNoDecayMaterial = (Material)Resources.Load("Materials/Prototyping/Interactable-No-Decay", typeof(Material));
        interactableNoDecayMaterial.SetColor("_Color", new Color(randomInteractableNoDecayColorHSV.r, randomInteractableNoDecayColorHSV.g, randomInteractableNoDecayColorHSV.b, randomInteractableNoDecayColorHSV.a));

        //Selection = Complement of Interactable
        float complementInteractable = (interactableHue * 360) >= 180 ? (interactableHue * 360 - 180) / 360 : (interactableHue * 360 + 180) / 360;
        Color randomSelectedColorHSV = Random.ColorHSV(complementInteractable, complementInteractable, 0.70f, 0.70f, 1.0f, 1.0f, 0.70f, 0.70f);
        //Color randomSelectedColorRGB = new Color(randomSelectedColorHSV.r, randomSelectedColorHSV.g, randomSelectedColorHSV.b, randomSelectedColorHSV.a);
        float selectedHue, selectedSat, selectedVal;
        Color.RGBToHSV(randomInteractableColorRGB, out selectedHue, out selectedSat, out selectedVal);
        Material selectedMaterial = (Material)Resources.Load("Materials/Prototyping/Selected", typeof(Material));
        selectedMaterial.SetColor("_Color", new Color(randomSelectedColorHSV.r, randomSelectedColorHSV.g, randomSelectedColorHSV.b, randomSelectedColorHSV.a));
    }
}