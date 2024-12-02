using System.Collections.Generic;
using System.Linq;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHr.Authorization.Roles;
using ProjectHr.Authorization.Users;
using ProjectHr.Controllers;
using ProjectHr.Enums;
using ProjectHr.ExportFiles;
using ProjectHr.Extensions;
using ProjectHr.Users.Dto;

namespace ProjectHr.Controllers
{
    [AbpAuthorize]
    [Route("api/")]
    public class ExportController : ProjectHrControllerBase
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly RoleManager _roleManager;
        
        public ExportController(
            IRepository<User, long> userRepository,
            RoleManager roleManager
        )
        {
            _userRepository = userRepository;
            _roleManager = roleManager;
        }
        

        [HttpPost("users/export-file")]
        public IActionResult Users([FromBody] ExportUserWithRoleInput withRoleInput)
        {
            var users = _userRepository.GetAll()
                .Include(x => x.Roles)
                .ToList();
            
            var roles = _roleManager.Roles.ToList();
            
            var newUsers = users.Select(user => new
            {
                User = user,
                RoleNames = string.Join(", ", user.Roles.Select(userRole => roles.FirstOrDefault(role => role.Id == userRole.RoleId)?.Name).ToList()),
                Status = user.IsActive ? "Aktif Çalışan" : "Eski Çalışan"
            }).ToList();
            var exportHelper = new ExportFileHelper();
            
            var bytes = exportHelper.ExportFile(withRoleInput.ExportType, newUsers,
                withRoleInput.Columns.Select(x => x.GetAlternateValue()).ToArray(),
                withRoleInput.Columns.Select(x => x.GetDescription()).ToArray());
            
            return GenerateExportFile(withRoleInput.ExportType, bytes);
            return null;
        }

        private IActionResult GenerateExportFile(DataConverter ExportType, byte[] bytes)
        {
            if (ExportType == DataConverter.Excel)
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "users.xlsx");

            if (ExportType == DataConverter.Csv)
                return File(bytes, "text/csv", "users.csv");

            return File(bytes, "application/pdf", "user.pdf");
        }
    }
}