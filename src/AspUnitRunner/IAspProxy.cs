﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace AspUnitRunner {
    public interface IAspProxy {
        string GetTestResults(string uri, string postData, ICredentials credentials);
    }
}