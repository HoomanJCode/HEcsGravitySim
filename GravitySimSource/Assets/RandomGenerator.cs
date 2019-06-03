using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenerator : MonoBehaviour
{
    [SerializeField]
    Vector2 x, y, z, scale,mass;
    [SerializeField]
    int HowMany = 100;
    [SerializeField]
    GameObject prefab;
    Transform temp;
    float tempScale;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < HowMany; i++)
        {

            temp=Instantiate(prefab, new Vector3(Random.Range(x.x, x.y)
                                        , Random.Range(y.x, y.y)
                                        , Random.Range(z.x, z.y)), prefab.transform.rotation).transform;
            tempScale = Random.Range(scale.x, scale.y);
            temp.localScale = new Vector3(tempScale, tempScale, tempScale);
            temp.GetComponent<Rigidbody>().mass = Random.Range(mass.x, mass.y);
        }
    }
}