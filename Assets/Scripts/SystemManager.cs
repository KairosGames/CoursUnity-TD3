using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] Transform localCamera;
    [SerializeField] Transform cameraTarget;
    [SerializeField] Transform rotator;
    [SerializeField] float cameraAngSpeed = 90.0f;

    [Header("Spawner")]
    [SerializeField] List<Orbit> orbitList = new List<Orbit>();
    [SerializeField] GameObject planetPrefab;
    [SerializeField] Transform spawnerParent;

    [Header("Tooltip")]
    [SerializeField] public PlanetTooltip tooltip;

    bool areRotating = true;

    private void Awake()
    {
        foreach (var orbit in orbitList)
            orbit.cameraTransform = localCamera;
    }

    void Update()
    {
        HandleCameraRotation();
        HandlePlanetsRotation();
        HandleCameraDistance();
        if (Input.GetMouseButtonDown(0))
            SpawnAsteroid();
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    void HandleCameraRotation()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Rotate(Vector3.up, cameraAngSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.RightArrow))
            transform.Rotate(Vector3.up, -cameraAngSpeed * Time.deltaTime);
        
        if (Input.GetKey(KeyCode.UpArrow))
            rotator.Rotate(Vector3.right, cameraAngSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.DownArrow))
            rotator.Rotate(Vector3.right, -cameraAngSpeed * Time.deltaTime);

        if (Vector3.Distance(localCamera.localPosition, cameraTarget.localPosition) > 0.1f)
            localCamera.localPosition = Vector3.Lerp(localCamera.localPosition, cameraTarget.localPosition, 0.03f);
    }

    void HandleCameraDistance()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0)
        {
            float dist = Vector3.Distance(cameraTarget.position, transform.position) - scroll * 2.5f;
            dist = Mathf.Clamp(dist, 10.0f, 80.0f);
            cameraTarget.position = transform.position + ((cameraTarget.position - transform.position).normalized * dist);
        }
    }

    void HandlePlanetsRotation()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            areRotating = !areRotating;
            foreach (Orbit orbit in orbitList)
                orbit.isRotating = !orbit.isRotating;
        }
    }

    void SpawnAsteroid()
    {
        GameObject newAst = Instantiate(planetPrefab, spawnerParent);
        Vector2 rdnVec2 = Random.insideUnitCircle * 45.0f;
        newAst.transform.position = new Vector3(rdnVec2.x, 0.0f, rdnVec2.y);
        newAst.transform.position += (newAst.transform.position - transform.position).normalized * 5.0f;
        Orbit newOrbit = newAst.GetComponent<Orbit>();
        newOrbit.pivot = transform;
        newOrbit.angularSpeed = Random.Range(10.0f, 90.0f);
        newOrbit.selfRotationSpeed = Random.Range(30.0f, 180.0f);
        newOrbit.isRotating = areRotating;
        newOrbit.cameraTransform = localCamera;
        newOrbit.manager = this;
        orbitList.Add(newOrbit);
    }

    public void DestroyOrbit(Orbit orbit)
    {
        if (orbit == null)
            return;

        if (orbitList.Remove(orbit))
        {
            orbit.meshRenderer.enabled = false;
            orbit.isDestroyed = true;
            orbit.destroyDelayed.AutoDestroy(1.2f);
        }
    }

    void OnMouseEnter()
    {
        tooltip.LoadDataAndActivate("Sun", 0.0f);
    }

    void OnMouseExit()
    {
        tooltip.AutoDisable();
    }
}
