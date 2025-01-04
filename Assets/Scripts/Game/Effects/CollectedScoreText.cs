using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedScoreText : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private TMPro.TextMeshPro text;
    [SerializeField] private GameObject effectOnDissapear;

    [Header("Config")]

    //Time
    float time = 0;
    [SerializeField] private float duration;

    //Font size
    float startFontSize = 0;
    [SerializeField] private float minFontSize;

    //Rigidbody
    [SerializeField] private float upSpeed;


    private void Awake() => gameObject.SetActive(false);
    public void SetScore(int score) => text.text = ""+score;
    public void Show(Transform cam, Rigidbody playerRB, Vector3 position)
    {
        transform.SetParent(null, true);
        transform.position = position;
        gameObject.SetActive(true);
        transform.LookAt(cam);
        transform.Rotate(0, 180, 0);
        rb.velocity = new Vector3(playerRB.velocity.x, playerRB.velocity.y + upSpeed, playerRB.velocity.z);
    }

    private void OnEnable()
    {
        TryGetComponent(out rb);
        if (effectOnDissapear) effectOnDissapear.SetActive(false);
        startFontSize = text.fontSize;
        time = 0;
    }

    void Update()
    {
        time += Time.deltaTime;
        text.fontSize = Mathf.Lerp(startFontSize, minFontSize, time / duration);
        if (time > duration)
        {
            if (effectOnDissapear)
            {
                effectOnDissapear.transform.parent = null;
                effectOnDissapear.SetActive(true);
            }
            Destroy(gameObject);
        }
    }
}
