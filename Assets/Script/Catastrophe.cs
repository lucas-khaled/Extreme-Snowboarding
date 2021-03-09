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

    private Vector3 nextMovementPoint;
    private bool isMoving;
    private GameObject mainCamera;
    private LayerMask layerMask;

    void Start()
    {
        layerMask = LayerMask.GetMask("Track");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        isMoving = false;
        StartCoroutine(CatastropheStartTimer());
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
        StartCoroutine(CameraShake());
        // Aviso?

        GetNextMovementPoint();
    }

    IEnumerator CameraShake()
    {
        if (!deactivateCameraShake)
        {
            Vector3 originalPoisition = mainCamera.transform.localPosition;

            float timeElapsed = 0f;

            while (timeElapsed < shakingDuration)
            {
                float x = Random.Range(-1f, 1f) * magnetude;
                float y = Random.Range(-1f, 1f) * magnetude;

                mainCamera.transform.localPosition = new Vector3(x + originalPoisition.x, 
                                                 y + originalPoisition.y,
                                                 originalPoisition.z);

                timeElapsed += Time.deltaTime;

                yield return null;
            }

            mainCamera.transform.localPosition = originalPoisition;
        }
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
