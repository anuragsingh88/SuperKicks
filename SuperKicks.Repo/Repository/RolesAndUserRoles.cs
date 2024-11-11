using Microsoft.EntityFrameworkCore;
using SuperKicks.Data.Models;
using SuperKicks.Repo.Repository.Interface;
using SuperKicks.Repo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SuperKicks.Repo.Repository
{
    public class RolesAndUserRoles(ApplicationDbContext db) : IRoleAndUserRoles
    {
        private readonly ApplicationDbContext _db = db;

        public List<Role> GetRoles()
        {
            return [.. _db.Roles];
        }
        public string AddUpdRole(RoleViewModel viewModel)
        {
            if (viewModel.Id == null || viewModel.Id == Guid.Empty)
            {
                var roleExist = _db.Roles.FirstOrDefault(x => x.Name.ToLower() == viewModel.Name.ToLower());
                if (roleExist != null)
                {
                    return roleExist.IsDeleted
                        ? $"{StatusName.Failed} Role with {viewModel.Name} already exists in inactive state!"
                        : $"{StatusName.Failed} Role with {viewModel.Name} already exists!";
                }
                var newRole = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = viewModel.Name,
                    IsDeleted = false,
                };
                TrackUser.Created(newRole);
                _db.Roles.Add(newRole);
                _db.SaveChanges();
                return "Role added successfully.";
            }
            else
            {
                var role = _db.Roles.FirstOrDefault(x => x.Id == viewModel.Id);
                if (role == null) return $"{StatusName.Failed} Role not found!";
                role.Name = viewModel.Name;
                TrackUser.Updated(role);
                _db.SaveChanges();
                return "Role updated successfully.";
            }
        }
        public string ActiveInactiveRole(bool flag, Guid id)
        {
            var role = _db.Roles.FirstOrDefault(x => x.Id == id);
            if (role == null) return $"{StatusName.Failed}Role not found!";
            role.IsDeleted = flag;
            _db.SaveChanges();
            return StatusName.Success;
        }
        public string AssignUnAssignRolesToUser(UserRoleViewModel viewModel)
        {
            using var scope = new TransactionScope();
            //Assign Roles
            if (viewModel.AssingUnAssignRoles)
            {
                var existingAssignments = _db.UserRoles
                    .Where(x => x.UserId == viewModel.UserId)
                    .Select(x => x.RoleId)
                    .ToList();

                var newAssignments = viewModel.RoleIds.Except(existingAssignments).ToList();
                if (newAssignments.Count != 0)
                {
                    var userRoles = newAssignments.Select(roleID => new UserRole
                    {
                        UserId = viewModel.UserId,
                        RoleId = roleID
                    }).ToList();

                    userRoles.ForEach(ur => TrackUser.Created(ur));
                    _db.UserRoles.AddRangeAsync(userRoles);
                    _db.SaveChangesAsync();
                }
            }
            else
            {
                //UnAssign Role
                _db.UserRoles.Where(x => x.UserId == viewModel.UserId && viewModel.RoleIds.Contains(x.RoleId)).ExecuteDelete();
            }
            scope.Complete();
            return StatusName.Success;
        }
    }
}
