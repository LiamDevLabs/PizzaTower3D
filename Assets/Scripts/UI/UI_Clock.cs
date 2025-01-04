using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Clock : MonoBehaviour
{
    [SerializeField] private Image clockFillBar;

    public void UpdateTime(float currentTime, float maxTime) => clockFillBar.fillAmount = 1 - currentTime / maxTime;
}
