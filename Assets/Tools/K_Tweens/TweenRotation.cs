using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KawaSquad
{
    namespace Tween
    {
        public class TweenRotation : Tweener
        {
            [Header("Tween Rotation")]
            [SerializeField]
            public bool isLocal = true;
            [SerializeField]
            public bool useQuaternion = true;
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
                        if (useQuaternion)
                        {
                            if (isLocal)
                                ThisRectTranform.localRotation = Quaternion.Euler(src);
                            else
                                ThisRectTranform.rotation = Quaternion.Euler(src);
                        }
                        else
                        {
                            if (isLocal)
                                ThisRectTranform.localEulerAngles = src;
                            else
                                ThisRectTranform.eulerAngles = src;
                        }
                        break;
                    case Mode.Object:
                        if (useQuaternion)
                        {
                            if (isLocal)
                                ThisTranform.localRotation = Quaternion.Euler(src);
                            else
                                ThisTranform.rotation = Quaternion.Euler(src);
                        }
                        else
                        {
                            if (isLocal)
                                ThisTranform.localEulerAngles = src;
                            else
                                ThisTranform.eulerAngles = src;
                        }
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
                        if (useQuaternion)
                        {
                            if (isLocal)
                                ThisRectTranform.localRotation = Quaternion.Euler(dst);
                            else
                                ThisRectTranform.rotation = Quaternion.Euler(dst);
                        }
                        else
                        {
                            if (isLocal)
                                ThisRectTranform.localEulerAngles = dst;
                            else
                                ThisRectTranform.eulerAngles = dst;
                        }
                        break;
                    case Mode.Object:
                        if (useQuaternion)
                        {
                            if (isLocal)
                                ThisTranform.localRotation = Quaternion.Euler(dst);
                            else
                                ThisTranform.rotation = Quaternion.Euler(dst);
                        }
                        else
                        {
                            if (isLocal)
                                ThisTranform.localEulerAngles = dst;
                            else
                                ThisTranform.eulerAngles = dst;
                        }
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
                        if (useQuaternion)
                        {
                            if (isLocal)
                                ThisRectTranform.localRotation = Quaternion.Lerp(Quaternion.Euler(src), Quaternion.Euler(dst), curve.Evaluate(factor));
                            else
                                ThisRectTranform.rotation = Quaternion.Lerp(Quaternion.Euler(src), Quaternion.Euler(dst), curve.Evaluate(factor));
                        }
                        else
                        {
                            if (isLocal)
                                ThisRectTranform.localEulerAngles = Vector3.Lerp(src, dst, curve.Evaluate(factor));
                            else
                                ThisRectTranform.eulerAngles = Vector3.Lerp(src, dst, curve.Evaluate(factor));
                        }
                        break;
                    case Mode.Object:
                        if (useQuaternion)
                        {
                            if (isLocal)
                                ThisTranform.localRotation = Quaternion.Lerp(Quaternion.Euler(src), Quaternion.Euler(dst), curve.Evaluate(factor));
                            else
                                ThisTranform.rotation = Quaternion.Lerp(Quaternion.Euler(src), Quaternion.Euler(dst), curve.Evaluate(factor));
                        }
                        else
                        {
                            if (isLocal)
                                ThisTranform.localEulerAngles = Vector3.Lerp(src, dst, curve.Evaluate(factor));
                            else
                                ThisTranform.eulerAngles = Vector3.Lerp(src, dst, curve.Evaluate(factor));
                        }
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
