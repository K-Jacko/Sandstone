using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionLibrary 
{
    public delegate Vector3 Function (float u, float v, float t, float incr);

    public enum FunctionName { Wave, Multiwave, Ripple, Sphere, Torus }
    static Function[] functions = { Wave, Multiwave, Ripple, Sphere, Torus };

    public static FunctionName GetNextFunctionName (FunctionName name)
    {
        return (int)name < functions.Length - 1 ? name + 1 : 0;
        
    }

    public static Vector3 Morph(float u, float v, float t, float incr, Function from, Function to, float progress)
    {
        return Vector3.LerpUnclamped(from(u, v, t, incr), to(u, v, t, incr), SmoothStep(0f, 1f, progress));
    }

    public static Function GetFunction (FunctionName name)
    {
        return functions[(int)name];
    }

    public static FunctionName GetRandomFunctionNameOtherThan (FunctionName name) 
    {
		var choice = (FunctionName)Random.Range(0, functions.Length);
		return choice;
	}


    public static Vector3 Wave (float u, float v, float t, float incr)
    {
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (u + v + t) * incr);
        p.z = v;
        return p;
    }

    public static Vector3 Multiwave (float u, float v, float t, float incr)
    {
		Vector3 p;
		p.x = u;
		p.y = Sin(PI * (u + 0.5f * t));
		p.y += 0.5f * Sin(2f * PI * (v + t));
		p.y += Sin(PI * (u + v + 0.25f * t) * incr);
		p.y *= 1f / 2.5f;
		p.z = v;
		return p;
    }

    public static Vector3 Ripple (float u, float v, float t, float incr)
    {
		float d = Sqrt(u * u + v * v);
		Vector3 p;
		p.x = u;
		p.y = Sin(PI * (4f * d - t));
		p.y /= (1f + 10f * d) * incr;
		p.z = v;
		return p;
    }

    public static Vector3 Sphere (float u, float v, float t, float incr)
    {
        float r = incr + 0.1f * Sin(PI * (6f * u + 4f * v + t));
        float s = r * Cos(0.5f * PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r * Sin(PI * 0.5f * v) * incr;
        p.z = s * Cos(PI * u);
        return p;
    }

    public static Vector3 Torus (float u, float v, float t, float incr) 
    {
		float r1 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t) * incr);
		float r2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t) * incr);
		float s = r1 + r2 * Cos(PI * v);
		Vector3 p;
		p.x = s * Sin(PI * u);
		p.y = r2 * Sin(PI * v) * incr;
		p.z = s * Cos(PI * u);
		return p;
	}
}
