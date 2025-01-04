using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;



public class UI_Health : MonoBehaviour
{
    int playerMaxHealth, bossMaxHealth;

    [SerializeField] private Spritesheet playerSprites;
    private Spritesheet bossSprites;

    [SerializeField] private GameObject playerContainer, bossContainer;
    Image[] playerHealthImages, bossHealthImages;

    [SerializeField] UnityEvent OnEnableEvent;
    [SerializeField] UnityEvent OnDisableEvent;

    [SerializeField] private bool animate;
    [SerializeField] private float timeBetweenFrames;

    private void Awake()
    {
        OnDisable();
    }

    public void SetBossUI(Spritesheet bossSprites, int maxHealth)
    {
        this.bossSprites = bossSprites;
        bossMaxHealth = maxHealth;
    }
    public void SetPlayerUI(int maxHealth)
    {
        playerMaxHealth = maxHealth;
    }

    private void OnLevelWasLoaded() => StartCoroutine("Intialize");

    IEnumerator Intialize()
    {
        bossMaxHealth = 0;
        yield return null;
        yield return null;
        if (bossMaxHealth <= 0) 
            OnDisable();
        else
        {
            OnEnable();
            playerHealthImages = CreateImages(playerSprites.sprites[0], playerContainer.transform, playerMaxHealth, "PlayerHealth");
            bossHealthImages = CreateImages(bossSprites.sprites[0], bossContainer.transform, bossMaxHealth, "BossHealth");
            StartCoroutine(Animation(playerHealthImages, playerSprites));
            StartCoroutine(Animation(bossHealthImages, bossSprites));
        }
    }

    Image[] CreateImages(Sprite sprite, Transform container, int maxHealth, string objectName = "Health")
    {
        Image[] images = new Image[maxHealth];
        RemoveChildren(container);
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject ImgGameObject = new GameObject(objectName + "-" + (i + 1));
            ImgGameObject.transform.parent = container;
            images[i] = ImgGameObject.AddComponent<Image>();
            images[i].sprite = sprite;
        }
        return images;
    }

    private void RemoveChildren(Transform container)
    {
        foreach (Transform child in container) Destroy(child.gameObject);
    }

    void UpdateImageArray(Image[] images, int currentHealth, int maxHealth)
    {
        for (int i = 0; i < maxHealth; i++)
        {
            //images[i].gameObject.SetActive(currentHealth > i);
            images[i].color = currentHealth > i ? new Color(255, 255, 255) : new Color(0, 0, 0);
        }
    }

    public void UpdatePlayerHealth(int currentHealth) => UpdateImageArray(playerHealthImages, currentHealth, playerMaxHealth);
    public void UpdateBossHealth(int currentHealth) => UpdateImageArray(bossHealthImages, currentHealth, bossMaxHealth);

    public IEnumerator Animation(Image[] images, Spritesheet spritesheet)
    {
        while (animate)
        {
            for (int i = 0; i < spritesheet.sprites.Length; i++)
            {
                for (int j = 0; j < images.Length; j++)
                {
                    images[j].sprite = spritesheet.sprites[i];
                }
                yield return new WaitForSeconds(timeBetweenFrames);
            }
        }
    }

    public void OnEnable() => OnEnableEvent.Invoke();

    public void OnDisable() => OnDisableEvent.Invoke();
}
