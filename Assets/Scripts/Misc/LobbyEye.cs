using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyEye : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string levelName;
    [SerializeField] private int id;

    void Start()
    {
        int value = PlayerPrefs.GetInt(levelName + "_Secret_" + id, 0);
        animator.SetBool("Open", value == 1);
    }


}
