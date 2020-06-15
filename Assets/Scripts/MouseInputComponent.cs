using UnityEngine;

public class MouseInputComponent : IInputComponent
{
    public bool IsButtonPressed()
    {
        return Input.GetKey(KeyCode.Mouse0);
    }
}
