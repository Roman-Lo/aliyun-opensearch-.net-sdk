﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliCloudOpenSearch.com.API.Builder
{
    /// <summary>
    /// Used to generate filter clause
    /// </summary>
    public class Filter : IBuilder
    {
        private IList<IBuilder> _andFilters = new List<IBuilder>();
        private IList<IBuilder> _orFilters = new List<IBuilder>();

        private string _filter;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filter">filter</param>
        public Filter(string filter)
        {
            Utilities.Guard(() => !string.IsNullOrEmpty(filter), "filter cannot be null or empty");

            _filter = filter;
        }

        /// <summary>
        /// Add 'and' filter
        /// </summary>
        /// <param name="filter">filter</param>
        /// <returns>Filter instance</returns>
        public Filter And(Filter filter)
        {
            _andFilters.Add(filter);
            return this;
        }

        /// <summary>
        /// Add 'or' filter
        /// </summary>
        /// <param name="filter">filter</param>
        /// <returns>Filter instance</returns>
        public Filter Or(Filter filter)
        {
            _orFilters.Add(filter);
            return this;
        }

        /// <summary>
        /// Generate filter clause
        /// </summary>
        /// <returns></returns>
        string IBuilder.BuildQuery()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append(_filter);

            Action<IList<IBuilder>, string> func = (lstQyr, op) =>
            {
                foreach (var q in lstQyr)
                {
                    qry.Append(op).Append("(").Append(q.BuildQuery()).Append(")");
                }
            };

            func(_andFilters, " AND ");
            func(_orFilters, " OR ");

            return qry.ToString();
        }
    }
}
