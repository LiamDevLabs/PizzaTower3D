using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    enum TrailBy
    {
        MachRun, Velocity, HorizontalSpeed, Always
    }


    [SerializeField] private PeppinoController player;
    [SerializeField] private GameObject target;
    [SerializeField] private TrailBy trailBy;
    [SerializeField] private float trailByMinValue;
    [SerializeField] private Material[] trailMaterials;
    [SerializeField] private float trailRate;
    [SerializeField] private int maxTrails;
    float lastTrail = 0;

    private Queue<GameObject> trailList = new Queue<GameObject>();

    bool isTrail = false;
    bool trailDestroyed = false;

    // Update is called once per frame
    void Update()
    {
        switch (trailBy)
        {
            case TrailBy.MachRun:
                if (player.CurrentMachRun >= trailByMinValue) Trail(); else isTrail = false; break;
            case TrailBy.HorizontalSpeed:
                if (player.CurrentSpeed >= trailByMinValue) Trail(); else isTrail = false; break;
            case TrailBy.Velocity:
                if (player.Rigidbody.velocity.magnitude >= trailByMinValue) Trail(); else isTrail = false; break;
            case TrailBy.Always:
                Trail(); break;
        }

        if(!isTrail && !trailDestroyed)
        {
            foreach (GameObject trail in trailList) Destroy(trail);
            trailList.Clear();
            trailDestroyed = true;
        }
    }

    private void Trail()
    {
        isTrail = true;
        trailDestroyed = false;

        if (Time.time > lastTrail + trailRate)
        {
            //Trail gameobject
            GameObject trail = Instantiate(target, target.transform.position, target.transform.rotation);
            trail.name = "Trail";
            trail.layer = LayerMask.NameToLayer("Ignore Raycast");
            trail.transform.localScale = target.transform.lossyScale;
            Destroy(trail.GetComponent<Animator>());
            //Destroy TV Cam
            foreach (Camera camera in trail.GetComponentsInChildren<Camera>())
                Destroy(camera.gameObject);
            //Materials
            Material material = trailMaterials[Random.Range(0, trailMaterials.Length)];
            foreach (Renderer renderer in trail.GetComponentsInChildren<Renderer>())
                renderer.material = material;
            //Add to list
            trailList.Enqueue(trail);
            //Timer
            lastTrail = Time.time;
        }

        //Remove trail
        if(trailList.Count > maxTrails)
        {
            Destroy(trailList.Dequeue());
        }
    }


}
