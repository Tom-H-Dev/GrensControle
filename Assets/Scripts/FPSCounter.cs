using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public float _fps;
    public TMP_Text _fpsText;

    private void Start()
    {
        InvokeRepeating(nameof(GetFPS), 1, 0.5f);
    }

    public void GetFPS()
    {
        _fps = (int)(1f / Time.unscaledDeltaTime);
        _fpsText.text = "FPS:  " + _fps;
    }
}