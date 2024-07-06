using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraCapture : MonoBehaviour
{
    [SerializeField] private RawImage feedRawImage;
    private WebCamTexture _webCamTexture;

    private int _cameraNum;
    
    private void Start()
    {
        _webCamTexture = new WebCamTexture();
        feedRawImage.texture = _webCamTexture;
        _webCamTexture.Play();
    }

    public void CapturePhoto()
    {
        Texture2D capturedPhoto = new Texture2D(_webCamTexture.width, _webCamTexture.height);
        capturedPhoto.SetPixels(_webCamTexture.GetPixels());
        capturedPhoto.Apply();

        SkinsManager.SaveSkin(capturedPhoto);
        LoadScene(1);
    }

    public void FlipCamera()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0) return;

        if (_cameraNum == 0) _cameraNum = 1;
        else _cameraNum = 0;
        
        _webCamTexture = new WebCamTexture(devices[_cameraNum].name);
        feedRawImage.texture = _webCamTexture;
        _webCamTexture.Play();
    }

    public void LoadScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }
}
