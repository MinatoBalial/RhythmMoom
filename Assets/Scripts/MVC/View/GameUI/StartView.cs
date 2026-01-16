using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartView : BaseView
{
    protected override void OnAwake()
    {
        base.OnAwake();
        Find<Button>("StartGame").onClick.AddListener(onStartGameBtn);
        Find<Button>("ContinueGame").onClick.AddListener(onContinueGameBtn);
        Find<Button>("MemoryRecall").onClick.AddListener(onMemoryRecallBtn);
        Find<Button>("AfterStory").onClick.AddListener(onAfterStroyBtn);
    }

    private void onAfterStroyBtn()
    {
    }

    private void onMemoryRecallBtn()
    {
    }

    private void onContinueGameBtn()
    {
    }

    private void onStartGameBtn()
    {
        
    }




}
