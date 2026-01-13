namespace NBWebApp.Utils
{
    /// <summary>
    /// 对象静态类
    /// </summary>
    public class ObjectCopy
    {
        ///// <summary>
        ///// 对象拷贝
        ///// </summary>
        ///// <typeparam name="TIn"></typeparam>
        ///// <typeparam name="TOut"></typeparam>
        ///// <param name="tIn"></param>
        ///// <returns></returns>
        //public static TOut TransReflection<TIn, TOut>(TIn tIn)
        //{
        //    TOut tOut = Activator.CreateInstance<TOut>();
        //    var tInType = tIn.GetType();
        //    foreach (var itemOut in tOut.GetType().GetProperties())
        //    {
        //        var itemIn = tInType.GetProperty(itemOut.Name); ;
        //        if (itemIn != null)
        //        {
        //            itemOut.SetValue(tOut, itemIn.GetValue(tIn));
        //        }
        //    }
        //    return tOut;
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tIn"></param>
        /// <param name="tOut"></param>
        public static void TransReflection<T>(T tIn, T tOut)
        {
            if(tIn!=null&&tOut!=null)
            {
                var tInType = tIn.GetType();

                foreach (var itemOut in tOut.GetType().GetProperties())
                {
                    var itemIn = tInType.GetProperty(itemOut.Name); ;
                    if (itemIn != null && itemOut.GetValue(tOut) != itemIn.GetValue(tIn))
                    {
                        itemOut.SetValue(tOut, itemIn.GetValue(tIn));
                    }
                }
            }
        }

    }
}
