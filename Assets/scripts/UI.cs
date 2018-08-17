using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]
public class UI {
    [Serializable]
    public class HUD {
        [Header("Text")]
        public Text txtCoinCount;
        public Text txtHeartCount;
        public Text txtTimer;
        [Header("Others")]
        public GameObject hudPanel;
    }
    public HUD hud;
}
