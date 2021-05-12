using UnityEngine;
using System.Collections;

public class XplorerGuitarInput : MonoBehaviour
{
    public bool DebugController = false;

    public int A;
    public int B;
    public int X;
    public int Y;
    public int START;
    public int SELECT;
    public int DPadLeft = 0;
    public int DPadRight = 0;
    public int DPadUp = 0;
    public int DPadDown = 0;
    public int rightShoulder;
    public int leftShoulder;
    public int strum = 0;

    public bool green;
    public bool red;
    public bool yellow;
    public bool blue;
    public bool orange;

    // Update is called once per frame
    void Update()
    {
        GetControllerInput();
    }

    // Retrieves the current controller input.
    private void GetControllerInput()
    {
        float dplr = 0;
        float dpud = 0;

        green = Input.GetButton("A");
        red = Input.GetButton("B");
        yellow = Input.GetButton("Y");
        blue = Input.GetButton("X");
        orange = Input.GetButton("Left Shoulder");

        A = returnState(green, A);
        B = returnState(red, B);
        X = returnState(blue, X);
        Y = returnState(yellow, Y);
        START = returnState(Input.GetButton("START"), START);
        SELECT = returnState(Input.GetButton("SELECT"), SELECT);
        rightShoulder = returnState(Input.GetButton("Right Shoulder"), rightShoulder);
        leftShoulder = returnState(orange, leftShoulder);

        dplr = Input.GetAxisRaw("DPadLeftRight");
        dpud = Input.GetAxisRaw("DPadUpDown");

        bool tempLeft = false;
        bool tempRight = false;
        bool tempUp = false;
        bool tempDown = false;

        // Manage d-pad
        if (dplr == -1)
        {
            tempLeft = true;
        }

        else if (dplr == 1)
        {
            tempRight = true;
        }

        if (dpud == -1)
        {
            tempDown = true;
        }

        else if (dpud == 1)
        {
            tempUp = true;
        }

        // Return dpad state.
        DPadLeft = returnState(tempLeft, DPadLeft);
        DPadRight = returnState(tempRight, DPadRight);
        DPadDown = returnState(tempDown, DPadDown);
        DPadUp = returnState(tempUp, DPadUp);
    }


    /* The return state method checks at which state the controller currently is.
     * 0 = no input.
     * 1 = on input down
     * 2 = input hold
     * 3 = on input up
     */
    int returnState(bool action, int state)
    {
        if (action && state == 0)
        {
            state = 1;
        }
        else if (action && (state == 1 || state == 2))
        {
            state = 2;
        }
        else if (!action && (state == 1 || state == 2))
        {
            state = 3;
        }
        else if (!action && (state == 3 || state == 0))
        {
            state = 0;
        }
        return state;
    }

    // Debugging Controller
    private void OnGUI()
    {
        if(DebugController)
        {
            string tmp;

            tmp = "Guitar Controller Input\n" +
                "  Green: " + green + "\n" +
                "  Red: " + red + "\n" +
                "  Yellow: " + yellow + "\n" +
                "  Blue: " + blue + "\n" +
                "  Orange: " + orange + "\n";

            GUI.Label(new Rect(0, 0, Screen.height, Screen.width), tmp);
        }
    }
}
