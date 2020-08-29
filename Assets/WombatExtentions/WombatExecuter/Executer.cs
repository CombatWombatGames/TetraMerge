using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace WombatExecuter
{
    class Executer
    {
        object script;
        MonoBehaviour mono_script;

        public Executer(object script)
        {
            this.script = script;
            mono_script = this.script as MonoBehaviour;
        }

        public InvokeId DelayExecute(float DelayInSeconds, Action<object[]> lambda, params object[] parameters)
        {

            return new InvokeId(mono_script.StartCoroutine(Delayed(DelayInSeconds, lambda, parameters)));
        }

        public InvokeId DelayExecute(float DelayInSeconds, string methodName, params object[] parameters)
        {
            foreach (MethodInfo method in script.GetType().GetMethods())
            {
                if (method.Name == methodName)
                {
                    return new InvokeId(mono_script.StartCoroutine(Delayed(DelayInSeconds, method, parameters)));
                }
            }
            return null;
        }

        public InvokeId ConditionExecute(Func<bool> condition, string methodName, params object[] parameters)
        {
            foreach (MethodInfo method in script.GetType().GetMethods())
            {
                if (method.Name == methodName)
                {
                    return new InvokeId(mono_script.StartCoroutine(Delayed(condition, method, parameters)));
                }
            }
            return null;
        }

        public InvokeId ConditionExecute(Func<bool> condition, Action<object[]> lambda, params object[] parameters)
        {
            return new InvokeId(mono_script.StartCoroutine(Delayed(condition, lambda, parameters)));
        }

        public void StopExecute(InvokeId id)
        {
            mono_script.StopCoroutine(id.coroutine);
        }

        IEnumerator Delayed(float DelayInSeconds, Action<object[]> lambda, params object[] parameters)
        {
            yield return new WaitForSeconds(DelayInSeconds);
            lambda.Invoke(parameters);
        }

        IEnumerator Delayed(float DelayInSeconds, MethodInfo method, params object[] parameters)
        {
            yield return new WaitForSeconds(DelayInSeconds);
            method.Invoke(script, parameters);
        }

        IEnumerator Delayed(Func<bool> condition, Action<object[]> lambda, params object[] parameters)
        {
            yield return new WaitUntil(condition);
            lambda.Invoke(parameters);
        }

        IEnumerator Delayed(Func<bool> condition, MethodInfo method, params object[] parameters)
        {
            yield return new WaitUntil(condition);
            method.Invoke(script, parameters);
        }
    }

    class InvokeId
    {
        public readonly Coroutine coroutine;
        public InvokeId(Coroutine coroutine)
        {
            this.coroutine = coroutine;
        }
    }
}

// using CombatExecuter;

// public class Game_Script : MonoBehaviour
//{
//    void Awake
//    {
//        Executer exe = new Executer(this);
//        exe.DelayExecute(2f , x=> Debug.Log("after 2 seconds"));
//        exe.DelayExecute(2f , x=> Debug.log(x[0]) , "after 2 seconds + params");
//        exe.DelayExecute(2f , "after2seconds");
//        exe.DelayExecute(2f , "afterXseconds", 2);
//        exe.DelayExecute(2f , "afterXY", 2, " seconds");
//    }
//    void after2seconds()
//    {
//        Debug.Log("after 2 seconds");
//    }
//    void afterXseconds(int x)
//    {
//        Debug.log("after" + x + "seconds");
//    }
//    void afterXY(int x, string y)
//    {
//        Debug.log("after " + x + y);
//    }
//}

//Example stop:
//  var id = exe.DelayExecute(0.5f, x => Debug.Log("not gonna run"));
//  exe.StopExecute(id);

//Example when:
//  exe.ConditionExecute(() => score > 100 , x => Debug.Log("you're good"));