﻿using Core;
using Interfaces.Attendance;
using Interfaces.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class AttendanceService
    {
        private readonly IAttendance _attendance;

        public AttendanceService(IAttendance attendance) => this._attendance = attendance;



        public async Task<Attendance> GetAttendanceByGroupIdAndDate(int GroupId, DateTime Date)
        {
            return await _attendance.GetAttendanceByGroupIdAndDate(GroupId, Date);
        }

        public async Task<IEnumerable<Attendance>> GetAttendancesByDateRange(DateTime start, DateTime end)
        {
            return await _attendance.GetAttendancesByDateRange(start, end);
        }

        public async Task<bool> OnAddAttendanceByDateAndGroupId(Attendance attendance)
        {
            return await _attendance.OnAddAttendanceByDateAndGroupId(attendance);
        }


    }
}
