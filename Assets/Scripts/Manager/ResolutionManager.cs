using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    [SerializeField] Dropdown resolution_dropdown;
    [SerializeField] Toggle screenmode_toggle;
    FullScreenMode screenmode;
    Vector2Int resolution;
    private void Awake()
    {
        screenmode = FullScreenMode.Windowed;
        resolution = new Vector2Int(1920, 1080);
    }
    private void Start()
    {
        ResolutionUpdate();
    }
    public void ResolutionUpdate()
    {
        Screen.SetResolution(resolution.x, resolution.y, screenmode);
    }
    void Set854()
    {
        resolution = new Vector2Int(854, 480);
    }
    void Set960()
    {
        resolution = new Vector2Int(960, 540);
    }
    void Set1280()
    {
        resolution = new Vector2Int(1280, 720);
    }
    void Set1600()
    {
        resolution = new Vector2Int(1600, 900);
    }
    void Set1920()
    {
        resolution = new Vector2Int(1920, 1080);
    }
    public void SetWindow()
    {
        screenmode = FullScreenMode.Windowed;
    }
    public void SetFullscreen()
    {
        screenmode = FullScreenMode.FullScreenWindow;
    }
    public void ChangeScreenMode()
    {
        if (screenmode_toggle.isOn)
        {
            SetFullscreen();
        }
        else
        {
            SetWindow();
        }
    }
    public void ChangeResolutionSize()
    {
        switch(resolution_dropdown.value)
        {
            case 0:
                Set854();
                break;
            case 1:
                Set960();
                break;
            case 2:
                Set1280();
                break;
            case 3:
                Set1600();
                break;
            case 4:
                Set1920();
                break;
        }
    }
}
