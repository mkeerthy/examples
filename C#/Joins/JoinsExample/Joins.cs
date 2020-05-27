﻿using System.Collections.Generic;

namespace System.Linq
{
    public static class Joins
    {
        //public static IEnumerable<(TLeft l, TRight r)> InnerJoin<TLeft, TRight>(this IEnumerable<TLeft> left,
        //    Func<TLeft, int> keySelector,
        //    IEnumerable<TRight> right,
        //    Func<TRight, int> fkSelector)
        //{
        //    var rdict = right.ToLookup(fkSelector);
        //    foreach (var l in left)
        //    {
        //        var lkey = keySelector(l);
        //        if (rdict.Contains(lkey))
        //        {
        //            foreach (var r in rdict[lkey])
        //            {
        //                yield return (l, r);
        //            }
        //        }
        //    }
        //}

        public static IEnumerable<(TLeft l, TRight r)> InnerJoin<TLeft, TRight>(this IEnumerable<TLeft> left,
            IEnumerable<TRight> right,
            Func<TLeft, TRight, bool> where)
        {
            foreach (var l in left)
            {
                foreach (var r in right)
                {
                    if (where(l, r))
                    {
                        yield return (l, r);
                    }
                }
            }
        }

        //public static IEnumerable<(TLeft l, TRight r)> LeftJoin<TLeft, TRight>(this IEnumerable<TLeft> left,
        //    Func<TLeft, int> keySelector,
        //    IEnumerable<TRight> right,
        //    Func<TRight, int> fkSelector,
        //    Func<TRight> nullObjectFactory)
        //{
        //    var rdict = right.ToLookup(fkSelector);
        //    foreach (var l in left)
        //    {
        //        var lkey = keySelector(l);
        //        if (rdict.Contains(lkey))
        //        {
        //            foreach (var r in rdict[lkey])
        //            {
        //                yield return (l, r);
        //            }
        //        }
        //        else
        //        {
        //            yield return (l, nullObjectFactory());
        //        }
        //    }
        //}

        public static IEnumerable<(TLeft l, TRight r)> LeftJoin<TLeft, TRight>(this IEnumerable<TLeft> left,
            IEnumerable<TRight> right,
            Func<TRight> nullObjectFactory,
            Func<TLeft, TRight, bool> where)
        {
            foreach (var l in left)
            {
                var founded = false;
                foreach (var r in right)
                {
                    if (where(l, r))
                    {
                        yield return (l, r);
                        founded = true;
                    }
                }

                if (!founded)
                    yield return (l, nullObjectFactory());
            }
        }

        public static IEnumerable<(TLeft l, TRight r)> LeftJoin<TLeft, TRight>(this IEnumerable<TLeft> left,
            IEnumerable<TRight> right,
            TRight nullObject,
            Func<TLeft, TRight, bool> where)
        {
            return LeftJoin(left, right, () => nullObject, where);
        }

        public static IEnumerable<(TLeft l, TRight r)> LeftJoin<TLeft, TRight>(this IEnumerable<TLeft> left,
            IEnumerable<TRight> right,
            Func<TLeft, TRight, bool> where)
        {
            return LeftJoin(left, right, default(TRight), where);
        }

        //public static IEnumerable<(TLeft l, TRight r)> RightJoin<TLeft, TRight>(this IEnumerable<TLeft> left,
        //    Func<TLeft, int> pkSelector,
        //    IEnumerable<TRight> right,
        //    Func<TRight, int> fkSelector,
        //    Func<TLeft> nullObjectFactory)
        //{
        //    var ldict = left.ToLookup(pkSelector);
        //    foreach (var r in right)
        //    {
        //        var rkey = fkSelector(r);
        //        if (ldict.Contains(rkey))
        //        {
        //            foreach (var l in ldict[rkey])
        //            {
        //                yield return (l, r);
        //            }
        //        }
        //        else
        //        {
        //            yield return (nullObjectFactory(), r);
        //        }
        //    }
        //}

        public static IEnumerable<(TLeft l, TRight r)> RightJoin<TLeft, TRight>(this IEnumerable<TLeft> left,
            Func<TLeft> nullObjectFactory,
            IEnumerable<TRight> right,
            Func<TLeft, TRight, bool> where)
        {
            foreach (var r in right)
            {
                var founded = false;
                foreach (var l in left)
                {
                    if (where(l, r))
                    {
                        yield return (l, r);
                        founded = true;
                    }
                }

                if (!founded)
                    yield return (nullObjectFactory(), r);
            }
        }

        public static IEnumerable<(TLeft l, TRight r)> RightJoin<TLeft, TRight>(this IEnumerable<TLeft> left,
            TLeft nullObject,
            IEnumerable<TRight> right,
            Func<TLeft, TRight, bool> where)
        {
            return RightJoin(left, () => nullObject, right, where);
        }

        public static IEnumerable<(TLeft l, TRight r)> RightJoin<TLeft, TRight>(this IEnumerable<TLeft> left,
            IEnumerable<TRight> right,
            Func<TLeft, TRight, bool> where)
        {
            return RightJoin(left, default(TLeft), right, where);
        }

        //public static IEnumerable<(TLeft l, TRight r)> OuterJoin<TLeft, TRight>(this IEnumerable<TLeft> left,
        //    Func<TLeft, int> keySelector,
        //    Func<TLeft> lNullObjectFactory,
        //    IEnumerable<TRight> right,
        //    Func<TRight, int> fkSelector,
        //    Func<TRight> rNullObjectFactory)
        //{
        //    return LeftJoin(left, keySelector, right, fkSelector, rNullObjectFactory)
        //        .Concat(RightJoin(left, keySelector, right, fkSelector, lNullObjectFactory));
        //}

        public static IEnumerable<(TLeft l, TRight r)> OuterJoin<TLeft, TRight>(this IEnumerable<TLeft> left,
            Func<TLeft> lNullObjectFactory,
            IEnumerable<TRight> right,
            Func<TRight> rNullObjectFactory,
            Func<TLeft, TRight, bool> where)
        {
            return LeftJoin(left, right, rNullObjectFactory, where)
                .Concat(RightJoin(left, lNullObjectFactory, right, where));
        }

        public static IEnumerable<(TLeft l, TRight r)> OuterJoin<TLeft, TRight>(this IEnumerable<TLeft> left,
            TLeft lNullObject,
            IEnumerable<TRight> right,
            TRight rNullObject,
            Func<TLeft, TRight, bool> where)
        {
            return OuterJoin(left, () => lNullObject, right, () => rNullObject, where);
        }

        public static IEnumerable<(TLeft l, TRight r)> OuterJoin<TLeft, TRight>(this IEnumerable<TLeft> left,
            IEnumerable<TRight> right,
            Func<TLeft, TRight, bool> where)
        {
            return OuterJoin(left, default(TLeft), right, default(TRight), where);
        }
    }
}
