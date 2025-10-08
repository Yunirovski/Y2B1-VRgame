using UnityEngine;

public class PlateBehavior : MonoBehaviour
{
    private GameObject Plate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Plate = Resources.Load<GameObject>("Prefabs/Paper_Plate_Brown_Goob_Mold_empty");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        Instantiate(Plate, new Vector3(1.67858005f, 1.03970003f, -5.71996021f), Quaternion.identity);
    }
}
