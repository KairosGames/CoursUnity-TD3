using UnityEngine;

public class Orbit : MonoBehaviour
{
    [SerializeField] public SystemManager manager;
    [SerializeField] GameObject nameCanva;
    [SerializeField] public Transform pivot;
    [SerializeField] public float angularSpeed = 20f;
    [SerializeField] public float selfRotationSpeed = 30f;
    [SerializeField] public bool isPlanet;

    Material material;

    [HideInInspector] public bool isRotating = true;
    [HideInInspector] public Transform cameraTransform;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        if (nameCanva.activeInHierarchy && cameraTransform != null)
            nameCanva.transform.LookAt(nameCanva.transform.position  + (nameCanva.transform.position - cameraTransform.position).normalized);

        if (!isRotating)
            return;

        if (pivot != null)
            transform.RotateAround(pivot.position, Vector3.up, angularSpeed * Time.deltaTime);

        transform.Rotate(Vector3.up, selfRotationSpeed * Time.deltaTime, Space.Self);
    }

    void OnMouseEnter()
    {
        material.EnableKeyword("_EMISSION");
        nameCanva.SetActive(true);
    }

    void OnMouseExit()
    {
        material.DisableKeyword("_EMISSION");
        nameCanva.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Orbit orbit = collision.gameObject.GetComponent<Orbit>();
        if (orbit != null)
        {
            if (!orbit.isPlanet)
                Destroy(orbit.gameObject);

            if (!isPlanet)
                Destroy(gameObject);
        }
    }
}
