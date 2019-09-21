using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KawaSquad
{
    namespace Tween
    {
        public class TweenScale : Tweener
        {
            [Header("Tween Scale")]
            [SerializeField]
            public Vector3 src = Vector3.one;
            [SerializeField]
            public Vector3 dst = Vector3.one;

            public override void ResetAtBeginning()
            {
                base.ResetAtBeginning();
                switch (mode)
                {
                    case Mode.UI:
                        ThisRectTranform.localScale = src;
                        break;
                    case Mode.Object:
                        ThisTranform.localScale = src;
                        break;
                    default:
                        break;
                }
            }
            public override void ResetAtTheEnd()
            {
                base.ResetAtTheEnd();
                switch (mode)
                {
                    case Mode.UI:
                        ThisRectTranform.localScale = dst;
                        break;
                    case Mode.Object:
                        ThisTranform.localScale = dst;
                        break;
                    default:
                        break;
                }
            }
            public override void Animate()
            {
                base.Animate();
                switch (mode)
                {
                    case Mode.UI:
                        ThisRectTranform.localScale = Vector3.Lerp(src, dst, curve.Evaluate(factor)); 
                        break;
                    case Mode.Object:
                        ThisTranform.localScale = Vector3.Lerp(src, dst, curve.Evaluate(factor)); 
                        break;
                    default:
                        break;
                }
            }
            private void Update()
            {
                FactorUpdate();
                Animate();
                CheckEndTween();
            }
        }
    }
}
