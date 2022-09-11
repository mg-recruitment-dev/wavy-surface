using UnityEngine;
using TMPro;

public class InfoDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text infoLabel;
    [SerializeField] private float fpsUpdateFrequency = 1f;

    private float timer;
    private int frameCount;

    private void Update()
    {
        timer += Time.unscaledDeltaTime;
        frameCount++;

        if ( timer < fpsUpdateFrequency )
            return;

        UpdateLabel();

        timer = 0f;
        frameCount = 0;
    }

    private void UpdateLabel()
    {
        float averageFramerate = frameCount / timer;
        infoLabel.text = $"FPS: {averageFramerate.ToString( "0.0" )} (sampled over {fpsUpdateFrequency}s)";
    }
}
