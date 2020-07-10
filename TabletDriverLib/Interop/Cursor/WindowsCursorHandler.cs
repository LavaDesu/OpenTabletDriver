using System;
using NativeLib.Windows;
using NativeLib.Windows.Input;
using System.Linq;
using TabletDriverPlugin;

namespace TabletDriverLib.Interop.Cursor
{
    using static Windows;

    public class WindowsCursorHandler : ICursorHandler
    {
        public WindowsCursorHandler()
        {
            _offsetX = Platform.VirtualScreen.Position.X;
            _offsetY = Platform.VirtualScreen.Position.Y;
        }

        private float _offsetX, _offsetY;

        public Point GetCursorPosition()
        {
            GetCursorPos(out POINT pt);
            return new Point(pt.X + _offsetX, pt.Y + _offsetY);
        }

        public void SetCursorPosition(Point pos)
        {
            SetCursorPos((int)(pos.X - _offsetX), (int)(pos.Y - _offsetY));
        }
        public void SetPressure(uint pressure) { }
        public void SetActive(bool active) { }
        public void Update() { }

        private void MouseEvent(MOUSEEVENTF arg, uint dwData = 0)
        {
            var pos = GetCursorPosition();
            mouse_event((uint)arg, (uint)pos.X, (uint)pos.Y, dwData, 0);
        }

        public void MouseDown(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    MouseEvent(MOUSEEVENTF.LEFTDOWN);
                    return;
                case MouseButton.Middle:
                    MouseEvent(MOUSEEVENTF.MIDDLEDOWN);
                    return;
                case MouseButton.Right:
                    MouseEvent(MOUSEEVENTF.RIGHTDOWN);
                    return;
                case MouseButton.Backward:
                    MouseEvent(MOUSEEVENTF.XDOWN, (uint)XBUTTON.XBUTTON1);
                    return;
                case MouseButton.Forward:
                    MouseEvent(MOUSEEVENTF.XDOWN, (uint)XBUTTON.XBUTTON2);
                    return;
            }
        }

        public void MouseUp(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    MouseEvent(MOUSEEVENTF.LEFTUP);
                    return;
                case MouseButton.Middle:
                    MouseEvent(MOUSEEVENTF.MIDDLEUP);
                    return;
                case MouseButton.Right:
                    MouseEvent(MOUSEEVENTF.RIGHTUP);
                    return;
                case MouseButton.Backward:
                    MouseEvent(MOUSEEVENTF.XUP, (uint)XBUTTON.XBUTTON1);
                    return;
                case MouseButton.Forward:
                    MouseEvent(MOUSEEVENTF.XUP, (uint)XBUTTON.XBUTTON2);
                    return;
            }
        }
    }
}