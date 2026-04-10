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
                "Bonus" => new BonusTouchFunction(),
                "BonusClick" => new BonusTouchFunction(),
                "SpeedBoost" => new SpeedBoostFunction(),
                "Critical" => new CriticalTouchFunction(),
                "CriticalClick" => new CriticalTouchFunction(),
                "PowerBoost" => new PowerBoostFunction(data.FunctionName, Mathf.RoundToInt(data.Multiplier)),
                "BonusPoints" => new BonusPointsFunction(data.FunctionName, Mathf.RoundToInt(data.Multiplier)),
                "TripleClick" => new TripleClickFunction(),
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
            else if (function is PowerBoostFunction powerBoost)
            {
                // PowerBoostFunction initialized with data
            }
            else if (function is BonusPointsFunction bonusPoints)
            {
                // BonusPointsFunction initialized with data
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
