
using System;
using UnityEngine;

namespace Elecular.Core
{
    [Serializable]
    public class Variation
    {
        [SerializeField]
        private string variationName;

        [SerializeField]
        private Setting[] variables;
        
        /// <summary>
        /// Name of the variation
        /// </summary>
        public string Name
        {
            get
            {
                return variationName;
            }
        }
        
        /// <summary>
        /// All the settings of this variation
        /// </summary>
        public Setting[] Settings
        {
            get
            {
                return variables;
            }
        }
    }
    
    [Serializable]
    public class Setting
    {
        [SerializeField]
        private string variableName;
        
        [SerializeField]
        private string variableValue;
        
        /// <summary>
        /// Name of the setting
        /// </summary>
        public string Name 
        {
            get
            {
                return variableName;
            }
        }
        
        /// <summary>
        /// Value of the setting
        /// </summary>
        public string Value
        {
            get
            {
                return variableValue;
            }
        }
    }
}

