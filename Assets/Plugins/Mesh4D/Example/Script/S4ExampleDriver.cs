using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using M4DLib;

public class S4ExampleDriver : MonoBehaviour {

    public Quaternion4 current = Quaternion4.identity;
    public Quaternion4 target = Quaternion4.identity;
    public float smoothPhase = 3f;
    public float walkSpeed = 2f;
    [Space]
    public Text orientation;

    S4Viewer view;

    void Start ()
    {
        view = GetComponent<S4Viewer>();
         UpdateOrientationText();

    }

    float GetAxis (KeyCode min, KeyCode max)
    {
        return (Input.GetKey(max) ? 1 : (Input.GetKey(min) ? -1 : 0));
    }

	// Update is called once per frame
	void Update () {
        var dir = new Vector4(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"), GetAxis(KeyCode.Q, KeyCode.E)) * walkSpeed;
        view.position += target * ((Quaternion4)Camera.main.transform.rotation * dir);
		view.rotation = current = Quaternion4.Slerp(current, target, smoothPhase * Time.deltaTime);

        if (Input.anyKeyDown) {
            if (Input.GetKeyDown(KeyCode.I))
                target *= Quaternion4.Euler (0, 0, 0, 0, 0, 90);
            else if (Input.GetKeyDown(KeyCode.K))
                target *= Quaternion4.Euler (0, 0, 0, 0, 0, -90);
            else if (Input.GetKeyDown(KeyCode.L))
                target *= Quaternion4.Euler (0, 90, 0, 0, 0, 0);
            else if (Input.GetKeyDown(KeyCode.J))
                target *= Quaternion4.Euler (0, -90, 0, 0, 0, 0);

            UpdateOrientationText();
        }

        if (Time.frameCount % 15 == 0)
            UpdateOrientationText();
	}

//    static Dictionary<int, string> dictXWTxt
    
    void UpdateOrientationText ()
    {
        if (!orientation)
            return;
        var camForward = target * (Camera.main.transform.forward);
        var worldX = target * (Vec4.right);
        var worldY = target * (Vec4.up);
        var worldZ = target * (Vec4.forward);
        orientation.text = "Position : " + view.position.ToString("0.0") + "\n" +
        "Orientation : " + GetTextFromAxis(worldX) + " " + GetTextFromAxis(worldY) + " " + GetTextFromAxis(worldZ) + "\n" +
         "Facing : " + GetTextFromAxis(camForward);
    }

    static string[] dirsTxt = new string[] { "X", "Y", "Z", "W" };

    string GetTextFromAxis (Vector4 axis)
    {
        int dir = 0;
        if (axis.x * axis.x > 0.5f)
            dir = 0;
        else if (axis.y * axis.y > 0.5f)
            dir = 1;
        else if (axis.z * axis.z > 0.5f)
            dir = 2;
        else if (axis.w * axis.w > 0.5f)
            dir = 3;
        return dirsTxt[dir] + (axis[dir] > 0 ? "+" : "-");
    }
}
