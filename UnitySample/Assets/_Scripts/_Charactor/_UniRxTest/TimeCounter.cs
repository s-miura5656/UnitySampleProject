using System.Collections;
using UnityEngine;

/// <summary>
/// 100からカウントダウンし値を通知するサンプル
/// </summary>
public class TimeCounter : MonoBehaviour
{
    public delegate void TimerEventHandler(int time);
    public event TimerEventHandler OnTimeChanged;


    void Start()
    {
        StartCoroutine(TimerCoroutine());
    }

    IEnumerator TimerCoroutine()
    {
        var time = 100;

        while (time > 0) 
        {
            time--;

            OnTimeChanged(time);

            yield return new WaitForSeconds(1);
        }
    }
}