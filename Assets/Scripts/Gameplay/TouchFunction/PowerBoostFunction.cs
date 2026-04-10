using UnityEngine;

namespace ClickerGame.Gameplay.TouchFunction
{
    public class PowerBoostFunction : ITouchFunction
    {
        private string _functionName;
        private int _powerBonus;
        
        public string FunctionName => _functionName;
        public int TriggerCount => 0;
        public float Multiplier => 1f;
        public float Duration => 0f;
        public float Cooldown => 0f;
        public bool IsReusable => true;
        
        public int PowerBonus => _powerBonus;
        
        public PowerBoostFunction(string name, int powerBonus)
        {
            _functionName = name;
            _powerBonus = powerBonus;
        }
        
        public bool CanActivate(int currentCount, float lastActivateTime)
        {
            return true;
        }
        
        public TouchEffect Activate()
        {
            return new TouchEffect(_powerBonus, 1f, 0f, FunctionName);
        }
        
        public void Reset()
        {
        }
        
        public ITouchFunction Clone()
        {
            return new PowerBoostFunction(_functionName, _powerBonus);
        }
    }
}