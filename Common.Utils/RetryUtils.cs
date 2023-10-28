using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Utils
{
    /// <summary>
    /// 重试执行操作帮助类
    /// </summary>
    public static class RetryUtils
    {
        public static RetryPolicy<TResult> HandleResult<TResult>(Func<TResult, bool> retryCondition)
        {
            return new RetryPolicy<TResult>(retryCondition);
        }
        public static RetryPolicy<TResult> Handle<TResult>(Func<Exception, bool> retryCondition)
        {
            return new RetryPolicy<TResult>(retryCondition);
        }
    }
    /// <summary>
    /// 构建重试操作
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class RetryPolicy<TResult>
    {
        /// <summary>
        /// 最大重试此时 默认最大10次
        /// </summary>
        private int MaxRetryCount { set; get; }

        /// <summary>
        /// 睡眠间隔 默认间隔1秒
        /// </summary>
        private TimeSpan SleepTimeSpan { set; get; }

        /// <summary>
        /// 重试条件按结果 返回执行结果  是否
        /// </summary>
        private Func<TResult, bool> RetryResultCondition { set; get; }

        /// <summary>
        /// 重试条件按异常 返回执行结果  是否
        /// </summary>
        private Func<Exception, bool> RetryExceptionCondition { set; get; }
        /// <summary>
        /// 重试策略 指定间隔  当前重试次数，返回 重试间隔
        /// </summary>
        private Func<int, TimeSpan> SleepNumberDurationProvider { set; get; }
        public RetryPolicy(Func<TResult, bool> retryCondition)
        {
            MaxRetryCount = 10;
            SleepTimeSpan = TimeSpan.FromSeconds(1);
            RetryResultCondition = retryCondition;
        }
        public RetryPolicy(Func<Exception, bool> retryCondition)
        {
            MaxRetryCount = 10;
            SleepTimeSpan = TimeSpan.FromSeconds(1);
            RetryExceptionCondition = retryCondition;
        }
        public RetryPolicy<TResult> SetMaxRetryCount(int retryMaxCount)
        {
            MaxRetryCount = retryMaxCount;
            return this;
        }

        public RetryPolicy<TResult> SetSleepTimeSpan(TimeSpan sleepTimeSpan)
        {
            SleepTimeSpan = sleepTimeSpan;
            return this;
        }
        public RetryPolicy<TResult> SetSleepTimeSpanSeconds(int seconds)
        {
            SleepTimeSpan = TimeSpan.FromSeconds(seconds);
            return this;
        }
        
        public RetryPolicy<TResult> WaitAndRetry(int retryMaxCount, Func<int, TimeSpan> sleepDurationProvider)
        {
            MaxRetryCount = retryMaxCount;
            SleepNumberDurationProvider = sleepDurationProvider;
            return this;
        }
        public RetryPolicy<TResult> WaitAndRetry(Func<int, TimeSpan> sleepDurationProvider)
        {
            SleepNumberDurationProvider = sleepDurationProvider;
            return this;
        }
        public async Task<TResult> ExecuteAsync(Func<Task<TResult>> action)
        {
            TResult t = default(TResult);
            if (action == null)
            {
                return t;
            }

            var curCount = 0;
            while (curCount < MaxRetryCount)
            {
                try
                {
                    t = await action.Invoke();
                }
                catch (Exception ex)
                {
                    if (RetryExceptionCondition != null && RetryExceptionCondition.Invoke(ex))
                    {
                        if (SleepNumberDurationProvider != null)
                        {
                            SleepTimeSpan = SleepNumberDurationProvider(curCount);
                       
                        }
                        Thread.Sleep(SleepTimeSpan);
                        curCount++;
                        continue;
                    }
                    throw;
                }
                
                if (RetryResultCondition == null)
                {
                    break;
                }
                var yes = RetryResultCondition.Invoke(t);
                if (!yes)
                {
                    break;
                }

                if (SleepNumberDurationProvider != null)
                {
                    SleepTimeSpan = SleepNumberDurationProvider(curCount);
                }

                Thread.Sleep(SleepTimeSpan);
                curCount++;
            }

            return t;
        }
        public TResult Execute(Func<TResult> action)
        {
            TResult t = default(TResult);
            if (action == null)
            {
                return t;
            }

            var curCount = 0;
            while (curCount < MaxRetryCount)
            {
                try
                {
                    t = action.Invoke();
                }
                catch (Exception ex)
                {
                    if (RetryExceptionCondition != null && RetryExceptionCondition.Invoke(ex))
                    {
                        if (SleepNumberDurationProvider != null)
                        {
                            SleepTimeSpan = SleepNumberDurationProvider(curCount);
                       
                        }
                        Thread.Sleep(SleepTimeSpan);
                        curCount++;
                        continue;
                    }
                    throw;
                }
                
                if (RetryResultCondition == null)
                {
                    break;
                }
                var yes = RetryResultCondition.Invoke(t);
                if (!yes)
                {
                    break;
                }

                if (SleepNumberDurationProvider != null)
                {
                    SleepTimeSpan = SleepNumberDurationProvider(curCount);
                }

                Thread.Sleep(SleepTimeSpan);
                curCount++;
            }

            return t;
        }
    }
}