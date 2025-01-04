using UnityEngine;

public abstract class PlayerBaseController : MonoBehaviour 
{
    [HideInInspector] public Player player;
    [SerializeField] protected bool enablePlayerCam = false;
    [SerializeField] protected Transform camTransform;

    [field: SerializeField] public PlayerHealth Health { get; protected set; }

    //[HideInInspector] public PlayerBaseController powerup;

    public PlayerBaseController(Player player)
    {
        this.player = player;
    }

    virtual protected void Start()
    {
        SetCam();
    }

    private void SetCam()
    {
        if (enablePlayerCam)
        {
            player.Cam.enabled = true;

            if (camTransform != null)
            {
                player.Cam.transform.parent = camTransform;
                player.Cam.transform.localPosition = Vector3.zero;
                player.Cam.transform.localEulerAngles= Vector3.zero;
            }

        }
        else
        {
            player.Cam.enabled = false;
            player.Cam.transform.parent = player.transform;
            player.Cam.transform.localPosition = Vector3.zero;
            player.Cam.transform.localEulerAngles = Vector3.zero;
        }
    }

    protected abstract void SetSettings();

    public abstract void PlayerStart();

    public abstract void PlayerUpdate();

    public abstract void PlayerFixedUpdate();

    public abstract void PlayerLateUpdate();

    public virtual void GameOver(bool win)
    {
        player.enabled = false;
    }
}
