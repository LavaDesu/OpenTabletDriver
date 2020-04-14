using TabletDriverPlugin;

namespace TabletDriverLib.Interop.Cursor
{
    public interface ICursorHandler
    {
        Point GetCursorPosition();
        void SetCursorPosition(Point pos);
        void SetPressure(uint pressure);
        void SetActive(bool active);
        void Update();

        void MouseDown(MouseButton button);
        void MouseUp(MouseButton button);
    }
}