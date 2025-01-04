using UnityEngine;
using UnityEngine.Events;

public class PizzaTimeManager : MonoBehaviour
{
    static public bool PizzaTime { get; private set; } = false;
    bool timeOver = false;

    public float Time { get; private set; }
    [field:SerializeField] public float LevelTime { get; private set; }
    [SerializeField] private Hitbox johnHitbox;
    [SerializeField] private GameObject pizzaFace;
    [SerializeField] private UnityEvent OnTimeEnabled;
    [SerializeField] private UnityEvent OnTimeOver;

    [SerializeField] private GameObject[] enableForPizzaTime;
    [SerializeField] private GameObject[] cloneForPizzaTime;
    GameObject[][] clonedLaps;
    [SerializeField] private int laps;

    bool paused;

    void Awake()
    {
        Time = 0;
        PizzaTime = false;
        timeOver = false;
        paused = false;
        johnHitbox.OnKill += JohnKilled;
        clonedLaps = new GameObject[laps][];
        for (int i=0; i < laps; i++) clonedLaps[i] = Clone(cloneForPizzaTime);
    }

    void JohnKilled()
    {
        PizzaTime = true;
        OnTimeEnabled.Invoke();
        JohnBlock.Toogle();
        EnableClones(1);
        EnableForPizzaTime();
    }

    public void TimeOver()
    {
        timeOver = true;
        pizzaFace.SetActive(true);
        OnTimeOver.Invoke();
        Time = LevelTime;
    }

    private void Update()
    {
        if (PizzaTime && !timeOver && !paused)
        {
            Time += UnityEngine.Time.deltaTime;
            if (Time > LevelTime)
                TimeOver();
        }
    }

    private GameObject[] Clone(GameObject[] objectsToClone)
    {
        int i = 0;
        GameObject[] clonedGameObjects = new GameObject[objectsToClone.Length];
        foreach(GameObject gameObject in objectsToClone)
        {
            GameObject clone = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent); clone.SetActive(false);
            clonedGameObjects[i] = clone;
            i++;
        }
        return clonedGameObjects;
    }

    public void EnableClones(int lap)
    {
        foreach (GameObject original in cloneForPizzaTime) original.SetActive(false); //desactiva los objetos originales que estan desde el comienzo del nivel
        foreach (GameObject clone in clonedLaps[lap - 1]) clone.SetActive(true); //activa los objetos de la lap actual
        if(lap > 1) foreach (GameObject clone in clonedLaps[lap - 2]) clone.SetActive(false); //desactiva los objetos de la lap anterior
    }

    private void EnableForPizzaTime() { foreach (GameObject gameObject in enableForPizzaTime) gameObject.SetActive(true); }

    private void OnLevelWasLoaded() => PizzaTime = false;

    public void Pause(bool pause)
    {
        paused = pause;
        pizzaFace.SetActive(!pause && timeOver);
    }
}
