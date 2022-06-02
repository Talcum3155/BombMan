using Bomb;
using UnityEngine;

namespace Enemy.Cucumber
{
    public class Cucumber : AllEnemy.Enemy
    {
        public void SetOffBomb() //炸弹爆炸的一瞬间会导致对象为空，所以需要进行空指针判断
            => targetPoint.GetComponent<BombController>()?.TurnOff();
    }
}
