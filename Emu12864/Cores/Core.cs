/*Comment one if you do not want to use it*/
//#define PIXEL_DRAW
#define BOX_DRAW

using System;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using DxDLL;
using OpenGL;

namespace Emu12864
{
    public enum Keys
    {
        KeyBACK = 14,
        KeyTAB = 15,
        KeyRETURN = 28,
        KeyLSHIFT = 42,
        KeyRSHIFT = 54,
        KeyLCONTROL = 29,
        KeyRCONTROL = 157,
        KeyESCAPE = 1,
        KeySPACE = 57,
        KeyPGUP = 201,
        KeyPGDN = 209,
        KeyEND = 207,
        KeyHOME = 199,
        KeyLEFT = 203,
        KeyUP = 200,
        KeyRIGHT = 205,
        KeyDOWN = 208,
        KeyINSERT = 210,
        KeyDELETE = 211,
        KeyMINUS = 12,
        KeyYEN = 125,
        KeyPREVTRACK = 144,
        KeyPERIOD = 52,
        KeySLASH = 53,
        KeyLALT = 56,
        KeyRALT = 184,
        KeySCROLL = 70,
        KeySEMICOLON = 39,
        KeyCOLON = 146,
        KeyLBRACKET = 26,
        KeyRBRACKET = 27,
        KeyAT = 145,
        KeyBACKSLASH = 43,
        KeyCOMMA = 51,
        KeyKANJI = 148,
        KeyCONVERT = 121,
        KeyNOCONVERT = 123,
        KeyKANA = 112,
        KeyAPPS = 221,
        KeyCAPSLOCK = 58,
        KeySYSRQ = 183,
        KeyPAUSE = 197,
        KeyLWIN = 219,
        KeyRWIN = 220,
        KeyNUMLOCK = 69,
        KeyNUMPAD0 = 82,
        KeyNUMPAD1 = 79,
        KeyNUMPAD2 = 80,
        KeyNUMPAD3 = 81,
        KeyNUMPAD4 = 75,
        KeyNUMPAD5 = 76,
        KeyNUMPAD6 = 77,
        KeyNUMPAD7 = 71,
        KeyNUMPAD8 = 72,
        KeyNUMPAD9 = 73,
        KeyMULTIPLY = 55,
        KeyADD = 78,
        KeySUBTRACT = 74,
        KeyDECIMAL = 83,
        KeyDIVIDE = 181,
        KeyNUMPADENTER = 156,
        KeyF1 = 59,
        KeyF2 = 60,
        KeyF3 = 61,
        KeyF4 = 62,
        KeyF5 = 63,
        KeyF6 = 64,
        KeyF7 = 65,
        KeyF8 = 66,
        KeyF9 = 67,
        KeyF10 = 68,
        KeyF11 = 87,
        KeyF12 = 88,
        KeyA = 30,
        KeyB = 48,
        KeyC = 46,
        KeyD = 32,
        KeyE = 18,
        KeyF = 33,
        KeyG = 34,
        KeyH = 35,
        KeyI = 23,
        KeyJ = 36,
        KeyK = 37,
        KeyL = 38,
        KeyM = 50,
        KeyN = 49,
        KeyO = 24,
        KeyP = 25,
        KeyQ = 16,
        KeyR = 19,
        KeyS = 31,
        KeyT = 20,
        KeyU = 22,
        KeyV = 47,
        KeyW = 17,
        KeyX = 45,
        KeyY = 21,
        KeyZ = 44,
        Key0 = 11,
        Key1 = 2,
        Key2 = 3,
        Key3 = 4,
        Key4 = 5,
        Key5 = 6,
        Key6 = 7,
        Key7 = 8,
        Key8 = 9,
        Key9 = 10,
    }

    public class Core
    {

        public class Base
        {
            public class FPSCounter
            {
                /* 进行帧率计算和控制的类
                 * 此类需要实例化
                 * 精度为小数点后一位
                 * 其中的Update要加入主游戏循环
                 */
                private int StartTime;
                private int Count;
                private float FPSNum;
                private const int N = 30;
                private const int FPS = 60;

                public FPSCounter()
                {
                    StartTime = 0;
                    Count = 0;
                    FPSNum = 0;
                }

                public void Update()
                {
                    if (Count == 0)
                        StartTime = System.Environment.TickCount;
                    if (Count == N)
                    {
                        int TmpTime = System.Environment.TickCount;
                        FPSNum = 1000f / (((float)TmpTime - (float)StartTime) / (float)N);
                        Count = 0;
                        StartTime = TmpTime;
                    }
                    Count++;
                }

                public void WaitTime()
                {
                    int TookTime = System.Environment.TickCount - StartTime;
                    int WaitTime = Count * 1000 / FPS - TookTime;
                    if (WaitTime > 0)
                        DX.WaitTimer(WaitTime);
                }

                public void GetFPS()
                {
                    DxCS.DxTitle(Launcher.GameTitle + "  " + FPSNum.ToString("f2") + "fps");
                }
            }

            public static bool GetKey(Keys KeyCode)
            {
                /* 键盘输入检测
                 */
                return Convert.ToBoolean(DX.CheckHitKey((int)KeyCode));
            }

        }

        public class DxCS
        {
            /* 对DxLib的二次封装
             * 操作DxLib的函数尽量在这里进行封装
             */
            private const int ExRate = 10;

            public static bool DxInit(IntPtr IconHandle, string Title, bool IsFullScreen, int Width, int Height)
            {
                /* 游戏窗口创建
                 * 注意当返回值为false的时候
                 * 需要提示信息，DirectX加载失败
                 * 这里的全屏是一种比较友好的全屏
                 * 可以随时切换回桌面
                 */
                DX.SetWindowIconHandle(IconHandle);

                if (IsFullScreen)
                {
                    DX.ChangeWindowMode(1);
                    DX.SetWindowStyleMode(2);
                    DX.SetWindowSize(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width,
                                     System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
                }
                else
                {
                    DX.ChangeWindowMode(1);
                    DX.SetWindowStyleMode(5);
                    DX.SetWindowSize(Width, Height);
                }

                DX.SetGraphMode(128 * ExRate, 64 * ExRate, 32);
                DX.SetOutApplicationLogValidFlag(0);
                //DX.SetWaitVSyncFlag(0);
                DX.SetWindowText(Title);
                if (DX.DxLib_Init() == -1)
                    return false;
                DX.SetDrawScreen(DX.DX_SCREEN_BACK);
                return true;
            }

            public static void DxEnd()
            {
                /* 关闭游戏窗口
                 * 处理这条信息
                 * DX.ProcessMessage == -1
                 * 退出主游戏循环后执行这个函数
                 */
                DX.DxLib_End();
            }

            public static void DxFlip()
            {
                /* 翻转调色板
                 * 并清除另一面
                 */
                DX.ScreenFlip();
                DX.ClearDrawScreen();
            }

            public static void DxWait(int msTime)
            {
                /* 延时的函数
                 */
                DX.WaitTimer(msTime);
            }

            public static void DxTitle(string Title)
            {
                DX.SetWindowText(Title);
            }

            public static void DrawPixel(int x, int y, int Color)
            {
                /* 绘制像素的函数
                 */
#if PIXEL_DRAW
                for (int i = 0; i < ExRate; i++)
                    for (int j = 0; j < ExRate; j++)
                        DX.DrawPixel(x * ExRate + i, y * ExRate + j, Color);
#endif

#if BOX_DRAW
                DX.DrawBox(x * ExRate, y * ExRate, (x + 1) * ExRate, (y + 1) * ExRate, Color, 1);
#endif
            }

        }

        public class GlCS
        {

        }

        public class EngineBase
        {
            private const int BMPTRANS = -1;

            /*图片结构体*/
            public struct EmuBMP
            {
                public int[,] Dat;

                public void Draw(int x, int y)
                {
                    int i = 0, j = 0;
                    for (i = 0; i < Dat.GetLength(0); i++)
                        for (j = 0; j < Dat.GetLength(1); j++)
                            if (Dat[i, j] != -1) DxCS.DrawPixel(x + i, y + j, Dat[i, j]);
                }

            }

            /*EMU写数据，带颜色和是否透明*/
            /*于2016.11.1修改为寻址式坐标*/
            private static void EMU_WrDat(int x, int y, int Dat, int Color, bool Transparent)
            {
                for (int i = 0; i < 8; i++) //发送一个八位数据 
                {
                    if (((Dat << i) & 0x80) != 0) DxCS.DrawPixel(x, y * 8 + 8 - i, Color);
                    else if (!Transparent) DxCS.DrawPixel(x, y * 8 + 8 - i, 0x000000);

                }
            }

            /*功能描述：显示6*8一组标准ASCII字符串*/
            private static void EMU_P6x8Str(int x, int y, string Str, int Color, bool Transparent)
            {
                int c = 0, i = 0, j = 0;
                while (j < Str.Length)
                {
                    c = Str[j] - 32;
                    if (x > 126) { x = 0; y++; }
                    for (i = 0; i < 6; i++)
                        EMU_WrDat(x + i, y, Res.F6x8[c, i], Color, Transparent);
                    x += 6;
                    j++;
                }
            }

            /*功能描述：显示8*16一组标准ASCII字符串*/
            private static void EMU_P8x16Str(int x, int y, string Str, int Color, bool Transparent)
            {
                int c = 0, i = 0, j = 0;
                while (j < Str.Length)
                {
                    c = Str[j] - 32;
                    if (x > 120) { x = 0; y++; }

                    for (i = 0; i < 8; i++)
                        EMU_WrDat(x + i, y, Res.F8X16[c, i], Color, Transparent);
                    for (i = 0; i < 8; i++)
                        EMU_WrDat(x + i, y + 1, Res.F8X16[c, i + 8], Color, Transparent);
                    x += 8;
                    j++;
                }
            }

            /*绘制字符串*/
            public void DrawString(int x, int y, string Str, int Color, bool BigFont, bool Transparent)
            {
                if (BigFont) EMU_P8x16Str(x, y, Str, Color, Transparent);
                else EMU_P6x8Str(x, y, Str, Color, Transparent);
            }

            /*绘制字符串*/
            public void DrawString(string Str)
            {
                EMU_P6x8Str(0, 0, Str, 0xFFFFFF, false);
            }

            /*绘制像素*/
            public void DrawPixel(int x, int y, int Color)
            {
                DxCS.DrawPixel(x, y, Color);
            }

            /*获取按键*/
            public bool GetKey(Keys Key)
            {
                return Base.GetKey(Key);
            }
        }

        public class Editor
        {
            public static string RunCMD(string Commands, int TimeOutMS = 1000)
            {
                Process myProcess = new Process();
                ProcessStartInfo myProcessStartInfo = new ProcessStartInfo("cmd.exe");
                myProcessStartInfo.UseShellExecute = false;
                myProcessStartInfo.RedirectStandardOutput = true;
                myProcessStartInfo.CreateNoWindow = true;
                myProcessStartInfo.Arguments = "/c " + Commands;
                myProcess.StartInfo = myProcessStartInfo;
                myProcess.Start();
                myProcess.WaitForExit(TimeOutMS);
                StreamReader myStreamReader = myProcess.StandardOutput;
                string myString = myStreamReader.ReadToEnd();
                myProcess.Close();
                return myString;
            }

            private static int RgbToInt(int r, int g, int b)
            {
                return r << 16 | g << 8 | b;
            }

            public static string LoadBMP(Bitmap BMP)
            {
                string CookedData = "new int[,] { \n";
                for (int i = 0; i < BMP.Width; i++)
                {
                    CookedData += "{ ";
                    for (int j = 0; j < BMP.Height; j++)
                    {
                        if (BMP.GetPixel(i, j).A == 0)
                        {
                            if (j == BMP.Height - 1) CookedData += "BMPTRANS, " + " },\n";
                            else CookedData += "BMPTRANS, ";
                        }
                        else
                        {
                            if (j == BMP.Height - 1) CookedData += "0x" + RgbToInt(BMP.GetPixel(i, j).R, BMP.GetPixel(i, j).G, BMP.GetPixel(i, j).B).ToString("X6") + " },\n";
                            else CookedData += "0x" + RgbToInt(BMP.GetPixel(i, j).R, BMP.GetPixel(i, j).G, BMP.GetPixel(i, j).B).ToString("X6") + ", ";
                        }
                    }
                }
                CookedData += "};";
                return CookedData;
            }
        }

    }

    public class GameBase
    {
        private static Core.Base.FPSCounter FPSer;
        protected Core.EngineBase Engine;
        public GameBase()
        {
            FPSer = new Core.Base.FPSCounter();
            Engine = new Core.EngineBase();
            Start();
        }

        public void Work()
        {
            Core.DxCS.DxFlip();
            MainRender();
            FPSer.Update();
            FPSer.WaitTime();
            FPSer.GetFPS();
        }

        private void MainRender()
        {
            Loop();
        }

        public virtual void Start()
        {

        }

        public virtual void Loop()
        {

        }
    }

}
