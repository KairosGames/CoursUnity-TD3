using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlanetTooltip : MonoBehaviour
{
    [SerializeField] RectTransform toolTip;

    [Header("components")]
    [SerializeField] TextMeshProUGUI planetName;
    [SerializeField] TextMeshProUGUI distance;

    void Update()
    {
        toolTip.position = Input.mousePosition;
    }

    public void AutoDisable()
    {
        gameObject.SetActive(false);
    }

    public void LoadDataAndActivate(string name, float dist)
    {
        planetName.text = name;
        distance.text = Mathf.RoundToInt(dist).ToString() + "m";
        gameObject.SetActive(true);
    }
}
