using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catastrophe : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField]
    private bool deactivateCameraShake;

    [Header("")]
    [SerializeField]    
    private float velocity;
    [SerializeField]
    private float shakingDuration;
    [SerializeField]
    private float magnetude;
    [SerializeField]
    private float tempoEspera;

    private GameCamera[] cameraScript;
    private Vector3 nextMovementPoint;
    private bool isMoving;
    private LayerMask layerMask;

    void Start()
    {
        layerMask = LayerMask.GetMask("Track");
        isMoving = false;
        StartCoroutine(CatastropheStartTimer());
        cameraScript = GameObject.FindGameObjectWithTag("CorridaController").GetComponent<CorridaController>().cameras;
    }

    void Update()
    {
        Movement();
    }

    IEnumerator CatastropheStartTimer()
    {
        yield return new WaitForSeconds(tempoEspera);
        isMoving = true;
        StartCatastrophe();
    }

    void StartCatastrophe()
    {
        // Animação catastrophe iniciando
        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<SphereCollider>().enabled = true;
        cameraScript[0].StartCoroutine(cameraScript[0].CameraShake(deactivateCameraShake, shakingDuration, magnetude));
        cameraScript[1].StartCoroutine(cameraScript[1].CameraShake(deactivateCameraShake, shakingDuration, magnetude));
        cameraScript[2].StartCoroutine(cameraScript[2].CameraShake(deactivateCameraShake, shakingDuration, magnetude));
        cameraScript[3].StartCoroutine(cameraScript[3].CameraShake(deactivateCameraShake, shakingDuration, magnetude));
        // Aviso?

        GetNextMovementPoint();
    }

    Vector3 GetNextMovementPoint()
    {
        Vector3 newPoint = new Vector3(this.transform.position.x + 5f, 
                                       this.transform.position.y, 
                                       this.transform.position.z);

        RaycastHit hit;

        if (Physics.Raycast(newPoint, transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity, layerMask))
            newPoint = hit.point;
        else if (Physics.Raycast(newPoint, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity, layerMask))
            newPoint = hit.point;

        return newPoint;
    }

    void Movement()
    {
        if (isMoving)
        {
            if (Vector3.Distance(this.transform.position, nextMovementPoint) <= 0.01f)
                nextMovementPoint = GetNextMovementPoint();

            this.transform.position = Vector3.MoveTowards(this.transform.position,     // Posicao inicial 
                                                          nextMovementPoint,           // Posicao destino
                                                          velocity * Time.deltaTime);  // Velocidade movimento
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().ChangeState(new Dead());
        }
    }
}
