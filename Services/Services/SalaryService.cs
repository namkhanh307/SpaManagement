using AutoMapper;
using Core.Infrastructures;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repos.Entities;
using Repos.IRepos;
using Repos.ViewModels.SalaryVM;
using Services.IServices;

namespace Services.Services
{
    public class SalaryService : ISalaryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SalaryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task PostAsync(PostSalaryVM model)
        {
            Schedule? schedule = await _unitOfWork.GetRepo<Schedule>().Entities.Where(s => s.Date.Month == model.Month && s.Date.Year == model.Year).FirstOrDefaultAsync() ?? throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Lich lam viec khong ton tai!");
            List<UserSchedule> userSchedules = await _unitOfWork.GetRepo<UserSchedule>().Entities.Where(u => u.ScheduleId == schedule.Id).ToListAsync();

            var groupUserSchedules = userSchedules.GroupBy(u => u.UserId);
            foreach (var group in groupUserSchedules)
            {
                // If a specific UserId is provided, process only that user
                if (!string.IsNullOrWhiteSpace(model.UserId) && group.Key != model.UserId) continue;
                // Retrieve the user
                User? user = await _unitOfWork.GetRepo<User>().Entities.Where(u => u.Id == group.Key).Include(U => U.PayRates).FirstOrDefaultAsync()
                    ?? throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Người dùng không tồn tại!");

                // Retrieve the pay rate
                PayRate? payRate = user.PayRates.FirstOrDefault()
                    ?? throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, $"Lương theo giờ của người dùng {user.FullName} không tồn tại!");

                // Calculate total salary for the user
                double totalSalary = group.Sum(us =>
                {
                    if (us.StartTime.HasValue && us.EndTime.HasValue)
                    {
                        var hoursWorked = (us.EndTime.Value - us.StartTime.Value).TotalHours;
                        return hoursWorked * payRate.Amount;
                    }
                    return 0;
                });

                // Create and save the salary record
                Salary salary = new()
                {
                    UserId = group.Key,
                    Total = totalSalary,
                    Month = model.Month,
                    Year = model.Year
                };

                await _unitOfWork.GetRepo<Salary>().Insert(salary);
            }
            await _unitOfWork.Save();
        }
    }
}
