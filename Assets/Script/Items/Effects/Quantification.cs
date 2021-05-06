using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Items.Effects
{
    [System.Serializable]
    public class Quantification
    {
        public delegate void QuantificationCallback(QuantificationCallbackType callbackType);
        public event QuantificationCallback quantificationCallback;
        public enum QuantificationCallbackType
        {
            APPLICATION,
            RECUPERATON,
            END
        }
        private enum MultipleQuantityType
        {
            RANDOM,
            SPECIFIC
        };

        [SerializeField] [Min(1)]private int quantitity = 1;
        [SerializeField] private MultipleQuantityType typeOfQuantityCall = MultipleQuantityType.SPECIFIC;

        private float iterations = 0;
        private float iterationsTime = 0;
        private float totalTime = 0;
        
        public void StartQuantification(float totalTime)
        {
            this.totalTime = totalTime;
            if (quantitity > 1)
            {
                iterationsTime = totalTime / quantitity;
                EffectsController.instance.StartCoroutine(QuantityProccess());
            }
            else
            {
                EffectsController.instance.StartCoroutine(QuantityUnique());
            }
            
        }

        IEnumerator QuantityUnique()
        {
            if(quantificationCallback != null)
                quantificationCallback.Invoke(QuantificationCallbackType.APPLICATION);

            yield return new WaitForSeconds(totalTime);

            if (quantificationCallback != null)
            {
                quantificationCallback.Invoke(QuantificationCallbackType.RECUPERATON);
                quantificationCallback.Invoke(QuantificationCallbackType.END);
            }

            ClearQuantification();
        }

        IEnumerator QuantityProccess()
        {
            QuantificationCallbackType type = QuantificationCallbackType.APPLICATION;
            float waitingTime = (typeOfQuantityCall == MultipleQuantityType.SPECIFIC) ? iterationsTime * 0.5f : 0;
            while (iterations<totalTime)
            {
                if (typeOfQuantityCall == MultipleQuantityType.RANDOM)
                {
                    if (type == QuantificationCallbackType.APPLICATION)
                        waitingTime = Random.Range(iterationsTime * 0.1f, iterationsTime * 0.9f);
                    else
                        waitingTime = iterationsTime - waitingTime;
                }
                
                if(quantificationCallback != null)
                    quantificationCallback.Invoke(type);
                
                yield return new WaitForSeconds(waitingTime);
                
                iterations += waitingTime;
                type = (type == QuantificationCallbackType.RECUPERATON)
                    ? QuantificationCallbackType.APPLICATION
                    : QuantificationCallbackType.RECUPERATON;
            }

            type = QuantificationCallbackType.END;
            if(quantificationCallback != null)
                quantificationCallback.Invoke(type);
            
            ClearQuantification();
        }

        void ClearQuantification()
        {
            iterations = 0;
            iterationsTime = 0;
            totalTime = 0;

            foreach (Delegate d in quantificationCallback.GetInvocationList())
            {
                quantificationCallback -= (QuantificationCallback)d;
            }
        }
    }
}