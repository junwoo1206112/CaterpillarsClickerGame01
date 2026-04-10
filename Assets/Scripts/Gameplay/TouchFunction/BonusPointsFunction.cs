using UnityEngine;

namespace ClickerGame.Gameplay.TouchFunction
{
    public class BonusPointsFunction : ITouchFunction
    {
        private string _functionName;
        private int _bonusPoints;
        
        public string FunctionName => _functionName;
        public int TriggerCount => 0;
        public float Multiplier => 1f;
        public float Duration => 0f;
        public float Cooldown => 0f;
        public bool IsReusable => true;
        
        public int BonusPoints => _bonusPoints;
        
        public BonusPointsFunction(string name, int bonusPoints)
        {
            _functionName = name;
            _bonusPoints = bonusPoints;
        }
        
        public bool CanActivate(int currentCount, float lastActivateTime)
        {
            return true;
        }
        
        public TouchEffect Activate()
        {
            return new TouchEffect(_bonusPoints, 1f, 0f, FunctionName);
        }
        
        public void Reset()
        {
        }
        
        public ITouchFunction Clone()
        {
            return new BonusPointsFunction(_functionName, _bonusPoints);
        }
    }
}