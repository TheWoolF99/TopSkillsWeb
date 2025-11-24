using Core;
using Core.Logger;
using Data.Services;
using Interfaces.Abonement;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AbonementModel = Core.Abonement.Abonement;

namespace Data.Repository
{
    public class AbonementRepository(DbContextFactory context, LoggerService logger, IHttpContextAccessor httpContextAccessor) : IAbonement

    {
        private DbContextFactory _context = context;
        private readonly LoggerService _log = logger;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        private (string? userName, string? userId) GetCurrentUserInfo()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userName = user?.Identity?.Name;
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value; ;

            return (userName, userId);
        }

        public async Task<IEnumerable<AbonementModel>> GetAllAbonements()
        {
            var db = _context.Create(typeof(AbonementRepository));
            return await db.Abonements.Include(s => s.Student).ToListAsync();
        }

        public async Task<AbonementModel?> GetAbonementStudent(int StudentId)
        {
            var db = _context.Create(typeof(AbonementRepository));
            return db.Abonements.Where(x => x.StudentId == StudentId).FirstOrDefault();
        }

        public async Task<IEnumerable<AbonementModel>> GetAbonementGroupStudents(int groupId)
        {
            var db = _context.Create(typeof(AbonementRepository));
            var Group = await db.Groups.Include(s => s.Students).Where(g => g.GroupId == groupId).FirstOrDefaultAsync();
            List<AbonementModel> result = new();
            if (Group != null)
            {
                result = await db.Abonements.Where(x => Group.Students.Select(x => x.StudentId).Contains(x.StudentId)).ToListAsync();
            }

            return result;
        }

        public async Task AddNewAbonement(AbonementModel abonement)
        {
            var db = _context.Create(typeof(AbonementRepository));
            var StandartRemainingVisits = db.GlobalOptions.Where(x => x.OptionName.ToLower() == "StandartRemainingVisits".ToLower()).FirstOrDefault()?.OptionValue;
            StandartRemainingVisits ??= "4";
            abonement.RemainingVisits = Convert.ToInt32(StandartRemainingVisits);
            await db.Abonements.AddAsync(abonement);

            await AddLogRefreshAbonement(abonement.StudentId.ToString(), "0", StandartRemainingVisits, "Создание абонемента при создании студента");

            await db.SaveChangesAsync();
        }

        public async Task UpdateAbonement(AbonementModel abonement)
        {
            var db = _context.Create(typeof(AbonementRepository));
            db.Abonements.Update(abonement);
            await db.SaveChangesAsync();
        }

        public async Task UpdateCountAbonementByStudentId(int studentId, int countVisits)
        {
            var db = _context.Create(typeof(AbonementRepository));
            var abonement = await db.Abonements.FirstOrDefaultAsync(x => x.StudentId == studentId);
            if (abonement != null)
            {
                var before = abonement.RemainingVisits;
                abonement.RemainingVisits = countVisits;
                await AddLogRefreshAbonement(abonement.StudentId.ToString(), before.ToString(), countVisits.ToString(), "Ручное обновление количества посещений");
            }

            await db.SaveChangesAsync();
        }

        public async Task RefreshAbonement(int StudentId)
        {
            var db = _context.Create(typeof(AbonementRepository));
            var abonement = db.Abonements.Where(x => x.StudentId == StudentId).FirstOrDefault();
            var StandartRemainingVisits = db.GlobalOptions.Where(x => x.OptionName.ToLower() == "StandartRemainingVisits".ToLower()).FirstOrDefault()?.OptionValue;
            StandartRemainingVisits ??= "4";
            if (abonement == null)
                throw new Exception("Не найден абонемент студента");
            if (abonement.RemainingVisits != 0)
                throw new Exception("У студента еще не закончился абонемент");
            if (abonement != null)
            {
                var before = abonement.RemainingVisits;
                abonement.RemainingVisits = Convert.ToInt32(StandartRemainingVisits);
                abonement.StartDate = DateTime.Now;
                await AddLogRefreshAbonement(abonement.StudentId.ToString() ,before.ToString(), StandartRemainingVisits, "Обновление абонемента");
            }

            await db.SaveChangesAsync();
        }

        private async Task AddLogRefreshAbonement(string studentId, string beforeValue, string afterValue, string message)
        {
            var (userName, userId) = GetCurrentUserInfo();
            await _log.AddLog(new LoggerItem
            {
                ServiceName = nameof(AbonementRepository),
                UserId = userId ?? "System",
                UserName = userName ?? "System",
                BeforeValue = beforeValue,
                AfterValue = afterValue,
                OperationName = message,
                ObjectInfo = studentId
            });
        }
    }
}