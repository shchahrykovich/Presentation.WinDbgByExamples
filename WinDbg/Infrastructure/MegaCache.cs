using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Infrastructure
{
    public class MegaCache
    {
        public static List<Type> SafeTypes = new List<Type>();

        static MegaCache()
        {
            SafeTypes.Add(typeof(UserProfile));
            SafeTypes.Add(typeof(String));
            SafeTypes.Add(typeof(DateTime));
        }

        public void Set<TValue>(string key, TValue value) where TValue : class
        {
            if (IsSafeToCache(typeof (TValue)))
            {
                HttpContext.Current.Cache[key] = value;
            }
        }

        public TValue Get<TValue>(String key) where TValue : class
        {
            object val = HttpContext.Current.Cache.Get(key);
            return val as TValue;
        }

        private static bool IsSafeToCache(Type type)                           
        {                                                                      
            try                                                                
            {                                                                  
                Type typeToCheck = type ;                                      
                if (type.IsGenericType)                                        
                {                                                              
                    typeToCheck = type.GetGenericArguments().SingleOrDefault();
                }                                                              
                                                                       
                return SafeTypes.Contains(typeToCheck);                        
            }                                                                  
            catch (Exception)                                                  
            {                                                                  
                return false;                                                  
            }                                                                  
        }                                                                      
    }
}


#region Answer

//private static bool IsSafeToCache(Type type)
//{
//    try
//    {
//        Type typeToCheck = type ;
//        if (type.IsGenericType)
//        {
//            typeToCheck = type.GetGenericArguments().SingleOrDefault();
//        }

//        return SafeTypes.Contains(typeToCheck);
//    }
//    catch (Exception)
//    {
//        return false;
//    }
//}

//FIX:
//private static bool IsSafeToCache(Type type)
//{
//    try
//    {
//        Type[] typesToCheck = {type};
//        if (type.IsGenericType)
//        {
//            typesToCheck = type.GetGenericArguments();
//        }

//        return typesToCheck.All(t => SafeTypes.Contains(t));
//    }
//    catch (Exception)
//    {
//        return false;
//    }
//}

#endregion