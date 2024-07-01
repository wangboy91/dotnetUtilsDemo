using System;
using System.Collections.Generic;

namespace Common.Utils
{
    /// <summary>
    /// 通过委托快速实现自定义Compare方法
    /// 判断优先级
    /// 引用地址是否相同
    /// 如果任意一个对象为空，返回空
    /// 判断Type类别是否一致
    /// 判断查询委托条件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CustomComparer<T> : IEqualityComparer<T> where T : class
    {
        private readonly Func<T, object> _proValueGetFunc;

        public CustomComparer(Func<T, object> getPropertyValueFunc)
        {
            _proValueGetFunc = getPropertyValueFunc;
        }

        public bool Equals(T x, T y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            object xValue = _proValueGetFunc(x);
            object yValue = _proValueGetFunc(y);
            return xValue.Equals(yValue);
        }

        public int GetHashCode(T obj)
        {
            object propertyValue = _proValueGetFunc(obj);
            return propertyValue == null ? 0 : propertyValue.GetHashCode();
        }
    }
}