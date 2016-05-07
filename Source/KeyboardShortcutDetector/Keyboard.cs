using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace KeyboardShortcutDetector
{
    internal class Keyboard : IKeyboard
    {
        public enum KeyEvent
        {
            WmKeydown = 256,
            WmKeyup = 257,
            WmSyskeyup = 261,
            WmSyskeydown = 260
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProcedure lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, UIntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        public delegate IntPtr LowLevelKeyboardProcedure(int nCode, UIntPtr wParam, IntPtr lParam);
        public static int WhKeyboardLl = 13;

        private delegate void KeyboardCallbackAsync(KeyEvent keyEvent, int vkCode);
        private readonly KeyboardCallbackAsync _hookedKeyboardCallbackAsync;
        private readonly LowLevelKeyboardProcedure _hookedLowLevelKeyboardProc;
        private readonly IntPtr _hookId;

        private KeyboardState _state = KeyboardState.Empty();

        public void Restart()
        {
            _state = KeyboardState.Empty();;
        }

        public event Action<KeyboardState> StateChanged;

        public Keyboard()
        {
            _hookedLowLevelKeyboardProc = LowLevelKeyboardProc;
            _hookId = SetHook(_hookedLowLevelKeyboardProc);
            //_hookedKeyboardCallbackAsync = KeyboardCallback;
        }
        
        private void KeyboardCallback(KeyEvent keyEvent, int vkCode)
        {
            var selectedKey = KeyInterop.KeyFromVirtualKey(vkCode);

            if (WasKeyPressed(keyEvent) && _state.IsReleased(selectedKey))
            {
                KeyWasPressed(selectedKey);
            }
            else if (WasKeyReleased(keyEvent) && _state.IsPressed(selectedKey))
            {
                KeyWasReleased(selectedKey);
            }
        }

        private static bool WasKeyReleased(KeyEvent keyEvent)
        {
            return keyEvent == KeyEvent.WmKeyup ||
                   keyEvent == KeyEvent.WmSyskeyup;
        }

        private static bool WasKeyPressed(KeyEvent keyEvent)
        {
            return keyEvent == KeyEvent.WmKeydown ||
                   keyEvent == KeyEvent.WmSyskeydown;
        }

        private void KeyWasReleased(Key releasedKey)
        {
            _state = _state.KeyWasReleased(releasedKey);
            StateChanged?.Invoke(_state);
        }

        private void KeyWasPressed(Key pressedKey)
        {
            _state = _state.KeyWasPressed(pressedKey);
            StateChanged?.Invoke(_state);
        }

        public static IntPtr SetHook(LowLevelKeyboardProcedure proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WhKeyboardLl, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private IntPtr LowLevelKeyboardProc(int nCode, UIntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                if (wParam.ToUInt32() == (int)KeyEvent.WmKeydown ||
                    wParam.ToUInt32() == (int)KeyEvent.WmKeyup ||
                    wParam.ToUInt32() == (int)KeyEvent.WmSyskeydown ||
                    wParam.ToUInt32() == (int)KeyEvent.WmSyskeyup)
                {
                    //_hookedKeyboardCallbackAsync.BeginInvoke((KeyEvent) wParam.ToUInt32(), Marshal.ReadInt32(lParam),
                    //    null, null);
                    KeyboardCallback((KeyEvent)wParam.ToUInt32(), Marshal.ReadInt32(lParam));
                }
            }
            return CallNextHookEx(_hookId, nCode, wParam, lParam);
        }
        
        public void Dispose()
        {
            UnhookWindowsHookEx(_hookId);
        }
    }
}