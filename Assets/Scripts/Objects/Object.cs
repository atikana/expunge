using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    [SerializeField] ObjectType objectType;
    [SerializeField] Material activeMat;
    [SerializeField] Material inactiveMat;

    public enum ObjectType
    {
        Active,
        Inactive,
       
    }

    private void Awake()
    {
        if (objectType == ObjectType.Active)
        {
            ChangeMaterial(activeMat);
        }
        else
        {
            ChangeMaterial(inactiveMat);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ObjectType GetObject()
    {
        return objectType;
    }

    public void ChangeObjectType()
    {
        if (objectType == ObjectType.Active)
        {
            objectType = ObjectType.Inactive;
            ChangeMaterial(inactiveMat);
        }
        else
        {
            objectType = ObjectType.Active;
            ChangeMaterial(activeMat);
        }

    }

    private void ChangeMaterial(Material mat)
    {
        Material[] mats = GetComponent<MeshRenderer>().materials;

        mats[0] = mat;
       
        GetComponent<MeshRenderer>().materials = mats;
    }
}
