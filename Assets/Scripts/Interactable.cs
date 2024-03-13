using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    MeshRenderer _meshRenderer;
    public Material _highlightMaterial;
    private List<Material> _oldMaterials = new List<Material>(); // Initialize the list
    private List<Material> _newMaterials = new List<Material>(); // Initialize the list

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _oldMaterials.AddRange(_meshRenderer.materials); // Add all materials to oldMaterials
    }

    private void OnMouseEnter()
    {
        print("hover highlight");
        _newMaterials.Clear();
        _newMaterials.AddRange(_oldMaterials);
        _newMaterials.Add(_highlightMaterial);
        ApplyMaterials(_newMaterials);
    }

    private void OnMouseExit()
    {
        ApplyMaterials(_oldMaterials);
    }

    private void ApplyMaterials(List<Material> materials)
    {
        _meshRenderer.materials = materials.ToArray();
    }
}
