using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum CollectableType
    {
        SmallPizzaToppin, BigPizzaToppin, Toppin, Treasure, Other
    }

    [Header("References")]
    [SerializeField] private GameObject destroyOnCollect;
    [SerializeField] private GameObject collectedEffect;
    [SerializeField] private CollectedScoreText scoreText;

    [field:Header("Variables")]
    [field: SerializeField] public CollectableType Type { get; private set; }
    [field: SerializeField] public int Score { get; private set; }

    [SerializeField] private float comboTime;

    private void Awake()
    {
        if (!destroyOnCollect) destroyOnCollect = gameObject;
        if (collectedEffect) collectedEffect.SetActive(false);
        if (scoreText) scoreText.SetScore(Score);
        gameObject.layer = LayerMask.NameToLayer("Collectable");
        gameObject.tag = "Collectable";
    }

    virtual public CollectableType Collect(PlayerBaseController controller, Rigidbody playerRB)
    {
        controller.player.Score.UpdateScore(Score);
        controller.player.Combo.AddTime(comboTime);

        if (collectedEffect)
        {
            collectedEffect.SetActive(true);
            collectedEffect.transform.parent = null;
            if (playerRB && collectedEffect.TryGetComponent(out DestroyedEffect destroyedEffectScript)) destroyedEffectScript.SetPlayer(playerRB);
        }

        if (scoreText) scoreText.Show(controller.player.Cam.transform, playerRB, transform.position);

        Destroy(destroyOnCollect);

        return Type;
    }
}
