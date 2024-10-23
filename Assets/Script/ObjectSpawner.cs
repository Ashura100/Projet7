using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private Transform _cubeSpawner;
    [SerializeField] private Transform _sphereSpawner;
    [SerializeField] private Transform _capsuleSpawner;

    public void DebugSpawner(string type)
    {
        if (type == "Cube")
        {
            Debug.Log("Spawn du cube");
        }
        else if (type == "Sphere")
        {
            Debug.Log("Spawn de la sphère");
        }
        else if (type == "Capsule")
        {
            Debug.Log("Spawn de la capsule");
        }
    }

    public void SpawnObjects(string type)
    {
        if (type == "Cube")
        {
            Debug.Log("Spawn du cube");
            var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.AddComponent<Rigidbody>();
            obj.AddComponent<BoxCollider>();
            obj.AddComponent<XRGrabInteractable>();
            Instantiate(obj);
            obj.transform.position = _cubeSpawner.position;
        }
        else if (type == "Sphere")
        {
            Debug.Log("Spawn de la sphère");
            var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj.AddComponent<Rigidbody>();
            obj.AddComponent<SphereCollider>();
            obj.AddComponent<XRGrabInteractable>();
            Instantiate(obj);
            obj.transform.position = _sphereSpawner.position;
        }
        else if (type == "Capsule")
        {
            Debug.Log("Spawn de la capsule");
            var obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            obj.AddComponent<Rigidbody>();
            obj.AddComponent<CapsuleCollider>();
            obj.AddComponent<XRGrabInteractable>();
            Instantiate(obj);
            obj.transform.position = _capsuleSpawner.position;
        }
    }
}
