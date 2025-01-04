using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Countdown : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] int count = 3;
    [SerializeField] string ready = "¡Ya!";
    private int startCount;
    public bool Ended { get; private set; } = false ;

    private void Awake()
    {
        startCount = count;
    }

    private void OnEnable()
    {
        text.text = "" + count;
    }

    public void Rest()
    {
        count--;
        UpdateText();
    }


    void UpdateText()
    {
        if (count <= 0)
        {
            text.text = ready;
            count = startCount;
            Ended = true;
            StartCoroutine(ResetEndedFlag());
            return;
        }

        text.text = "" + count;
    }

    private IEnumerator ResetEndedFlag()
    {
        yield return null;
        Ended = false;
    }
}
