using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Menu_Select : MonoBehaviour
{
    [SerializeField] private Button selectOnEnable;
    [SerializeField] private Button selectOnDisable;


    private void OnEnable()
    {
        if(selectOnEnable)
            selectOnEnable.Select();
    }
    private void OnDisable()
    {
        if (selectOnDisable)
            selectOnDisable.Select();
    }
}
