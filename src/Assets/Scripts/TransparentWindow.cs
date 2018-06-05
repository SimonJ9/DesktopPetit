using System;
using System.Runtime.InteropServices;   //for importing windows native api
using UnityEngine;

public class TransparentWindow : MonoBehaviour
{
    [SerializeField]
    private Material m_Material;

    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    //not sure
    //Windows Native API ??????
    //need to be defined ????


    //"retieves the window handle to the active window attached to the calling thread's message queue
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    //"Change an attribute of the specified window"
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNetLong);

    //"Extends the window frame into the client area"
    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    //??????
    //Definitions of window styles
    const int GWL_STYLE = -16;
    const uint WS_POPUP = 0x80000000;
    const uint WS_VISIBLE = 0x10000000;
    

    private void Start()
    {
#if !UNITY_EDITOR
        var margins = new MARGINS() { cxLeftWidth = -1};

        //handle
        var hwnd = GetActiveWindow();

        //set properties
        //standard window: WS_CAPTION | WS_POPUP | WS_VISIBLE | WS_CLIPSIBLINGS | WS_SYSMENU
        //removes border and title
        SetWindowLong(hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);

        //Extend window into client area
        DwmExtendFrameIntoClientArea(hwnd, ref margins);

#endif
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, m_Material);
    }
}
