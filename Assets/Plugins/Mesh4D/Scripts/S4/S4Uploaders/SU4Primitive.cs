using UnityEngine;
using System.Collections;

namespace M4DLib {

[AddComponentMenu("Scripts/Mesh4D/Uploaders/SU4 Primitive Uploader")]
public class SU4Primitive : S4UploaderBase
{
    public enum Shape {
        Hyperplane,
        Pentatope,
        Tesseract,
        Hexdecahedroid,
    }

    public Shape shape = Shape.Tesseract;
    public float radius = 1;
    public override void UploadBuffer(S4Buffer buffer)
    {
        switch (shape)
        {
            case Shape.Hyperplane:
                MakeHyperplane(buffer);
                break;
            case Shape.Pentatope:
                MakePentatope(buffer);
                break;
            case Shape.Tesseract:
                MakeTesseract(buffer);
                break;
            case Shape.Hexdecahedroid:
                MakeHexdecahedroid(buffer);
                break;
        }
    }

    void MakeHyperplane (S4Buffer buffer)
    {
        buffer.Expand(8, 5 * 4);

        for (int x = -1; x <= 1; x += 2)
            for (int z = -1; z <= 1; z += 2)
                for (int w = -1; w <= 1; w += 2)
                        {
                            var v = new Vector4(x,0,z,w);
                            buffer.AddVert(v * radius, MakeProfile(v));
                        }
        
        buffer.AddCube(00, 01, 02, 03, 04, 05, 06, 07);
    }

    static Vector4[] _pentatopeVertices = {
        new Vector4(1 / Mathf.Sqrt(6), -1 / Mathf.Sqrt(10), 1 / Mathf.Sqrt(3), -1),
        new Vector4(1 / Mathf.Sqrt(6), -1 / Mathf.Sqrt(10), 1 / Mathf.Sqrt(3), 1),
        new Vector4(1 / Mathf.Sqrt(6), -1 / Mathf.Sqrt(10), -2 / Mathf.Sqrt(3), 0),
        new Vector4(-Mathf.Sqrt(3 / 2f), -1 / Mathf.Sqrt(10), 0, 0),
        new Vector4(0, 2 * Mathf.Sqrt(2 / 5f), 0, 0),
    };

    void MakePentatope (S4Buffer buffer)
    {
        buffer.Expand(16, 5 * 4);

        for (int a = 0; a < 5; a++)
            buffer.vertices.Add(_pentatopeVertices[a] * radius);
        
        buffer.AddTrimid(0, 1, 2, 3);
        buffer.AddTrimid(0, 1, 2, 4);

        buffer.AddTrimid(0, 1, 3, 4);
        buffer.AddTrimid(1, 2, 3, 4);
        buffer.AddTrimid(2, 0, 3, 4);
    }

    static VertProfile MakeProfile (Vector4 pos)
    {
        pos = (pos) * 0.5f;
        var color = new Color(Mathf.Lerp(pos.x, 0.5f, pos.w), Mathf.Lerp(pos.y, 0.5f, pos.w), Mathf.Lerp(pos.z, 0.5f, pos.w));
        var uv = new Vector4(pos.x, pos.z, pos.w, pos.y);

        return new VertProfile(color, uv, uv);
    }

    void MakeTesseract (S4Buffer buffer)
    {
        buffer.Expand(16, 8 * 5 * 4);

        for (int x = -1; x <= 1; x += 2)
            for (int y = -1; y <= 1; y += 2)
                for (int z = -1; z <= 1; z += 2)
                    for (int w = -1; w <= 1; w += 2) 
                        {
                            var v = new Vector4(x,y,w,z);
                            buffer.AddVert(v * radius, MakeProfile(v));
                        }

       buffer.AddCube(00, 01, 02, 03, 04, 05, 06, 07);
       buffer.AddCube(08, 09, 10, 11, 12, 13, 14, 15);
       buffer.AddCube(00, 01, 02, 03, 08, 09, 10, 11);
       buffer.AddCube(04, 05, 06, 07, 12, 13, 14, 15);

       buffer.AddCube(00, 01, 04, 05, 08, 09, 12, 13);
       buffer.AddCube(02, 03, 06, 07, 10, 11, 14, 15);
       buffer.AddCube(00, 02, 04, 06, 08, 10, 12, 14);
       buffer.AddCube(01, 03, 05, 07, 09, 11, 13, 15);

    }

    void MakeHexdecahedroid (S4Buffer buffer)
    {
        for (int a = 0; a < 4; a += 1)
            for (int b = -1; b <= 1; b += 2)
                {
                    var v = Vector4.zero;
                    v[a] = b;
                    buffer.AddVert(v * radius, MakeProfile(v));
                }

        buffer.AddTrimid(0, 2, 4, 6);
        buffer.AddTrimid(1, 3, 5, 7);

        buffer.AddTrimid(1, 2, 4, 6);
        buffer.AddTrimid(0, 3, 4, 6);
        buffer.AddTrimid(0, 2, 5, 6);
        buffer.AddTrimid(0, 2, 4, 7);

        buffer.AddTrimid(0, 3, 5, 7);
        buffer.AddTrimid(1, 2, 5, 7);
        buffer.AddTrimid(1, 3, 4, 7);
        buffer.AddTrimid(1, 3, 5, 6);

        buffer.AddTrimid(1, 3, 4, 6);
        buffer.AddTrimid(1, 2, 5, 6);
        buffer.AddTrimid(1, 2, 4, 7);
        buffer.AddTrimid(0, 3, 5, 6);
        buffer.AddTrimid(0, 3, 4, 7);
        buffer.AddTrimid(0, 2, 5, 7);

    }
}

}