    public static class LinqExtensions
    {
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, string propertyName)
        {

            PropertyInfo prop = typeof(TSource).GetProperty(propertyName);
            if (prop == null)
            {
                throw new Exception("No property '" + propertyName + "' in + " + typeof(TSource).Name + "'");
            }
            return source.OrderBy(x => prop.GetValue(x, null));
        }

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource>(this IEnumerable<TSource> source, string propertyName)
        {
            PropertyInfo prop = typeof(TSource).GetProperty(propertyName);
            if (prop == null)
            {
                throw new Exception("No property '" + propertyName + "' in + " + typeof(TSource).Name + "'");
            }
            return source.OrderByDescending(x => prop.GetValue(x, null));
        }
        
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return (IOrderedQueryable<T>)OrderBy((IQueryable)source, propertyName);
        }

        public static IQueryable OrderBy(this IQueryable source, string propertyName)
        {

            var x = Expression.Parameter(source.ElementType, "x");

            var selector = Expression.Lambda(Expression.PropertyOrField(x, propertyName), x);

            return source.Provider.CreateQuery(

                Expression.Call(typeof(Queryable), "OrderBy", new Type[] { source.ElementType, selector.Body.Type },

                     source.Expression, selector

                     ));

        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return (IOrderedQueryable<T>)OrderByDescending((IQueryable)source, propertyName);
        }

        public static IQueryable OrderByDescending(this IQueryable source, string propertyName)
        {

            var x = Expression.Parameter(source.ElementType, "x");

            var selector = Expression.Lambda(Expression.PropertyOrField(x, propertyName), x);

            return source.Provider.CreateQuery(

                Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { source.ElementType, selector.Body.Type },

                     source.Expression, selector

                     ));
        }
    }
