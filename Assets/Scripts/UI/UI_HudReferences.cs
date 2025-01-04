using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_HudReferences : MonoBehaviour
{
    [System.Serializable]
    public class SpritesHud
    {
        public Sprite pizza;
        public Sprite[] ranks;
        public Sprite tv_frame;
        public Sprite combo_handle_purple;
        public Sprite combo_handle_yellow;
        public Sprite combo_frame;
        public TMP_FontAsset combo_font;
        public TMP_FontAsset score_font;
        public Sprite bar_frame;
        public Sprite bar_fill;
        public float bar_fill_pixelPerUnitMultiplier;
        public Vector2 bar_pizzaFaceSize;
        public RuntimeAnimatorController bar_controller;
    }

    [Header("HUD References")]
    [SerializeField] private Image pizza;
    [SerializeField] private Image rank;
    [SerializeField] private Image tv_frame;
    [SerializeField] private Image combo_handle;
    [SerializeField] private Image combo_frame;
    [SerializeField] private TextMeshProUGUI combo_text;
    [SerializeField] private TextMeshProUGUI score_text;
    [SerializeField] private Image bar_frame;
    [SerializeField] private Image bar_fill;
    [SerializeField] private RectTransform bar_pizzaFace;
    [SerializeField] private Animator bar_animator;

    [Header("Other References")]
    [SerializeField] PlayerScore score;
    [SerializeField] PlayerCombo combo;
    [SerializeField] UI_PizzaTime pizzatime;


    [Header("HUD Sprites Types")]
    [SerializeField] private int enabledHudIndex;
    [SerializeField] private SpritesHud[] hudTypes;

    private void Awake()
    {
        if (enabledHudIndex >= hudTypes.Length || enabledHudIndex < 0) 
            enabledHudIndex = 0;
    }

    public void ChangeHud(int index)
    {
        enabledHudIndex = index;
        UpdateHud();
    }

    public void UpdateHud()
    {
        pizza.sprite = hudTypes[enabledHudIndex].pizza;
        tv_frame.sprite = hudTypes[enabledHudIndex].tv_frame;
        combo_frame.sprite = hudTypes[enabledHudIndex].combo_frame;
        combo_text.font = hudTypes[enabledHudIndex].combo_font;
        score_text.font = hudTypes[enabledHudIndex].score_font;
        ChangeToGhost(!combo.FirstComboLost);
        score.UpdateScore(0); //Adentro de esta funcion se usa el "ChangeToRank()"

        bar_frame.sprite = hudTypes[enabledHudIndex].bar_frame;
        bar_fill.sprite = hudTypes[enabledHudIndex].bar_fill;
        bar_fill.pixelsPerUnitMultiplier = hudTypes[enabledHudIndex].bar_fill_pixelPerUnitMultiplier;
        bar_pizzaFace.sizeDelta = hudTypes[enabledHudIndex].bar_pizzaFaceSize;
        bar_animator.runtimeAnimatorController = hudTypes[enabledHudIndex].bar_controller;
        pizzatime.UpdateHud();
    }

    public void ChangeToRank(string rankLetter)
    {
        switch (rankLetter.ToLower())
        {
            case "d":
                rank.sprite = hudTypes[enabledHudIndex].ranks[0];
                break;
            case "c":
                rank.sprite = hudTypes[enabledHudIndex].ranks[1];
                break;
            case "b":
                rank.sprite = hudTypes[enabledHudIndex].ranks[2];
                break;
            case "a":
                rank.sprite = hudTypes[enabledHudIndex].ranks[3];
                break;
            case "s":
                rank.sprite = hudTypes[enabledHudIndex].ranks[4];
                break;
            case "p":
                rank.sprite = hudTypes[enabledHudIndex].ranks[5];
                break;
            case "p+":
                rank.sprite = hudTypes[enabledHudIndex].ranks[6];
                break;
        }
    }

    public void ChangeToGhost(bool purple)
    {
        combo_handle.sprite = purple ? hudTypes[enabledHudIndex].combo_handle_purple : hudTypes[enabledHudIndex].combo_handle_yellow;
    }

}
