﻿using System;

namespace VisualStudio
{
    class NicknameOption : OptionSet<string>
    {
        public NicknameOption()
        {
            Add("nick|nickname:", "Optional nickname to use", n => Value = n);
        }
    }
}
