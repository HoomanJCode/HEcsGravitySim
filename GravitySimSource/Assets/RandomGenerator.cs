using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenerator : MonoBehaviour
{
    [SerializeField]
    private Vector2 x = Vector2.one, y = Vector2.one, z = Vector2.one, scale = Vector2.one, mass = Vector2.one;
    [SerializeField]
    private int HowMany = 100;
    [SerializeField]
    private GameObject prefab=null;
    private Transform temp;
    private float tempScale;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < HowMany; i++)
        {

            temp = Instantiate(prefab, new Vector3(Random.Range(x.x, x.y)
                                        , Random.Range(y.x, y.y)
                                        , Random.Range(z.x, z.y)), prefab.transform.rotation).transform;
            tempScale = Random.Range(scale.x, scale.y);
            temp.localScale = new Vector3(tempScale, tempScale, tempScale);
            temp.GetComponent<Rigidbody>().mass = Random.Range(mass.x, mass.y);
        }
    }
}