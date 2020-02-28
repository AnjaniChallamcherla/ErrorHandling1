using System;
using System.Reflection;

namespace TalentErrorHandling
{
    public static class GetValue
    {

        public static Tuple<string, string> GetPropValue(this Object obj, String propName)
        {
            string[] nameParts = propName.Split('.');
            string resultType = string.Empty;
            string resultValue = string.Empty;
            if (nameParts.Length == 1)
            {
                //check if property exists
                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(propName);
                if (info == null)
                {
                    resultType = string.Empty;
                    resultValue = string.Empty;
                    return Tuple.Create(resultType, resultValue);
                }
                resultType = obj.GetType().GetProperty(propName).PropertyType.Name;
                object obje = obj.GetType().GetProperty(propName).GetValue(obj, null);
                if (obje != null)
                {
                    resultValue = obje.ToString();
                }
                else
                {
                    resultValue = string.Empty;
                }

                return Tuple.Create(resultType, resultValue);
            }

            foreach (String part in nameParts)
            {
                if (obj == null)
                {
                    resultType = string.Empty;
                    resultValue = string.Empty;
                    return Tuple.Create(resultType, resultValue);
                }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null)
                {
                    resultType = string.Empty;
                    resultValue = string.Empty;
                    return Tuple.Create(resultType, resultValue);
                }

                obj = info.GetValue(obj, null);

                resultType = info.PropertyType.Name;
                if (obj != null)
                {
                    resultValue = obj.ToString(); ;
                }
                else
                {
                    resultValue = string.Empty;
                }

            }
            return Tuple.Create(resultType, resultValue);
        }
    }
}
