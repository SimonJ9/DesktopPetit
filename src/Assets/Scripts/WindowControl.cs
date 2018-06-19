using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

//game window attr
[System.Serializable]
public class WinRect
{
    public int width = 100;
    public int height = 200;
}

public class WindowControl : MonoBehaviour {

    //winapi rect struct
    struct Rectw
    {
        public uint left;
        public uint top;
        public uint right;
        public uint bottom;
    }

    //winapi point struct
    struct Pointw
    {
        public int x;
        public int y;
    }

    //winapi
    [DllImport("user32.dll")]
    private static extern int SetWindowPos(IntPtr hwnd,
        IntPtr hwndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    [DllImport("user32.dll")]
    private static extern int GetWindowRect(IntPtr hwnd, ref Rectw lpRect);
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll")]
    private static extern int GetSystemMetrics(int nIndex);
    [DllImport("user32.dll")]
    private static extern int GetCursorPos(ref Pointw lpPoint);

    //sprite object ref
    public GameObject sprite_obj;
    public WinRect w = new WinRect();
    public Text t;

    private Vector3 sprite_pos;
    private Sprite sprite;
    private Rectw win_pos;                      //current window position
    private Pointw lp;                          //current cursor position
    private int sx, sy;                         //screen res
    private float mx0, my0, mx1, my1 = 0f;      //mouse position
    private bool dragging = false;
    private Vector3 cent;

    private void Start()
    {
#if !UNITY_EDITOR
        var hwnd = GetActiveWindow();
        GetWindowRect(hwnd, ref win_pos);
        GetCursorPos(ref lp);
        sx = GetSystemMetrics(0);
        sy = GetSystemMetrics(1);
#endif
        
        //unity script
        sprite_pos = sprite_obj.transform.position;
        sprite = sprite_obj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        w.width = (int)sprite.rect.width;
        w.height = (int)sprite.rect.height;

        cent = Camera.main.WorldToScreenPoint(Vector3.zero);
        //Screen.SetResolution(w.width, w.height, false);
    }

    //todo: resize window size to fit the sprite
    private void Update()
    {
        //keep sprite at the center
        //sprite_obj.transform.position = sprite_pos;

        Rect r = sprite.rect;
        Vector3 p = Camera.main.WorldToScreenPoint(sprite_pos);
        //print("current res: " + sx + ", " + sy);
        //print("sprite screen pos: " + p.x + ", " + p.y);

#if !UNITY_EDITOR
        var hwnd = GetActiveWindow();
        GetWindowRect(hwnd, ref win_pos);
        GetCursorPos(ref lp);
#endif

        int nx, ny, ncx, ncy;
        //nx = (int)win_pos.left + (int)p.x - (int)sprite.rect.width / 2;
        //ny = (int)win_pos.top + w.height - (int)p.y - (int)sprite.rect.height / 2;
        nx = (int)win_pos.left;
        ny = (int)win_pos.top;
        ncx = w.width;
        ncy = w.height;

        //Mouse drag function
        if(Input.GetMouseButtonDown(0))
        {
            //if(InSpriteRange(Input.mousePosition, sprite))
            //{
                dragging = true;
                mx0 = lp.x;
                my0 = lp.y;
            //}
        }
        if(Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }

        if(dragging)
        {
            mx1 = lp.x;
            my1 = lp.y;
            if(mx1 != mx0 && my1 != my0)
            {
                nx += (int)(mx1 - mx0);
                ny += (int)(my1 - my0);
                mx0 = mx1;
                my0 = my1;
#if !UNITY_EDITOR
        SetWindowPos(hwnd, new IntPtr(-1), nx, ny, ncx, ncy, 0x4000);
#endif
            }

        }
        //t.text = nx.ToString() + ", " + ny.ToString() + ", " + ncx.ToString() + ", " + ncy.ToString();
        //t.text = "current pos: " + p.x + ", " + p.y;
        //t.text = "mouse pos: " + lp.x.ToString() + ", " + lp.y.ToString();
    }

    private bool InSpriteRange(Vector3 pos, Sprite s)
    {
        return pos.x > cent.x - s.rect.width / 2 && pos.x < cent.x + s.rect.width / 2 &&
            pos.y > cent.y - s.rect.height / 2 && pos.y < cent.y + s.rect.height / 2;
    }
}
