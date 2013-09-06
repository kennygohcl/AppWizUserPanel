using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dFrontierAppWizard.WebUI.Utils
{
    public static class NullableType
    {
       public static Type GetNullableType(Type type)
        {
            // Use Nullable.GetUnderlyingType() to remove the Nullable<T> wrapper if type is already nullable.
            type = Nullable.GetUnderlyingType(type);
            if (type.IsValueType)
                return typeof(Nullable<>).MakeGenericType(type);
            else
                return type;
        }
    }
}