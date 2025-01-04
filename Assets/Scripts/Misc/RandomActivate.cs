using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomActivate : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjectsToSelect;
    [SerializeField] private bool activateSelected = true;

    private void Start()
    {
        int selectedIndex = Random.Range(0, gameObjectsToSelect.Length);
        for(int i=0; i<gameObjectsToSelect.Length; i++)
        {
            if (selectedIndex == i)
                gameObjectsToSelect[i].SetActive(activateSelected);
            else
                gameObjectsToSelect[i].SetActive(!activateSelected);
        }
    }
}
