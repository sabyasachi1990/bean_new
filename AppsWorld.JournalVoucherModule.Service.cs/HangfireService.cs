using AppsWorld.BillModule.Entities;
using AppsWorld.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AppsWorld.JournalVoucherModule.Service.cs
{
    public class HangfireService:IHangfireService
    {
        //#region  Hangfire
        //JournalContext context = new JournalContext();

        //[AutomaticRetry(Attempts = 1)]
        //public void archieveleaveentitlements()
        //{
        //    string dateandmonth = DateTime.Now.ToString("mm-dd");
        //    var lstbeansettings = from a in context.FinancialSettings
        //                          where a.status == RecordStatusEnum.Active &&
        //                              a.periodenddate.tostring().contains(dateandmonth)
        //                          select a;
        //    var sample = lstbeansettings.tolist();
        //    if (sample.any())
        //    {
        //        foreach (var beansettings in sample)
        //        {


        //            var lstleaveentitlement =
        //                context.employees.where(
        //                    a => a.status == recordstatusenum.active && a.companyid == hrsettings.companyid)
        //                    .selectmany(c => c.leaveentitlements.where(a => a.status == recordstatusenum.active)).include(c => c.leavetype)
        //                    .tolist();
        //            if (lstleaveentitlement.count > 0)
        //            {
        //                foreach (var leaveentitlement in lstleaveentitlement)
        //                {
        //                    //leaveentitlement leaveentitlements = new leaveentitlement();
        //                    //leaveentitlements = leaveentitlement;
        //                    //leaveentitlements.status = recordstatusenum.inactive;
        //                    //// context.entry(leaveentitlements).state = entitystate.modified;
        //                    //lstentitlements.add(leaveentitlements);
        //                    leaveentitlement entitlement = leaveentitlement;
        //                    entitlement.status = recordstatusenum.inactive;
        //                    entitlement.objectstate = objectstate.modified;
        //                    context.savechanges();


        //                }

        //                //context.leaveentitlements.addrange(lstentitlements);
        //                //context.savechangesasync();
        //                generatenewentiltements(lstleaveentitlement);
        //            }
        //        }
        //    }
        //}

        ////private void GenerateNewEntiltements(List<LeaveEntitlement> lstLeaveEntitlement)
        ////{
        ////    List<LeaveEntitlement> lstLeaveEntitlements = new List<LeaveEntitlement>();
        ////    double? leaveBalance = null;
        ////    foreach (var leaveEntitlement in lstLeaveEntitlement)
        ////    {
        ////        LeaveEntitlement leaveEntitlements = new LeaveEntitlement();
        ////        leaveEntitlements.Id = Guid.NewGuid();
        ////        leaveEntitlements.CreatedDate = DateTime.UtcNow;
        ////        leaveEntitlements.Status = RecordStatusEnum.Active;
        ////        leaveEntitlements.LeaveTypeId = leaveEntitlement.LeaveTypeId;
        ////        leaveEntitlements.EmployeeId = leaveEntitlement.EmployeeId;
        ////        leaveEntitlements.AnnualLeaveEntitlement = leaveEntitlement.LeaveType.NoOfLeaves;

        ////        leaveEntitlements.StartDate = DateTime.UtcNow;

        ////        leaveEntitlements.EndDate = leaveEntitlements.StartDate.Value.AddYears(1);
        ////        leaveBalance = (((leaveEntitlement.Prorated ?? 0) + (leaveEntitlement.BroughtForward ?? 0) +
        ////                         (leaveEntitlement.ApprovedAndNotTaken ?? 0)) - (leaveEntitlement.ApprovedAndTaken ?? 0));
        ////        if (leaveBalance != 0 && leaveEntitlement.LeaveType.IsCarryForward == true)
        ////        {
        ////            if (leaveEntitlement.LeaveType.NoOfDays != null)
        ////            {
        ////                double sum = Convert.ToDouble(leaveEntitlement.LeaveType.NoOfDays);
        ////                if (leaveBalance < sum)
        ////                {
        ////                    leaveEntitlements.BroughtForward = leaveBalance;
        ////                }
        ////                else
        ////                {
        ////                    leaveEntitlements.BroughtForward = sum;
        ////                }
        ////            }
        ////        }
        ////        else
        ////        {
        ////            leaveEntitlements.BroughtForward = 0;
        ////        }
        ////        leaveEntitlements.LeaveApprovers = leaveEntitlement.LeaveApprovers;
        ////        leaveEntitlements.ApprovedAndTaken = 0;
        ////        leaveEntitlements.ApprovedAndNotTaken = 0;
        ////        if (leaveEntitlement.LeaveType.LeaveAccuralType == "Yearly")
        ////        {
        ////            leaveEntitlements.Prorated = leaveEntitlement.LeaveType.NoOfLeaves;
        ////        }
        ////        else
        ////        {
        ////            leaveEntitlements.Prorated = 0;
        ////        }
        ////        leaveEntitlements.LeaveRecommenders = leaveEntitlement.LeaveRecommenders;
        ////        leaveEntitlements.ObjectState = ObjectState.Added;
        ////        lstLeaveEntitlements.Add(leaveEntitlements);
        ////    }
        ////    context.LeaveEntitlements.AddRange(lstLeaveEntitlements);s
        ////    context.SaveChangesAsync();
        ////}

        ////public void GetProratedLeaves()
        ////{
        ////    IQueryable<LeaveEntitlement> lstLeaveEntitlements =
        ////        context.LeaveEntitlements.Where(c => c.Status == RecordStatusEnum.Active)
        ////            .Include(c => c.LeaveType).Include(c => c.Employee).Include(c => c.Employee.Employments).Where(c => c.LeaveType.LeaveAccuralType == "Monthly" && c.Status == RecordStatusEnum.Active && c.Employee.Status == RecordStatusEnum.Active)
        ////            .AsQueryable();
        ////    if (lstLeaveEntitlements.Any())
        ////    {

        ////        foreach (var leaveEntitlement in lstLeaveEntitlements)
        ////        {
        ////            var employment = leaveEntitlement.Employee.Employments
        ////                .FirstOrDefault(c => c.EmployeeId == leaveEntitlement.EmployeeId);
        ////            if (employment != null)
        ////            {
        ////                DateTime? startDate = employment.StartDate;
        ////                DateTime currentDate = DateTime.UtcNow;
        ////                TimeSpan? difference = (currentDate - startDate);
        ////                if (difference.Value.TotalDays > leaveEntitlement.LeaveType.AccuralDays)
        ////                {
        ////                    if (leaveEntitlement.LeaveType.NoOfLeaves != null)
        ////                    {
        ////                        double leave = (((double)leaveEntitlement.LeaveType.NoOfLeaves) / 12.0);
        ////                        double prorated = ((leaveEntitlement.Prorated ?? 0) + leave);
        ////                        leaveEntitlement.Prorated = prorated;
        ////                        leaveEntitlement.ObjectState = ObjectState.Modified;
        ////                        context.SaveChangesAsync();
        ////                    }
        ////                }
        ////            }
        ////        }
        ////    }
        ////}

        //#endregion Hangfire


    }
}
