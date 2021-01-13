using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimation : MonoBehaviour
{
    public enum AnimationType
    {
        WAIT,
        WALK,
        RUN,
        MOTION_1,
        MOTION_2,
        MOTION_3,
        MOTION_4,
        MAX
    }

    //! Keyの最大数
    private const int keyNumMax = 4;

    //[Header("左手のサイリウム")]
    //[SerializeField] private GameObject leftPsyllium = null;
    //[Header("右手のサイリウム")]
    //[SerializeField] private GameObject rightPsyllium = null;
    [Header("モデルのアニメーター")]
    [SerializeField] private Animator animator = null;

    private string[] animationParametor = { "IsIdle", "IsWalk", "IsRun", "IsMotion_1", "IsMotion_2", "IsMotion_3", "IsMotion_4" };

    private bool standardMotionFlag = false;
    private bool[] buttonFlag = new bool[keyNumMax];
    private int buttonNum = 0;
    private AnimationType animationType = AnimationType.WAIT;

    public bool StandardMotionFlag
    {
        set { standardMotionFlag = value; }
    }

    // Update is called once per frame
    public void MyUpdate()
    {
        if (standardMotionFlag)
        {
            animationType = AnimationType.WALK;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                animationType = AnimationType.RUN;
            }
        }
        else
        {
            int count = 0;

            for (int i = 0; i < buttonFlag.Length; i++)
            {
                if (buttonFlag[i])
                    count++;
            }

            if (count == 0)
                animationType = AnimationType.WAIT;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            MotionSwitch(1);
            animationType = AnimationType.MOTION_1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            MotionSwitch(2);
            animationType = AnimationType.MOTION_2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            MotionSwitch(3);
            animationType = AnimationType.MOTION_3;
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            MotionSwitch(4);
            animationType = AnimationType.MOTION_4;
        }

        AnimationChanger();
    }

    /// <summary>
    /// モーションアニメ切り替え用
    /// </summary>
    public void MotionSwitch(int buttonNumber)
    {
        standardMotionFlag = false;

        buttonNum = buttonNumber - 1;

        for (int i = 0; i < buttonFlag.Length; i++)
        {
            if (buttonNum != i)
                buttonFlag[i] = false;
        }

        buttonFlag[buttonNum] = buttonFlag[buttonNum] ? false : true;

        //! サイリウムの有効化と無効化
        if (buttonNum == 3)
        {
            //leftPsyllium.SetActive(buttonFlag[buttonNum]);
            //rightPsyllium.SetActive(buttonFlag[buttonNum]);
        }
        else
        {
            //leftPsyllium.SetActive(false);
            //rightPsyllium.SetActive(false);
        }

    }

    void AnimationChanger()
    {
        switch (animationType)
        {
            case AnimationType.WAIT:
                StandardAnimationPlay();
                //stateText.text = $"現在の状態_待機";
                break;
            case AnimationType.WALK:
                StandardAnimationPlay();
                //stateText.text = $"現在の状態_歩き";
                break;
            case AnimationType.RUN:
                StandardAnimationPlay();
                //stateText.text = $"現在の状態_走り";
                break;
            case AnimationType.MOTION_1:
                MotionAnimationPlay();
                //stateText.text = $"現在の状態_モーション１";
                break;
            case AnimationType.MOTION_2:
                MotionAnimationPlay();
                //stateText.text = $"現在の状態_モーション２";
                break;
            case AnimationType.MOTION_3:
                MotionAnimationPlay();
                //stateText.text = $"現在の状態_モーション３";
                break;
            case AnimationType.MOTION_4:
                MotionAnimationPlay();
                //stateText.text = $"現在の状態_モーション４";
                break;
        }
    }

    /// <summary>
    /// 待機状態や歩く走るのアニメーション再生用
    /// </summary>
    void StandardAnimationPlay()
    {
        for (int i = 0; i < buttonFlag.Length; i++)
        {
            buttonFlag[i] = false;
        }

        //leftPsyllium.SetActive(false);
        //rightPsyllium.SetActive(false);

        if (animationType != AnimationType.WAIT)
        {
            animator.SetBool(animationParametor[(int)animationType], true);

            for (int i = 0; i < (int)AnimationType.MAX; i++)
            {
                if ((int)animationType != i)
                    animator.SetBool(animationParametor[i], false);
            }
        }
        else
        {
            for (int i = 0; i < (int)AnimationType.MAX; i++)
            {
                animator.SetBool(animationParametor[i], false);
            }
        }
    }

    /// <summary>
    /// モーションアニメーション再生用
    /// </summary>
    void MotionAnimationPlay()
    {
        if (buttonFlag[buttonNum])
        {
            animator.SetBool(animationParametor[(int)animationType], true);

            for (int i = 0; i < (int)AnimationType.MAX; i++)
            {
                if ((int)animationType != i)
                    animator.SetBool(animationParametor[i], false);
            }
        }
        else
        {
            for (int i = 0; i < (int)AnimationType.MAX; i++)
            {
                animator.SetBool(animationParametor[i], false);
            }
        }
    }
}