using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PizzaTime : MonoBehaviour
{
    [SerializeField] private AnimatorNamedParameters animator;
    [SerializeField] private Slider slider;
    private PizzaTimeManager pizzaTimeManager;

    private bool started=false, ended=false;

    // Start is called before the first frame update
    void Start() => Initialize();

    private void OnLevelWasLoaded() => Initialize();

    void Initialize()
    {
        pizzaTimeManager = FindObjectOfType<PizzaTimeManager>();
        if (!pizzaTimeManager) return;

        started = false;
        ended = false;
        animator.SetString("Reset");
        slider.maxValue = pizzaTimeManager.LevelTime;
        slider.value = pizzaTimeManager.Time;
    }

    void Update()
    {
        if (!pizzaTimeManager) return;
        if (pizzaTimeManager && pizzaTimeManager.Time > 0 && !ended)
        {
            slider.value = pizzaTimeManager.Time;

            if (!started)
            {
                animator.SetString("Start");
                started = true;
            }

            if (pizzaTimeManager.Time >= pizzaTimeManager.LevelTime)
            {
                animator.SetString("End");
                ended = true;
            }
        }
    }

    public void UpdateHud() => started = false;
}
