using System;
using NativeLib.OSX;
using TabletDriverPlugin;

namespace TabletDriverLib.Interop.Cursor
{
    using static NativeLib.OSX.OSX;
    public class MacOSCursorHandler : ICursorHandler
    {
        private InputDictionary InputDictionary = new InputDictionary();

        public Point GetCursorPosition()
        {
            IntPtr eventRef = CGEventCreate();
            CGPoint cursor = CGEventGetLocation(ref eventRef);
            CFRelease(eventRef);
            return new Point(cursor.x, cursor.y);
        }

        public void SetCursorPosition(Point pos)
        {
            CGWarpMouseCursorPosition(new CGPoint(pos.X, pos.Y));
        }
        public void SetPressure(uint pressure) { }
        public void SetActive(bool active) { }
        public void Update() { }

        private void PostMouseEvent(CGEventType type, CGMouseButton cgButton)
        {
            var eventRef = CGEventCreate();
            var curPos = GetCursorPosition();
            var cgPos = new CGPoint(curPos.X, curPos.Y);
            var mouseEventRef = CGEventCreateMouseEvent(ref eventRef, type, cgPos, cgButton);
            CGEventPost(ref mouseEventRef, type, cgPos, cgButton);
            CFRelease(eventRef);
            CFRelease(mouseEventRef);
        }

        public void MouseDown(MouseButton button)
        {
            CGEventType type;
            CGMouseButton cgButton;
            switch (button)
            {
                case MouseButton.Left:
                    type = CGEventType.kCGEventLeftMouseDown;
                    cgButton = CGMouseButton.kCGMouseButtonLeft;
                    break;
                case MouseButton.Middle:
                    type = CGEventType.kCGEventOtherMouseDown;
                    cgButton = CGMouseButton.kCGMouseButtonCenter;
                    break;
                case MouseButton.Right:
                    type = CGEventType.kCGEventRightMouseDown;
                    cgButton = CGMouseButton.kCGMouseButtonRight;
                    break;
                case MouseButton.Backward:
                    type = CGEventType.kCGEventOtherMouseDown;
                    cgButton = CGMouseButton.kCGMouseButtonBackward;
                    break;
                case MouseButton.Forward:
                    type = CGEventType.kCGEventOtherMouseDown;
                    cgButton = CGMouseButton.kCGMouseButtonForward;
                    break;
                default:
                    return;
            }
            PostMouseEvent(type, cgButton);
            InputDictionary.UpdateState(button, true);
        }

        public void MouseUp(MouseButton button)
        {
            CGEventType type;
            CGMouseButton cgButton;
            switch (button)
            {
                case MouseButton.Left:
                    type = CGEventType.kCGEventLeftMouseUp;
                    cgButton = CGMouseButton.kCGMouseButtonLeft;
                    break;
                case MouseButton.Middle:
                    type = CGEventType.kCGEventOtherMouseUp;
                    cgButton = CGMouseButton.kCGMouseButtonCenter;
                    break;
                case MouseButton.Right:
                    type = CGEventType.kCGEventRightMouseUp;
                    cgButton = CGMouseButton.kCGMouseButtonRight;
                    break;
                case MouseButton.Backward:
                    type = CGEventType.kCGEventOtherMouseUp;
                    cgButton = CGMouseButton.kCGMouseButtonBackward;
                    break;
                case MouseButton.Forward:
                    type = CGEventType.kCGEventOtherMouseUp;
                    cgButton = CGMouseButton.kCGMouseButtonForward;
                    break;
                default:
                    return;
            }
            PostMouseEvent(type, cgButton);
            InputDictionary.UpdateState(button, false);
        }

        public bool GetMouseButtonState(MouseButton button)
        {
            return InputDictionary.TryGetValue(button, out var state) ? state : false;
        }
    }
}