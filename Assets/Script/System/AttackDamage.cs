using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamage : MonoBehaviour
{
    public LayerMask layer;
    public float radius = 0.2f;
    public float damage = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layer);
        if (hits.Length > 0)
        {
            
            gameObject.SetActive(false);
            foreach(Collider hit in hits)
            {
                print(GetRootParentName(gameObject) + " hit " + layer.value + " name "+hit.name);
                hit.GetComponent<HealthScript>().ApplyDamage(damage);
            }
            
        }
    }
    string GetRootParentName(GameObject obj)
    {
        Transform currentTransform = obj.transform;

        while (currentTransform.parent != null)
        {
            currentTransform = currentTransform.parent;
        }

        return currentTransform.name;
    }
}
