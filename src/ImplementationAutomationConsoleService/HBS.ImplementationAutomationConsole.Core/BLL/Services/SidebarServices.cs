using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using HBS.ImplementationAutomationConsole.Core.Models;

namespace HBS.ImplementationAutomationConsole.Core.BLL.Services
{
    public class SidebarServices : ISidebarServices
    {

        private readonly ISidebarRepository _isidebarRepository;

        public SidebarServices(ISidebarRepository isidebarRepository)
        {
            _isidebarRepository = isidebarRepository;
        }


        public async Task<IList<SidebarDto>> GetSubsectionItems()
        {
            var list = await _isidebarRepository.GetSubsectionItems();
            var eimList = list.Where(x => x.Module_Id == 2).ToList();
            var absenceList = list.Where(x => x.Module_Id == 14).ToList();
            var attendanceList = list.Where(x => x.Module_Id == 65).ToList();

            foreach (var item in eimList)
            {
                if (item.Approval_Status == "Not Approved")
                {
                    item.Enabled = true;
                    break;
                }

                if (item.Approval_Status == "Approved" && item.Vendor_ApprovalStatus == "Approved")
                {
                    item.Enabled = true;
                }

                if (item.Approval_Status == "Approved" && item.Vendor_ApprovalStatus == "Rejected")
                {
                    item.Enabled = true;
                }

                if (item.Approval_Status == "Rejected")
                {
                    item.Enabled = true;
                }
            }

            foreach (var item in absenceList)
            {
                if (item.Approval_Status == "Not Approved")
                {
                    item.Enabled = true;
                    break;
                }

                if (item.Approval_Status == "Approved" && item.Vendor_ApprovalStatus == "Approved")
                {
                    item.Enabled = true;
                }

                if (item.Approval_Status == "Approved" && item.Vendor_ApprovalStatus == "Rejected")
                {
                    item.Enabled = true;
                }

                if (item.Approval_Status == "Rejected")
                {
                    item.Enabled = true;
                }
            }

            foreach (var item in attendanceList)
            {
                if (item.Approval_Status == "Not Approved")
                {
                    item.Enabled = true;
                    break;
                }

                if (item.Approval_Status == "Approved" && item.Vendor_ApprovalStatus == "Approved")
                {
                    item.Enabled = true;
                }

                if (item.Approval_Status == "Approved" && item.Vendor_ApprovalStatus == "Rejected")
                {
                    item.Enabled = true;
                }

                if (item.Approval_Status == "Rejected")
                {
                    item.Enabled = true;
                }
            }

            var finalList = eimList.Concat(absenceList).Concat(attendanceList).ToList();
            return finalList;
        }

        public async Task<string> GetCopyRightText()
        {
            var copyrightText = await _isidebarRepository.GetCopyRightText();
            return copyrightText;
        }

        public async Task<List<ModuleDto>> GetActiveModules()
        {
            var list = await _isidebarRepository.GetSubsectionItems();
            var eimList = list.Where(x => x.Module_Id == 2).ToList();
            var absenceList = list.Where(x => x.Module_Id == 14).ToList();
            var attendanceList = list.Where(x => x.Module_Id == 65).ToList();

            bool eimComplete = true;
            bool absenceComplete = true;
            bool attendanceComplete = true;
            foreach (var item in eimList)
            {
                if (item.Vendor_ApprovalStatus != "Approved")
                {
                    eimComplete = false;
                    break;
                }
            }

            foreach (var item in absenceList)
            {
                if (item.Vendor_ApprovalStatus != "Approved")
                {
                    absenceComplete = false;
                    break;
                }
            }

            foreach (var item in attendanceList)
            {
                if (item.Vendor_ApprovalStatus != "Approved")
                {
                    attendanceComplete = false;
                    break;
                }
            }

            var modelList = await _isidebarRepository.GetActiveModules();

            var secondActiveModuleId = (modelList.Where(x => x.order == 2).FirstOrDefault() == null) ? 0 : modelList.Where(x => x.order == 2).FirstOrDefault().moduleId;
            var thirdActiveModule = (modelList.Where(x => x.order == 3).FirstOrDefault() == null) ? 0 : modelList.Where(x => x.order == 3).FirstOrDefault().moduleId;

            if (secondActiveModuleId == 14 && thirdActiveModule == 65)
            {
                modelList.Where(x => x.moduleId == 14).FirstOrDefault().is_enable = eimComplete;
                modelList.Where(x => x.moduleId == 65).FirstOrDefault().is_enable = absenceComplete;
            }
            else if (secondActiveModuleId == 65 && thirdActiveModule == 14)
            {
                modelList.Where(x => x.moduleId == 14).FirstOrDefault().is_enable = attendanceComplete;
                modelList.Where(x => x.moduleId == 65).FirstOrDefault().is_enable = eimComplete;
            }

            modelList.Where(x => x.order == 1).FirstOrDefault().is_enable = true;

            return modelList;
        }

        public async Task<List<ModuleDto>> GetActiveModulesFromDashboard()
        {
            var list = await _isidebarRepository.GetDashboardApprovalItems();

            var eimList = list.Where(x => x.ModuleID == 2).ToList();
            var absenceList = list.Where(x => x.ModuleID == 14).ToList();
            var attendanceList = list.Where(x => x.ModuleID == 65).ToList();

            var employeeDetailsList = eimList.Where(x => x.SubsectionID == "1").ToList();
            var bankDetailsList = eimList.Where(x => x.SubsectionID == "2").ToList();
            var reportingDetailsList = eimList.Where(x => x.SubsectionID == "3").ToList();
            var dependentDetailsList = eimList.Where(x => x.SubsectionID == "4").ToList();
            var emergencyDetailsList = eimList.Where(x => x.SubsectionID == "5").ToList();

            var statutoryLeaveList = absenceList.Where(x => x.SubsectionID == "6").ToList();
            var shortLeaveList = absenceList.Where(x => x.SubsectionID == "7").ToList();

            var shiftInfoList = attendanceList.Where(x => x.SubsectionID == "8").ToList();
            var rosterInfoList = attendanceList.Where(x => x.SubsectionID == "9").ToList();


            var employeeDataApproved = (employeeDetailsList.Count == 0) ? false : employeeDetailsList.Any(x => x.ApprovalStatus == "Approved");
            var bankDataApproved = (bankDetailsList.Count == 0) ? false : bankDetailsList.Any(x => x.ApprovalStatus == "Approved");
            var reportingDataApproved = (reportingDetailsList.Count == 0) ? false : reportingDetailsList.Any(x => x.ApprovalStatus == "Approved");
            var dependentDataApproved = (dependentDetailsList.Count == 0) ? false : dependentDetailsList.Any(x => x.ApprovalStatus == "Approved");
            var emergencyDataApproved = (emergencyDetailsList.Count == 0) ? false : emergencyDetailsList.Any(x => x.ApprovalStatus == "Approved");

            var statutoryLeaveApproved = (statutoryLeaveList.Count == 0) ? false : statutoryLeaveList.Any(x => x.ApprovalStatus == "Approved");
            var shortLeaveApproved = (shortLeaveList.Count == 0) ? false : shortLeaveList.Any(x => x.ApprovalStatus == "Approved");

            var shiftInfoApproved = (shiftInfoList.Count == 0) ? false : shiftInfoList.Any(x => x.ApprovalStatus == "Approved");
            var rosterInfoApproved = (rosterInfoList.Count == 0) ? false : rosterInfoList.Any(x => x.ApprovalStatus == "Approved");

            var eimActiveSubsecList = await _isidebarRepository.GetActiveSubsectionsEim();
            var absenceActiveSubsecList = await _isidebarRepository.GetActiveSubsectionsLeave();
            var attendanceActiveSubsecList = await _isidebarRepository.GetActiveSubsectionsAttendance();

            var eimSubsectionFlagList = new List<bool>();
            var absenceSubsectionFlagList = new List<bool>();
            var attendanceSubsectionFlagList = new List<bool>();

            foreach (var subsec in eimActiveSubsecList)
            {
                if (subsec.SubsectionId == "1")
                {
                    eimSubsectionFlagList.Add(employeeDataApproved);
                }
                else if (subsec.SubsectionId == "2")
                {
                    eimSubsectionFlagList.Add(bankDataApproved);
                }
                else if (subsec.SubsectionId == "3")
                {
                    eimSubsectionFlagList.Add(reportingDataApproved);
                }
                else if (subsec.SubsectionId == "4")
                {
                    eimSubsectionFlagList.Add(dependentDataApproved);
                }
                else if (subsec.SubsectionId == "5")
                {
                    eimSubsectionFlagList.Add(emergencyDataApproved);
                }
            }

            foreach (var subsec in absenceActiveSubsecList)
            {
                if (subsec.SubsectionId == "6")
                {
                    absenceSubsectionFlagList.Add(statutoryLeaveApproved);
                }
                else if (subsec.SubsectionId == "7")
                {
                    absenceSubsectionFlagList.Add(shortLeaveApproved);
                }
            }

            foreach (var subsec in attendanceActiveSubsecList)
            {
                if (subsec.SubsectionId == "8")
                {
                    attendanceSubsectionFlagList.Add(shiftInfoApproved);
                }
                else if (subsec.SubsectionId == "9")
                {
                    attendanceSubsectionFlagList.Add(rosterInfoApproved);
                }
            }

            bool eimComplete = (eimSubsectionFlagList.Count == 0) ? false : eimSubsectionFlagList.All(x => x == true);
            bool absenceComplete = (absenceSubsectionFlagList.Count == 0) ? false : absenceSubsectionFlagList.All(x => x == true);
            bool attendanceComplete = (attendanceSubsectionFlagList.Count == 0) ? false : attendanceSubsectionFlagList.All(x => x == true);

            var modelList = await _isidebarRepository.GetActiveModules();

            var secondActiveModuleId = (modelList.Where(x => x.order == 2).FirstOrDefault() == null) ? 0 : modelList.Where(x => x.order == 2).FirstOrDefault().moduleId;
            var thirdActiveModule = (modelList.Where(x => x.order == 3).FirstOrDefault() == null) ? 0 : modelList.Where(x => x.order == 3).FirstOrDefault().moduleId;

            if (!eimComplete)
            {
                modelList.Where(x => x.moduleId == 14).FirstOrDefault().is_enable = false;
                modelList.Where(x => x.moduleId == 65).FirstOrDefault().is_enable = false;
                modelList.Where(x => x.order == 1).FirstOrDefault().is_enable = true;
                return modelList;
            }
            else
            {

                if (secondActiveModuleId == 14 && thirdActiveModule == 65)
                {
                    modelList.Where(x => x.moduleId == 14).FirstOrDefault().is_enable = eimComplete;
                    modelList.Where(x => x.moduleId == 65).FirstOrDefault().is_enable = absenceComplete;
                }
                else if (secondActiveModuleId == 65 && thirdActiveModule == 14)
                {
                    modelList.Where(x => x.moduleId == 14).FirstOrDefault().is_enable = attendanceComplete;
                    modelList.Where(x => x.moduleId == 65).FirstOrDefault().is_enable = eimComplete;
                }

                modelList.Where(x => x.order == 1).FirstOrDefault().is_enable = true;
                return modelList;
            }
        }

        public async Task<List<ActiveSubsectionDto>> GetActiveSubsectionsEim()
        {
            return await _isidebarRepository.GetActiveSubsectionsEim();
        }

        public async Task<List<ActiveSubsectionDto>> GetActiveSubsectionsLeave()
        {
            return await _isidebarRepository.GetActiveSubsectionsLeave();
        }

        public async Task<List<ActiveSubsectionDto>> GetActiveSubsectionsAttendance()
        {
            return await _isidebarRepository.GetActiveSubsectionsAttendance();
        }

        public async Task<List<int>> GetActiveModuleIDs()
        {
            return await _isidebarRepository.GetActiveModuleIDs();
        }

        public async Task<bool> CheckIfSubsectionApproved(string subsectionName)
        {
            return await _isidebarRepository.CheckIfSubsectionApproved(subsectionName);
        }
    }
}