﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public interface IMainGameView {
    Animator Animator { get; }
    GameObject CurrentGameObject { get; }
}

