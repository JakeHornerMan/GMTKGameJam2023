using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CarStorage
{
    public static List<Car> Cars { get; set; }

    static CarStorage()
    {
        Cars = new List<Car>();
    }
}