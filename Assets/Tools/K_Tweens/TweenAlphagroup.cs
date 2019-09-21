using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KawaSquad
{
    namespace Tween
    {
        [RequireComponent(typeof(CanvasGroup))]
        public class TweenAlphagroup : Tweener
        {
            //[Header("Tween Alpha group")]
            //[SerializeField]
            float src = 0f;
            //[SerializeField]
            float dst = 1f;

            private CanvasGroup alphaGroup;
            public CanvasGroup AlphaGroup
            {
                get
                {
                    if (alphaGroup == null)
                        alphaGroup = this.GetComponent<CanvasGroup>();
                    return alphaGroup;
                }
            }


            public override void ResetAtBeginning()
            {
                base.ResetAtBeginning();
                AlphaGroup.alpha = src;
                AlphaGroup.blocksRaycasts = false;
            }
            public override void ResetAtTheEnd()
            {
                base.ResetAtTheEnd();
                AlphaGroup.alpha = dst;
                AlphaGroup.blocksRaycasts = true;
            }

            public override void PlayForward()
            {
                base.PlayForward();
                AlphaGroup.blocksRaycasts = true;
            }
            public override void PlayBackward()
            {
                base.PlayBackward();
                AlphaGroup.blocksRaycasts = false;
            }

            public override void Animate()
            {
                base.Animate();
                AlphaGroup.alpha = Mathf.Lerp(src, dst, curve.Evaluate(factor)); 
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
