using System;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Assertions;

namespace starikcetin.hexfallClone.game
{
    public class CallbackAggregator
    {
        private int JobCount { get; set; }
        private readonly Action _masterCallback;
        private bool _callbackPermission;
        private bool _masterInvoked;

        public CallbackAggregator(Action masterCallback)
        {
            if (masterCallback == null)
            {
                Debug.LogWarning("CallbackAggregator: Master callback is null. " +
                                 "Why create a CallbackAggregator if you don't have a callback in the first place?");
            }

            _masterCallback = masterCallback;
        }

        public void PermitCallback()
        {
            _callbackPermission = true;
            CallbackIfCompleted();
        }

        public void JobStarted()
        {
            JobCount++;
        }

        public void JobFinished()
        {
            JobCount--;
            CallbackIfCompleted();
        }

        private void CallbackIfCompleted()
        {
            if (_masterInvoked)
            {
                Debug.LogWarning("CallbackAggregator: Master is already invoked before. Something is wrong here!");
            }

            if (_callbackPermission && JobCount == 0)
            {
                _masterInvoked = true;
                _masterCallback?.Invoke();
            }
        }

        [ContractInvariantMethod]
        private void _Invariant()
        {
            Assert.IsFalse(JobCount < 0, "We got negative job count somehow boss, something is wrong here.");
        }
    }
}
