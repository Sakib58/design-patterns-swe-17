﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Reports.Kpis
{
    public abstract class Kpi : IKpi
    {

        private readonly IEnumerable<ClassInfo> classHours;
        private readonly DayOfWeek dayOfWeek;

        public Kpi(IEnumerable<ClassInfo> classHours
            , DayOfWeek dayOfWeek)
        {
            this.classHours = classHours;
            this.dayOfWeek = dayOfWeek;
        }
        public IDictionary<string, double> Calculate()
        {
            var filtered = classHours.Where(ch => ch.date.DayOfWeek == dayOfWeek);
            var grouped = filtered.GroupBy(ch => ch.department);
            var mapped = GroupToDictionary(grouped);
            return mapped;
        }

        protected abstract IDictionary<string, double> GroupToDictionary(IEnumerable<IGrouping<string, ClassInfo>> grouped);
    }
}
