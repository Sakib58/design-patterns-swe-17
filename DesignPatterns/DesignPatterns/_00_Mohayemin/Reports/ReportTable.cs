﻿using static System.DayOfWeek;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;

namespace DesignPatterns._00_Mohayemin.Reports
{
    class ReportTable : IRenderable
    {
        private List<ReportRow> rows;
        private List<ReportColumn> cols;
        private ReportCell[,] cells;

        public ReportTable(List<ClassHour> classHours)
        {
            BuildComponents(classHours);
        }

        public void Render(StringBuilder builder)
        {
            for (int r = 0; r < cells.GetLength(0); r++)
            {
                builder.Append("|");
                for (int c = 0; c < cells.GetLength(1); c++)
                {
                    builder.Append(" ");
                    cells[r, c].Render(builder);
                    builder.Append(" |");
                }
                builder.Append("\n");
            }
        }

        private void BuildComponents(List<ClassHour> classHours)
        {
            var departments = classHours.ConvertAll(ch => ch.department).Distinct().ToList();

            rows = new List<ReportRow>();
            rows.Add(new ReportRow());
            foreach (var department in departments)
            {
                rows.Add(new ReportRow());
            }
            rows.Add(new ReportRow());
            //rows = departments.ConvertAll(d => new ReportRow(d, classHours.FindAll(ch => ch.department == d)));


            cols = new List<ReportColumn>();
            cols.Add(new ReportColumn());

            cells = new ReportCell[1 + departments.Count /*+ 1*/, 1 + 7 + 1];

            int r = 0, c = 0;
            cells[r, c] = new ReportCell("Department");
            for (c++; c <= 7; c++)
            {
                cells[r, c] = new ReportCell(AllDaysOfWeek.FromMonday[c - 1].ToString());
            }
            cells[r, c] = new ReportCell("Total");

            for (r++; r < departments.Count + 1; r++)
            {
                c = 0;
                var department = departments[r - 1];
                cells[r, c] = new ReportCell(department);
                var total = 0;
                for (c++; c <= 7; c++)
                {
                    var dow = AllDaysOfWeek.FromMonday[c - 1];
                    var value = classHours.FindAll(ch => ch.date.DayOfWeek == dow && ch.department == department).Sum(ch => ch.durationHours);
                    cells[r, c] = new ReportDataCell(value);
                    total += 0;
                }
                cells[r, c] = new ReportDataCell(total);
            }

            
        }
    }

    class ReportRow
    {
        private readonly ReportCell departmentCell;
        private readonly List<ReportCell> dayCells;
        private readonly ReportCell totalCell;

        public ReportRow()
        {

        }
    }

    class ReportColumn
    {
        private List<ReportCell> allCells;
        private ReportCell headerCell;
        private List<ReportDataCell> dataCells;
        public ReportColumn()
        {
            allCells = new List<ReportCell>();
        }

        public void SetHeaderCell(ReportCell headerCell)
        {
            this.headerCell = headerCell;
            allCells.Add(headerCell);
        }

        public void AddDataCell(ReportDataCell dataCell)
        {
            allCells.Add(dataCell);
            dataCells.Add(dataCell);
        }

        public int CalculateWidth()
        {
            return allCells.Max(c => c.content.Length);
        }
    }

    class ReportCell : IRenderable
    {
        public readonly string content;
        public ReportCell(string content)
        {
            this.content = content;
        }

        public void Render(StringBuilder builder)
        {
            builder.Append($"{content}");
        }
    }

    class ReportDataCell : ReportCell
    {
        public readonly double value;

        public ReportDataCell(double value) : base(value.ToString())
        {
            this.value = value;
        }
    }

    interface IRenderable
    {
        void Render(StringBuilder builder);
    }

    static class AllDaysOfWeek
    {
        public static DayOfWeek[] FromMonday = new[] { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday };
    }
}