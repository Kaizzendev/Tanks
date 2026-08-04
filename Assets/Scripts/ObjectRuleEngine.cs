using System.Collections.Generic;
using UnityEngine;

public static class ObjectRuleEngine
{
    public static ProceduralObject SelectObject(float density, List<ProceduralObject> list)
    {
        List<ProceduralObject> valid = new List<ProceduralObject>();

        // filtrar por rango
        foreach (var obj in list)
        {
            if (density >= obj.minDensity && density <= obj.maxDensity)
                valid.Add(obj);
        }

        if (valid.Count == 0)
            return null;

        // elegir por probabilidad
        foreach (var obj in valid)
        {
            if (Random.value < obj.probability)
                return obj;
        }

        return null;
    }
}