
using System;

namespace Elecular.Core
{
    [Serializable]
    public class Variation
    {
        public string variationName;

        public bool controlGroup;

        public Variable[] variables;
    }
    
    [Serializable]
    public class Variable
    {
        public string variableName;

        public string variableType;

        public string variableValue;
    }
}

