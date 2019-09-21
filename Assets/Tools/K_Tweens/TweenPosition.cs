using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KawaSquad
{
    namespace Tween
    {
        public class TweenPosition : Tweener
        {
            [Header("Tween Position")]
            [SerializeField]
            public bool isLocal = true;
            [SerializeField]
            public Vector3 src = Vector3.zero;
            [SerializeField]
            public Vector3 dst = Vector3.zero;

            public override void ResetAtBeginning()
            {
                base.ResetAtBeginning();
                switch (mode)
                {
                    case Mode.UI:
                        if (isLocal)
                            ThisRectTranform.localPosition = src;
                        else
                            ThisRectTranform.position = src;
                        break;
                    case Mode.Object:
                        if (isLocal)
                            ThisTranform.localPosition = src;
                        else
                            ThisTranform.position = src;
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
                        if (isLocal)
                            ThisRectTranform.localPosition = dst;
                        else
                            ThisRectTranform.position = dst;
                        break;
                    case Mode.Object:
                        if (isLocal)
                            ThisTranform.localPosition = dst;
                        else
                            ThisTranform.position = dst;
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
                        if (isLocal)
                            ThisRectTranform.localPosition = Vector3.Lerp(src, dst, curve.Evaluate(factor));
                        else
                            ThisRectTranform.position = Vector3.Lerp(src, dst, curve.Evaluate(factor));
                        break;
                    case Mode.Object:
                        if (isLocal)
                            ThisTranform.localPosition = Vector3.Lerp(src, dst, curve.Evaluate(factor));
                        else
                            ThisTranform.position = Vector3.Lerp(src, dst, curve.Evaluate(factor));
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
