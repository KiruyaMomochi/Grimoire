using System;
using System.Collections.Generic;
using System.Reflection;
using Grimoire.Explore.Infrastructure;
using Grimoire.Explore.Parameter;

namespace Grimoire.Explore.Abstractions
{
    public abstract class CommandDescriptor
    {
        public string Command { get; init; }
        public SourceSet SourceSet { get; init; }
        public string DisplayName { get; init; }

        public IList<ParameterInfo> Parameters { get; init; }
        #nullable enable
        public IList<ParameterType> ParameterTypes
        {
            get
            {
                if (_parameterTypes != null)
                    return _parameterTypes;
                
                if (Parameters == null)
                    throw new ArgumentNullException();
                
                _parameterTypes = new List<ParameterType>(Parameters.Count);
                
                foreach (var parameter in Parameters) 
                    _parameterTypes.Add(ConvertParameter(parameter.ParameterType));
                
                return _parameterTypes;
            }
        }
        #nullable restore

        private IList<ParameterType> _parameterTypes;
        
        private static ParameterType ConvertParameter(Type reflectionType)
        {
            if (reflectionType == typeof(int))
                return ParameterType.Signed;
            if (reflectionType == typeof(uint))
                return ParameterType.Unsigned;
            if (reflectionType == typeof(string))
                return ParameterType.String;

            throw new NotImplementedException();
        }
    }
}
