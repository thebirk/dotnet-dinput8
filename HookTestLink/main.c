#include <Windows.h>
#include <stdio.h>
#include <dinput.h>

INT WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, PSTR lpCmdLine, INT nCmdShow)
{
    OutputDebugStringA("main.c");
    IDirectInput8A* dinp;
    DirectInput8Create(GetModuleHandleA(0), DIRECTINPUT_VERSION, &IID_IDirectInput8A, (void**)&dinp, 0);

    WNDCLASSA cls = {0};
    cls.lpszClassName = "ClassName";
    cls.style = CS_OWNDC | CS_VREDRAW | CS_HREDRAW;
    cls.lpfnWndProc = DefWindowProcA;
    cls.hbrBackground = CreateSolidBrush(RGB(127, 127, 127));
    RegisterClassA(&cls);

    HWND hwnd = CreateWindowA(cls.lpszClassName, "", WS_OVERLAPPEDWINDOW, CW_USEDEFAULT, CW_USEDEFAULT, 800, 600, 0, 0, 0, 0);
    ShowWindow(hwnd, nCmdShow);

    MSG msg;
    while ((GetMessage(&msg, NULL, 0, 0)) != 0)
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    return 0;
}

//int main(int argc, char **argv)
//{
//    
//    return 0;
//}
