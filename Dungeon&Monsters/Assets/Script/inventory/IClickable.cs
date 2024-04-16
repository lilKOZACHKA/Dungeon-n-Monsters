using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.UI;

    public interface IClickable
    {
        Image MyIcon
        {
            get;
            set;
        }
        int MyCount
        {
            get;
        }
    }
