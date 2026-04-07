using UnityEngine;
using System.Collections.Generic;
using ClickerGame.Data.Models;

namespace ClickerGame.Gameplay.TouchFunction
{
    public class TouchFunctionFactory
    {
        public static ITouchFunction Create(TouchFunctionDataModel data)
        {
            ITouchFunction function = data.FunctionType switch
            {
                "BonusClick" => new BonusTouchFunction(),
                "SpeedBoost" => new SpeedBoostFunction(),
                "CriticalClick" => new CriticalTouchFunction(),
                _ => new BonusTouchFunction()
            };

            InitializeFunction(function, data);

            return function;
        }

        private static void InitializeFunction(ITouchFunction function, TouchFunctionDataModel data)
        {
            if (function is BonusTouchFunction bonus)
            {
                // BonusTouchFunction uses fixed values
            }
            else if (function is SpeedBoostFunction speedBoost)
            {
                // SpeedBoostFunction uses fixed values
            }
            else if (function is CriticalTouchFunction critical)
            {
                critical.SetCriticalChance(data.CriticalChance);
            }
        }

        public static List<ITouchFunction> CreateList(List<TouchFunctionDataModel> dataList)
        {
            var functions = new List<ITouchFunction>();

            if (dataList == null)
            {
                Debug.LogWarning("[TouchFunctionFactory] Data list is null");
                return functions;
            }

            foreach (var data in dataList)
            {
                if (data.IsActive)
                {
                    var function = Create(data);
                    functions.Add(function);
                    Debug.Log($"[TouchFunctionFactory] Created: {data.FunctionName}");
                }
            }

            return functions;
        }
    }
}
