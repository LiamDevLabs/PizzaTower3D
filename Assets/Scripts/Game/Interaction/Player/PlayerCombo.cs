using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombo : MonoBehaviour
{
    public int Combo { get; private set; }
    public int MaxCombo { get; private set; }
    public bool PerfectCombo { get; private set; }
    public bool FirstComboLost { get; private set; } = false;

    public bool pause = false;

    [SerializeField] private PlayerScore score;
    [SerializeField] private Slider timeBar;
    [SerializeField] private UI_HudReferences hudReferences;
    [SerializeField] private TMPro.TextMeshProUGUI comboUI;
    [SerializeField] private Animator combometerAnimator;
    [SerializeField] private float maxTime = 6.7f;
    private float time = 0;
    private bool reseted = true;

    private void Awake() => Initialize();

    private void OnLevelWasLoaded() => Initialize();

    public void Initialize()
    {
        Combo = 0;
        MaxCombo = int.MinValue;
        time = 0;
        timeBar.minValue = 0;
        timeBar.maxValue = maxTime;
        reseted = true;
        FirstComboLost = false;
        PerfectCombo = false;
        combometerAnimator.SetBool("Show", false);
        hudReferences.ChangeToGhost(true);
    }

    public void LooseFirstCombo()
    {
        FirstComboLost = true;
        PerfectCombo = false;
        hudReferences.ChangeToGhost(false);
    }

    public void AddCombo() => AddCombo(maxTime);
    
    public void AddCombo(float addedTime)
    {
        PerfectCombo = !FirstComboLost;
        score.UpdateScore(0);
        reseted = false;
        time += addedTime;
        if (time > maxTime) time = maxTime;
        Combo++;
        comboUI.text = "" + Combo;
        if (Combo > MaxCombo)
            MaxCombo = Combo;
        combometerAnimator.SetBool("Show", true);
    }

    public void AddTime(float addedTime)
    {
        if (Combo <= 0) return;
        time += addedTime;
        if (time > maxTime) time = maxTime;
    }

    public void RemoveTime(float removedTime)
    {
        time -= removedTime;
        if (time < 0) time = 0;
    }

    private void Update()
    {
        timeBar.value = time;

        if (time > 0) 
        {
            if (!pause) time -= Time.deltaTime; 
        }
        else if (!reseted)
            ResetCombo();
    }

    private void ResetCombo()
    {
        PerfectCombo = false;
        CalculateComboScore();
        time = 0;
        Combo = 0;
        comboUI.text = "" + Combo;
        reseted = true;
        LooseFirstCombo();
        combometerAnimator.SetBool("Show", false);
    }

    public void CalculateComboScore() => score.UpdateScore((int)(Combo * Combo * 0.25f) + 10 * Combo);
}
