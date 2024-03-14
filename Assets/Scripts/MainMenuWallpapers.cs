using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuWallpapers : MonoBehaviour
{
    public List<Sprite> _images;
    public Image _background;

    private void Start()
    {
        StartCoroutine(RandomizeWallpapaers());
    }

    private IEnumerator RandomizeWallpapaers()
    {
        Debug.Log("New wallpaper");
        _background.sprite = _images[Random.Range(0, _images.Count)];
        yield return new WaitForSeconds(30f);
        StartCoroutine(RandomizeWallpapaers());
    }
}
