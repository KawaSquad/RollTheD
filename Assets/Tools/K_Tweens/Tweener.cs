using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KawaSquad
{
    namespace Tween
    {
        public class Tweener : MonoBehaviour
        {
            public enum Behavior
            {
                Once,
                Loop,
                PingPong,
                PingPongOnce,
            }

            public enum Direction
            {
                Forward,
                Backward,
            }

            private enum PingPongStep
            {
                Forward = 0,
                Backward = 1,
                Stop = 2,
            }

            public enum Mode
            {
                UI,
                Object,
            }

            protected float factor = 0;

            [Header("Tweener")]
            [SerializeField]
            protected Mode mode = Mode.Object;
            [SerializeField]
            protected float duration = 1;
            [SerializeField]
            protected bool resetOnStop = false;
            [SerializeField]
            protected AnimationCurve curve = new AnimationCurve(
                new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });
            [SerializeField]
            protected Behavior behavior;
            [SerializeField]
            protected Direction direction = Direction.Forward;

            private PingPongStep pingPongStep = PingPongStep.Forward;
            private Transform thisTranform;
            private RectTransform thisRectTranform;

            #region Play
            public virtual void PlayForward()
            {
                if (!gameObject.activeSelf)
                    gameObject.SetActive(true);

                pingPongStep = PingPongStep.Forward;
                direction = Direction.Forward;
                this.enabled = true;
            }

            public virtual void PlayBackward()
            {
                if (!gameObject.activeSelf)
                    gameObject.SetActive(true);

                pingPongStep = PingPongStep.Forward;
                direction = Direction.Backward;
                this.enabled = true;
            }
            #endregion

            #region Reset
            public void Stop()
            {
                if (resetOnStop)
                {
                    if (direction == Direction.Forward)
                        ResetAtBeginning();
                    else if (direction == Direction.Backward)
                        ResetAtTheEnd();
                }
                this.enabled = false;
            }

            public virtual void ResetAtBeginning()
            {
                factor = 0;
                Stop();
            }

            public virtual void ResetAtTheEnd()
            {
                factor = 1;
                Stop();
            }
            #endregion

            #region Updated
            protected void FactorUpdate()
            {
                if (direction == Direction.Forward)
                    factor += Time.deltaTime * (1 / duration);
                else
                    factor -= Time.deltaTime * (1 / duration);
            }

            public virtual void Animate()
            {

            }

            protected void CheckEndTween()
            {
                if (factor < 0 || factor > 1)
                {
                    switch (behavior)
                    {
                        case Tweener.Behavior.Once:

                            Stop();

                            break;

                        case Tweener.Behavior.Loop:

                            factor = (direction == Direction.Forward) ? 0 : 1;

                            break;

                        case Tweener.Behavior.PingPong:

                            factor = (direction == Direction.Forward) ? 1 : 0;
                            direction = (direction == Direction.Forward) ? Direction.Backward : Direction.Forward;

                            break;

                        case Tweener.Behavior.PingPongOnce:

                            if (pingPongStep == PingPongStep.Forward)
                            {
                                pingPongStep = PingPongStep.Backward;

                                factor = (direction == Direction.Forward) ? 1 : 0;
                                direction = (direction == Direction.Forward) ? Direction.Backward : Direction.Forward;
                            }
                            else if (pingPongStep == PingPongStep.Backward)
                            {
                                pingPongStep = PingPongStep.Stop;
                                Stop();
                            }

                            break;
                    }
                }
            }
            #endregion

            #region Getter
            public Transform ThisTranform
            {
                get
                {
                    if (thisTranform == null)
                        thisTranform = this.transform;
                    return thisTranform;
                }
            }

            public RectTransform ThisRectTranform
            {
                get
                {
                    if (thisRectTranform == null)
                        thisRectTranform = GetComponent<RectTransform>();
                    return thisRectTranform;
                }
            }

            #endregion
        }
    }
}