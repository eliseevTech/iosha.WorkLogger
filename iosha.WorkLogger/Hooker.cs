using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace iosha.WorkLogger
{
    public class Hooker
    {
        public Hooker()
        {
            SetHook();
        }

        private const int WH_KEYBOARD_LL = 13;        
        private LowLevelKeyboardProcDelegate _callback;
        private IntPtr _hHook;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(
            int idHook,
            LowLevelKeyboardProcDelegate lpfn,
            IntPtr hMod, int dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandle(IntPtr lpModuleName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CallNextHookEx(
            IntPtr hhk,
            int nCode, IntPtr wParam, IntPtr lParam);


        public event EventHandler KeyboardWasPressedEvent;


        private IntPtr LowLevelKeyboardHookProc(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            KeyboardWasPressedEvent?.Invoke(this, null);
            return CallNextHookEx(_hHook, nCode, wParam, lParam);
            //какой-то код который перехватывает ALT TAB https://www.cyberforum.ru/csharp-beginners/thread1327419.html
            //if (nCode < 0)
            //{
            //    return CallNextHookEx(m_hHook, nCode, wParam, lParam);
            //}
            //else
            //{
            //    var khs = (KeyboardHookStruct)
            //              Marshal.PtrToStructure(lParam,
            //              typeof(KeyboardHookStruct));

            //    if (khs.VirtualKeyCode == 9 &&
            //        wParam.ToInt32() == 260 &&
            //        khs.ScanCode == 15) //alt+tab 
            //    {
            //        IntPtr val = new IntPtr(1);
            //        return val;
            //    }

            //    else
            //    {
            //        return CallNextHookEx(m_hHook, nCode, wParam, lParam);
            //    }

            //}
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KeyboardHookStruct
        {
            public readonly int VirtualKeyCode;
            public readonly int ScanCode;
            public readonly int Flags;
            public readonly int Time;
            public readonly IntPtr ExtraInfo;

        }
        private delegate IntPtr LowLevelKeyboardProcDelegate(
            int nCode, IntPtr wParam, IntPtr lParam);
    

        public void SetHook()
        {
            _callback = LowLevelKeyboardHookProc;
            _hHook = SetWindowsHookEx(WH_KEYBOARD_LL,
                _callback,
                GetModuleHandle(IntPtr.Zero), 0);
        }

        public void Unhook()
        {
            UnhookWindowsHookEx(_hHook);
        }
    }
}
