﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

interface ITaskEntryController {
    ITask TaskBeingDisplayed { set; }
}
