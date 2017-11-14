using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;

namespace SmartValley.Data.SQL.Tests
{
    public class TestEnvironment
    {
        public readonly AppDBContext Context;

        public TestEnvironment()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDBContext>();
            optionsBuilder.UseSqlServer(@"Data Source=.\SQLEXPRESS;Initial Catalog=smartValleyDB;Integrated Security=True;MultipleActiveResultSets=True");
            Context = new AppDBContext(optionsBuilder.Options);
        }

        /// <summary>
        /// Добавление в базу
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task LoadData<T>(IEnumerable<T> entities) where T : class
        {
            await Context.AddRangeAsync(entities);
            Context.SaveChanges();
        }

        /// <summary>
        /// Удаление из базы
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        public void DeleteData<T>(IEnumerable<T> entities) where T : class
        {
            Context.RemoveRange(entities);
            Context.SaveChanges();
        }
    }
}
