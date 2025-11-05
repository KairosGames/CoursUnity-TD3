using UnityEngine;

public class Orbit : MonoBehaviour
{
    [SerializeField] public SystemManager manager;
    [SerializeField] public Transform pivot;
    [SerializeField] public float angularSpeed = 20f;
    [SerializeField] public float selfRotationSpeed = 30f;
    [SerializeField] public bool isPlanet;
    [SerializeField] public string planetName;

    [HideInInspector] public Transform cameraTransform;
    [HideInInspector] public MeshRenderer meshRenderer;
    [HideInInspector] public AutoDestruction destroyDelayed;
    [HideInInspector] public bool isRotating = true;
    [HideInInspector] public bool isDestroyed = false;


    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        destroyDelayed = GetComponent<AutoDestruction>();
    }

    void Update()
    {
        if (!isRotating || isDestroyed)
            return;

        if (pivot != null)
            transform.RotateAround(pivot.position, Vector3.up, angularSpeed * Time.deltaTime);

        transform.Rotate(Vector3.up, selfRotationSpeed * Time.deltaTime, Space.Self);
    }

    void OnMouseEnter()
    {
        meshRenderer.material.EnableKeyword("_EMISSION");
        manager.tooltip.LoadDataAndActivate(planetName, Vector3.Distance(transform.position, pivot.position));
    }

    void OnMouseExit()
    {
        meshRenderer.material.DisableKeyword("_EMISSION");
        manager.tooltip.AutoDisable();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Orbit orbit = collision.gameObject.GetComponent<Orbit>();
        if (orbit != null)
        {
            if (!orbit.isPlanet)
                manager.DestroyOrbit(orbit);

            if (!isPlanet)
                manager.DestroyOrbit(this);
        }
    }
}
